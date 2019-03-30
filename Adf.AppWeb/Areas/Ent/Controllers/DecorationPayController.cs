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
    /// 2018.11.07 创建 顾健
    /// 首页轮转图
    /// </summary>
    public class decorationpayController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "decorationpay/decorationpay.list.cshtml";
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
            String date = RequestHelper.GetQueryString("date");
            String username = RequestHelper.GetQueryString("username");

            string startdate = "";
            string enddate = "";
            if (!string.IsNullOrEmpty(date))
            {
                startdate = date.Split('至')[0];
                enddate = date.Split('至')[1];
            }
            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .CompanyPay()
                .GetList(username, startdate, enddate, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount,GlobalCompanyCode);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;

            String jsonInfo = layUiPager.ToJson;
            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="paycode">支付编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String paycode)
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
                mEntity = DecorationService.Instance().CompanyPay().InitDataRow();
                mEntity.Set("paycode", Guid.NewGuid().ToString("N"));
                mEntity.Set("companycode",GlobalCompanyCode);
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().CompanyPay().GetEntityByPaycode(paycode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "decorationpay/decorationpay.detail.cshtml");
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
                MDataRow mEntity = DecorationService.Instance().CompanyPay().InitDataRow();
                mEntity.LoadFrom(true);
               
                exeMsgInfo = DecorationService.Instance().CompanyPay().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                //修改
                MDataRow mEntity = DecorationService.Instance().CompanyPay().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().CompanyPay().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String paycode = RequestHelper.GetFormString("paycode");
                exeMsgInfo = DecorationService.Instance().CompanyPay().Delete(paycode);
            }
            //返回结果
            return Json(exeMsgInfo);
        }

    }
}