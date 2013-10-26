using System.Web.Http;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    public class AboutController : ApiController
    {
        [AllowAnonymous]
        public About Get()
        {
            return new About
            {
                Description =
                    "Example source code that accompanies my \"ASP.NET MVC 4 and the Web API: Building a REST Service from Start to Finish\" book (http://www.amazon.com/dp/1430249773)."
            };
        }
    }

    public class About
    {
        public string Description { get; set; }
    }
}
