using System;
using System.Linq;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Data
{
    public interface IUserRepository
    {
        IQueryable<User> AllUsers();
        void SaveUser(Guid userId, string firstname, string lastname);
        User GetUser(Guid userId);
    }
}
