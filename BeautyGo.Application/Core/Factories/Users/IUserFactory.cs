using BeautyGo.Application.Users.Commands.CreateUser;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Factories.Users;

public interface IUserFactory
{
    User Create(CreateUserCommand command);
}
