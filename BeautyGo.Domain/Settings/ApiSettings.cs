namespace BeautyGo.Domain.Settings
{
    public class ApiSettings : ISettings
    {
        public string SettingsKey => "Api";

        public string Host { get; set; }

        public Endpoint Endpoints { get; set; }
    }

    public class Endpoint
    {
        public string UserConfirmEmail { get; set; }
        public string BusinessConfirmEmail { get; set; }
    }
}
