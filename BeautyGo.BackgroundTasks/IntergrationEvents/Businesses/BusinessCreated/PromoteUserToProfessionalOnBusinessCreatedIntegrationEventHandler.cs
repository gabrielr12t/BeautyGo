using BeautyGo.Application.Businesses.Commands.BusinessCreated;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Customers;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.BusinessCreated;

internal class PromoteUserToProfessionalOnBusinessCreatedIntegrationEventHandler : IIntegrationEventHandler<BusinessCreatedIntegrationEvent>
{
    #region Fields

    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Business> _businessRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public PromoteUserToProfessionalOnBusinessCreatedIntegrationEventHandler(IBaseRepository<User> userRepository, IBaseRepository<Business> businessRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Handle

    public async Task Handle(BusinessCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        //var business = await _businessRepository.GetFirstOrDefaultAsync(new EntityByIdSpecification<Business>(notification.BusinessId), true, cancellationToken);

        //if (business is null)
        //    throw new DomainException(DomainErrors.Business.BusinessNotFound(notification.BusinessId));

        //var owner = await _userRepository.GetFirstOrDefaultAsync(new EntityByIdSpecification<User>(business.OwnerId), cancellationToken: cancellationToken);

        //if (owner is Customer customer)
        //{
        //    var professional = customer.PromoteToProfessional();

        //    business.Owner = professional;
        //    business.OwnerId = professional.Id;

        //    _userRepository.Remove(owner);
        //    await _userRepository.InsertAsync(professional, cancellationToken);

        //    await _unitOfWork.SaveChangesAsync(cancellationToken);
        //}
    }

    #endregion
}
