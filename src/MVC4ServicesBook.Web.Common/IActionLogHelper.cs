using System.Web.Http.Controllers;

namespace MVC4ServicesBook.Web.Common
{
    public interface IActionLogHelper
    {
        void LogEntry(HttpActionDescriptor actionDescriptor);
        void LogExit(HttpActionDescriptor actionDescriptor);
        void LogAction(HttpActionDescriptor actionDescriptor, string prefix);
    }
}