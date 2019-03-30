using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface ITaskCate
    {
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcatecode">编号</param>
        /// <returns></returns>
        MDataRow GetEntityByProjectCateCode(string projectcatecode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">编号</param>
        /// <param name="catename">分类名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string projectcode, string catename, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRows">阶段数据集</param>
        /// <param name="detail">项目任务集合</param>
        /// <returns></returns>
        ExeMsgInfo Add(List<MDataRow> dataRows, List<MDataRow> detail);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo UpdateByProjectCateCode(MDataRow dataRow);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="projectcatecode">项目阶段编号</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="catecode">任务分类编号</param>
        /// <returns></returns>
        ExeMsgInfo DeleteByProjectCateCode(string projectcatecode, string projectcode,string catecode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据项目编号获取所有阶段天数总和 排除当前阶段
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectcatecode">阶段编号</param>
        /// <returns></returns>
        int GetSumDayByProjectCode(string projectcode, string projectcatecode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据项目编号获取所有数据
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        MDataTable GetAll(string projectcode);
    }
}