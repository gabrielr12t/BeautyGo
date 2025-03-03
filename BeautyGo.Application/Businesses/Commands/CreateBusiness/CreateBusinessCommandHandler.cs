﻿using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Common;
using BeautyGo.Domain.Entities.Events;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications.Business;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Businesses.Commands.CreateBusiness;

internal class CreateBusinessCommandHandler : ICommandHandler<CreateBusinessCommand, Result>
{ 
    private readonly IBaseRepository<Business> _storeRepository;
    private readonly IBaseRepository<Address> _addressRepository;
    private readonly IBaseRepository<Event> _eventRepository;

    private readonly IReceitaFederalIntegrationService _receitaFederalIntegration;
    private readonly IViaCepIntegrationService _viaCepIntegration;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork; 

    public CreateBusinessCommandHandler(
        IBaseRepository<Business> storeRepository,
        IBaseRepository<Address> addressRepository,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IReceitaFederalIntegrationService receitaFederalIntegration,
        IViaCepIntegrationService viaCepIntegration,
        IBaseRepository<Event> eventRepository)
    {
        _receitaFederalIntegration = receitaFederalIntegration;
        _storeRepository = storeRepository;
        _addressRepository = addressRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _viaCepIntegration = viaCepIntegration;
        _eventRepository = eventRepository;
    }

    #region Utilities

    private async Task<Result> BussinessValidationAsync(CreateBusinessCommand request, CancellationToken cancellationToken)
    {
        if (!CommonHelper.IsValidCnpj(request.Cnpj))
            return Result.Failure(DomainErrors.Business.InvalidCnpj(request.Cnpj));

        var businessByCnpjSpec = new BusinessByCnpjSpecification(request.Cnpj);
        if (await _storeRepository.ExistAsync(businessByCnpjSpec, cancellationToken))
            return Result.Failure(DomainErrors.Business.CnpjAlreadyExists);

        ////var cnpjReceitaFederalResponse = await _receitaFederalIntegration.GetCnpjDataAsync(request.Cnpj, cancellationToken);
        ////if (!cnpjReceitaFederalResponse.HasValue)
        ////    return Result.Failure(DomainErrors.Business.InvalidCnpj);

        ////if (!_receitaFederalIntegration.IsValidCnpjStatus(cnpjReceitaFederalResponse.Value.Status, cnpjReceitaFederalResponse.Value.Situacao))
        ////    return Result.Failure(DomainErrors.Business.CnpjRestricted);

        ////var VerifyCompanyNameSimilarity = CommonHelper.CheckProximityWithThreshold(cnpjReceitaFederalResponse.Value.Nome.ToUpper(), request.Name.ToUpper(), 0.8);
        ////if (!VerifyCompanyNameSimilarity)
        ////    return Result.Failure(DomainErrors.Business.CnpjNameInvalid);

        return Result.Success();
    }

    #endregion

    public async Task<Result> Handle(CreateBusinessCommand request, CancellationToken cancellationToken)
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

        var newBusiness = Business.Create(request.Name,
            request.HomePageTitle,
            request.HomePageDescription,
            request.Cnpj,
            currentUser.Id, newAddress.Id);

        newBusiness.AddValidationToken();

        var hasWorkingHours = request.WorkingHours is not null && request.WorkingHours.Any();
        if (hasWorkingHours)
        {
            var workingHours = request.WorkingHours.Select(p => BusinessWorkingHours.Create(p.Day, p.EndTime, p.EndTime));
            newBusiness.AddWorkingHours(workingHours);
        }

        await _storeRepository.InsertAsync(newBusiness, cancellationToken); 
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
