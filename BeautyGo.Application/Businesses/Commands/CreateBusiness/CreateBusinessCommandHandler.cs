using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Common;
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

    private bool UserIsAlreadyProfessional(User user) =>
        user is Professional;

    #endregion

    #region Handle

    public async Task<Result> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

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

        var newBusiness = Business.Create(request.Name,
            request.HomePageTitle,
            request.HomePageDescription,
            request.Cnpj,
            currentUser.Id, newAddress.Id);

        newBusiness.AddValidationToken();

        if (!UserIsAlreadyProfessional(currentUser))
        {
            currentUser = currentUser as Professional;
        }

        await _businessRepository.InsertAsync(newBusiness, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion 
}
