using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;
using Decoration.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    /// <summary>
    /// 功能：平台角色
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public class DecorationSysRoleMngController : BaseController
    {
        /// <summary>
        /// 功能：平台角色首页
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "decoration/decorationsysrole/decorationsysrole.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 功能：平台角色首页json数据
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
            String roleName = Server.UrlDecode(RequestHelper.GetQueryString("roleName"));
            //其他条件（排序）
            String sOrderBy = "";
         
            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .DecorationSysRole()
                .GetList(roleName, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

         
            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;
           
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 功能：平台角色首页详细页
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="docmd">操作编码</param>
        /// <param name="rolecode">主键</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String rolecode)
        {
            MDataRow mEntity = new MDataRow();

            #region 可提醒角色数据

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
            MDataTable sRoledtTips = DecorationService.Instance().DecorationSysRole().GetTipRoleTableByRoleCode(rolecode);
            List<string> valSelFrontModule3 = new List<string>();
            foreach (MDataRow mDataRow in sRoledtTips.Rows)
            {
                valSelFrontModule3.Add(mDataRow.Get("tiprolecode", ""));
            }

            #endregion 可提醒角色数据结束

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().DecorationSysRole().InitDataRow();

            }
            else if (docmd.Equals("modify"))
            {
                mEntity = DecorationService.Instance().DecorationSysRole().GetEntityWithRoleCode(rolecode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            ViewBag.valSelRoles = string.Join("|", valSelFrontModule3);
            ViewBag.dtRoles = dtRoles;
            ViewBag.txtRoles = string.Join("|", txtRoles);
            ViewBag.valRoles = string.Join("|", valRoles);

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "decoration/decorationsysrole/decorationsysrole.detail.cshtml");
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
                MDataRow mEntity = DecorationService.Instance().DecorationSysRole().InitDataRow();
                mEntity.LoadFrom(true);          
                string tiproles = RequestHelper.GetFormString("tiproles", "");
                exeMsgInfo = DecorationService.Instance().DecorationSysRole().Add(mEntity, tiproles);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().DecorationSysRole().InitDataRow();
                mEntity.LoadFrom(true);
                string tiproles = RequestHelper.GetFormString("tiproles", "");
                exeMsgInfo = DecorationService.Instance().DecorationSysRole().Update(mEntity, tiproles);
            }
            else if (doCmd.Equals("delete"))
            {
                String rolecode = RequestHelper.GetFormString("rolecode");
                exeMsgInfo = DecorationService.Instance().DecorationSysRole().Delete(rolecode);
            }

            //返回结果
            return Json(exeMsgInfo);
        }
    }
}