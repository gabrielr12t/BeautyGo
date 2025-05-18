namespace BeautyGo.Infrastructure.Services.Authentication;

public static partial class BeautyGoAuthenticationDefaults
{
    public static string AuthenticationScheme => "Bearer";

    public static string Authorization = "Authorization";

    public static string ClaimsIssuer => "bg-auth";

    public static string SaltDefault => "=kL5rAAEneWO7";

    public static string RelatedRefreshTokenPrefix => "BeautyGo.relatedrefreshtoken.byrefreshtoken.{0}";
}
