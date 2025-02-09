namespace BeautyGo.Domain.Common.Defaults;

public class BeautyGoServicesDefaults
{
    public const string DefaultHashedPasswordFormat = "SHA512";

    public static int PasswordSaltKeySize => 5;
}
