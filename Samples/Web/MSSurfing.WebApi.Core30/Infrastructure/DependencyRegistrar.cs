using Autofac;
using Autofac.Engine;
using MSSurfing.Services;
using MSSurfing.Services.Logging;
using MSSurfing.WebApi.Core30.Configuration;

namespace MSSurfing.WebApi.Core30.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.dll", "Service").AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();

            //repository 
            builder.RegisterGeneric(typeof(MemoryRepository<>)).As(typeof(IRepository<>)).SingleInstance();

            // registered instance (change action)
            var msConfig = new MSConfig() { Version = "5.0.0.1" };
            builder.Register(msConfig, true);
            //builder.Register<Action<MSConfig>>(context => config => msConfig = config);
        }
    }
}
