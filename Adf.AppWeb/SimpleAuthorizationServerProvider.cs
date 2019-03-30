using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Adf.AppWeb
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            context.TryGetFormCredentials(out clientId, out clientSecret);

            
            //这里设置系统的授权码，如果与客户端过来的不相同则，不能进行连接，返回错误的提示吗
            //客户端过来的值 与 服务器端的进行比较 相同的则提示返回值,返回accesstoken
            String sysAppId = "1234";
            String sysSecrect = "5678";
            
            if (clientId == sysAppId && clientSecret == sysSecrect)
            {
                context.Validated(clientId);
            }

            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, "iOS App"));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);

            return base.GrantClientCredentials(context);
        }
    }
}