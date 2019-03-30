using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    /// <summary>
    /// 顾健 项目施工任务
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();



         ExeMsgInfo SetCode(MDataRow dataRow);
      


         MDataRow GetCode(string newcode);
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="taskcode">编号</param>
        /// <returns></returns>
        MDataRow GetEntityByTaskcode(string taskcode);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据分类编号 项目编号获取所有任务
        /// </summary>
        /// <param name="taskcode">编号</param>
        /// <returns></returns>
        MDataTable GetAllRole(string taskcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">编号</param>
        /// <param name="catename">分类名称</param>
        /// <param name="catecode">分类编码</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string projectcode, string catename, string catecode, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo UpdateBytaskcode(MDataRow dataRow);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="taskcode">项目阶段编号</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="catecode">任务分类编号</param>
        /// <returns></returns>
        ExeMsgInfo DeleteBytaskcode(string taskcode, string projectcode, string catecode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据分类编号 项目编号获取所有任务
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="cateCode">分类编号</param>
        /// <returns></returns>
        MDataTable GetAll(string projectcode, string cateCode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：项目编号获取所有任务
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        MDataTable GetAll(string projectcode);
        /// <summary>
        /// 执行记录 设置完成
        /// </summary>
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        ExeMsgInfo Complete(string taskcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-20
        /// 功能：根据项目编号，任务分类编号 获取该阶段下所有天数总和 排除当前任务
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="catecode">任务分类编号</param>
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        int GetSumDayByProjectCode(string projectcode, string catecode, string taskcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：总数
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        int GetRemindCount(string companyCode);


        /// <summary>
        /// 删除项目任务
        /// 侯鑫辉 2018.11.13
        /// </summary>
        /// <returns></returns>
        ExeMsgInfo DeleteByTaskCode(string taskCode);
    }
}