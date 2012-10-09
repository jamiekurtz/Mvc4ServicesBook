using MVC4ServicesBook.Web.Api.Models;

namespace MVC4ServicesBook.Web.Api
{
    public interface IUserManager
    {
        User CreateUser(string username, string password, string firstname, string lastname, string email);
    }
}