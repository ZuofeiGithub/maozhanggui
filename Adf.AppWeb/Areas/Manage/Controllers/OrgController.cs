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
    public class OrgController : BaseController
    {
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/Org/Org.index.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData(String doCmd = "list",String orgCode="")
        {
            if (String.IsNullOrEmpty(orgCode))
            {
                orgCode = "root";
            }
            MDataTable mDataTable = FrameWorkService.Instance().Org().GetChildAll(orgCode, false);

            String curJson = mDataTable.ToJson(false, false, RowOp.None, true);

            //ztree树状控件 需要isParent来指明当前的节点为父结点
            curJson = StringHelper.ReplaceAll(curJson, "isparent", "isParent");

            return Content(curJson, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 得到当前机构下级所有机构列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListData(String parentCode)
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 20);
            String orgCode = RequestHelper.GetQueryString("orgcode");
            String sOrderBy = RequestHelper.GetQueryString("OrderBy");

            //
            if (String.IsNullOrEmpty(parentCode))
            {
                parentCode = "root";
            }

            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .Org()
                .GetList(parentCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="orgCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, string orgCode)
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
                String sParentCode = RequestHelper.GetQueryString("ParentCode");
                
                //增加          
                mEntity = FrameWorkService.Instance().Org().InitDataRow();
                mEntity.Set("parentcode", sParentCode);
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().Org().GetEntityByOrgCode(orgCode);
            }

            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/Org/Org.detail.cshtml");
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
                MDataRow mEntity = FrameWorkService.Instance().Org().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = FrameWorkService.Instance().Org().Insert(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().Org().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = FrameWorkService.Instance().Org().UpdateByOrgCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                string orgCode = RequestHelper.GetFormString("orgcode");
                exeMsgInfo = FrameWorkService.Instance().Org().DeleteByOrgCode(orgCode);
            }

            return Json(exeMsgInfo);
        }

        //选择一个机构
        public ActionResult Select()
        {
            String dataKey = RequestHelper.GetQueryString("DataKey");
            ViewBag.DataKey = dataKey;
            String viewName = FrameWorkService.GetLoginedViewName("system/Org/Org.select.index");
            return View(viewName);
        }

        /// <summary>
        /// 得到一个机构，为选择控制
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectOneOrg()
        {
            /*得到数据*/
            MDataTable dtAllOrg = FrameWorkService.Instance().Org().GetAll();

            /*视图传参*/
            ViewBag.DtAllOrg = dtAllOrg;

            /*视图*/
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/Org/Org.SelectOneOrg.index.cshtml");
            return View(viewName);
        }
    }
}
