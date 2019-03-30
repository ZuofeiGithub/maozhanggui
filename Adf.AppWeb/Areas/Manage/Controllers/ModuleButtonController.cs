using System;
using System.Text;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class ModuleButtonController : BaseController
    {
        /// <summary>
        /// 模块首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(String moduleCode)
        {

            //分页
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetFormString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 100);

            //条件
            //String moduleCode = Server.UrlDecode(RequestHelper.GetQueryString("moduleCode"));


            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .ModuleButton()
                .GetAll(moduleCode);

            if (dtInfo != null)
            {
                recordCount = dtInfo.Rows.Count;
            }

            curPagerInfo.RecordCount = recordCount;
            curPagerInfo.PageCount = pageCount;

            ViewBag.DtMain = dtInfo;
            ViewBag.MainPagerInfo = curPagerInfo;

            String viewName = FrameWorkService.GetLoginedViewName("system/modulebutton/modulebutton.list");
            return View(viewName);
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData(String doCmd = "list")
        {
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("list"))
            {
                //分页
                int pageCount = 1;
                int recordCount = 0;
                PagerInfo curPagerInfo = new PagerInfo();
                curPagerInfo.PageIndex = RequestHelper.GetFormString("page", 1);
                curPagerInfo.PageSize = RequestHelper.GetFormString("pagesize", 100);

                //条件
                String moduleCode = Server.UrlDecode(RequestHelper.GetQueryString("moduleCode"));


                //获取数据
                MDataTable dtInfo = FrameWorkService.Instance()
                    .ModuleButton()
                    .GetAll(moduleCode);

                if (dtInfo != null)
                {
                    recordCount = dtInfo.Rows.Count;
                }

                curPagerInfo.RecordCount = recordCount;
                curPagerInfo.PageCount = pageCount;

                //返回数据
                String retData = "[]";
                if (dtInfo != null)
                {
                    retData = dtInfo.ToJson(true, false);
                }

                return Content(retData, "text/json", Encoding.UTF8);
            }


            return Content("[]", "text/json", Encoding.UTF8);

        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, int buttonId=0)
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
                mEntity = FrameWorkService.Instance().ModuleButton().InitDataRow();
                mEntity.Set("ButtonId", 0);

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().ModuleButton().GetEntityById(buttonId);
            }

            //
            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetLoginedViewName("system/modulebutton/modulebutton.detail");
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
                MDataRow mEntity = FrameWorkService.Instance().ModuleButton().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = FrameWorkService.Instance().ModuleButton().Insert(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().ModuleButton().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = FrameWorkService.Instance().ModuleButton().UpdateByButtonId(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                int buttonId = RequestHelper.GetFormString("buttonId",0);
                exeMsgInfo = FrameWorkService.Instance().ModuleButton().DeleteByButtonId(buttonId);
            }

            return Json(exeMsgInfo);

        }





    }
}
