using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Users;

public class UserByCpfSpecification : Specification<User>
{
    private readonly string _cpf;

    public UserByCpfSpecification(string cpf) =>
        _cpf = cpf;

    public override Expression<Func<User, bool>> ToExpression() =>
        user => user.Cpf == _cpf;
}
