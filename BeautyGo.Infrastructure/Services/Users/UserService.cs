using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Domain.Entities.Customers;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.Infrastructure.Services.Users;

internal class UserService : IUserService
{
    #region Fields

    private readonly IBaseRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    #region Ctor

    public UserService(IBaseRepository<User> userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task PromoteCustomerUserToOwnerlAsync(Customer customer, CancellationToken cancellationToken)
    {
        var owner = customer.PromoteToOwner();

        _userRepository.Remove(customer);
        await _userRepository.InsertAsync(owner, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
