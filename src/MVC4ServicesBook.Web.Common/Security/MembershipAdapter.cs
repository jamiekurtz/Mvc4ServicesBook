using System.Web.Security;
using BasicAuthForWebAPI;

namespace MVC4ServicesBook.Web.Common.Security
{
    public class MembershipAdapter : IMembershipAdapter
    {
        public MembershipProviderUser CreateMembershipUser(MembershipUser user)
        {
            if (user == null)
            {
                return null;
            }

            return new MembershipProviderUser
                       {
                           UserId = (string)user.ProviderUserKey,
                           Email = user.Email,
                           Username = user.UserName
                       };
        }

        public MembershipProviderUser CreateUser(string username, string password, string email)
        {
            var user = Membership.CreateUser(username, password, email);
            return CreateMembershipUser(user);
        }
    }
}
