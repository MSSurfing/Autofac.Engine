using System;
using System.Linq;
using Autofac.Engine;
using Microsoft.AspNetCore.Mvc;
using MSSurfing.Sdk.Plugins;
using MSSurfing.WebApi.Core30.Configuration;

namespace MSSurfing.WebApi.Core30.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PluginController : BaseController
    {
        [HttpGet, Route("LoadAllPlugins")]
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
                instance = EngineContext.BeginLifetimeScope().ResolveUnregistered(type);

            plugin = instance as IPlugin;
            if (plugin == null)
                return Json(false);

            var executedResult = plugin.Execute();
            return Json(executedResult);
        }

        [HttpGet, Route("GetVersion")]
        public JsonResult GetVersion()
        {
            var config = EngineContext.Resolve<MSConfig>();

            return Json(config);
        }

        [HttpGet, Route("SetVersion")]
        public JsonResult SetVersion()
        {
            var config = new MSConfig() { Version = Guid.NewGuid().ToString() };
            EngineContext.UpdateInstance(config);

            config = EngineContext.Resolve<MSConfig>();
            return Json(config);
        }
    }
}