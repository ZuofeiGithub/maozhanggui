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
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// <summary>
    /// 企业信息
    /// 王浩
    /// 2018-10-16
    /// </summary>
    public class CompanyController : EntBaseController
    {
        /// <summary>
        /// 企业信息首页
        /// 王浩
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = GlobalAdminViewPath + "company/company.list.cshtml";
            return View(viewName);
        }

        /// <summary>
        /// 当前登录用户的企业信息
        /// 王浩
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ActionResult UserWithCompany()
        {
            MDataRow DrMainEntity = DecorationService.Instance().Company().GetCompanyWithUserCode(GlobalUserCode);
            ViewBag.DrMainEntity = DrMainEntity;
            String viewName = GlobalAdminViewPath + "company/company.index.cshtml";
            return View(viewName);
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 20);
            String companyName = Server.UrlDecode(RequestHelper.GetQueryString("companyName"));
            String sOrderBy = "";
            //获取数据
            MDataTable dtInfo = DecorationService.Instance().Company().GetList(companyName, GlobalCompanyCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 获取当前用户的企业信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDetailed()
        {

            string usercode = GlobalUserCode;
            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .Company().GetCompanyWithUserCode(usercode);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.DtData = dtInfo;
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "ent/company/company.index.cshtml");
            return View(viewName);
        }


        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String companyCode)
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
                mEntity = DecorationService.Instance().Company().InitDataRow();
                mEntity.Set("companyCode", Guid.NewGuid().ToString("N"));
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);
                mEntity.Set("companystatus", 0);
                mEntity.Set("parentcode", GlobalCompanyCode);
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().Company().GetEntityWithCompanyCode(companyCode);
            }
            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "company/company.detail.cshtml");

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
                MDataRow mEntity = DecorationService.Instance().Company().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().Company().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().Company().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().Company().Update(mEntity);
            }
            else if (doCmd.Equals("update"))
            {
                MDataRow mEntity = DecorationService.Instance().Company().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().Company().UpdateWithAdmin(mEntity);
            }

            else if (doCmd.Equals("delete"))
            {
                String companyCode = RequestHelper.GetFormString("companyCode");
                exeMsgInfo = DecorationService.Instance().Company().Delete(companyCode);
            }
           
            else if (doCmd.Equals("enable"))//启用操作
            {

                String companyCode = RequestHelper.GetFormString("companyCode");
                exeMsgInfo = DecorationService.Instance().Company().Enable(companyCode);
            }
            else if (doCmd.Equals("restart"))
            {
                String companyCode = RequestHelper.GetFormString("companycode");
                exeMsgInfo = DecorationService.Instance().Company().Restart(companyCode);
            }
            return Json(exeMsgInfo);

        }

        #region 管理员账号 管理企业 2018.10.20 侯鑫辉

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminIndex()
        {
            String viewName = GlobalAdminViewPath + "companyadmin/companyadmin.list.cshtml";
            return View(viewName);
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminGetList()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 20);

            String companyName = Server.UrlDecode(RequestHelper.GetQueryString("companyname"));
            String sOrderBy = "";

            //获取数据
            MDataTable dtInfo = DecorationService.Instance().Company().AdminGetList(companyName, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

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
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public ActionResult AdminDetail(String docmd, String companyCode)
        {
            MDataRow mEntity = new MDataRow();
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyadmin/companyadmin.detail.cshtml");

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().Company().AdminGetEntityWithCompanyCode(companyCode);
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
        public ActionResult AdminExecute(String doCmd)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("add"))
            {
                MDataRow mEntity = DecorationService.Instance().Company().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("companycode", Guid.NewGuid().ToString("N"));
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);
                mEntity.Set("parentcode", "");
                exeMsgInfo = DecorationService.Instance().Company().AdminAdd(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().Company().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().Company().AdminUpdate(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String companyCode = RequestHelper.GetFormString("companycode");
                exeMsgInfo = DecorationService.Instance().Company().AdminDelete(companyCode);
            }

            return Json(exeMsgInfo);

        }

        ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// 管理员添加用户首页
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminAddCompanyUserIndex()
        {
            String companyCode = RequestHelper.GetQueryString("companycode");
            ViewBag.curCompanyCode = companyCode;
            String viewName = GlobalAdminViewPath + "companyadmin/adminaddcompanyuser.list.cshtml";
            return View(viewName);
        }

        /// <summary>
        /// 得到列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminGetCompanyUserList()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 20);

            //查询条件
            String companyCode = RequestHelper.GetQueryString("companycode");
            String userName = Server.UrlDecode(RequestHelper.GetQueryString("username"));
            String cellphone = Server.UrlDecode(RequestHelper.GetQueryString("cellphone"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .AdminCompanyUser()
                .GetList(userName, companyCode, cellphone, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

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
        public ActionResult AdminAddCompanyUserDetail(String docmd, String userCode)
        {
            MDataRow mEntity = new MDataRow();
            String companyCode = RequestHelper.GetQueryString("companycode");

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().AdminCompanyUser().GetEntityWithUserCode(userCode);
            }

            ViewBag.curCompanyCode = companyCode;
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyadmin/adminaddcompanyuser.detail.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        public ActionResult AdminAddCompanyUserExecute(String doCmd)
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
                MDataRow mEntity = DecorationService.Instance().AdminCompanyUser().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("usercode", Guid.NewGuid());
                string pwd = mEntity.Get("userpassword", "");
                pwd = pwd.Trim();
                if (pwd.Length < 6)
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "请输入6位以上密码.";
                    return Json(exeMsgInfo, JsonRequestBehavior.AllowGet);
                }
                mEntity.Set("userpassword", Md5Helper.ToMd5(pwd));
                mEntity.Set("isadmin", 1);

                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().AdminCompanyUser().InitDataRow();
                mEntity.LoadFrom(true);
                string pwd = mEntity.Get("userpassword", "");
                pwd = pwd.Trim();
                if (pwd.Length < 6)
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "请输入6位以上密码.";
                    return Json(exeMsgInfo, JsonRequestBehavior.AllowGet);
                }
                mEntity.Set("userpassword", Md5Helper.ToMd5(pwd));

                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String userCode = RequestHelper.GetFormString("usercode");
                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().Delete(userCode);
            }

            //返回结果
            return Json(exeMsgInfo);
        }

        #endregion

    }
}
