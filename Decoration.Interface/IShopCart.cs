using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    /// <summary>
    /// 【顾健】业主购物车
    /// </summary>
    public interface IShopCart
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="productcode">商品编码</param>
        /// <param name="productnum">数量</param>
        /// <param name="createusercode">创建人</param>
        /// <returns></returns>
        ExeMsgInfo Add(string productcode, int productnum, string createusercode);


        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="productcode">商品编码</param>
        /// <param name="productnum">数量</param>
        /// <param name="shopcartcode">购物车编码</param>
        /// <returns></returns>
        ExeMsgInfo AddNumber(string shopcartcode, string productcode, int productnum);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="shopcartcode">购物车编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String shopcartcode);

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
        MDataTable GetList(string userCode, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);
    }
}
