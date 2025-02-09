namespace BeautyGo.Domain.Settings
{
    public class RedisSettings : ISettings
    {
        public string SettingsKey => "RedisSettings";

        public string Name => "RedisSettings";

        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }

        public int? DatabaseId { get; set; }

        public bool UseCaching { get; set; }

        public bool StoreDataProtectionKeys { get; set; }

        public bool IgnoreTimeoutException { get; set; }
    }
}
