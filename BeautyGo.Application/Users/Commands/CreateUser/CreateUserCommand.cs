using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Users;
using BeautyGo.Domain.Core.Primitives.Results;

namespace BeautyGo.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FirstName, 
    string LastName, 
    string Email, 
    string Password, 
    string CPF,
    string Phone,
    UserTypeEnum UserType) : ICommand<Result<CreateUserResponse>>;
