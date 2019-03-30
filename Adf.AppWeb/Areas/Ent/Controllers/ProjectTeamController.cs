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
    public class ProjectTeamController : EntBaseController
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
            MDataTable dtInfo = DecorationService.Instance().ProjectTeam().GetList(projectcode, curPagerInfo.PageIndex, curPagerInfo.PageSize, "", ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
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
                string usercodes = RequestHelper.GetFormString("usercode");
                string projectcode = RequestHelper.GetFormString("projectcode");
                if (!string.IsNullOrEmpty(usercodes))
                {
                    List<MDataRow> dataRows = new List<MDataRow>();
                    string[] usercode = usercodes.Split('|');
                    foreach (string u in usercode)
                    {
                        MDataRow entity = DecorationService.Instance().ProjectTeam().InitDataRow();
                     
                        entity.Set("teamcode", Guid.NewGuid().ToString("N"));
                        entity.Set("projectcode", projectcode);
                        entity.Set("usercode", u);
                        dataRows.Add(entity);
                    }
                    exeMsgInfo = DecorationService.Instance().ProjectTeam().Add(dataRows,projectcode);

                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "请至少选择一个成员";
                }
            }
            else if (doCmd.Equals("delete"))
            {
                string teamcode = RequestHelper.GetFormString("teamcode");

                exeMsgInfo = DecorationService.Instance().ProjectTeam().DeleteByTeamCode(teamcode);
            }
           
            return Json(exeMsgInfo);
        }
    }
}