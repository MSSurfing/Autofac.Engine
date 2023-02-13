using System.Reflection;

namespace IoCServiceProvider
{
    public interface ITypeFinder
    {
        const string IGNORE_ASSEMBLY_PATTERN = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

        IEnumerable<Assembly> GetAssemblies(string ignoreAssemblyPattern = IGNORE_ASSEMBLY_PATTERN);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true, string ignoreAssemblyPattern = IGNORE_ASSEMBLY_PATTERN);

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true, string ignoreAssemblyPattern = IGNORE_ASSEMBLY_PATTERN);

        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}
