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
    public class UserController : BaseController
    {
        /// <summary>
        /// 得到用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListData()
        {
            //分页
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 20);

            //条件

            //查询条件
            String sUserCode = Server.UrlDecode(RequestHelper.GetQueryString("UserCode"));
            String sUserName = Server.UrlDecode(RequestHelper.GetQueryString("UserName"));
            String sTelphone = Server.UrlDecode(RequestHelper.GetQueryString("Telphone"));
            String sCellPhone = Server.UrlDecode(RequestHelper.GetQueryString("CellPhone"));
            String sIdCard = Server.UrlDecode(RequestHelper.GetQueryString("IdCard"));
            String sEmail = Server.UrlDecode(RequestHelper.GetQueryString("Email"));

            //其他条件
            String sOpenCondition = RequestHelper.GetQueryString("OpenCondition");
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));


            Dictionary<String, String> dicData = new Dictionary<string, string>();
            dicData.Add("OpenCondition", sOpenCondition);
            dicData.Add("OrderBy", sOrderBy);

            dicData.Add("UserCode", sUserCode);
            dicData.Add("UserName", sUserName);
            dicData.Add("Telphone", sTelphone);



            ViewBag.DicData = dicData;
            //


            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .User()
                .GetList(sUserCode, sUserName, sTelphone, sCellPhone, sIdCard, curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;


            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 模块首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/User/User.list.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/User/User.Select.list.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="userCode">角色编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String userCode)
        {
            MDataRow mEntity = new MDataRow();

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            //所有的角色
            MDataTable dtRole = FrameWorkService.Instance().Role().GetAll();

            String roleDataList = "[]";
            String roleCodeStrs = "";
            string roleNameStrs = "";

            if (dtRole != null && dtRole.Rows.Count > 0)
            {
                //roleDataList = dtRole.ToJson(false, false);
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




            if (docmd.Equals("add"))
            {


                //增加          
                mEntity = FrameWorkService.Instance().User().InitDataRow();

            }
            else if (docmd.Equals("modify"))
            {
                //当前用户所拥有的角色
                String userRoleDataList = "";
                MDataTable dtRoleOfUser = FrameWorkService.Instance().UserRole().GetAllOfUser(userCode);
                if (dtRoleOfUser != null && dtRoleOfUser.Rows.Count > 0)
                {
                    foreach (MDataRow dataRow in dtRoleOfUser.Rows)
                    {
                        userRoleDataList += dataRow.Get("RoleCode", "").Trim() + "|";
                    }

                    userRoleDataList = userRoleDataList.Substring(0, userRoleDataList.Length - 1);

                    ViewBag.AllRoleOfUser = userRoleDataList;
                }

                //修改
                mEntity = FrameWorkService.Instance().User().GetEntity(userCode);
            }

            //
            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/User/User.detail.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        public ActionResult Execute(String doCmd, String temp)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("add"))
            {
                MDataRow mEntity = FrameWorkService.Instance().User().InitDataRow();
                mEntity.LoadFrom(true);


                //用户资料更新后，更新当前用户的角色
                String sUserRoleCodes = RequestHelper.GetFormString("userrolecodes");
                String[] arrRole = null;
                if (!String.IsNullOrEmpty(sUserRoleCodes))
                {
                    arrRole = sUserRoleCodes.Split(',');
                }
                exeMsgInfo = FrameWorkService.Instance().User().InsertWithRoles(mEntity, arrRole);

                

            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().User().InitDataRow();
                mEntity.LoadFrom(true);
                //用户资料更新后，更新当前用户的角色
                String sUserRoleCodes = RequestHelper.GetFormString("userrolecodes");
                String[] arrRole = null;
                if (!String.IsNullOrEmpty(sUserRoleCodes))
                {
                    arrRole = sUserRoleCodes.Split(',');
                }
                exeMsgInfo = FrameWorkService.Instance().User().UpdateWithRoles(mEntity, arrRole);


            }
            else if (doCmd.Equals("delete"))
            {
                String sUserCode = RequestHelper.GetFormString("UserCode");
                exeMsgInfo = FrameWorkService.Instance().User().DeleteByUserCode(sUserCode);
            }
            else if (doCmd.Equals("modifypwd"))
            {
                String sUserCode = GlobalUserCode;
                String olduserpassword = RequestHelper.GetFormString("olduserpassword");


                String userpassword = RequestHelper.GetFormString("userpassword");
                String reuserpassword = RequestHelper.GetFormString("reuserpassword");

                if (userpassword != reuserpassword)
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "两次新密码输入不正确";
                    return Json(exeMsgInfo);
                }

                exeMsgInfo = FrameWorkService.Instance().User().ModifyPwdByLoginUser(sUserCode, olduserpassword, userpassword);
            } else if (doCmd.Equals("myinfo"))
            {
                MDataRow mEntity = FrameWorkService.Instance().User().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = FrameWorkService.Instance().User().ModifyInfoByUser(mEntity);
            }

            return Json(exeMsgInfo);

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPwd()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/User/User.modifypwd.detail");
            return View(viewName);
        }


        /// <summary>
        /// 我的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyInfo()
        {

            //所有的角色
            MDataTable dtRole = FrameWorkService.Instance().UserRole().GetAllOfUser(GlobalUserCode);

            string roleNameStrs = "";

            if (dtRole != null && dtRole.Rows.Count > 0)
            {
                //roleDataList = dtRole.ToJson(false, false);
                foreach (MDataRow dataRow in dtRole.Rows)
                {
                    roleNameStrs += dataRow.Get("RoleName", "").Trim() + "|";
                }

                roleNameStrs = roleNameStrs.Substring(0, roleNameStrs.Length - 1);

            }
            ViewBag.roleNameStrs = roleNameStrs;

            

            //修改
            MDataRow mEntity = FrameWorkService.Instance().User().GetEntity(GlobalUserCode);
            ViewBag.DrMainEntity = mEntity;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/User/User.MyInfo.detail");
            return View(viewName);
        }
    }
}
