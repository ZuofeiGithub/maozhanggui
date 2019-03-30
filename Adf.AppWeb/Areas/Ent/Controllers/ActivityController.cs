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
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// 活动推送
    /// 侯鑫辉
    /// 2018.10.20
    public class ActivityController : EntBaseController
    {
        /// <summary>
        /// 活动推送列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "activity/activity.index.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 活动推送列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 20);

            //查询条件
            String activityTitle = Server.UrlDecode(RequestHelper.GetQueryString("activitytitle"));
            String activitySummary = Server.UrlDecode(RequestHelper.GetQueryString("activitysummary"));
            String activityContent = Server.UrlDecode(RequestHelper.GetQueryString("activitycontent"));
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .Activity()
                .GetListWithCompanyCode(activityTitle, activitySummary, activityContent, companyCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="activityCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String activityCode = "")
        {
            MDataRow mEntity = new MDataRow();
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "activity/activity.detail.cshtml");

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().Activity().GetEntityWithActivityCode(activityCode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Execute(String doCmd)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("add"))
            {
                MDataRow mEntity = DecorationService.Instance().Activity().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("activitycode", Guid.NewGuid().ToString("N"));
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);
                mEntity.Set("companycode", companyCode);
                exeMsgInfo = DecorationService.Instance().Activity().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().Activity().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().Activity().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String activityCode = RequestHelper.GetFormString("activitycode");
                exeMsgInfo = DecorationService.Instance().Activity().Delete(activityCode);
            }else if (doCmd.Equals("copyadd"))
            {
                String activityCode = RequestHelper.GetFormString("activitycode");
                exeMsgInfo = DecorationService.Instance().Activity().CopyAdd(activityCode);
            }
            return Json(exeMsgInfo);
        }
    }
}