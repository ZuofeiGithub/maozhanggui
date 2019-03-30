using Adf.Core.Entity;
using CYQ.Data.Table;
using System;

namespace Decoration.Interface
{
    /// <summary>
    ///  顾健
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

         ExeMsgInfo UpdateByProjectCode1(string projectcode, string projectname, string projectaddress,
            string areaName, string areaCode, string houseNumber);
        MDataTable GetAllListByUserCode(string companycode, string usercode, string projectstatus);
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        MDataRow GetEntityByProjectCode(string projectcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：列表
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="projectname">项目名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string companycode, string projectstatus, string projectname, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：列表
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="usercode">用户编码</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetListByUserCode(string companycode, string usercode, string projectstatus, int pageIndex,
            int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 创建人：侯鑫辉
        /// 时间：2018-10-22
        /// 功能：增加
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectAddress">详细地址</param>
        /// <param name="createUserCode">创建人</param>
        /// <param name="companyCode">施工单位</param>
        /// <returns></returns>
        ExeMsgInfo Add(string projectName, string projectAddress, string createUserCode, string companyCode);


        /// <summary>
        /// [急][奚德荣]外部接口
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectAddress">项目地址</param>
        /// <param name="createUserCode">创建人编号</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="areaName">省市区名称</param>
        /// <param name="areaCode">省市区编号</param>
        /// <param name="houseNumber">门牌号</param>
        /// <returns></returns>
        ExeMsgInfo Add(string projectName, string projectAddress, string createUserCode, string companyCode,
            String areaName, String areaCode, String houseNumber);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo UpdateByProjectCode(MDataRow dataRow);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：修改
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectname">项目名称</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="projectaddress">位置</param>
        /// <returns></returns>
        ExeMsgInfo UpdateByProjectCode(string projectcode,string projectname,string projectstatus,string startdate,string enddate,string projectaddress, string areaName, string areaCode, string houseNumber);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：修改
        /// [急][奚哥]
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectname">项目名称</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="projectaddress">位置</param>
        /// <param name="houseNumber">门牌号</param>
        /// <returns></returns>
        ExeMsgInfo UpdateByProjectCode(string projectcode, string projectname, string projectstatus, string startdate, string enddate, string projectaddress,String houseNumber);

        /// <summary>
        /// 获取所有项目 排除自己
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="companycode">企业编号</param>
        /// <returns></returns>
        MDataTable GetAll(string projectcode, string companycode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：删除
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        ExeMsgInfo DeleteByProjectCode(string projectcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：总数
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        int GetRemindCount(string companyCode);

        /// <summary>
        /// [急][奚哥]导入模板 为当前项目引入阶段以及任务
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="templateCode">模板标题</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExeMsgInfo ImportTemplate(String projectCode,String templateCode,String startDate, String endDate);

    }
}
