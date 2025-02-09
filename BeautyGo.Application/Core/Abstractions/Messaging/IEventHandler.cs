using MediatR;

namespace BeautyGo.Application.Core.Abstractions.Messaging;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : INotification
{
}
