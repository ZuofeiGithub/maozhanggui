using Adf.Core.Entity;
using CYQ.Data.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decoration.Interface
{

    /// <summary>
    /// 功能：平台模板
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public interface IDecorationSysTemplate
    {

        /// <summary>
        /// 功能：初始化
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 
        /// 功能：得到实体
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="tempLatecode">编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithTempLatecode(String tempLatecode, bool bCache = false);

        /// <summary>
        /// 功能：增加
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 功能：修改角色及相关信息
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow">角色实体</param>
        /// <returns>返回ExeMsgInfo</returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 功能：删除
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="tempLatecode">编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String tempLatecode);

        /// <summary>
        /// 功能：列表信息
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="tempLatename">模板名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string tempLatename, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 功能：获得所有模板
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        MDataTable GetTemplateTable();
    }
}
