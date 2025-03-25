using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Patterns.Specifications.ProfessionalRequests;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Businesses.Commands.SendProfessionalRequest;

internal class SendProfessionalRequestCommandHandler : ICommandHandler<SendProfessionalRequestCommand, Result>
{
    #region Fields

    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Business> _businessRepository;
    private readonly IBaseRepository<ProfessionalRequest> _professionalRequestRepository;

    #endregion

    #region Ctor

    public SendProfessionalRequestCommandHandler(
        IUserService userService,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IBaseRepository<User> userRepository,
        IBaseRepository<Business> businessRepository,
        IBaseRepository<ProfessionalRequest> professionalRequestRepository)
    {
        _userService = userService;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _businessRepository = businessRepository;
        _professionalRequestRepository = professionalRequestRepository;
    }

    #endregion

    #region Utilities

    private async Task<Result> BussinessValidationAsync(SendProfessionalRequestCommand request, CancellationToken cancellationToken)
    {
        if (!await _userService.AuthorizeAsync(BeautyGoUserRoleDefaults.OWNER, cancellationToken))
            return Result.Failure(DomainErrors.General.UnauthorizedUser);

        if (!await _userRepository.ExistAsync(new EntityByIdSpecification<User>(request.UserId), cancellationToken))
            return Result.Failure(DomainErrors.User.UserNotFound);

        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Business>(request.BusinessId),
            cancellationToken: cancellationToken);

        if (business is null)
            return Result.Failure(DomainErrors.Business.BusinessNotFound(request.BusinessId));

        if (!business.IsOwner(await _authService.GetCurrentUserAsync(cancellationToken)))
            return Result.Failure(DomainErrors.Business.UserNotOwnerOfBusiness);

        var professionalRequestByUserIdSpec = new ProfessionalRequestByUserIdSpecification(request.UserId);
        var professionalRequestByBusinessIdSpec = new ProfessionalRequestByBusinessIdSpecification(request.BusinessId);

        var existingProfessionalRequest = await _professionalRequestRepository.GetFirstOrDefaultAsync(
                professionalRequestByUserIdSpec.And(professionalRequestByBusinessIdSpec));

        var alreadyExistProfessionalRequest = existingProfessionalRequest != null;

        if (alreadyExistProfessionalRequest && !existingProfessionalRequest.IsExpired())
            return Result.Failure(DomainErrors.ProfessionalRequest.ProfessionalRequestAlreadyExists);

        return Result.Success();
    }

    #endregion

    #region Handle

    public async Task<Result> Handle(SendProfessionalRequestCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await BussinessValidationAsync(request, cancellationToken);
        if (!validationResult.IsSuccess)
            return validationResult;

        var userTarget = await _userRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<User>(request.UserId),
            true,
            cancellationToken: cancellationToken);

        var businessTarget = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Business>(request.BusinessId),
            true,
            cancellationToken: cancellationToken);

        businessTarget.SendProfessionalRequest(userTarget);

        _businessRepository.Update(businessTarget);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion
}
