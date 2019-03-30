using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class DocImpl:IDoc
    {
        #region Implementation of IDoc
        private const String CurrentTableName = "decoration_doc";
        private const String VCurrentTableName = "decoration_vdoc";
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="doccode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByDocCode(string doccode)
        {
            string sWhere = " doccode=" + DbService.SetQuotesValue(doccode);
            return DbService.GetOne(CurrentTableName, sWhere);

        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="doctype">类型</param>
        /// <returns></returns>
        public MDataRow GetEntityByDocCode(string projectcode, string doctype)
        {
            string sWhere = " doctype=" + DbService.SetQuotesValue(doctype)+" and projectcode=" + DbService.SetQuotesValue(projectcode);
            return DbService.GetOne(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="doctype">类型</param>
        /// <returns></returns>
        public MDataTable GetTable(string projectcode, string doctype)
        {
            string sWhere = " doctype=" + DbService.SetQuotesValue(doctype) + " and projectcode=" + DbService.SetQuotesValue(projectcode);
            return DbService.GetTable(CurrentTableName,0, sWhere);
        }

        /// <summary>
        /// 获取实体重载方法(不区分类型)
        /// </summary>
        /// <param name="projectcode"></param>
        /// <returns></returns>
        public MDataTable GetTable(string projectcode)
        {
            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode) + " order by createdatetime desc";
            return DbService.GetTable(VCurrentTableName, 0, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetList(string projectcode, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            string sWhere = "1=1";

            if (!string.IsNullOrEmpty(projectcode))
            {
                sWhere += " and projectcode=" + DbService.SetQuotesValue(projectcode);
            }

         
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by createdatetime desc ";
            }



            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="doctype">类型</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="doccontent">内容</param>
        /// <param name="docimages">图片</param>
        /// <param name="createusercode">创建人</param>
        /// <returns></returns>
        public ExeMsgInfo Add(string doctype, string projectcode, string doccontent, string docimages, string createusercode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (string.IsNullOrEmpty(projectcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目编号不能为空";
                return exeMsgInfo;
            }
            if (string.IsNullOrEmpty(doctype))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "档案类型不能为空";
                return exeMsgInfo;
            }
       
         
            if (string.IsNullOrEmpty(createusercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建人不能为空";
                return exeMsgInfo;
            }

            MDataRow dataRow = this.InitDataRow();
            dataRow.Set("doccode", Guid.NewGuid().ToString("N"));
            dataRow.Set("projectcode", projectcode);
            dataRow.Set("doctype", doctype);
            dataRow.Set("doccontent", doccontent);
            dataRow.Set("docimages", docimages);
            dataRow.Set("createusercode", createusercode);
            dataRow.Set("createdatetime", DateTime.Now);

            return DbService.Insert(CurrentTableName, dataRow, "doccode,projectcode,doctype,doccontent,docimages,createusercode,createdatetime", true);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByDocCode(MDataRow dataRow)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// </summary>
        /// <param name="doccode">编码</param>
        /// <param name="doccontent">内容</param>
        /// <param name="docimages">图片</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByDocCode(string doccode, string doccontent, string docimages)
        {
            MDataRow dataRow = this.GetEntityByDocCode(doccode);
            dataRow.Set("doccontent", doccontent);
            dataRow.Set("docimages", docimages);

            string sWhere = "doccode="+DbService.SetQuotesValue(doccode);
            return DbService.Update(CurrentTableName, dataRow, sWhere, "doccontent,docimages", true);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="doccode">编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteByDocCode(string doccode)
        {
            string sWhere = "doccode=" + DbService.SetQuotesValue(doccode);
            return DbService.Delete(CurrentTableName, sWhere);
        }


        #endregion
    }
}