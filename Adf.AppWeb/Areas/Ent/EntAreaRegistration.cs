using System.Web.Mvc;

namespace Adf.AppWeb.Areas.Ent
{
    public class EntAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Ent_default",
                "Ent/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
                , new string[] { "Adf.AppWeb.Areas.Ent.Controllers" }
            );
        }
    }
}