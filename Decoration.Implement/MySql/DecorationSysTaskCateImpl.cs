using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;
using Decoration.Interface.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decoration.Implement.MySql
{

    /// <summary>
    /// 功能：平台任务类型
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public class DecorationSysTaskCateImpl: IDecorationSysTaskCate
    {

        private const String CurrentTableName = DecorationDb.Table_Decoration_sys_taskcate;

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
        /// 功能： 得到实体
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="cateCode">分类编码</param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        public MDataRow GetEntityWithTaskcateCode(string cateCode, bool bCache = false)
        {
            String cacheKey = "Taskcate-GetEntity" + cateCode;
            String strWhere = "catecode=" + DbService.SetQuotesValue(cateCode);

            return DbService.GetOne(CurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 功能： 增加
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String catecode = dataRow.Get("catecode","").ToString();
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类编码不能为空";
                return exeMsgInfo;
            }
            String catename = dataRow.Get("catename", "").ToString(); 
            if (String.IsNullOrEmpty(catename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称不能为空";
                return exeMsgInfo;
            }     
            String templatecode = dataRow.Get("templatecode", "").ToString(); 
            if (String.IsNullOrEmpty(templatecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择模板";
                return exeMsgInfo;
            }
            if (DbService.Exists(CurrentTableName, "catename=" + DbService.SetQuotesValue(dataRow.Get("catename", "")) + " and catecode <> " + DbService.SetQuotesValue(catecode)))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "分类名称已经存在,不能重复添加";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("cateorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            String sWhere = "catecode=" + DbService.SetQuotesValue(catecode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前编码已经存在";
                return exeMsgInfo;
            }

            String controlFieldNames = "catecode,catename,cateorder,templatecode";
            return DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
        }

        /// <summary>
        /// 功能： 修改
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String catecode = dataRow.Get("catecode", "").ToString();
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类编码不能为空";
                return exeMsgInfo;
            }
            String catename = dataRow.Get("catename", "").ToString();
            if (String.IsNullOrEmpty(catename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称不能为空";
                return exeMsgInfo;
            }
            String templatecode = dataRow.Get("templatecode", "").ToString();
            if (String.IsNullOrEmpty(templatecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择模板";
                return exeMsgInfo;
            }

           
            String aWhere = " catename=" + DbService.SetQuotesValue(catename) + " and catecode <> " + DbService.SetQuotesValue(catecode);
            if (DbService.Exists(CurrentTableName, aWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称已经存在,不能重复添加";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(dataRow.Get("cateorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            String sWhere = "catecode=" + DbService.SetQuotesValue(catecode);

            String controlFieldNames = "catename,cateorder,templatecode";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }

        /// <summary>
        /// 功能： 删除
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="cateCode">任务编码</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string cateCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(cateCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            String sWhere = "catecode=" + DbService.SetQuotesValue(cateCode);
           
            //平台类型
            if (DbService.Exists(DecorationDb.Table_Decoration_sys_task, sWhere))
            {

                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前编号已在平台任务表中存在,不可删除!";
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
        /// 功能： 得到列表
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="cateName">分类名称</param>      
        /// <param name="tempLatecode">模板</param>      
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string cateName, string tempLatecode,  int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";
            string sql = "select a1.*,a2.templatename from decoration_sys_taskcate a1 left join decoration_sys_template a2 on a1.templatecode=a2.templatecode";

            if (!String.IsNullOrEmpty(tempLatecode))
            {
                sWhere += " and templatecode = '" + tempLatecode + "'";
            }

            if (!String.IsNullOrEmpty(cateName))
            {
                sWhere += " and cateName like '%" + cateName + "%'";
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by cateorder desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 功能：根据模板编号获取分类
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="sTemplateCode">编号</param>
        /// <returns></returns>
        public MDataTable GetTableByTemplateCode(string sTemplateCode)
        {
            string sWhere = "1=1  and templatecode=" + DbService.SetQuotesValue(sTemplateCode);

            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }

        /// <summary>
        /// 功能：获取分类
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public MDataTable GetTable()
        {
            string sWhere = "1=1";

            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }
    }
}
