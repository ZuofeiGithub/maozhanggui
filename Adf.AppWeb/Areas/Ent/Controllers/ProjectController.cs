using System;
using System.Collections.Generic;
using System.Data;
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
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    public class ProjectController : EntBaseController
    {
        //  顾健
        //
      
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "project/project.index.cshtml");
            return View(viewName);
        }


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "project/Select.list.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 选择数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectListData()
        {
            string projectcode = RequestHelper.GetQueryString("projectcode");
            MDataTable dtInfo = DecorationService.Instance().Project().GetAll(projectcode, GlobalUserLogin.Get("CompanyCode", ""));

            MDataTable Select = DecorationService.Instance().Case().GetAll(projectcode);

            dtInfo.Columns.Add("LAY_CHECKED", SqlDbType.Bit);
            foreach (MDataRow item in dtInfo.Rows)
            {
                foreach (MDataRow SelectTableRow in Select.Rows)
                {
                    if (item.Get("projectcode", "") == SelectTableRow.Get("caseprojectcode", ""))
                    {
                        item.Set("LAY_CHECKED", true, 2);
                    }
                }
            }
            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = 0;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            jsonInfo = jsonInfo.Replace("lay_checked", "LAY_CHECKED");
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }
        
        /// <summary>
        /// 得到列表Json值
        /// 2018-10-18
        /// 顾健
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListData()
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 10);

            string projectname = RequestHelper.GetQueryString("projectname");
            string projectstatus = RequestHelper.GetQueryString("projectstatus");

           
            //获取数据
            MDataTable dtInfo = DecorationService.Instance().Project().GetList(GlobalUserLogin.Get("CompanyCode", ""), projectstatus, projectname, curPagerInfo.PageIndex, curPagerInfo.PageSize, "", ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }


        /// <summary>
        /// 获取项目设计档案
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDocListData()
        {
            string projectcode = RequestHelper.GetQueryString("projectcode");
       
            //获取数据
            MDataTable dtInfo = DecorationService.Instance().Doc().GetTable(projectcode)??new MDataTable();

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = 1;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }
        


        /// <summary>
        /// 模块详细页
        /// 2018-10-18
        /// 顾健
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="projectcode">主键</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String projectcode)
        {
            MDataRow mEntity = new MDataRow();
            ViewBag.CurrentCmd = docmd;
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "project/project.detail");
            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().Project().InitDataRow();
            }
            else if (docmd.Equals("modify"))
            {
                mEntity = DecorationService.Instance().Project().GetEntityByProjectCode(projectcode);
            }else if (docmd.Equals("view"))
            {
                mEntity = DecorationService.Instance().Project().GetEntityByProjectCode(projectcode);
                viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "project/project.details");
            }
          

            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;


            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// 2018-10-18
        /// 顾健
        /// </summary>
        /// <param name="doCmd">参数</param>
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
                MDataRow mEntity = DecorationService.Instance().Project().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("projectcode",Guid.NewGuid().ToString("N"));
                mEntity.Set("companycode", GlobalUserLogin.Get("CompanyCode", ""));
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);
                mEntity.Set("updatedatetime", DateTime.Now);
                mEntity.Set("projectprocess",0);
                exeMsgInfo = DecorationService.Instance().Project().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().Project().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("updatedatetime", DateTime.Now);
                exeMsgInfo = DecorationService.Instance().Project().UpdateByProjectCode(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String sprojectcode = RequestHelper.GetFormString("projectcode");
                exeMsgInfo = DecorationService.Instance().Project().DeleteByProjectCode(sprojectcode);
            }
            //else if(doCmd.Equals("synchronization"))
            //{
            //    String sprojectcode = RequestHelper.GetFormString("projectcode");
            //    exeMsgInfo = DecorationService.Instance().Project().DeleteByProjectCode(sprojectcode);
            //}          
            return Json(exeMsgInfo);

        }


    }
}