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
    public class AreaController : BaseController
    {

        /// <summary>
        /// 数据字典首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/area/area.index.cshtml");
            return View(viewName);
        }

        

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        public ActionResult GetListData(String parentCode = "086")
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 20);

            String sOrderBy = "";
            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .Area()
                .GetChildList(parentCode, curPagerInfo.PageIndex, curPagerInfo.PageSize,sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData(String doCmd = "list", String areacode = "086")
        {
            //条件
            if (String.IsNullOrEmpty(areacode))
            {
                areacode = "086";
            }

            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance().Area().GetChildAll(areacode);

            String curJson = dtInfo.ToJson(false, false, RowOp.None, true);
            curJson = StringHelper.ReplaceAll(curJson, "isparent", "isParent");

            return Content(curJson, "text/json", Encoding.UTF8);

        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="areacode">地区编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String areacode = "")
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
                mEntity = FrameWorkService.Instance().Area().InitDataRow();
                String sParentCode = RequestHelper.GetQueryString("parentcode");
                mEntity.Set("parentcode", sParentCode);

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().Area().GetEntityWithAreaCode(areacode);
            }

            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/area/area.detail");
            return View(viewName);
        }

        /// <summary>
        /// 加载地区树,这个是运费模板服务的
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadAreaTree()
        {
            MDataTable dtInfo = FrameWorkService.Instance().Area().GetAllByChina();

            String jsonData = dtInfo.ToJson(false, false);
            ViewBag.TreeJsonData = jsonData;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/area/area.selecttree");
            return View(viewName);
        }

        /// <summary>
        /// 加载地区树,这个是运费模板服务的
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadAreaOneTree()
        {
            MDataTable dtInfo = FrameWorkService.Instance().Area().GetAllByChina();

            String jsonData = dtInfo.ToJson(false, false);
            ViewBag.TreeJsonData = jsonData;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/area/area.LoadAreaOneTree");
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
                MDataRow mEntity = FrameWorkService.Instance().Area().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = FrameWorkService.Instance().Area().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().Area().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = FrameWorkService.Instance().Area().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String areacode = RequestHelper.GetFormString("areacode");
                exeMsgInfo = FrameWorkService.Instance().Area().Delete(areacode);
            }

            return Json(exeMsgInfo);

        }


        /// <summary>
        /// 得到数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAreaCodeByParentCode(string parentcode)
        {
            MDataTable curDataTable=new MDataTable();
            curDataTable = FrameWorkService.Instance().Area().GetChildAll(parentcode);//根据区县选择社区
            return Content(curDataTable.ToJson(), "text/json", Encoding.UTF8);
        }


    }
}
