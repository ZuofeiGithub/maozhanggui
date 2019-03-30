using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;
using Decoration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Adf.AppWeb.Areas.Manage.Controllers
{

    /// <summary>
    /// 功能：平台施工任务
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public class DecorationSysTaskMngController : BaseController
    {
        /// <summary>
        /// 功能：平台施工任务首页
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            //模板数据
            MDataTable curTemplateTable = DecorationService.Instance().DecorationSysTemplate().GetTemplateTable();


            ViewBag.curTemplateTable = curTemplateTable;
            String viewPath = GlobalAdminViewPath + "decoration/decorationsystask/decorationsystask.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 功能：平台施工任务首页json数据
        /// 创建时间：2018.11.13
        /// 创建人：张硕
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
            String catename = Server.UrlDecode(RequestHelper.GetQueryString("catename"));
            String templatecode = RequestHelper.GetQueryString("templatecode", "");
            String catecode = RequestHelper.GetQueryString("catecode", "");
            //其他条件（排序）
            String sOrderBy = "";

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .DecorationSysTask()
                .GetList(catename, catecode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);


            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 功能：平台施工任务详细页
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="docmd">操作编码</param>
        /// <param name="taskcode">主键</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String taskcode)
        {
            MDataRow mEntity = new MDataRow();

            #region 平台任务角色角色数据

            //所有数据
            MDataTable dtRoles = DecorationService.Instance().DecorationSysRole().GetRoleTable();
            List<string> txtRoles = new List<string>();//显示数据
            List<string> valRoles = new List<string>();//值
            foreach (MDataRow item in dtRoles.Rows)
            {
                string txt = item.Get("rolename", "");
                string val = item.Get("rolecode", "");
                txtRoles.Add(txt);
                valRoles.Add(val);
            }

            //选中数据
            MDataTable sRoledtTips = DecorationService.Instance().DecorationSysTask().GetRoleTableByTaskCode(taskcode);
            List<string> valSelFrontModule3 = new List<string>();
            foreach (MDataRow mDataRow in sRoledtTips.Rows)
            {
                valSelFrontModule3.Add(mDataRow.Get("rolecode", ""));
            }

            #endregion 平台任务角色数据结束

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().DecorationSysTask().InitDataRow();

            }
            else if (docmd.Equals("modify"))
            {
                mEntity = DecorationService.Instance().DecorationSysTask().GetEntityWithTaskCode(taskcode);
            }

            //模板数据
            MDataTable curTemplateTable = DecorationService.Instance().DecorationSysTemplate().GetTemplateTable();
            //模板数据
            MDataTable curCateTable = DecorationService.Instance().DecorationSysTaskCate().GetTable();


            ViewBag.curCateTable = curCateTable;
            ViewBag.curTemplateTable = curTemplateTable;
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            ViewBag.valSelRoles = string.Join("|", valSelFrontModule3);
            ViewBag.dtRoles = dtRoles;
            ViewBag.txtRoles = string.Join("|", txtRoles);
            ViewBag.valRoles = string.Join("|", valRoles);

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "decoration/decorationsystask/decorationsystask.detail.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 功能：执行
        /// 创建时间：2018.11.13
        /// 创建人：张硕
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
                MDataRow mEntity = DecorationService.Instance().DecorationSysTask().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("taskcode", Guid.NewGuid());
                string tiproles = RequestHelper.GetFormString("tiproles", "");
                exeMsgInfo = DecorationService.Instance().DecorationSysTask().Add(mEntity, tiproles);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().DecorationSysTask().InitDataRow();
                mEntity.LoadFrom(true);
            
                string tiproles = RequestHelper.GetFormString("tiproles", "");
                exeMsgInfo = DecorationService.Instance().DecorationSysTask().Update(mEntity, tiproles);
            }
            else if (doCmd.Equals("delete"))
            {
                String taskcode = RequestHelper.GetFormString("taskcode");
                exeMsgInfo = DecorationService.Instance().DecorationSysTask().Delete(taskcode);
            }
            else if (doCmd.Equals("gettaskcate"))
            {
                String templatecode = RequestHelper.GetFormString("templatecode");
                MDataTable curTable = DecorationService.Instance().DecorationSysTaskCate().GetTableByTemplateCode(templatecode);
                return Content(curTable.ToJson(), "text/json", Encoding.UTF8);
            }
            //返回结果
            return Json(exeMsgInfo);
        }
    }
}