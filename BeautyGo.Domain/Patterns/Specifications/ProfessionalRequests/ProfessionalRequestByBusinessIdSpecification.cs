using BeautyGo.Domain.Entities.Professionals;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.ProfessionalRequests;

public sealed class ProfessionalRequestByBusinessIdSpecification : Specification<ProfessionalRequest>
{
    private readonly Guid _businessId;

    public ProfessionalRequestByBusinessIdSpecification(Guid businessId)
    {
        _businessId = businessId;
    }

    public override Expression<Func<ProfessionalRequest, bool>> ToExpression() =>
        pr => pr.BusinessId == _businessId;
}
