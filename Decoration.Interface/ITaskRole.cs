using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface ITaskRole
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
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        ExeMsgInfo Add(List<MDataRow> dataRow, string taskcode);

    }
}