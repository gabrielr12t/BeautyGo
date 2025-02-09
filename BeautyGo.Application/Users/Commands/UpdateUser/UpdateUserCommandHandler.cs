using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Application.Users.Commands.UpdateUser;

internal class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IBaseRepository<User> _userRepository;

    public UpdateUserCommandHandler(IAuthService authService,
        IBaseRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();

        currentUser.ChangeName(request.FirstName, request.LastName);

        _userRepository.Update(currentUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
