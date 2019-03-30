using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    /// <summary>
    /// 【顾健】PC 企业支出，管理员执行
    /// </summary>
    public interface ICompanyPay
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <returns></returns>
        MDataRow GetEntityByPaycode(string paycode);
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payCode"></param>
        /// <returns></returns>
        ExeMsgInfo Delete(String payCode);

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
        MDataTable GetList(string userName, String startDate, String endDate, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount, string companycode = "");


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
        MDataTable GetListByUser(String companyCode, string userCode,String startDate, String endDate, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);



    }
}
