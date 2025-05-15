using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Contracts.Common.Filters;
using BeautyGo.Domain.Core.Abstractions;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Extensions;
using BeautyGo.Domain.Patterns.Specifications.UserRoles;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Infrastructure.Services.Users;

internal class UserService : IUserService
{
    #region Fields

    private readonly IAuthService _authService;
    private readonly IEFBaseRepository<User> _userRepository;
    private readonly IEFBaseRepository<UserRoleMapping> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public UserService(
        IEFBaseRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IEFBaseRepository<UserRoleMapping> userRoleRepository)
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
        if (string.IsNullOrEmpty(role))
            return false;

        var userRoleSpecification = new UserRoleByUserIdSpecification(user.Id);
        var userRoles = await _userRoleRepository.GetAsync(userRoleSpecification, cancellationToken: cancellationToken);

        return userRoles.Any(p => string.Equals(p.UserRole.Description, role, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task PromoteCustomerToOwnerAsync(Customer customer, CancellationToken cancellationToken)
    {
        var owner = customer.PromoteToOwner();

        await _userRepository.RemoveAsync(customer);
        await _userRepository.InsertAsync(owner, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PromoteCustomerToProfessionalAsync(Customer customer, Guid businessId, CancellationToken cancellationToken)
    {
        var professional = customer.PromoteToProfessional(businessId);

        await _userRepository.RemoveAsync(customer);
        await _userRepository.InsertAsync(professional, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IPagedList<User>> GetOnlineUsersAsync(FilterBase filter, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        var lastActivityFrom = DateTime.Now.AddMinutes(-10);

        var query = _userRepository.QueryNoTracking();

        query = query.Where(p => lastActivityFrom <= p.LastActivityDate);

        query = query.OrderByDescending(p => p.LastActivityDate);

        return await query.ToPagedListAsync(filter.PageIndex, filter.PageSize, false, cancellationToken);
    }
}
