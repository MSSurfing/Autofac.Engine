using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace IoCServiceProvider
{
    public static class IoC
    {
        #region Properties
        public static IServiceProvider? ServiceProvider { get; private set; }


        public static IServiceProvider? GetServiceProvider(IServiceScope? scope = null)
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

        #region GetService methods
        public static T? GetService<T>(IServiceScope? scope = null) where T : class
            => Resolve<T>(scope);

        public static object? GetService(Type type, IServiceScope? scope = null) =>
            Resolve(type, scope);

        public static IEnumerable<T> GetServices<T>(IServiceScope? scope = null) =>
            ResolveAll<T>(scope);

        public static object? GetServiceUnregistered(Type type) =>
            ResolveUnregistered(type);
        #endregion

        #region Resolve methods

        public static T? Resolve<T>(IServiceScope? scope = null) where T : class
        {
            return Resolve(typeof(T), scope) as T;
        }

        public static object? Resolve(Type type, IServiceScope? scope = null)
        {
            return GetServiceProvider(scope)?.GetService(type);
        }

        public static IEnumerable<T> ResolveAll<T>(IServiceScope? scope = null)
        {
            var services = GetServiceProvider(scope)?.GetServices(typeof(T));
            if (services == null)
                return Enumerable.Empty<T>();

            return (IEnumerable<T>)services;
        }

        public static object? ResolveUnregistered(Type type)
        {
            Exception? innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new Exception("Unknown dependency");
                        return service;
                    });

                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new Exception("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #endregion


        #region AddIoC & UseIoC
        public static IServiceCollection AddIoC(this IServiceCollection services, IConfiguration configuration)
        {
            var typeFinder = new AppDomainTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            var instances = typeFinder.FindClassesOfType<IStartup>()
                .Select(startup => (IStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);

            return services;
        }

        public static void UseIoC(this IApplicationBuilder app)
        {
            ServiceProvider = app.ApplicationServices;

            var typeFinder = Singleton<ITypeFinder>.Instance;
            var instances = typeFinder.FindClassesOfType<IStartup>()
                .Select(startup => (IStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            foreach (var instance in instances)
                instance.Configure(app);
        }
        #endregion
    }
}
