namespace BeautyGo.Domain.Settings
{
    public sealed class MessageBrokerSettings : ISettings
    {
        public   string SettingsKey => "MessageBroker";

        public string Name => "MessageBroker";

        public string HostName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string ExchangeName { get; set; }

        public string Password { get; set; }

        public string QueueName { get; set; }

        public string RetryQueueName { get; set; }

        public string DLQName { get; set; }
    }
}
