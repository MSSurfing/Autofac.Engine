using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Autofac.Engine
{
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 匹配注册
        /// （例：containerBuidler.RegisterMapper(typeFinder,"MSSurfing.Service.dll","Service"，则 扫描该dll中所有 以Service结尾的类，以接口实现的方式注册。）
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="typeFinder">ITypeFinder</param>
        /// <param name="assembleFullname">程序集全称，含.dll（如：MSSurfing.WebApi.dll）</param>
        /// <param name="endsWithTypeNames">匹配类名的后缀</param>
        public static void RegisterMapper(this ContainerBuilder builder, ITypeFinder typeFinder, string assembleFullname, params string[] endsWithTypeNames)
        {
            Assembly[] assemblies;
            if (string.IsNullOrEmpty(assembleFullname))
                assemblies = typeFinder.GetAssemblies().ToArray();
            else
                assemblies = typeFinder.GetAssemblies().Where(a => a.ManifestModule.Name.Equals(assembleFullname, StringComparison.OrdinalIgnoreCase)).ToArray();

            builder.RegisterMapper(assemblies, endsWithTypeNames);
        }

        /// <summary>
        /// 匹配注册
        /// （例：containerBuidler.RegisterMapper(typeFinder,assemblies,"Controller"，则 扫描该 程序集中 所有 以Controller结尾的类，以接口实现的方式注册。）
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        /// <param name="assemblies">扫描的程序集</param>
        /// <param name="endsWithTypeNames">匹配类名的后缀</param>
        public static void RegisterMapper(this ContainerBuilder builder, Assembly[] assemblies, params string[] endsWithTypeNames)
        {
            if (assemblies == null || assemblies.Length == 0)
                return;

            for (int i = 0; i < endsWithTypeNames.Length; i++)
            {
                var endsName = endsWithTypeNames[i];
                builder.RegisterAssemblyTypes(assemblies)
                        .Where(c => c.Name.EndsWith(endsName))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
            }
        }
    }
}
