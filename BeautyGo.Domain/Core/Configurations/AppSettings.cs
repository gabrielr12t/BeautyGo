using BeautyGo.Domain.Core.Exceptions;
using BeautyGo.Domain.Core.Primitives;
using BeautyGo.Domain.Settings;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace BeautyGo.Domain.Core.Configurations;

public partial class AppSettings
{
    #region Fields

    protected readonly Dictionary<Type, ISettings> _configurations;

    #endregion

    #region Ctor

    public AppSettings(IList<ISettings> configurations = null)
    {
        _configurations = configurations
                              ?.OrderBy(config => config.GetOrder())
                              ?.ToDictionary(config => config.GetType(), config => config)
                          ?? new Dictionary<Type, ISettings>();
    }

    #endregion

    #region Methods

    public TConfig Get<TConfig>() where TConfig : class, ISettings
    {
        if (_configurations[typeof(TConfig)] is not TConfig config)
            throw new DomainException(new Error("", $"No configuration with type '{typeof(TConfig)}' found"));

        return config;
    }

    public void Update(IList<ISettings> configurations)
    {
        foreach (var config in configurations)
        {
            _configurations[config.GetType()] = config;
        }
    }

    #endregion

    #region Properties

    [JsonExtensionData]
    public Dictionary<string, JToken> Configuration { get; set; }

    #endregion
}
