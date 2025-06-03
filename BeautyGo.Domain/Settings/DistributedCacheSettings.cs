using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace BeautyGo.Domain.Settings;

public partial class DistributedCacheSettings : ISettings
{
    public string SettingsKey => "DistributedCache";

    [JsonConverter(typeof(StringEnumConverter))]
    public DistributedCacheType DistributedCacheType { get; protected set; } = DistributedCacheType.Memory;

    public bool Enabled { get; protected set; } = true;

    public string ConnectionString { get; protected set; } = "127.0.0.1:6379,ssl=False";

    public string SchemaName { get; protected set; } = "dbo";

    public string TableName { get; protected set; } = "DistributedCache";

    public string InstanceName { get; protected set; } = "BeautyGo";

    public int PublishIntervalMs { get; protected set; } = 500;
}

public enum DistributedCacheType
{
    [EnumMember(Value = "memory")]
    Memory,
    [EnumMember(Value = "sqlserver")]
    SqlServer,
    [EnumMember(Value = "redis")]
    Redis,
    [EnumMember(Value = "redissynchronizedmemory")]
    RedisSynchronizedMemory
}
