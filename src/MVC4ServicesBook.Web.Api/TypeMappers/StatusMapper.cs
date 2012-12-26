using System.Collections.Generic;
using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public class StatusMapper : IStatusMapper
    {
        public Status CreateStatus(Data.Model.Status status)
        {
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
                                               },
                                           new Link
                                               {
                                                   Title = "All Statuses",
                                                   Rel = "all",
                                                   Href = "/api/statuses"
                                               }
                                       }
                       };
        }
    }
}