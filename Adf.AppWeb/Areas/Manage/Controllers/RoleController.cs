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
    public class RoleController : BaseController
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
            String sRoleCode = Server.UrlDecode(RequestHelper.GetQueryString("RoleCode"));
            String sRoleName = Server.UrlDecode(RequestHelper.GetQueryString("RoleName"));
            String sOrderBy = RequestHelper.GetQueryString("OrderBy");


            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .Role()
                .GetList(sRoleCode, sRoleName, curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo,"text/json",Encoding.UTF8);
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/role/role.list.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 选择一个角色
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {
            String viewName = FrameWorkService.GetLoginedViewName("system/role/role.select.list");
            return View(viewName);
        }

        /// <summary>
        /// 选择一个角色
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectOneRole()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/role/role.SelectOneRole.list");
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
                String sRoleCode = Server.UrlDecode(RequestHelper.GetQueryString("RoleCode"));
                String sRoleName = Server.UrlDecode(RequestHelper.GetQueryString("RoleName"));


                //获取数据
                MDataTable dtInfo = FrameWorkService.Instance()
                    .Role()
                    .GetList(sRoleCode, sRoleName, curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);


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
        /// <param name="roleCode">角色编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String roleCode)
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
                mEntity = FrameWorkService.Instance().Role().InitDataRow();

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().Role().GetEntity(roleCode);
            }

            //
            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/role/role.detail.cshtml");
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
                MDataRow mEntity = FrameWorkService.Instance().Role().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = FrameWorkService.Instance().Role().Insert(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().Role().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = FrameWorkService.Instance().Role().UpdateByRoleCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String sRoleCode = RequestHelper.GetFormString("rolecode");
                exeMsgInfo = FrameWorkService.Instance().Role().DeleteByRoleCode(sRoleCode);
            }

            return Json(exeMsgInfo);

        }





    }
}
