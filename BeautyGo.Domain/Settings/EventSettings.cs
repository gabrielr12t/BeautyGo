namespace BeautyGo.Domain.Settings;

public class EventSettings : ISettings
{
    public string SettingsKey => "Event";

    public int MaxAttempsFailed { get; set; }
}
