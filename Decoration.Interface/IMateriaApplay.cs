using Adf.Core.Entity;
using CYQ.Data.Table;

namespace Decoration.Interface
{
    public interface IMateriaApplay
    {
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取所有
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        MDataTable GetAll(string projectcode);

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();


        /// <summary>
        /// 申请退货
        /// </summary>
        /// <param name="applycode"></param>
        /// <returns></returns>
        ExeMsgInfo UpdateIsReturn(string applycode);
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加 
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="materialtype">材料类型</param>
        /// <param name="receivetime">期望到货时间</param>
        /// <param name="receiver">接受人</param>
        /// <param name="receiverphone">接受人电话</param>
        /// <param name="applayremark">备注</param>
        /// <returns></returns>
        ExeMsgInfo Add(string projectcode,string materialtype,string receivetime,string receiver,string receiverphone,string applayremark);
    }
}