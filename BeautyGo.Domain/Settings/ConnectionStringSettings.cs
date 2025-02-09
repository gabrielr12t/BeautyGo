namespace BeautyGo.Domain.Settings
{
    public sealed class ConnectionStringSettings : ISettings
    {
        public string SettingsKey => "Connection";

        public string Name => "ConnectionStrings";

        public ConnectionStringSettings(string value) => Value = value;

        public ConnectionStringSettings() { }

        public string Value { get; set; }

        public static implicit operator string(ConnectionStringSettings connectionString) => connectionString.Value;
    }
}
