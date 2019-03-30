using Decoration.Interface;
using System;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Adf.Core.Db;
using CYQ.Data;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 王浩
    /// 2018-10-18
    /// 企业施工任务列表
    /// </summary>
    public class SignInImpl : ISignIn
    {
        private const String CurrentTableName = "decoration_signin";
        private const String VCurrentTableName = "decoration_signin";


        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

       
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="signInCode"></param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        public MDataRow GetEntityWithUserCode(string signInCode, bool bCache = false)
        {
            String cacheKey = "SignIn-GetEntity" + signInCode;
            String strWhere = "signincode=" + DbService.SetQuotesValue(signInCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加签到记录
        /// </summary>
        /// <param name="userCode">用户账号</param>
        /// <param name="gps">签到位置</param>
        /// <param name="signinaddress">签到地址</param>
        /// <param name="signinaddress">projectcode</param>
        /// <returns></returns>
        public ExeMsgInfo SigniInAdd(String userCode,String gps, String signinaddress,string projectcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (string.IsNullOrEmpty(userCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "用户账号不能为空";
                return exeMsgInfo;
            }

            //if (DbService.Exists(CurrentTableName,
            //    " usercode=" + DbService.SetQuotesValue(userCode) + " and projectcode=" +
            //    DbService.SetQuotesValue(projectcode) + " and signintime between '" +
            //    DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' and '" +
            //    DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "'"))
            //{
            //    exeMsgInfo.RetStatus = 400;
            //    exeMsgInfo.RetValue = "今日已签到，请勿重复签到";
            //    return exeMsgInfo;
            //}

            using (MAction action = new MAction(CurrentTableName))
            {
                action.Set("usercode", userCode);
                action.Set("signincode", Guid.NewGuid().ToString()); 
                action.Set("gps", gps);
                action.Set("signinaddress", signinaddress);
                action.Set("signintime", DateTime.Now);
                action.Set("projectcode", projectcode);
                bool flag = action.Insert();
                if (flag)
                {
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "新增成功";
                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "新增失败";
                }
            }
            return exeMsgInfo;

           
        }

        /// <summary>
        /// 根据usercode获取分页数据
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListWithUserCode
        (string userCode, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
           
            String sOrderBy = "";
            string sql = "select * from decoration_signin where usercode='"+ userCode + "'";
            
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by signintime desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 根据usercode projectCode获取分页数据
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListWithUserCode
        (string userCode, string projectCode, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";

            String sOrderBy = "";
            string sql = "select a.*,b.username from decoration_signin a left join decoration_companyuser b on a.usercode=b.usercode where a.usercode='" + userCode + "' and projectcode='" + projectCode + "'";

            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by signintime desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 项目下的所有签到记录
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListWithProjectCode(string projectCode, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            String sWhere = " 1=1";
            string sql = "select a.*,b.username from decoration_signin a left join decoration_companyuser b on a.usercode=b.usercode where projectcode='" + projectCode + "'";

            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by signintime desc";
            }
            sWhere += orderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

    }
}
