using Decoration.Interface;
using System;
using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Adf.Core.Db;
using Adf.Core.Util;
using CYQ.Data;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 管理员信息
    /// 王浩
    /// 2018-10-17
    /// </summary>
    public class AdminCompanyUserlmpl : IAdminCompanyUser
    {
        private const String CurrentTableName = "decoration_companyuser";
        private const String VCurrentTableName = "decoration_companyuser";

        /// <summary>
        /// 初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        MDataRow IAdminCompanyUser.InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        /// 获取企业用户信息列表
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="companyCode">用户名称</param>
        /// <param name="cellphone">手机号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string userName,string companyCode,string cellphone, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";
            string sql = "select  * from decoration_companyuser where companycode='"+ companyCode + "' and isadmin=1";
            
            if (!String.IsNullOrEmpty(userName))
            {
                sWhere += " and username like '%" + userName + "%'";
            }
            if (!String.IsNullOrEmpty(cellphone))
            {
                sWhere += " and cellphone like '%" + cellphone + "%'";
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by firstletter";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithUserCode(string userCode, bool bCache = false)
        {
            String cacheKey = "Companyuser-GetEntity" + userCode;
            String strWhere = " usercode=" + DbService.SetQuotesValue(userCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 修改当前登录用户的密码
        /// </summary>
        /// <param name="userCode">当前用户编号</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>返回 ExeMsgInfo</returns>
        public ExeMsgInfo UpdateWithAdminPwd(string userCode, string oldPassword, string newPassword)
        {
            ExeMsgInfo exeMsgInfo=new ExeMsgInfo();
            if (String.IsNullOrEmpty(userCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "管理员编码不能为空";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(oldPassword))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "原密码不能为空";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(newPassword))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "新密码不能为空";
                return exeMsgInfo;
            }

            string sWhereOldPassword = string.Format("usercode={0} and userpassword={1}",
                DbService.SetQuotesValue(userCode), DbService.SetQuotesValue(Md5Helper.ToMd5(oldPassword)));
            bool flag1 = DbService.Exists(CurrentTableName, sWhereOldPassword);
            if (!flag1)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "原密码不正确";
                return exeMsgInfo;
            }

            string sWhere = "userCode=" + DbService.SetQuotesValue(userCode);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("userpassword", Md5Helper.ToMd5(newPassword));
            exeMsgInfo = DbService.Update(CurrentTableName, sWhere, dic);
            return exeMsgInfo;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String userCode = dataRow.Get("usercode", "");
            if (String.IsNullOrEmpty(userCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业用户编码不能为空";
                return exeMsgInfo;
            }
            String cellphone = dataRow.Get("cellphone", "");
            if (String.IsNullOrEmpty(cellphone))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "手机号不能为空";
                return exeMsgInfo;
            }
            String username = dataRow.Get("username", "");
            if (String.IsNullOrEmpty(username))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "姓名不能为空";
                return exeMsgInfo;
            }
            
            String uid = dataRow.Get("uid", "");
            if (String.IsNullOrEmpty(uid))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "账号不能为空";
                return exeMsgInfo;
            }
            String sWhere = " usercode=" + DbService.SetQuotesValue(userCode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前编码已经存在";
                return exeMsgInfo;
            }

            if (DbService.Exists(CurrentTableName, " uid=" + DbService.SetQuotesValue(dataRow.Get("uid", ""))))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前账号已经存在";
                return exeMsgInfo;
            }
            if (DbService.Exists(CurrentTableName, " cellphone=" + DbService.SetQuotesValue(dataRow.Get("cellphone", ""))))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前手机号已经存在";
                return exeMsgInfo;
            }
            String controlFieldNames = "usercode,companycode,uid,userpassword,username,cellphone,remark,status,firstletter,isadmin,iscreateproject";
            return DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String userCode = dataRow.Get("usercode", "");

            if (String.IsNullOrEmpty(userCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业用户编码不能为空";
                return exeMsgInfo;
            }
            String cellphone = dataRow.Get("cellphone", "");
            if (String.IsNullOrEmpty(cellphone))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "手机号不能为空";
                return exeMsgInfo;
            }
            String username = dataRow.Get("username", "");
            if (String.IsNullOrEmpty(username))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "姓名不能为空";
                return exeMsgInfo;
            }
            String uid = dataRow.Get("uid", "");
            if (String.IsNullOrEmpty(uid))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "账号不能为空";
                return exeMsgInfo;
            }
            if (!String.IsNullOrEmpty(uid))
            {
                String aWhere = " uid=" + DbService.SetQuotesValue(uid) + " and userCode <> " + DbService.SetQuotesValue(userCode);
                if (DbService.Exists(CurrentTableName, aWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前账号已经存在";
                    return exeMsgInfo;
                }
            }

            if (!String.IsNullOrEmpty(cellphone))
            {
                String aWhere = " cellphone=" + DbService.SetQuotesValue(cellphone) + " and userCode <> " + DbService.SetQuotesValue(userCode);
                if (DbService.Exists(CurrentTableName, aWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前手机号已经存在";
                    return exeMsgInfo;
                }
            }
            String sWhere = " usercode=" + DbService.SetQuotesValue(userCode);
            String controlFieldNames = "uid,userpassword,username,cellphone,remark,status,firstletter,isadmin,iscreateproject";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo UpdateWithAdmin(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String userCode = dataRow.Get("usercode", "");

            if (String.IsNullOrEmpty(userCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业用户编码不能为空";
                return exeMsgInfo;
            }
            String cellphone = dataRow.Get("cellphone", "");
            if (String.IsNullOrEmpty(cellphone))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "手机号不能为空";
                return exeMsgInfo;
            }
            String username = dataRow.Get("username", "");
            if (String.IsNullOrEmpty(username))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "姓名不能为空";
                return exeMsgInfo;
            }

            if (!String.IsNullOrEmpty(cellphone))
            {
                String aWhere = " cellphone=" + DbService.SetQuotesValue(cellphone) + " and userCode <> " + DbService.SetQuotesValue(userCode);
                if (DbService.Exists(CurrentTableName, aWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前手机号已经存在";
                    return exeMsgInfo;
                }
            }
            String sWhere = " usercode=" + DbService.SetQuotesValue(userCode);
            String controlFieldNames = "username,cellphone,remark";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string userCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(userCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业用户编码不能为空";
                return exeMsgInfo;
            }
            String sWhere = " usercode=" + DbService.SetQuotesValue(userCode);
            return DbService.Delete(CurrentTableName, sWhere);
        }
        
        /// <summary>
        /// 允许或不允许创建项目
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public ExeMsgInfo Changes(string userCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (String.IsNullOrEmpty(userCode)) { throw new Exception("参数不正确"); }
                string sql = @"
update decoration_companyuser set iscreateproject=ABS(ifnull(iscreateproject,0)-1)
where usercode=@userCode;
";
                sql = sql.Replace("@userCode", DbService.SetQuotesValue(userCode));
                int i = DbService.ExeNonQueryBySql(sql);
                if (i > 0)
                {
                    exeMsgInfo.RetStatus = 200;
                    exeMsgInfo.RetValue = "ok";
                }
                else { throw new Exception("更新失败"); }
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;

        }

        /// <summary>
        /// 禁用或企业管理员信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public ExeMsgInfo Status(string userCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (String.IsNullOrEmpty(userCode)) { throw new Exception("参数不正确"); }
                string sql = @"
update decoration_companyuser set status=ABS(ifnull(status,0)-1)
where usercode=@userCode;
";
                sql = sql.Replace("@userCode", DbService.SetQuotesValue(userCode));
                int i = DbService.ExeNonQueryBySql(sql);
                if (i > 0)
                {
                    exeMsgInfo.RetStatus = 200;
                    exeMsgInfo.RetValue = "ok";
                }
                else { throw new Exception("更新失败"); }
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;

        }
        /// <summary>
        /// 得到所有角色
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public MDataTable GetRoleListWithCompanyCode(string companyCode)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            return DbService.GetTable("decoration_companyrole", 0, sWhere);
        }

        /// <summary>
        /// 根据当前登录人编号，获取基本信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public MDataRow GetAdminInfo(string userCode)
        {
            string sql = @"select * from decoration_companyuser where usercode='"+ userCode + "'";
            return DbService.GetOne(sql, "");
        }
    }
}
