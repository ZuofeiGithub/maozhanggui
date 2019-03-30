using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 活动推送
    /// 侯鑫辉
    /// 2018.10.18 
    /// </summary>
    public class ActivityImpl : IActivity
    {
        private const String CurrentTableName = "decoration_activity";
        private const String VCurrentTableName = "decoration_activity";

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
        /// <param name="activityCode">活动编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithActivityCode(string activityCode, bool bCache = false)
        {
            String sWhere = " activitycode=" + DbService.SetQuotesValue(activityCode);
            String cacheKey = "Activity-" + activityCode;

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

            String activitycode = dataRow.Get("activitycode", "");
            if (String.IsNullOrEmpty(activitycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "活动编号不能为空";
                return exeMsgInfo;
            }
            String activitytitle = dataRow.Get("activitytitle", "");
            if (String.IsNullOrEmpty(activitytitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写活动标题";
                return exeMsgInfo;
            }
            String activitysummary = dataRow.Get("activitysummary", "");
            if (String.IsNullOrEmpty(activitysummary))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写活动简介";
                return exeMsgInfo;
            }
            String activitycontent = dataRow.Get("activitycontent", "");
            if (String.IsNullOrEmpty(activitycontent))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写活动内容";
                return exeMsgInfo;
            }
            String activitythumbnail = dataRow.Get("activitythumbnail", "");
            if (String.IsNullOrEmpty(activitythumbnail))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请上传活动封面";
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

            String controlFieldNames = "activitycode,activitytitle,activitysummary,activitycontent,activitythumbnail,createusercode,createdatetime,ishot,activitystatus,companycode";

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

            String activitycode = dataRow.Get("activitycode", "");
            if (String.IsNullOrEmpty(activitycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "活动编号不能为空";
                return exeMsgInfo;
            }
            String activitytitle = dataRow.Get("activitytitle", "");
            if (String.IsNullOrEmpty(activitytitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写活动标题";
                return exeMsgInfo;
            }
            String activitysummary = dataRow.Get("activitysummary", "");
            if (String.IsNullOrEmpty(activitysummary))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写活动简介";
                return exeMsgInfo;
            }
            String activitycontent = dataRow.Get("activitycontent", "");
            if (String.IsNullOrEmpty(activitycontent))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写活动内容";
                return exeMsgInfo;
            }
            String activitythumbnail = dataRow.Get("activitythumbnail", "");
            if (String.IsNullOrEmpty(activitythumbnail))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请上传活动封面";
                return exeMsgInfo;
            }

            String sWhere = " activitycode=" + DbService.SetQuotesValue(activitycode);
            String controlFieldNames = "activitytitle,activitysummary,activitycontent,activitythumbnail,ishot,activitystatus";

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="activityCode">活动编号</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string activityCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(activityCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "活动编号不能为空";
                return exeMsgInfo;
            }

            String sWhere = " activitycode=" + DbService.SetQuotesValue(activityCode);
            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);

            return exeMsgInfo;
        }

        /// <summary>
        /// 得到所有的活动信息
        /// </summary>
        /// <param name="activityTitle">活动标题</param>
        /// <param name="activitySummary">活动简介</param>
        /// <param name="activityContent">活动内容</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListWithCompanyCode(string activityTitle, string activitySummary, string activityContent, string companyCode, int pageIndex,
            int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            if (!String.IsNullOrEmpty(activityTitle))
            {
                sWhere += " and activitytitle like '%" + activityTitle + "%'";
            }
            if (!String.IsNullOrEmpty(activitySummary))
            {
                sWhere += " and activitysummary like '%" + activitySummary + "%'";
            }
            if (!String.IsNullOrEmpty(activityContent))
            {
                sWhere += " and activitycontent like '%" + activityContent + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by createdatetime desc";
            }
            sWhere += orderBy;

            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 前端得到活动信息
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListWithCompanyCode(string companyCode, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by createdatetime desc";
            }
            sWhere += orderBy;
            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 复制新增一个实体
        /// </summary>
        /// <param name="activityCode">活动编号</param>
        /// <returns></returns>
        public ExeMsgInfo CopyAdd(string activityCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(activityCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "活动编号不能为空";
                return exeMsgInfo;
            }
            String sWhere = " activitycode=" + DbService.SetQuotesValue(activityCode);
            MDataRow activityRow = DbService.GetOne(CurrentTableName, sWhere);
            if (activityRow == null)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "没有找到数据";
                return exeMsgInfo;
            }
            activityRow.Set("activitycode", Guid.NewGuid().ToString("N"));
            activityRow.Set("createdatetime", DateTime.Now);

            String controlFieldNames = "activitycode,activitytitle,activitysummary,activitycontent,activitythumbnail,createusercode,createdatetime,ishot,activitystatus,companycode";

            exeMsgInfo = DbService.Insert(CurrentTableName, activityRow, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 得到活动数量
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public int GetRemindCount(string companyCode)
        {
            String sql = "select count(*) from decoration_activity";
            MDataTable dataTable = DbService.GetTable(sql, 0, "");
            if (dataTable.Rows.Count > 0)
            {
                String count = dataTable.Rows[0][0].ToString();
                if (String.IsNullOrEmpty(count))
                {
                    return 0;
                }
                return Convert.ToInt32(count);
            }
            return 0;
        }
    }
}
