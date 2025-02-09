using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Helpers;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Patterns.Specifications.Users;
using BeautyGo.Domain.Repositories;
using MediatR;

namespace BeautyGo.Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result>
{
    #region Fields

    private readonly IBaseRepository<User> userRepository;
    private readonly IBaseRepository<UserRole> userRoleRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMediator mediator;
    private readonly IAuthService authService;
    private readonly IReceitaFederalIntegrationService _receitaIntegration;

    #endregion

    #region Ctor

    public CreateUserCommandHandler(IBaseRepository<User> userRepository,
        IBaseRepository<UserRole> userRoleRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        IAuthService authService,
        IReceitaFederalIntegrationService receitaIntegration)
    {
        this.userRepository = userRepository;
        this.userRoleRepository = userRoleRepository;
        this.unitOfWork = unitOfWork;
        this.mediator = mediator;
        this.authService = authService;
        _receitaIntegration = receitaIntegration;
    }

    #endregion

    #region Utilities

    private async Task<Result> BussinessValidateUser(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!CommonHelper.IsValidCpf(request.CPF))
            return Result.Failure(DomainErrors.User.InvalidCPF);

        if (!CommonHelper.IsValidEmail(request.Email))
            return Result.Failure(DomainErrors.User.InvalidEmail);

        var userByEmailSpec = new UserByEmailSpecification(request.Email);
        if (await userRepository.ExistAsync(userByEmailSpec))
            return Result.Failure(DomainErrors.User.EmailAlreadyExists);

        var userByCpfSpec = new UserByCpfSpecification(request.CPF);
        if (await userRepository.ExistAsync(userByCpfSpec))
            return Result.Failure(DomainErrors.User.CPFAlreadyExists);

        return Result.Success();
    }

    #endregion

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Result bussinessValidate = await BussinessValidateUser(request, cancellationToken);

        if (!bussinessValidate.IsSuccess)
            return Result.Failure<TokenModel>(bussinessValidate.Error);

        var customerRoleSpecification = new UserRoleByDescriptionSpecification(BeautyGoUserRoleDefaults.CUSTOMER);
        var customerRole = await userRoleRepository.GetFirstOrDefaultAsync(customerRoleSpecification, cancellationToken: cancellationToken);

        var saltKey = EncryptionHelper.CreateSaltKey(request.Password.Length);
        var hashedPassword = EncryptionHelper.CreatePasswordHash(request.Password, saltKey);

        var user = User.CreateCustomer(
            request.FirstName,
            request.LastName,
            request.Email,
            CommonHelper.EnsureNumericOnly(request.CPF));

        user.IncludeUserRole(customerRole);
        user.IncludeUserPassword(hashedPassword, saltKey);

        await userRepository.InsertAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}
