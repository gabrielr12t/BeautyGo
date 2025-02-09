using BeautyGo.Application.Core.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BeautyGo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => 
            cfg
                .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
                .AddOpenBehavior(typeof(ValidationBehaviour<,>))
                .AddOpenBehavior(typeof(TransactionBehaviour<,>)));

        return services;
    }
}
