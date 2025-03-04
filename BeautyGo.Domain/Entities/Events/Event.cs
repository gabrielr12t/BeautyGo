using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Settings;
using Newtonsoft.Json;

namespace BeautyGo.Domain.Entities.Events;

public sealed class Event : BaseEntity, IAuditableEntity, ISoftDeletableEntity
{
    public Event()
    {
        EventErrors = new List<EventError>();
    }

    public Guid UserId { get; private set; }

    public int Attempts { get; set; }

    public DateTime Schedule { get; set; }

    public DateTime? Executed { get; set; }

    public DateTime? Modified { get; }

    public DateTime? Deleted { get; set; }

    public EventStatus Status { get; set; }

    public string EventSource { get; set; }

    public static Event Create(Guid userId, IEvent @event, DateTime schuleAt)
    {
        var eventPayload = JsonConvert.SerializeObject(@event, typeof(IEvent), new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
        });

        return new Event
        {
            EventSource = eventPayload,
            UserId = userId,
            CreatedOn = DateTime.Now,
            Schedule = schuleAt,
            Status = EventStatus.Pendind,
            Executed = null,
            Attempts = 0,
        };
    }

    public ICollection<EventError> EventErrors { get; set; }

    public Result MarkAsExecuted()
    {
        Executed = DateTime.Now;
        Status = EventStatus.Success;

        EventErrors.Clear();

        return Result.Success();
    }

    public Result NextAttempt()
    {
        var eventSettings = Singleton<EventSettings>.Instance;

        if ((Attempts + 1) > eventSettings.MaxAttempsFailed)
        {
            return Result.Failure(DomainErrors.Event.MaxAttemps);
        }

        return Result.Success();
    } 
}
