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
    /// 2018-10-13
    /// 任务分类信息
    /// </summary>
    public class CompanyTaskcateImpl : ICompanyTaskcate
    {
        private const String CurrentTableName = "decoration_companytaskcate";
        private const String VCurrentTableName = "decoration_companytaskcate";


      
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="cateCode">分类编码</param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        public MDataRow GetEntityWithTaskcateCode(string cateCode, bool bCache = false)
        {
            String cacheKey = "CompanyTaskcate-GetEntity" + cateCode;
            String strWhere = "catecode=" + DbService.SetQuotesValue(cateCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加企业任务分类信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String catecode = dataRow["catecode"].ToString();
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类编码不能为空";
                return exeMsgInfo;
            }
            String catename = dataRow["catename"].ToString();
            if (String.IsNullOrEmpty(catename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称不能为空";
                return exeMsgInfo;
            }
            String companycode = dataRow["companycode"].ToString();
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }

            String templatecode = dataRow["templatecode"].ToString();
            if (String.IsNullOrEmpty(templatecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择模板";
                return exeMsgInfo;
            }
            //if (DbService.Exists(CurrentTableName, "catename=" + DbService.SetQuotesValue(dataRow.Get("catename", ""))+ " and companycode <> " + DbService.SetQuotesValue(companycode)))
            //{
            //    exeMsgInfo.RetStatus = 200;
            //    exeMsgInfo.RetValue = "分类名称已经存在,不能重复添加";
            //    return exeMsgInfo;
            //}
          
            String sWhere = "catecode=" + DbService.SetQuotesValue(catecode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前编码已经存在";
                return exeMsgInfo;
            }

            String controlFieldNames = "catecode,catename,cateorder,companycode,templatecode";
            return DbService.Insert(CurrentTableName, dataRow, controlFieldNames,true);
        }

        /// <summary>
        /// 修改企业任务分类信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String catecode = dataRow["catecode"].ToString();

            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类编码不能为空";
                return exeMsgInfo;
            }
            String templatecode = dataRow["templatecode"].ToString();
            if (String.IsNullOrEmpty(templatecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择模板";
                return exeMsgInfo;
            }
            String catename = dataRow["catename"].ToString();
            if (String.IsNullOrEmpty(catename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称不能为空";
                return exeMsgInfo;
            }

            String companycode = dataRow["companycode"].ToString();
            //String aWhere = " catename=" + DbService.SetQuotesValue(catename) + " and catecode <> " + DbService.SetQuotesValue(catecode) + " and companycode <> " + DbService.SetQuotesValue(companycode);
            //    if (DbService.Exists(CurrentTableName, aWhere))
            //    {
            //        exeMsgInfo.RetStatus = 400;
            //        exeMsgInfo.RetValue = "分类名称已经存在,不能重复添加";
            //        return exeMsgInfo;
            //    }
          

            String sWhere = "catecode=" + DbService.SetQuotesValue(catecode);

            String controlFieldNames = "catename,cateorder,templatecode";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }
        /// <summary>
        /// 删除任务分类信息
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

            String sWhere = "cateCode=" + DbService.SetQuotesValue(cateCode);
            return DbService.Delete(CurrentTableName, sWhere);
        }



        /// <summary>
        /// 获取任务分类数据（弹窗使用）
        /// </summary>
        /// <param name="cateName">分类名称</param>
        /// <param name="companycode">企业编号</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetAll(string cateName, string companycode, string projectcode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";
            string sql = "select * from decoration_companytaskcate where catecode NOT in(select catecode from decoration_taskcate where projectcode='"+ projectcode + "') and templatecode = (select templatecode from decoration_project  where projectcode ='"+ projectcode + "')";


            if (!String.IsNullOrEmpty(cateName))
            {
                sWhere += " and cateName like '%" + cateName + "%'";
            }
            if (!String.IsNullOrEmpty(cateName))
            {
                sWhere += " and companycode ="+DbService.SetQuotesValue(companycode);
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by cateorder desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }



        /// <summary>
        /// 获取企业施工任务分类列表
        /// </summary>
        /// <param name="cateName"></param>
        /// <param name="companyCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string cateName, string companyCode,  int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount,string templatecode = "")
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";
            string sql = "select a1.*,a2.templatename from decoration_companytaskcate a1 left join decoration_tasktemplate a2 on a1.templatecode=a2.templatecode where a1.companycode='" + companyCode + "'";

            if (!String.IsNullOrEmpty(templatecode))
            {
                sWhere += " and templatecode = '" + templatecode + "'";
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
        ///  初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        MDataRow ICompanyTaskcate.InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        /// 获取所有的任务分类
        /// </summary>
        /// <returns></returns>
        public MDataTable GetEntityByCateCode(string companycode)
        {
            return DbService.GetTable(CurrentTableName,0, "companycode="+DbService.SetQuotesValue(companycode));
        }

        /// <summary>
        /// 获取所有的任务分类
        /// </summary>
        /// <returns></returns>
        public MDataTable GetEntityByCateCode(string companycode,string templatecode)
        {
            return DbService.GetTable(CurrentTableName, 0, "companycode=" + DbService.SetQuotesValue(companycode)+ " and templatecode="+DbService.SetQuotesValue(templatecode)+" order by cateorder desc");
        }

    }
}
