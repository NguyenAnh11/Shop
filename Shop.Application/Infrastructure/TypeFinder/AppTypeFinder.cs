using System.Reflection;

namespace Shop.Application.Infrastructure.TypeFinder
{
    public class AppTypeFinder : TypeFinder
    {
        private readonly bool _ensureBinFolderAssembliesLoaded = true;
        private bool _binFolderAssembliesLoaded = false;

        public override IEnumerable<Assembly> Assemblies()
        {
            if (_ensureBinFolderAssembliesLoaded && _binFolderAssembliesLoaded)
                return base.Assemblies();

            _binFolderAssembliesLoaded = true;

            LoadAssemblies(AppContext.BaseDirectory);

            return base.Assemblies();
        }
    }
}
