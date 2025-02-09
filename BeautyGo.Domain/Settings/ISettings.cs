using Newtonsoft.Json;

namespace BeautyGo.Domain.Settings;

public interface ISettings
{
    string SettingsKey { get; }

    [JsonIgnore]
    string Name => GetType().Name;

    public int GetOrder() => 1;
}
