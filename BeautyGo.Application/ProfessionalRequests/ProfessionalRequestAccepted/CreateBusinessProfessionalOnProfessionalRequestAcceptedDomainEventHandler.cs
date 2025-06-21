using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.DomainEvents;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace BeautyGo.Application.ProfessionalRequests.ProfessionalRequestAccepted;

internal class CreateBusinessProfessionalOnProfessionalRequestAcceptedDomainEventHandler
    : IDomainEventHandler<ProfessionalRequestAcceptedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IEFBaseRepository<Business> _businessRepository;
    private readonly IEFBaseRepository<Professional> _professionalRepository;

    public CreateBusinessProfessionalOnProfessionalRequestAcceptedDomainEventHandler(
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IEFBaseRepository<Business> businessRepository,
        IEFBaseRepository<Professional> professionalRepository)

    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _businessRepository = businessRepository;
        _professionalRepository = professionalRepository;
    }

    #region Utilities

    private async Task<Business> GetBusinessByIdAsync(Guid businessId, CancellationToken cancellationToken)
    {
        var businessByIdSpec = new EntityByIdSpecification<Business>(businessId);
        businessByIdSpec.AddInclude(q => q.Include(i => i.Professionals));

        return await _businessRepository.GetFirstOrDefaultAsync(businessByIdSpec, true, cancellationToken);
    }

    #endregion

    public async Task Handle(ProfessionalRequestAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var business = await GetBusinessByIdAsync(notification.ProfessionalRequest.BusinessId, cancellationToken);

        var professional = await _professionalRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Professional>(notification.ProfessionalRequest.UserId), false, cancellationToken);

        business.AddProfessional(professional);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
