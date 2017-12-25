using CastleGo.Application;
using CastleGo.Application.Clients;
using CastleGo.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CastleGo.WebApi.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class AdminOAuthAuthorizationProvider : OAuthAuthorizationServerProvider
    {
        private readonly IClientService _clientService;

        /// <summary>
        /// 
        /// </summary>
        public UserManager<ApplicationUser> UserManager { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientService"></param>
        public AdminOAuthAuthorizationProvider(IClientService clientService)
        {
            _clientService = clientService;
            UserManager = MongoDbProvider.GetMongoDbSetting().UserManager;
        }

        /// <inheritdoc />
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
                context.TryGetFormCredentials(out clientId, out clientSecret);
            if (context.ClientId == null)
            {
                context.Validated();
                return Task.FromResult<object>(null);
            }
            ClientDto client = _clientService.FindClient(context.ClientId);
            if (client == null)
            {
                context.SetError("invalid_clientId", $"Client '{(object)context.ClientId}' is not registered in the system.");
                return Task.FromResult<object>(null);
            }
            if (client.ClientAppType == ClientAppTypes.NativeApp)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                if (client.Secret != StringHelper.GetHash(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                    return Task.FromResult<object>(null);
                }
            }
            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            if (!context.OwinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new string[]
                {
          allowedOrigin
                });
            ApplicationUser user = null;
            try
            {
                ApplicationUser applicationUser = await UserManager.FindByEmailAsync(context.UserName);
                user = applicationUser;
            }
            catch (Exception)
            {
                // ignored
            }
            if (user == null)
            {
                context.SetError("invalid_grant", "User not found.");
            }
            else
            {
                PasswordVerificationResult ph = UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, context.Password);
                if (ph != PasswordVerificationResult.Success)
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                else if (!user.IsAdmin)
                {
                    context.SetError("invalid_grant", "Have not enough permission.");
                }
                else
                {
                    ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Name));
                    identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id));
                    identity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", user.IsAdmin ? "admin" : "user"));
                    identity.AddClaim(new Claim("sub", context.UserName));
                    AuthenticationProperties props = new AuthenticationProperties(new Dictionary<string, string>() { { "as:client_id", string.IsNullOrEmpty(context.ClientId) ? string.Empty : context.ClientId }, { "userName", context.UserName } });
                    context.Validated(new AuthenticationTicket(identity, props)
                    {
                        Properties = {
              ExpiresUtc = new DateTimeOffset?(DateTime.UtcNow.AddDays(60.0))
            }
                    });
                }
            }
        }

        /// <inheritdoc />
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            if (context.Ticket.Properties.Dictionary["as:client_id"] != context.ClientId)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }
            ClaimsIdentity identity = new ClaimsIdentity(context.Ticket.Identity);
            Claim claim = identity.Claims.FirstOrDefault(c => c.Type == "newClaim");
            if (claim != null)
                identity.RemoveClaim(claim);
            identity.AddClaim(new Claim("newClaim", "newValue"));
            AuthenticationTicket ticket = new AuthenticationTicket(identity, context.Ticket.Properties);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        /// <inheritdoc />
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> keyValuePair in context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(keyValuePair.Key, keyValuePair.Value);
            return Task.FromResult<object>(null);
        }
    }
}
