using BeautyGo.Application.Business.Events.BusinessCreated;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Business;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Business.BeautyBusinessCreated;

internal class CreateBeautyBusinessAddressCoordinatesOnBeautyBusinessCreatedIntegrationEventHandler : IIntegrationEventHandler<BeautyBusinessCreatedIntegrationEvent>
{
    #region Fields

    private readonly IOpenStreetMapIntegrationService _openStreetMapIntegrationService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IBaseRepository<Address> _addressRepository;
    private readonly IBaseRepository<BeautyBusiness> _businessRepository;

    #endregion

    #region Ctor

    public CreateBeautyBusinessAddressCoordinatesOnBeautyBusinessCreatedIntegrationEventHandler(
        IOpenStreetMapIntegrationService openStreetMapIntegrationService,
        IBaseRepository<Address> addressRepository,
        IBaseRepository<BeautyBusiness> businessRepository,
        IUnitOfWork unitOfWork)
    {
        _openStreetMapIntegrationService = openStreetMapIntegrationService;
        _addressRepository = addressRepository;
        _businessRepository = businessRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task Handle(BeautyBusinessCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<BeautyBusiness>(notification.BusinessId).AddInclude(
                p => p.Address), cancellationToken: cancellationToken);

        if (business is null)
            throw new DomainException(DomainErrors.General.NotFound);

        var addressCoordinates = await _openStreetMapIntegrationService.GetAddressCoordinateAsync(business.Address.Street,
           business.Address.Number, business.Address.City, cancellationToken);

        business.Address.ChangeCoordinates(addressCoordinates.Value.Latitude, addressCoordinates.Value.Longitude);

        _businessRepository.Update(business);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
