using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MVC4ServicesBook.Web.Common.Security
{
    public class BasicAuthorizationMessageHandler : DelegatingHandler
    {
        public const string BasicScheme = "Basic";
        public const string ChallengeAuthorizationHeaderName = "WWW-Authenticate";
        public const char AuthorizationHeaderSeparator = ':';

        private readonly IMembershipAdapter _membershipAdapter;

        public BasicAuthorizationMessageHandler()
            : this(new MembershipAdapter())
        {
        }

        public BasicAuthorizationMessageHandler(IMembershipAdapter membershipAdapter)
        {
            _membershipAdapter = membershipAdapter;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
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
            var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(encodedCredentials));
            var parts = credentials.Split(AuthorizationHeaderSeparator);

            if(parts.Length != 2)
            {
                return CreateUnauthorizedResponse();
            }

            var username = parts[0].Trim();
            var password = parts[1].Trim();

            if (!_membershipAdapter.ValidateUser(username, password))
            {
                return CreateUnauthorizedResponse();
            }

            SetPrincipal(username);

            return base.SendAsync(request, cancellationToken);
        }

        private void SetPrincipal(string username)
        {
            var identity = new GenericIdentity(username, BasicScheme);
            //var roles = _membershipAdapter.GetRolesForUser(username);
            var roles = new[] {"admin"};
            var principal = new GenericPrincipal(identity, roles);

            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private Task<HttpResponseMessage> CreateUnauthorizedResponse()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.Headers.Add(ChallengeAuthorizationHeaderName, BasicScheme);

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}