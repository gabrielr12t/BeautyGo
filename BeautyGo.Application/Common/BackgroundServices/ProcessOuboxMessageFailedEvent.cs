using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Common.BackgroundServices;

public record ProcessOuboxMessageFailedEvent(Guid MessageId, Exception Error) : IEvent;
