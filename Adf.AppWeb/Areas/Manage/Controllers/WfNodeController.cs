using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class WfNodeController : BaseController
    {
        /// <summary>
        /// 得到列表Json值
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 100);

            //条件
            String wfcode = Server.UrlDecode(RequestHelper.GetQueryString("wfcode"));
            String nodecode = Server.UrlDecode(RequestHelper.GetQueryString("nodecode"));
            String nodename = Server.UrlDecode(RequestHelper.GetQueryString("nodename"));
            String sOrderBy = RequestHelper.GetQueryString("OrderBy");


            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .WfNode()
                .GetList(wfcode, nodecode,nodename, sOrderBy, curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/wfmng/wfnode/wfnode.list.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="wfcode">编码</param>
        /// <param name="nodecode">节点编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String wfcode,String nodecode)
        {
            MDataRow mEntity = new MDataRow();

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = FrameWorkService.Instance().WfNode().InitDataRow();

                mEntity.Set("wfcode", wfcode);

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().WfNode().GetDataEntityByWfNodeCode(wfcode,nodecode);
            }

            //
            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/wfmng/wfnode/wfnode.detail.cshtml");
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
                MDataRow mEntity = FrameWorkService.Instance().WfNode().InitDataRow();
                mEntity.LoadFrom(true);


                exeMsgInfo = FrameWorkService.Instance().WfNode().Add(mEntity);

            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().WfNode().InitDataRow();
                mEntity.LoadFrom(true);


                exeMsgInfo = FrameWorkService.Instance().WfNode().UpdateByWfNodeId(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String wfcode = RequestHelper.GetFormString("wfcode");
                String nodecode = RequestHelper.GetFormString("nodecode");
                exeMsgInfo = FrameWorkService.Instance().WfNode().DeleteByWfNodeCode(wfcode,nodecode);
            }

            return Json(exeMsgInfo);

        }



    }
}
