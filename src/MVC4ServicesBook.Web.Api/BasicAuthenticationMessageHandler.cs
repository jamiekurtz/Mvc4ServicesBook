using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MVC4ServicesBook.Data.Model;
using MVC4ServicesBook.Web.Common.Security;
using NHibernate;

namespace MVC4ServicesBook.Web.Api
{
    public class BasicAuthenticationMessageHandler : DelegatingHandler
    {
        public const string BasicScheme = "Basic";
        public const string ChallengeAuthenticationHeaderName = "WWW-Authenticate";
        public const char AuthorizationHeaderSeparator = ':';

        private readonly IMembershipAdapter _membershipAdapter;
        private readonly ISessionFactory _sessionFactory;

        public BasicAuthenticationMessageHandler(IMembershipAdapter membershipAdapter, ISessionFactory sessionFactory)
        {
            _membershipAdapter = membershipAdapter;
            _sessionFactory = sessionFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            var authHeader = request.Headers.Authorization;
            if (authHeader == null)
            {
                return CreateUnauthorizedResponse();
            }

            if (authHeader.Scheme != BasicScheme)
            {
                return CreateUnauthorizedResponse();
            }

            var encodedCredentials = authHeader.Parameter;
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.ASCII.GetString(credentialBytes);
            var credentialParts = credentials.Split(AuthorizationHeaderSeparator);

            if (credentialParts.Length != 2)
            {
                return CreateUnauthorizedResponse();
            }

            var username = credentialParts[0].Trim();
            var password = credentialParts[1].Trim();

            if (!_membershipAdapter.ValidateUser(username, password))
            {
                return CreateUnauthorizedResponse();
            }

            SetPrincipal(username);

            return base.SendAsync(request, cancellationToken);
        }

        private void SetPrincipal(string username)
        {
            var roles = _membershipAdapter.GetRolesForUser(username);
            var user = _membershipAdapter.GetUser(username);

            User modelUser;
            using (var session = _sessionFactory.OpenSession())
            {
                modelUser = session.Get<User>(user.UserId);
            }

            var identity = CreateIdentity(user.Username, modelUser);

            var principal = new GenericPrincipal(identity, roles);
            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private GenericIdentity CreateIdentity(string username, User modelUser)
        {
            var identity = new GenericIdentity(username, BasicScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, modelUser.UserId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, modelUser.Firstname));
            identity.AddClaim(new Claim(ClaimTypes.Surname, modelUser.Lastname));
            identity.AddClaim(new Claim(ClaimTypes.Email, modelUser.Email));
            return identity;
        }

        private Task<HttpResponseMessage> CreateUnauthorizedResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.Add(ChallengeAuthenticationHeaderName, BasicScheme);

            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(response);
            return taskCompletionSource.Task;
        }
    }
}