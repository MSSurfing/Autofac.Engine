using Autofac;
using Autofac.Engine;
using MSSurfing.Services;
using MSSurfing.Services.Logging;

namespace MSSurfing.WebApi.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 1;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
            builder.RegisterTypeMapper(typeFinder, "MSSurfing.Services.dll", "Service");
            builder.RegisterType<Logger>().As<ILogger>().InstancePerLifetimeScope();

            //repository 
            builder.RegisterGeneric(typeof(BatchRepository<>)).As(typeof(IRepository<>)).SingleInstance();
        }
    }
}
