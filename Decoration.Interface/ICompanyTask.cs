using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    /// 企业施工任务接口
    /// 吴尧
    /// 2018-10-15
    /// </summary>
    public interface ICompanyTask
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();


        /// <summary>
        /// 根据分类编号 获取数据
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <param name="catecode">分类编号</param>
        /// <returns></returns>
         MDataTable GetList( string catecode);
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="taskCode">编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithTaskCode(String taskCode, bool bCache = false);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="taskCode">编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String taskCode);

        /// <summary>
        /// 列表信息
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="cateCode">阶段编码</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>

        MDataTable GetList(string taskName, string companyCode, string cateCode, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount,string templatecode ="");


    }
}
