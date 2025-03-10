using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Customers;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications.Businesses;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Businesses.Commands.CreateBusiness;

internal class CreateBusinessCommandHandler : ICommandHandler<CreateBusinessCommand, Result>
{
    #region Fields

    private readonly IBaseRepository<Business> _businessRepository;
    private readonly IBaseRepository<Address> _addressRepository;
    private readonly IBaseRepository<User> _userRepository;

    private readonly IViaCepIntegrationService _viaCepIntegration;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public CreateBusinessCommandHandler(
        IBaseRepository<Business> businessRepository,
        IBaseRepository<Address> addressRepository,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IViaCepIntegrationService viaCepIntegration,
        IBaseRepository<User> userRepository)
    {
        _businessRepository = businessRepository;
        _addressRepository = addressRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _viaCepIntegration = viaCepIntegration;
        _userRepository = userRepository;
    }

    #endregion

    #region Utilities

    private async Task<Result> BussinessValidationAsync(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        if (!CommonHelper.IsValidCnpj(request.Cnpj))
            return Result.Failure(DomainErrors.Business.InvalidCnpj(request.Cnpj));

        var businessByCnpjSpec = new BusinessByCnpjSpecification(request.Cnpj);
        if (await _businessRepository.ExistAsync(businessByCnpjSpec, cancellationToken))
            return Result.Failure(DomainErrors.Business.CnpjAlreadyExists);

        return Result.Success();
    }

    private async Task<Result> ValidateBusinessAsync(CreateBusinessCommand request, CancellationToken cancellationToken) =>
        await BussinessValidationAsync(request, cancellationToken).ConfigureAwait(false);

    private async Task<Result<Address>> CreateAndStoreAddressAsync(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        var addressResponse = await _viaCepIntegration.GetAddressByCepAsync(request.AddressCep, cancellationToken);
        if (addressResponse.HasNoValue)
            return Result.Failure<Address>(DomainErrors.Address.CepNotFound);

        var newAddress = Address.Create(
            request.AddressFirstName,
            request.AddressLastName,
            addressResponse.Value.City,
            addressResponse.Value.State,
            addressResponse.Value.StateAbbreviation,
            addressResponse.Value.Neighborhood,
            request.AddressNumber,
            addressResponse.Value.Street,
            request.AddressCep,
            request.AddressPhoneNumber);

        await _addressRepository.InsertAsync(newAddress, cancellationToken);
        return Result.Success(newAddress);
    }

    private Business CreateBusiness(CreateBusinessCommand request, Guid ownerId, Guid addressId)
    {
        return Business.Create(
            request.Name,
            request.HomePageTitle,
            request.HomePageDescription,
            request.Cnpj,
            ownerId,
            addressId);
    }

    private async Task<Guid> PromoteUserIfNecessaryAsync(User currentUser, CancellationToken cancellationToken)
    {
        if (currentUser is Customer customer)
        {
            var professional = customer.PromoteToProfessional();

            _userRepository.Remove(currentUser);
            await _userRepository.InsertAsync(professional);

            return professional.Id;
        }

        return currentUser.Id;
    }

    #endregion

    #region Handle

    public async Task<Result> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        var validationResult = await ValidateBusinessAsync(request, cancellationToken);
        if (!validationResult.IsSuccess)
            return Result.Failure(validationResult.Error);

        var address = await CreateAndStoreAddressAsync(request, cancellationToken);
        if (address.IsFailure)
            return Result.Failure(address.Error);

        var ownerId = await PromoteUserIfNecessaryAsync(currentUser, cancellationToken);

        var business = CreateBusiness(request, ownerId, address.Value.Id);
        business.AddValidationToken();

        await _businessRepository.InsertAsync(business, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion 
}
