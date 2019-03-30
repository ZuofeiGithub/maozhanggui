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
    /// 【顾健】申请延期
    /// </summary>
    public interface IApplyDelay
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
         MDataRow GetEntity(string delaycode);
        /// <summary>
        /// 【顾健】增加申请，由[项目经理支做]
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 工程经理审核
        /// </summary>
        /// <param name="delaycode">主键</param>
        /// <param name="checkusercode1">工程经理编码</param>
        /// <param name="ischeck1">申请状态</param>
        /// <returns></returns>
        ExeMsgInfo Check(string delaycode, string checkusercode1, string ischeck1);

        /// <summary>
        /// 业主审核
        /// </summary>
        /// <param name="delaycode">延期主键</param>
        /// <param name="checkusercode2">业主编码</param>
        /// <param name="ischeck2">审批状态</param>
        /// <returns></returns>
        ExeMsgInfo CheckByCustomer(string delaycode, string checkusercode2, string ischeck2);

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
        MDataTable GetList(string companycode, string delaystatus, string checkusercode1, string checkusercode2, int pageIndex,
            int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);






    }
}
