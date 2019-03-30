using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class CompanyPayImpl: ICompanyPay
    {
        #region Implementation of ICompanyPay
        private const String CurrentTableName = "decoration_pay";
        private const String VCurrentTableName = "decoration_vpay";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }


        public MDataRow GetEntityByPaycode(string paycode)
        {
            string sWhere = "paycode=" + DbService.SetQuotesValue(paycode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (!VerificationHelper.CheckStr(dataRow.Get("paycode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "支付编码不能为空";
                return exeMsgInfo;
            }

            if (!VerificationHelper.CheckStr(dataRow.Get("companycode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("payusercode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "支付人员编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("paymoney", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "支付金额不能为空或格式不正确";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("paydate", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "支付日期不能为空";
                return exeMsgInfo;
            }
            
            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, "paycode,companycode,payusercode,paymoney,paydate,payremark,createusercode,createdatetime", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
        

            if (!VerificationHelper.CheckStr(dataRow.Get("companycode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("payusercode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "支付人员编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("paymoney", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "支付金额不能为空或格式不正确";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("paydate", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "支付日期不能为空";
                return exeMsgInfo;
            }
            string sWhere = "paycode="+DbService.SetQuotesValue(dataRow.Get("paycode",""));
            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere,"companycode,payusercode,paymoney,paydate,payremark,createusercode,createdatetime", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="payCode"></param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string payCode)
        {
            string sWhere = "payCode=" + DbService.SetQuotesValue(payCode);
            return DbService.Delete(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 管理员列表
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public MDataTable GetList(string userName, string startDate, string endDate, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount,string companycode="")
        {
            string sWhere = " 1=1";
            if (!string.IsNullOrEmpty(userName))
            {
                sWhere += " and userName like '%"+ userName + "%'" ;
            }
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                sWhere += " and paydate between '" + startDate + " 00:00:00' and '" + endDate + " 23:59:59' ";
            }
            if (!string.IsNullOrEmpty(companycode))
            {
                sWhere += " and companycode=" + DbService.SetQuotesValue(companycode);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += " order by " + orderBy;
            }
            else
            {
                sWhere += " order by paydate desc";
            }

            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// [前台]用户得到自己的收入
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="userCode">用户编码</param>
        /// <param name="endDate"></param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public MDataTable GetListByUser(string companyCode, string userCode, string startDate, string endDate, int pageIndex,
            int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            string sWhere = " 1=1 and companyCode="+DbService.SetQuotesValue(companyCode)+ " and payusercode=" + DbService.SetQuotesValue(userCode);
            if (!string.IsNullOrEmpty(startDate)&&!string.IsNullOrEmpty(endDate))
            {
                sWhere += " and paydate between '" + startDate + " 00:00:00' and '"+endDate+" 23:59:59'";
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += " order by " + orderBy;
            }
            else
            {
                sWhere += " order by paydate desc";
            }

            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        #endregion
    }
}