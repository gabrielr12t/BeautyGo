using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Media;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Stores;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications.Stores;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Stores.Commands.CreateStore;

internal class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, Result>
{
    private readonly IBaseRepository<Store> _storeRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Address> _addressRepository;

    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly IViaCepIntegrationService _viaCepIntegration;
    private readonly IOpenStreetMapIntegrationService _openStreetMapIntegrationService;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStoreCommandHandler(
        IBaseRepository<Store> storeRepository,
        IBaseRepository<User> userRepository,
        IBaseRepository<Address> addressRepository,
        IPictureService pictureService,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IReceitaFederalIntegrationService receitaFederalIntegration,
        IViaCepIntegrationService viaCepIntegration,
        IOpenStreetMapIntegrationService openStreetMapIntegrationService)
    {
        _receitaFederalIntegration = receitaFederalIntegration;
        _storeRepository = storeRepository;
        _addressRepository = addressRepository;
        _authService = authService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _viaCepIntegration = viaCepIntegration;
        _openStreetMapIntegrationService = openStreetMapIntegrationService;
    }

    #region Utilities

    private async Task<Result> BussinessValidationAsync(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        if (!CommonHelper.IsValidCnpj(request.Cnpj))
            return Result.Failure(DomainErrors.Store.InvalidCnpj);

        var storeByCnpjSpec = new StoreByCnpjSpecification(request.Cnpj);
        if (await _storeRepository.ExistAsync(storeByCnpjSpec, cancellationToken))
            return Result.Failure(DomainErrors.Store.CnpjAlreadyExists);

        var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(request.Cnpj, cancellationToken);
        if (!cnpjReceitaFederalResponse.HasValue)
            return Result.Failure(DomainErrors.Store.InvalidCnpj);

        if (!_receitaFederalIntegration.IsValidCnpjStatus(cnpjReceitaFederalResponse.Value.Status, cnpjReceitaFederalResponse.Value.Situacao))
            return Result.Failure(DomainErrors.Store.CnpjRestricted);

        var VerifyCompanyNameSimilarity = CommonHelper.CheckProximityWithThreshold(
            cnpjReceitaFederalResponse.Value.Nome.ToUpper(),
            request.Name.ToUpper(), 0.8);
        if (!VerifyCompanyNameSimilarity)
            return Result.Failure(DomainErrors.Store.CnpjNameInvalid);

        return Result.Success();
    }

    #endregion

    public async Task<Result> Handle(CreateStoreCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();

        var bussinessValidate = await BussinessValidationAsync(request, cancellationToken).ConfigureAwait(false);

        if (!bussinessValidate.IsSuccess)
            return Result.Failure(bussinessValidate.Error);

        var addressResponse = await _viaCepIntegration.GetAddressByCepAsync(request.AddressCep, cancellationToken);
        if (addressResponse.HasNoValue)
            return Result.Failure(DomainErrors.Address.CepNotFound);

        var newAddress = Address.Create(request.AddressFirstName, request.AddressLastName, addressResponse.Value.City,
            addressResponse.Value.State, addressResponse.Value.StateAbbreviation, addressResponse.Value.Neighborhood, request.AddressNumber,
            addressResponse.Value.Street, request.AddressCep, request.AddressPhoneNumber);

        await _addressRepository.InsertAsync(newAddress, cancellationToken);

        var newStore = Store.Create(request.Name,
            request.HomePageTitle,
            request.HomePageDescription,
            request.Cnpj,
            currentUser.Id, newAddress.Id);

        await _storeRepository.InsertAsync(newStore, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
