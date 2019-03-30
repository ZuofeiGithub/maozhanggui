using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface ICase
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
        /// <param name="casecode">编号</param>
        /// <returns></returns>
        MDataRow GetEntityByCaseCode(string casecode);



        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">编号</param>
        /// <returns></returns>
         MDataTable GetAll(string projectcode);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string projectcode, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string usercode, string projectcode, int pageIndex, int pageSize, string orderBy,
            ref int recordCount,
            ref int pageCount);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        ExeMsgInfo Add(List<MDataRow> dataRows, string projectcode);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="createusercode">创建人</param>
        /// <param name="caseprojectcode">案例项目编号</param>
        /// <returns></returns>
        ExeMsgInfo Add(string casecode, string projectcode,string createusercode,string caseprojectcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="casecode">编号</param>
        /// <returns></returns>
        ExeMsgInfo DeleteByCaseCode(string casecode);
    }
}