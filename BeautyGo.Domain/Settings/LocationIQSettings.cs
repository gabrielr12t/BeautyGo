namespace BeautyGo.Domain.Settings;

public class LocationIQSettings : IntegrationSettings, ISettings
{
    public string SettingsKey => "LocationIQIntegration";

    public string Token { get; set; }
}
