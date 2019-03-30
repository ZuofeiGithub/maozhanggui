using System;
using System.Web.Http;
using Adf.AppWeb;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Adf.AppWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //授权认证
            ConfigureAuth(app);

            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            app.UseWebApi(configuration);
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new SimpleAuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            app.UseOAuthAuthorizationServer(oAuthOptions);
        }
    }
}
