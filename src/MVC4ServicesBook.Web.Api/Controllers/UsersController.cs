using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common;

namespace MVC4ServicesBook.Web.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserManager _userManager;

        public UsersController(IUserRepository userRepository, IUserManager userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [Queryable]
        public IQueryable<Data.Model.User> Get()
        {
            return _userRepository.AllUsers();
        }

        [LoggingNHibernateSessions]
        public Data.Model.User Get(Guid id)
        {
            var user = _userRepository.GetUser(id);
            if(user == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ReasonPhrase = string.Format("User {0} not found", id)
                        });
            }

            return user;
        }

        [LoggingNHibernateSessions]
        public HttpResponseMessage Post(HttpRequestMessage request, User user)
        {
            var newUser = _userManager.CreateUser(user.Username, user.Password, user.Firstname, user.Lastname, user.Email);

            var response = request.CreateResponse(HttpStatusCode.Created, newUser);
            response.Headers.Add("Location", "/api/users/" + newUser.UserId);

            return response;
        }
    }
}
