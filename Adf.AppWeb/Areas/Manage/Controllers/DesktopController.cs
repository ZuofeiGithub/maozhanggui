using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Db;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;


namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class DesktopController : BaseController
    {
        //
        // GET: /Manage/Desktop/

        public ActionResult Index()
        {
            //得到桌面模块,桌面模块的顶级地址是
            //001204002

            //FrameWorkService.Instance().Module().GetChildAll("001204002");

            //一级桌面（统计型）
            MDataTable dtInfo1 = FrameWorkService.Instance().RoleModule().GetSubAllModuleOfUser(GlobalUserCode, "001204002001", false);
            //二级桌面（列表型）
            MDataTable dtInfo2 = FrameWorkService.Instance().RoleModule().GetSubAllModuleOfUser(GlobalUserCode, "001204002002", false);

         

            ViewBag.DtRoleModule1 = dtInfo1;
            ViewBag.DtRoleModule2 = dtInfo2;
            String viewPath = FrameWorkService.GetLoginedViewName("desktop/desktop");
            return View(viewPath);
        }

      
    }
}
