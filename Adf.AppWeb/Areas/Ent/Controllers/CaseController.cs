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
    public class CaseController : EntBaseController
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
            MDataTable dtInfo = DecorationService.Instance().Case().GetList(projectcode, curPagerInfo.PageIndex, curPagerInfo.PageSize, "", ref recordCount, ref pageCount);

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
                string caseprojectcodes = RequestHelper.GetFormString("caseprojectcode");
                string projectcode = RequestHelper.GetFormString("projectcode");
                if (!string.IsNullOrEmpty(caseprojectcodes))
                {
                    List<MDataRow> dataRows = new List<MDataRow>();
                    string[] caseprojectcode = caseprojectcodes.Split('|');
                    foreach (string u in caseprojectcode)
                    {
                  
                        MDataRow entity = DecorationService.Instance().Case().InitDataRow();
                        entity.Set("caseprojectcode", u);
                        entity.Set("caseorder",0);
                        entity.Set("projectcode", projectcode);
                        entity.Set("createdatetime", DateTime.Now);
                        entity.Set("createusercode", GlobalUserCode);
                        entity.Set("ishot", 0);
                        entity.Set("casecode", Guid.NewGuid().ToString("N"));
                        dataRows.Add(entity);
                    }
                    exeMsgInfo = DecorationService.Instance().Case().Add(dataRows,projectcode);

                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "请至少选择一个成员";
                }
            }
            else if (doCmd.Equals("delete"))
            {
                string casecode = RequestHelper.GetFormString("casecode");

                exeMsgInfo = DecorationService.Instance().Case().DeleteByCaseCode(casecode);
            }else if (doCmd.Equals("setrole"))
            {
                string casecode = RequestHelper.GetFormString("casecode");
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
                        MDataRow roleRow = DecorationService.Instance().CaseRole().InitDataRow();
                        roleRow.Set("casecode", casecode);
                        roleRow.Set("rolecode", role);
                        roleList.Add(roleRow);
                    }
                    exeMsgInfo = DecorationService.Instance().CaseRole().Add(roleList, casecode);

                }
            }
           
            return Json(exeMsgInfo);
        }
    }
}