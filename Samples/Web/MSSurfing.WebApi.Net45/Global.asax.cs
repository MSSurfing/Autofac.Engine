using Autofac.Engine;
using Autofac.Integration.WebApi;
using System.Web.Http;

namespace MSSurfing.WebApi.Net45
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            EngineContext.Initialize(ScopeTag.Http);            //用于 Web / Api
            //System.Web.Mvc.DependencyResolver.SetResolver(new AutofacDependencyResolver(EngineContext.Scope));       //用于 Web Mvc, //PM> 需引入 Install-Package Autofac.Mvc5
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(EngineContext.Scope);        //用于Web Api,需引入 Install-Package Autofac.WebApi2
        }
    }
}
