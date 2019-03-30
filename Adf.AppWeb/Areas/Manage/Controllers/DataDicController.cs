using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    public class DataDicController : BaseController
    {

        /// <summary>
        /// 数据字典首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/DataDic/DataDic.index.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        public ActionResult GetListData(String parentCode = "system")
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 20);


            //获取数据
            MDataTable dtInfo = FrameWorkService.Instance()
                .DataDic()
                .GetList(parentCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, ref recordCount, ref pageCount);

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
        public ActionResult GetTreeData(String doCmd = "list",String dicCode="root")
        {
                //条件
            if (String.IsNullOrEmpty(dicCode))
            {
                dicCode = "root";
            }

                //获取数据
                MDataTable dtInfo = FrameWorkService.Instance().DataDic().GetAll(dicCode);

            String curJson = dtInfo.ToJson(false, false, RowOp.None, true);
            curJson = StringHelper.ReplaceAll(curJson, "isparent", "isParent");

            return Content(curJson, "text/json", Encoding.UTF8);
        
        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="dicCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd,String dicCode="")
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
                mEntity = FrameWorkService.Instance().DataDic().InitDataRow();
                String sParentCode = RequestHelper.GetQueryString("ParentCode");
                mEntity.Set("ParentCode", sParentCode);

            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = FrameWorkService.Instance().DataDic().GetEntityByDicCode(dicCode);
            }

            //
            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath,"system/DataDic/DataDic.detail");
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
                MDataRow mEntity = FrameWorkService.Instance().DataDic().InitDataRow();
                mEntity.LoadFrom(true);

                if (String.IsNullOrEmpty(mEntity.Get("DicCode", "")))
                {
                    mEntity.Set("DicCode", System.Guid.NewGuid().ToString("N"));
                }
               
                exeMsgInfo = FrameWorkService.Instance().DataDic().Insert(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = FrameWorkService.Instance().DataDic().InitDataRow();
                mEntity.LoadFrom(true);
                exeMsgInfo = FrameWorkService.Instance().DataDic().UpdateByDicCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String sDicCode = RequestHelper.GetFormString("DicCode");
                exeMsgInfo = FrameWorkService.Instance().DataDic().DeleteByDicCode(sDicCode);
            }

            return Json(exeMsgInfo);

        }





    }
}
