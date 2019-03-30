using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
  public  class TaskTemplateImpl: ITaskTemplate
    {
        private const String CurrentTableName = "decoration_tasktemplate";
        private const String VCurrentTableName = "decoration_tasktemplate";

        /// <summary>
        /// 初始化
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
        /// <param name="templatecode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByTemplatecode(string templatecode)
        {
            string sWhere = " templatecode=" + DbService.SetQuotesValue(templatecode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (!VerificationHelper.CheckStr(dataRow.Get("templatecode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("templatename", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模板名称不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("totaldays", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务总天数不能为空";
                return exeMsgInfo;
            }

            if (DbService.Exists(CurrentTableName,
                "templatecode=" + DbService.SetQuotesValue(dataRow.Get("templatecode", ""))))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码已存在";
                return exeMsgInfo;
            }
            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, "templatecode,templatename,totaldays,companycode,templateorder", true);

            return exeMsgInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (!VerificationHelper.CheckStr(dataRow.Get("templatecode", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("templatename", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模板名称不能为空";
                return exeMsgInfo;
            }
            string sWhere = "templatecode=" + DbService.SetQuotesValue(dataRow.Get("templatecode", ""));
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模板名称已存在";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("totaldays", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务总天数不能为空";
                return exeMsgInfo;
            }

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, "templatename,totaldays,templateorder", true);

            return exeMsgInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string companyCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = "templatecode=" + DbService.SetQuotesValue(companyCode);
        exeMsgInfo = DbService.Delete(CurrentTableName,  sWhere);

        return exeMsgInfo;
        }

        public ExeMsgInfo Copy(string templatecode, string templatename, string lasttime)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();

                    string newtemplatecode = Guid.NewGuid().ToString();
                    MDataTable template = action.Select("templatecode=" + DbService.SetQuotesValue(templatecode));
                    foreach (MDataRow templateRow in template.Rows)
                    {
                        action.Data.LoadFrom(templateRow);
                        action.Set("templatecode", newtemplatecode);
                        action.Set("templatename", templatename);
                        action.Set("totaldays", lasttime);
                        action.Insert();

                        action.ResetTable("decoration_companytaskcate");
                        MDataTable taskcate = action.Select("templatecode=" + DbService.SetQuotesValue(templatecode));

                        foreach (MDataRow taskcateRow in taskcate.Rows)
                        {
                            string catecode = Guid.NewGuid().ToString();
                            action.Data.LoadFrom(taskcateRow);
                            action.Set("templatecode", newtemplatecode);
                            action.Set("catecode", catecode);
                            action.Insert();
                            action.ResetTable("decoration_companytask");
                            MDataTable task =
                                action.Select("catecode=" + DbService.SetQuotesValue(taskcateRow.Get("catecode", "")));

                            foreach (MDataRow taskRow in task.Rows)
                            {
                                action.Data.LoadFrom(taskRow);
                                action.Set("catecode", catecode);
                                action.Set("taskcode", Guid.NewGuid().ToString());
                                action.Insert();
                            }
                            action.ResetTable("decoration_companytaskcate");
                        }
                        action.ResetTable(CurrentTableName);

                    }
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "复制成功";
                }
                catch (Exception ex)
                {
                    action.RollBack();
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue =ex.Message;
                }
            }
            return exeMsgInfo;
        }



        /// <summary>
            /// 获得该企业任务模版
            /// </summary>
            /// <param name="companyCode">企业编号</param>
            /// <returns></returns>
            public MDataTable GetAll(string companyCode)
        {
            string sWhere = " companycode=" + DbService.SetQuotesValue(companyCode)+" order by templateorder desc";
            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="templatename">模板名称</param>
        /// <param name="companycode">企业编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string templatename,string companycode, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            string sWhere = "1=1";

            if (!string.IsNullOrEmpty(templatename))
            {
                sWhere += " and templatename like '%" + templatename + "%'";
            }

            if (!string.IsNullOrEmpty(companycode))
            {
                sWhere += " and companycode=" + DbService.SetQuotesValue(companycode);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by templateorder desc ";
            }



            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }
    }
}
