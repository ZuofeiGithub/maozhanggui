using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    /// 签到记录
    /// 王浩
    /// 2018-10-20
    /// </summary>
    public interface ISignIn
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();


        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="signInCode">签到编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithUserCode(String signInCode, bool bCache = false);


        /// <summary>
        /// 增加签到记录
        /// </summary>
        /// <param name="userCode">用户账号</param>
        /// <param name="gps">签到位置</param>
        /// <param name="signinaddress">签单地址</param>
        /// <returns></returns>
        ExeMsgInfo SigniInAdd(String userCode, String gps, String signinaddress, string projectcode);

        /// <summary>
        /// 根据usercode获取分页数据
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithUserCode
            (string userCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 根据usercode projectCode获取分页数据
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithUserCode
            (string userCode, string projectCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 项目下的所有签到记录
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithProjectCode(string projectCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);
    }
}
