using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Adf.Core.Db;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{

    public class MainController : BaseController
    {

        /// <summary>
        /// 主框架
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //
            MDataTable dtInfo = FrameWorkService.Instance().RoleModule().GetSubAllModuleOfUser(GlobalUserCode, "root");
           
            //FrameWorkService.Instance().RoleModule().get
            if (dtInfo == null || dtInfo.Rows.Count == 0)
            {
                return View(FrameWorkService.GetLoginedViewName("login/index"));
            }
            else
            {
                String viewName = FrameWorkService.GetLoginedViewName("main/main");
                //根目录
                MDataTable dtRoot = dtInfo.Select("ModuleParentCode='root' and isshow=1 ");
                ViewBag.DtRootModule = dtRoot;
                return View(viewName);
            }            
          
        }

        /// <summary>
        /// 桌面管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            return View(FrameWorkService.GetLoginedViewName("main/desktop"));
        }

        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <returns></returns>
        public ActionResult RefreshCache()
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            CacheHelper.ClearCache();
            SessionHelper.Clear();
            exeMsgInfo.RetStatus = 100;
            exeMsgInfo.RetValue = "清空成功";

            return Json(exeMsgInfo, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 得到二级子菜单
        /// </summary>
        /// <param name="parentCode">父级菜单</param>
        /// <returns></returns>
        public ActionResult GetChildLevel2(String parentCode)
        {
            //
            String sUserCode = GlobalUserLogin.Get("UserCode", "");
            MDataTable dtAllModuleOfUser = FrameWorkService.Instance().RoleModule().GetSubAllModuleOfUser(sUserCode, parentCode);


            String sAppCode = ConfigHelper.ReadAppSetting("DefaultAppCode");
            MDataRow drApp = FrameWorkService.Instance().AppInfo().GetEntity(sAppCode, true);
            String appDir = drApp.Get("AppDir", "");

            String retValue = "";

            MDataTable dtLevel1 = dtAllModuleOfUser.Select("ModuleParentCode=" + DbService.SetQuotesValue(parentCode) + " and isshow=1");
            StringBuilder sbInfo = new StringBuilder();
            sbInfo.Append("{");
            sbInfo.Append("\"state\": 1,");
            sbInfo.Append("\"message\": \"Success！\",");
            sbInfo.Append("\"data\": [");

            StringBuilder sbLevel1 = new StringBuilder();
            if (dtLevel1 != null && dtLevel1.Rows.Count > 0)
            {
                String moduleUrl = "";

                for (int i = 0; i < dtLevel1.Rows.Count; i++)
                {

                    MDataRow dr1 = dtLevel1.Rows[i];
                    moduleUrl = dr1.Get("ModuleUrl", "");

                    sbLevel1.Append("{");
                    sbLevel1.Append("\"title\": \"" + dr1.Get("ModuleName", "").Trim() + "\",");
                    sbLevel1.Append("\"icon\": \"fa fa-file-text-o\",");

                    sbLevel1.Append("\"href\": \"" + moduleUrl + "\",");
                    sbLevel1.Append("\"spread\": true,");

                    String curModuleCode = dr1.Get("ModuleCode", "");
                    MDataTable dtLevel2 = dtAllModuleOfUser.Select("ModuleParentCode=" + DbService.SetQuotesValue(curModuleCode));

                    if (dtLevel2 != null && dtLevel2.Rows.Count > 0)
                    {
                        sbLevel1.Append("\"children\":[");
                        for (int j = 0; j < dtLevel2.Rows.Count; j++)
                        {
                            MDataRow dr2 = dtLevel2.Rows[j];
                            moduleUrl = dr2.Get("ModuleUrl", "");
                            sbLevel1.Append("{");
                            sbLevel1.Append("\"title\": \"" + dr2.Get("ModuleName", "").Trim() + "\",");
                            sbLevel1.Append("\"icon\": \"fa fa-file-text-o\",");
                            sbLevel1.Append("\"href\": \"" + moduleUrl + "\",");
                            sbLevel1.Append("\"spread\": false");
                            sbLevel1.Append("}");

                            if (j < dtLevel2.Rows.Count - 1)
                            {
                                sbLevel1.Append(",");
                            }

                        }
                        sbLevel1.Append("]");
                    }
                    else
                    {
                        sbLevel1.Append("\"children\": []");
                    }

                    sbLevel1.Append("}");

                    if (i < dtLevel1.Rows.Count - 1)
                    {
                        sbLevel1.Append(",");
                    }

                }
            }
            sbInfo.Append(sbLevel1.ToString());
            sbInfo.Append("]");
            sbInfo.Append("}");

            sbInfo = sbInfo.Replace("@appdir", appDir);

            retValue = sbInfo.ToString();
            return Content(retValue, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 根据一级菜单得到左侧导航
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        public ActionResult GetLeftNav(String moduleCode)
        {

            MDataRow drApp = FrameWorkService.Instance().AppInfo().GetDefault(true);
            String appDir = drApp.Get("AppDir", "");

            // step 1 得到当前模块实体
            MDataRow drModuleEntity = FrameWorkService.Instance().Module().GetEntity(moduleCode);
            ViewBag.DrModuleEntity = drModuleEntity;

            MDataTable dtChildAll = FrameWorkService.Instance().RoleModule().GetAllModuleOfUser(GlobalUserCode);
            ViewBag.DtSubModuleAll = dtChildAll;


            //一级菜单
            StringBuilder sbHtml = new StringBuilder();
            MDataTable dtFirst = dtChildAll.Select(" isshow=1 and ModuleParentCode=" + DbService.SetQuotesValue(moduleCode) + " order by ModuleOrder asc");
            if (dtFirst != null && dtFirst.Rows.Count > 0)
            {
                foreach (MDataRow dataRow in dtFirst.Rows)
                {
                    String sModuleCode = dataRow.Get("ModuleCode", "");
                    String moduleInfo = "0";
                    if (dataRow.Get("ModuleIsModule", "") == "1")
                    {
                        moduleInfo = "1";
                    }

                    String sModuleUrl = dataRow.Get("ModuleUrl", "");
                    string baseUrl = !sModuleUrl.Contains("?") ? sModuleUrl : sModuleUrl.Substring(0, sModuleUrl.IndexOf("?"));
                    sModuleUrl = sModuleUrl.ToLower();
                    Dictionary<string, string> urlParam = RequestHelper.GetUrlParam(sModuleUrl);
                    if (!urlParam.ContainsKey("mccode"))
                    {
                        urlParam.Add("mccode", sModuleCode);
                    }

                    //设置新的Url信息
                    String sNewModuleUrl = HttpHelper.GetUrl(baseUrl, urlParam);

                    sbHtml.Append("<li class=\"site-menu-item\">");
                    sbHtml.Append("<a class=\"link\" rel=\"" + sNewModuleUrl + "\" data-code=\"" + moduleInfo + "-" + sModuleCode + "\"><i class=\"fa fa-laptop\"></i><span class=\"site-menu-title\">" + dataRow.Get("ModuleName", "") + "</span><span class=\"site-menu-arrow\"><i class=\"fa fa-angle-right\"></i></span></a>");
                    String rValue = GetSub(dataRow.Get("ModuleCode", ""), dtChildAll);
                    sbHtml.Append(rValue);
                    sbHtml.Append("</li>");
                }
            }

            sbHtml = sbHtml.Replace("@appdir", appDir);
            //String retHtml = sbHtml.ToString();

            ViewBag.LeftNavHtml = sbHtml.ToString();


            String viewName = FrameWorkService.GetLoginedViewName("main/leftnav");
            return PartialView(viewName);

        }

        public String GetSub(String moduleCode, MDataTable allModules)
        {
            String rValue = "";
            MDataTable dtInfo = allModules.Select("ModuleParentCode=" + DbService.SetQuotesValue(moduleCode) + " and isshow=1");

            if (dtInfo != null && dtInfo.Rows.Count > 0)
            {
                rValue += "<ul class=\"site-menu-sub\">";
                foreach (MDataRow dataRow in dtInfo.Rows)
                {
                    String sModuleCode = dataRow.Get("ModuleCode", "");
                    String moduleInfo = "0";
                    if (dataRow.Get("ModuleIsModule", "") == "1")
                    {
                        moduleInfo = "1";
                    }

                    String sModuleUrl = dataRow.Get("ModuleUrl", "");
                    string baseUrl = !sModuleUrl.Contains("?") ? sModuleUrl : sModuleUrl.Substring(0, sModuleUrl.IndexOf("?"));
                    sModuleUrl = sModuleUrl.ToLower();
                    Dictionary<string, string> urlParam = RequestHelper.GetUrlParam(sModuleUrl);
                    if (!urlParam.ContainsKey("mccode"))
                    {
                        urlParam.Add("mccode", sModuleCode);
                    }

                    //设置新的Url信息
                    String sNewModuleUrl = HttpHelper.GetUrl(baseUrl, urlParam);


                    rValue += "<li class=\"site-menu-item\">";
                    rValue +=
                        "<a class=\"link\" rel=\"" + sNewModuleUrl + "\" data-code=\"" + moduleInfo + "-" + dataRow.Get("ModuleCode", "") + "\"><span class=\"site-menu-title\">" + dataRow.Get("ModuleName", "") + "</span><span class=\"site-menu-arrow\"><i class=\"fa fa-angle-right\"></i></span></a>";

                    rValue += GetSub(dataRow.Get("ModuleCode", ""), allModules);

                    rValue += "</li>";


                }
                rValue += "</ul>";
            }


            return rValue;

        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Exit()
        {
            CacheHelper.ClearCache();
            SessionHelper.Clear();

            return Redirect("/manage/login");
        }

        



    }
}
