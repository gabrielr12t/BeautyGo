using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Repositories;
using BeautyGo.Domain.Settings;
using BeautyGo.Persistence.Interceptors;
using BeautyGo.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;

namespace BeautyGo.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        var connectionSettings = Singleton<AppSettings>.Instance.Get<ConnectionStringSettings>();

        connectionSettings.Value = configuration.GetConnectionString(connectionSettings.SettingsKey);

        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<DispatchDomainEventInterceptor>();
        services.AddScoped<SoftDeletableInterceptor>();
        services.AddScoped<SetCreatedOnInterceptor>();

        services.AddDbContext<BeautyGoContext>((sp, options) =>
                options
                    .UseSqlServer(connectionSettings.Value)
                    .EnableSensitiveDataLogging()
                    .AddInterceptors(
                        sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>(),
                        sp.GetRequiredService<DispatchDomainEventInterceptor>(),
                        sp.GetRequiredService<SoftDeletableInterceptor>(),
                        sp.GetRequiredService<SetCreatedOnInterceptor>()),
                    contextLifetime: ServiceLifetime.Scoped);

        services.AddScoped<IDbConnection>(sp => new SqlConnection(connectionSettings.Value));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IOutboxMessageRepository, OutboxMessagerepository>();
        services.AddScoped<IBusinessRepository, BusinessRepository>();

        return services;
    }
}
