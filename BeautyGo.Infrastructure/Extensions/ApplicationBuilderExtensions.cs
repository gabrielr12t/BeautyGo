using BeautyGo.Domain.Common.Defaults;
using BeautyGo.Domain.Providers.Files;
using BeautyGo.Infrastructure.Core;
using BeautyGo.Infrastructure.Mvc.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace BeautyGo.Infrastructure.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void UseBeautyGoStaticFiles(this IApplicationBuilder app)
    {
        app.UseResponseCompression();

        var fileProvider = EngineContext.Current.ResolveUnregistered(typeof(BeautyGoFileProvider)) as IBeautyGoFileProvider;

        var provider = new FileExtensionContentTypeProvider
        {
            Mappings = { [".bak"] = "application/octet-stream" }
        };

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(fileProvider.GetAbsolutePath(BeautyGoCommonDefault.DbBackupsPath)),
            RequestPath = new PathString("/db_backups"),
            ContentTypeProvider = provider
        });
    }

    public static IApplicationBuilder BeautyGoUseMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseMiddleware<BeautyGoValidationTokenMiddleware>();
        app.UseMiddleware<TransactionMiddleware>();
        app.UseMiddleware<InstallationMiddleware>();

        return app;
    }
}
