using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface ICaseRole
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
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="casecode">案例编号</param>
        /// <returns></returns>
        ExeMsgInfo Add(List<MDataRow> dataRow, string casecode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据案例编号 获取案例角色
        /// </summary>
        /// <param name="casecode">编号</param>
        /// <returns></returns>
         MDataTable GetAllRole(string casecode);
    }
}