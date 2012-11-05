using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data.Model;
using NHibernate;

namespace MVC4ServicesBook.Web.Api.HttpFetchers
{
    public class HttpStatusFetcher : IHttpStatusFetcher
    {
        private readonly ISession _session;

        public HttpStatusFetcher(ISession session)
        {
            _session = session;
        }

        public Status GetStatus(long statusId)
        {
            var status = _session.Get<Status>(statusId);
            if (status == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("Status {0} not found", statusId)
                        });
            }

            return status;
        }
    }
}