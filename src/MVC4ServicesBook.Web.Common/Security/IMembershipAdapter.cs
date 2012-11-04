using System;

namespace MVC4ServicesBook.Web.Common.Security
{
    public interface IMembershipAdapter
    {
        MembershipUserWrapper GetUser(string username);
        MembershipUserWrapper GetUser(Guid userId);
        MembershipUserWrapper CreateUser(string username, string password, string email);
        bool ValidateUser(string username, string password);
        string[] GetRolesForUser(string username);
    }
}