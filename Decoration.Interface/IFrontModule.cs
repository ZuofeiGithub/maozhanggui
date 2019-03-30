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
    /// 【王孝为】
    /// 
    /// </summary>
    public interface IFrontModule
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="usercode">编号</param>
        /// <returns></returns>
         MDataTable GetAllStatusForUserCode(string usercode);
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="frontModuleCode">前端模块编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithFrontModuleCode(String frontModuleCode, bool bCache = false);

        /// <summary>
        /// 功能：新增
        /// 创建人：王孝为
        /// 时间2018-11-07
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);
        /// <summary>
        /// 功能：修改
        /// 创建人：王孝为
        /// 时间2018-11-07
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 功能：删除
        /// 创建人：王孝为
        /// 时间2018-11-07
        /// </summary>
        /// <param name="frontmodulecode">模块编号</param>
        /// <returns></returns>
        ExeMsgInfo Delete(string frontmodulecode);

        /// <summary>
        /// 功能：列表
        /// 创建人：王孝为
        /// 时间2018-11-07
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);


        #region 奚德荣 2018.11.8
        /// <summary>
        /// 获取项目详细页中的所有模块
        /// 奚德荣 2018.11.8
        /// </summary>
        /// <returns></returns>
        MDataTable GetFrontModuleForProject();

        /// <summary>
        /// 获取项目列表页中的所有状态
        /// 奚德荣 2018.11.8
        /// </summary>
        /// <returns></returns>
        MDataTable GetFrontModuleForProjectStatus();

        /// <summary>
        /// 获取项目详细页中的所有模块
        /// 奚德荣 2018.11.8
        /// </summary>
        /// <returns></returns>
        MDataTable GetFrontModuleForProject(string rolecode);

        /// <summary>
        /// 获取项目列表页中的所有状态
        /// 奚德荣 2018.11.8
        /// </summary>
        /// <returns></returns>
        MDataTable GetFrontModuleForProjectStatus(string rolecode);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="usercode">编号</param>
        /// <returns></returns>
         MDataTable GetAllModuleForUserCode(string usercode);

        #endregion

    }
}
