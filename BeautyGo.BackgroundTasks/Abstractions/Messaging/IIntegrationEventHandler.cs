using BeautyGo.Application.Core.Abstractions.Messaging;
using MediatR;

namespace BeautyGo.BackgroundTasks.Abstractions.Messaging;

public interface IIntegrationEventHandler<in TIntegrationEvent> : INotificationHandler<TIntegrationEvent>
   where TIntegrationEvent : IIntegrationEvent
{
}
