using CastleGo.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CastleGo.WebApi.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicClientId"></param>
        /// <param name="userManagerFactory"></param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        public ApplicationOAuthProvider(string publicClientId, Func<UserManager<ApplicationUser>> userManagerFactory)
        {
            if (publicClientId == null)
                throw new ArgumentNullException(nameof(publicClientId));
            if (userManagerFactory == null)
                throw new ArgumentNullException(nameof(userManagerFactory));
            _publicClientId = publicClientId;
            _userManagerFactory = userManagerFactory;
        }

        /// <inheritdoc />
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            UserManager<ApplicationUser> userManager = _userManagerFactory();
            try
            {
                ApplicationUser applicationUser = await userManager.FindAsync(context.UserName, context.Password);
                ApplicationUser user = applicationUser;
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                ClaimsIdentity claimsIdentity1 = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);
                ClaimsIdentity oAuthIdentity = claimsIdentity1;
                ClaimsIdentity claimsIdentity2 = await userManager.CreateIdentityAsync(user, "Cookies");
                ClaimsIdentity cookiesIdentity = claimsIdentity2;
                AuthenticationProperties properties = CreateProperties(user.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            finally
            {
                userManager?.Dispose();
            }
        }

        /// <inheritdoc />
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> keyValuePair in context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(keyValuePair.Key, keyValuePair.Value);
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId && new Uri(context.Request.Uri, "/").AbsoluteUri == context.RedirectUri)
                context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static AuthenticationProperties CreateProperties(string userName)
        {
            return new AuthenticationProperties(new Dictionary<string, string>() { { "userName", userName } });
        }
    }
}
