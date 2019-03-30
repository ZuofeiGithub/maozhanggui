using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    /// 企业用户
    /// 侯鑫辉
    /// 2018-10-13
    /// </summary>
    public interface ICompanyUser
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 获取企业用户信息列表
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="userName">用户名称</param>
        /// <param name="cellphone">手机号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithCompanyCode(string companyCode, string userName, string cellphone, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithUserCode(String userCode, bool bCache = false);

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="uid">账号</param>
        /// <returns></returns>
        MDataRow GetEntityWithUid(String uid);

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
        /// 删除
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String userCode);

        /// <summary>
        /// 得到所有角色
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        MDataTable GetRoleListWithCompanyCode(String companyCode);

        /// <summary>
        /// 得到所属角色
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <returns></returns>
        MDataTable GetRoleWithUserCode(string userCode);

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="sUserRoleCodes">所属角色</param>
        /// <returns></returns>
        ExeMsgInfo SetRole(string userCode, string sUserRoleCodes);

        /// <summary>
        /// 得到企业编号
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <returns></returns>
        string GetCompanyCodeWithUserCode(String userCode);

        /// <summary>
        /// 根据项目编号得到服务团队
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        MDataTable GetCompanyUserWithProjectCode(string projectCode);

        /// <summary>
        /// 前端获得企业用户
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        MDataTable GetCompanyUser(string companyCode, int pageIndex, int pageSize);

        /// <summary>
        /// 选择企业用户弹窗列表（status==1启用）
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="userName">用户名称</param>
        /// <param name="cellphone">手机号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable SelCompanyUserListData(string companyCode, string userName, string cellphone, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 根据 uid和userpassword登录的验证
        /// </summary>
        /// <param name="uid">账号</param>
        /// <param name="userPassword">用户密码</param>
        /// <returns></returns>
        ExeMsgInfo CheckLoginWithUid(String uid, String userPassword);

        /// <summary>
        /// 前端新增企业用户
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="userName">姓名</param>
        /// <param name="cellPhone">手机号</param>
        /// <param name="remark">备注</param>
        /// <param name="userRole">角色</param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        ExeMsgInfo AddCompanyUser(string usercode, String companyCode, String userName, String cellPhone, String remark, String userRole,String projectCode);

        /// <summary>
        /// 得到企业用户数量
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        int GetRemindCount(String companyCode);

        #region 佣金规则
        /// <summary>
        /// 佣金规则列表
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="dicValue"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetCommissionRulesData(string parentCode, string companyCode, string dicValue, int pageIndex,
            int pageSize, string orderBy, ref int recordCount, ref int pageCount);
        MDataRow GetEntityWithDicCode(String dicCode);
        ExeMsgInfo AddCommissionRules(MDataRow dtRow);
        ExeMsgInfo UpdateCommissionRules(MDataRow dtRow);
        ExeMsgInfo DeleteCommissionRules(string dicCode);
        #endregion

        #region 郑金国 新增

        /// <summary>
        /// 查找用户名与密码在当前数据库是否存在
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="userPassword">用户密码</param>
        /// <returns></returns>
        ExeMsgInfo CheckLogin(String userCode, String userPassword);



        #endregion

        #region 奚德荣扩展
        /// <summary>
        /// 验证用户登录信息
        /// </summary>
        /// <param name="account">登录账号（usercode或uid）</param>
        /// <param name="password">登录密码</param>
        /// <returns>返回登录成功的用户编号</returns>
        string CheckLoginWithAccount(string account, string password);




        #endregion


        #region 顾健 新增

        /// <summary>
        /// 根据会员编号 更新头像
        /// </summary>
        /// <param name="usercode">会员编号</param>
        /// <param name="headimg">头像地址</param>
        /// <returns></returns>
        ExeMsgInfo UpDateHeadImgByUserCode(string usercode, string headimg);

        #endregion
    }
}
