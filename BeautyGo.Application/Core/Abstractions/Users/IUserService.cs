using BeautyGo.Domain.Entities.Customers;

namespace BeautyGo.Application.Core.Abstractions.Users;

public interface IUserService
{
    Task PromoteCustomerUserToOwnerlAsync(Customer customer, CancellationToken cancellationToken = default);
}
