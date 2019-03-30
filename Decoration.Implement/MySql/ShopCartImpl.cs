using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class ShopCartImpl:IShopCart
    {
        #region Implementation of IShopCart

        private const String CurrentTableName = "decoration_product_shopcart";
        private const String VCurrentTableName = "decoration_product_vshopcart";
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
        /// 增加
        /// </summary>
        /// <param name="productcode">商品编码</param>
        /// <param name="productnum">数量</param>
        /// <param name="createusercode">创建人</param>
        /// <returns></returns>
        public ExeMsgInfo Add(string productcode,int productnum, string createusercode)
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
            dataRow.Set("shopcartcode", Guid.NewGuid().ToString("N"));
            dataRow.Set("productcode", productcode);
            dataRow.Set("productnum", productnum);
            dataRow.Set("createusercode", createusercode);
            dataRow.Set("createdatetime", DateTime.Now);
            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, "shopcartcode,productnum,productcode,createusercode,createdatetime", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="productcode">商品编码</param>
        /// <param name="productnum">数量</param>
        /// <returns></returns>
        public ExeMsgInfo AddNumber(string shopcartcode, string productcode, int productnum)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (!VerificationHelper.CheckStr(shopcartcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "购物车不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(productcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "产品编码不能为空";
                return exeMsgInfo;
            }
            MDataRow dataRow =DbService.GetOne(CurrentTableName, "shopcartcode="+DbService.SetQuotesValue(shopcartcode));
            dataRow.Set("productnum", dataRow.Get("productnum",0)+productnum);
            return DbService.Update(CurrentTableName,  dataRow,
                "shopcartcode=" + DbService.SetQuotesValue(shopcartcode), "productnum", true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="shopcartcode">购物车编码</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string shopcartcode)
        {
            string sWhere = " shopcartcode=" + DbService.SetQuotesValue(shopcartcode);
            return DbService.Delete(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 购物车列表
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