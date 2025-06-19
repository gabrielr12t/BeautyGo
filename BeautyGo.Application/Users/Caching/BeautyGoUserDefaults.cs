using BeautyGo.Domain.Caching;

namespace BeautyGo.Application.Users.Caching;

public static class BeautyGoUserDefaults
{
    public static CacheKey UserByUserEmail => new("BeautyGo.user.byemail.{0}");
}
