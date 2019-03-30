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
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// 系统公告
    /// 侯鑫辉
    /// 2018.10.20
    public class SysMsgController : EntBaseController
    {
        /// <summary>
        /// 系统公告列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "sysmsg/sysmsg.index.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 系统公告列表数据
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
            String msgTitle = Server.UrlDecode(RequestHelper.GetQueryString("msgtitle"));
            String msgContent = Server.UrlDecode(RequestHelper.GetQueryString("msgcontent"));
            String msgStatus = Server.UrlDecode(RequestHelper.GetQueryString("msgstatus"));
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .SysMsg()
                .GetListWithCompanyCode(companyCode, msgTitle, msgContent, msgStatus, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="msgCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String msgCode = "")
        {
            MDataRow mEntity = new MDataRow();
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "sysmsg/sysmsg.detail.cshtml");

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().SysMsg().GetEntityWithMsgCode(msgCode);
            }
            else if (docmd.Equals("send"))
            {
                //发送
                //所有的角色
                MDataTable dtRole = DecorationService.Instance().CompanyUser().GetRoleListWithCompanyCode(companyCode);
                String roleCodeStrs = "";
                string roleNameStrs = "";
                if (dtRole != null && dtRole.Rows.Count > 0)
                {
                    foreach (MDataRow dataRow in dtRole.Rows)
                    {
                        roleCodeStrs += dataRow.Get("RoleCode", "").Trim() + "|";
                        roleNameStrs += dataRow.Get("RoleName", "").Trim() + "|";
                    }
                    roleNameStrs = roleNameStrs.Substring(0, roleNameStrs.Length - 1);
                    roleCodeStrs = roleCodeStrs.Substring(0, roleCodeStrs.Length - 1);
                }
                ViewBag.roleCodeStrs = roleCodeStrs;
                ViewBag.roleNameStrs = roleNameStrs;

                mEntity = DecorationService.Instance().SysMsg().GetEntityWithMsgCode(msgCode);
                viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "sysmsg/sysmsg.send.cshtml");
            }
            else if (docmd.Equals("view"))
            {
                //所有的角色
                MDataTable dtRole = DecorationService.Instance().CompanyUser().GetRoleListWithCompanyCode(companyCode);
                String roleCodeStrs = "";
                string roleNameStrs = "";
                if (dtRole != null && dtRole.Rows.Count > 0)
                {
                    foreach (MDataRow dataRow in dtRole.Rows)
                    {
                        roleCodeStrs += dataRow.Get("RoleCode", "").Trim() + "|";
                        roleNameStrs += dataRow.Get("RoleName", "").Trim() + "|";
                    }
                    roleNameStrs = roleNameStrs.Substring(0, roleNameStrs.Length - 1);
                    roleCodeStrs = roleCodeStrs.Substring(0, roleCodeStrs.Length - 1);
                }
                ViewBag.roleCodeStrs = roleCodeStrs;
                ViewBag.roleNameStrs = roleNameStrs;

                //当前消息所发送的角色
                String msgRoleDataList = "";
                MDataTable dtRoleOfMsg = DecorationService.Instance().SysMsg().GetRoleListWithSysCode(msgCode);
                if (dtRoleOfMsg != null && dtRoleOfMsg.Rows.Count > 0)
                {
                    foreach (MDataRow dataRow in dtRoleOfMsg.Rows) { msgRoleDataList += dataRow.Get("rolecode", "").Trim() + "|"; }
                    msgRoleDataList = msgRoleDataList.Substring(0, msgRoleDataList.Length - 1);
                    ViewBag.AllRoleOfMsg = msgRoleDataList;
                }

                mEntity = DecorationService.Instance().SysMsg().GetEntityWithMsgCode(msgCode);
                viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "sysmsg/sysmsg.view.cshtml");
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

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
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("add"))
            {
                MDataRow mEntity = DecorationService.Instance().SysMsg().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("msgcode", Guid.NewGuid().ToString("N"));
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);
                mEntity.Set("msgstatus", 0);
                mEntity.Set("companycode", companyCode);
                exeMsgInfo = DecorationService.Instance().SysMsg().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().SysMsg().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().SysMsg().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String msgCode = RequestHelper.GetFormString("msgcode");
                exeMsgInfo = DecorationService.Instance().SysMsg().Delete(msgCode);
            }
            else if (doCmd.Equals("send"))
            {
                String msgCode = RequestHelper.GetFormString("msgcode");
                String sUserRoleCodes = RequestHelper.GetFormString("userrolecodes");
                exeMsgInfo = DecorationService.Instance().SysMsg().Send(msgCode,sUserRoleCodes);
            }
            return Json(exeMsgInfo);
        }
    }
}