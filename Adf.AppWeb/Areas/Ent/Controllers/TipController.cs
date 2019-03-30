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
    public class TipController : EntBaseController
    {

        /// <summary>
        /// 得到列表Json值
        /// 2018-10-19
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

            string projectcatecode = RequestHelper.GetQueryString("projectcatecode");
            string projectcode = RequestHelper.GetQueryString("projectcode");
            string rolecode = RequestHelper.GetQueryString("rolecode");
            string tiptitle = RequestHelper.GetQueryString("tiptitle");
            string issend = RequestHelper.GetQueryString("issend");


            //获取数据
            MDataTable dtInfo = DecorationService.Instance().Tip().GetList(projectcode, projectcatecode, rolecode, tiptitle, issend, curPagerInfo.PageIndex, curPagerInfo.PageSize, "", ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 获取项目下所有提醒数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllListData()
        {
            string projectcatecode = RequestHelper.GetQueryString("projectcatecode");
            string projectcode = RequestHelper.GetQueryString("projectcode");
         
            //获取数据
            MDataTable dtInfo = DecorationService.Instance().Tip().GetList(projectcode, projectcatecode);

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
        /// <param name="Tipcode">主键</param>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String Tipcode,string projectcode)
        {
            MDataRow mEntity = new MDataRow();
            ViewBag.CurrentCmd = docmd;
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "Tip/Tip.detail");
            MDataTable taskCate = DecorationService.Instance().TaskCate().GetAll(projectcode);
            MDataTable role = DecorationService.Instance().CompanyRole().GetCompanyRole(GlobalCompanyCode);

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();
            if (docmd.Equals("add"))
            {
                string projectcatecode = RequestHelper.GetQueryString("projectcatecode");
                mEntity = DecorationService.Instance().Tip().InitDataRow();
                mEntity.Set("tipcode", Guid.NewGuid().ToString("N"));
                mEntity.Set("projectcatecode", projectcatecode);
                mEntity.Set("projectcode", projectcode);
                mEntity.Set("issend", 0);
            }

            if (docmd.Equals("modify"))
            {
                mEntity = DecorationService.Instance().Tip().GetEntityByTipcode(Tipcode);
            }

            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            ViewBag.taskCate = taskCate;
            ViewBag.role = role;

            return View(viewName);
        }


        /// <summary>
        /// 执行
        /// 2018-10-19
        /// 顾健
        /// </summary>
        /// <returns></returns>
        public ActionResult Execute(string doCmd)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();
            if (doCmd.Equals("add"))
            {
                MDataRow entity = DecorationService.Instance().Tip().InitDataRow();
                entity.LoadFrom(true);
                entity.Set("createusercode",GlobalUserCode);
                entity.Set("createdatetime", DateTime.Now);
                exeMsgInfo = DecorationService.Instance().Tip().Add(entity);

            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow dataRow = DecorationService.Instance().Tip().InitDataRow();
                dataRow.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().Tip().UpdateByTipcode(dataRow);
            }else if (doCmd.Equals("delete"))
            {
                string Tipcode = RequestHelper.GetFormString("tipcode");

                exeMsgInfo = DecorationService.Instance().Tip().DeleteByTipcode(Tipcode);
            }else if (doCmd.Equals("sendtip"))
            {
                string Tipcode = RequestHelper.GetFormString("tipcode");

                exeMsgInfo = DecorationService.Instance().Tip().SendByTipcode(Tipcode);
            }
           
            return Json(exeMsgInfo);
        }
    }
}