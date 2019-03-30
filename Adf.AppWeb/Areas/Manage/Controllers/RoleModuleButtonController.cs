using System;
using System.Collections.Generic;
using System.Linq;
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
    public class RoleModuleButtonController : BaseController
    {
        /// <summary>
        /// 模块首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(String moduleCode)
        {

            String sRoleCode = RequestHelper.GetQueryString("RoleCode");
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
                .RoleModuleButton()
                .GetAll(sRoleCode, moduleCode);

            if (dtInfo != null)
            {
                recordCount = dtInfo.Rows.Count;
            }

            curPagerInfo.RecordCount = recordCount;
            curPagerInfo.PageCount = pageCount;

            ViewBag.DtMain = dtInfo;
            ViewBag.MainPagerInfo = curPagerInfo;

            ViewBag.RoleCode = sRoleCode;
            ViewBag.ModuleCode = moduleCode;

            String viewName = FrameWorkService.GetLoginedViewName("system/rolemodulebutton/rolemodulebutton.list");
            return View(viewName);
        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="buttonId"></param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, int buttonId = 0)
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
                String sRoleCode = RequestHelper.GetQueryString("RoleCode");
                String sModuleCode = RequestHelper.GetQueryString("ModuleCode");
                mEntity.Set("ModuleCode", sModuleCode);
                ViewBag.RoleCode = sRoleCode;



            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().RoleModuleButton().GetEntityByButtonId(buttonId);
                String sRoleCode = RequestHelper.GetQueryString("RoleCode");
                ViewBag.RoleCode = sRoleCode;
                //String moduleCode = drRoleModuleButtonEntity.Get("ModuleCode", "");

                //String buttonCode = drRoleModuleButtonEntity.Get("ButtonCode", "");

                //if (mEntity != null)
                //{

                //    MDataRow drRoleModuleButtonEntity = FrameWorkService.Instance().RoleModuleButton().GetEntityByButtonId(buttonId);
                //    String roleCode = drRoleModuleButtonEntity.Get("RoleCode", "");
                //    MDataRow drEntityOfRole = FrameWorkService.Instance()
                //        .RoleModuleButton()
                //        .GetEntity(roleCode, moduleCode, buttonCode);

                //    if (drEntityOfRole != null)
                //    {
                //        ViewBag.HasButton = 1;
                //    }
                //}
            }

            //
            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetLoginedViewName("system/rolemodulebutton/rolemodulebutton.detail");
            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        public ActionResult Execute(String doCmd)
        {
            String roleCode = RequestHelper.GetFormString("RoleCode");
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
                if (exeMsgInfo.RetStatus == 100)
                {
                    if (!String.IsNullOrEmpty(roleCode))
                    {
                        exeMsgInfo = FrameWorkService.Instance()
                             .RoleModuleButton()
                             .Set(roleCode, mEntity.Get("ModuleCode", ""), mEntity.Get("ButtonCode", ""));
                    }
                }
            }
            else if (doCmd.Equals("modify"))
            {
                if (String.IsNullOrEmpty(roleCode))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "角色编码不能为空";
                }
                else
                {
                    MDataRow mEntity = FrameWorkService.Instance().ModuleButton().InitDataRow();
                    mEntity.LoadFrom(true);
                    exeMsgInfo = FrameWorkService.Instance().ModuleButton().UpdateByButtonId(mEntity);
                    if (exeMsgInfo.RetStatus == 101)
                    {
                        if (!String.IsNullOrEmpty(roleCode))
                        {
                            exeMsgInfo = FrameWorkService.Instance()
                                 .RoleModuleButton()
                                 .Set(roleCode, mEntity.Get("ModuleCode", ""), mEntity.Get("ButtonCode", ""));
                        }
                    }
                }


            }
            else if (doCmd.Equals("delete"))
            {
                int buttonId = RequestHelper.GetFormString("buttonId", 0);
                exeMsgInfo = FrameWorkService.Instance().ModuleButton().DeleteByButtonId(buttonId);

            }
            else if (doCmd.Equals("permission"))
            {
                //根据 角色 模块 按钮改变当前角色的授权
                String sModuleCode = RequestHelper.GetFormString("ModuleCode");
                String sButtonCode = RequestHelper.GetFormString("ButtonCode");
                exeMsgInfo = FrameWorkService.Instance()
                                 .RoleModuleButton()
                                 .Set(roleCode, sModuleCode, sButtonCode);


            }

            return Json(exeMsgInfo);

        }

    }
}
