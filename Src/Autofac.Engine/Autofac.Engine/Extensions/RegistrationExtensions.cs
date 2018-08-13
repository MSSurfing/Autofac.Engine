using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace Autofac.Engine
{
    /// <summary>
    /// 扩展 ContainerBuilder (实现Assembly 的匹配注册)
    /// </summary>
    public static class RegistrationExtensions
    {
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
            Assembly[] assemblies;
            if (string.IsNullOrEmpty(assembleFullname))
                assemblies = typeFinder.GetAssemblies().ToArray();
            else
                assemblies = typeFinder.GetAssemblies().Where(e => e.ManifestModule.Name.Equals(assembleFullname, StringComparison.OrdinalIgnoreCase)).ToArray();

            return builder.RegisterAssemblyTypes(assemblies).Where(e => e.Name.EndsWith(typeEndName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
