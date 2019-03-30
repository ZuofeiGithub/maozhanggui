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
    /// 功能：平台施工任务
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public interface IDecorationSysTask
    {
        /// <summary>
        ///  功能：初始化
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        ///  功能：得到实体
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="taskCode"></param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        MDataRow GetEntityWithTaskCode(string taskCode, bool bCache = false);

        /// <summary>
        ///  功能：增加
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow, string tiproles);

        /// <summary>
        ///  功能：修改
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow, string tiproles);

        /// <summary>
        ///  功能：删除
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="taskCode">任务编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(string taskCode);

        /// <summary>
        ///  功能：获取任务列表
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="cateCode">任务分类编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string taskName, string cateCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 功能：根据任务编号获取角色
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="sTaskCode">编号</param>
        /// <returns></returns>
        MDataTable GetRoleTableByTaskCode(string sTaskCode);
    }
}
