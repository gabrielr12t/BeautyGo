using BeautyGo.Contracts.Authentication;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Core.Abstractions.Authentication;

public interface IAuthService
{
    Task<TokenModel> AuthenticateAsync(User user);

    Task<Result<RefreshTokenModel>> RefreshTokenAsync(Guid userId, string refreshToken);

    Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default);

    Task<bool> AuthorizeAsync(string role, CancellationToken cancellationToken = default);

    Task<bool> AuthorizeAsync(string role, User user, CancellationToken cancellationToken = default);

    Task PromoteCustomerToOwnerAsync(Customer customer, CancellationToken cancellationToken = default);

    Task PromoteCustomerToProfessionalAsync(Customer customer, Guid businessId, CancellationToken cancellationToken = default);
}
