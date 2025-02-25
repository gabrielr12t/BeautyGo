using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.DomainEvents.Events;

namespace BeautyGo.Domain.Entities.Events;

public class EventError : BaseEntity, IAuditableEntity
{
    public string Message { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; }

    public static EventError Create(string message, Guid eventId)
    {
        var @event = new EventError
        {
            Message = message,
            EventId = eventId
        };

        @event.AddDomainEvent(new EventErrorCreatedDomainEvent(@event));

        return @event;
    }
}
