namespace BeautyGo.Domain.Settings
{
    public class SecuritySettings : ISettings
    {
        public string SettingsKey => "Security";

        public string EncryptionKey { get; set; }

        public string PrivateKeyFilePath { get; set; }

        public bool UseAesEncryptionAlgorithm { get; set; }

        public string PublicKeyFilePath { get; set; }

        public string Name => SettingsKey;
    }
}
