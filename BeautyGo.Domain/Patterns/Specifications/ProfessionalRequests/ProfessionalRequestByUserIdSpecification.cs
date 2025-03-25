using BeautyGo.Domain.Entities.Professionals;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.ProfessionalRequests;

public sealed class ProfessionalRequestByUserIdSpecification : Specification<ProfessionalRequest>
{
    private readonly Guid _userId;

    public ProfessionalRequestByUserIdSpecification(Guid userId)
    {
        _userId = userId;
    }

    public override Expression<Func<ProfessionalRequest, bool>> ToExpression() =>
        pr => pr.UserId == _userId;
}
