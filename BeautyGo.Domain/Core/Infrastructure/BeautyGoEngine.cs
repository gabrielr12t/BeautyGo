using BeautyGo.Domain.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using static BeautyGo.Domain.Core.Errors.DomainErrors;

namespace BeautyGo.Domain.Core.Infrastructure;

public class BeautyGoEngine : IEngine
{
    #region Utilities

    protected IServiceProvider GetServiceProvider(IServiceScope scope = null)
    {
        if (scope == null)
        {
            var accessor = ServiceProvider?.GetService<IHttpContextAccessor>();
            var context = accessor?.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }
        return scope.ServiceProvider;
    }

    #endregion

    #region Methods

    public void ConfigureRequestPipeline(IApplicationBuilder application)
    {
        ServiceProvider = application.ApplicationServices;
    }

    public T Resolve<T>(IServiceScope scope = null) where T : class
    {
        return (T)Resolve(typeof(T), scope);
    }

    public object Resolve(Type type, IServiceScope scope = null)
    {
        return GetServiceProvider(scope)?.GetService(type);
    }

    public virtual IEnumerable<T> ResolveAll<T>()
    {
        return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
    }

    public virtual object ResolveUnregistered(Type type)
    {
        Exception innerException = null;
        foreach (var constructor in type.GetConstructors())
        {
            try
            {
                var parameters = constructor.GetParameters().Select(parameter =>
                {
                    var service = Resolve(parameter.ParameterType);
                    if (service == null)
                        throw new DomainException(General.ServiceNotRegistered);
                    return service;
                });

                return Activator.CreateInstance(type, parameters.ToArray());
            }
            catch (Exception ex)
            {
                innerException = ex;
            }
        }

        throw new DomainException(General.ConstructorNotFound);
    }

    #endregion

    #region Properties

    public virtual IServiceProvider ServiceProvider { get; protected set; }

    #endregion
}

