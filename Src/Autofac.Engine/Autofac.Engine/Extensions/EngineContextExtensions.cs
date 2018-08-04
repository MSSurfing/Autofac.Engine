using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autofac.Engine
{
    public partial class EngineContext
    {
        #region Resolve Methods
        public static T Resolve<T>() where T : class
        {
            return Scope.Resolve<T>();
        }
        public static T Resolve<T>(string key) where T : class
        {
            return Scope.ResolveKeyed<T>(key);
        }
        public static object Resolve(Type type)
        {
            return Scope.Resolve(type);
        }
        public static T[] ResolveAll<T>()
        {
            return Scope.Resolve<IEnumerable<T>>().ToArray();
        }
        public static T[] ResolveAll<T>(string key)
        {
            return Scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }
        public static bool TryResolve<T>(out T instance)
        {
            return Scope.TryResolve<T>(out instance);
        }
        public static bool TryResolve(Type serviceType, out object instance)
        {
            return Scope.TryResolve(serviceType, out instance);
        }
        public static T ResolveUnregistered<T>() where T : class
        {
            return Scope.ResolveUnregistered(typeof(T)) as T;
        }
        public static object ResolveUnregistered(Type type)
        {
            return Scope.ResolveUnregistered(type);
        }
        #endregion
    }
}
