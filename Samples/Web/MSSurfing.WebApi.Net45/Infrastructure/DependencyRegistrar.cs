using Autofac;
using Autofac.Engine;
using Autofac.Integration.WebApi;
using MSSurfing.Services;
using MSSurfing.Services.Logging;
using System.Web;
using System.Linq;
using MSSurfing.WebApi.Configuration;

namespace MSSurfing.WebApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            #region Http & Web
            builder.Register(c => new HttpContextWrapper(HttpContext.Current) as HttpContextBase).As<HttpContextBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session).As<HttpSessionStateBase>().InstancePerLifetimeScope();
            #endregion

            #region Controllers
            //register mvc controller
            //builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            //register api controller
            builder.RegisterApiControllers(typeFinder.GetAssemblies().ToArray());
            #endregion


            //builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.dll", "Service").AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();

            //repository 
            builder.RegisterGeneric(typeof(MemoryRepository<>)).As(typeof(IRepository<>)).SingleInstance();

            // registered instance (change action)
            var msConfig = new MSConfig() { Version = "5.0.0.1" };
            builder.Register(msConfig, true);
        }
    }
}