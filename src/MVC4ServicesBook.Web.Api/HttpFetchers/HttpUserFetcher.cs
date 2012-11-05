using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data.Model;
using NHibernate;

namespace MVC4ServicesBook.Web.Api.HttpFetchers
{
    public class HttpUserFetcher : IHttpUserFetcher
    {
        private readonly ISession _session;

        public HttpUserFetcher(ISession session)
        {
            _session = session;
        }

        public User GetUser(Guid userId)
        {
            var user = _session.Get<User>(userId);
            if (user == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("User {0} not found", userId)
                        });
            }

            return user;
        }
    }
}