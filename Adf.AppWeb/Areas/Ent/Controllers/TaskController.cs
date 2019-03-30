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
    public class TaskController : EntBaseController
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

            string projectcode = RequestHelper.GetQueryString("projectcode");
            string catecode = RequestHelper.GetQueryString("catecode");


            //获取数据
            MDataTable dtInfo = DecorationService.Instance().Task().GetList(projectcode, "", catecode, curPagerInfo.PageIndex, curPagerInfo.PageSize, "", ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
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
        /// <param name="taskcode">主键</param>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String taskcode,string projectcode)
        {
            MDataRow mEntity = new MDataRow();
            ViewBag.CurrentCmd = docmd;
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "task/task.detail");
            MDataTable taskCate = DecorationService.Instance().TaskCate().GetAll(projectcode);
            MDataTable role = DecorationService.Instance().CompanyRole().GetCompanyRole(GlobalUserLogin.Get("CompanyCode", ""));
            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();
            if (docmd.Equals("add"))
            {
                string catecode = RequestHelper.GetQueryString("catecode");
                mEntity = DecorationService.Instance().Task().InitDataRow();
                mEntity.Set("catecode", catecode);
                mEntity.Set("projectcode", projectcode);
                mEntity.Set("taskstatus", 0);
            }

            if (docmd.Equals("modify"))
            {
                mEntity = DecorationService.Instance().Task().GetEntityByTaskcode(taskcode);
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
                MDataRow entity = DecorationService.Instance().Task().InitDataRow();
                entity.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().Task().Add(entity);

            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow dataRow = DecorationService.Instance().Task().InitDataRow();
                dataRow.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().Task().UpdateBytaskcode(dataRow);
            }else if (doCmd.Equals("delete"))
            {
                string taskcode = RequestHelper.GetFormString("taskcode");
                string projectcode = RequestHelper.GetFormString("projectcode");
                string catecode = RequestHelper.GetFormString("catecode");

                exeMsgInfo = DecorationService.Instance().Task().DeleteBytaskcode(taskcode,projectcode, catecode);
            }
            else if (doCmd.Equals("setrole"))
            {
                string taskcode = RequestHelper.GetFormString("taskcode");
                string rolecode = RequestHelper.GetFormString("rolecode");
                if (string.IsNullOrEmpty(rolecode))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "请至少选择一个角色";
                }
                else
                {
                    List<MDataRow> roleList = new List<MDataRow>();
                    string[] roles = rolecode.Split('|');
                    foreach (string role in roles)
                    {
                        MDataRow roleRow = DecorationService.Instance().TaskRole().InitDataRow();
                        roleRow.Set("taskcode", taskcode);
                        roleRow.Set("rolecode", role);
                        roleList.Add(roleRow);
                    }
                    exeMsgInfo = DecorationService.Instance().TaskRole().Add(roleList, taskcode);

                }
            }
            return Json(exeMsgInfo);
        }
    }
}