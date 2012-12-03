using System;
using System.Web.Security;

namespace MVC4ServicesBook.Web.Common.Security
{
    public class MembershipAdapter : IMembershipInfoProvider
    {
        public MembershipUserWrapper GetUser(string username)
        {
            var user = Membership.GetUser(username);
            return CreateMembershipUserWrapper(user);
        }

        public MembershipUserWrapper GetUser(Guid userId)
        {
            var user = Membership.GetUser(userId);
            return CreateMembershipUserWrapper(user);
        }

        public MembershipUserWrapper CreateMembershipUserWrapper(MembershipUser user)
        {
            if (user == null)
            {
                return null;
            }

            return new MembershipUserWrapper
                       {
                           UserId = Guid.Parse(user.ProviderUserKey.ToString()),
                           Email = user.Email,
                           Username = user.UserName
                       };
        }

        public MembershipUserWrapper CreateUser(string username, string password, string email)
        {
            var user = Membership.CreateUser(username, password, email);
            return CreateMembershipUserWrapper(user);
        }

        public bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        public string[] GetRolesForUser(string username)
        {
            return Roles.GetRolesForUser(username);
        }
    }
}
