using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 产品分类
    /// 侯鑫辉
    /// 2018.10.17
    /// </summary>
    public class ProductCateImpl : IProductCate
    {
        private const String CurrentTableName = "decoration_productcate";
        private const String VCurrentTableName = "decoration_productcate";

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
        /// <param name="cateCode">产品分类编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithCateCode(string cateCode, bool bCache = false)
        {
            String sWhere = " catecode=" + DbService.SetQuotesValue(cateCode);
            String cacheKey = "ProductCate-" + cateCode;

            return DbService.GetOne(VCurrentTableName, sWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加一个分类
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            string catecode = Guid.NewGuid().ToString("N");
            dataRow.Set("catecode", catecode);

            String catename = dataRow.Get("catename", "");
            if (String.IsNullOrEmpty(catename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写分类名称";
                return exeMsgInfo;
            }

            String parentcode = dataRow.Get("parentcode", "");
            if (String.IsNullOrEmpty(parentcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "父级编号不能为空";
                return exeMsgInfo;
            }

            String companycode = dataRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }

            String sWhere = " companycode=" + DbService.SetQuotesValue(companycode) + " and catename=" + DbService.SetQuotesValue(catename) + " and catecode<>" + DbService.SetQuotesValue(catecode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称已存在";
                return exeMsgInfo;
            }

            String controlFieldNames = "catecode,catename,parentcode,cateorder,companycode";
            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);

            return exeMsgInfo;
        }

        /// <summary>
        /// 更新一个分类
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            String catecode = dataRow.Get("catecode", "");
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类编码不能为空";
                return exeMsgInfo;
            }

            String catename = dataRow.Get("catename", "");
            if (String.IsNullOrEmpty(catename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称不能为空";
                return exeMsgInfo;
            }

            String companycode = dataRow.Get("companycode", "");
            String sWhere = " companycode=" + DbService.SetQuotesValue(companycode) + " and catename=" + DbService.SetQuotesValue(catename) + " and catecode<>" + DbService.SetQuotesValue(catecode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称已存在";
                return exeMsgInfo;
            }

            sWhere = " catecode=" + DbService.SetQuotesValue(catecode);
            String controlFieldNames = "catename,cateorder";

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 根据CateCode删除一个分类
        /// </summary>
        /// <param name="cateCode"></param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string cateCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String sWhere = " parentcode=" + DbService.SetQuotesValue(cateCode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "存在子级分类,不能删除";
                return exeMsgInfo;
            }

            sWhere = "catecode=" + DbService.SetQuotesValue(cateCode);
            if (DbService.Exists("decoration_product", sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "已经用于产品表中，不能删除";
                return exeMsgInfo;
            }

            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
            return exeMsgInfo;
        }

        /// <summary>
        /// 得到所有的分类
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="parentCode">父级分类编码</param>
        /// <param name="cateName">分类名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListData(string companyCode, string parentCode, string cateName, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode) + " and parentcode=" + DbService.SetQuotesValue(parentCode);

            if (!String.IsNullOrEmpty(cateName))
            {
                sWhere += " and catename like '%" + cateName + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by cateorder desc";
            }

            sWhere += orderBy;
            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 得到一个父级编码下所有的子分类
        /// </summary>
        /// <param name="parentCode">父级分类编码</param>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public MDataTable GetChildAll(string parentCode, string companyCode)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            if (string.IsNullOrEmpty(parentCode))
            {
                sWhere+= " and parentcode=" + DbService.SetQuotesValue(parentCode);
            }

            string sOrderby = " order by cateorder desc ";
            sWhere += sOrderby;
            return DbService.GetTable(VCurrentTableName, 0, sWhere);
        }

        /// <summary>
        /// 得到所有分类
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public MDataTable GetAll(string companyCode,string catecode)
        {
            
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode) + " and parentcode='"+ catecode + "'";
            string sOrderby = " order by cateorder desc ";
            sWhere += sOrderby;
            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }
    }
}
