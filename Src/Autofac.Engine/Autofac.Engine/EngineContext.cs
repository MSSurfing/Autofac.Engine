using Autofac.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Autofac.Engine
{
    public partial class EngineContext
    {
        #region Fields
        private static Func<IContainer, LifetimeScope> _scope;
        private static IContainer _container;

        public static LifetimeScope Scope
        {
            get
            {
                return _scope.Invoke(_container);
            }
        }
        #endregion

        #region Scope Utilities
        protected static LifetimeScope DefaultScope(IContainer container)
        {
            return container as LifetimeScope;
        }
        protected static void SetScope(ScopeTag tag, Func<IContainer, LifetimeScope> providerScope = null)
        {
            switch (tag)
            {
                case ScopeTag.Http:
                    _scope = (e) => { return e.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag) as LifetimeScope; };
                    break;
                case ScopeTag.Provider:
                    _scope = providerScope ?? DefaultScope;
                    break;
                default:
                    _scope = DefaultScope;
                    break;
            }
        }
        #endregion

        #region Utilities
        protected static IContainer BuildContainer(Action<ContainerBuilder> populate = null)
        {
            //var builder = new ContainerBuilder();
            //var container = builder.Build();

            var typeFinder = new DomainTypeFinder();
            var builder = new ContainerBuilder();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();

            //builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));

            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder);

            populate?.Invoke(builder);
            _container = builder.Build();
            return _container;
        }
        #endregion

        #region Methods
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IContainer Initialize(bool forceRecreate, Action<ContainerBuilder> populate = null, ScopeTag tag = ScopeTag.None, Func<IContainer, LifetimeScope> providerScope = null)
        {
            if (_container == null || forceRecreate)
            {
                BuildContainer(populate);
            }
            SetScope(tag, providerScope);
            return _container;
        }
        #endregion
    }
}
