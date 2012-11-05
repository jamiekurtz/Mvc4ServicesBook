using System;
using MVC4ServicesBook.Data.Model;

namespace MVC4ServicesBook.Data
{
    public interface IUserRepository
    {
        void SaveUser(Guid userId, string firstname, string lastname);
        User GetUser(Guid userId);
    }
}
