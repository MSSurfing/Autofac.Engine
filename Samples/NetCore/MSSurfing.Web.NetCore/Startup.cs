using Autofac.Core.Lifetime;
using Autofac.Engine;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MSSurfing.Web.NetCore.Services;
using System;

namespace MSSurfing.Web.NetCore
{
    public class Startup
    {
        private static IServiceProvider serviceProvider { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            EngineContext.Initialize(false, build => build.Populate(services), ScopeTag.Provider,
                container =>
                {
                    if (serviceProvider == null)
                        serviceProvider = new AutofacServiceProvider(container);

                    return serviceProvider as LifetimeScope;    //it will be null;
                });

            var user = EngineContext.Resolve<IUserService>();
            int c = user.Count();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id}");
            });
        }
    }
}
