using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    /// <summary>
    /// 设计档案
    /// </summary>
    public interface IDoc
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
        /// <param name="doccode">编号</param>
        /// <returns></returns>
        MDataRow GetEntityByDocCode(string doccode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="doctype">类型</param>
        /// <returns></returns>
         MDataTable GetTable(string projectcode, string doctype);

        /// <summary>
        /// 获取实体重载方法(不区分类型)
        /// </summary>
        /// <param name="projectcode"></param>
        /// <returns></returns>
        MDataTable GetTable(string projectcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="doctype">类型</param>
        /// <returns></returns>
        MDataRow GetEntityByDocCode(string projectcode,string doctype);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string projectcode, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount);

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
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="doctype">类型</param>
        /// <param name="projectcode">项目编号</param>
        /// <param name="doccontent">内容</param>
        /// <param name="docimages">图片</param>
        /// <param name="createusercode">创建人</param>
        /// <returns></returns>
        ExeMsgInfo Add(string doctype,string projectcode,string doccontent,string docimages,string createusercode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo UpdateByDocCode(MDataRow dataRow);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// [急]
        /// </summary>
        /// <param name="doccode">编码</param>
        /// <param name="doccontent">内容</param>
        /// <param name="docimages">图片</param>
        /// <returns></returns>
        ExeMsgInfo UpdateByDocCode(string doccode,string doccontent,string docimages);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="doccode">编号</param>
        /// <returns></returns>
        ExeMsgInfo DeleteByDocCode(string doccode);

        
    }
}