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
using CYQ.Data.Tool;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// <summary>
    /// 2018.10.16 创建 侯鑫辉
    /// 企业用户管理
    /// </summary>
    public class CompanyUserController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "companyuser/companyuser.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 报备客户
        /// </summary>
        /// <returns></returns>
        public ActionResult Poten()
        {
            String viewPath = GlobalAdminViewPath + "companyuser/potential.list.cshtml";
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
            String userName = Server.UrlDecode(RequestHelper.GetQueryString("username"));
            String cellphone = Server.UrlDecode(RequestHelper.GetQueryString("cellphone"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyUser()
                .GetListWithCompanyCode(companyCode, userName, cellphone, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 得到报备客户列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPotentialListData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 20);

            //查询条件
            String potentialusername = RequestHelper.GetQueryString("potentialusername");

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .Potential()
                .GetList(GlobalCompanyCode, potentialusername, curPagerInfo.PageIndex, curPagerInfo.PageSize, "", ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String userCode)
        {
            MDataRow mEntity = new MDataRow();
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyuser/companyuser.detail.cshtml");

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().CompanyUser().GetEntityWithUserCode(userCode);
            }
            else if (docmd.Equals("setrole"))
            {
                String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
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

                //当前用户所拥有的角色
                String userRoleDataList = "";
                MDataTable dtRoleOfUser = DecorationService.Instance().CompanyUser().GetRoleWithUserCode(userCode);
                if (dtRoleOfUser != null && dtRoleOfUser.Rows.Count > 0)
                {
                    foreach (MDataRow dataRow in dtRoleOfUser.Rows) { userRoleDataList += dataRow.Get("rolecode", "").Trim() + "|"; }
                    userRoleDataList = userRoleDataList.Substring(0, userRoleDataList.Length - 1);
                    ViewBag.AllRoleOfUser = userRoleDataList;
                }

                //设置角色
                mEntity = DecorationService.Instance().CompanyUser().GetEntityWithUserCode(userCode);
                viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyuser/companyuser.setrole.cshtml");
            }
            else if (docmd.Equals("viewpotential"))
            {
                viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyuser/potential.detail.view.cshtml");
                string potentialcode = RequestHelper.GetQueryString("potentialcode", "");
                MDataRow potential = DecorationService.Instance().Potential().GetEntityByCode(potentialcode)??new MDataRow();
                ViewBag.potential = potential;
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

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("add"))
            {
                //增加
                String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
                MDataRow mEntity = DecorationService.Instance().CompanyUser().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("usercode", Guid.NewGuid());
                mEntity.Set("companycode", companyCode);

                exeMsgInfo = DecorationService.Instance().CompanyUser().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().CompanyUser().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().CompanyUser().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String userCode = RequestHelper.GetFormString("usercode");
                exeMsgInfo = DecorationService.Instance().CompanyUser().Delete(userCode);
            }
            else if (doCmd.Equals("setrole"))
            {
                String userCode = RequestHelper.GetFormString("usercode");
                String sUserRoleCodes = RequestHelper.GetFormString("userrolecodes");
                exeMsgInfo = DecorationService.Instance().CompanyUser().SetRole(userCode, sUserRoleCodes);
            }

            //返回结果
            return Json(exeMsgInfo);
        }

        /// <summary>
        /// 弹窗选择企业用户
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectCompanyUser()
        {
            String projectCode = RequestHelper.GetQueryString("projectcode");
            ViewBag.curProjectCode = projectCode;
            String viewPath = GlobalAdminViewPath + "companyuser/companyuser.select.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 弹窗选择企业用户（status=1启用）
        /// </summary>
        /// <returns></returns>
        public ActionResult SelCompanyUserListData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 10000);

            //查询条件
            String userName = Server.UrlDecode(RequestHelper.GetQueryString("username"));
            String cellphone = Server.UrlDecode(RequestHelper.GetQueryString("cellphone"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyUser()
                .SelCompanyUserListData(companyCode, userName, cellphone, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            String projectCode = RequestHelper.GetQueryString("projectcode");
            MDataTable userTable = DecorationService.Instance().CompanyUser().GetCompanyUserWithProjectCode(projectCode);


            dtInfo.Columns.Add("LAY_CHECKED", SqlDbType.Bit);
            foreach (MDataRow item in dtInfo.Rows)
            {
                foreach (MDataRow userTableRow in userTable.Rows)
                {
                    if (item.Get("usercode", "") == userTableRow.Get("usercode", ""))
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
        /// 佣金规则列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CommissionRulesIndex()
        {
            String viewPath = GlobalAdminViewPath + "commissionrules/commissionrules.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 佣金规则数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCommissionRulesData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 20);

            //查询条件
            String dicValue = Server.UrlDecode(RequestHelper.GetQueryString("dicvalue"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyUser()
                .GetCommissionRulesData("commissionrules", companyCode, dicValue, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="dicCode"></param>
        /// <returns></returns>
        public ActionResult CommissionRulesDetail(String docmd, String dicCode)
        {
            MDataRow mEntity = new MDataRow();
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "commissionrules/commissionrules.detail.cshtml");

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().CompanyUser().GetEntityWithDicCode(dicCode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            return View(viewName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        public ActionResult CommissionRulesExecute(String doCmd)
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
                String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
                MDataRow mEntity = new MDataRow(DBTool.GetColumns("decoration_commissionrules"));
                mEntity.LoadFrom(true);
                mEntity.Set("diccode", Guid.NewGuid());
                mEntity.Set("companycode", companyCode);

                exeMsgInfo = DecorationService.Instance().CompanyUser().AddCommissionRules(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = new MDataRow(DBTool.GetColumns("decoration_commissionrules"));
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().CompanyUser().UpdateCommissionRules(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String dicCode = RequestHelper.GetFormString("diccode");
                exeMsgInfo = DecorationService.Instance().CompanyUser().DeleteCommissionRules(dicCode);
            }

            //返回结果
            return Json(exeMsgInfo);
        }
    }
}