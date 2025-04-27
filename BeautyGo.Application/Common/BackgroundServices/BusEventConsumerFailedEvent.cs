using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Common.BackgroundServices;

public record BusEventConsumerFailedEvent(object @Event, Exception Error) : IEvent;
