namespace BeautyGo.Infrasctructure.Services.Authentication;

public static partial class BeautyGoAuthenticationDefaults
{
    public static string AuthenticationScheme => "Bearer";

    public static string ClaimsIssuer => "fh-auth";

    public static string SaltDefault => "=kL5rAAEneWO7";

    public static string RelatedRefreshTokenPrefix => "BeautyGo.relatedrefreshtoken.byrefreshtoken.{0}";
}
