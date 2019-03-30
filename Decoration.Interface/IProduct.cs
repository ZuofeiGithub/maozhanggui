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
    /// 产品管理
    /// 侯鑫辉
    /// 2018.10.17
    /// </summary>
    public interface IProduct
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithProductCode(String productCode, bool bCache = false);

        /// <summary>
        /// 增加一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String productCode);

        /// <summary>
        /// 得到所有的产品信息
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
        MDataTable GetListByCompanyCode(string cateCode, string companyCode, string productName, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

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
        MDataTable GetListByCompanyCode(string companyCode, string cateCode, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 得到所有分类(不包括catecode=sys，目前只有一级分类)
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataTable GetProductCateWithCompanyCode(string companyCode);
    }
}
