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
    /// 活动推送
    /// 侯鑫辉
    /// 2018.10.18 
    /// </summary>
    public interface IActivity
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="activityCode">活动编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithActivityCode(String activityCode, bool bCache = false);

        /// <summary>
        /// 增加一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="activityCode">活动编号</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String activityCode);

        /// <summary>
        /// 得到所有的活动信息
        /// </summary>
        /// <param name="activityTitle">活动标题</param>
        /// <param name="activitySummary">活动简介</param>
        /// <param name="activityContent">活动内容</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithCompanyCode(string activityTitle, string activitySummary, string activityContent, string companyCode, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 前端得到活动信息
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithCompanyCode(string companyCode, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 复制新增一个实体
        /// </summary>
        /// <param name="activityCode">活动编号</param>
        /// <returns></returns>
        ExeMsgInfo CopyAdd(String activityCode);

        /// <summary>
        /// 得到活动数量
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        int GetRemindCount(String companyCode);

    }
}
