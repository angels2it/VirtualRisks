using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CastleGo.Application.Users;
using CastleGo.Providers;
using CastleGo.Shared;
using CastleGo.Shared.Users;
using CastleGo.WebApi.Models;
using CastleGo.WebApi.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CastleGo.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix(Startup.ApiPrefix)]
    public class AccountController : ApiController
    {
        private readonly IUserService _userService;

        private readonly IFacebookProvider _facebookProvider;
        /// <summary>
        /// 
        /// </summary>
        public UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="facebookProvider"></param>
        public AccountController(
            IUserService userService, IFacebookProvider facebookProvider)
        {
            UserManager = Startup.UserManagerFactory();
            _userService = userService;
            _facebookProvider = facebookProvider;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="externalAccessToken"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken, string email = "")
        {
            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }
            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            var user = await UserManager.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));
            var userByEmail = string.IsNullOrEmpty(email) ? null : await UserManager.FindByEmailAsync(email);
            var hasRegistered = user != null;
            if (!hasRegistered && userByEmail == null)
            {
                // create user
                var createdUser = await RegisterExternalInternal(provider, string.Empty, verifiedAccessToken.user_id, externalAccessToken);
                StartSyncFriend(createdUser.Id, provider, externalAccessToken);
                return Ok(ParseTokenFromUserData(createdUser));
            }
            if (!hasRegistered && userByEmail != null)
            {
                throw new Exception("This email was created by another account!");
            }
            var accessTokenResponse = ParseTokenFromUserData(Mapper.Map<UserDto>(user));
            //IF user logs in with an authentication method, 
            //where ProviderSpecificUserAccountID exists in IAD, 
            //AND the Login Token email is correct for that account, 
            //then we will just log him in as that account
            if (string.IsNullOrEmpty(email) || (!string.IsNullOrEmpty(user.Email) && user.Email.Equals(email)))
            {
                StartSyncFriend(user.Id, provider, externalAccessToken);
                return Ok(accessTokenResponse);
            }
            //ProviderSpecificUserAccountID in Login Token exists in IAD, 
            //BUT the email is not correct AND the email of Login Token exists as part of another account, THEN we
            //•	Set to NULL the account reference in IAD with Token’s ProviderSpecificUserAccountID
            //•	Add to the account which has email of Login Token to have ProviderSpecificUserAccountID
            //•	Log him in as the account to where we just moved the ProviderSpecificUserAccountID

            if (userByEmail != null)
            {
                var userEmailToken = ParseTokenFromUserData(Mapper.Map<UserDto>(userByEmail));
                StartSyncFriend(userByEmail.Id, provider, externalAccessToken);
                return Ok(userEmailToken);
            }
            accessTokenResponse.Remove("email");
            accessTokenResponse.Add(new JProperty("email", email));
            StartSyncFriend(user.Id, provider, externalAccessToken);
            return Ok(accessTokenResponse);
        }

        private void StartSyncFriend(string id, string provider, string token)
        {
            Task.Factory.StartNew(() => SyncFriend(id, provider, token));
        }

        private async Task SyncFriend(string id, string provider, string token)
        {
            if (provider != LoginType.Facebook.ToString())
                return;
            var friends = await _facebookProvider.Friends(token);
            if (friends == null)
                return;
            await _userService.UpdateFriendsAsync(id, Mapper.Map<List<FriendModel>>(friends));
        }


        private static JObject ParseTokenFromUserData(UserDto user)
        {
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.Name, user.Id, user.Email);
            if (!string.IsNullOrEmpty(user.Avatar))
                accessTokenResponse.Add(new JProperty("avatar", user.Avatar));
            accessTokenResponse.Add(new JProperty("isAdminUser", user.IsAdmin));
            return accessTokenResponse;
        }


        private string ParseVerifyTokenEndPoint(string provider, string accessToken)
        {
            string verifyTokenEndPoint;

            if (provider == LoginType.Facebook.ToString())
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = System.Configuration.ConfigurationManager.AppSettings["FacebookAppToken"];
                verifyTokenEndPoint =
                    $"https://graph.facebook.com/debug_token?input_token={accessToken}&access_token={appToken}";
            }
            else if (provider == LoginType.Google.ToString())
            {
                verifyTokenEndPoint = $"https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={accessToken}";
            }
            //else if (provider == LoginType.Instagram.ToString())
            //{
            //    verifyTokenEndPoint = $"https://api.instagram.com/v1/users/self?access_token={accessToken}";
            //}
            //else if (provider == LoginType.LinkedIn.ToString())
            //{
            //    verifyTokenEndPoint =
            //        $"https://api.linkedin.com/v1/people/~?format=json&oauth2_access_token={accessToken}";
            //}
            else
            {
                return null;
            }
            return verifyTokenEndPoint;
        }

        private async Task<ParsedExternalAccessToken> ParseTokenInfo(string verifyTokenEndPoint, string provider)
        {
            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)JsonConvert.DeserializeObject(content);

                var parsedToken = new ParsedExternalAccessToken();

                if (provider == LoginType.Facebook.ToString())
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.FacebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == LoginType.Google.ToString())
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    //if (!string.Equals(Startup.GoogleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    return null;
                    //}
                }
                //else if (provider == LoginType.Instagram.ToString())
                //{
                //    parsedToken.user_id = jObj["data"]["id"];
                //    parsedToken.app_id = string.Empty;
                //}
                //else if (provider == LoginType.LinkedIn.ToString())
                //{
                //    parsedToken.user_id = jObj["id"];
                //}
                return parsedToken;
            }
            return null;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            //Parse url for getting user profile
            var verifyTokenEndPoint = ParseVerifyTokenEndPoint(provider, accessToken);

            return await ParseTokenInfo(verifyTokenEndPoint, provider);
        }

        private static JObject GenerateLocalAccessTokenResponse(string userName, string id, string email)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(id))
                return new JObject(new JProperty("isOk", false));

            var tokenExpiration = TimeSpan.FromDays(30);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            var tokenResponse = new JObject(new JProperty("isOk", true),
                new JProperty("userId", id),
                new JProperty("userName", userName),
                new JProperty("email", email),
                new JProperty("fullName", userName),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in",
                tokenExpiration.TotalSeconds.ToString(CultureInfo.InvariantCulture)),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString()));

            return tokenResponse;
        }

        private async Task<UserDto> RegisterExternalInternal(string provider, string userName, string userId, string externalAccessToken, string fullname = "")
        {
            var user = new ApplicationUser()
            {
                IsActive = true
            };
            if (provider == LoginType.Facebook.ToString())
            {
                #region get fb info

                var client = new RestClient(new Uri("https://graph.facebook.com"));
                var request = new RestRequest("me", Method.GET);
                request.AddQueryParameter("access_token", externalAccessToken);
                request.AddQueryParameter("fields", "id,email,first_name,birthday,last_name");
                SocialInfo profile = client.Execute<SocialInfo>(request).Data;
                if (profile != null)
                {
                    user.Email = profile.Email;
                    user.Name = profile.FullName;
                    user.UserName = profile.Email;
                    user.Avatar = $"https://graph.facebook.com/{userId}/picture?width=300&height=300";
                }

                #endregion
            }
            else if (provider == LoginType.Google.ToString())
            {
                #region get google info

                var client = new RestClient(new Uri("https://www.googleapis.com/oauth2/v1/"));
                var request = new RestRequest("userinfo", Method.GET);
                request.AddQueryParameter("access_token", externalAccessToken);
                request.AddQueryParameter("fields", "id,email,name");
                SocialInfo profile = client.Execute<SocialInfo>(request).Data;
                if (profile != null)
                {
                    if (!string.IsNullOrEmpty(profile.Email))
                        user.Email = profile.Email;
                    user.Name = profile.Name;

                    // get avatar
                    client = new RestClient("https://www.googleapis.com/plus/v1/people/");
                    request = new RestRequest(userId, Method.GET);
                    request.AddQueryParameter("fields", "image");
                    request.AddQueryParameter("key", ConfigurationManager.AppSettings["GoogleApiKey"]);
                    var avatarInfo = client.Execute<AvatarInfo>(request).Data;
                    if (avatarInfo != null)
                        user.Avatar = avatarInfo.Image?.Url;
                    //if (!string.IsNullOrEmpty(profile.Gender))
                    //    user.Gender = ("male".Equals(profile.Gender) ? GenderTypes.Male : GenderTypes.Female);
                    //if (!string.IsNullOrEmpty(profile.Birthday))
                    //{
                    //    user.BirthDay = DateTime.ParseExact(profile.Birthday, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //}
                }

                #endregion
            }
            //else if (provider == LoginType.LinkedIn.ToString())
            //{
            //    #region get linked info

            //    var client = new RestClient(new Uri("https://api.linkedin.com/v1/"));
            //    var request = new RestRequest("people/~:(id,first-Name,last-Name,email-address)", Method.GET);
            //    request.AddQueryParameter("format", "json");
            //    request.AddQueryParameter("oauth2_access_token", externalAccessToken);
            //    LinkedInInfo profile = client.Execute<LinkedInInfo>(request).Data;
            //    if (profile != null)
            //    {
            //        if (!string.IsNullOrEmpty(profile.EmailAddress))
            //            user.Email = profile.EmailAddress;
            //        //user.FullName = profile.FullName;
            //        //if (!string.IsNullOrEmpty(profile.Gender))
            //        //    user.Gender = ("male".Equals(profile.Gender) ? GenderTypes.Male : GenderTypes.Female);
            //        //if (!string.IsNullOrEmpty(profile.Birthday))
            //        //{
            //        //    user.BirthDay = DateTime.ParseExact(profile.Birthday, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //        //}
            //    }

            //    #endregion
            //}
            //else if (provider == LoginType.Instagram.ToString())
            //{
            //    #region get instagram info

            //    var client = new RestClient(new Uri("https://api.instagram.com/v1/"));
            //    var request = new RestRequest("users/self", Method.GET);
            //    request.AddQueryParameter("access_token", externalAccessToken);
            //    InstagramInfo profile = client.Execute<InstagramInfo>(request).Data;
            //    if (profile != null)
            //    {
            //        //user.FullName = profile.Data.FullName;
            //        user.Email = $"{provider}{profile.Data.UserName}@iad.com";
            //    }

            //    #endregion
            //}
            //else if (provider == LoginType.Twitter.ToString())
            //{
            //    // generate email
            //    //user.FullName = fullname;
            //    user.Email = $"{userName}@iad.com";
            //}
            //create new user
            var userDto = Mapper.Map<UserDto>(user);
            userDto.Logins = new List<LoginsDto>()
            {
                new LoginsDto()
                {
                    LoginProvider = provider,
                    ProviderKey = userId
                }
            };
            await _userService.InsertAsync(userDto);
            return userDto;
        }
    }
}
