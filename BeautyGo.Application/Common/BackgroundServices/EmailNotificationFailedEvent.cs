using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Common.BackgroundServices;

public record EmailNotificationFailedEvent(Guid EmailId, Exception Error) : IEvent;
