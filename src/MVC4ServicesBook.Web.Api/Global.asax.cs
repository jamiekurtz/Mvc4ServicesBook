using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MVC4ServicesBook.Web.Api.App_Start;
using Ninject;

namespace MVC4ServicesBook.Web.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            var container = new StandardKernel();
            var containerConfigurator = new NinjectConfigurator();
            containerConfigurator.Configure(container);


            GlobalConfiguration.Configuration.MessageHandlers.Add(container.Get<BasicAuthorizationMessageHandler>());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}