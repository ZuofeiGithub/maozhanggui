using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Util;
using Adf.FrameWork.Service;
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class EntBaseController : Controller
    {
        //应用程序信息
        public MDataRow GlobalApp = null;

        //当前用户登录信息
        public MDataRow GlobalUserLogin = null;

        //当前用户名称
        public String GlobalUserCode = "";

        //Pc登录后的主模板
        public MDataRow GlobalAdminTheme = null;

        //管理模板视图
        public String GlobalAdminViewPath = "";

        //管理资源路径
        public String GlobalAdminResPath = "";

        //Pc全局的登录页模板
        public MDataRow GlobalLoginTheme = null;

        //登录页的视图路径
        public String GlobalLoginViewPath = "";

        /// <summary>
        /// 登录页的资源路径
        /// </summary>
        public String GlobalLoginResPath = "";

        //用户所拥有的角色列表
        public MDataTable GlobalUserRoles = null;

        //当前模块信息
        public MDataRow GlobalModuleEntity = null;

        //用户权限信息
        public MDataTable GlobalUserPermission = null;

        //当前模块
        public String CurrentModuleCode = "";

        public String GlobalModuleCode = "";


        //当前公司编号
        public String GlobalCompanyCode = "";


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*获取参数*/
            CurrentModuleCode = RequestHelper.GetQueryString("McCode");
            GlobalModuleCode = CurrentModuleCode;

            #region 根据是否是开发状态，来设定当前的应用
            String isDev = ConfigHelper.ReadAppSetting("IsDev");
            if (isDev == "1")
            {
                //如果是开发模式，采用开发模式的app信息
                String appCode = ConfigHelper.ReadAppSetting("AppCode");
                GlobalApp = FrameWorkService.Instance().AppInfo().GetEntity(appCode);
            }
            else
            {
                /*得到数据*/
                //应用程序
                GlobalApp = FrameWorkService.Instance().AppInfo().GetDefault(true);
            }

            #endregion


            //判断应用程序
            if (GlobalApp == null)
            {
                filterContext.HttpContext.Response.Write("应用程序信息未配置.");
                filterContext.HttpContext.Response.End();
                return;
            }

            GlobalLoginResPath ="/style/v2/";
            GlobalLoginViewPath = "/views/shared/theme/ent/";

            GlobalAdminViewPath = "/views/shared/theme/ent/";
            GlobalAdminResPath = "/style/v2/";



            //得到用户登录信息
            GlobalUserLogin = EntLoginService.GetLoginInfo();
            if (GlobalUserLogin != null)
            {
                GlobalUserCode = GlobalUserLogin.Get("UserCode", "");
                GlobalCompanyCode = GlobalUserLogin.Get("companycode", "");


                //得到用户角色
                //MDataTable dtUserRoles = FrameWorkService.Instance().UserRole().GetAllOfUser(GlobalUserCode, true);

                if (!String.IsNullOrEmpty(GlobalModuleCode))
                {


                    

                    ////如果模块编码不为空,则用户模块授权
                    ////当前模块信息
                    //GlobalModuleEntity = FrameWorkService.Instance().Module().GetEntity(GlobalModuleCode, true);

                    //if (GlobalModuleEntity == null)
                    //{
                    //    filterContext.HttpContext.Response.Write("当前模块信息不存在.缺少模块信息.模块编码是:" + GlobalModuleCode);
                    //    filterContext.HttpContext.Response.End();
                    //    return;
                    //}

                    ////对用户模块授权
                    ////用户对当前模块的权限
                    //GlobalUserPermission =
                    //    FrameWorkService.Instance()
                    //        .RoleModuleButton()
                    //        .GetAllModuleButtonOfUser(GlobalUserCode, GlobalModuleCode, false);
                }

                //ViewBag.GlobalUserRoles = dtUserRoles;
            }



            /*页面传参*/
            ViewBag.GlobalUserLogin = GlobalUserLogin;
            ViewBag.GlobalModuleCode = GlobalModuleCode;

            ViewBag.GlobalApp = GlobalApp;
            ViewBag.GlobalUserCode = GlobalUserCode;
            ViewBag.GlobalUserPermission = GlobalUserPermission;
            ViewBag.GlobalModuleEntity = GlobalModuleEntity;
            ViewBag.GlobalLoginResPath = GlobalLoginResPath;
            ViewBag.GlobalAdminResPath = GlobalAdminResPath;
            ViewBag.GlobalCompanyCode = GlobalCompanyCode; 

            base.OnActionExecuting(filterContext);
        }



    }
}