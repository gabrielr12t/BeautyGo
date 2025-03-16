using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrastructure.Services.Users;

internal class UserService : IUserService
{
    #region Fields

    private readonly IAuthService _authService;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserRoleMapping> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public UserService(
        IBaseRepository<User> userRepository, 
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IBaseRepository<UserRoleMapping> userRoleRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _authService = authService;
        _userRoleRepository = userRoleRepository;
    }

    #endregion

    public async Task<bool> AuthorizeAsync(string role, CancellationToken cancellationToken = default)
    {
        return await AuthorizeAsync(role, await _authService.GetCurrentUserAsync(cancellationToken));
    }

    public async Task<bool> AuthorizeAsync(string role, User user, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrEmpty(role)) 
            return false;

        var userRoleSpecification = new UserRoleByUserIdSpecification(user.Id);
        var userRoles = await _userRoleRepository.GetAsync(userRoleSpecification, cancellationToken: cancellationToken);

        return userRoles.Any(p => string.Equals(p.UserRole.Description, role, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task PromoteCustomerToOwnerAsync(Customer customer, CancellationToken cancellationToken)
    {
        var owner = customer.PromoteToOwner();

        _userRepository.Remove(customer);
        await _userRepository.InsertAsync(owner, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PromoteCustomerToProfessionalAsync(Customer customer, Guid businessId, CancellationToken cancellationToken)
    {
        var professional = customer.PromoteToProfessional(businessId);

        _userRepository.Remove(customer);
        await _userRepository.InsertAsync(professional, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
