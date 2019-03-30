using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class LayUiTestController : BaseController
    {
        //
        // GET: /Manage/LayUiTest/

        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "Test/LayUi/Test.index.cshtml");
            return View(viewName);
        }

    }
}
