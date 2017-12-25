using CastleGo.WebApi.Models;
using CastleGo.WebApi.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

namespace CastleGo.WebApi
{
    /// <summary>Represents the entry point into an application.</summary>
    public class Startup
    {
        internal static string PublicClientId = "self";
        internal const string ApiPrefix = "api";

        internal static OAuthAuthorizationServerOptions OAuthOptions { get; }

        internal static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; }

        internal static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }

        internal static Func<UserManager<ApplicationUser>> UserManagerFactory { get; set; }

        static Startup()
        {
            MongoDbSetting mg = MongoDbProvider.GetMongoDbSetting();
            UserManagerFactory = () => mg.UserManager;
            OAuthAuthorizationServerOptions authorizationServerOptions = new OAuthAuthorizationServerOptions();
            authorizationServerOptions.TokenEndpointPath = new PathString("/login");
            authorizationServerOptions.Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory);
            authorizationServerOptions.AccessTokenExpireTimeSpan = TimeSpan.FromDays(14.0);
            int num = 1;
            authorizationServerOptions.AllowInsecureHttp = num != 0;
            OAuthOptions = authorizationServerOptions;
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }

        /// <summary>
        /// Specifies how the ASP.NET application will respond to individual HTTP request.
        /// </summary>
        /// <param name="app">Instance of <see cref="T:Owin.IAppBuilder" />.</param>
        public void Configuration(IAppBuilder app)
        {
            CorsConfig.ConfigureCors(ConfigurationManager.AppSettings["cors"]);
            app.UseCors(CorsConfig.Options);
            HttpConfiguration configuration = new HttpConfiguration();
            configuration
                .UsingSwagger()
                .UsingAutoMapper()
                .UsingAutofac()
                .UsingFormatter()
                .UsingService()
                .UsingRoute();
                
            app.UseHangfire();

            configuration.UsingGameConfig();

            app.SetDefaultSignInAsAuthenticationType("ExternalCookie");
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(180.0),
                Provider = (IOAuthAuthorizationServerProvider)configuration.DependencyResolver.GetService(typeof(AdminOAuthAuthorizationProvider))
            });
            FacebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = ConfigurationManager.AppSettings["FacebookAppId"],
                AppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"],
                Provider = new FacebookAuthProvider()
            };
            FacebookAuthOptions.Scope.Add("email");
            app.UseOAuthBearerAuthentication(OAuthBearerOptions).UseFacebookAuthentication(FacebookAuthOptions).UseAutofacMiddleware(AutofacConfig.Container).UseWebApi(configuration).UseOAuthBearerTokens(OAuthOptions);
        }
    }
}
