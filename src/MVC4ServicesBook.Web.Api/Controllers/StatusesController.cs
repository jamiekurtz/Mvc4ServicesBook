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
    [LoggingNHibernateSessions]
    public class StatusesController : ApiController
    {
        private readonly ISession _session;
        private readonly IStatusMapper _statusMapper;
        private readonly IHttpStatusFetcher _statusFetcher;

        public StatusesController(
            ISession session, 
            IStatusMapper statusMapper,
            IHttpStatusFetcher statusFetcher)
        {
            _session = session;
            _statusMapper = statusMapper;
            _statusFetcher = statusFetcher;
        }

        public IEnumerable<Status> Get()
        {
            return _session
                .QueryOver<Data.Model.Status>()
                .List()
                .Select(_statusMapper.CreateStatus);
        }

        public Status Get(long id)
        {
            var status = _statusFetcher.GetStatus(id);
            return _statusMapper.CreateStatus(status);
        }
    }
}
