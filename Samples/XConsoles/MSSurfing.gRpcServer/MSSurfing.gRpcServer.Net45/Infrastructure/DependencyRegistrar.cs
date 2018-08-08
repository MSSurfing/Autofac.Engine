using Autofac;
using Autofac.Engine;
using MSSurfing.gRpcServer.Net45.GServices;
using MSSurfing.gRpcServer.Net45.Services;
using MSSurfing.Services;
using MSSurfing.Services.Logging;

namespace MSSurfing.gRpcServer.Net45.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //match
            builder.RegisterTypeMapper(typeFinder, "MSSurfing.Services.dll", "Service");
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();

            //repository generic type
            builder.RegisterGeneric(typeof(BatchRepository<>)).As(typeof(IRepository<>)).SingleInstance();

            //repository instance class
            builder.RegisterType<GUserService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GPluginService>().AsSelf().InstancePerLifetimeScope();
            //builder.Register(c => new UserProcessor(c.Resolve<IUserService>())).InstancePerDependency();

        }
    }
}
