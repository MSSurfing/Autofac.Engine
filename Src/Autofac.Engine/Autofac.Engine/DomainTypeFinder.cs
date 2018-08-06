using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Autofac.Engine
{
    public class DomainTypeFinder : ITypeFinder
    {
        #region Fields
        private const string IGNORE_ASSEMBLY_PATTERN = "^System|^mscorlib|^Microsoft";
        #endregion

        #region Properties

        public virtual AppDomain Domain
        {
            get { return AppDomain.CurrentDomain; }
        }

        #endregion

        #region Utilities
        protected virtual string GetBinDirectory()
        {
#if NET45
            return AppDomain.CurrentDomain.BaseDirectory;
#else
            return System.AppContext.BaseDirectory;
#endif
        }

        protected virtual bool TypeFilter(Type type, Object filterCriteria)
        {
            return true;
        }

        protected virtual bool IsMatch(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        protected virtual void LoadMatchingAssemblies(string directoryPath, List<string> ignoreAssemblyNames)
        {
            if (!Directory.Exists(directoryPath))
                return;

            if (ignoreAssemblyNames == null)
                ignoreAssemblyNames = new List<string>();

            foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);
                    if (!IsMatch(an.FullName, IGNORE_ASSEMBLY_PATTERN) && !ignoreAssemblyNames.Contains(an.FullName))
                        Domain.Load(an);
                }
                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        private void AddAssembliesInAppDomain(List<string> assemblyNames, List<Assembly> assemblies)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!IsMatch(assembly.FullName, IGNORE_ASSEMBLY_PATTERN))
                {
                    if (!assemblyNames.Contains(assembly.FullName))
                    {
                        assemblies.Add(assembly);
                        assemblyNames.Add(assembly.FullName);
                    }
                }
            }
        }

        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
                foreach (var baseInterfaceType in type.FindInterfaces(TypeFilter, null))
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

        #region Methods
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();
            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch { }
                    if (types != null)
                    {
                        foreach (var t in types)
                        {
                            if (assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                            {
                                if (!t.IsInterface)
                                {
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

        public virtual IList<Assembly> GetAssemblies(bool allInBinDirectory = true)
        {
            var assemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            AddAssembliesInAppDomain(assemblyNames, assemblies);
            if (allInBinDirectory)
            {
                var binPath = GetBinDirectory();
                LoadMatchingAssemblies(binPath, assemblyNames);
            }

            return assemblies;
        }

        #endregion

        #region Overload FindClassesOfType
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }
        #endregion
    }
}
