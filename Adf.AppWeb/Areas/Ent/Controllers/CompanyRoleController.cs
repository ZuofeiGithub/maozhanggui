using System;
using System.Collections.Generic;
using System.Data;
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
    /// <summary>
    /// 2018.10.17 创建 王浩
    /// 企业人员角色
    /// </summary>
    public class CompanyRoleController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "companyrole/companyrole.list.cshtml";
            return View(viewPath);
        }


        /// <summary>
        /// 得到列表数据
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
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            string taskcode = RequestHelper.GetQueryString("taskcode");

            MDataTable task = DecorationService.Instance().Task().GetAllRole(taskcode);

       

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyRole()
                .GetList(roleName, GlobalCompanyCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            dtInfo.Columns.Add("LAY_CHECKED", SqlDbType.Bit);
            foreach (MDataRow item in dtInfo.Rows)
            {
                foreach (MDataRow userTableRow in task.Rows)
                {
                    if (item.Get("rolecode", "") == userTableRow.Get("rolecode", ""))
                    {
                        item.Set("LAY_CHECKED", true, 2);
                    }
                }
            }

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;
            jsonInfo = jsonInfo.Replace("lay_checked", "LAY_CHECKED");

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 弹窗任务阶段数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {



            String viewPath = GlobalAdminViewPath + "companyrole/companyrole.select.cshtml";
            return View(viewPath);
        }


        /// <summary>
        ///企业用户弹窗选择
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectList()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 999999);

            //查询条件
            String roleName = Server.UrlDecode(RequestHelper.GetQueryString("roleName"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));
            string taskcode = RequestHelper.GetQueryString("taskcode");
            string casecode = RequestHelper.GetQueryString("casecode");

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyRole()
                .GetList(roleName, GlobalCompanyCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);
            if (casecode != "")
            {
                MDataTable task = DecorationService.Instance().CaseRole().GetAllRole(casecode);
                dtInfo.Columns.Add("LAY_CHECKED", SqlDbType.Bit);
                foreach (MDataRow item in dtInfo.Rows)
                {
                    foreach (MDataRow userTableRow in task.Rows)
                    {
                        if (item.Get("rolecode", "") == userTableRow.Get("rolecode", ""))
                        {
                            item.Set("LAY_CHECKED", true, 2);
                        }
                    }
                }
            }
            else
            {
                MDataTable dtTable = DecorationService.Instance().Task().GetAllRole(taskcode);
                dtInfo.Columns.Add("LAY_CHECKED", SqlDbType.Bit);
                foreach (MDataRow item in dtInfo.Rows)
                {
                    foreach (MDataRow userTableRow in dtTable.Rows)
                    {
                        if (item.Get("rolecode", "") == userTableRow.Get("rolecode", ""))
                        {
                            item.Set("LAY_CHECKED", true, 2);
                        }
                    }
                }
            }
           

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;
            jsonInfo = jsonInfo.Replace("lay_checked", "LAY_CHECKED");

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }


        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="rolecode">轮转图编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String rolecode)
        {
            MDataRow mEntity = new MDataRow();


            MDataTable dataTable = DecorationService.Instance().CompanyRole().GetFrontModuleCode();

            String roleCodeStrs = "";
            string roleNameStrs = "";

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (MDataRow dataRow in dataTable.Rows)
                {
                    roleCodeStrs += dataRow.Get("diccode", "").Trim() + "|";
                    roleNameStrs += dataRow.Get("dickey", "").Trim() + "|";
                }
                roleNameStrs = roleNameStrs.Substring(0, roleNameStrs.Length - 1);
                roleCodeStrs = roleCodeStrs.Substring(0, roleCodeStrs.Length - 1);
            }
            ViewBag.roleCodeStrs = roleCodeStrs;
            ViewBag.roleNameStrs = roleNameStrs;

            #region 前端授权模块/授权状态/可提醒角色

            MDataTable dtFrontModule1 = DecorationService.Instance().FrontModule().GetFrontModuleForProject();
            MDataTable dtFrontModule2 = DecorationService.Instance().FrontModule().GetFrontModuleForProjectStatus();
            MDataTable dtCompanyRoles = DecorationService.Instance().CompanyRole().GetCompanyRole(GlobalCompanyCode);

            List<string> txtFrontModule1 = new List<string>();
            List<string> valFrontModule1 = new List<string>();
            foreach (MDataRow item in dtFrontModule1.Rows)
            {
                string txt = item.Get("frontmodulename", "");
                string val = item.Get("frontmodulecode", "");
                txtFrontModule1.Add(txt);
                valFrontModule1.Add(val);
            }
            List<string> txtFrontModule2 = new List<string>();
            List<string> valFrontModule2 = new List<string>();
            foreach (MDataRow item in dtFrontModule2.Rows)
            {
                string txt = item.Get("frontmodulename", "");
                string val = item.Get("frontmodulecode", "");
                txtFrontModule2.Add(txt);
                valFrontModule2.Add(val);
            }

            List<string> txtCompanyRoles = new List<string>();
            List<string> valCompanyRoles = new List<string>();
            foreach (MDataRow item in dtCompanyRoles.Rows)
            {
                string txt = item.Get("rolename", "");
                string val = item.Get("rolecode", "");
                txtCompanyRoles.Add(txt);
                valCompanyRoles.Add(val);
            }

            ViewBag.dtFrontModule1 = dtFrontModule1;
            ViewBag.txtFrontModule1 = string.Join("|", txtFrontModule1);
            ViewBag.valFrontModule1 = string.Join("|", valFrontModule1);

            ViewBag.dtFrontModule2 = dtFrontModule2;
            ViewBag.txtFrontModule2 = string.Join("|", txtFrontModule2);
            ViewBag.valFrontModule2 = string.Join("|", valFrontModule2);

            ViewBag.dtCompanyRoles = dtCompanyRoles;
            ViewBag.txtCompanyRoles = string.Join("|", txtCompanyRoles);
            ViewBag.valCompanyRoles = string.Join("|", valCompanyRoles);


            //取有选中的值（3项）
            //ViewBag.selValCompanyRoles

            MDataTable sRoledtFrontModule1 = DecorationService.Instance().FrontModule().GetFrontModuleForProject(rolecode);
            MDataTable sRoledtFrontModule2 = DecorationService.Instance().FrontModule().GetFrontModuleForProjectStatus(rolecode);
            MDataTable sRoledtTips = DecorationService.Instance().Tip().GetTipRole(rolecode);

            List<string> valSelFrontModule1 = new List<string>();
            foreach (MDataRow mDataRow in sRoledtFrontModule1.Rows)
            {
                valSelFrontModule1.Add(mDataRow.Get("frontmodulecode", ""));
            }
            List<string> valSelFrontModule2 = new List<string>();

            foreach (MDataRow mDataRow in sRoledtFrontModule2.Rows)
            {
                valSelFrontModule2.Add(mDataRow.Get("frontmodulecode", ""));
            }
            List<string> valSelFrontModule3 = new List<string>();
            foreach (MDataRow mDataRow in sRoledtTips.Rows)
            {
                valSelFrontModule3.Add(mDataRow.Get("tiprolecode", ""));
            }

            #endregion
            ViewBag.valSelFrontModule1 = string.Join("|", valSelFrontModule1);
            ViewBag.valSelFrontModule2 = string.Join("|", valSelFrontModule2);
            ViewBag.valSelFrontModule3 = string.Join("|", valSelFrontModule3);


            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().CompanyRole().InitDataRow();
               
            }
            else if (docmd.Equals("modify"))
            {
                //修改

                //当前用户所拥有的角色
                string wst = "";
                MDataRow dtFrontModuleCode = DecorationService.Instance().CompanyRole().FrontModuleCode(rolecode);
                string frontmodulecode = dtFrontModuleCode.Get("frontmodulecode", "");
                if (frontmodulecode!="")
                {
                    string[] userRoleDataList = frontmodulecode.Split(',');

                    for (int i = 0; i < userRoleDataList.Length; i++)
                    {

                        wst += frontmodulecode.Split(',')[i].Trim() + "|";
                        
                    }
                    wst = wst.Substring(0, wst.Length - 1);
                }

                ViewBag.AllRoleOfUser = wst;
                mEntity = DecorationService.Instance().CompanyRole().GetEntityWithRoleCode(rolecode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            ViewBag.dataTable = dataTable;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyrole/companyrole.detail.cshtml");
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
                //增加
                MDataRow mEntity = DecorationService.Instance().CompanyRole().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("rolecode", Guid.NewGuid());
                mEntity.Set("companycode", GlobalCompanyCode);
                string frontmodule1 = RequestHelper.GetFormString("frontmodule1", "");
                string frontmodule2 = RequestHelper.GetFormString("frontmodule2", "");
                string tiproles = RequestHelper.GetFormString("tiproles", "");
                exeMsgInfo = DecorationService.Instance().CompanyRole().Add(mEntity, frontmodule1, frontmodule2,tiproles);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().CompanyRole().InitDataRow();
                mEntity.LoadFrom(true);
                string frontmodule1 = RequestHelper.GetFormString("frontmodule1", "");
                string frontmodule2 = RequestHelper.GetFormString("frontmodule2", "");
                string tiproles = RequestHelper.GetFormString("tiproles", "");
                exeMsgInfo = DecorationService.Instance().CompanyRole().Update(mEntity,frontmodule1,frontmodule2,tiproles);
            }
            else if (doCmd.Equals("delete"))
            {
                String rolecode = RequestHelper.GetFormString("rolecode");
                exeMsgInfo = DecorationService.Instance().CompanyRole().Delete(rolecode);
            }

            //返回结果
            return Json(exeMsgInfo);
        }
    }
}