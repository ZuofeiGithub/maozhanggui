﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace Adf.AppWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();

            //注册webapi
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //注册路由
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}