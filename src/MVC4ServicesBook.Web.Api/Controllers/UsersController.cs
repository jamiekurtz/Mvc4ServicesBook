using System.Linq;
using System.Web.Http;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;

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

        public IQueryable<Data.Model.User> Get()
        {
            return _userRepository.AllUsers();
        }

        public User Post(User user)
        {
            return _userManager.CreateUser(user.Username, user.Password, user.Firstname, user.Lastname, user.Email);
        }
    }
}
