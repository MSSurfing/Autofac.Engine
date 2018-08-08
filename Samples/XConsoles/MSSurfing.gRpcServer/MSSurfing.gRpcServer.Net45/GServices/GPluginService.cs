using Autofac.Engine;
using MSSurfing.gRpcServer.Net45.Services.Base;
using MSSurfing.Sdk.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.gRpcServer.Net45.GServices
{
    public class GPluginService : GBaseService
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
    }
}
