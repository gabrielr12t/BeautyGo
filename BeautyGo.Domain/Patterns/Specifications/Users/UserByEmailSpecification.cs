using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Users;

public class UserByEmailSpecification : Specification<User>
{
    private readonly string _email;

    public UserByEmailSpecification(string email) =>
        _email = email;

    public override Expression<Func<User, bool>> ToExpression() =>
        user => user.Email == _email;
}
