using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    /// 公司列表
    /// 王浩
    /// 2018-10-13
    /// </summary>
    public interface ICompany
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 修改当前登录的企业信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo UpdateWithAdmin(MDataRow dataRow);
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="companyCode">公司编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithCompanyCode(String companyCode, bool bCache = false);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 企业用户初始化
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        ExeMsgInfo Restart(string companycode);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="companyCode">公司编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String companyCode);

       

        /// <summary>
        /// 启用操作
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        ExeMsgInfo Enable(string companycode);

        /// <summary>
        /// 获取公司信息列表
        /// </summary>
        /// <param name="companynName">公司名称</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string companynName, string companyCode, int pageIndex, int pageSize, String orderBy,
            ref int recordCount,
            ref int pageCount);


        /// <summary>
        /// 根据当前登录的用户编号，获取该用户的企业信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        MDataRow GetCompanyWithUserCode(string userCode);

        #region 管理员账号 管理企业 2018.10.20 侯鑫辉

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataRow AdminGetEntityWithCompanyCode(String companyCode);

        /// <summary>
        /// 得到企业列表
        /// </summary>
        /// <param name="companyName">企业名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable AdminGetList(string companyName, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        ExeMsgInfo AdminAdd(MDataRow dtRow);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        ExeMsgInfo AdminUpdate(MDataRow dtRow);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        ExeMsgInfo AdminDelete(String companyCode);

        #endregion
    }
}
