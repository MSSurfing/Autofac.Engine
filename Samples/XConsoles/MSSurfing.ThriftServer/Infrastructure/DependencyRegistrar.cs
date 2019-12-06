using Autofac;
using Autofac.Engine;
using MSSurfing.Services;
using MSSurfing.Services.Logging;
using MSSurfing.ThriftServer.Processors;

namespace MSSurfing.ThriftServer.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.dll", "Service").AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();

            //repository generic type
            builder.RegisterGeneric(typeof(MemoryRepository<>)).As(typeof(IRepository<>)).SingleInstance();

            //repository instance class
            builder.RegisterType<UserProcessor>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<PluginProcessor>().AsSelf().InstancePerLifetimeScope();
            //builder.Register(c => new UserProcessor(c.Resolve<IUserService>())).InstancePerDependency();

        }
    }
}
