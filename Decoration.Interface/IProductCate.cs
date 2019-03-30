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
    /// 产品分类
    /// 侯鑫辉
    /// 2018.10.17
    /// </summary>
    public interface IProductCate
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="cateCode">产品分类编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithCateCode(String cateCode, bool bCache = false);

        /// <summary>
        /// 增加一个分类
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 更新一个分类
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 根据CateCode删除一个分类
        /// </summary>
        /// <param name="cateCode"></param>
        /// <returns></returns>
        ExeMsgInfo Delete(String cateCode);

        /// <summary>
        /// 得到所有的分类
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="parentCode">上级分类编号</param>
        /// <param name="cateName">分类名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListData(string companyCode, string parentCode, string cateName, int pageIndex, int pageSize, String orderBy, ref int recordCount,
            ref int pageCount);

        /// <summary>
        /// 得到一个父级编码下所有的子分类
        /// </summary>
        /// <param name="parentCode">父级分类编码</param>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataTable GetChildAll(String parentCode, String companyCode);

        /// <summary>
        /// 得到所有分类
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataTable GetAll(String companyCode, string catecode);
    }
}
