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
    public class FrontModuleController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "frontmodule/frontmodule.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 得到列表数据
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

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance().FrontModule().GetList(curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="frontModuleCode">前端模块编号</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd,String frontModuleCode)
        {
            MDataRow mEntity = new MDataRow();
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "frontmodule/frontmodule.detail.cshtml");

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //新增
                
            }
            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().FrontModule().GetEntityWithFrontModuleCode(frontModuleCode);
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
        public ActionResult Execute(String doCmd)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("add"))
            {
                //增加
                MDataRow mEntity = DecorationService.Instance().FrontModule().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().FrontModule().Add(mEntity);
            }

            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().FrontModule().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().FrontModule().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String frontModuleCode = RequestHelper.GetFormString("frontmodulecode");
                exeMsgInfo = DecorationService.Instance().FrontModule().Delete(frontModuleCode);
            }
            //返回结果
            return Json(exeMsgInfo);
        }

    }
}