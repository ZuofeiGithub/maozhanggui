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
    public class PermissionController : BaseController
    {
        /// <summary>
        /// 权限首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //得到所有的角色
            MDataTable dtInfo = FrameWorkService.Instance().Role().GetAll();
            ViewBag.DtRole = dtInfo.ToDataTable();

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/Permission/Permission.index");
            return View(viewName);
        }

        
        /// <summary>
        /// 得到角色模块按钮数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleModuleButtonListData()
        {
                int pageCount = 1;
                int recordCount = 0;
                PagerInfo curPagerInfo = new PagerInfo();
                curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
                curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 10);

                //条件
                String moduleCode = Server.UrlDecode(RequestHelper.GetQueryString("moduleCode"));
                String roleCode = RequestHelper.GetQueryString("RoleCode");


                //获取数据
                MDataTable dtInfo = FrameWorkService.Instance()
                    .RoleModuleButton()
                    .GetAll(roleCode,moduleCode,false);

                if (dtInfo != null)
                {
                    recordCount = dtInfo.Rows.Count;
                }

                LayUiPager layUiPager = new LayUiPager();
                layUiPager.Count = recordCount;
                layUiPager.DtData = dtInfo;

                String jsonInfo = layUiPager.ToJson;
                return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 权限详细页
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
                String sRoleCode = RequestHelper.GetQueryString("rolecode");
                String sModuleCode = RequestHelper.GetQueryString("ModuleCode");
                mEntity.Set("modulecode", sModuleCode);
                ViewBag.RoleCode = sRoleCode;

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().RoleModuleButton().GetEntityByButtonId(buttonId);
                String sRoleCode = RequestHelper.GetQueryString("RoleCode");
            }

            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/Permission/Permission.Detail");
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
        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleModuleTreeData(String doCmd = "list")
        {
            String sModuleCode = RequestHelper.GetFormString("ModuleCode");
            String sRoleCode = RequestHelper.GetQueryString("RoleCode");
            if (String.IsNullOrEmpty(sModuleCode))
            {
                sModuleCode = "000";
            }
            MDataTable mDataTable = FrameWorkService.Instance().RoleModule().GetAll(sRoleCode, sModuleCode, false);

            String curJson = mDataTable.ToJson(false, false, RowOp.None, true);

            //ztree树状控件 需要isParent来指明当前的节点为父结点
            curJson = StringHelper.ReplaceAll(curJson, "isparent", "isParent");

            

            return Content(curJson, "text/json", Encoding.UTF8);

        }


        #region 模块授权至角色

        public ActionResult SetModuleToRole(String roleCode, String modulecode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(roleCode) || String.IsNullOrEmpty(modulecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色或者模块编码都不能为空";

            }
            else
            {
                exeMsgInfo = FrameWorkService.Instance().RoleModule().Set(roleCode, modulecode);
            }

            return Json(exeMsgInfo);
        }

        #endregion

        #region 授权操作按钮至角色

        public ActionResult SetButtonToRole(String roleCode, String moduleCode, String buttonCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(roleCode) || String.IsNullOrEmpty(moduleCode) || String.IsNullOrEmpty(buttonCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色或者模块编码或者按钮编码都不能为空";

            }
            else
            {
                exeMsgInfo = FrameWorkService.Instance().RoleModuleButton().Set(roleCode,moduleCode,buttonCode);
            }

            return Json(exeMsgInfo);
        }

        #endregion





    }
}
