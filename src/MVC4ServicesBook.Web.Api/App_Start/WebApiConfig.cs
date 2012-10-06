using System.Web.Http;

namespace MVC4ServicesBook.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});

            config.Routes.MapHttpRoute(
                name: "TaskStatysApiRoute",
                routeTemplate: "api/tasks/{taskId}/status/{statusId}",
                defaults: new { controller = "TaskStatus", statusId = RouteParameter.Optional }
            );
        }
    }
}
