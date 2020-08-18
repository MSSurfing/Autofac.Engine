using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Autofac.Engine
{
    /// <summary>
    /// 类型查询器
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 获取/加载 所有程序集
        /// </summary>
        /// <returns></returns>
        IList<Assembly> GetAssemblies();

        /// <summary>
        /// 查找 指定类型的类。可获取指定接口的所有实现类，如：typeFinder.FindClassesOfType&lt;IDependencyRegistrar>()&gt;，将获取到程序中所有实现了 IDependencyRegistrar的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyConcreteClasses">仅具体类（不含抽象类）</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找 指定类型的类。可获取指定接口的所有实现类，如：typeFinder.FindClassesOfType&lt;IDependencyRegistrar>()&gt;，将获取到程序中所有实现了 IDependencyRegistrar的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyConcreteClasses">仅具体类（不含抽象类）</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找 指定类型的类。可获取指定接口的所有实现类，如：typeFinder.FindClassesOfType&lt;IDependencyRegistrar>()&gt;，将获取到 assemblies 中所有实现了 IDependencyRegistrar的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyConcreteClasses">仅具体类（不含抽象类）</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找 指定类型的类。可获取指定接口的所有实现类，如：typeFinder.FindClassesOfType&lt;IDependencyRegistrar>()&gt;，将获取到 assemblies 中所有实现了 IDependencyRegistrar的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyConcreteClasses">仅具体类（不含抽象类）</param>
        /// <returns></returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);
    }
}
