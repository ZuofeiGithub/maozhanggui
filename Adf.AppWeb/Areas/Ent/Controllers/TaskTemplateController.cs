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
using CYQ.Data.Table;
using Decoration.Service;

namespace Adf.AppWeb.Areas.Ent.Controllers
{
    /// <summary>
    /// 2018.10.17 创建 王浩
    /// 首页轮转图
    /// </summary>
    public class TaskTemplateController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "tasktemplate/tasktemplate.list.cshtml";
            return View(viewPath);
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
            curPagerInfo.PageSize = RequestHelper.GetQueryString("ps", 20);

            //查询条件
            String tasktemplatetitle = Server.UrlDecode(RequestHelper.GetQueryString("tasktemplatetitle"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .TaskTemplate()
                .GetList(tasktemplatetitle, GlobalCompanyCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 跳转
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
                mEntity = DecorationService.Instance().TaskTemplate().InitDataRow();
                mEntity.Set("templatecode", Guid.NewGuid().ToString("N"));
                mEntity.Set("companycode", GlobalCompanyCode);
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().TaskTemplate().GetEntityByTemplatecode(templatecode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "tasktemplate/tasktemplate.detail.cshtml");
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
                //增加
                MDataRow mEntity = DecorationService.Instance().TaskTemplate().InitDataRow();
                mEntity.LoadFrom(true);
               
                exeMsgInfo = DecorationService.Instance().TaskTemplate().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().TaskTemplate().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().TaskTemplate().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String templatecode = RequestHelper.GetFormString("templatecode");
                exeMsgInfo = DecorationService.Instance().TaskTemplate().Delete(templatecode);
            }
            else if (doCmd.Equals("copy"))
            {
                String templatecode = RequestHelper.GetQueryString("templatecode");
                string templatename = RequestHelper.GetQueryString("templatename");
                string lasttime = RequestHelper.GetQueryString("lasttime");
                exeMsgInfo = DecorationService.Instance().TaskTemplate().Copy(templatecode, templatename, lasttime);

                
            }
            //返回结果
            return Json(exeMsgInfo);
        }

       
    }
}