using BeautyGo.Application.Core.Providers;
using System.Reflection;

namespace BeautyGo.Infrastructure.Core;

public class WebAppTypeFinder : AppDomainTypeFinder
{
    #region Fields

    private bool _binFolderAssembliesLoaded;

    #endregion

    #region Ctor

    public WebAppTypeFinder(IBeautyGoFileProvider fileProvider) : base(fileProvider)
    {
    }

    #endregion

    #region Properties

    public bool EnsureBinFolderAssembliesLoaded { get; set; } = true;

    #endregion

    #region Methods

    public virtual string GetBinDirectory()
    {
        return AppContext.BaseDirectory;
    }

    public override IList<Assembly> GetAssemblies()
    {
        if (!EnsureBinFolderAssembliesLoaded || _binFolderAssembliesLoaded)
            return base.GetAssemblies();

        _binFolderAssembliesLoaded = true;
        var binPath = GetBinDirectory();
        LoadMatchingAssemblies(binPath);

        return base.GetAssemblies();
    }

    #endregion
}
