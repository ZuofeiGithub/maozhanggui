using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    /// <summary>
    /// 通用上传
    /// </summary>
    public class BoUploadController : BaseController
    {
        private static String UploadPath = ConfigurationManager.AppSettings["UploadPath"];
        //
        // GET: /Manage/BoUpload/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 得到一个业务所有的上传列表
        /// </summary>
        /// <param name="bocode"></param>
        /// <returns></returns>
        public ActionResult GetAll(String bocode)
        {
            //根据业务得到所有的附件
            MDataTable dtInfo = FrameWorkService.Instance().BoUpload().GetAll(bocode);
            ViewBag.DtMainList = dtInfo;

            //视图
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/boupload/boupload.list.cshtml");
            return View(viewName);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="docmd"></param>
        /// <returns></returns>
        public ActionResult Execute(String docmd)
        {
            return Content("");
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult DoUploadImage()
        {
            UploadStatusInfo uploadStatusInfoEntity = new UploadStatusInfo();


            int retStatus = 900;
            String retMsg = "上传失败...";
            String newFileName = "";
            HttpPostedFileBase file = Request.Files["file"];
            //dirPath = Server.UrlDecode(dirPath);

            if (file != null)
            {
                if (file.ContentLength == 0)
                {
                    uploadStatusInfoEntity.ErrorCode = "03";
                    uploadStatusInfoEntity.ErrorMsg = "文件有问题";
                    return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);

                }
                else
                {
                    try
                    {
                        String fileName = file.FileName;
                        var fileExtension = fileName.Substring(fileName.LastIndexOf("."));
                        newFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;
                        if (String.IsNullOrEmpty(UploadPath))
                        {
                            UploadPath = "upload";
                        }
                        String vDirPath = "/" + UploadPath;
                        //if (dirPath.Length > 0)
                        //{
                        //    vDirPath += "/" + dirPath;
                        //}
                        var savePath = Path.Combine(Server.MapPath(vDirPath), newFileName);
                        LogHelper.WriteLog("图片上传的路径:" + savePath,savePath);
                        //file.SaveAs(Server.MapPath("~/") + System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(savePath);

                        //存储至上传信息表中


                        String sServerFileName = vDirPath + "/" + newFileName;
                    

                        uploadStatusInfoEntity.ErrorCode = "";
                        uploadStatusInfoEntity.ErrorMsg = "";
                        uploadStatusInfoEntity.OriginalFileName = fileName;
                        uploadStatusInfoEntity.ServerFileName = newFileName;
                        uploadStatusInfoEntity.ServerRelativeFileName = sServerFileName;



                        return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);

                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(ex.Message);
                        uploadStatusInfoEntity.ErrorCode = "10";
                        uploadStatusInfoEntity.ErrorMsg = ex.ToString();
                        return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);

                    }

                }
            }

            uploadStatusInfoEntity.ErrorCode = "09";
            uploadStatusInfoEntity.ErrorMsg = "未选择文件";
            return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);
        }

        /// <summary>
        /// 执行上传
        /// </summary>
        /// <param name="boCode"></param>
        /// <returns></returns>
        public ActionResult DoUpload(String boCode)
        {
            UploadStatusInfo uploadStatusInfoEntity = new UploadStatusInfo();
            if (String.IsNullOrEmpty(boCode))
            {
                uploadStatusInfoEntity.ErrorCode = "01";

                return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);
            }



            int retStatus = 900;
            String retMsg = "上传失败...";
            String newFileName = "";
            HttpPostedFileBase file = Request.Files["file"];
            //dirPath = Server.UrlDecode(dirPath);

            if (file != null)
            {
                if (file.ContentLength == 0)
                {
                    uploadStatusInfoEntity.ErrorCode = "03";
                    uploadStatusInfoEntity.ErrorMsg = "文件有问题";
                    return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);

                }
                else
                {
                    try
                    {
                        String fileName = file.FileName;
                        var fileExtension = fileName.Substring(fileName.LastIndexOf("."));
                        newFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;
                        if (String.IsNullOrEmpty(UploadPath))
                        {
                            UploadPath = "upload";
                        }
                        String vDirPath = "/" + UploadPath;
                        //if (dirPath.Length > 0)
                        //{
                        //    vDirPath += "/" + dirPath;
                        //}
                        var savePath = Path.Combine(Server.MapPath(vDirPath), newFileName);
                        //file.SaveAs(Server.MapPath("~/") + System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(savePath);

                        //存储至上传信息表中


                        String sServerFileName = vDirPath + "/" + newFileName;
                        MDataRow drEntity = FrameWorkService.Instance().BoUpload().InitDataRow();
                        drEntity.Set("bocode", boCode);
                        drEntity.Set("uploadfilename", fileName);
                        drEntity.Set("uploadfileserverpath", sServerFileName);
                        drEntity.Set("uploaddate", DateTime.Now);
                        drEntity.Set("filetype", fileExtension);
                        drEntity.Set("uploadusercode", GlobalUserCode);
                        drEntity.Set("uploadusername", GlobalUserLogin.Get("username", ""));

                        ExeMsgInfo tExeMsgInfo = FrameWorkService.Instance().BoUpload().Insert(drEntity);
                        if (tExeMsgInfo.RetStatus == 100)
                        {
                            uploadStatusInfoEntity.ErrorCode = "";
                            uploadStatusInfoEntity.ErrorMsg = "";
                            uploadStatusInfoEntity.OriginalFileName = fileName;
                            uploadStatusInfoEntity.ServerFileName = sServerFileName;

                            String sJson = drEntity.ToJson(true);

                            return Content(sJson, "text/html", Encoding.UTF8);
                        }
                        else
                        {
                            //上传失败处理
                            uploadStatusInfoEntity.ErrorCode = "02";
                            uploadStatusInfoEntity.ErrorMsg = tExeMsgInfo.RetValue;
                            return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);

                        }

                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteLog(ex.Message);
                        uploadStatusInfoEntity.ErrorCode = "10";
                        uploadStatusInfoEntity.ErrorMsg = ex.ToString();
                        return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);

                    }

                }
            }

            uploadStatusInfoEntity.ErrorCode = "09";
            uploadStatusInfoEntity.ErrorMsg = "未选择文件";
            return Content(ConvertHelper.ClassToJson(uploadStatusInfoEntity), "text/html", Encoding.UTF8);
        }

    }


}
