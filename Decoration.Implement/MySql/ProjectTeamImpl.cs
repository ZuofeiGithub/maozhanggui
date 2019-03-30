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
    public class ProjectTeamImpl : IProjectTeam
    {
        #region Implementation of IProjectTeam
        private const String CurrentTableName = "decoration_projectteam";
        private const String VCurrentTableName = "decoration_vprojectteam";
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
        /// 功能：获取实体
        /// </summary>
        /// <param name="teamcode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByTeamCode(string teamcode)
        {
            string sWhere = " teamcode=" + DbService.SetQuotesValue(teamcode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

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
        public MDataTable GetList(string projectcode, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            string sWhere = "1=1 ";

            if (!string.IsNullOrEmpty(projectcode))
            {
                sWhere += " and projectcode=" + DbService.SetQuotesValue(projectcode);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by uid desc ";
            }

            string sql = @"
select *,GROUP_CONCAT(rolename,',') as rolenames from decoration_vprojectteam
group by projectcode,usercode";

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRows">实体</param>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public ExeMsgInfo Add(List<MDataRow> dataRows,string projectcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();
                    string sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode);

                    action.Delete(sWhere);
                    foreach (MDataRow dataRow in dataRows)
                    {
                        //最终验证
                        exeMsgcheck = CheckDataRow(dataRow);

                        if (exeMsgcheck.RetStatus == 100)
                        {
                            action.Data.LoadFrom(dataRow);
                            action.Insert();
                        }
                        else
                        {
                            throw new Exception(exeMsgcheck.RetValue);
                        }
                    }
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "选择成功";
                }
                catch (Exception ex)
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue =ex.Message;
                }
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="teamcode">编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteByTeamCode(string teamcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = " teamcode=" + DbService.SetQuotesValue(teamcode);
            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
            return exeMsgInfo;
        }

        #endregion

        #region 实体公共验证方法

        /// <summary>
        /// 验证数据是否通过
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public static ExeMsgInfo CheckDataRow(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (!VerificationHelper.CheckStr(dataRow.Get("teamcode", "")))
                {
                    throw new Exception("团队编号不能为空");
                }

                if (!VerificationHelper.CheckStr(dataRow.Get("projectcode", "")))
                {
                    throw new Exception("项目编号不能为空");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("usercode", "")))
                {
                    throw new Exception("企业用户编号不能为空");
                }
                exeMsgInfo.RetStatus = 100;
            }
            catch (Exception ex)
            {

                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;
        }
        #endregion
    }
}