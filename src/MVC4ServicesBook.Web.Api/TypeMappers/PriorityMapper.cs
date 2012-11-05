using System.Collections.Generic;
using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api.TypeMappers
{
    public class PriorityMapper : IPriorityMapper
    {
        public Priority CreatePriority(Data.Model.Priority priority)
        {
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
                                       }
                       };
        }
    }

}