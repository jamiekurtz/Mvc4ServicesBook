using NHibernate;

namespace MVC4ServicesBook.Web.Common
{
    public interface ICurrentSessionContextAdapter
    {
        bool HasBind(ISessionFactory sessionFactory);
        ISession Unbind(ISessionFactory sessionFactory);
    }
}