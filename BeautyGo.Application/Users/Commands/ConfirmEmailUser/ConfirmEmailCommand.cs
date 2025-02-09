using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Users.Commands.ConfirmEmailUser;

public record ConfirmEmailCommand(
    string Token) : ICommand<Result>;

