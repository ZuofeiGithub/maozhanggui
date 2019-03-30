using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class ProductCollectionImpl: IProductCollection
    {
        #region Implementation of IProductCollection

        private const String CurrentTableName = "decoration_product_collection";
        private const String VCurrentTableName = "decoration_product_vcollection";
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="productcode">商品编码</param>
        /// <param name="createusercode">创建人</param>
        /// <returns></returns>
        public ExeMsgInfo Add(string productcode, string createusercode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (!VerificationHelper.CheckStr(productcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "产品编码不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(createusercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建人编码不能为空";
                return exeMsgInfo;
            }
            MDataRow dataRow = this.InitDataRow();
            dataRow.Set("collectioncode", Guid.NewGuid().ToString("N"));
            dataRow.Set("productcode", productcode);
            dataRow.Set("createusercode", createusercode);
            dataRow.Set("createdatetime", DateTime.Now);
            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, "collectioncode,productcode,createusercode,createdatetime", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collectioncode">收藏编码</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string collectioncode)
        {
            string sWhere = " collectioncode=" + DbService.SetQuotesValue(collectioncode);
            return DbService.Delete(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 收藏列表
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string userCode, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            string sWhere = " 1=1";
            if (!string.IsNullOrEmpty(userCode))
            {
                sWhere += " and createUsercode=" + DbService.SetQuotesValue(userCode);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += " order by " + orderBy;
            }
            else
            {
                sWhere += " order by createdatetime desc";
            }

            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        #endregion
    }
}