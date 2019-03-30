using System;
using System.Collections.Generic;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class TaskImpl:ITask

    {
        #region Implementation of ITask
        private const String CurrentTableName = "decoration_task";
        private const String VCurrentTableName = "decoration_task";
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
        /// <param name="taskcode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByTaskcode(string taskcode)
        {
            string sWhere = " taskcode=" + DbService.SetQuotesValue(taskcode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">编号</param>
        /// <param name="catename">分类名称</param>
        /// <param name="catecode">分类编码</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetList(string projectcode, string catename,string catecode, int pageIndex, int pageSize, string orderBy,
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

            if (!string.IsNullOrEmpty(catecode))
            {
                sWhere += " and catecode=" + DbService.SetQuotesValue(catecode);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by taskstartdate ";
            }



            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="detail">项目任务集合</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                string sWhere = "taskcode=" + DbService.SetQuotesValue(dataRow.Get("taskcode", ""));
                if (DbService.Exists(CurrentTableName, sWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前编号已存在";
                    return exeMsgInfo;
                }
                sWhere = " projectcode=" + DbService.SetQuotesValue(dataRow.Get("projectcode", "")) + " and  catecode=" +
                         DbService.SetQuotesValue(dataRow.Get("catecode", ""));
                MDataRow projectRow = DbService.GetOne("decoration_taskcate", sWhere);


                int taskCateDays = projectRow.Get("days", 0);

                if (VerificationHelper.CheckTime(dataRow.Get("taskstartdate", DateTime.Now),
                        projectRow.Get("startdate", DateTime.Now)) && !VerificationHelper.CheckTime1(
                        dataRow.Get("taskstartdate", DateTime.Now), projectRow.Get("enddate", DateTime.Now)))
                {

                }
                else
                {
                    exeMsgInfo.RetValue = "开始日期必须在所在阶段时间范围内";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }
                if (VerificationHelper.CheckTime(dataRow.Get("taskenddate", DateTime.Now),
                        projectRow.Get("startdate", DateTime.Now)) && VerificationHelper.CheckTime(
                        projectRow.Get("enddate", DateTime.Now), dataRow.Get("taskenddate", DateTime.Now)))
                {

                }
                else
                {
                    exeMsgInfo.RetValue = "结束日期必须在所在阶段时间范围内";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }
                int sumDays = this.GetSumDayByProjectCode(dataRow.Get("projectcode", ""), dataRow.Get("catecode", ""),dataRow.Get("taskcode",""));

                if ((sumDays + dataRow.Get("taskdays", 0)) > taskCateDays)
                {
                    exeMsgInfo.RetValue = "阶段下任务总天数已超出该阶段设置天数";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }

                string sCanFilds = "taskcode,projectcode,taskname,taskstatus,taskdes,catecode,taskstartdate,taskenddate,taskdays,thumbnail";
                exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, sCanFilds, true);
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
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateBytaskcode(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                string sWhere = "projectcode=" + DbService.SetQuotesValue(dataRow.Get("projectcode", ""));
                //if (VerificationHelper.CheckProjectIsExecute(dataRow.Get("projectcode", "")))
                //{
                //    exeMsgInfo.RetValue = "当前项目正在执行，无法修改！";
                //    exeMsgInfo.RetStatus = 400;
                //    return exeMsgInfo;
                //}

                sWhere = " projectcode=" + DbService.SetQuotesValue(dataRow.Get("projectcode", "")) + " and  catecode=" +
                         DbService.SetQuotesValue(dataRow.Get("catecode", ""));
                MDataRow projectRow = DbService.GetOne("decoration_taskcate", sWhere);


                int taskCateDays = projectRow.Get("days", 0);

                if (VerificationHelper.CheckTime(dataRow.Get("taskstartdate", DateTime.Now),
                        projectRow.Get("startdate", DateTime.Now)) && !VerificationHelper.CheckTime1(
                        dataRow.Get("taskstartdate", DateTime.Now), projectRow.Get("enddate", DateTime.Now)))
                {
                   
                }
                else
                {
                    exeMsgInfo.RetValue = "开始日期必须在所在阶段时间范围内";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }
                if (VerificationHelper.CheckTime(dataRow.Get("taskenddate", DateTime.Now),
                        projectRow.Get("startdate", DateTime.Now)) && VerificationHelper.CheckTime(
                        projectRow.Get("enddate", DateTime.Now), dataRow.Get("taskenddate", DateTime.Now)))
                {
                   
                }
                else
                {
                    exeMsgInfo.RetValue = "结束日期必须在所在阶段时间范围内";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }
                int sumDays = this.GetSumDayByProjectCode(dataRow.Get("projectcode", ""), dataRow.Get("catecode", ""), dataRow.Get("taskcode", ""));

                if ((sumDays + dataRow.Get("taskdays", 0)) > taskCateDays)
                {
                    exeMsgInfo.RetValue = "阶段下任务总天数已超出该阶段设置天数";
                    exeMsgInfo.RetStatus = 400;
                    return exeMsgInfo;
                }
                sWhere = "taskcode=" + DbService.SetQuotesValue(dataRow.Get("taskcode", ""));
                string sCanFilds = "taskname,taskdes,taskstartdate,taskenddate,taskdays,thumbnail";
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
        /// <param name="taskcode">项目阶段编号</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="catecode">任务分类编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteBytaskcode(string taskcode, string projectcode, string catecode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode)+" and taskcode="+DbService.SetQuotesValue(taskcode)+" and catecode="+DbService.SetQuotesValue(catecode);
            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
            return exeMsgInfo;
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据分类编号 项目编号获取所有任务
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="cateCode">分类编号</param>
        /// <returns></returns>
        public MDataTable GetAll(string projectcode, string cateCode)
        {
            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode) ;
            if (!string.IsNullOrEmpty(cateCode))
            {
                sWhere += "and catecode = " + DbService.SetQuotesValue(cateCode);
            }
            return DbService.GetTable(CurrentTableName, 0, sWhere+" order by taskorder desc");
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：项目编号获取所有任务
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public MDataTable GetAll(string projectcode)
        {
            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode);
            return DbService.GetTable(CurrentTableName, 0, sWhere + " order by taskorder desc");
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据分类编号 项目编号获取所有任务
        /// </summary>
        /// <param name="taskcode">编号</param>
        /// <returns></returns>
        public MDataTable GetAllRole( string taskcode)
        {
            string sWhere = " taskcode=" + DbService.SetQuotesValue(taskcode);
          
            return DbService.GetTable("decoration_taskrole", 0, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：设置完成
        /// </summary>
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        public ExeMsgInfo Complete(string taskcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (string.IsNullOrEmpty(taskcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务编号不能为空";
                return exeMsgInfo;
            }
            string sWhere = "taskcode=" + DbService.SetQuotesValue(taskcode);
            MDataRow dataRow = DbService.GetOne(CurrentTableName, sWhere);
            dataRow.Set("taskstatus", 1);
            exeMsgInfo= DbService.Update(CurrentTableName, dataRow, sWhere, "taskstatus", true);
            if (exeMsgInfo.RetStatus != 400)
            {
                MDataRow task = DbService.GetOne("decoration_task", "taskcode="+DbService.SetQuotesValue(taskcode));
                int days = task.Get("taskdays", 0);
                MDataRow project = DbService.GetOne("decoration_project", "projectcode=" + DbService.SetQuotesValue(task.Get("projectcode", "")));

                project.Set("projectprocess", project.Get("projectprocess", 0) + days);
                DbService.Update("decoration_project", project,"projectcode='"+ task.Get("projectcode", "") + "'" ,"projectprocess", true);
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据项目编号，任务分类编号 获取该阶段下所有天数总和 排除当前任务
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="catecode">任务分类编号</param>
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        public int GetSumDayByProjectCode(string projectcode, string catecode,string taskcode)
        {
            int days = 0;
            string sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode) + " and taskcode <>'" + taskcode + "' and catecode="+DbService.SetQuotesValue(catecode);
            MDataTable table = DbService.GetTable(CurrentTableName, 0, sWhere);

            foreach (MDataRow dataRow in table.Rows)
            {
                days += dataRow.Get("taskdays", 0);
            }

            return days;
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：总数
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public int GetRemindCount(string companyCode)
        {
            string sql =
                @"select * from decoration_task where projectcode in (select projectcode from decoration_project where companycode='"+ companyCode + "' )";
            return DbService.GetTable(sql, 0, "").Rows.Count;
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
                if (!VerificationHelper.CheckStr(dataRow.Get("projectcode", "")))
                {
                    throw new Exception("项目编号不能为空");
                }

                if (!VerificationHelper.CheckStr(dataRow.Get("taskname", ""), 100))
                {
                    throw new Exception("任务名称不能为空或字符超出最大限制");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("taskstatus", "")))
                {
                    throw new Exception("任务状态不能为空");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("taskdes", ""), 500))
                {
                    throw new Exception("任务描述不能为空或字符超出最大限制");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("catecode", "")))
                {
                    
                        throw new Exception("任务分类编码不能为空");
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
        

        /// <summary>
        /// 删除项目任务
        /// 侯鑫辉 2018.11.13
        /// </summary>
        /// <returns></returns>
        public ExeMsgInfo DeleteByTaskCode(string taskCode)
        {
            string sWhere = " taskcode=" + DbService.SetQuotesValue(taskCode);
            ExeMsgInfo exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
            return exeMsgInfo;
        }

        public ExeMsgInfo SetCode(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = DbService.Insert("WxACode",dataRow,"",false);
            return exeMsgInfo;
        }


        public MDataRow GetCode(string newcode)
        {
            return DbService.GetOne("WxACode", "newcode='" + newcode + "'");
        }
    }
}