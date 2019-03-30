using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using Adf.Core.Util;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class DecortTionPenalizeImpl : IDecoraTionPenalize
    {
        #region Implementation of IDecoraTionPenalize
        private const String CurrentTableName = "decoration_penalize";
        private const String VCurrentTableName = "decoration_vpenalize";
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="penalizecode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityWithPenalizecode(string penalizecode)
        {
            string sWhere = " penalizecode=" + DbService.SetQuotesValue(penalizecode);
            return DbService.GetOne(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (!VerificationHelper.CheckStr(dataRow.Get("companycode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }

            if (!VerificationHelper.CheckNumber(dataRow.Get("penalizemoney", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "罚款金额不能为空";
                return exeMsgInfo;
            }

            if (ConvertHelper.ObjectToT(dataRow.Get("penalizemoney", ""), 0.00m) <= 0)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "罚款金额不能小于零";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("taskcode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务编号不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("createusercode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建人不能为空";
                return exeMsgInfo;
            }

            dataRow.Set("penalizecode", Guid.NewGuid().ToString("N"));

            dataRow.Set("createdatetime", DateTime.Now);
            

            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, "penalizecode,companycode,penalizemoney,penalizeremark,penalizeimg,createusercode,createdatetime,taskcode", true);
           
            return exeMsgInfo;

        }

        /// <summary>
        /// 获取处罚列表
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="taskcode">任务编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string companycode, string taskcode, string companyCode, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            string sWhere = "companycode=" + DbService.SetQuotesValue(companycode);

            if (!string.IsNullOrEmpty(taskcode))
            {
                sWhere += " and  taskcode=" + DbService.SetQuotesValue(taskcode);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by createdatetime desc ";
            }
            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        #endregion
    }
}