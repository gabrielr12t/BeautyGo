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
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Application.Businesses.Commands.SendProfessionalRequest;

public class SendProfessionalRequestCommandHandler : ICommandHandler<SendProfessionalRequestCommand, Result>
{
    #region Fields

    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEFBaseRepository<User> _userRepository;
    private readonly IEFBaseRepository<Business> _businessRepository;
    private readonly IEFBaseRepository<ProfessionalRequest> _professionalRequestRepository;

    #endregion

    #region Ctor

    public SendProfessionalRequestCommandHandler(
        IUserService userService,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IEFBaseRepository<User> userRepository,
        IEFBaseRepository<Business> businessRepository,
        IEFBaseRepository<ProfessionalRequest> professionalRequestRepository)
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

    private async Task<Result> BusinessValidationAsync(SendProfessionalRequestCommand request, CancellationToken cancellationToken)
    {
        if (!await _userService.AuthorizeAsync(BeautyGoUserRoleDefaults.OWNER, cancellationToken))
            return Result.Failure(DomainErrors.General.UnauthorizedUser);

        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        if (request.UserId == currentUser.Id)
            return Result.Failure(DomainErrors.ProfessionalRequest.CannotSendRequestToYourself);

        if (!await _userRepository.ExistAsync(new EntityByIdSpecification<User>(request.UserId), cancellationToken))
            return Result.Failure(DomainErrors.User.UserNotFound);

        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Business>(request.BusinessId),
            cancellationToken: cancellationToken);

        if (business is null)
            return Result.Failure(DomainErrors.Business.BusinessNotFound(request.BusinessId));

        if (!business.IsOwner(currentUser))
            return Result.Failure(DomainErrors.Business.UserNotOwnerOfBusiness);

        if (!business.CanWork())
            return Result.Failure(DomainErrors.Business.CannotWork);

        var professionalRequestByUserIdSpec = new ProfessionalRequestByUserIdSpecification(request.UserId);
        var professionalRequestByBusinessIdSpec = new ProfessionalRequestByBusinessIdSpecification(request.BusinessId);

        var existingProfessionalRequest = await _professionalRequestRepository.GetFirstOrDefaultAsync(
                professionalRequestByUserIdSpec.And(professionalRequestByBusinessIdSpec), cancellationToken: cancellationToken);

        var alreadyExistProfessionalRequest = existingProfessionalRequest != null;

        if (alreadyExistProfessionalRequest && !existingProfessionalRequest.IsExpired())
            return Result.Failure(DomainErrors.ProfessionalRequest.ProfessionalRequestAlreadyExists);

        return Result.Success();
    }

    #endregion

    #region Handle

    //TESTAR A PARTIR DAQUI
    public async Task<Result> Handle(SendProfessionalRequestCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await BusinessValidationAsync(request, cancellationToken);
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

        await _businessRepository.UpdateAsync(businessTarget);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion
}
