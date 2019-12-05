using Autofac;
using Autofac.Engine;
using MSSurfing.NRpcServer.Core30.Processors;
using MSSurfing.Services;
using MSSurfing.Services.Logging;

namespace MSSurfing.NRpcServer.Core30.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.dll", "Service").AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();

            //repository generic type
            builder.RegisterGeneric(typeof(BatchRepository<>)).As(typeof(IRepository<>)).SingleInstance();

            //repository instance class
            builder.RegisterType<UserProcessor>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<PluginProcessor>().AsSelf().InstancePerLifetimeScope();
            //builder.Register(c => new UserProcessor(c.Resolve<IUserService>())).InstancePerDependency();

        }
    }
}
