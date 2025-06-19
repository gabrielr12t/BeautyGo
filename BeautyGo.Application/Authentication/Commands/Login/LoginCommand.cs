using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Authentication.Commands.Login;

public record class LoginCommand(
    string Email,
    string Password) : ICommand<Result<AuthResponse>>;
