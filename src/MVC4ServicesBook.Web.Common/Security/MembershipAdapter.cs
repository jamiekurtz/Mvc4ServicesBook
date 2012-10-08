using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MVC4ServicesBook.Web.Common.Security
{
    public class MembershipAdapter
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
    }

    public class MembershipUserWrapper
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
