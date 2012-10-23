using System;
using System.Web.Security;

namespace MVC4ServicesBook.Web.Common.Security
{
    public class MembershipAdapter : IMembershipAdapter
    {
        public MembershipUserWrapper GetUser(string username)
        {
            var user = Membership.GetUser(username);
            return user == null ? null : CreateMembershipUserWrapper(user);
        }

        public MembershipUserWrapper GetUser(Guid userId)
        {
            var user = Membership.GetUser(userId);
            return user == null ? null : CreateMembershipUserWrapper(user);
        }

        public MembershipUserWrapper CreateMembershipUserWrapper(MembershipUser user)
        {
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

        public void SaveUser(Guid userId, string email)
        {
            var user = Membership.GetUser(userId);
            if(user == null)
            {
                throw new ArgumentException(string.Format("UserId {0} not found", userId));
            }

            user.Email = email;

            Membership.UpdateUser(user);
        }        

        public bool ValidateUser(string username, string password)
        {
            return true;
            //return Membership.ValidateUser(username, password);
        }

        public string[] GetRolesForUser(string username)
        {
            return Roles.GetRolesForUser(username);
        }
    }
}
