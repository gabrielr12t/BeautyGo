using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

public record BusinessCreatedEvent(Guid BeautyBusinessId) : IEvent;

