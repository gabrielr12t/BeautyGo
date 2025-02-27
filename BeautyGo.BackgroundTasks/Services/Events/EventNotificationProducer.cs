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

public class EventNotificationProducer : IEventNotificationProducer
{
    #region Fields

    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Event> _eventRepository;

    #endregion

    #region Ctor

    public EventNotificationProducer(
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

                pendingEvent.MarkAsExecuted();

                tasks.Add(_mediator.Publish(@event, cancellationToken));
            }
            catch (Exception ex)
            {
                pendingEvent.Attempts++;

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
