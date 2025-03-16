using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
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

    #endregion

    #region Ctor

    public SendProfessionalRequestCommandHandler(
        IUserService userService,
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IBaseRepository<User> userRepository,
        IBaseRepository<Business> businessRepository)
    {
        _userService = userService;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _businessRepository = businessRepository;
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

        if (!business.IsUserOwner(await _authService.GetCurrentUserAsync(cancellationToken)))
            return Result.Failure(DomainErrors.Business.UserNotOwnerOfBusiness);

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

        var business = await _businessRepository.GetFirstOrDefaultAsync(
            new EntityByIdSpecification<Business>(request.BusinessId),
            true,
            cancellationToken: cancellationToken);

        business.SendProfessionalRequest(userTarget);

        _businessRepository.Update(business);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion
}
