using System.Collections.Generic;
using MVC4ServicesBook.Data;
using MVC4ServicesBook.Web.Api.Models;
using MVC4ServicesBook.Web.Common.Security;

namespace MVC4ServicesBook.Web.Api
{
    public class UserManager : IUserManager
    {
        private readonly IMembershipAdapter _membershipAdapter;
        private readonly IUserRepository _userRepository;

        public UserManager(IMembershipAdapter membershipAdapter, IUserRepository userRepository)
        {
            _membershipAdapter = membershipAdapter;
            _userRepository = userRepository;
        }

public User CreateUser(string username, string password, string firstname, string lastname, string email)
{
    var wrapper = _membershipAdapter.CreateUser(username, password, email);

    _userRepository.SaveUser(wrapper.UserId, firstname, lastname);

    var user = new User
                    {
                        UserId = wrapper.UserId,
                        Username = username,
                        Email = email,
                        Firstname = firstname,
                        Lastname = lastname,
                        Links = new List<Link>()
                    };

    return user;
}
    }
}