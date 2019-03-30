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
    /// 功能：平台任务类型
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public interface IDecorationSysTaskCate
    {
        /// <summary>
        ///  功能：初始化
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();
     
        /// <summary>
        /// 功能： 得到实体
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="cateCode">分类编码</param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        MDataRow GetEntityWithTaskcateCode(string cateCode, bool bCache = false);
        
        /// <summary>
        /// 功能： 增加
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);
       
        /// <summary>
        /// 功能： 修改
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);
      
        /// <summary>
        /// 功能： 删除
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="cateCode">任务编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(string cateCode);
       
        /// <summary>
        /// 功能： 得到列表
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="cateName">分类名称</param>      
        /// <param name="tempLatecode">模板</param>      
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string cateName, string tempLatecode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 功能：根据模板编号获取分类
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="sTemplateCode">编号</param>
        /// <returns></returns>
        MDataTable GetTableByTemplateCode(string sTemplateCode);

        /// <summary>
        /// 功能：获取分类
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        MDataTable GetTable();
    }
}
