using System.Web.Mvc;

namespace MVC4ServicesBook.Web.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new LoggingNHibernateSessionAttribute());
        }
    }
}