namespace BeautyGo.Domain.Settings;

public class AuthSettings : ISettings
{
    public string SettingsKey => "Auth";

    public string Name => "Auth";

    public string Url { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int ExpirationTokenMinutes { get; set; }
}
