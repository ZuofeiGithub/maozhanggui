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
    /// 企业施工任务
    /// </summary>
    public class CompanyTaskController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MDataTable catecodeTable = DecorationService.Instance().CompanyTaskcate().GetEntityByCateCode(GlobalCompanyCode);
            ViewBag.catecodeTable = catecodeTable;
            String viewPath = GlobalAdminViewPath + "companytask/companytask.list.cshtml";
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
            String taskName = Server.UrlDecode(RequestHelper.GetQueryString("taskName"));
            String cateCode = Server.UrlDecode(RequestHelper.GetQueryString("cateCode"));

            MDataTable template = DecorationService.Instance().TaskTemplate().GetAll(GlobalCompanyCode) ?? new MDataTable();
            string defaultTemplatecode = "";
            if (template.Rows.Count > 0)
            {
                defaultTemplatecode = template.Rows[0].Get("templatecode", "");
            }
            String templatecode = RequestHelper.GetQueryString("templatecode", defaultTemplatecode);
            
            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyTask()
                .GetList(taskName, GlobalCompanyCode, cateCode ,curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount, templatecode);

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
        /// <param name="taskCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String taskCode)
        {
            MDataRow mEntity = new MDataRow();
            string templatecode = RequestHelper.GetQueryString("templatecode");
            MDataTable catecodeTable = DecorationService.Instance().CompanyTaskcate().GetEntityByCateCode(GlobalCompanyCode, templatecode);
            ViewBag.catecodeTable = catecodeTable;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().CompanyTask().InitDataRow();
               
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().CompanyTask().GetEntityWithTaskCode(taskCode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "companytask/companytask.detail.cshtml");
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
                MDataRow mEntity = DecorationService.Instance().CompanyTask().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("taskcode", Guid.NewGuid());

                exeMsgInfo = DecorationService.Instance().CompanyTask().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().CompanyTask().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().CompanyTask().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String taskcode = RequestHelper.GetFormString("taskcode");
                exeMsgInfo = DecorationService.Instance().CompanyTask().Delete(taskcode);
            }
            //返回结果
            return Json(exeMsgInfo);
        }


        public ActionResult GetTaskCate(string templatecode)
        {
            MDataTable dt = DecorationService.Instance().CompanyTaskcate().GetEntityByCateCode(GlobalCompanyCode, templatecode);

            List< string> dic = new List< string>();
            foreach (MDataRow mDataRow in dt.Rows)
            {
                dic.Add(mDataRow.Get("catename", "")+"|"+ mDataRow.Get("catecode", ""));
            }
            return Json(dic);
        }
    }
}