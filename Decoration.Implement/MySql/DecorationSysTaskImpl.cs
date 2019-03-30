
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;
using Decoration.Interface.Entity;
using System;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 功能：平台施工任务
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public class DecorationSysTaskImpl : IDecorationSysTask
    {

        private const String CurrentTableName = DecorationDb.Table_Decoration_sys_task;
        private const String VCurrentTableName = "decoration_sys_vtask";
        
        /// <summary>
        ///  功能：初始化
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        ///  功能：得到实体
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="taskCode"></param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        public MDataRow GetEntityWithTaskCode(string taskCode, bool bCache = false)
        {
            String cacheKey = "Task-GetEntity" + taskCode;
            String strWhere = "taskcode=" + DbService.SetQuotesValue(taskCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        ///  功能：增加
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow, string tiproles)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String taskCode = dataRow.Get("taskcode", "").ToString();

            if (String.IsNullOrEmpty(taskCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String taskname = dataRow.Get("taskname", "").ToString();
            if (String.IsNullOrEmpty(taskname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务名称不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("taskdays", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的任务天数";
                return exeMsgInfo;
            }
            String catecode = dataRow.Get("catecode", "").ToString();
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务类型不能为空";
                return exeMsgInfo;
            }
          
            if (DbService.Exists(CurrentTableName, "taskname=" + DbService.SetQuotesValue(taskname)))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "任务名称已经存在,不能重复添加";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("taskorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            string sWhere = "taskcode=" + DbService.SetQuotesValue(taskCode);
            string sFields = "";
            using (MAction action = new MAction(CurrentTableName))
            {
                action.BeginTransation();

                try
                {
                    action.Data.LoadFrom(dataRow);
                    sFields = "taskcode,taskname,taskremark,catecode,taskdays,taskorder";
                    DbService.SetMAction(action, dataRow, sFields);

                    //先修改主表
                    if (action.Insert(InsertOp.None))
                    {
                        //切换可提醒角色
                        action.ResetTable(DecorationDb.Table_Decoration_sys_taskrole);

                        action.Delete(sWhere);

                        foreach (string item3 in tiproles.Split(','))
                        {
                            action.Set("rolecode", item3);
                            action.Set("taskcode", taskCode);
                            action.Insert(InsertOp.None);
                        }
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
        ///  功能：修改
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow, string tiproles)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String taskCode = dataRow.Get("taskcode", "").ToString();

            if (String.IsNullOrEmpty(taskCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String taskname = dataRow.Get("taskname", "").ToString();
            if (String.IsNullOrEmpty(taskname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务名称不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("taskdays", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的任务天数";
                return exeMsgInfo;
            }
            String catecode = dataRow.Get("catecode", "").ToString();
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务类型不能为空";
                return exeMsgInfo;
            }
        
            String sIsWhere = " taskname=" + DbService.SetQuotesValue(taskname) + " and taskcode <> " + DbService.SetQuotesValue(taskCode);
            if (DbService.Exists(CurrentTableName, sIsWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "任务名称已经存在,不能重复添加";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("taskorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            String sWhere = "taskcode=" + DbService.SetQuotesValue(taskCode);


            string sWhere2 = "taskcode=" + DbService.SetQuotesValue(taskCode);
            string sFields = "";
            using (MAction action = new MAction(CurrentTableName))
            {
                action.BeginTransation();

                try
                {
                    action.Data.LoadFrom(dataRow);
                    sFields = "taskname,taskremark,catecode,taskdays,taskorder";
                    DbService.SetMAction(action, dataRow, sFields);

                    //先修改主表
                    if (action.Update(sWhere2))
                    {
                        //切换可提醒角色
                        action.ResetTable(DecorationDb.Table_Decoration_sys_taskrole);

                        action.Delete(sWhere);

                        foreach (string item3 in tiproles.Split(','))
                        {
                            action.Set("rolecode", item3);
                            action.Set("taskcode", taskCode);
                            action.Insert(InsertOp.None);
                        }
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
    ///  功能：删除
    /// 创建时间：2018.11.13
    /// 创建人：张硕
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
    ///  功能：获取任务列表
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    /// <param name="taskName">任务名称</param>
    /// <param name="cateCode">任务分类编号</param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="orderBy"></param>
    /// <param name="recordCount"></param>
    /// <param name="pageCount"></param>
    /// <returns></returns>
    public MDataTable GetList(string taskName, string cateCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
    {
        String sWhere = " 1=1 ";

        string sql = " select  a.*,b.catename,b.cateorder  from  decoration_sys_task as a" +
                     " LEFT JOIN decoration_sys_taskcate as b on a.catecode = b.catecode";


        if (!String.IsNullOrEmpty(taskName))
        {
            sWhere += " and taskname like '%" + taskName + "%'";
        }
        if (!String.IsNullOrEmpty(cateCode))
        {
            sWhere += " and catecode like '%" + cateCode + "%'";
        }
        //if (String.IsNullOrEmpty(orderBy))
        //{
        //    orderBy = "  taskorder desc,catecode desc";
        //}
        sWhere += " order by cateorder desc ,taskorder desc ";

        return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
    }

    /// <summary>
    /// 功能：根据任务编号获取角色
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    /// <param name="sTaskCode">编号</param>
    /// <returns></returns>
    public MDataTable GetRoleTableByTaskCode(string sTaskCode)
    {
        string sWhere = "1=1  and taskcode=" + DbService.SetQuotesValue(sTaskCode);

        return DbService.GetTable(DecorationDb.Table_Decoration_sys_taskrole, 0, sWhere);
    }

    }
}
