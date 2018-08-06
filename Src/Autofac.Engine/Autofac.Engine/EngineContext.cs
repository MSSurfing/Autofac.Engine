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

#if NETSTANDARD2_0
        private static IServiceProvider _serviceProvider { get; set; }
        public static IServiceProvider ServiceProvider => _serviceProvider;
#endif
        #endregion

        #region Utilities
        protected static ILifetimeScope DefaultScope(IContainer container)
        {
            return container.BeginLifetimeScope();
        }

        protected static void SetScope(string tag)
        {
            _scope = (e) => e.BeginLifetimeScope(tag);
        }
        protected static void SetScope(ScopeTag tag)
        {
            switch (tag)
            {
                case ScopeTag.Http:
                    _scope = (e) => e.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
                    break;
                default:
                    _scope = DefaultScope;
                    break;
            }
        }

#if NETSTANDARD2_0
        private static IServiceProvider GetServiceProvider()
        {
            if (_serviceProvider == null)
                _serviceProvider = new AutofacServiceProvider(_container);

            //ToImprove  Microsoft.AspNetCore.Http.HttpContext.IServiceProvider
            return _serviceProvider;
        }

        protected static IContainer RegisterDependencies(IServiceCollection services = null)
        {
            var containerBuilder = new ContainerBuilder();

            var typeFinder = new DomainTypeFinder();
            containerBuilder.RegisterInstance<ITypeFinder>(typeFinder).As<ITypeFinder>().SingleInstance();

            var dependencyTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var dependencyInstances = new List<IDependencyRegistrar>();
            foreach (var dependency in dependencyTypes)
            {
                dependencyInstances.Add((IDependencyRegistrar)Activator.CreateInstance(dependency));
            }


            dependencyInstances = dependencyInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in dependencyInstances)
                dependencyRegistrar.Register(containerBuilder, typeFinder);

            if (services != null)
                containerBuilder.Populate(services);


            _container = containerBuilder.Build();
            return _container;
        }
#endif

#if NET45
        protected static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            var typeFinder = new DomainTypeFinder();
            builder.RegisterInstance<ITypeFinder>(typeFinder).As<ITypeFinder>().SingleInstance();

            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));

            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder);

            _container = builder.Build();
        }
#endif
        #endregion

        #region Methods

#if NETSTANDARD2_0
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IServiceProvider Initialize(IServiceCollection services = null, ScopeTag tag = ScopeTag.None)
        {
            if (services != null)
                services.BuildServiceProvider();

            SetScope(tag);
            RegisterDependencies(services);
            return GetServiceProvider();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IServiceProvider Initialize(string tag)
        {
            return Initialize(null, tag);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IServiceProvider Initialize(IServiceCollection services, string tag)
        {
            if (services != null)
                services.BuildServiceProvider();

            SetScope(tag);
            RegisterDependencies(services);
            return GetServiceProvider();
        }
#endif

#if NET45
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IContainer Initialize(ScopeTag tag = ScopeTag.None)
        {
            RegisterDependencies();
            SetScope(tag);
            return _container;
        }

        [Obsolete("已不支持forceRecreate，可以使用 Initialize(ScopeTag)", false)]
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IContainer Initialize(bool forceRecreate, ScopeTag tag = ScopeTag.None)
        {
            return Initialize(tag);
        }
#endif
        #endregion
    }
}