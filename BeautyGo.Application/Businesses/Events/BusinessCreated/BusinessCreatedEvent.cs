using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Businesses.Events.BusinessCreated;

public record BusinessCreatedEvent(Guid BeautyBusinessId) : IEvent;
