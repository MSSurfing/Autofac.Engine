using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IoCServiceProvider
{
    public class AppDomainTypeFinder : ITypeFinder
    {
        #region Cotr
        public AppDomainTypeFinder() { }
        #endregion

        #region Utilities
        protected virtual string GetBinDirectory()
        {
            return System.AppContext.BaseDirectory;
        }

        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var baseInterfaceType in type.FindInterfaces((t, f) => true, null))
                {
                    if (!baseInterfaceType.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(baseInterfaceType.GetGenericTypeDefinition());
                    return isMatch;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public virtual IEnumerable<Assembly> GetAssemblies(string ignoreAssemblyPattern = ITypeFinder.IGNORE_ASSEMBLY_PATTERN)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(e => e.FullName != null && !Regex.IsMatch(e.FullName, ignoreAssemblyPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled));
        }

        public virtual IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true, string ignoreAssemblyPattern = ITypeFinder.IGNORE_ASSEMBLY_PATTERN)
        {
            return FindClassesOfType(typeof(T), onlyConcreteClasses, ignoreAssemblyPattern);
        }

        public virtual IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true, string ignoreAssemblyPattern = ITypeFinder.IGNORE_ASSEMBLY_PATTERN)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(ignoreAssemblyPattern), onlyConcreteClasses);
        }

        public virtual IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }

        public virtual IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    Type[]? types = null;
                    try { types = a.GetTypes(); } catch { }

                    if (types != null)
                    {
                        foreach (var t in types)
                        {
                            if (!assignTypeFrom.IsAssignableFrom(t) && (!assignTypeFrom.IsGenericTypeDefinition || !DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                                continue;

                            if (t.IsInterface)
                                continue;

                            if (onlyConcreteClasses)
                            {
                                if (t.IsClass && !t.IsAbstract)
                                {
                                    result.Add(t);
                                }
                            }
                            else
                            {
                                result.Add(t);
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }
            return result;
        }
    }
}
