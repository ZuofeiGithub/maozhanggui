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
    /// 功能：平台模板
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public class DecorationSysTemplateImpl : IDecorationSysTemplate
    {

        private const String CurrentTableName = DecorationDb.Table_Decoration_sys_template;

        /// <summary>
        /// 功能：初始化
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
        /// 功能：得到实体
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="tempLatecode">编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithTempLatecode(string tempLatecode, bool bCache = false)
        {
            String cacheKey = "DecorationTemplate-GetEntity" + tempLatecode;
            String strWhere = " templatecode=" + DbService.SetQuotesValue(tempLatecode);

            return DbService.GetOne(CurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 功能：增加
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
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
                exeMsgInfo.RetValue = "请输入正确的任务总天数";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("templateorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            string sWhere = "templatecode=" + DbService.SetQuotesValue(dataRow.Get("templatecode", ""));
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码已存在";
                return exeMsgInfo;
            }
            string sFields = "templatecode,templatename,totaldays,templateorder";

            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, sFields, true);

            return exeMsgInfo;
        }

        /// <summary>
        /// 功能：修改角色及相关信息
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow">角色实体</param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns>返回ExeMsgInfo</returns>
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
            if (!VerificationHelper.CheckStr(dataRow.Get("totaldays", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的任务总天数";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("templateorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            string sWhere = "templatecode=" + DbService.SetQuotesValue(dataRow.Get("templatecode", ""));
         
            string sFields = "templatename,totaldays,templateorder";

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, sFields, true);

            return exeMsgInfo;
        }

        /// <summary>
        /// 功能：删除
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="tempLatecode">编码</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string tempLatecode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(tempLatecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String sWhere = " templatecode=" + DbService.SetQuotesValue(tempLatecode);

            //平台类型
            if (DbService.Exists(DecorationDb.Table_Decoration_sys_taskcate, sWhere))
            {

                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前编号已在平台类型表中存在,不可删除!";
                return exeMsgInfo;
            }
            try
            {
                using (MAction action = new MAction(CurrentTableName))
                {
                    try
                    {
                        action.BeginTransation();
                       
                        action.Delete(sWhere); 

                        exeMsgInfo.RetStatus = 103;
                        exeMsgInfo.RetValue = "删除成功";
                    }
                    catch (Exception ex)
                    {
                        action.RollBack();
                        exeMsgInfo.RetStatus = 400;
                        exeMsgInfo.RetValue = ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 功能：列表信息
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="tempLatename">模板名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string tempLatename, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
           
            if (!String.IsNullOrEmpty(tempLatename))
            {
                sWhere += " and templatename like '%" + tempLatename + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " templateorder desc";
            }
            sWhere += " order by "+ orderBy;

            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 功能：获得所有模板
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public MDataTable GetTemplateTable()
        {
            string sWhere = "1=1";

            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }

    }
}
