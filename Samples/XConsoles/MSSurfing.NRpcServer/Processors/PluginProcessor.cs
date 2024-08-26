using Autofac.Engine;
using MSSurfing.NRpcServer.Core30.Domain.Configuration;
using MSSurfing.Sdk.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSurfing.NRpcServer.Core30.Processors
{
    public class PluginProcessor : BaseProcessor
    {
        public string LoadAllPlugins()
        {
            string fullname = "MicroServices.Alipay.Plugins.AlipayPlugin, MicroServices.Alipay.Plugins";
            var type = Type.GetType(fullname);
            if (type == null)
            {
                var typeFinder = EngineContext.Resolve<ITypeFinder>();
                type = typeFinder.FindClassesOfType(typeof(IPlugin)).FirstOrDefault(e => e.FullName.Equals(fullname));

                if (type == null)
                    return Json(false);
            }

            IPlugin plugin = null;
            if (!EngineContext.TryResolve(type, out object instance))
                instance = EngineContext.ResolveUnregistered(type);

            plugin = instance as IPlugin;
            if (plugin == null)
                return Json(false);

            var executedResult = plugin.Execute();
            return Json(executedResult);
        }

        public string GetVersion()
        {
            var config = EngineContext.Resolve<MSConfig>();

            return Json(config);
        }

        public string SetVersion(string version)
        {
            var config = new MSConfig() { Version = version };
            EngineContext.UpdateInstance(config);

            config = EngineContext.Resolve<MSConfig>();
            return Json(config);
        }
    }
}
