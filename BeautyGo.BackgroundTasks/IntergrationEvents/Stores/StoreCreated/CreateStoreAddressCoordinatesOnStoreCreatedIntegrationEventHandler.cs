using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Stores.Events.StoreCreated;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Stores.StoreCreated;

internal class CreateStoreAddressCoordinatesOnStoreCreatedIntegrationEventHandler : IIntegrationEventHandler<StoreCreatedIntegrationEvent>
{
    #region Fields

    private readonly IOpenStreetMapIntegrationService _openStreetMapIntegrationService;
    private readonly IUnitOfWork _unitOfWork;

    private readonly IBaseRepository<Address> _addressRepository;
    private readonly IBaseRepository<Store> _storeRepository;

    #endregion

    #region Ctor

    public CreateStoreAddressCoordinatesOnStoreCreatedIntegrationEventHandler(
        IOpenStreetMapIntegrationService openStreetMapIntegrationService,
        IBaseRepository<Address> addressRepository,
        IBaseRepository<Store> storeRepository,
        IUnitOfWork unitOfWork)
    {
        _openStreetMapIntegrationService = openStreetMapIntegrationService;
        _addressRepository = addressRepository;
        _storeRepository = storeRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task Handle(StoreCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Store>(notification.StoreId).AddInclude(
                p => p.Address), cancellationToken: cancellationToken);

        if (store is null)
            throw new DomainException(DomainErrors.General.NotFound);

        var addressCoordinates = await _openStreetMapIntegrationService.GetAddressCoordinateAsync(store.Address.Street,
           store.Address.Number, store.Address.City, cancellationToken);

        store.Address.ChangeCoordinates(addressCoordinates.Value.Latitude, addressCoordinates.Value.Longitude);

        _storeRepository.Update(store);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
