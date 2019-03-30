using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class ApplyDelayImpl :IApplyDelay
    {
        /// <summary>
        /// 【顾健】申请延期
        /// </summary>
        /// 
        #region Implementation of IApplyDelay

        private const String CurrentTableName = "decoration_delay";
        private const String VCurrentTableName = "decoration_vdelay";


        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow GetEntity(string delaycode)
        {
            return DbService.GetOne(CurrentTableName, " delaycode='" + delaycode + "'");
        }
        /// <summary>
        /// 【顾健】增加申请，由[项目经理支做]
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (!VerificationHelper.CheckStr(dataRow.Get("delaycode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            string sWhere = "1=1 and delaycode=" + DbService.SetQuotesValue(dataRow.Get("delaycode", ""));
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码已存在";
                return exeMsgInfo;
            }

            if (!VerificationHelper.CheckStr(dataRow.Get("projectcode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目编号不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("delayreason", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "延期原因不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("delaydays", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "延期天数不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("submitusercode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目经理编号不能为空";
                return exeMsgInfo;
            }
         
            dataRow.Set("delaystatus","10");
            dataRow.Set("issubmit", "1");
            dataRow.Set("submitdatetime", DateTime.Now);
            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, "delaycode,projectcode,delayreason,delaydays,projectenddate,delaystatus,applaydatetime,submitusercode,issubmit,submitdatetime", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 工程经理审核
        /// </summary>
        /// <param name="delaycode">主键</param>
        /// <param name="checkusercode1">工程经理编码</param>
        /// <param name="ischeck1">申请状态</param>
        /// <returns></returns>
        public ExeMsgInfo Check(string delaycode,string checkusercode1,string ischeck1)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = "1=1 and delaycode="+DbService.SetQuotesValue(delaycode);
            if (!VerificationHelper.CheckStr(delaycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            if (!VerificationHelper.CheckStr(checkusercode1))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "工程经理编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(ischeck1))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择审批状态";
                return exeMsgInfo;
            }
            MDataRow dataRow = DbService.GetOne(CurrentTableName, sWhere);
            if (ischeck1 == "1")
            {
                dataRow.Set("delaystatus", "20");
            }

            if (ischeck1 == "-1")
            {
                dataRow.Set("delaystatus", "11");
            }
            dataRow.Set("checkusercode1", checkusercode1);
            dataRow.Set("ischeck1", 1);
            dataRow.Set("checkdatetime1", DateTime.Now);
            exeMsgInfo= DbService.Update(CurrentTableName,  dataRow, sWhere, "checkusercode1,checkdatetime1,delaystatus,ischeck1", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 业主审核
        /// </summary>
        /// <param name="delaycode">延期主键</param>
        /// <param name="checkusercode2">业主编码</param>
        /// <param name="ischeck2">审批状态</param>
        /// <returns></returns>
        public ExeMsgInfo CheckByCustomer(string delaycode, string checkusercode2, string ischeck2)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = "1=1 and delaycode=" + DbService.SetQuotesValue(delaycode);
            if (!VerificationHelper.CheckStr(delaycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            if (!VerificationHelper.CheckStr(checkusercode2))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "业主编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(ischeck2))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择审批状态";
                return exeMsgInfo;
            }
            MDataRow dataRow = DbService.GetOne(CurrentTableName, sWhere);
            if (ischeck2 == "1")
            {
                dataRow.Set("delaystatus", "21");
            }

            if (ischeck2 == "-1")
            {
                dataRow.Set("delaystatus", "22");
            }
            dataRow.Set("checkusercode2", checkusercode2);
            dataRow.Set("checkdatetime2", DateTime.Now);
            dataRow.Set("ischeck2", 1);
            exeMsgInfo= DbService.Update(CurrentTableName, dataRow, sWhere, "checkusercode2,checkdatetime2,delaystatus,ischeck2", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 【顾健】
        /// 得到需要工程部经理审核
        /// </summary>
        /// <param name="companycode"></param>
        /// <param name="delaystatus"></param>
        /// <param name="checkusercode1"></param>
        /// <param name="checkusercode2"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string projectCode, string delaystatus, string checkusercode1,string checkusercode2,int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            string sWhere = " 1=1";
            if (!string.IsNullOrEmpty(projectCode))
            {
                sWhere += " and projectCode=" + DbService.SetQuotesValue(projectCode);
            }

            if (!string.IsNullOrEmpty(delaystatus))
            {
                sWhere += " and delaystatus=" + DbService.SetQuotesValue(delaystatus);
            }
            if (!string.IsNullOrEmpty(checkusercode1))
            {
                sWhere += " and checkusercode1=" + DbService.SetQuotesValue(checkusercode1);
            }
            if (!string.IsNullOrEmpty(checkusercode2))
            {
                sWhere += " and checkusercode2=" + DbService.SetQuotesValue(checkusercode2);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += " order by " + orderBy;
            }
            else
            {
                sWhere += " order by applaydatetime desc";
            }

            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        #endregion
    }
}