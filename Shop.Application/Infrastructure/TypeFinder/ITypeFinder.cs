using System.Reflection;

namespace Shop.Application.Infrastructure.TypeFinder
{
    public interface ITypeFinder
    {
        IEnumerable<Assembly> Assemblies();

        IEnumerable<Type> FindClassOfType<T>(bool onlyConcrete = true);

        IEnumerable<Type> FindClassOfType(Type baseType, bool onlyConcrete = true);
    }
}
