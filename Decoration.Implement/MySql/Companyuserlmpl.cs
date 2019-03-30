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
    /// 企业用户信息
    /// 侯鑫辉
    /// 2018-10-13
    /// </summary>
    public class CompanyUserlmpl : ICompanyUser
    {
        private const String CurrentTableName = "decoration_companyuser";
        private const String VCurrentTableName = "decoration_vcompanyuser";

        /// <summary>
        /// 初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        /// 获取企业用户信息列表
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="userName">用户名称</param>
        /// <param name="cellphone">手机号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetListWithCompanyCode(string companyCode, string userName, string cellphone, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode) + " and isadmin=0"; ;
            if (!String.IsNullOrEmpty(userName))
            {
                sWhere += " and username like '%" + userName + "%'";
            }
            if (!String.IsNullOrEmpty(cellphone))
            {
                sWhere += " and cellphone like '%" + cellphone + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by firstletter";
            }
            sWhere += orderBy;

            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithUserCode(string userCode, bool bCache = false)
        {
            String cacheKey = "CompanyUser-GetEntity" + userCode;
            String strWhere = " usercode=" + DbService.SetQuotesValue(userCode);

            return DbService.GetOne(CurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="uid">账号</param>
        /// <returns></returns>
        public MDataRow GetEntityWithUid(string uid)
        {
            String sWhere = " uid=" + DbService.SetQuotesValue(uid);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String usercode = dataRow.Get("usercode", "");
            if (String.IsNullOrEmpty(usercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业用户编码不能为空";
                return exeMsgInfo;
            }
            String companycode = dataRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }
            String uid = dataRow.Get("uid", "");
            if (String.IsNullOrEmpty(uid))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写账号";
                return exeMsgInfo;
            }
            if (DbService.Exists(CurrentTableName, " uid=" + DbService.SetQuotesValue(uid)))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前账号已经存在";
                return exeMsgInfo;
            }
            String userpassword = dataRow.Get("userpassword", "");
            if (String.IsNullOrEmpty(userpassword))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写密码";
                return exeMsgInfo;
            }
            dataRow.Set("userpassword", Md5Helper.ToMd5(userpassword));
            String status = dataRow.Get("status", "");
            if (String.IsNullOrEmpty(status))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择启用状态";
                return exeMsgInfo;
            }

            dataRow.Set("isadmin", 0);

            string sql = @"
select (ifnull(a1.companycount,0)-ifnull(a2.t,0)) as mind from decoration_Company a1
left join 

