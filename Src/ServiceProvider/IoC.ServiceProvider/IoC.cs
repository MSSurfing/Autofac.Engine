using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.ServiceProvider
{
    public static class IoC
    {
        #region Properties
        public static IServiceProvider ServiceProvider { get; private set; }
        #endregion


        public static IServiceCollection AddIoC(this IServiceCollection services, IConfiguration configuration)
        {
            var typeFinder = new AppDomainTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;
            services.AddSingleton<ITypeFinder>(typeFinder);

            // todo provide ?IConfigureServices startup to add services to IServiceCollection

            return services;
        }

        public static void UseIoC(this IApplicationBuilder app)
        {
            ServiceProvider = app.ApplicationServices;

            // todo provide ?IConfigure startup configure the HTTP request pipeline.
        }
    }
}
