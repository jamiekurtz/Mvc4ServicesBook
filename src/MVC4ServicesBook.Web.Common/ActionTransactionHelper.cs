using System.Web.Http.Filters;
using NHibernate;
using NHibernate.Context;

namespace MVC4ServicesBook.Web.Common
{
    public class ActionTransactionHelper : IActionTransactionHelper
    {
        private readonly ISessionFactory _sessionFactory;

        public ActionTransactionHelper(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void BeginTransaction()
        {
            var session = _sessionFactory.GetCurrentSession();
            if (session != null)
            {
                session.BeginTransaction();
            }
        }

        public void EndTransaction(HttpActionExecutedContext filterContext)
        {
            var session = _sessionFactory.GetCurrentSession();

            if (session == null) return;
            if (!session.Transaction.IsActive) return;

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

        public void CloseSession()
        {
            var sessionFactory = WebContainerManager.Get<ISessionFactory>();
            if (CurrentSessionContext.HasBind(sessionFactory))
            {
                var session = sessionFactory.GetCurrentSession();
                session.Close();
                session.Dispose();
                CurrentSessionContext.Unbind(sessionFactory);
            }
        }
    }
}