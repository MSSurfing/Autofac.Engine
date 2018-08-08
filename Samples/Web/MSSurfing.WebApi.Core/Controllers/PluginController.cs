using System;
using System.Linq;
using Autofac.Engine;
using Microsoft.AspNetCore.Mvc;
using MSSurfing.Sdk.Plugins;

namespace MSSurfing.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Plugin")]
    public class PluginController : BasePublicController
    {
        public JsonResult LoadAllPlugins()
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