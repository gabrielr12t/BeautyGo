using Serilog;
using BeautyGo.Persistence;
using BeautyGo.Infrastructure;
using BeautyGo.BackgroundTasks;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .WriteTo.Console()
        .MinimumLevel.Information();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddHttpContextAccessor()
     .AddInfrastructure(builder.Configuration, builder.Environment)
     .AddPersistence(builder.Configuration)
     .AddBackgroundTasks(builder.Configuration);


var app = builder.Build();


app.Run();
 