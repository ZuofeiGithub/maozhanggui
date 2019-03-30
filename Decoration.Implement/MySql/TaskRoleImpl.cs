using System;
using System.Collections.Generic;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class TaskRoleImpl :ITaskRole
    {
        #region Implementation of ITaskRole
        private const String CurrentTableName = "decoration_taskrole";
        private const String VCurrentTableName = "decoration_taskrole";
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        public ExeMsgInfo Add(List<MDataRow> dataRow,string taskcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();
                    action.Delete("taskcode=" + DbService.SetQuotesValue(taskcode));
                    foreach (MDataRow mDataRow in dataRow)
                    {
                        action.Data.LoadFrom(mDataRow);
                        action.Insert();
                    }
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "增加成功";
                }
                catch (Exception ex)
                {
                    action.RollBack();
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = ex.Message;
                }
            }
            return exeMsgInfo;
        }

        #endregion
    }
}