using System.Web.Http.Controllers;
using log4net;

namespace MVC4ServicesBook.Web.Common
{
    public class ActionLogHelper : IActionLogHelper
    {
        public const string EnteringText = "ENTERING";
        public const string ExitingText = "EXITING";

        private readonly ILog _logger;

        public ActionLogHelper(ILog logger)
        {
            _logger = logger;
        }

        public void LogEntry(HttpActionDescriptor actionDescriptor)
        {
            LogAction(actionDescriptor, EnteringText);
        }

        public void LogExit(HttpActionDescriptor actionDescriptor)
        {
            LogAction(actionDescriptor, ExitingText);
        }

        public void LogAction(HttpActionDescriptor actionDescriptor, string prefix)
        {
            _logger.DebugFormat(
                "{0} {1}::{2}",
                prefix,
                actionDescriptor.ControllerDescriptor.ControllerType.FullName,
                actionDescriptor.ActionName);
        }
    }
}