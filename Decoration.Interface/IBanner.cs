using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    /// 首页轮换图
    /// 王浩
    /// 2018-10-17
    /// </summary>
    public interface IBanner
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();



         MDataTable GetSelectRole(string bannerCode);
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="bannerCode">轮换图编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithBannerCode(String bannerCode, bool bCache = false);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);
        ExeMsgInfo Add(MDataRow dataRow, string rolecodes);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        ExeMsgInfo Update(MDataRow dataRow, string rolecodes);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="bannerCode">轮换图编号</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String bannerCode);

        /// <summary>
        /// 获取首页轮换图信息列表
        /// </summary>
        /// <param name="bannerTitle">标题</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(String bannerTitle, string companyCode, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataTable GetBannerByCompanyCode(string companyCode);


        /// <summary>
        /// 根据用户编号 获取所有轮播图（按角色）
        /// </summary>
        /// <param name="usercode">用户编号</param>
        /// <param name="num">获取数量</param>
        /// <returns></returns>
        MDataTable GetBannerByUserCode(string usercode, int num);

    }
}
