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
    /// 企业模板
    /// </summary>
    public interface ITaskTemplate
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="templatecode">编号</param>
        /// <returns></returns>
        MDataRow GetEntityByTemplatecode(string templatecode);
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        ExeMsgInfo Delete(string companyCode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);
        /// <summary>
        /// 获得该企业任务模版
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataTable GetAll(String companyCode);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="templatename">模板名称</param>
        /// <param name="companycode">企业编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string templatename, string companycode, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);


        /// <summary>
        /// 复制新增
        /// 顾健
        /// </summary>
        /// <param name="templatecode"></param>
        /// <param name="templatename"></param>
        /// <param name="lasttime"></param>
        /// <returns></returns>
        ExeMsgInfo Copy(string templatecode, string templatename, string lasttime);
    }
}
