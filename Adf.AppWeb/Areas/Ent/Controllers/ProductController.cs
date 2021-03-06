﻿using System;
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
    /// 产品管理
    /// 侯鑫辉
    /// 2018.10.17
    public class ProductController : EntBaseController
    {
        /// <summary>
        /// 产品列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);
            ViewBag.curProductCate = DecorationService.Instance().Product().GetProductCateWithCompanyCode(companyCode);
            String viewPath = GlobalAdminViewPath + "product/product.index.cshtml";
            return View(viewPath);
        }

        /// <summary>
        /// 产品列表数据
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
            String productName = Server.UrlDecode(RequestHelper.GetQueryString("productname"));
            String cateCode = Server.UrlDecode(RequestHelper.GetQueryString("catecode"));
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .Product()
                .GetListByCompanyCode(cateCode, companyCode, productName, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

            LayUiPager layUiPager = new LayUiPager();
            layUiPager.Count = recordCount;
            layUiPager.DtData = dtInfo;
            String jsonInfo = layUiPager.ToJson;

            return Content(jsonInfo, "text/json", Encoding.UTF8);
        }


        /// <summary>
        /// 模块详细页
        /// </summary>
        /// <param name="docmd"></param>
        /// <param name="productCode">编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String productCode = "")
        {
            MDataRow mEntity = new MDataRow();
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "product/product.detail.cshtml");
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            ViewBag.CurrentCmd = docmd;

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            ViewBag.curProductCate = DecorationService.Instance().Product().GetProductCateWithCompanyCode(companyCode);

            if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().Product().GetEntityWithProductCode(productCode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;

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
            String companyCode = DecorationService.Instance().CompanyUser().GetCompanyCodeWithUserCode(GlobalUserCode);

            if (String.IsNullOrEmpty(doCmd))
            {
                doCmd = "";
            }
            doCmd = doCmd.ToLower();

            if (doCmd.Equals("add"))
            {
                MDataRow mEntity = DecorationService.Instance().Product().InitDataRow();
                mEntity.LoadFrom(true);
                
                mEntity.Set("companycode", companyCode); 
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);

                exeMsgInfo = DecorationService.Instance().Product().Add(mEntity);
            }
            else if (doCmd.Equals("modify"))
            {
                MDataRow mEntity = DecorationService.Instance().Product().InitDataRow();
                mEntity.LoadFrom(true);

                mEntity.Set("companycode", companyCode);

                exeMsgInfo = DecorationService.Instance().Product().Update(mEntity);
            }
            else if (doCmd.Equals("delete"))
            {
                String productcode = RequestHelper.GetFormString("productcode");
                exeMsgInfo = DecorationService.Instance().Product().Delete(productcode);
            }
            return Json(exeMsgInfo);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="filename">图片名称</param>
        /// <returns></returns>
        public ActionResult DelUploadFile(string filename)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (string.IsNullOrEmpty(filename)) { throw new Exception("参数不能为空"); }
                string uploadfilename = Server.MapPath(filename);
                System.IO.FileInfo fi = new System.IO.FileInfo(uploadfilename);
                if (!fi.Exists)
                {
                    exeMsgInfo.RetStatus = 200;
                    exeMsgInfo.RetValue = "文件不存在，已删除";
                    return Json(exeMsgInfo, JsonRequestBehavior.AllowGet);
                }

                if (!filename.ToLower().StartsWith("/upload"))
                {
                    throw new Exception("只能删除upload目录下的文件");
                }
                fi.Delete();

                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "图片删除成功";
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return Json(exeMsgInfo, JsonRequestBehavior.AllowGet);
        }
    }
}