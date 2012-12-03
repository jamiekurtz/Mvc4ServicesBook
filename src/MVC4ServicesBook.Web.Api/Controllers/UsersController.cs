using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Web.Api.HttpFetchers;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Api.TypeMappers;
using MVC4ServicesBook.Web.Common;
using NHibernate;
using NHibernate.Linq;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ISession _session;
        private readonly IUserManager _userManager;
        private readonly IUserMapper _userMapper;
        private readonly IHttpUserFetcher _userFetcher;

        public UsersController(
            IUserManager userManager,
            IUserMapper userMapper,
            IHttpUserFetcher userFetcher,
            ISession session)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _userFetcher = userFetcher;
            _session = session;
        }

        [Queryable]
        public IQueryable<Data.Model.User> Get()
        {
            return _session.Query<Data.Model.User>();
        }

        [LoggingNHibernateSession]
        public User Get(Guid id)
        {
            var user = _userFetcher.GetUser(id);
            return _userMapper.CreateUser(user);
        }

        [LoggingNHibernateSession]
        public HttpResponseMessage Post(HttpRequestMessage request, User user)
        {
            var newUser = _userManager.CreateUser(user.Username, user.Password, user.Firstname, user.Lastname,
                                                  user.Email);

            var href = newUser.Links.First(x => x.Rel == "self").Href;

            var response = request.CreateResponse(HttpStatusCode.Created, newUser);
            response.Headers.Add("Location", href);

            return response;
        }
    }
}
