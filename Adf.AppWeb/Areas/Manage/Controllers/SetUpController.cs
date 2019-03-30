using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    /// <summary>
    /// 程序安装控制器
    /// </summary>
    public class SetUpController : BaseController
    {
        /// <summary>
        /// 模块首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            /*获取数据*/
            MDataRow drEntity = FrameWorkService.Instance().AppInfo().GetEntity("02");

            /*传参*/
            ViewBag.DrMainEntity = drEntity;
            ViewBag.CurrentCmd = "modify";

            /*视图*/
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/setup/setup.detail.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Execute(String doCmd)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();
            MDataRow mEntity = null;
            if (doCmd.Equals("modify"))
            {
                mEntity = FrameWorkService.Instance().AppInfo().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = FrameWorkService.Instance().AppInfo().UpdateByAppCode(mEntity);
            }
            

            return Json(exeMsgInfo);
        }

    }
}
