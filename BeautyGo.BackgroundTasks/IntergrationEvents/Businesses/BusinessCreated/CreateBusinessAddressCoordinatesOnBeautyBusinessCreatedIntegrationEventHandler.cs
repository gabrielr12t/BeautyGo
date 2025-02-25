using BeautyGo.Application.Businesses.Events.BusinessCreated;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.BusinessCreated;

internal class CreateBusinessAddressCoordinatesOnBeautyBusinessCreatedIntegrationEventHandler : IIntegrationEventHandler<BusinessCreatedIntegrationEvent>
{
    #region Fields

    private readonly IOpenStreetMapIntegrationService _openStreetMapIntegrationService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IBaseRepository<Address> _addressRepository;
    private readonly IBaseRepository<Domain.Entities.Businesses.Business> _businessRepository;

    #endregion

    #region Ctor

    public CreateBusinessAddressCoordinatesOnBeautyBusinessCreatedIntegrationEventHandler(
        IOpenStreetMapIntegrationService openStreetMapIntegrationService,
        IBaseRepository<Address> addressRepository,
        IBaseRepository<Domain.Entities.Businesses.Business> businessRepository,
        IUnitOfWork unitOfWork)
    {
        _openStreetMapIntegrationService = openStreetMapIntegrationService;
        _addressRepository = addressRepository;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task Handle(BusinessCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Domain.Entities.Businesses.Business>(notification.BusinessId).AddInclude(
                p => p.Address), cancellationToken: cancellationToken);

        if (business is null)
            throw new DomainException(DomainErrors.General.NotFound);

        var addressCoordinates = await _openStreetMapIntegrationService.GetAddressCoordinateAsync(business.Address.Street,
           business.Address.Number, business.Address.City, cancellationToken);

        if (addressCoordinates is null)
            throw new DomainException(DomainErrors.Address.CoordinatesNotFoundToBusiness(business.Id));

        business.Address.ChangeCoordinates(addressCoordinates.Value.Latitude, addressCoordinates.Value.Longitude);

        _businessRepository.Update(business);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
