using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Util;

namespace Decoration.Service
{
    /// <summary>
    /// 用户验证
    /// </summary>
    public class UserAuthorizeFilter : AuthorizeAttribute
    {
        /// <summary>
        /// 重载实现用户验证
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //当前controller名称
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //当前Action名称
            var actionName = filterContext.ActionDescriptor.ActionName;
            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// 判断当前用户是否进行登录.可以指定特定的页面不进行授权控制.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(global::System.Web.HttpContextBase httpContext)
        {
            String curUrl = "";
            if (httpContext.Request.Url != null)
            {
                curUrl = httpContext.Request.Url.ToString().ToLower();
                if (curUrl.Contains("/admin/login"))
                {
                    LogHelper.WriteLog("当前是登录页面");
                    return true;
                }                   
            }

            //如果登录失败,系统记录当前用户不能登录
            if (EntLoginService.GetLoginInfo() == null)
            {
                return false;
            }
            return true;
        }

        #region Overrides of AuthorizeAttribute

        /// <summary>
        /// 处理授权失败的 HTTP 请求。
        /// </summary>
        /// <param name="filterContext">封装用于 <see cref="T:System.Web.Mvc.AuthorizeAttribute"/> 的信息。 <paramref name="filterContext"/> 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                //根据Web.Config的文档，指出未登录的用户，提示进行登录.
                //这里需要判断get方法还是post方法
                //如果是post方法
                string path = filterContext.HttpContext.Request.Path;
                String reDirectUrl = ConfigurationManager.AppSettings["EntUserLoginUrl"];               
                string strUrl = reDirectUrl + "?returnUrl={0}";

                //
                filterContext.HttpContext.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path)), true);
            } 
        }

        #endregion
    }
}
