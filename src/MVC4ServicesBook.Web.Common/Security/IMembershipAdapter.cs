using BasicAuthForWebAPI;

namespace MVC4ServicesBook.Web.Common.Security
{
    public interface IMembershipAdapter
    {
        MembershipProviderUser CreateUser(string username, string password, string email);
    }
}