using Decoration.Interface;
using System;
using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Adf.Core.Db;
using CYQ.Data;
using Decoration.Interface.Entity;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 企业人员角色管理
    /// 王浩
    /// 2018-10-17
    /// </summary>
    public class CompanyRolelmpl : ICompanyRole
    {
        private const String CurrentTableName = DecorationDb.Table_Decoration_Companyrole;
        private const String VCurrentTableName = "decoration_companyrole";

        /// <summary>
        /// 初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        MDataRow ICompanyRole.InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }


        /// <summary>
        /// 获取企业用户信息列表
        /// </summary>
        /// <param name="roleName">用户名称</param>
        /// <param name="companyCode">企业编码</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string roleName, string companyCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";
            string sql = @"SELECT * from decoration_companyrole where companycode='" + companyCode + "' ";
            if (!String.IsNullOrEmpty(roleName))
            {
                sWhere += " and roleName like '%" + roleName + "%'";
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by roleorder desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 当前企业下的所有角色
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public MDataTable GetCompanyRole(string companyCode)
        {
            string sql = "SELECT * from decoration_companyrole where companycode = '" + companyCode + "' order by roleorder desc ";
            return DbService.GetTable(sql, 0);
        }

        /// <summary>
        /// 获取前台模块信息
        /// </summary>
        /// <returns></returns>
        public MDataTable GetFrontModuleCode()
        {
            string sql = "select * from adf_datadic where parentcode = 'frontmodulecode'";
            return DbService.GetTable(sql, 0);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="roleCode">企业用户编号</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithRoleCode(string roleCode, bool bCache = false)
        {
            String cacheKey = "CompanyRole-GetEntity" + roleCode;
            String strWhere = "rolecode=" + DbService.SetQuotesValue(roleCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }


        public MDataTable GetRoleList(string usercode)
        {
            string sql = @"select a1.*,a2.rolename from decoration_companyuser_role a1
left join decoration_companyrole a2 on a1.rolecode=a2.rolecode";

            return DbService.GetTable(sql, 0, " usercode="+DbService.SetQuotesValue(usercode));


        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String rolecode = dataRow.Get("rolecode", "");
            if (String.IsNullOrEmpty(rolecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色编码不能为空";
                return exeMsgInfo;
            }
            String companycode = dataRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }
            String rolename = dataRow.Get("rolename", "");
            if (String.IsNullOrEmpty(rolename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色名称不能为空";
                return exeMsgInfo;
            }
            if (DbService.Exists(CurrentTableName, "rolename=" + DbService.SetQuotesValue(dataRow.Get("rolename", ""))))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "角色已经存在";
                return exeMsgInfo;
            }
            String sWhere = "rolecode=" + DbService.SetQuotesValue(rolecode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前编码已经存在";
                return exeMsgInfo;
            }
            using (MAction action = new MAction(CurrentTableName))
            {
                action.Data.LoadFrom(dataRow);
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
            //String controlFieldNames = "rolecode,companycode,rolename,roleorder,frontmodulecode";
            //return DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
        }

        public ExeMsgInfo Add(MDataRow dataRow, string frontmodule1, string frontmodule2, string tiproles)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                String rolecode = dataRow.Get("rolecode", "");
                if (String.IsNullOrEmpty(rolecode))
                {
                    throw new Exception("角色编码不能为空");
                }
                String rolename = dataRow["rolename"].ToString();
                if (String.IsNullOrEmpty(rolename))
                {
                    throw new Exception("角色名称不能为空");
                }
                String companycode = dataRow["companycode"].ToString();
                if (String.IsNullOrEmpty(companycode))
                {
                    throw new Exception("企业编号不能为空");
                }
                String sWhere = " companycode=" + DbService.SetQuotesValue(companycode) + " and rolename=" + DbService.SetQuotesValue(rolename) ;
                if (DbService.Exists(CurrentTableName, sWhere))
                {
                    throw new Exception("当前企业下的角色名称已存在，不能重复添加");
                }

                using (MAction action = new MAction(DecorationDb.Table_Decoration_Companyrole))
                {
                    action.BeginTransation();
                    try
                    {
                        action.Data.LoadFrom(dataRow);
                        bool flag2 = action.Insert();
                        if (!flag2)
                        {
                            throw new Exception("企业角色表增加失败");
                        }

                        string sWhereFrontModuleRemove1 = "rolecode=" + DbService.SetQuotesValue(rolecode) + " and frontmoduletype=0";
                        string sWhereFrontModuleRemove2 = "rolecode=" + DbService.SetQuotesValue(rolecode) + " and frontmoduletype=1";
                        //切换 前端授权模块
                        action.ResetTable(DecorationDb.Table_Decoration_Role_FrontModule);
                        action.Delete(sWhereFrontModuleRemove1);
                        action.Delete(sWhereFrontModuleRemove2);

                        foreach (string item1 in frontmodule1.Split(','))
                        {
                            action.Set("frontmodulecode", item1);
                            action.Set("rolecode", rolecode);
                            action.Set("frontmoduletype", 0);
                            if (!action.Insert())
                            {
                                throw new Exception("授权表发生异常");
                            };
                        }


                        foreach (string item2 in frontmodule2.Split(','))
                        {
                            action.Set("frontmodulecode", item2);
                            action.Set("rolecode", rolecode);
                            action.Set("frontmoduletype", 1);
                            if (!action.Insert())
                            {
                                throw new Exception("授权表发生异常");
                            };
                        }

                        //切换 可提醒角色
                        action.ResetTable(DecorationDb.Table_Decoration_Role_TipRole);
                        string sWhereTipRoleRemove = "rolecode=" + DbService.SetQuotesValue(rolecode);
                        action.Delete(sWhereTipRoleRemove);

                        foreach (string item3 in tiproles.Split(','))
                        {
                            action.Set("tiprolecode", item3);
                            action.Set("rolecode", rolecode);
                            if (!action.Insert())
                            {
                                throw new Exception("提醒角色表发生异常");
                            };
                        }

                    }
                    catch (Exception ex)
                    {
                        action.RollBack();
                        throw ex;
                    }
                }

                exeMsgInfo.RetStatus = 100;
                exeMsgInfo.RetValue = "增加成功";
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String rolecode = dataRow.Get("rolecode", "");
            if (String.IsNullOrEmpty(rolecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色编码不能为空";
                return exeMsgInfo;
            }
            String rolename = dataRow["rolename"].ToString();
            if (String.IsNullOrEmpty(rolename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色名称不能为空";
                return exeMsgInfo;
            }

            if (!String.IsNullOrEmpty(rolename))
            {
                String aWhere = " rolename=" + DbService.SetQuotesValue(rolename) + " and rolecode <> " + DbService.SetQuotesValue(rolecode);
                if (DbService.Exists(CurrentTableName, aWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "角色名称已存在，不能重复添加";
                    return exeMsgInfo;
                }
            }
            String sWhere = " roleCode=" + DbService.SetQuotesValue(rolecode);
            String controlFieldNames = "rolename,roleorder,frontmodulecode";

            //原始方法更新丢失
            using (MAction action = new MAction(CurrentTableName))
            {
                action.Data.LoadFrom(dataRow);
                bool flag = action.Update(sWhere);
                if (flag)
                {
                    exeMsgInfo.RetStatus = 101;
                    exeMsgInfo.RetValue = "修改成功";
                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "修改失败";
                }

            }
            return exeMsgInfo;
            //return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }
        /// <summary>
        /// 修改角色及相关信息
        /// 奚德荣 2018.11.8
        /// </summary>
        /// <param name="dataRow">角色实体</param>
        /// <param name="frontmodule1">授权模块，多个用逗号分隔</param>
        /// <param name="frontmodule2">授权状态，多个用逗号分隔</param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns>返回ExeMsgInfo</returns>
        public ExeMsgInfo Update(MDataRow dataRow, string frontmodule1, string frontmodule2, string tiproles)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                String rolecode = dataRow.Get("rolecode", "");
                if (String.IsNullOrEmpty(rolecode))
                {
                    throw new Exception("角色编码不能为空");
                }
                String rolename = dataRow["rolename"].ToString();
                if (String.IsNullOrEmpty(rolename))
                {
                    throw new Exception("角色名称不能为空");
                }
                String companycode = dataRow["companycode"].ToString();
                if (String.IsNullOrEmpty(companycode))
                {
                    throw new Exception("企业编号不能为空");
                }
                String sWhere = " companycode=" + DbService.SetQuotesValue(companycode) + " and rolename=" + DbService.SetQuotesValue(rolename) + " and rolecode <> " + DbService.SetQuotesValue(rolecode);
                if (DbService.Exists(CurrentTableName, sWhere))
                {
                    throw new Exception("当前企业下的角色名称已存在，不能重复添加");
                }

                using (MAction action = new MAction(DecorationDb.Table_Decoration_Companyrole))
                {
                    action.BeginTransation();
                    try
                    {
                        string sWhereCompanyRole = "companycode="+DbService.SetQuotesValue(companycode)+" and rolecode="+DbService.SetQuotesValue(rolecode);
                        bool flag1 = action.Fill(sWhereCompanyRole);
                        if (!flag1)
                        {
                            throw new Exception("角色不存在，修改失败");
                        }

                        action.Data.SetState(0);
                        action.Data.Set("rolename", rolename, 2);
                        action.Data.Set("roleorder", dataRow.Get("roleorder",0), 2);
                        
                        bool flag2 = action.Update(sWhereCompanyRole);
                        if (!flag2)
                        {
                            throw new Exception("企业角色表修改失败");
                        }

                        string sWhereFrontModuleRemove1 = "rolecode=" + DbService.SetQuotesValue(rolecode) + " and frontmoduletype=0";
                        string sWhereFrontModuleRemove2 = "rolecode=" + DbService.SetQuotesValue(rolecode) + " and frontmoduletype=1";
                        //切换 前端授权模块
                        action.ResetTable(DecorationDb.Table_Decoration_Role_FrontModule);
                        action.Delete(sWhereFrontModuleRemove1);
                        action.Delete(sWhereFrontModuleRemove2);

                        foreach (string item1 in frontmodule1.Split(','))
                        {
                            action.Set("frontmodulecode", item1);
                            action.Set("rolecode", rolecode);
                            action.Set("frontmoduletype", 0);
                            if (!action.Insert())
                            {
                                throw new Exception("授权表发生异常");
                            };
                        }

                       
                        foreach (string item2 in frontmodule2.Split(','))
                        {
                            action.Set("frontmodulecode", item2);
                            action.Set("rolecode", rolecode);
                            action.Set("frontmoduletype",1);
                            if (!action.Insert())
                            {
                                throw new Exception("授权表发生异常");
                            };
                        }

                        //切换 可提醒角色
                        action.ResetTable(DecorationDb.Table_Decoration_Role_TipRole);
                        string sWhereTipRoleRemove = "rolecode=" + DbService.SetQuotesValue(rolecode);
                        action.Delete(sWhereTipRoleRemove);

                        foreach (string item3 in tiproles.Split(','))
                        {
                            action.Set("tiprolecode", item3);
                            action.Set("rolecode", rolecode);
                            if (!action.Insert())
                            {
                                throw new Exception("提醒角色表发生异常");
                            };
                        }

                    }
                    catch (Exception ex)
                    {
                        action.RollBack();
                        throw ex;
                    }
                }

                exeMsgInfo.RetStatus = 101;
                exeMsgInfo.RetValue = "修改成功";
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="roleCode">企业用户编号</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string roleCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(roleCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色编码不能为空";
                return exeMsgInfo;
            }
            String sWhere = " roleCode=" + DbService.SetQuotesValue(roleCode);

            string sql = @"select sum(t) as num from (
select count(*) as t from decoration_role_frontmodule where rolecode='"+ roleCode + "' union all select count(*)as t from decoration_role_tiprole  where rolecode= '"+ roleCode + "') ts";

            MDataRow dr = DbService.GetOne(sql, "");
            if (dr.Get("num", 0) > 0)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "该角色已使用，无法删除";
                return exeMsgInfo;
            }

            return DbService.Delete(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 根据角色编码获取前台模块
        /// </summary>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        public MDataRow FrontModuleCode(string rolecode)
        {
            string sql = @"
select * from decoration_companyrole where rolecode='" + rolecode + "'";
            return DbService.GetOne(sql, "");
        }




    }
}

