using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class LogInfoController : BaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/loginfo/loginfo.List");
            return View(viewName);
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
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 10);

            //条件
            String sOrderBy = RequestHelper.GetQueryString("OrderBy");
            String logusername = Server.UrlDecode(RequestHelper.GetQueryString("logusername"));


            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .LogInfo()
                .GetList(logusername, curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;


            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }


    }
}
