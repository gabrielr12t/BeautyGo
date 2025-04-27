using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Common.BackgroundServices;

public record EventProcessorFailedEvent(Guid EventId, Exception Error) : IEvent;
