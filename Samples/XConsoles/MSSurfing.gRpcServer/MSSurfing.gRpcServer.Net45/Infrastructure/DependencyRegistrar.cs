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
            builder.RegisterAssemblyTypes(typeFinder, "MSSurfing.Services.dll", "Service")
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();
            //register, with name 
            builder.RegisterType<Logger>().As<ILogger>().Keyed<ILogger>("surfing_logger").InstancePerLifetimeScope();

            //repository generic type
            builder.RegisterGeneric(typeof(BatchRepository<>)).As(typeof(IRepository<>)).SingleInstance();

            //register, implemented class
            builder.RegisterType<GUserService>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GPluginService>().AsSelf().InstancePerLifetimeScope();


            //register, instance (config)
            //var msConfig = ConfigurationManager.GetSetion("MSConfig") as MSConfig;
            //builder.RegisterInstance(msConfig).As<MSSurfing>().SingleInstance();

            //register, instance
            //builder.Register(c => new UserProcessor(c.Resolve<IUserService>())).InstancePerDependency();

            //register, with parameters
            //builder.RegisterType<UserActionService>().As<IUserActionService>()
            //    .WithProperties(Autofac.Core.ResolvedParameter.ForKeyed<ILogger>("surfing_logger"))
            //    .InstancePerLifetimeScope();

        }
    }
}
