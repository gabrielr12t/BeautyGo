using System.Reflection;

namespace BeautyGo.Infrastructure.Core;

public interface ITypeFinder
{
    IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

    IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

    IList<Assembly> GetAssemblies();
}
