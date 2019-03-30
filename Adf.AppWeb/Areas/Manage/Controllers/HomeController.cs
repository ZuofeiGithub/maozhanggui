using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Db;
using Adf.Core.Util;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 主框架
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MDataTable dtInfo = FrameWorkService.Instance().RoleModule().GetSubAllModuleOfUser(GlobalUserCode, "000");

            if (dtInfo == null || dtInfo.Rows.Count == 0)
            {
                return View(FrameWorkService.GetModuleViewName(GlobalLoginViewPath, "login"));
            }
            else
            {
                String viewName = GlobalAdminViewPath + "home/index.cshtml";
                //根目录
                MDataTable dtRoot = dtInfo.Select("ModuleParentCode='000' and isshow=1 ");
                ViewBag.DtRootModule = dtRoot;
                return View(viewName);
            }
        }

        /// <summary>
        /// 得到左侧导航菜单
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        public ActionResult GetLeftNav(String moduleCode)
        {
            String appDir = GlobalApp.Get("AppDir", "");


            MDataTable dtChildAll = FrameWorkService.Instance().RoleModule().GetAllModuleOfUser(GlobalUserCode);



            //一级菜单
            StringBuilder sbHtml = new StringBuilder();
            MDataTable dtFirst = dtChildAll.Select(" isshow=1 and ModuleParentCode=" + DbService.SetQuotesValue(moduleCode) + " order by ModuleOrder desc");
            if (dtFirst != null && dtFirst.Rows.Count > 0)
            {


                for (int i = 0; i < dtFirst.Rows.Count; i++)
                {
                    MDataRow dataRow = dtFirst.Rows[i];

                    String firstItemClass = "";
                    if (i == 0)
                    {
                        firstItemClass = "layui-nav-itemed";
                    }

                    String sModuleCode = dataRow.Get("ModuleCode", "");
                    String moduleInfo = "0";
                    if (dataRow.Get("ModuleIsModule", "") == "1")
                    {
                        moduleInfo = "1";
                    }

                    //设置模块的Url
                    String sModuleUrl = dataRow.Get("ModuleUrl", "");
                    String sModuleName = dataRow.Get("ModuleName", "");
                    String sIcon = dataRow.Get("moduleicon", "");
                    string baseUrl = !sModuleUrl.Contains("?") ? sModuleUrl : sModuleUrl.Substring(0, sModuleUrl.IndexOf("?"));
                    sModuleUrl = sModuleUrl.ToLower();
                    Dictionary<string, string> urlParam = RequestHelper.GetUrlParam(sModuleUrl);
                    if (!urlParam.ContainsKey("mccode"))
                    {
                        urlParam.Add("mccode", sModuleCode);
                    }

                    //设置新的Url信息
                    String sNewModuleUrl = HttpHelper.GetUrl(baseUrl, urlParam);

                    sbHtml.Append("<li data-name=\"" + sModuleCode + "\" class=\"layui-nav-item " + firstItemClass + "\">");
                    sbHtml.Append("<a href=\"javascript:;\" lay-tips=\"" + sModuleName + "\" lay-direction=\"2\">");
                    sbHtml.Append("<i class=\"layui-icon " + sIcon + "\"></i>");
                    sbHtml.Append("<cite>" + sModuleName + "</cite>");
                    sbHtml.Append("</a>");
                    sbHtml.Append(GetSub(sModuleCode, dtChildAll));
                    sbHtml.Append("</li>");
                }
            }

            sbHtml = sbHtml.Replace("@appdir", appDir);
            return Content(sbHtml.ToString());

        }

        public String GetSub(String moduleCode, MDataTable allModules)
        {
            String rValue = "";
            MDataTable dtInfo = allModules.Select("ModuleParentCode=" + DbService.SetQuotesValue(moduleCode) + " and isshow=1");

            if (dtInfo != null && dtInfo.Rows.Count > 0)
            {
                rValue += "<dl class=\"layui-nav-child\">";
                for (int i = 0; i < dtInfo.Rows.Count; i++)
                {
                    MDataRow dataRow = dtInfo.Rows[i];
                    String sModuleCode = dataRow.Get("ModuleCode", "");
                    String sModuleName = dataRow.Get("ModuleName", "");
                    String sIcon = dataRow.Get("moduleicon", "");

                    //组合模块url
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

                    String sFirst = "";
                    if (i == 0)
                    {
                        sFirst = "class=\"layui-this\"";
                    }

                    rValue += "<dd  data-name=\"" + sModuleCode + "\" " + sFirst + ">";
                    rValue += "<a lay-href=\"" + sNewModuleUrl + "\">";
                    rValue += "<i class=\"layui-icon " + sIcon + "\"></i>";
                    rValue += "<cite>" + sModuleName + "</cite>";
                    rValue += "</a>";
                    rValue += GetSub(sModuleCode, allModules);
                    rValue += "</dd>";
                }
                rValue += "</dl>";
            }
            return rValue;
        }

        /// <summary>
        /// 桌面
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            return View(FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "home/desktop.cshtml"));
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Exit()
        {
            CacheHelper.ClearCache();
            SessionHelper.Clear();

            return Redirect("/manage/home/exit");
        }

    }
}
