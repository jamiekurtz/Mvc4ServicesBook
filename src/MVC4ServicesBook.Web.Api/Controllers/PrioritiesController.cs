using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MVC4ServicesBook.Web.Api.HttpFetchers;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Api.TypeMappers;
using MVC4ServicesBook.Web.Common;
using NHibernate;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    [LoggingNHibernateSession]
    public class PrioritiesController : ApiController
    {
        private readonly ISession _session;
        private readonly IPriorityMapper _priorityMapper;
        private readonly IHttpPriorityFetcher _priorityFetcher;

        public PrioritiesController(
            ISession session,
            IPriorityMapper priorityMapper,
            IHttpPriorityFetcher priorityFetcher)
        {
            _session = session;
            _priorityMapper = priorityMapper;
            _priorityFetcher = priorityFetcher;
        }

        public IEnumerable<Priority> Get()
        {
            return _session
                .QueryOver<Data.Model.Priority>()
                .List()
                .Select(_priorityMapper.CreatePriority);
        }

        public Priority Get(long id)
        {
            var priority = _priorityFetcher.GetPriority(id);
            return _priorityMapper.CreatePriority(priority);
        }
    }
}
