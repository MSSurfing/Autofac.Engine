using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Autofac.Builder;
using Autofac.Features.Scanning;
using System.IO;

namespace Autofac.Engine
{
    /// <summary>
    /// 扩展 ContainerBuilder (实现Assembly 的匹配注册)
    /// </summary>
    public static class RegistrationExtensions
    {
        #region RegisterAssemblyTypes Extensions
        /// <summary>
        /// 匹配注册
        /// </summary>
        /// <example>
        /// 例[e.g]：
        ///     containerBuidler.RegisterAssemblyTypes(typeFinder,"Service").AsImplementedInterfaces();
        /// </example>
        /// <remarks>
        /// main code : {  builder.RegisterAssemblyTypes(typeFinder.GetAssemblies(...)).Where(e => e.Name.EndsWith(typeEndName)); }
        /// </remarks>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">扫描的程序集</param>
        /// <param name="typeEndName">匹配类名的后缀（是实例名，不是接口名[name of Implemented type,Is not the name of the interface]）</param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyTypes(this ContainerBuilder builder, ITypeFinder typeFinder, string typeEndName)
        {
            Assembly[] assemblies = typeFinder.GetAssemblies().ToArray();
            return builder.RegisterAssemblyTypes(assemblies, typeEndName);
        }

        /// <summary>
        /// 匹配注册
        /// </summary>
        /// <example>
        /// 例[e.g]：
        ///     containerBuidler.RegisterAssemblyTypes(typeFinder,"Service").AsImplementedInterfaces();
        /// </example>
        /// <remarks>
        /// main code : {  builder.RegisterAssemblyTypes(typeFinder.GetAssemblies(...)).Where(e => e.Name.EndsWith(typeEndName)); }
        /// </remarks>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">扫描的程序集</param>
        /// <param name="assembleFullname">程序集全称，含.dll（如：MSSurfing.WebApi.dll）</param>
        /// <param name="typeEndName">匹配类名的后缀（是实例名，不是接口名[name of Implemented type,Is not the name of the interface]）</param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyTypes(this ContainerBuilder builder, Assembly[] assemblies, string typeEndName)
        {
            return builder.RegisterAssemblyTypes(assemblies).Where(e => e.Name.EndsWith(typeEndName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 匹配注册
        /// </summary>
        /// <example>
        /// 例[e.g]：
        ///     containerBuidler.RegisterAssemblyTypes(typeFinder,"MSSurfing.Service.dll","Service").AsImplementedInterfaces();
        /// </example>
        /// <remarks>
        /// main code : {  builder.RegisterAssemblyTypes(typeFinder.GetAssemblies(...)).Where(e => e.Name.EndsWith(typeEndName)); }
        /// </remarks>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">扫描的程序集</param>
        /// <param name="assembleFullname">程序集全称，含.dll（如：MSSurfing.WebApi.dll）</param>
        /// <param name="typeEndName">匹配类名的后缀（是实例名，不是接口名[name of Implemented type,Is not the name of the interface]）</param>
        /// <returns></returns>
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyTypes(this ContainerBuilder builder, ITypeFinder typeFinder, string assembleFullname, string typeEndName)
        {
            Assembly[] assemblies = typeFinder.GetAssemblies().Where(e => e.ManifestModule.Name.Equals(assembleFullname, StringComparison.OrdinalIgnoreCase)).ToArray();
            return builder.RegisterAssemblyTypes(assemblies, typeEndName);
        }
        #endregion

        #region Register Instance Extensions
        /// <summary>
        /// 注册委托实例（实例 可更新/重新注册）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="instance"></param>
        /// <param name="useUpdateInstance">是否可被更新。如果为是，可通过<see cref="EngineContext.UpdateInstance{T}(T)"/>方法更新该实例值。
        /// 注：可被更新时，不能带上 .SingleInstance();</param>
        /// <returns></returns>
        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(this ContainerBuilder builder, T instance, bool useUpdateInstance) where T : class
        {
            if (useUpdateInstance)
                builder.Register<Action<T>>(c => t => instance = t);

            return builder.Register(c => instance);
        }
        #endregion
    }
}
