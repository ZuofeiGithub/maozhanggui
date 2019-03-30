using System;
using System.Collections.Generic;
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
    public class ModuleController : BaseController
    {
       /// <summary>
       /// 模块首页
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/module/module.index.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData(String doCmd = "list")
        {
            String moduleCode = Server.UrlDecode(RequestHelper.GetFormString("moduleCode"));
            if (String.IsNullOrEmpty(moduleCode))
            {
                moduleCode = "root";
            }
            MDataTable mDataTable = FrameWorkService.Instance().Module().GetChildAll(moduleCode, false);

            String curJson = mDataTable.ToJson(false, false,RowOp.None, true);

            //ztree树状控件 需要isParent来指明当前的节点为父结点
            curJson = StringHelper.ReplaceAll(curJson, "isparent", "isParent");

            return Content(curJson, "text/json", Encoding.UTF8);
        }

        public ActionResult GetListData(String parentCode)
        {
            if (String.IsNullOrEmpty(parentCode))
            {
                parentCode = "000";
            }

            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .Module()
                .GetChildAll(parentCode);

            int rowCount = 0;
            if (dtInfo != null)
            {
                rowCount = dtInfo.Rows.Count;
            }

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = rowCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }


        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="moduleCode"></param>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String moduleCode,String parentCode)
        {
            MDataRow mEntity = new MDataRow();

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            MDataTable dtModuleType = FrameWorkService.Instance().DataDic().GetAll("moduletype");
            ViewBag.DtModuleCate = dtModuleType;

            if (docmd.Equals("add"))
            {
                if (String.IsNullOrEmpty(parentCode))
                {
                    parentCode = "000";
                }

                //增加          
                mEntity = FrameWorkService.Instance().Module().InitDataRow();
                mEntity.Set("ModuleParentCode", parentCode);

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().Module().GetEntity(moduleCode);
            }

            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/module/module.detail.cshtml");
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
                MDataRow mEntity = FrameWorkService.Instance().Module().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = FrameWorkService.Instance().Module().Insert(mEntity);
                
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().Module().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = FrameWorkService.Instance().Module().UpdateByModuleCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String sModuleCode = RequestHelper.GetFormString("ModuleCode");
                exeMsgInfo = FrameWorkService.Instance().Module().DeleteByModuleCode(sModuleCode);
            }

            return Json(exeMsgInfo);

        }
    }
}
