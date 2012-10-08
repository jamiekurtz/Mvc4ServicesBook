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
                name: "TaskStatusApiRoute",
                routeTemplate: "api/tasks/{taskId}/status/{statusId}",
                defaults: new {controller = "TaskStatus", statusId = RouteParameter.Optional});

            config.Routes.MapHttpRoute(
                name: "TaskPriorityApiRoute",
                routeTemplate: "api/tasks/{taskId}/priority/{priorityId}",
                defaults: new {controller = "TaskPriority", priorityId = RouteParameter.Optional});

            config.Routes.MapHttpRoute(
                name: "TaskUsersApiRoute",
                routeTemplate: "api/tasks/{taskId}/users/{userId}",
                defaults: new {controller = "TaskUsers", userId = RouteParameter.Optional});
        }
    }
}
