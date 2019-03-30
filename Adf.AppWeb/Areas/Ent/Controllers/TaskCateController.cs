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
    public class TaskCateController : EntBaseController
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


            //获取数据
            MDataTable dtInfo = DecorationService.Instance().TaskCate().GetList(projectcode, "", curPagerInfo.PageIndex, curPagerInfo.PageSize, "", ref recordCount, ref pageCount);

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
        /// <param name="projectcatecode">主键</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String projectcatecode)
        {
            MDataRow mEntity = new MDataRow();
            ViewBag.CurrentCmd = docmd;
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "taskcate/taskcate.detail");
            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();
         
             if (docmd.Equals("modify"))
            {
                mEntity = DecorationService.Instance().TaskCate().GetEntityByProjectCateCode(projectcatecode);
            }

            ViewBag.MainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;


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
                string cateRow = RequestHelper.GetFormString("cateRow");
                string projectcode = RequestHelper.GetFormString("projectcode");
                if (!string.IsNullOrEmpty(cateRow))
                {
                    List<MDataRow> entitys = new List<MDataRow>();
                    List<MDataRow> detailRow = new List<MDataRow>();
                    string[] sCate = cateRow.Split('$');
                    for (var i = 0; i < sCate.Length; i++)
                    {
                        MDataRow entity = DecorationService.Instance().TaskCate().InitDataRow();
                        entity.Set("projectcatecode",Guid.NewGuid().ToString("N"));
                        entity.Set("projectcode", projectcode);
                        entity.Set("catecode", sCate[i].Split('|')[0]);
                        entity.Set("catename", sCate[i].Split('|')[1]);
                        entity.Set("cateorder", sCate[i].Split('|')[2]);
                        entitys.Add(entity);

                        MDataTable task = DecorationService.Instance().CompanyTask().GetList(sCate[i].Split('|')[0]);
                        foreach (MDataRow mDataRow in task.Rows)
                        {
                            MDataRow detail = DecorationService.Instance().Task().InitDataRow();
                            detail.Set("taskcode", Guid.NewGuid().ToString("N"));
                            detail.Set("projectcode", projectcode);
                            detail.Set("taskname", mDataRow.Get("taskname",""));
                            detail.Set("taskstatus", 0);
                            detail.Set("catecode", mDataRow.Get("catecode",""));
                            detailRow.Add(detail);
                        }
                    }
                    exeMsgInfo = DecorationService.Instance().TaskCate().Add(entitys, detailRow);
                }
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow dataRow = DecorationService.Instance().TaskCate().InitDataRow();
                dataRow.LoadFrom(true);
                exeMsgInfo = DecorationService.Instance().TaskCate().UpdateByProjectCateCode(dataRow);
            }else if (doCmd.Equals("delete"))
            {
                string projectcatecode = RequestHelper.GetFormString("projectcatecode");
                string projectcode = RequestHelper.GetFormString("projectcode");
                string catecode = RequestHelper.GetFormString("catecode");

                exeMsgInfo = DecorationService.Instance().TaskCate().DeleteByProjectCateCode(projectcatecode,projectcode, catecode);
            }
            return Json(exeMsgInfo);
        }

        /// <summary>
        /// 获取选择框数据
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public ActionResult Select(string projectcode)
        {
            MDataTable taskcate = DecorationService.Instance().TaskCate().GetAll(projectcode);
            string json = taskcate.ToJson();
            return Json(json);
        }
    }
}