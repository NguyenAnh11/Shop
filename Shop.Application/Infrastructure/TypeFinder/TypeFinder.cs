using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Shop.Application.Infrastructure.TypeFinder
{
    public abstract class TypeFinder : ITypeFinder
    {
        private const string ASSEMBLY_RESTRICT_PATTERN = ".*";
        private const string ASSEMBLY_SKIP_PATTERN = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

        private static bool Matches(string assemblyName)
        {
            return Matches(assemblyName, ASSEMBLY_RESTRICT_PATTERN) &&
                !Matches(assemblyName, ASSEMBLY_SKIP_PATTERN);
        }

        private static bool Matches(string assemblyName, string pattern)
        {
            return Regex.IsMatch(assemblyName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public virtual IEnumerable<Assembly> Assemblies()
            => AppDomain.CurrentDomain.GetAssemblies();

        public static void LoadAssemblies(string binPath)
        {
            Guard.IsNotEmpty(binPath, nameof(binPath));

            var binDirectory = new DirectoryInfo(binPath);
            if (!binDirectory.Exists)
                throw new DirectoryNotFoundException();

            List<string> loadedAssemblyNames = new();

            foreach (var file in binDirectory.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var am = AssemblyName.GetAssemblyName(file.FullName);

                    if (Matches(am.FullName) && !loadedAssemblyNames.Contains(am.FullName))
                    {
                        AppDomain.CurrentDomain.Load(am);

                        loadedAssemblyNames.Add(am.FullName);
                    }
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.StackTrace);
                }
            }
        }

        public IEnumerable<Type> FindClassOfType<T>(bool onlyConcrete = true)
            => FindClassOfType(typeof(T), onlyConcrete);

        public IEnumerable<Type> FindClassOfType(Type baseType, bool onlyConcrete = true)
        {
            Guard.IsNotNull(baseType, nameof(baseType));

            var assemblies = Assemblies();

            if (assemblies == null || !assemblies.Any())
                return null;

            List<Type> types = new();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!baseType.IsAssignableFrom(type))
                        continue;

                    if (!type.IsClass)
                        continue;

                    if (onlyConcrete && type.IsAbstract)
                        continue;

                    types.Add(type);
                }
            }

            return types;
        }
    }
}
