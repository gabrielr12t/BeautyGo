namespace BeautyGo.Domain.Patterns.Singletons;

public class BaseSingleton
{
    static BaseSingleton()
    {
        AllSingletons = new Dictionary<Type, object>();
    }

    public static IDictionary<Type, object> AllSingletons { get; }
}
