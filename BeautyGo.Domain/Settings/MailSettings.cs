namespace BeautyGo.Domain.Settings;

public class MailSettings : ISettings
{
    public string SettingsKey => "Mail";

    public string SenderDisplayName { get; set; }

    public string SenderEmail { get; set; }

    public string SmtpPassword { get; set; }

    public string SmtpServer { get; set; }

    public int SmtpPort { get; set; }
}