(select count(*) as t,companycode from decoration_Companyuser  group by companycode)

 a2 on a1.companycode=a2.companycode where a1.companycode='" + companycode + "'";

            MDataRow company = DbService.GetOne(sql,
                "");

            int sumcount = company.Get("mind", 0);

            if (sumcount <= 0)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "已超出最大人员数量，请联系总后台管理员！";
                return exeMsgInfo;
            }

            String controlFieldNames = "usercode,companycode,uid,userpassword,username,cellphone,remark,status,firstletter,isadmin,iscreateproject";
            return DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String usercode = dataRow.Get("usercode", "");
            if (String.IsNullOrEmpty(usercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业用户编码不能为空";
                return exeMsgInfo;
            }
            String uid = dataRow.Get("uid", "");
            if (String.IsNullOrEmpty(uid))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写账号";
                return exeMsgInfo;
            }
            if (DbService.Exists(CurrentTableName, " uid=" + DbService.SetQuotesValue(uid) + " and usercode<>" + DbService.SetQuotesValue(usercode)))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前账号已经存在";
                return exeMsgInfo;
            }
            String userpassword = dataRow.Get("userpassword", "");
            if (String.IsNullOrEmpty(userpassword))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写密码";
                return exeMsgInfo;
            }

            string passwordWhere = " usercode = " + DbService.SetQuotesValue(usercode) + " and userpassword = " + DbService.SetQuotesValue(userpassword);
            if (!DbService.Exists(CurrentTableName, passwordWhere)) //修改页面绑定的是加密的密码，如果改动了，重新加密保存
            {
                userpassword = Md5Helper.ToMd5(userpassword);
            }
            dataRow.Set("userpassword", userpassword);

            String status = dataRow.Get("status", "");
            if (String.IsNullOrEmpty(status))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择启用状态";
                return exeMsgInfo;
            }

            String sWhere = " usercode=" + DbService.SetQuotesValue(usercode);
            String controlFieldNames = "uid,userpassword,username,cellphone,remark,status,firstletter,iscreateproject";
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
        /// 得到所属角色
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <returns></returns>
        public MDataTable GetRoleWithUserCode(string userCode)
        {
            String sWhere = " usercode=" + DbService.SetQuotesValue(userCode);
            return DbService.GetTable("decoration_companyuser_role", 0, sWhere);
        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="sUserRoleCodes">所属角色</param>
        /// <returns></returns>
        public ExeMsgInfo SetRole(string userCode, string sUserRoleCodes)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (String.IsNullOrEmpty(userCode)) { throw new Exception("用户编号不能为空"); }
                String sWhere = " usercode=" + DbService.SetQuotesValue(userCode);
                using (MAction action = new MAction("decoration_companyuser_role"))
                {
                    bool flag = true;
                    if (action.Fill(sWhere))
                    {
                        flag = action.Delete(sWhere);
                    }
                    if (!flag) { throw new Exception("删除原角色数据失败"); }
                    if (String.IsNullOrEmpty(sUserRoleCodes)) { throw new Exception("请选择一个角色"); }
                    MDataRow mDataRow = new MDataRow(DBTool.GetColumns("decoration_companyuser_role"));
                    mDataRow.Set("usercode", userCode);
                    for (int i = 0; i < sUserRoleCodes.Split(',').Length; i++)
                    {
                        if (!String.IsNullOrEmpty(sUserRoleCodes.Split(',')[i]))
                        {
                            mDataRow.Set("rolecode", sUserRoleCodes.Split(',')[i]);
                            action.Data.LoadFrom(mDataRow);
                            action.Insert(InsertOp.None);
                        }
                    }
                }
                exeMsgInfo.RetStatus = 100;
                exeMsgInfo.RetValue = "设置成功";
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 得到企业编号
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <returns></returns>
        public string GetCompanyCodeWithUserCode(string userCode)
        {
            String sql = "select companycode from decoration_companyuser where usercode=" + DbService.SetQuotesValue(userCode);
            return DbService.ExcuteScalarBySql(sql).ToString();
        }

        /// <summary>
        /// 根据项目编号得到服务团队
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        public MDataTable GetCompanyUserWithProjectCode(string projectCode)
        {
            String sWhere = " projectcode=" + DbService.SetQuotesValue(projectCode);
            return DbService.GetTable("decoration_projectteam", 0, sWhere);
        }

        /// <summary>
        /// 前端获得企业用户
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public MDataTable GetCompanyUser(string companyCode, int pageIndex, int pageSize)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode); ;
            sWhere += " order by firstletter";

            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere);
        }

        /// <summary>
        /// 选择企业用户弹窗列表（status==1启用）
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="userName">用户名称</param>
        /// <param name="cellphone">手机号</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable SelCompanyUserListData(string companyCode, string userName, string cellphone, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode) + " and status=1"; ;
            if (!String.IsNullOrEmpty(userName))
            {
                sWhere += " and username like '%" + userName + "%'";
            }
            if (!String.IsNullOrEmpty(cellphone))
            {
                sWhere += " and cellphone like '%" + cellphone + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by firstletter";
            }
            sWhere += orderBy;

            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 根据 uid和userpassword登录的验证
        /// </summary>
        /// <param name="uid">账号</param>
        /// <param name="userPassword">用户密码</param>
        /// <returns></returns>
        public ExeMsgInfo CheckLoginWithUid(string uid, string userPassword)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo(400, "空操作");
            if (String.IsNullOrEmpty(uid))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入账号";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(userPassword))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入密码";
                return exeMsgInfo;
            }
            MDataRow drUser = GetEntityWithUid(uid);
            if (drUser == null)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前用户不存在";
                return exeMsgInfo;
            }

            userPassword = userPassword.ToLower();

            String md5Password = Md5Helper.ToMd5(userPassword);
            if (md5Password == drUser.Get("UserPassword", ""))
            {
                exeMsgInfo.RetStatus = 100;
                exeMsgInfo.RetValue = "用户登录成功";
            }
            else
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "用户名或者密码不正确";
            }

            return exeMsgInfo;

        }


        /// <summary>
        /// 前端新增企业用户
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="userName">姓名</param>
        /// <param name="cellPhone">手机号</param>
        /// <param name="remark">备注</param>
        /// <param name="userRole">角色</param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public ExeMsgInfo AddCompanyUser(string usercode,string companyCode, string userName, string cellPhone, string remark, string userRole, string projectCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                String userCode = "";
                if (!string.IsNullOrEmpty(usercode))
                {
                    userCode = usercode;
                }
                else
                {
                    userCode = Guid.NewGuid().ToString();
                }
                
                String uid = cellPhone;

                String sWhere = " usercode=" + DbService.SetQuotesValue(usercode) + " and username=" + DbService.SetQuotesValue(userName);
                bool flag = DbService.Exists(CurrentTableName, sWhere);//flag=true 只需要添加服务团队表

                //if (flag)
                //{
                //    userCode = DbService.ExcuteScalarBySql("select usercode from decoration_companyuser where uid=" + DbService.SetQuotesValue(uid)).ToString();
                //}


              

                using (MAction mAction = new MAction(CurrentTableName))
                {
                    mAction.BeginTransation();
                    try
                    {
                        if (!flag)
                        {



                            if (String.IsNullOrEmpty(companyCode))
                            {
                                throw new Exception("企业编码不能为空");
                            }
                            if (String.IsNullOrEmpty(userName))
                            {
                                throw new Exception("请输入姓名");
                            }
                            if (String.IsNullOrEmpty(uid))
                            {
                                throw new Exception("请输入手机号码");
                            }
                            if (DbService.Exists(CurrentTableName, " uid=" + DbService.SetQuotesValue(uid)))
                            {
                                throw new Exception("当前手机号码已经存在");
                            }
                            if (String.IsNullOrEmpty(userRole))
                            {
                                throw new Exception("请选择角色");
                            }

                            MDataRow dtRow = InitDataRow();
                            dtRow.Set("usercode", userCode);
                            dtRow.Set("companycode", companyCode);
                            dtRow.Set("uid", uid);
                            dtRow.Set("userpassword", Md5Helper.ToMd5("1"));
                            dtRow.Set("username", userName);
                            dtRow.Set("cellphone", cellPhone);
                            dtRow.Set("remark", remark);
                            dtRow.Set("status", 1);
                            dtRow.Set("isadmin", 0);
                            dtRow.Set("iscreateproject", 0);
                            mAction.Data.LoadFrom(dtRow);






                            if (!mAction.Insert())
                            {
                                throw new Exception("新增企业用户失败");
                            }


                            mAction.ResetTable("decoration_companyuser_role");
                            MDataRow userRoleRow = new MDataRow(DBTool.GetColumns("decoration_companyuser_role"));
                            userRoleRow.Set("rolecode", userRole);
                            userRoleRow.Set("usercode", userCode);
                            mAction.Data.LoadFrom(userRoleRow);
                            if (!mAction.Insert())
                            {
                                throw new Exception("设置企业用户角色失败");
                            }
                       
                        }
                   
                        mAction.ResetTable("decoration_projectteam");

                        if (mAction.Exists("usercode=" + DbService.SetQuotesValue(usercode)+" and projectcode="+DbService.SetQuotesValue(projectCode)))
                        {
                            mAction.ResetTable("decoration_companyuser_role");
                            if (!mAction.Exists("usercode=" + DbService.SetQuotesValue(usercode) + " and rolecode=" +
                                               DbService.SetQuotesValue(userRole)))
                            {
                                mAction.Set("rolecode", userRole);
                                mAction.Set("usercode", usercode);
                                if (!mAction.Insert())
                                {
                                    throw new Exception("更新企业用户角色失败");
                                }
                            }
                        }
                        else
                        {
                            MDataRow projectTeamRow = new MDataRow(DBTool.GetColumns("decoration_projectteam"));
                            projectTeamRow.Set("teamcode", Guid.NewGuid().ToString());
                            projectTeamRow.Set("projectcode", projectCode);
                            projectTeamRow.Set("usercode", userCode);
                            mAction.Data.LoadFrom(projectTeamRow);
                            if (!mAction.Insert()) { throw new Exception("插入服务团队表失败"); }
                        }
                        exeMsgInfo.RetStatus = 100;
                        exeMsgInfo.RetValue = "保存成功";
                    }
                    catch (Exception ex)
                    {
                        mAction.RollBack();
                        exeMsgInfo.RetValue = ex.Message;
                        exeMsgInfo.RetStatus = 400;
                    }
                }
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetValue = ex.Message;
                exeMsgInfo.RetStatus = 400;
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 得到企业用户数量
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public int GetRemindCount(string companyCode)
        {
            String sql = "select count(*) from decoration_companyuser";
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

        #region 佣金规则
        /// <summary>
        /// 佣金规则列表
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="companyCode"></param>
        /// <param name="dicValue"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetCommissionRulesData(string parentCode, string companyCode, string dicValue, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode) + " and parentcode=" + DbService.SetQuotesValue(parentCode);
            if (!String.IsNullOrEmpty(dicValue))
            {
                sWhere += " and dicvalue like '%" + dicValue + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by dicorder desc";
            }
            sWhere += orderBy;

            return DbService.GetPageTable("decoration_commissionrules", pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }
        public MDataRow GetEntityWithDicCode(string dicCode)
        {
            String sWhere = " diccode=" + DbService.SetQuotesValue(dicCode);
            return DbService.GetOne("decoration_commissionrules", sWhere);
        }

        public ExeMsgInfo AddCommissionRules(MDataRow dtRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String diccode = dtRow.Get("diccode", "");
            if (String.IsNullOrEmpty(diccode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编号不能为空";
                return exeMsgInfo;
            }
            String companycode = dtRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }
            String dicvalue = dtRow.Get("dicvalue", "");
            if (String.IsNullOrEmpty(dicvalue))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写佣金规则";
                return exeMsgInfo;
            }
            dtRow.Set("parentcode", "commissionrules");
            dtRow.Set("dickey", diccode);

            String controlFieldNames = "dicorder,parentcode,dickey,dicvalue,diccode,companycode";
            return DbService.Insert("decoration_commissionrules", dtRow, controlFieldNames, true);
        }

        public ExeMsgInfo UpdateCommissionRules(MDataRow dtRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String diccode = dtRow.Get("diccode", "");
            if (String.IsNullOrEmpty(diccode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编号不能为空";
                return exeMsgInfo;
            }
            String dicvalue = dtRow.Get("dicvalue", "");
            if (String.IsNullOrEmpty(dicvalue))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写佣金规则";
                return exeMsgInfo;
            }

            String sWhere = " diccode=" + DbService.SetQuotesValue(diccode); ;
            String controlFieldNames = "dicorder,dicvalue";
            return DbService.Update("decoration_commissionrules", dtRow, sWhere, controlFieldNames, true);
        }

        public ExeMsgInfo DeleteCommissionRules(string dicCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(dicCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编号不能为空";
                return exeMsgInfo;
            }
            String sWhere = " diccode=" + DbService.SetQuotesValue(dicCode); ;
            return DbService.Delete("decoration_commissionrules", sWhere);
        }
        #endregion

        /// <summary>
        /// 查找用户名与密码在当前数据库是否存在
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="userPassword">用户密码</param>
        /// <returns></returns>
        public ExeMsgInfo CheckLogin(string userCode, string userPassword)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo(400, "空操作");

            if (String.IsNullOrEmpty(userCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "用户名不能为空";
                return exeMsgInfo;
            }

            if (String.IsNullOrEmpty(userPassword))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "密码不能为空";
                return exeMsgInfo;
            }

            MDataRow drUser = GetEntityWithUserCode(userCode);
            if (drUser == null)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前用户不存在";
                return exeMsgInfo;
            }

            userPassword = userPassword.ToLower();

            String md5Password = Md5Helper.ToMd5(userPassword);
            if (md5Password == drUser.Get("UserPassword", ""))
            {
                exeMsgInfo.RetStatus = 100;
                exeMsgInfo.RetValue = "用户登录成功";
            }
            else
            {
                exeMsgInfo.RetStatus = 401;
                exeMsgInfo.RetValue = "用户名或者密码不正确";
            }



            return exeMsgInfo;
        }


        #region 奚德荣扩展2018.10.20
        /// <summary>
        /// 验证用户登录信息
        /// </summary>
        /// <param name="account">登录账号（usercode或uid）</param>
        /// <param name="password">登录密码</param>
        /// <returns>返回登录成功的用户编号</returns>
        public string CheckLoginWithAccount(string account, string password)
        {
            string loginUserCode = "";
            try
            {
                if (String.IsNullOrWhiteSpace(account)) { throw new Exception("请输入登录账号"); }
                if (String.IsNullOrWhiteSpace(password)) { throw new Exception("请输入登录密码"); }
                string md5pwd = Md5Helper.ToMd5(password);
                using (MAction action = new MAction(CurrentTableName))
                {
                    //支持系统usercode或自定义的uid账号登录系统，可扩展其他手机号或邮箱登录方式
                    string accountWhere = string.Format("(usercode={0} or uid={0})", DbService.SetQuotesValue(account));
                    string sWhere = string.Format("{0} and userpassword={1}", accountWhere, DbService.SetQuotesValue(md5pwd));
                    bool accountFlag = action.Exists(accountWhere);
                    if (!accountFlag) { throw new Exception("错误：登录账号不正确"); }
                    if (action.Fill(sWhere))
                    {
                        bool isenable = action.Get<int>("status", 0) == 1;
                        if (!isenable) { throw new Exception("账号已禁用，请与管理员联系"); }
                        loginUserCode = action.Get("usercode", "");
                    }
                    else
                    {
                        throw new Exception("错误：登录密码不正确");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loginUserCode;

        }

     

        #endregion


        #region 顾健 增加 2018-11-08

        /// <summary>
        /// 根据会员编号 更新头像
        /// </summary>
        /// <param name="usercode">会员编号</param>
        /// <param name="headimg">头像地址</param>
        /// <returns></returns>
        public ExeMsgInfo UpDateHeadImgByUserCode(string usercode, string headimg)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (!VerificationHelper.CheckStr(usercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "用户编码不能为空";
                return exeMsgInfo;
            }
            string sWhere = " usercode=" + DbService.SetQuotesValue(usercode);
            MDataRow dataRow = DbService.GetOne(CurrentTableName,sWhere);
            dataRow.Set("headimg", headimg);
            return DbService.Update(CurrentTableName, dataRow, sWhere, "headimg", true);
        }
        #endregion

       
    }
}
