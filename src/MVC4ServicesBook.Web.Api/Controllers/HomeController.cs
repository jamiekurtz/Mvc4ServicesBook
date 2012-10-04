using System.Web.Mvc;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
