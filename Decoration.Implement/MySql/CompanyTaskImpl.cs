using Decoration.Interface;
using System;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Adf.Core.Db;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 王浩
    /// 2018-10-18
    /// 企业施工任务列表
    /// </summary>
    public class CompanyTaskImpl : ICompanyTask
    {
        private const String CurrentTableName = "decoration_companytask";
        private const String VCurrentTableName = "decoration_companytask";


        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="taskCode"></param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        public MDataRow GetEntityWithTaskCode(string taskCode, bool bCache = false)
        {
            String cacheKey = "CompanyTask-GetEntity" + taskCode;
            String strWhere = "taskCode=" + DbService.SetQuotesValue(taskCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加一个首页轮换图信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String taskCode = dataRow["taskCode"].ToString();

            if (String.IsNullOrEmpty(taskCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String taskname = dataRow["taskname"].ToString();
            if (String.IsNullOrEmpty(taskname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务名称不能为空";
                return exeMsgInfo;
            }
            String catecode = dataRow["catecode"].ToString();
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "阶段编码不能为空";
                return exeMsgInfo;
            }
            String taskdays = dataRow["taskdays"].ToString();
            if (String.IsNullOrEmpty(taskdays))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务天数不能为空";
                return exeMsgInfo;
            }
            //if (DbService.Exists(CurrentTableName, "taskname=" + DbService.SetQuotesValue(dataRow.Get("taskname", ""))))
            //{
            //    exeMsgInfo.RetStatus = 200;
            //    exeMsgInfo.RetValue = "任务名称已经存在,不能重复添加";
            //    return exeMsgInfo;
            //}

            String controlFieldNames = "taskcode,taskname,taskremark,catecode,taskdays,taskorder";
            return DbService.Insert(CurrentTableName, dataRow, controlFieldNames,true);
        }

        /// <summary>
        /// 根据编码修改一条企业信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String taskCode = dataRow["taskCode"].ToString();

            if (String.IsNullOrEmpty(taskCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String taskname = dataRow["taskname"].ToString();
            if (String.IsNullOrEmpty(taskname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务名称不能为空";
                return exeMsgInfo;
            }
           
            String catecode = dataRow["catecode"].ToString();
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "阶段编码不能为空";
                return exeMsgInfo;
            }
            //String aWhere = " taskname=" + DbService.SetQuotesValue(taskname) + " and taskcode <> " + DbService.SetQuotesValue(taskCode)+" and  catecode";
            //if (DbService.Exists(CurrentTableName, aWhere))
            //{
            //    exeMsgInfo.RetStatus = 400;
            //    exeMsgInfo.RetValue = "任务名称已经存在,不能重复添加";
            //    return exeMsgInfo;
            //}

            String taskdays = dataRow["taskdays"].ToString();
            if (String.IsNullOrEmpty(taskdays))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务天数不能为空";
                return exeMsgInfo;
            }

            String sWhere = "taskCode=" + DbService.SetQuotesValue(taskCode);

            String controlFieldNames = "taskname,taskremark,catecode,taskdays,taskorder";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }
        /// <summary>
        /// 根据编码删除一条信息
        /// </summary>
        /// <param name="taskCode">任务编码</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string taskCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(taskCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            String sWhere = "taskCode=" + DbService.SetQuotesValue(taskCode);
            return DbService.Delete(CurrentTableName, sWhere);
        }



        /// <summary>
        /// 获取企业施工任务列表
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="cateCode">任务分类编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string taskName, string companyCode, string cateCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount, string templatecode = "")
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";
            string sql = " select  a.*,b.catename,b.cateorder  from  decoration_companytask as a" +
                         " LEFT JOIN decoration_companytaskcate as b on a.catecode = b.catecode  where b.companycode = '" + companyCode + "'";

            if (!String.IsNullOrEmpty(templatecode))
            {
                sql += " and b.templatecode = '" + templatecode + "'";
            }
            if (!String.IsNullOrEmpty(taskName))
            {
                sWhere += " and taskName like '%" + taskName + "%'";
            }
            if (!String.IsNullOrEmpty(cateCode))
            {
                sWhere += " and catecode like '%" + cateCode + "%'";
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by cateorder desc, taskorder desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 根据分类编号 获取数据
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <param name="catecode">分类编号</param>
        /// <returns></returns>
        public MDataTable GetList( string catecode)
        {
            string sWhere = " catecode=" +  DbService.SetQuotesValue(catecode);
            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }
      

        /// <summary>
        ///  初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        MDataRow ICompanyTask.InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }
    }

    
}
