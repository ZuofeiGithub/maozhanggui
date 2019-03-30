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

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class WfInfoController : BaseController
    {
        /// <summary>
        /// 得到列表Json值
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 100);

            //条件
            String wfcode = Server.UrlDecode(RequestHelper.GetQueryString("wfcode"));
            String wfName = Server.UrlDecode(RequestHelper.GetQueryString("wfName"));
            String sOrderBy = RequestHelper.GetQueryString("OrderBy");


            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .WfInfo()
                .GetList(wfcode,wfName,sOrderBy,curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/wfmng/wfinfo/wfinfo.list.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="wfcode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String wfcode)
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
                mEntity = FrameWorkService.Instance().WfInfo().InitDataRow();

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().WfInfo().GetDataEntityByWfCode(wfcode);
            }

            //
            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/wfmng/wfinfo/wfinfo.detail.cshtml");
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
                MDataRow mEntity = FrameWorkService.Instance().WfInfo().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);

                exeMsgInfo = FrameWorkService.Instance().WfInfo().Add(mEntity);

            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().WfInfo().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("modifyusercode", GlobalUserCode);
                mEntity.Set("modifydatetime", DateTime.Now);

                exeMsgInfo = FrameWorkService.Instance().WfInfo().UpdateByWfCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String wfcode = RequestHelper.GetFormString("wfcode");
                exeMsgInfo = FrameWorkService.Instance().WfInfo().DeleteByWfCode(wfcode);
            }

            return Json(exeMsgInfo);

        }



    }
}
