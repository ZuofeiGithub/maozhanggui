using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 系统公告
    /// 侯鑫辉
    /// 2018.10.18 
    /// </summary>
    public class SysMsgImpl : ISysMsg
    {
        private const String CurrentTableName = "decoration_sysmsg";
        private const String VCurrentTableName = "decoration_sysmsg";

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithMsgCode(string msgCode, bool bCache = false)
        {
            String sWhere = " msgcode=" + DbService.SetQuotesValue(msgCode);
            String cacheKey = "SysMsg-" + msgCode;

            return DbService.GetOne(VCurrentTableName, sWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            String msgcode = dataRow.Get("msgcode", "");
            if (String.IsNullOrEmpty(msgcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "消息编号不能为空";
                return exeMsgInfo;
            }
            String msgtitle = dataRow.Get("msgtitle", "");
            if (String.IsNullOrEmpty(msgtitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写消息标题";
                return exeMsgInfo;
            }
            String msgcontent = dataRow.Get("msgcontent", "");
            if (String.IsNullOrEmpty(msgcontent))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写消息内容";
                return exeMsgInfo;
            }
            String createusercode = dataRow.Get("createusercode", "");
            if (String.IsNullOrEmpty(createusercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建人不能为空";
                return exeMsgInfo;
            }
            String createdatetime = dataRow.Get("createdatetime", "");
            if (String.IsNullOrEmpty(createdatetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建时间不能为空";
                return exeMsgInfo;
            }
            String companycode = dataRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }

            String controlFieldNames = "msgcode,msgtitle,msgcontent,createusercode,createdatetime,msgstatus,companycode";

            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            String msgcode = dataRow.Get("msgcode", "");
            if (String.IsNullOrEmpty(msgcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "消息编号不能为空";
                return exeMsgInfo;
            }
            String msgtitle = dataRow.Get("msgtitle", "");
            if (String.IsNullOrEmpty(msgtitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写消息标题";
                return exeMsgInfo;
            }
            String msgcontent = dataRow.Get("msgcontent", "");
            if (String.IsNullOrEmpty(msgcontent))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写消息内容";
                return exeMsgInfo;
            }

            String sWhere = " msgcode=" + DbService.SetQuotesValue(msgcode);
            String controlFieldNames = "msgtitle,msgcontent";

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string msgCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(msgCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "消息编号不能为空";
                return exeMsgInfo;
            }

            String sWhere = " msgcode=" + DbService.SetQuotesValue(msgCode);
            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);

            return exeMsgInfo;
        }

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
        public MDataTable GetListWithCompanyCode(string companyCode, string msgTitle, string msgContent, string msgStatus, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            if (!String.IsNullOrEmpty(msgTitle))
            {
                sWhere += " and msgtitle like '%" + msgTitle + "%'";
            }
            if (!String.IsNullOrEmpty(msgContent))
            {
                sWhere += " and msgcontent like '%" + msgContent + "%'";
            }
            if (!String.IsNullOrEmpty(msgStatus))
            {
                sWhere += " and msgstatus='" + msgStatus + "'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by createdatetime desc";
            }
            sWhere += orderBy;

            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 发送系统公告（选择公告发送对象）
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <param name="sUserRoleCodes">角色编号</param>
        /// <returns></returns>
        public ExeMsgInfo Send(string msgCode, string sUserRoleCodes)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (String.IsNullOrEmpty(msgCode)) { throw new Exception("消息编号不能为空"); }
                String sWhere = " msgcode=" + DbService.SetQuotesValue(msgCode);
                using (MAction action = new MAction(CurrentTableName))
                {
                    action.BeginTransation();
                    try
                    {
                        action.Set("msgstatus", 1);
                        if (!action.Update(sWhere)){throw new Exception("更新发送状态失败");}

                        if (String.IsNullOrEmpty(sUserRoleCodes)) { throw new Exception("请选择一个角色"); }

                        action.ResetTable("decoration_sysmsg_role");
                        MDataRow mDataRow = new MDataRow(DBTool.GetColumns("decoration_sysmsg_role"));
                        mDataRow.Set("msgcode", msgCode);

                        for (int i = 0; i < sUserRoleCodes.Split(',').Length; i++)
                        {
                            if (!String.IsNullOrEmpty(sUserRoleCodes.Split(',')[i]))
                            {
                                mDataRow.Set("rolecode", sUserRoleCodes.Split(',')[i]);
                                action.Data.LoadFrom(mDataRow);
                                action.Insert(InsertOp.None);
                            }
                        }

                        exeMsgInfo.RetStatus = 100;
                        exeMsgInfo.RetValue = "发送成功";
                    }
                    catch (Exception ex)
                    {
                        action.RollBack();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 公告发送的对象
        /// </summary>
        /// <param name="msgCode">消息编号</param>
        /// <returns></returns>
        public MDataTable GetRoleListWithSysCode(string msgCode)
        {
            String sWhere = " msgcode=" + DbService.SetQuotesValue(msgCode);
            return DbService.GetTable("decoration_sysmsg_role", 0, sWhere);
        }

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
        public MDataTable GetListWithCompanyCode(string companyCode, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by createdatetime desc";
            }
            sWhere += orderBy;

            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }
    }
}
