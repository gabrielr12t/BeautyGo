using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Domain.Helpers;
using System.Linq.Expressions;

namespace BeautyGo.Domain.Patterns.Specifications.Stores;

public class StoreByCnpjSpecification : Specification<Store>
{
    private readonly string _cnpj;
    public StoreByCnpjSpecification(string cnpj) =>
        _cnpj = CommonHelper.EnsureNumericOnly(cnpj);

    public override Expression<Func<Store, bool>> ToExpression() =>
        store => store.Cnpj == _cnpj;
}
