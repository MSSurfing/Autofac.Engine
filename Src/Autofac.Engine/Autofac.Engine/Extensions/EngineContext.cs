using Autofac.Core;
using Autofac.Core.Registration;
using System;
using System.Collections.Generic;

namespace Autofac.Engine
{
    /// <summary>
    /// EngineContext
    /// </summary>
    public partial class EngineContext
    {
        #region ServiceProvider Resolve
        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        /// <typeparam name="T">The type to which the result will be cast.</typeparam>
        /// <returns>The component instance that provides the service.</returns>
        /// <exception cref="ComponentNotRegisteredException"/>
        /// <exception cref="DependencyResolutionException"/>
        public static T Resolve<T>() where T : class
        {
            return Scope.Resolve<T>();
        }

        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        /// <returns>The component instance that provides the service.</returns>
        /// <exception cref="ComponentNotRegisteredException"/>
        /// <exception cref="DependencyResolutionException"/>
        public static object Resolve(Type type)
        {
#if NET45
            return Scope.Resolve(type);
#else
            return Scope.Resolve(type);
#endif
        }

        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        /// <typeparam name="T">The type to which the result will be cast.</typeparam>
        /// <returns>The component instance that provides the service.</returns>
        /// <exception cref="ComponentNotRegisteredException"/>
        /// <exception cref="DependencyResolutionException"/>
        public static IEnumerable<T> ResolveAll<T>()
        {
#if NET45
            return Scope.Resolve<IEnumerable<T>>();
#else
            return Scope.Resolve<IEnumerable<T>>();
#endif
        }
        #endregion

        #region Scope Resolve
        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        /// <typeparam name="T">The type to which the result will be cast.</typeparam>
        /// <param name="key">Key of the service.</param>
        /// <returns>The component instance that provides the service.</returns>
        /// <exception cref="ComponentNotRegisteredException"/>
        /// <exception cref="DependencyResolutionException"/>
        public static T Resolve<T>(string key)
        {
            return Scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// Retrieve a service from the context.
        /// </summary>
        /// <typeparam name="T">The type to which the result will be cast.</typeparam>
        /// <param name="key">Key of the service.</param>
        /// <returns>The component instance that provides the service.</returns>
        /// <exception cref="ComponentNotRegisteredException"/>
        /// <exception cref="DependencyResolutionException"/>
        public static IEnumerable<T> ResolveAll<T>(string key)
        {
            return Scope.ResolveKeyed<IEnumerable<T>>(key);
        }

        /// <summary>
        /// Try to retrieve a service from the context.
        /// </summary>
        /// <typeparam name="T">The service type to resolve.</typeparam>
        /// <param name="instance">The resulting component instance providing the service, or default(T).</param>
        /// <returns>True if a component providing the service is available.</returns>
        /// <exception cref="DependencyResolutionException"/>
        public static bool TryResolve<T>(out T instance)
        {
            return Scope.TryResolve<T>(out instance);
        }

        /// <summary>
        /// Try to retrieve a service from the context.
        /// </summary>
        /// <param name="serviceType">The service type to resolve.</param>
        /// <param name="instance"> resulting component instance providing the service, or null.</param>
        /// <returns>True if a component providing the service is available.</returns>
        /// <exception cref="DependencyResolutionException"/>
        public static bool TryResolve(Type serviceType, out object instance)
        {
            return Scope.TryResolve(serviceType, out instance);
        }
        #endregion

        #region Unregistered Resolve
        /// <summary>
        /// Try to retrieve an unregistered service from the context.
        /// </summary>
        /// <param name="type">The unregistered service type to resolve.</param>
        /// <returns>The component instance that provides the service.</returns>
        /// <exception cref="DependencyResolutionException"/>
        public static object ResolveUnregistered(Type type)
        {
            foreach (var ctor in type.GetConstructors())
            {
                try
                {
                    var parameters = ctor.GetParameters();
                    var instances = new object[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        var service = Resolve(param.ParameterType);
                        if (service == null)
                            throw new Exception();

                        instances[i] = service;
                    }
                    return Activator.CreateInstance(type, instances);
                }
                catch { }
            }
            throw new DependencyResolutionException("None of the constructors found!");
        }

        /// <summary>
        /// Try to retrieve an unregistered service from the context.
        /// </summary>
        /// <param name="type">The unregistered service type to resolve.</param>
        /// <param name="instance"> resulting component instance providing the service, or null.</param>
        /// <returns>True if a component providing the service is available.</returns>
        /// <exception cref="DependencyResolutionException"/>
        public static bool TryResolveUnregistered(Type type, out object instance)
        {
            foreach (var ctor in type.GetConstructors())
            {
                try
                {
                    var parameters = ctor.GetParameters();
                    var instances = new object[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        var service = Resolve(param.ParameterType);
                        if (service == null)
                            throw new Exception();

                        instances[i] = service;
                    }
                    instance = Activator.CreateInstance(type, instances);
                    return true;
                }
                catch { }
            }
            instance = null;
            return false;
        }
        #endregion

    }
}