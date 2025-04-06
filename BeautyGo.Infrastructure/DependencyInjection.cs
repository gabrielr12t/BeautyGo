using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Cryptography;
using BeautyGo.Application.Core.Abstractions.Emails;
using BeautyGo.Application.Core.Abstractions.FakeData;
using BeautyGo.Application.Core.Abstractions.Integrations;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.Application.Core.Abstractions.Media;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Application.Core.Abstractions.Notifications;
using BeautyGo.Application.Core.Abstractions.Security;
using BeautyGo.Application.Core.Abstractions.Business;
using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Application.Core.Abstractions.Web;
using BeautyGo.Domain.Core.Configurations;
using BeautyGo.Domain.Patterns.Singletons;
using BeautyGo.Domain.Providers.Files;
using BeautyGo.Domain.Settings;
using BeautyGo.Infrastructure.Core;
using BeautyGo.Infrastructure.Emails;
using BeautyGo.Infrastructure.Extensions;
using BeautyGo.Infrastructure.Messaging;
using BeautyGo.Infrastructure.Mvc.HttpServices.DelegatingHandlers;
using BeautyGo.Infrastructure.Notifications;
using BeautyGo.Infrastructure.Services.Authentication;
using BeautyGo.Infrastructure.Services.Integrations;
using BeautyGo.Infrastructure.Services.Media;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using BeautyGo.Infrastructure.Services.Users;
using BeautyGo.Infrastructure.Services.Installation;
using BeautyGo.Infrastructure.Services.Security;
using BeautyGo.Infrastructure.Services.Web;
using BeautyGo.Infrastructure.Services.Logging;
using BeautyGo.Infrastructure.Services.Cryptography;
using BeautyGo.Infrastructure.Services.Business;
using BeautyGo.Infrastructure.Services.Authentication.Events;
using BeautyGo.Application.Core.Abstractions.OutboxMessages;
using BeautyGo.Infrastructure.Services.OutboxMessages;
using System.Buffers.Text;
using System.Web;

namespace BeautyGo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        services.AddResponseCompression();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddMvc();
        services.AddSession();

        services.AddSettings(webHostEnvironment, configuration);

        services.AddBearerAuthentication(configuration); 
        services.AddRateLimiterIp();
        services.AddHttpContextAccessor();

        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();
        services.AddScoped<IPictureService, PictureService>();
        services.AddScoped<IBeautyGoFileProvider, BeautyGoFileProvider>();
        services.AddScoped<IWebHelper, WebHelper>();
        services.AddScoped<IWebWorkContext, WebWorkContext>();
        services.AddScoped<ILogger, DefaultLogger>();
        services.AddScoped<IBusinessContext, BusinessContext>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IEnvironmentVariableService, EnvironmentVariableService>();
        services.AddScoped<IInstallationService, InstallationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFakeDataService, FakeDataService>();
        services.AddScoped<IOutboxMessageService, OutboxMessageService>(); 
        services.AddScoped<IReceitaFederalIntegrationService, ReceitaFederalIntegrationService>();
        services.AddScoped<IViaCepIntegrationService, ViaCepIntegration>();
        services.AddScoped<ILocationIQIntegrationService, LocationIQIntegrationServiceIntegrationService>();

        services.AddSingleton<IPushNotificationService, PushNotificationService>();
        services.AddSingleton<IPublisherBusEvent, RabbitMqBusEvent>();

        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IPasswordHashChecker, PasswordHasher>();
        services.AddTransient<IEmailNotificationService, EmailNotificationService>();
        services.AddTransient<IUserEmailNotificationPublisher, UserEmailNotificationService>();
        services.AddTransient<IBusinessEmailNotificationPublisher, BusinessEmailNotificationService>();
        services.AddTransient<EmailService>();
        services.AddTransient<IEmailService>(serviceProvicer =>
        {
            var realSender = serviceProvicer.GetRequiredService<EmailService>();
            var emailSettings = serviceProvicer.GetRequiredService<AppSettings>().Get<RedirectMailSettings>();
            return new RedirectEmailSenderDecorator(realSender, emailSettings);
        });

        services.RegisterIntegrations();

        return services;
    }

    internal static IServiceCollection RegisterIntegrations(this IServiceCollection services)
    {
        services.AddConfiguredHttpClient<IReceitaFederalIntegrationService, ReceitaFederalIntegrationService, ReceitaFederalSettings>(
            settings => settings.Address,
            TimeSpan.FromSeconds(5000),
            TimeSpan.FromMinutes(5)
        );

        services.AddConfiguredHttpClient<IViaCepIntegrationService, ViaCepIntegration, ViaCepSettings>(
            settings => settings.Address,
            TimeSpan.FromSeconds(5000),
            TimeSpan.FromMinutes(5)
        );

        services.AddConfiguredHttpClient<ILocationIQIntegrationService, LocationIQIntegrationServiceIntegrationService, LocationIQSettings>(
            settings => settings.Address,
            TimeSpan.FromSeconds(5000),
            TimeSpan.FromMinutes(5),
            headers =>
            {
                headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            }
        );

        services.AddTransient<LoggerDelegatingHandler>();
        services.AddTransient<RetryDelegatingHandler>();

        return services;
    }

    public static void AddConfiguredHttpClient<TInterface, TImplementation, TSettings>(
        this IServiceCollection services,
        Func<TSettings, string> getAddress,
        TimeSpan timeout,
        TimeSpan handlerLifetime,
        Action<HttpRequestHeaders> addHeaders = null)
        where TImplementation : class, TInterface
        where TInterface : class
        where TSettings : class, ISettings
    {
        services
            .AddHttpClient<TInterface, TImplementation>((serviceProvider, client) =>
            {
                var settings = Singleton<AppSettings>.Instance.Get<TSettings>();
                var baseAddress = getAddress(settings);

                client.BaseAddress = new Uri(baseAddress);
                client.Timeout = timeout;

                addHeaders?.Invoke(client.DefaultRequestHeaders);
            })
            .AddHttpMessageHandler<LoggerDelegatingHandler>()
            .AddHttpMessageHandler<RetryDelegatingHandler>()
            .SetHandlerLifetime(handlerLifetime);
    }


    internal static IServiceCollection AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var security = Singleton<AppSettings>.Instance.Get<SecuritySettings>();
        var auth = Singleton<AppSettings>.Instance.Get<AuthSettings>();

        services.AddTransient<AuthBearerEvents>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var rsa = RSA.Create();
            var key = File.ReadAllText(security.PublicKeyFilePath);
            rsa.FromXmlString(key);

            options.EventsType = typeof(AuthBearerEvents);
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = auth.Issuer,
                ValidAudience = auth.Audience,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ClockSkew = TimeSpan.Zero,
            };
        });

        services.AddAuthorization();

        return services;
    }

    internal static IServiceCollection AddSettings(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        var fileProvider = new BeautyGoFileProvider(webHostEnvironment);

        var typeFinder = new WebAppTypeFinder(fileProvider);
        Singleton<ITypeFinder>.Instance = typeFinder;
        services.AddSingleton<ITypeFinder>(typeFinder);

        var configurations = typeFinder
            .FindClassesOfType<ISettings>()
            .Select(configType => (ISettings)Activator.CreateInstance(configType))
            .ToList();

        foreach (var config in configurations)
            configuration.GetSection(config.SettingsKey).Bind(config, options => options.BindNonPublicProperties = true);

        var appSettings = Singleton<AppSettings>.Instance ?? new AppSettings();
        appSettings.Update(configurations);
        Singleton<AppSettings>.Instance = appSettings;
        services.AddSingleton(appSettings);

        return services;
    }
}
