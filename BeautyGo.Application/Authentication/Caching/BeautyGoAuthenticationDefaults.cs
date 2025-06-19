using BeautyGo.Domain.Caching;

namespace BeautyGo.Application.Authentication.Caching;

public class BeautyGoAuthenticationDefaults
{
    public static CacheKey UserByUserEmail => new("BeautyGo.user.byemail.{0}");
}
