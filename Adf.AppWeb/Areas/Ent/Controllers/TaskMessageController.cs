using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    public class TaskMessageController : EntBaseController
    {

        /// <summary>
        /// 活动推送列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "TaskMessage/TaskMessage.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 活动推送列表数据
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
            String membername = RequestHelper.GetQueryString("membername");
            String address = RequestHelper.GetQueryString("address");

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .TaskMessage()
                .GetList(GlobalCompanyCode, membername, address, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }
    }
}