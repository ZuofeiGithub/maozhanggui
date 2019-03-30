using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;
using Microsoft.SqlServer.Server;

namespace Decoration.Interface
{
    public interface ITip
    {
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="tipcode">编号</param>
        /// <returns></returns>
        MDataRow GetEntityByTipcode(string tipcode);

         MDataTable GetAll(string userCode, string projectcode);
        /// <summary>
        /// 获取可提醒角色
        /// </summary>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        MDataTable GetTipRole(string rolecode);
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectcatecode">阶段编号</param>
        /// <param name="rolecode">角色编号</param>
        /// <param name="tiptitle">标题</param>
        /// <param name="issend">是否发送</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string projectcode, string projectcatecode, string rolecode, string tiptitle, string issend, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 获取提醒数据的重载方法
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectcatecode">阶段编号</param>
        /// <returns></returns>
        MDataTable GetList(string projectcode, string projectcatecode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-11-8
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="rolecodes">提醒角色集</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow,List<string>rolecodes);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo UpdateByTipcode(MDataRow dataRow);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="tipcode">编号</param>
        /// <returns></returns>
        ExeMsgInfo DeleteByTipcode(string tipcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：发送提醒
        /// </summary>
        /// <param name="tipcode">编号</param>
        /// <returns></returns>
        ExeMsgInfo SendByTipcode(string tipcode);

        /// <summary>
        /// 创建人：侯鑫辉
        /// 时间：2018-10-22
        /// 功能：前端得到列表
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetList(string userCode,string projectcode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 创建人：侯鑫辉
        /// 时间：2018-10-22
        /// 功能：新增
        /// </summary>
        /// <param name="roleCode"></param>
        /// <param name="tipTitle"></param>
        /// <param name="tipContent"></param>
        /// <param name="createUserCode"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        ExeMsgInfo Add(string roleCode, string tipTitle, string tipContent, string createUserCode, string projectCode);


        /// <summary>
        /// 获取前台可发送的角色
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
         MDataTable GetAllTipRole(string usercode);
    }
}