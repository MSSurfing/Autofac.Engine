using Autofac.Core;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autofac.Engine
{
    public static class LifetimeScopeExtensions
    {
        public static T Resolve<T>(this ILifetimeScope scope) where T : class
        {
            return (T)scope.Resolve(typeof(T));
        }

        public static IEnumerable<T> ResolveAll<T>(this ILifetimeScope scope)
        {
            return scope.Resolve<IEnumerable<T>>();
        }

        public static T Resolve<T>(this ILifetimeScope scope, string key)
        {
            return scope.ResolveKeyed<T>(key);
        }

        public static IEnumerable<T> ResolveAll<T>(this ILifetimeScope scope, string key)
        {
            return scope.ResolveKeyed<IEnumerable<T>>(key);
        }

        public static bool TryResolve<T>(this ILifetimeScope scope, out T instance)
        {
            var context = (IComponentContext)scope;
            context.TryResolve(typeof(T), out var x);

            if (x != null && x is T)
            {
                instance = (T)x;
                return true;
            }

            instance = default(T);
            return false;
        }

        //public static bool TryResolve(this ILifetimeScope scope, Type serviceType, out object instance)
        //{
        //    return scope.TryResolve()
        //}

        public static object ResolveUnregistered(this ILifetimeScope scope, Type type)
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
                        var service = scope.Resolve(param.ParameterType);
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

        public static bool TryResolveUnregistered(this ILifetimeScope scope, Type type, out object instance)
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
                        var service = scope.Resolve(param.ParameterType);
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
    }
}
