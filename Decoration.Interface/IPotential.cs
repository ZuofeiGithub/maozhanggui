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
    /// 报备客户信息
    /// </summary>
    public interface IPotential
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="potentialcode"></param>
        /// <returns></returns>
        MDataRow GetEntityByCode(string potentialcode);

        /// <summary>
        /// 增加一个报备
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 获取所有的列表 后台使用
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="potentialusername">报备人</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string companyCode, string potentialusername, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);





    }
}
