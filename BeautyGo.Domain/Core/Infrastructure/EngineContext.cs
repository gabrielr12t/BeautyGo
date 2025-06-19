using BeautyGo.Domain.Patterns.Singletons;
using System.Runtime.CompilerServices;

namespace BeautyGo.Domain.Core.Infrastructure;

public class EngineContext
{
    #region Methods

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static IEngine Create()
    {
        return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new BeautyGoEngine());
    }

    public static void Replace(IEngine engine)
    {
        Singleton<IEngine>.Instance = engine;
    }

    #endregion

    #region Properties

    public static IEngine Current
    {
        get
        {
            if (Singleton<IEngine>.Instance == null)
            {
                Create();
            }

            return Singleton<IEngine>.Instance;
        }
    }

    #endregion
}
