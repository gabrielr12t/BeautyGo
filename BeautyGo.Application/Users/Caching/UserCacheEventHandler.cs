using BeautyGo.Application.Core.Caching;
using BeautyGo.Domain.Entities.Users;

namespace BeautyGo.Application.Users.Caching;

internal class UserCacheEventHandler : CacheEventHandler<User>
{
    protected override async Task ClearCacheAsync(User entity, CancellationToken cancellationToken = default)
    {
        await RemoveAsync(BeautyGoUserDefaults.UserByUserEmail, entity);
    }
}
