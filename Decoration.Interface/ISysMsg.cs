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
    /// 系统公告
    /// 侯鑫辉
    /// 2018.10.18 
    /// </summary>
    public interface ISysMsg
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        MDataRow InitDataRow();

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        MDataRow GetEntityWithMsgCode(String msgCode, bool bCache = false);

        /// <summary>
        /// 增加一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Add(MDataRow dataRow);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        ExeMsgInfo Update(MDataRow dataRow);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <returns></returns>
        ExeMsgInfo Delete(String msgCode);

        /// <summary>
        /// 得到所有的系统公告
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="msgTitle">消息标题</param>
        /// <param name="msgContent">消息内容</param>
        /// <param name="msgStatus">发送状态</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithCompanyCode(string companyCode, string msgTitle, string msgContent, string msgStatus, int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);

        /// <summary>
        /// 发送系统公告（选择公告发送对象）
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <param name="sUserRoleCodes">角色编号</param>
        /// <returns></returns>
        ExeMsgInfo Send(String msgCode, String sUserRoleCodes);

        /// <summary>
        /// 公告发送的对象
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <returns></returns>
        MDataTable GetRoleListWithSysCode(String msgCode);

        /// <summary>
        /// 前端得到系统公告
        /// 2018.10.22
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        MDataTable GetListWithCompanyCode(string companyCode,int pageIndex, int pageSize, String orderBy, ref int recordCount, ref int pageCount);
    }
}
