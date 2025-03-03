using BeautyGo.Domain.Core.Events;

namespace BeautyGo.Application.Businesses.Commands.AccountConfirmed;

public record BusinessAccountConfirmedEvent(Guid BusinessId) : IEvent;


