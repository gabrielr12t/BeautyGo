﻿using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Application.Core.Factories.Users;
using BeautyGo.Contracts.Users;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;
using BeautyGo.Domain.Repositories.Bases;
using MediatR;

namespace BeautyGo.Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler :
    ICommandHandler<CreateUserCommand, Result<CreateUserResponse>>,
    IUserRoleHandlerVisitor
{
    #region Fields

    private readonly IEFBaseRepository<User> _userRepository;
    private readonly IEFBaseRepository<UserRole> userRoleRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserFactory userFactory;

    #endregion

    #region Ctor

    public CreateUserCommandHandler(
        IEFBaseRepository<User> userRepository,
        IEFBaseRepository<UserRole> userRoleRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IAuthService authService,
        IUserFactory userFactory)
    {
        this._userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
        this.unitOfWork = unitOfWork;
        this.userFactory = userFactory;
    }

    #endregion

    #region Utilities

    private async Task<Result> BussinessValidateAsync(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!CommonHelper.IsValidCpf(request.CPF))
            return Result.Failure(DomainErrors.User.InvalidCPF);

        if (!CommonHelper.IsValidEmail(request.Email))
            return Result.Failure(DomainErrors.User.InvalidEmail);

        if (!CommonHelper.IsValidPhoneNumber(request.Phone))
            return Result.Failure(DomainErrors.User.InvalidPhoneNumber);

        var userByEmailSpec = new UserByEmailSpecification(request.Email);
        if (await _userRepository.ExistAsync(userByEmailSpec))
            return Result.Failure(DomainErrors.User.EmailAlreadyExists);

        var userByCpfSpec = new UserByCpfSpecification(request.CPF);
        if (await _userRepository.ExistAsync(userByCpfSpec))
            return Result.Failure(DomainErrors.User.CPFAlreadyExists);

        var userByPhoneNumberSpec = new UserByPhoneNumberSpecification(request.CPF);
        if (await _userRepository.ExistAsync(userByPhoneNumberSpec))
            return Result.Failure(DomainErrors.User.PhoneNumberAlreadyExists);

        return Result.Success();
    }

    #endregion

    #region Handle User Role

    public async Task AssignRoleAsync(Customer customer, CancellationToken cancellationToken)
    {
        var customerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.CUSTOMER);
        var customerRole = await userRoleRepository.GetFirstOrDefaultAsync(customerRoleSpecification, true, cancellationToken);

        if (customerRole != null)
            customer.AddUserRole(customerRole);
    }

    public async Task AssignRoleAsync(Professional professional, CancellationToken cancellationToken)
    {
        var professionalRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.PROFESSIONAL);
        var professionalRole = await userRoleRepository.GetFirstOrDefaultAsync(professionalRoleSpecification, true, cancellationToken);

        if (professionalRole != null)
            professional.AddUserRole(professionalRole);
    }

    public async Task AssignRoleAsync(BusinessOwner owner, CancellationToken cancellationToken = default)
    {
        var ownerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.PROFESSIONAL);
        var ownerRole = await userRoleRepository.GetFirstOrDefaultAsync(ownerRoleSpecification, true, cancellationToken);

        if (ownerRole != null)
            owner.AddUserRole(ownerRole);
    }

    #endregion

    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Result bussinessValidate = await BussinessValidateAsync(request, cancellationToken);

        if (!bussinessValidate.IsSuccess)
            return Result.Failure<CreateUserResponse>(bussinessValidate.Error);

        var saltKey = EncryptionHelper.CreateSaltKey(request.Password.Length);
        var hashedPassword = EncryptionHelper.CreatePasswordHash(request.Password, saltKey);

        var user = userFactory.Create(request);

        user.ActivateUser();
        user.AddUserPassword(hashedPassword, saltKey);
        user.AddValidationToken();

        await user.HandleUserRoleAccept(this);

        await _userRepository.InsertAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateUserResponse(user.Id));
    }
}
