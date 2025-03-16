using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Businesses.Commands.SendProfessionalRequest;

public record SendProfessionalRequestCommand(
    Guid BusinessId,
    Guid UserId)
    : ICommand<Result>;
