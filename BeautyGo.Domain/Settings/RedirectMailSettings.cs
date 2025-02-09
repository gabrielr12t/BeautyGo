namespace BeautyGo.Domain.Settings;

public class RedirectMailSettings : ISettings
{
    public string SettingsKey => "RedirectMail";

    public bool RedirectEmailsInHomologation { get; set; }

    public string RedirectEmailTo { get; set; }
}
