using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Autofac.Engine
{
    /// <summary>
    /// 扩展 ContainerBuilder 实现Assembly 的匹配注册(ToImprove)
    /// </summary>
    public static class RegistrationExtensions
    {
        /// <summary>
        /// 匹配注册
        /// </summary>
        /// <remarks>
        /// （注：RegisterAssemblyTypes().AsImplementedInterfaces().InstancePerLifetimeScope()）
        /// （注：不包含泛型实例匹配注册[ignored GenericType]）
        /// </remarks>
        /// <example>
        /// 例[e.g]：
        ///     containerBuidler.RegisterMapper(typeFinder,"MSSurfing.Service.dll","Service"，则 扫描该dll中所有 以Service结尾的实例进行注册[不含泛型]。
        /// </example>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">ITypeFinder</param>
        /// <param name="assembleFullname">程序集全称，含.dll（如：MSSurfing.WebApi.dll）</param>
        /// <param name="typeEndNames">匹配类名的后缀</param>
        public static void RegisterTypeMapper(this ContainerBuilder builder, ITypeFinder typeFinder, string assembleFullname, params string[] typeEndNames)
        {
            Assembly[] assemblies;
            if (string.IsNullOrEmpty(assembleFullname))
                assemblies = typeFinder.GetAssemblies().ToArray();
            else
                assemblies = typeFinder.GetAssemblies().Where(a => a.ManifestModule.Name.Equals(assembleFullname, StringComparison.OrdinalIgnoreCase)).ToArray();

            builder.RegisterTypeMapper(assemblies, typeEndNames);
        }

        /// <summary>
        /// 匹配注册
        /// </summary>
        /// <remarks>
        /// （注：RegisterAssemblyTypes().AsImplementedInterfaces().InstancePerLifetimeScope()）
        /// （注：不包含泛型实例匹配注册[ignored GenericType]）
        /// </remarks>
        /// <example>
        /// 例[e.g]：
        ///     containerBuidler.RegisterMapper(typeFinder,assemblies,"Controller"，则 扫描该 程序集中 所有 以Controller结尾的实例进行注册[不含泛型]。
        /// </example>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="assemblies">扫描的程序集</param>
        /// <param name="typeEndNames">匹配类名的后缀（实例名不是接口名[Implemented type name,not interface type name）</param>
        public static void RegisterTypeMapper(this ContainerBuilder builder, Assembly[] assemblies, params string[] typeEndNames)
        {
            if (assemblies == null || assemblies.Length == 0)
                return;

            for (int i = 0; i < typeEndNames.Length; i++)
            {
                var endsName = typeEndNames[i];
                builder.RegisterAssemblyTypes(assemblies)
                        .Where(c => c.Name.EndsWith(endsName, StringComparison.OrdinalIgnoreCase))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
            }
        }

        //Todo RegisterMapper 
        /* 
        public static void RegisterMapper(this ContainerBuilder builder, ITypeFinder typeFinder, string assembleFullname, params string[] typeEndNames)
        {
            Assembly[] assemblies;
            if (string.IsNullOrEmpty(assembleFullname))
                assemblies = typeFinder.GetAssemblies().ToArray();
            else
                assemblies = typeFinder.GetAssemblies().Where(a => a.ManifestModule.Name.Equals(assembleFullname, StringComparison.OrdinalIgnoreCase)).ToArray();

            builder.RegisterMapper(assemblies, typeEndNames);
        }
        public static void RegisterMapper(this ContainerBuilder builder, Assembly[] assemblies, params string[] typeEndNames)
        {
            if (assemblies == null || assemblies.Length == 0)
                return;

            for (int i = 0; i < typeEndNames.Length; i++)
            {
                var endsName = typeEndNames[i];
                builder.RegisterAssemblyTypes(assemblies)
                        .Where(c => c.Name.EndsWith(endsName, StringComparison.OrdinalIgnoreCase))
                        .AsSelf()
                        .InstancePerLifetimeScope();
            }
        }
        */
        //Todo RegisterGenericMapper
    }
}
