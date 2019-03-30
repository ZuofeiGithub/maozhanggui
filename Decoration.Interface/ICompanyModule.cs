using System;
using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    /// <summary>
    /// 模块管理
    /// </summary>
    public interface ICompanyModule
    {
        /// <summary>
        /// 初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="sModuleCode">模块编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntity(String sModuleCode, bool bCache = false);

        /// <summary>
        /// 得到一个菜单所有的一级子级菜单
        /// </summary>
        /// <param name="sParentCode">父级菜单编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataTable GetChildAll(String sParentCode, Boolean bCache = false);

        /// <summary>
        /// 得到当前模块所有的子级模块，包含子级模块的子级模块。
        /// </summary>
        /// <param name="sParentCode">当前模块编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataTable GetSubAll(String sParentCode, Boolean bCache = false);

        /// <summary>
        /// 得到根级菜单
        /// </summary>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataTable GetRootModuleAll(Boolean bCache = false);

        /// <summary>
        /// 增加一个实体
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo Insert(MDataRow dataRow);

        /// <summary>
        /// 根据编码修改一个实体
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        ExeMsgInfo UpdateByModuleCode(MDataRow dataRow);

        /// <summary>
        /// 根据编码删除一个实体
        /// </summary>
        /// <param name="moduleCode">模块编码</param>
        /// <returns></returns>
        ExeMsgInfo DeleteByModuleCode(String moduleCode);






    }

}
