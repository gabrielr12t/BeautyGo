using BeautyGo.Application.Users.Commands.CreateUser;
using BeautyGo.Contracts.Users;
using BeautyGo.Domain.Entities.Customers;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Factories.Users;

internal class UserFactory : IUserFactory
{
    public User Create(CreateUserCommand command)
    {
        return command.UserType switch
        {
            UserTypeEnum.Customer => Customer.Create(command.FirstName, command.LastName, command.Email, command.Phone, command.CPF),
            UserTypeEnum.Professional => Professional.Create(command.FirstName, command.LastName, command.Email, command.Phone, command.CPF),
            _ => throw new ArgumentOutOfRangeException(nameof(command), $"Not expected user type value: {command.UserType}")
        };
    }
}
