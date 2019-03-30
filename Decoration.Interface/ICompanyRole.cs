using Adf.Core.Entity;
using CYQ.Data.Table;
using System;
using System.Collections.Generic;

namespace Decoration.Interface
{
    /// <summary>
    /// 企业人员角色接口
    /// 吴尧
    /// 2018-10-18
    /// </summary>
    public interface ICompanyRole
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

         MDataTable GetRoleList(string usercode);
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="roleCode">编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithRoleCode(String roleCode, bool bCache = false);

        /// <summary>
        /// 【急】【奚哥】PC 角色与模块关联， 与提醒角色关联
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);
        ExeMsgInfo Add(MDataRow dataRow,string frontmodule1,string frontmodule2,string tiproles);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);
        /// <summary>
        /// 修改角色及相关信息
        /// 奚德荣 2018.11.8
        /// </summary>
        /// <param name="dataRow">角色实体</param>
        /// <param name="frontmodule1">授权模块，多个用逗号分隔</param>
        /// <param name="frontmodule2">授权状态，多个用逗号分隔</param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns>返回ExeMsgInfo</returns>
        ExeMsgInfo Update(MDataRow dataRow, string frontmodule1, string frontmodule2, string tiproles);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="roleCode">编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String roleCode);

        /// <summary>
        /// 列表信息
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="companyCode">角色名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string roleName,string companyCode, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 当前企业下的所有角色
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        MDataTable GetCompanyRole(string companyCode);

        /// <summary>
        /// 获取前台模块信息
        /// </summary>
        /// <returns></returns>
        MDataTable GetFrontModuleCode();

        /// <summary>
        /// 根据角色编码获取前台模块
        /// </summary>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        MDataRow FrontModuleCode(string rolecode);

    }
}
