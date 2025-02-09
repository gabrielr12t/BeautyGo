namespace BeautyGo.Domain.Settings
{
    public class CacheSettings : ISettings
    {
        public string SettingsKey => "Cache";
        public string Name => "Cache";

        public int DefaultCacheTime { get; set; }

        public int ShortTermCacheTime { get; set; }

        public int LongCacheTime { get; set; }

        public int BundledFilesCacheTime { get; set; }
    }
}
