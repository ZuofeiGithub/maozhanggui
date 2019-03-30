using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface ITaskMessage
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
        /// <param name="messsagecode">编号</param>
        /// <param name="companycode">企业编号</param>
        /// <returns></returns>
        MDataRow GetEntityBymesssagecode(string messsagecode, string companycode);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="taskcode">编号</param>
        /// <returns></returns>
        MDataTable GetAll(string companycode,string taskcode);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="membername">企业编号</param>
        /// <param name="address">企业编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        MDataTable GetList(string companycode, string membername, string address, int pageIndex, int pageSize,
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
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="messsagecode">编号</param>
        /// <param name="companycode">企业编号</param>
        /// <returns></returns>
        ExeMsgInfo DeleteBymatercode(string messsagecode, string companycode);
    }
}