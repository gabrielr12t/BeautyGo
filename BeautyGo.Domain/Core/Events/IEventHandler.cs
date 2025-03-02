using MediatR;

namespace BeautyGo.Domain.Core.Events;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{
}
