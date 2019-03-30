using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    public class SystemConfigurationController : EntBaseController
    {
        /// <summary>
        /// 数据字典首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "systemconfiguration/systemconfiguration.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        public ActionResult GetListData(String parentCode = "system")
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 20);
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .SystemConfiguration()
                .GetList(companyCode,parentCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData(String doCmd = "list", String dicCode = "root")
        {
            //条件
            if (String.IsNullOrEmpty(dicCode))
            {
                dicCode = "root";
            }

            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
            //获取数据
            MDataTable dtInfo = DecorationService.Instance().SystemConfiguration().GetAll(dicCode, companyCode);

            String curJson = dtInfo.ToJson(false, false, RowOp.None, true);
            curJson = StringHelper.ReplaceAll(curJson, "isparent", "isParent");

            return Content(curJson, "text/json", Encoding.UTF8);

        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="dicCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String dicCode = "")
        {
            MDataRow mEntity = new MDataRow();

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().SystemConfiguration().InitDataRow();
                String sParentCode = RequestHelper.GetQueryString("ParentCode");
                mEntity.Set("ParentCode", sParentCode);

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().SystemConfiguration().GetEntityByDicCode(dicCode);
            }

            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = GlobalAdminViewPath + "systemconfiguration/systemconfiguration.detail.cshtml";
            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        public ActionResult Execute(String doCmd)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            if (doCmd.Equals("add"))
            {
                MDataRow mEntity = DecorationService.Instance().SystemConfiguration().InitDataRow();
                mEntity.LoadFrom(true);

                if (String.IsNullOrEmpty(mEntity.Get("DicCode", "")))
                {
                    mEntity.Set("DicCode", System.Guid.NewGuid().ToString("N"));
                }
                mEntity.Set("companycode", companyCode);
                string dicvalue = Server.UrlDecode(mEntity.Get("dicvalue", ""));
                mEntity.Set("dicvalue", dicvalue);

                exeMsgInfo = DecorationService.Instance().SystemConfiguration().Insert(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().SystemConfiguration().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("companycode", companyCode);
                string dicvalue = Server.UrlDecode(mEntity.Get("dicvalue", ""));
                mEntity.Set("dicvalue", dicvalue);
                exeMsgInfo = DecorationService.Instance().SystemConfiguration().UpdateByDicCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String sDicCode = RequestHelper.GetFormString("DicCode");
                exeMsgInfo = DecorationService.Instance().SystemConfiguration().DeleteByDicCode(sDicCode);
            }

            return Json(exeMsgInfo);

        }
    }
}