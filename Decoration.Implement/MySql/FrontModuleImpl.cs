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
using Decoration.Interface.Entity;

namespace Decoration.Implement.MySql
{
   public class FrontModuleImpl: IFrontModule
    {
        private const String CurrentTableName = DecorationDb.Table_Decoration_FrontModule;
        private const String VCurrentTableName = DecorationDb.Table_Decoration_FrontModule;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="frontModuleCode">前端模块编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithFrontModuleCode(string frontModuleCode, bool bCache = false)
        {
            String sWhere = " frontModuleCode=" + DbService.SetQuotesValue(frontModuleCode);
            String cacheKey = "frontModuleCode-" + frontModuleCode;

            return DbService.GetOne(VCurrentTableName, sWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 功能：新增
        /// 创建人：王孝为
        /// 时间2018-11-07
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            dataRow.Set("frontmodulecode", Guid.NewGuid().ToString("N"));

            String frontmodulename = dataRow.Get("frontmodulename", "");
            if (String.IsNullOrEmpty(frontmodulename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模块名称不能为空";
                return exeMsgInfo;
            }

            String frontmoduletype = dataRow.Get("frontmoduletype", "");
            if (String.IsNullOrEmpty(frontmoduletype))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择模块分类";
                return exeMsgInfo;
            }

            String controlFieldNames = "frontmodulecode,frontmodulename,frontmoduleicon,isshow,frontmoduleurl,isredirect,redirecturl,frontmoduletype,frontmoduleorder";

            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 功能：修改
        /// 创建人：王孝为
        /// 时间2018-11-07
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            String frontmodulecode = dataRow.Get("frontmodulecode", "");
            if (String.IsNullOrEmpty(frontmodulecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模块编号不能为空";
                return exeMsgInfo;
            }

            String frontmodulename = dataRow.Get("frontmodulename", "");
            if (String.IsNullOrEmpty(frontmodulename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模块名称不能为空";
                return exeMsgInfo;
            }

            String frontmoduletype = dataRow.Get("frontmoduletype", "");
            if (String.IsNullOrEmpty(frontmoduletype))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择模块分类";
                return exeMsgInfo;
            }

            String sWhere = " frontmodulecode=" + DbService.SetQuotesValue(frontmodulecode);
            String controlFieldNames = "frontmodulename,frontmoduleicon,isshow,frontmoduleurl,isredirect,redirecturl,frontmoduletype,frontmoduleorder";

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除一个前端模块
        /// 王孝为
        /// 2018-11-7
        /// </summary>
        /// <param name="frontmodulecode">模块编号</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string frontmodulecode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(frontmodulecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模块编号不能为空";
                return exeMsgInfo;
            }

            String sWhere = " frontmodulecode=" + DbService.SetQuotesValue(frontmodulecode);
            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);

            return exeMsgInfo;
        }

        /// <summary>
        /// 功能：列表
        /// 创建人：王孝为
        /// 时间2018-11-07
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " ";

            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by frontmoduleorder desc";
            }

            sWhere += orderBy;
            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }


        #region 奚德荣 2018.11.8
        /// <summary>
        /// 获取项目详细页中的所有模块
        /// </summary>
        /// <returns></returns>
        public MDataTable GetFrontModuleForProject()
        {
            string sWhere = "frontmoduletype = 0  order by frontmoduleorder desc ";
            string tablename = DecorationDb.Table_Decoration_FrontModule;
            MDataTable dt = DbService.GetTable(tablename, 0, sWhere) ?? new MDataTable();
            return dt;
        }




        /// <summary>
        /// 获取项目列表页中的所有状态
        /// </summary>
        /// <returns></returns>
        public MDataTable GetFrontModuleForProjectStatus()
        {
            string sWhere = "frontmoduletype = 1  order by frontmoduleorder desc ";
            string tablename = DecorationDb.Table_Decoration_FrontModule;
            MDataTable dt = DbService.GetTable(tablename, 0, sWhere) ?? new MDataTable();
            return dt;
        }


        /// <summary>
        /// 获取项目详细页中的所有模块
        /// </summary>
        /// <returns></returns>
        public MDataTable GetFrontModuleForProject(string rolecode)
        {
            string sWhere = "rolecode='" + rolecode + "' and frontmoduletype = 0 ";
            string tablename = "decoration_role_frontmodule";
            MDataTable dt = DbService.GetTable(tablename, 0, sWhere) ?? new MDataTable();
            return dt;
        }




        /// <summary>
        /// 获取项目列表页中的所有状态
        /// </summary>
        /// <returns></returns>
        public MDataTable GetFrontModuleForProjectStatus(string rolecode)
        {
            string sWhere = " rolecode='" + rolecode + "' and frontmoduletype = 1   ";
            string tablename = "decoration_role_frontmodule";
            MDataTable dt = DbService.GetTable(tablename, 0, sWhere) ?? new MDataTable();
            return dt;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="usercode">编号</param>
        /// <returns></returns>
        public MDataTable GetAllModuleForUserCode(string usercode)
        {
            string sql = @"
select * from Decoration_FrontModule

where frontmodulecode in (select DISTINCT frontmodulecode from decoration_role_frontmodule where rolecode in(select rolecode from decoration_companyuser_role where usercode ='"+ usercode + "')  and frontmoduletype = 0 )";
            return DbService.GetTable(sql, 0, " order by frontmoduleorder desc");
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="usercode">编号</param>
        /// <returns></returns>
        public MDataTable GetAllStatusForUserCode(string usercode)
        {
            string sql = @"
select * from Decoration_FrontModule

where frontmodulecode in (select DISTINCT frontmodulecode from decoration_role_frontmodule where rolecode in(select rolecode from decoration_companyuser_role where usercode ='" + usercode + "')  and frontmoduletype = 1 )";
            return DbService.GetTable(sql, 0, " order by frontmoduleorder desc");
        }

        #endregion
    }
}
