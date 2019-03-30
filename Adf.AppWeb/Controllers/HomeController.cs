using System;
using System.Text;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Decoration.Service;


namespace Adf.AppWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/ent/login");
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult test1()
        {
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Project().ImportTemplate("bc32153cd4ab435d8c35aaa29376c519", "a334e43bfbd740538bc89b3af567ed22", "2018-01-01", "2018-04-01");
            return null;
        }

        public ActionResult Test(String type="", String para="")
        {
            String retValue = "";
            type = type.ToLower();
            if (type == "strtoint")
            {
                retValue= ConvertHelper.ObjectToT<int>(para, -1) + "";
                return Content(retValue);
            }
            

            StringBuilder sbInfo = new StringBuilder();
            sbInfo.Append("未指定类型<br>");

            sbInfo.Append("type=strtoint,参数para 接受值");

            return Content(sbInfo.ToString());


            //return Content("error");
        }
    }
}