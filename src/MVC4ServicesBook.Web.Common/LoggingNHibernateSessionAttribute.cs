using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MVC4ServicesBook.Web.Common
{
    public class LoggingNHibernateSessionAttribute : ActionFilterAttribute
    {
        private readonly IActionLogHelper _actionLogHelper;
        private readonly IActionExceptionHandler _actionExceptionHandler;
        private readonly IActionTransactionHelper _actionTransactionHelper;

        public LoggingNHibernateSessionAttribute()
            : this(WebContainerManager.Get<IActionLogHelper>(),
            WebContainerManager.Get<IActionExceptionHandler>(),
            WebContainerManager.Get<IActionTransactionHelper>())
        {
        }

        public LoggingNHibernateSessionAttribute(
            IActionLogHelper actionLogHelper, 
            IActionExceptionHandler actionExceptionHandler,
            IActionTransactionHelper actionTransactionHelper)
        {
            _actionLogHelper = actionLogHelper;
            _actionExceptionHandler = actionExceptionHandler;
            _actionTransactionHelper = actionTransactionHelper;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            _actionLogHelper.LogEntry(actionContext.ActionDescriptor);
            _actionTransactionHelper.BeginTransaction();
        }
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _actionTransactionHelper.EndTransaction(actionExecutedContext);
            _actionTransactionHelper.CloseSession();
            _actionExceptionHandler.HandleException(actionExecutedContext);
            _actionLogHelper.LogExit(actionExecutedContext.ActionContext.ActionDescriptor);
        }
    }
}
