using System;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;
using Decoration.Interface.Entity;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// <summary>
    /// 用户登录页
    /// </summary>
    public class LoginController : EntBaseController
    {
        //
        // GET: /Admin/Login/

        /// <summary>
        /// 默认登录页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(GlobalLoginViewPath + "login.cshtml");
        }

        /// <summary>
        /// 审核（改为uid登录）
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="userPassword">用户密码</param>
        /// <returns></returns>
        public ActionResult Check(String userCode, String userPassword)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                string account = HttpUtility.UrlDecode(userCode);
                userPassword = HttpUtility.UrlDecode(userPassword);
                string loginUserCode = DecorationService.Instance().CompanyUser().CheckLoginWithAccount(account, userPassword);

                if (!String.IsNullOrWhiteSpace(loginUserCode))
                {
                    //得到用户的信息
                    MDataRow drUser = DecorationService.Instance().CompanyUser().GetEntityWithUserCode(loginUserCode, false);
                    if (drUser != null)
                    {
                        CacheHelper.Set(DecorationConstInfo.CompanyLoginUserCodeKeyForCache, drUser);
                        CookieHelper.SetObjByAppKey(DecorationConstInfo.CompanyLoginUserCodeKeyForCookie, loginUserCode);
                        SessionHelper.SetByAppKey(DecorationConstInfo.CompanyLoginInfoKeyForSession, drUser);
                    }
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "登录验证成功";
                }
                else
                {
                    throw new Exception("验证失败");
                }
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return Json(exeMsgInfo);
        }

        /// <summary>
        /// 根据gridCode生成标准文件
        /// </summary>
        /// <param name="gridCode"></param>
        /// <returns></returns>
        public ActionResult WriteGrid(String gridCode)
        {
            String retValue = "";

            //StringBuilder sbTable = new StringBuilder();

            //StringBuilder sbColumnGroup = new StringBuilder();
            //StringBuilder sbColumnHeader = new StringBuilder();
            //StringBuilder sbData = new StringBuilder();

            //MDataTable dtColumnInfo = FrameWorkService.Instance().GridColumn().GetAll(gridCode);

            ////表

            //if (dtColumnInfo != null && dtColumnInfo.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dtColumnInfo.Rows.Count; i++)
            //    {
            //        MDataRow drInfo = dtColumnInfo.Rows[i];

            //        String colWidth = drInfo.Get("GridColumnWidth", "");
            //        String colCaption = drInfo.Get("GridColumnCaption", "");
            //        String colFieldName = drInfo.Get("GridColumnName", "");

            //        if (i < dtColumnInfo.Rows.Count)
            //        {
            //            //列宽度
            //            sbColumnGroup.Append("<col width=\"" + colWidth + "\">");

            //            //字段列表
            //            sbColumnHeader.Append("<th>" + colCaption + "</th>");

            //            //字段内容
            //            sbData.Append("<td>" + "@dataRow.Get(\"" + colFieldName + "\",\"\")" + "</td>");

            //        }
            //    }
            //}
            //String sTplFile = Server.MapPath("/list-1.txt");
            //String fileContent = FileHelper.Read(sTplFile, "utf-8");

            //fileContent = fileContent.Replace("<!--ColGroup-->", sbColumnGroup.ToString());
            //fileContent = fileContent.Replace("<!--ColCaption-->", sbColumnHeader.ToString());
            //fileContent = fileContent.Replace("<!--ColRolwData-->", sbData.ToString());

            //String sTargetFile = Server.MapPath("/views/shared/theme/admin/t3/modulecache/" + gridCode + ".cshtml");
            //FileHelper.Write(fileContent, sTargetFile);


            return Content("写入成功");
        }



    }
}
