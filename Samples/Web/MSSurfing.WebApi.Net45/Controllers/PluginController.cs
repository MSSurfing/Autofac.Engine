using Autofac.Engine;
using MSSurfing.Sdk.Plugins;
using MSSurfing.WebApi.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MSSurfing.WebApi.Controllers
{
    [RoutePrefix("api/Plugin")]
    public class PluginController : BaseApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> LoadAllPlugins()
        {
            string fullname = "MicroServices.Alipay.Plugins.AlipayPlugin, MicroServices.Alipay.Plugins";
            var type = Type.GetType(fullname);
            if (type == null)
            {
                var typeFinder = EngineContext.Resolve<ITypeFinder>();
                type = typeFinder.FindClassesOfType(typeof(IPlugin)).FirstOrDefault(e => e.FullName.Equals(fullname));

                if (type == null)
                    return ErrorMessage(Data: false);
            }

            IPlugin plugin = null;
            if (!EngineContext.TryResolve(type, out object instance))
                instance = EngineContext.ResolveUnregistered(type);

            plugin = instance as IPlugin;
            if (plugin == null)
                return ErrorMessage(Data: false);

            var executedResult = plugin.Execute();
            return SuccessMessage(executedResult);
        }
    }
}
