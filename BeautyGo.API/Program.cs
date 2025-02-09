using BeautyGo.Application;
using BeautyGo.Infrastructure;
using BeautyGo.Infrastructure.Core;
using BeautyGo.Infrastructure.Extensions;
using BeautyGo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .MinimumLevel.Information();
});

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
builder.Services.AddEndpointsApiExplorer();

var engine = EngineContext.Create();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment)
    .AddPersistence(builder.Configuration);

var app = builder.Build();


EngineContext.Current.ConfigureRequestPipeline(app);

app.UseBeautyGoStaticFiles();

app.UseSession();

app.UseHttpsRedirection();

app.UseRateLimiter();

app.BeautyGoUseMiddleware();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
         name: "Store",
         pattern: "{storeHost}",
         defaults: new { controller = "Store", action = "Details" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
