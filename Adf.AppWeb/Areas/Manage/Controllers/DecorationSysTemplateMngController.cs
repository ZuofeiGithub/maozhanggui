using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;
using Decoration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class DecorationSysTemplateMngController : BaseController
    {

        /// <summary>
        /// 功能：平台模板
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "decoration/decorationsystemplate/decorationsystemplate.list.cshtml";
            return View(viewPath);
        }

        /// <summary>
        ///  功能：得到列表数据
        /// 创建时间：2018.11.13
        /// 创建人：张硕
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
            String templatename = Server.UrlDecode(RequestHelper.GetQueryString("templatename"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .DecorationSysTemplate()
                .GetList(templatename, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 功能：详细页
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="tasktemplateCode">轮转图编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String templatecode)
        {
            MDataRow mEntity = new MDataRow();


            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().DecorationSysTemplate().InitDataRow();
                mEntity.Set("templatecode", Guid.NewGuid().ToString("N"));

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().DecorationSysTemplate().GetEntityWithTempLatecode(templatecode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "decoration/decorationsystemplate/decorationsystemplate.detail.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 功能：执行
        /// 创建时间：2018.11.13
        /// 创建人：张硕
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
                //增加
                MDataRow mEntity = DecorationService.Instance().DecorationSysTemplate().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("templatecode", Guid.NewGuid());
                exeMsgInfo = DecorationService.Instance().DecorationSysTemplate().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().DecorationSysTemplate().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().DecorationSysTemplate().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String templatecode = RequestHelper.GetFormString("templatecode");
                exeMsgInfo = DecorationService.Instance().DecorationSysTemplate().Delete(templatecode);
            }
            //返回结果
            return Json(exeMsgInfo);
        }
    }
}