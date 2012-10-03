using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    public class PrioritiesController : ApiController
    {
        private readonly ICommonRepository _commonRepository;

        public PrioritiesController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public IEnumerable<Priority> Get()
        {
            return _commonRepository
                .GetAll<Data.Model.Priority>()
                .Select(x => new Priority
                                 {
                                     PriorityId = x.PriorityId,
                                     Name = x.Name,
                                     Ordinal = x.Ordinal,
                                     Links = new List<Link>
                                                 {
                                                     new Link
                                                         {
                                                             Title = "self",
                                                             Rel = "self",
                                                             Href = "/api/priorities/" + x.PriorityId
                                                         }
                                                 }
                                 })
                .ToList();
        }

        public Priority Get(long id)
        {
            var priority = _commonRepository.Get<Data.Model.Priority>(id);
            if(priority == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new Priority
                       {
                           PriorityId = priority.PriorityId,
                           Ordinal = priority.Ordinal,
                           Name = priority.Name,
                           Links = new List<Link>
                                       {
                                           new Link
                                               {
                                                   Title = "self",
                                                   Rel = "self",
                                                   Href = "/api/priorities/" + priority.PriorityId
                                               }
                                       }};
        }
    }
}
