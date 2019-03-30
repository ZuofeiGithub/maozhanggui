using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class TaskMessageImpl:ITaskMessage
    {
        #region Implementation of IMaterShare
        private const String CurrentTableName = "decoration_task_message";
        private const String VCurrentTableName = "decoration_task_message";
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
        /// <param name="messsagecode">编号</param>
        /// <param name="companycode">企业编号</param>
        /// <returns></returns>
        public MDataRow GetEntityBymesssagecode(string messsagecode, string companycode)
        {
            string sWhere = " messsagecode=" + DbService.SetQuotesValue(messsagecode) + " and companycode=" + DbService.SetQuotesValue(companycode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="taskcode">编号</param>
        /// <returns></returns>
        public MDataTable GetAll(string companycode, string taskcode)
        {
            string sWhere = " companycode=" + DbService.SetQuotesValue(companycode)+" and taskcode="+DbService.SetQuotesValue(taskcode);
            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }




        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="membername">名称</param>
        /// <param name="address">地址</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetList(string companycode,string membername,string address, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            string sWhere = "1=1";

            if (!string.IsNullOrEmpty(companycode))
            {
                sWhere += " and companycode=" + DbService.SetQuotesValue(companycode);
            }
            if (!string.IsNullOrEmpty(membername))
            {
                sWhere += " and membername like '%"+ membername + "%'";
            }
            if (!string.IsNullOrEmpty(address))
            {
                sWhere += " and address like '%"+ address + "%'";
            }

            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
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
            return DbService.Insert(CurrentTableName, dataRow, "", false);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            string sWhere = " messsagecode=" + DbService.SetQuotesValue(dataRow.Get("messsagecode", "")) + " and companycode=" + DbService.SetQuotesValue(dataRow.Get("companycode", ""));
            return DbService.Update(CurrentTableName, dataRow, sWhere, "taskcode,messsagecode,companycode", false);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="matercode">编号</param>
        /// <param name="companycode">企业编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteBymatercode(string matercode, string companycode)
        {
            string sWhere = " messsagecode=" + DbService.SetQuotesValue(matercode) + " and companycode=" + DbService.SetQuotesValue(companycode);
            return DbService.Delete(CurrentTableName, sWhere);
        }

        #endregion
    }
}