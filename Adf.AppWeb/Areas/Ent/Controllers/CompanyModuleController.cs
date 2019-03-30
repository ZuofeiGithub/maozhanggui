using System;
using System.Text;
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
    /// <summary>
    /// 企业模块扩展
    /// </summary>
    public class CompanyModuleController : BaseController
    {
        /// <summary>
        /// 模块首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/companymodule/module.index.cshtml");
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
            MDataTable mDataTable = DecorationService.Instance().CompanyModule().GetChildAll(moduleCode, false);

            String curJson = mDataTable.ToJson(false, false, RowOp.None, true);

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
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyModule()
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
        public ActionResult Detail(String docmd, String moduleCode, String parentCode)
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
                mEntity = DecorationService.Instance().CompanyModule().InitDataRow();
                mEntity.Set("ModuleParentCode", parentCode);

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().CompanyModule().GetEntity(moduleCode);
            }

            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/companymodule/module.detail.cshtml");
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
                MDataRow mEntity = DecorationService.Instance().CompanyModule().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().CompanyModule().Insert(mEntity);

            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().CompanyModule().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().CompanyModule().UpdateByModuleCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String sModuleCode = RequestHelper.GetFormString("ModuleCode");
                exeMsgInfo = DecorationService.Instance().CompanyModule().DeleteByModuleCode(sModuleCode);
            }

            return Json(exeMsgInfo);

        }
    }
}
