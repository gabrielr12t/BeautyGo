using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Abstractions.Users;

public interface IUserService
{
    Task<bool> AuthorizeAsync(string role, CancellationToken cancellationToken = default);
    Task<bool> AuthorizeAsync(string role, User user, CancellationToken cancellationToken = default);
    Task PromoteCustomerToOwnerAsync(Customer customer, CancellationToken cancellationToken = default);
    Task PromoteCustomerToProfessionalAsync(Customer customer, Guid businessId, CancellationToken cancellationToken = default);
}
