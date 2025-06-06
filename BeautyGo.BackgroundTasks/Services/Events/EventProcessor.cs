﻿using BeautyGo.Application.Common.BackgroundServices;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Events;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Patterns.Specifications.Events;
using BeautyGo.Domain.Repositories.Bases;
using BeautyGo.Domain.Settings;
using MediatR;
using Newtonsoft.Json;

namespace BeautyGo.BackgroundTasks.Services.Events;

public class EventProcessor : IEventProcessor
{
    #region Fields

    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEFBaseRepository<Event> _eventRepository;

    #endregion

    #region Ctor

    public EventProcessor(
        IMediator mediator,
        ILogger logger,
        IUnitOfWork unitOfWork,
        IEFBaseRepository<Event> eventRepository)
    {
        _mediator = mediator;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
    }

    #endregion

    #region Utilities

    private DateTime GetRetryExecutionTime(int attemptCount)
    {
        var retryDelays = new[]
        {
            TimeSpan.FromSeconds(20),  // 1ª tentativa → 20s
            TimeSpan.FromMinutes(2),   // 2ª tentativa → 2min
            TimeSpan.FromMinutes(10),  // 3ª tentativa → 10min
            TimeSpan.FromHours(1),     // 4ª tentativa → 1h
            TimeSpan.FromDays(1),      // 5ª tentativa → 1 dia
            TimeSpan.FromDays(3)       // 6ª tentativa → 3 dias
        };

        if (attemptCount <= retryDelays.Length)
            return DateTime.Now.Add(retryDelays[attemptCount - 1]); // Pegamos o tempo baseado na tentativa
        else
            return DateTime.Now.AddDays(7); // Se passar do limite, espera 7 dias (ou outro tempo que preferir)
    }

    #endregion

    public async Task ProduceAsync(int batchSize, CancellationToken cancellationToken)
    {
        var appSettings = Singleton<AppSettings>.Instance;
        var eventSettings = appSettings.Get<EventSettings>();

        var pendingEventsSpec = new PendingEventSpecification(eventSettings.MaxAttempsFailed);
        var pendingEvents = await _eventRepository.GetAsync(pendingEventsSpec, true, cancellationToken);

        foreach (var pendingEvent in pendingEvents)
        {
            try
            {
                IEvent @event = JsonConvert.DeserializeObject<IEvent>(pendingEvent.EventSource, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                await _mediator.Publish(@event, cancellationToken);

                pendingEvent.MarkAsExecuted();
            }
            catch (Exception ex)
            {
                pendingEvent.Attempts++;

                pendingEvent.Status = EventStatus.Error;
                pendingEvent.Schedule = GetRetryExecutionTime(pendingEvent.Attempts);

                pendingEvent.EventErrors.Add(EventError.Create(ex.Message, pendingEvent.Id));

                await _logger.ErrorAsync($"Erro ao executar o evento: {pendingEvent.Id}", ex, cancellation: cancellationToken);

                if (pendingEvent.Attempts == eventSettings.MaxAttempsFailed)
                {
                    await _mediator.Publish(new EventProcessorFailedEvent(pendingEvent.Id, ex), cancellationToken);
                }
            }
            finally
            {
                await _eventRepository.UpdateAsync(pendingEvent);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
