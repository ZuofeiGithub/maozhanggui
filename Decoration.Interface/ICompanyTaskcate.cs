using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    /// 企业任务分类（阶段）接口
    /// 王浩
    /// 2018-10-18
    /// </summary>
    public interface ICompanyTaskcate
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="taskcateCode">编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithTaskcateCode(String taskcateCode, bool bCache = false);

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
        /// <param name="taskcateCode">编码</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String taskcateCode);



        /// <summary>
        /// 获取任务分类数据（弹窗使用）
        /// </summary>
        /// <returns></returns>
        /// <param name="cateName">分类名称</param>
        /// <param name="companycode">企业编号</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetAll(string cateName,string companycode, string projectcode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 列表信息
        /// </summary>
        /// <param name="companyCode">企业编码</param>
        /// <param name="cateName">分类名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string companyCode,string cateName,int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount,string templatecode="");

        /// <summary>
        /// 获取所有的任务分类
        /// </summary>
        /// <returns></returns>
         MDataTable GetEntityByCateCode(string companycode);


        /// <summary>
        /// 获取所有的任务分类
        /// </summary>
        /// <returns></returns>
         MDataTable GetEntityByCateCode(string companycode, string templatecode);


    }
}
