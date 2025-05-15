using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Application.Businesses.Commands.AccountConfirmed;

public class ValidateDocumentOnBusinessAccountConfirmedEventHandler : IEventHandler<BusinessAccountConfirmedEvent>
{
    #region Fields

    private readonly IEFBaseRepository<Business> _businessRepository;
    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public ValidateDocumentOnBusinessAccountConfirmedEventHandler(IReceitaFederalIntegrationService receitaFederalIntegration, IEFBaseRepository<Business> businessRepository, IUnitOfWork unitOfWork)
    {
        _receitaFederalIntegration = receitaFederalIntegration;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task Handle(BusinessAccountConfirmedEvent notification, CancellationToken cancellationToken)
    {
        var businessByIdSpec = new EntityByIdSpecification<Business>(notification.BusinessId);
        var business = await _businessRepository.GetFirstOrDefaultAsync(businessByIdSpec);

        if (business is null)
            throw new DomainException(DomainErrors.Business.BusinessNotFound(notification.BusinessId));

        var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(business.Cnpj, cancellationToken);
        if (!cnpjReceitaFederalResponse.HasValue)
            throw new DomainException(DomainErrors.Business.InvalidCnpj(business.Cnpj));

        if (!_receitaFederalIntegration.IsValidCnpjStatus(cnpjReceitaFederalResponse.Value.Status, cnpjReceitaFederalResponse.Value.Situacao))
            throw new DomainException(DomainErrors.Business.CnpjRestricted(business.Cnpj));

        var VerifyCompanyNameSimilarity = CommonHelper.CheckProximityWithThreshold(cnpjReceitaFederalResponse.Value.Nome.ToUpper(), business.Name.ToUpper(), 0.7);
        if (!VerifyCompanyNameSimilarity)
            throw new DomainException(DomainErrors.Business.CnpjNameInvalid(business.Cnpj));

        business.ValidatedDocument();

        await _businessRepository.UpdateAsync(business);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
