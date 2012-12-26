using System.Web.Http.Filters;

namespace MVC4ServicesBook.Web.Common
{
    public interface IActionExceptionHandler
    {
        void HandleException(HttpActionExecutedContext filterContext);
    }
}