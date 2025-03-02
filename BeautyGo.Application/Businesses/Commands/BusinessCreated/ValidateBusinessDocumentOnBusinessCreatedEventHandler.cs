using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Businesses.Commands.BusinessCreated;

public class ValidateBusinessDocumentOnBusinessCreatedEventHandler : IEventHandler<BusinessCreatedEvent>
{
    #region Fields

    private readonly IBaseRepository<Business> _businessRepository;
    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;

    #endregion

    #region Ctor

    public ValidateBusinessDocumentOnBusinessCreatedEventHandler(IReceitaFederalIntegrationService receitaFederalIntegration, IBaseRepository<Business> businessRepository)
    {
        _receitaFederalIntegration = receitaFederalIntegration;
        _businessRepository = businessRepository;
    }

    #endregion

    public async Task Handle(BusinessCreatedEvent notification, CancellationToken cancellationToken)
    {
        //var businessByIdSpec = new EntityByIdSpecification<Business>(notification.BeautyBusinessId);
        //var business = await _businessRepository.GetFirstOrDefaultAsync(businessByIdSpec);

        //if (business is null)
        //    throw new DomainException(DomainErrors.Business.BusinessNotFound(notification.BeautyBusinessId));

        //var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(business.Cnpj, cancellationToken);
        //if (!cnpjReceitaFederalResponse.HasValue)
        //    throw new DomainException(DomainErrors.Business.InvalidCnpj(business.Cnpj));

        //if (!_receitaFederalIntegration.IsValidCnpjStatus(cnpjReceitaFederalResponse.Value.Status, cnpjReceitaFederalResponse.Value.Situacao))
        //    throw new DomainException(DomainErrors.Business.CnpjRestricted(business.Cnpj));

        //var VerifyCompanyNameSimilarity = CommonHelper.CheckProximityWithThreshold(cnpjReceitaFederalResponse.Value.Nome.ToUpper(), business.Name.ToUpper(), 0.8);
        //if (!VerifyCompanyNameSimilarity)
        //    throw new DomainException(DomainErrors.Business.CnpjNameInvalid(business.Cnpj));
    }
}
