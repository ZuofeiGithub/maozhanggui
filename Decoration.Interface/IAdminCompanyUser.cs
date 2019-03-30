using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    /// 管理员信息
    /// 王浩
    /// 2018-10-17
    /// </summary>
    public interface IAdminCompanyUser
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 获取企业用户信息列表
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="companyCode">用户名称</param>
        /// <param name="cellphone">手机号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string userName, string companyCode, string cellphone, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithUserCode(String userCode, bool bCache = false);

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo UpdateWithAdmin(MDataRow dataRow);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String userCode);

        /// <summary>
        /// 允许或不允许创建项目
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        ExeMsgInfo Changes(string userCode);



        /// <summary>
        /// 禁用或企业管理员信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        ExeMsgInfo Status(string userCode);
        /// <summary>
        /// 得到所有角色
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataTable GetRoleListWithCompanyCode(String companyCode);

        /// <summary>
        /// 修改当前登录用户的密码
        /// </summary>
        /// <param name="userCode">当前用户编号</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>返回 ExeMsgInfo</returns>
        ExeMsgInfo UpdateWithAdminPwd(string userCode, string oldPassword, string newPassword);

        /// <summary>
        /// 根据当前登录人编号，获取基本信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        MDataRow GetAdminInfo(string userCode);
    }
}
