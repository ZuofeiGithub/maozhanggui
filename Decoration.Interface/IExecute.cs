using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface IExecute
    {

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据任务编号获取执行情况
        /// </summary>
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        MDataTable GetAll(string taskcode, string usercode, string taskstatus);


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="taskcode">任务编号</param>
        /// <param name="usercode">会员编号</param>
        /// <param name="executecontent">内容</param>
        /// <param name="executeimage">图片</param>
        /// <returns></returns>
        ExeMsgInfo Add(string taskcode, string usercode, string executecontent, string executeimage);

        /// <summary>
        /// 根据项目编码查询任务执行记录
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        MDataTable GetExecuteWithProjectCode(string projectCode);

        /// <summary>
        /// 查询任务执行记录
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        MDataRow GetExecute(string projectCode);

        /// <summary>
        /// 修改任务记录
        /// 侯鑫辉 2018.11.13
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        ExeMsgInfo UpdateByExecuteCode(MDataRow dtRow);

        /// <summary>
        /// 删除任务记录
        /// 侯鑫辉 2018.11.13
        /// </summary>
        /// <returns></returns>
        ExeMsgInfo DeleteByExecuteCode(string executeCode);
    }
}