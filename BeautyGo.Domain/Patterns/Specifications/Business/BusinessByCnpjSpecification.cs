using BeautyGo.Domain.Helpers;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Business;

public class BusinessByCnpjSpecification : Specification<Entities.Businesses.Business>
{
    private readonly string _cnpj;
    public BusinessByCnpjSpecification(string cnpj) =>
        _cnpj = CommonHelper.EnsureNumericOnly(cnpj);

    public override Expression<Func<Entities.Businesses.Business, bool>> ToExpression() =>
        business => business.Cnpj == _cnpj;
}
