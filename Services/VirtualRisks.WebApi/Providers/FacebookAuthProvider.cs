using Microsoft.Owin.Security.Facebook;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CastleGo.WebApi.Providers
{
    /// <summary>
    /// auth provider for facebook login
    /// </summary>
    public class FacebookAuthProvider : FacebookAuthenticationProvider
    {
        /// <inheritdoc />
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return (Task)Task.FromResult<object>((object)null);
        }
    }
}
