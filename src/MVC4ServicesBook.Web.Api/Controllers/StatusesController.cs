using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    public class StatusesController : ApiController
    {
        private readonly ICommonRepository _commonRepository;

        public StatusesController(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public IEnumerable<Status> Get()
        {
            return _commonRepository
                .GetAll<Data.Model.Status>()
                .Select(x => new Status
                                 {
                                     StatusId = x.StatusId,
                                     Name = x.Name,
                                     Ordinal = x.Ordinal,
                                     Links = new List<Link>
                                                 {
                                                     new Link
                                                         {
                                                             Title = "self",
                                                             Rel = "self",
                                                             Href = "/api/statuses/" + x.StatusId
                                                         }
                                                 }
                                 })
                .ToList();
        }

        public Status Get(long id)
        {
            var status = _commonRepository.Get<Data.Model.Status>(id);
            if(status == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return new Status
                       {
                           StatusId = status.StatusId,
                           Ordinal = status.Ordinal,
                           Name = status.Name,
                           Links = new List<Link>
                                       {
                                           new Link
                                               {
                                                   Title = "self",
                                                   Rel = "self",
                                                   Href = "/api/statuses/" + status.StatusId
                                               }
                                       }
                       };
        }
    }
}
