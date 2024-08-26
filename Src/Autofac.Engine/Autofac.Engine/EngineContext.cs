using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Autofac.Engine
{
    public partial class EngineContext
    {
        #region Fields
        private static Func<IContainer, ILifetimeScope> _scope = DefaultScope;
        private static IContainer _container;
        public static ILifetimeScope Scope { get => _scope(_container); }

        private static IServiceProvider _serviceProvider { get; set; }
        public static IServiceProvider ServiceProvider => _serviceProvider;
        #endregion

        #region DefaultScope / ServiceProvider Utilities
        protected static ILifetimeScope DefaultScope(IContainer container)
        {
            return container.BeginLifetimeScope();
        }

        protected static void SetDefaultScope(object tag)
        {
            _scope = (e) => e.BeginLifetimeScope(tag);
        }
        protected static void SetDefaultScope(ScopeTag tag)
        {
            switch (tag)
            {
                case ScopeTag.Http:
                    _scope = (e) => e.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
                    break;
                case ScopeTag.NewTag:
                    _scope = (e) => e.BeginLifetimeScope(new object());
                    break;
                default:
                    _scope = DefaultScope;
                    break;
            }
        }

#if NETCOREAPP3_0 || NET8_0
        protected static ContainerBuilder RegisterDependencies(ContainerBuilder builder = null, bool onlySafeAssembly = true)
        {
            if (builder == null)
                builder = new ContainerBuilder();

            var typeFinder = new DomainTypeFinder(onlySafeAssembly);
            builder.RegisterInstance<ITypeFinder>(typeFinder).As<ITypeFinder>().SingleInstance();

            var dependencyTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var dependencyInstances = new List<IDependencyRegistrar>();
            foreach (var dependency in dependencyTypes)
            {
                dependencyInstances.Add((IDependencyRegistrar)Activator.CreateInstance(dependency));
            }

            dependencyInstances = dependencyInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in dependencyInstances)
                dependencyRegistrar.Register(builder, typeFinder);

            builder.RegisterBuildCallback(container => { _container = (IContainer)container; });
            return builder;
        }
#endif
        #endregion

        #region Methods
        /// <summary>可传入 IApplicationBuilder Build() 后的 ApplicationServices </summary>
        public static void SetServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider;
        }

        /// <summary>
        /// Begin a new nested scope. Component instances created via the new scope will be disposed along with it.
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope(out object tag)
        {
            tag = new object();
            return _container.BeginLifetimeScope(tag);
        }

        /// <summary>
        /// Begin a new nested scope. Component instances created via the new scope will be disposed along with it.
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope(object tag)
        {
            return _container.BeginLifetimeScope(tag);
        }

        /// <summary>
        /// Begin a new nested scope. Component instances created via the new scope will be disposed along with it.
        /// </summary>
        public static ILifetimeScope BeginLifetimeScope(ScopeTag tag = ScopeTag.NewTag)
        {
            return BeginLifetimeScope(new object());
        }


#if NETCOREAPP3_0 || NET8_0
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ContainerBuilder Initialize(IServiceCollection services, bool doBuild = true, ScopeTag tag = ScopeTag.None, bool onlySafeAssembly = true)
        {
            SetDefaultScope(tag);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            RegisterDependencies(builder, onlySafeAssembly);

            if (doBuild)
                builder.Build();

            return builder;
        }

        /// <summary>
        /// initialize & Register dependencies.
        /// </summary>
        /// <returns>return a ContainerBuilder, then you need to Build() the ContainerBuilder</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ContainerBuilder Initialize(ContainerBuilder builder, ScopeTag tag = ScopeTag.None, bool onlySafeAssembly = true)
        {
            SetDefaultScope(tag);

            RegisterDependencies(builder, onlySafeAssembly);

            return builder;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ContainerBuilder Initialize(bool doBuild = true, ScopeTag tag = ScopeTag.None, bool onlySafeAssembly = true)
        {
            SetDefaultScope(tag);

            var builder = RegisterDependencies(null, onlySafeAssembly);

            if (doBuild)
                builder.Build();

            return builder;
        }
#endif
        #endregion
    }
}