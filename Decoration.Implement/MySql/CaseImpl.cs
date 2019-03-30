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
    public class CaseImpl:ICase
    {
        #region Implementation of ICase

        private const String CurrentTableName = "decoration_case";
        private const String VCurrentTableName = "decoration_vcase";
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
        /// <param name="casecode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByCaseCode(string casecode)
        {
            string sWhere = " casecode=" + DbService.SetQuotesValue(casecode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">编号</param>
        /// <returns></returns>
        public MDataTable GetAll(string projectcode)
        {
            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode);
            return DbService.GetTable (CurrentTableName,0, sWhere);
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
            string sWhere = "1=1";

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
                sWhere += " order by caseorder desc ";
            }



            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="usercode">usercode</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetList(string usercode, string projectcode, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            string sWhere = "1=1";


            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by caseorder desc ";
            }

            string sql = @"
select * from decoration_vcase

where projectcode='"+projectcode+"' and   casecode in (select DISTINCT casecode from decoration_case_role where rolecode in ( select rolecode from decoration_companyuser_role where usercode='"+usercode+"' ))";

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRows">实体</param>
        /// <param name="projectcode">项目</param>
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
                    exeMsgInfo.RetValue = ex.Message;
                }
            }
            return exeMsgInfo;
        }
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="createusercode">创建人</param>
        /// <param name="caseprojectcode">案例项目编号</param>
        /// <returns></returns>
        public ExeMsgInfo Add(string casecode,string projectcode, string createusercode, string caseprojectcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();
            MDataRow dataRow = this.InitDataRow();
            dataRow.Set("projectcode", projectcode);
            dataRow.Set("createusercode", createusercode);
            dataRow.Set("caseprojectcode", caseprojectcode);
            dataRow.Set("createdatetime", DateTime.Now);
            dataRow.Set("caseorder", 0);
            dataRow.Set("ishot",0); 
            dataRow.Set("casecode", casecode); 
             exeMsgcheck = CheckDataRow(dataRow);
            ;
            if (exeMsgcheck.RetStatus == 100)
            {

                exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, "", false);
            }
            else
            {
                return exeMsgcheck;
            }
           
            return exeMsgInfo;
        }
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="casecode">编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteByCaseCode(string casecode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = " casecode=" + DbService.SetQuotesValue(casecode) ;
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
                if (!VerificationHelper.CheckStr(dataRow.Get("casecode", "")))
                {
                    throw new Exception("案例编号不能为空");
                }

                if (!VerificationHelper.CheckStr(dataRow.Get("projectcode", "")))
                {
                    throw new Exception("当前项目编号不能为空");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("caseprojectcode", "")))
                {
                    throw new Exception("案例项目编号不能为空");
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