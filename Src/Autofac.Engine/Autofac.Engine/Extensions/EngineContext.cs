using Autofac.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Autofac.Engine
{
    public partial class EngineContext
    {
        #region ServiceProvider Resolve
        public static T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }
        public static object Resolve(Type type)
        {
#if NET45
            return Scope.Resolve(type);
#else
            return ServiceProvider.GetRequiredService(type);
#endif
        }
        public static IEnumerable<T> ResolveAll<T>()
        {
#if NET45
            return Scope.Resolve<IEnumerable<T>>();
#else
            return ServiceProvider.GetService<IEnumerable<T>>();
#endif
        }
        #endregion

        #region Scope Resolve
        public static T Resolve<T>(string key)
        {
            return Scope.ResolveKeyed<T>(key);
        }
        public static IEnumerable<T> ResolveAll<T>(string key)
        {
            return Scope.ResolveKeyed<IEnumerable<T>>(key);
        }

        public static bool TryResolve<T>(out T instance)
        {
            return Scope.TryResolve<T>(out instance);
        }

        public static bool TryResolve(Type serviceType, out object instance)
        {
            return Scope.TryResolve(serviceType, out instance);
        }
        #endregion

        #region Unregistered Resolve
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