using BeautyGo.Application;
using BeautyGo.Infrastructure;
using BeautyGo.Infrastructure.Core;
using BeautyGo.Infrastructure.Extensions;
using BeautyGo.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Newtonsoft.Json;
using Serilog;
using System.Net.Security;

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

app.BeautyGoUseMiddleware();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();
app.MapControllers();

app.Run();
