using BeautyGo.Domain.Entities.Users;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Users;

public class UserByPhoneNumberSpecification : Specification<User>
{
    private readonly string _phoneNumber;

    public UserByPhoneNumberSpecification(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public override Expression<Func<User, bool>> ToExpression() =>
        user => user.PhoneNumber == _phoneNumber;
}
