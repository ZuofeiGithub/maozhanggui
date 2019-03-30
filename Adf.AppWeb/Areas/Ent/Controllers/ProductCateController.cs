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
    /// 产品分类
    /// 侯鑫辉
    /// 2018.10.17
    /// </summary>
    /// <returns></returns>
    public class ProductCateController : EntBaseController
    {
        /// <summary>
        /// 产品分类列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "productcate/productcate.index.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        public ActionResult GetListData(String parentCode = "sys")
        {
            int pageCount = 1;
            int recordCount = 0;
            PagerInfo curPagerInfo = new PagerInfo();
            curPagerInfo.PageIndex = RequestHelper.GetQueryString("p", 1);
            curPagerInfo.PageSize = RequestHelper.GetFormString("ps", 20);

            //查询条件
            String cateName = Server.UrlDecode(RequestHelper.GetQueryString("catename"));
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);


            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .ProductCate()
                .GetListData(companyCode, parentCode, cateName, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

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
        public ActionResult GetTreeData(String doCmd = "list", String cateCode = "sys")
        {
          

            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
            //获取数据
            MDataTable dtInfo = DecorationService.Instance().ProductCate().GetChildAll(cateCode, companyCode);

            String curJson = dtInfo.ToJson(false, false, RowOp.None, true);

            return Content(curJson, "text/json", Encoding.UTF8);

        }

        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="cateCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String cateCode = "")
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
                mEntity = DecorationService.Instance().ProductCate().InitDataRow();
                String parentCode = RequestHelper.GetQueryString("parentcode");
                mEntity.Set("parentcode", parentCode);
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().ProductCate().GetEntityWithCateCode(cateCode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "productcate/productcate.detail.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="doCmd"></param>
        /// <returns></returns>
        [ValidateInput(false)]
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
                String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
                MDataRow mEntity = DecorationService.Instance().ProductCate().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("companycode", companyCode);
                exeMsgInfo = DecorationService.Instance().ProductCate().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
                MDataRow mEntity = DecorationService.Instance().ProductCate().InitDataRow();
                mEntity.LoadFrom(true);
                mEntity.Set("companycode", companyCode);
                exeMsgInfo = DecorationService.Instance().ProductCate().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String catecode = RequestHelper.GetFormString("catecode");
                exeMsgInfo = DecorationService.Instance().ProductCate().Delete(catecode);
            }

            return Json(exeMsgInfo);
        }
    }
}