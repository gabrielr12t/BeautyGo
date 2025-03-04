using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Events;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Patterns.Specifications.Events;
using BeautyGo.Domain.Repositories;
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
    private readonly IBaseRepository<Event> _eventRepository;

    #endregion

    #region Ctor

    public EventProcessor(
        IMediator mediator,
        ILogger logger,
        IUnitOfWork unitOfWork,
        IBaseRepository<Event> eventRepository)
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
            return DateTime.UtcNow.Add(retryDelays[attemptCount - 1]); // Pegamos o tempo baseado na tentativa
        else
            return DateTime.UtcNow.AddDays(7); // Se passar do limite, espera 7 dias (ou outro tempo que preferir)
    }


    #endregion

    public async Task ProduceAsync(int batchSize, CancellationToken cancellationToken)
    {
        var appSettings = Singleton<AppSettings>.Instance;
        var eventSettings = appSettings.Get<EventSettings>();

        var pendingEventsSpec = new PendingEventSpecification(eventSettings.MaxAttempsFailed);
        var pendingEvents = await _eventRepository.GetAsync(pendingEventsSpec);

        var tasks = new List<Task>();

        foreach (var pendingEvent in pendingEvents)
        {
            try
            {
                var @event = JsonConvert.DeserializeObject<IEvent>(pendingEvent.EventSource, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                tasks.Add(_mediator.Publish(@event, cancellationToken));

                pendingEvent.MarkAsExecuted();
            }
            catch (Exception ex)
            {
                pendingEvent.Attempts++;

                pendingEvent.Status = EventStatus.Error;
                pendingEvent.Schedule = GetRetryExecutionTime(pendingEvent.Attempts);

                pendingEvent.EventErrors.Add(EventError.Create(ex.Message, pendingEvent.Id));

                await _logger.ErrorAsync($"Erro ao executar o evento: {pendingEvent.Id}", ex);
            }
            finally
            {
                _eventRepository.Update(pendingEvent);

                await _unitOfWork.SaveChangesAsync();
            }
        }

        await Task.WhenAll(tasks);
    }
}
