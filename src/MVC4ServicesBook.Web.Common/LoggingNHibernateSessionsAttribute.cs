using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NHibernate;
using NHibernate.Context;
using Ninject;
using log4net;

namespace MVC4ServicesBook.Web.Common
{
    public class LoggingNHibernateSessionsAttribute : ActionFilterAttribute
    {
        public const int MaxStatusDescriptionLength = 512;

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            LogAction(actionContext.ActionDescriptor, "ENTERING  ");
            BeginTransaction();
        }
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            EndTransaction(actionExecutedContext);
            CloseSession();
            LogException(actionExecutedContext);
            LogAction(actionExecutedContext.ActionContext.ActionDescriptor, "EXITING  ");
        }

        private void CloseSession()
        {
            var container = GetContainer();
            var sessionFactory = container.Get<ISessionFactory>();
            if (CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.GetCurrentSession();
                session.Close();
                session.Dispose();
                CurrentSessionContext.Unbind(sessionFactory);
            }
        }

        private void LogAction(HttpActionDescriptor actionDescriptor, string prefix)
        {
            var container = GetContainer();
            var logger = container.Get<ILog>();
            logger.DebugFormat(
                "{0}{1}::{2}",
                prefix,
                actionDescriptor.ControllerDescriptor.ControllerType.FullName,
                actionDescriptor.ActionName);
        }

        private void LogException(HttpActionExecutedContext filterContext)
        {
            var exception = filterContext.Exception;
            if (exception == null) return;

            var container = GetContainer();
            var logger = container.Get<ILog>();

            logger.Error("Exception occured:", exception);

            var reasonPhrase = GetExceptionMessage(exception);
            if (reasonPhrase.Length > MaxStatusDescriptionLength)
            {
                reasonPhrase = reasonPhrase.Substring(0, MaxStatusDescriptionLength);
            }

            reasonPhrase = reasonPhrase.Replace("\r\n", " ");

            filterContext.Response = new HttpResponseMessage
                                         {
                                             StatusCode = HttpStatusCode.InternalServerError,
                                             ReasonPhrase = reasonPhrase
                                         };
        }

        private string GetExceptionMessage(Exception ex)
        {
            var message = ex.Message;
            var innerException = ex.InnerException;
            while (innerException != null)
            {
                message += " --> " + innerException.Message;
                innerException = innerException.InnerException;
            }

            return message;
        }

        public void BeginTransaction()
        {
            var session = GetCurrentSession();
            if (session != null)
            {
                session.BeginTransaction();
            }
        }

        public void EndTransaction(HttpActionExecutedContext filterContext)
        {
            var session = GetCurrentSession();
            if (session != null)
            {
                if (session.Transaction.IsActive)
                {
                    if (filterContext.Exception == null)
                    {
                        session.Flush();
                        session.Transaction.Commit();
                    }
                    else
                    {
                        session.Transaction.Rollback();
                    }
                }
            }
        }

        private ISession GetCurrentSession()
        {
            //var container = GetContainer();
            //var session = container.Get<ISession>();
            //return session;

            var container = GetContainer();
            var sessionFactory = container.Get<ISessionFactory>();
            var session = sessionFactory.GetCurrentSession();
            return session;
        }

        private IKernel GetContainer()
        {
            var resolver = GlobalConfiguration.Configuration.DependencyResolver as NinjectDependencyResolver;
            if (resolver != null)
            {
                return resolver.Container;
            }

            throw new InvalidOperationException("NinjectDependencyResolver not being used as the MVC controller resolver");
        }
    }
}
