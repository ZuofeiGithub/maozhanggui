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
    public class TaskCateImpl : ITaskCate
    {
        #region Implementation of ITaskCate
        private const String CurrentTableName = "decoration_taskcate";
        private const String VCurrentTableName = "decoration_vtaskcate";
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
        /// <param name="projectcatecode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByProjectCateCode(string projectcatecode)
        {
            string sWhere = " projectcatecode=" + DbService.SetQuotesValue(projectcatecode);
            return DbService.GetOne(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="catename">分类名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetList(string projectcode, string catename, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            string sWhere = "1=1";

            if (!string.IsNullOrEmpty(projectcode))
            {
                sWhere += " and projectcode=" + DbService.SetQuotesValue(projectcode);
            }

        

            if (!string.IsNullOrEmpty(catename))
            {
                sWhere += " and catename like '%" + catename.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by cateorder desc";
            }

         

            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRows">数据集</param>
        /// <param name="detail">项目任务集合</param>
        /// <returns></returns>
        public ExeMsgInfo Add(List<MDataRow> dataRows,List<MDataRow>detail)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();
                    foreach (MDataRow dataRow in dataRows)
                    {
                        exeMsgcheck = CheckDataRow(dataRow);

                        if (exeMsgcheck.RetStatus == 100)
                        {
                            string sWhere = "projectcatecode=" + DbService.SetQuotesValue(dataRow.Get("projectcatecode", ""));
                            if (action.Exists(sWhere))
                            {
                                throw new Exception("当前编号已存在");
                            }
                      
                            action.Data.LoadFrom(dataRow);
                            action.Insert();
                        }
                        else
                        {
                            throw new Exception(exeMsgcheck.RetValue);
                        }
                    }
                    action.ResetTable("decoration_task");
                    foreach (MDataRow detailRow in detail)
                    {
                        string sWhereDetail = "taskcode=" + DbService.SetQuotesValue(detailRow.Get("taskcode", ""));
                        if (action.Exists(sWhereDetail))
                        {
                            throw new Exception("任务编号已存在");
                        }

                        if (!VerificationHelper.CheckStr(detailRow.Get("projectcode", "")))
                        {
                            throw new Exception("施工任务中项目编号不能为空");
                        }
                        if (!VerificationHelper.CheckStr(detailRow.Get("taskname", ""), 100))
                        {
                            throw new Exception("任务名称不能为空或超出字符限制");
                        }
                        if (!VerificationHelper.CheckStr(detailRow.Get("catecode", "")))
                        {
                            throw new Exception("任务分类编号不能为空");
                        }
                        action.Data.LoadFrom(detailRow);
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
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByProjectCateCode(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                string sWhere = "projectcode=" + DbService.SetQuotesValue(dataRow.Get("projectcode", ""));
                if (VerificationHelper.CheckProjectIsExecute(dataRow.Get("projectcode", "")))
                {
                    exeMsgInfo.RetValue = "当前项目正在执行，无法修改！";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }
                MDataRow projectRow = DbService.GetOne("decoration_project", sWhere);

             
                string projectEnd = projectRow.Get("enddate", "");
                int projectdays = projectRow.Get("projectdays", 0);

                if (VerificationHelper.CheckTime(dataRow.Get("startdate", DateTime.Now),
                        projectRow.Get("startdate", DateTime.Now)) && !VerificationHelper.CheckTime1(
                        dataRow.Get("startdate", DateTime.Now), projectRow.Get("enddate", DateTime.Now)))
                {

                }
                else
                {
                    exeMsgInfo.RetValue = "开始日期必须在所在项目时间范围内";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }
                if (VerificationHelper.CheckTime(dataRow.Get("enddate", DateTime.Now),
                        projectRow.Get("startdate", DateTime.Now)) && VerificationHelper.CheckTime(
                        projectRow.Get("enddate", DateTime.Now), dataRow.Get("enddate", DateTime.Now)))
                {

                }
                else
                {
                    exeMsgInfo.RetValue = "结束日期必须在所在项目时间范围内";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }

                int sumDays = this.GetSumDayByProjectCode(dataRow.Get("projectcode",""), dataRow.Get("projectcatecode", ""));

                if ((sumDays+dataRow.Get("days",0)) > projectRow.Get("projectdays", 0))
                {
                    exeMsgInfo.RetValue = "阶段总天数已超出项目周期天数";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }

                sWhere = "projectcatecode=" + DbService.SetQuotesValue(dataRow.Get("projectcatecode", ""));

                string sCanFilds = "catename,cateorder,startdate,enddate,days";
                exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, sCanFilds, true);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="projectcatecode">项目阶段编号</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="catecode">任务分类编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteByProjectCateCode(string projectcatecode,string projectcode,string catecode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();
            if (VerificationHelper.CheckProjectIsExecute(projectcode))
            {
                exeMsgInfo.RetValue = "当前项目正在执行，无法删除！";
                exeMsgInfo.RetStatus = 400;
                return exeMsgInfo;
            }

            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode)+ " and projectcatecode="+DbService.SetQuotesValue(projectcatecode);
            string sWhereCate= " projectcode=" + DbService.SetQuotesValue(projectcode) + " and catecode=" + DbService.SetQuotesValue(catecode);

            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();
                    if (action.Delete(sWhere))
                    {
                        action.ResetTable("decoration_task");
                        action.Delete(sWhereCate);
                    }
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "删除成功";

                }
                catch (Exception ex)
                {
                    action.RollBack();
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = ex.Message;
                }
                return exeMsgInfo;
            }
        }


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据项目编号获取所有阶段天数总和 排除当前阶段
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectcatecode">阶段编号</param>
        /// <returns></returns>
        public int GetSumDayByProjectCode(string projectcode,string projectcatecode)
        {
            int days = 0;
            string sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode)+ " and projectcatecode <>'"+ projectcatecode + "'";
            MDataTable table = DbService.GetTable(CurrentTableName, 0, sWhere);

            foreach (MDataRow dataRow in table.Rows)
            {
                days += dataRow.Get("days", 0);
            }

            return days;
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据项目编号获取所有数据
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public MDataTable GetAll(string projectcode)
        {
            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode) + "  order by cateorder desc";
            return DbService.GetTable(CurrentTableName, 0, sWhere);
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
                if (!VerificationHelper.CheckStr(dataRow.Get("projectcatecode", "")))
                {
                    throw new Exception("项目阶段编号不能为空");
                }

                if (!VerificationHelper.CheckStr(dataRow.Get("projectcode", "")))
                {
                    throw new Exception("项目编号不能为空");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("catecode", "")))
                {
                    throw new Exception("任务分类编号不能为空");
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