using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string FirstName,
    string LastName) : ICommand<Result>;

