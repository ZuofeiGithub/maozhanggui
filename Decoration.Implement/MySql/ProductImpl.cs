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
    /// 产品管理
    /// 侯鑫辉
    /// 2018.10.17
    /// </summary>
    public class ProductImpl : IProduct
    {
        private const String CurrentTableName = "decoration_product";
        private const String VCurrentTableName = "decoration_product";

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
        /// <param name="productCode">产品编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithProductCode(string productCode, bool bCache = false)
        {
            String sWhere = " productcode=" + DbService.SetQuotesValue(productCode);
            String cacheKey = "Product-" + productCode;

            return DbService.GetOne(VCurrentTableName, sWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            dataRow.Set("productcode", Guid.NewGuid().ToString("N"));

            String productname = dataRow.Get("productname", "");
            if (String.IsNullOrEmpty(productname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写产品名称";
                return exeMsgInfo;
            }

            String catecode = dataRow.Get("catecode", "");
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择产品分类";
                return exeMsgInfo;
            }

            String companycode = dataRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }

            String createusercode = dataRow.Get("createusercode", "");
            if (String.IsNullOrEmpty(createusercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建人不能为空";
                return exeMsgInfo;
            }

            String createdatetime = dataRow.Get("createdatetime", "");
            if (String.IsNullOrEmpty(createdatetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建时间不能为空";
                return exeMsgInfo;
            }

            String productstatus = dataRow.Get("productstatus", "");
            if (String.IsNullOrEmpty(productstatus))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择产品状态";
                return exeMsgInfo;
            }

            String ishot = dataRow.Get("ishot", "");
            if (String.IsNullOrEmpty(ishot))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择是否推荐";
                return exeMsgInfo;
            }

            String productorder = dataRow.Get("productorder", "");
            if (String.IsNullOrEmpty(productorder))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写排序";
                return exeMsgInfo;
            }

            String sWhere = " companycode=" + DbService.SetQuotesValue(companycode) + " and productname=" + DbService.SetQuotesValue(productname) + " and productcode<>" + DbService.SetQuotesValue(dataRow.Get("productcode", ""));
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "产品名称已存在";
                return exeMsgInfo;
            }

            String controlFieldNames = "productcode,productname,productimg,catecode,companycode,createusercode,createdatetime,productstatus,ishot,productorder,productcontent";

            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            String productcode = dataRow.Get("productcode", "");
            if (String.IsNullOrEmpty(productcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "产品编号不能为空";
                return exeMsgInfo;
            }

            String productname = dataRow.Get("productname", "");
            if (String.IsNullOrEmpty(productname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写产品名称";
                return exeMsgInfo;
            }

            String catecode = dataRow.Get("catecode", "");
            if (String.IsNullOrEmpty(catecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择产品分类";
                return exeMsgInfo;
            }

            String productstatus = dataRow.Get("productstatus", "");
            if (String.IsNullOrEmpty(productstatus))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择产品状态";
                return exeMsgInfo;
            }

            String ishot = dataRow.Get("ishot", "");
            if (String.IsNullOrEmpty(ishot))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择是否推荐";
                return exeMsgInfo;
            }

            String productorder = dataRow.Get("productorder", "");
            if (String.IsNullOrEmpty(productorder))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写排序";
                return exeMsgInfo;
            }

            String companycode = dataRow.Get("companycode", "");
            String sWhere = " companycode=" + DbService.SetQuotesValue(companycode) + " and productname=" + DbService.SetQuotesValue(productname) + " and productcode<>" + DbService.SetQuotesValue(dataRow.Get("productcode", ""));
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "分类名称已存在";
                return exeMsgInfo;
            }

            sWhere = " productcode=" + DbService.SetQuotesValue(productcode);
            String controlFieldNames = "productname,productimg,catecode,productstatus,ishot,productorder,productcontent,price";

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string productCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(productCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "产品编号不能为空";
                return exeMsgInfo;
            }

            String sWhere = " productcode=" + DbService.SetQuotesValue(productCode);
            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);

            return exeMsgInfo;
        }

        /// <summary>
        /// 得到所有产品信息
        /// </summary>
        /// <param name="cateCode">分类编号</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="productName">产品名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListByCompanyCode(string cateCode, string companyCode, string productName, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            if (!String.IsNullOrEmpty(cateCode))
            {
                sWhere += " and catecode='" + cateCode + "'";
            }
            if (!String.IsNullOrEmpty(productName))
            {
                sWhere += " and productname like '%" + productName + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by productorder desc,createdatetime desc";
            }
            sWhere += orderBy;

            String sql = "select a.*,b.catename from decoration_product a left join decoration_productcate b on a.catecode=b.catecode";

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 前端得到所有产品信息
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="cateCode">分类编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListByCompanyCode(string companyCode, string cateCode, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            if (!String.IsNullOrEmpty(cateCode))
            {
                sWhere += " and catecode='" + cateCode + "'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by productorder desc,createdatetime desc";
            }
            sWhere += orderBy;

            String sql = "select a.*,b.catename from decoration_product a left join decoration_productcate b on a.catecode=b.catecode";

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 得到所有分类
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public MDataTable GetProductCateWithCompanyCode(string companyCode)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode) + " and catecode!='sys'";
            return DbService.GetTable("decoration_productcate", 0, sWhere);
        }
    }
}
