using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Db;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using CYQ.Data;
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// <summary>
    /// 2018.10.16 创建 王浩
    /// 企业用户管理
    /// </summary>
    public class AdminCompanyUserController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "companyuser/companyuser.index.cshtml";
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

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .AdminCompanyUser()
                .GetList(userName, GlobalCompanyCode, cellphone ,curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

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

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().Company().InitDataRow();
                mEntity.Set("usercode", Guid.NewGuid().ToString("N"));
              
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().AdminCompanyUser().GetEntityWithUserCode(userCode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyuser/companyuser.Newdetail.cshtml");
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
                MDataRow mEntity = DecorationService.Instance().AdminCompanyUser().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("usercode", Guid.NewGuid());
                mEntity.Set("companycode", GlobalCompanyCode);
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
            else if (doCmd.Equals("UpdateWithAdmin"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().AdminCompanyUser().InitDataRow();
                mEntity.LoadFrom(true);
               
                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().UpdateWithAdmin(mEntity);
            }
            
            else if (doCmd.Equals("delete"))
            {
                String userCode = RequestHelper.GetFormString("usercode");
                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().Delete(userCode);
            }
            
            else if (doCmd.Equals("changes"))
            {
                String userCode = RequestHelper.GetFormString("usercode");
                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().Changes(userCode);
            }
            else if (doCmd.Equals("status"))
            {
                String userCode = RequestHelper.GetFormString("usercode");
                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().Status(userCode);
            }
            else if (doCmd.Equals("modifypwd"))
            {
                //修改密码保存
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
                exeMsgInfo = DecorationService.Instance().AdminCompanyUser().UpdateWithAdminPwd(sUserCode, olduserpassword, userpassword);
            }

            //返回结果
            return Json(exeMsgInfo);
        }




        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPwd()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companyuser/user.modifypwd.detail");
            return View(viewName);
        }


        /// <summary>
        /// 我的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyInfo()
        {

            //所有的角色(基础类库无效)
            //MDataTable dtRole = FrameWorkService.Instance().UserRole().GetAllOfUser(GlobalUserCode);
            string sql = @"
select rolename FROM adf_role as t1
inner join adf_userrole as t2 on t1.rolecode = t2.rolecode
where t2.usercode=@usercode
";
            sql = sql.Replace("@usercode", DbService.SetQuotesValue(GlobalUserCode));
            MDataTable dtRole = new MDataTable();
            using (MAction action = new MAction(sql))
            {
                dtRole = action.Select();
            }

            List<string> list = new List<string>();
            if (dtRole != null && dtRole.Rows.Count > 0)
            {
                foreach (MDataRow dataRow in dtRole.Rows)
                {
                    string roleName = dataRow.Get("RoleName", "");
                    if (!String.IsNullOrEmpty(roleName))
                    {
                        list.Add(roleName);
                    }
                }
            }
            string roleNameStrs = string.Join(",", list.ToArray());
            ViewBag.roleNameStrs = roleNameStrs;

            //修改
       

            MDataRow dr = DecorationService.Instance().AdminCompanyUser().GetAdminInfo(GlobalUserCode);
            ViewBag.dr = dr;
            String viewPath = GlobalAdminViewPath + "companyuser/User.MyInfo.detail.cshtml";
            return View(viewPath);
        }
    }
}