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
    /// 【顾健】会员收藏
    /// </summary>
    public interface IProductCollection
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
        /// <param name="createusercode">创建人</param>
        /// <returns></returns>
        ExeMsgInfo Add(string productcode,string createusercode);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collectioncode">收藏编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String collectioncode);

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
        MDataTable GetList(string userCode, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);
    }
}
