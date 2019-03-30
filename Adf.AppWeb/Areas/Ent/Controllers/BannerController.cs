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
    public class BannerController : EntBaseController
    {
        /// <summary>
        /// 企业用户列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            String viewPath = GlobalAdminViewPath + "banner/banner.list.cshtml";
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
            String bannertitle = Server.UrlDecode(RequestHelper.GetQueryString("bannertitle"));

            //其他条件（排序）
            String sOrderBy = Server.UrlDecode(RequestHelper.GetQueryString("OrderBy"));

            //获取数据
            MDataTable dtInfo = DecorationService.Instance()
                .Banner()
                .GetList(bannertitle, GlobalCompanyCode, curPagerInfo.PageIndex, curPagerInfo.PageSize, sOrderBy, ref recordCount, ref pageCount);

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
        /// <param name="bannerCode">轮转图编码</param>
        /// <returns></returns>
        public ActionResult Detail(String docmd, String bannerCode)
        {
            MDataRow mEntity = new MDataRow();

            MDataTable dtCompanyRoles = DecorationService.Instance().CompanyRole().GetCompanyRole(GlobalCompanyCode);
            MDataTable sRoledtBanner = DecorationService.Instance().Banner().GetSelectRole(bannerCode);


            List<string> txtCompanyRoles = new List<string>();
            List<string> valCompanyRoles = new List<string>();
            foreach (MDataRow item in dtCompanyRoles.Rows)
            {
                string txt = item.Get("rolename", "");
                string val = item.Get("rolecode", "");
                txtCompanyRoles.Add(txt);
                valCompanyRoles.Add(val);
            }

            List<string> valSelFrontModule3 = new List<string>();
            foreach (MDataRow mDataRow in sRoledtBanner.Rows)
            {
                valSelFrontModule3.Add(mDataRow.Get("rolecode", ""));
            }

            ViewBag.dtCompanyRoles = dtCompanyRoles;
            ViewBag.txtCompanyRoles = string.Join("|", txtCompanyRoles);
            ViewBag.valCompanyRoles = string.Join("|", valCompanyRoles);
            ViewBag.valSelFrontModule3= string.Join("|", valSelFrontModule3);

            if (String.IsNullOrEmpty(docmd))
            {
                return Content("未指明DoCmd");
            }
            docmd = docmd.ToLower();

            if (docmd.Equals("add"))
            {
                //增加          
                mEntity = DecorationService.Instance().Banner().InitDataRow();
                mEntity.Set("bannercode", Guid.NewGuid().ToString("N"));
                mEntity.Set("createusercode", GlobalUserCode);
                mEntity.Set("createdatetime", DateTime.Now);
                mEntity.Set("companycode", GlobalCompanyCode);
            }
            else if (docmd.Equals("modify"))
            {
                //修改
                mEntity = DecorationService.Instance().Banner().GetEntityWithBannerCode(bannerCode);
            }

            ViewBag.DrMainEntity = mEntity;
            ViewBag.CurrentCmd = docmd;
            
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "banner/banner.detail.cshtml");
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
                MDataRow mEntity = DecorationService.Instance().Banner().InitDataRow();
                mEntity.LoadFrom(true);
                string bannerroles = RequestHelper.GetFormString("bannerroles");


                exeMsgInfo = DecorationService.Instance().Banner().Add(mEntity, bannerroles);
            }
            else if (doCmd.Equals("modify"))
            {
                string bannerroles = RequestHelper.GetFormString("bannerroles");
                //修改
                MDataRow mEntity = DecorationService.Instance().Banner().InitDataRow();
                mEntity.LoadFrom(true);

                exeMsgInfo = DecorationService.Instance().Banner().Update(mEntity, bannerroles);
            }
            else if (doCmd.Equals("delete"))
            {
                String bannercode = RequestHelper.GetFormString("bannerCode");
                exeMsgInfo = DecorationService.Instance().Banner().Delete(bannercode);
            }
            //返回结果
            return Json(exeMsgInfo);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="filename"></param>
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