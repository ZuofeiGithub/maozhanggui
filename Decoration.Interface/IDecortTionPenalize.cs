using System;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    /// <summary>
    /// 处罚
    /// 顾健 2018-11-08
    /// </summary>
    public interface IDecoraTionPenalize
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="penalizecode">编号</param>
        /// <returns></returns>
        MDataRow GetEntityWithPenalizecode(String penalizecode);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);


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
        MDataTable GetList(String companycode,string taskcode, string companyCode, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);
    }
}