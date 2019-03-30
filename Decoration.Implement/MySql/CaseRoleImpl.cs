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
    public class CaseRoleImpl:ICaseRole

    {
        #region Implementation of ICaseRole
        private const String CurrentTableName = "decoration_case_role";
        private const String VCurrentTableName = "decoration_case_role";
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
        /// <param name="casecode">案例编号</param>
        /// <returns></returns>
        public ExeMsgInfo Add(List<MDataRow> dataRow, string casecode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();
                    action.Delete("casecode=" + DbService.SetQuotesValue(casecode));
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

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据案例编号 获取案例角色
        /// </summary>
        /// <param name="casecode">编号</param>
        /// <returns></returns>
        public MDataTable GetAllRole(string casecode)
        {
            string sWhere = " casecode=" + DbService.SetQuotesValue(casecode);

            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }

        #endregion
    }
}