using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;
using Decoration.Interface.Entity;
using System;

namespace Decoration.Implement.MySql
{

    /// <summary>
    /// 功能：平台角色
    /// 创建时间：2018.11.13
    /// 创建人：张硕
    /// </summary>
    public class DecorationSysRoleImpl : IDecorationSysRole
    {

        private const String CurrentTableName = DecorationDb.Table_Decoration_sys_role;

        /// <summary>
        /// 功能：初始化
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        /// 功能：得到实体
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="roleCode">编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntityWithRoleCode(string roleCode, bool bCache = false)
        {
            String cacheKey = "CompanyRole-GetEntity" + roleCode;
            String strWhere = "rolecode=" + DbService.SetQuotesValue(roleCode);

            return DbService.GetOne(CurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 功能：增加
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow, string tiproles)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String rolecode = dataRow.Get("rolecode", "");
            if (String.IsNullOrEmpty(rolecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色编号不能为空";
                return exeMsgInfo;
            }

            String strWhere = "rolecode=" + DbService.SetQuotesValue(rolecode);
            if (DbService.Exists(CurrentTableName, strWhere))
            {

                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前角色编号已存在，不能重复添加";
                return exeMsgInfo;
            }

            String rolename = dataRow.Get("rolename", "");
            if (String.IsNullOrEmpty(rolename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色名称不能为空";
                return exeMsgInfo;

            }
            if (!VerificationHelper.CheckStr(dataRow.Get("roleorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            string sFields = "";
            using (MAction action = new MAction(CurrentTableName))
            {
                action.BeginTransation();

                try
                {
                    action.Data.LoadFrom(dataRow);
                    sFields = "rolecode,rolename,roleorder";
                    DbService.SetMAction(action, dataRow, sFields);

                    //先增加主表
                    if (action.Insert(InsertOp.None))
                    {
                        //切换可提醒角色
                        action.ResetTable(DecorationDb.Table_Decoration_sys_role_tiprole);
                        string sWhereTipRoleRemove = "rolecode=" + DbService.SetQuotesValue(rolecode);
                        action.Delete(sWhereTipRoleRemove);

                        foreach (string item3 in tiproles.Split(','))
                        {
                            action.Set("tiprolecode", item3);
                            action.Set("rolecode", rolecode);
                            action.Insert(InsertOp.None);
                        }
                    }
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "增加成功";


                }
                catch (Exception ex)
                {
                    action.RollBack();
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = ex.Message;
                }
            }

            return exeMsgInfo;
        }

        /// <summary>
        /// 功能：修改角色及相关信息
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="dataRow">角色实体</param>
        /// <param name="tiproles">可提醒角色，多个用逗号分隔</param>
        /// <returns>返回ExeMsgInfo</returns>
        public ExeMsgInfo Update(MDataRow dataRow, string tiproles)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String rolecode = dataRow.Get("rolecode", "");
            if (String.IsNullOrEmpty(rolecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色编号不能为空";
                return exeMsgInfo;
            }

         

            String rolename = dataRow.Get("rolename", "");
            if (String.IsNullOrEmpty(rolename))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色名称不能为空";
                return exeMsgInfo;

            }
            if (!VerificationHelper.CheckStr(dataRow.Get("roleorder", "")))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请输入正确的排序";
                return exeMsgInfo;
            }
            string sFields = "";
            string sWhereTipRoleRemove = "rolecode=" + DbService.SetQuotesValue(rolecode);
            using (MAction action = new MAction(CurrentTableName))
            {
                action.BeginTransation();

                try
                {
                    action.Data.LoadFrom(dataRow);
                    sFields = "rolecode,rolename,roleorder";
                    DbService.SetMAction(action, dataRow, sFields);

                    //先修改主表
                    if (action.Update(sWhereTipRoleRemove))
                    {
                        //切换可提醒角色
                        action.ResetTable(DecorationDb.Table_Decoration_sys_role_tiprole);

                        action.Delete(sWhereTipRoleRemove);

                        foreach (string item3 in tiproles.Split(','))
                        {
                            action.Set("tiprolecode", item3);
                            action.Set("rolecode", rolecode);
                            action.Insert(InsertOp.None);
                        }
                    }
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "修改成功";


                }
                catch (Exception ex)
                {
                    action.RollBack();
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = ex.Message;
                }
            }

            return exeMsgInfo;
        }

        /// <summary>
        /// 功能：删除
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="roleCode">编码</param>
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
            String sWhere = " rolecode=" + DbService.SetQuotesValue(roleCode);

            if (DbService.Exists(DecorationDb.Table_Decoration_sys_role_tiprole, sWhere))
            {

                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前角色编号已在可提醒角色表中存在,不可删除!";
                return exeMsgInfo;
            }
            try
            {
                using (MAction action = new MAction(DecorationDb.Table_Decoration_sys_role_tiprole))
                {
                    try
                    {
                        action.BeginTransation();
                        action.Delete(sWhere); //删除(从表)

                        action.ResetTable(CurrentTableName);//切换主表操作
                        action.Delete(sWhere); //删除Test_A表（主表）

                        exeMsgInfo.RetStatus = 103;
                        exeMsgInfo.RetValue = "删除成功";
                    }
                    catch (Exception ex)
                    {
                        action.RollBack();
                        exeMsgInfo.RetStatus = 400;
                        exeMsgInfo.RetValue = ex.Message;
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
        /// 功能：列表信息
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string roleName, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";

            if (!String.IsNullOrEmpty(roleName))
            {
                sWhere += " and roleName like '%" + roleName + "%'";
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by roleorder desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 功能：获得所有角色
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <returns></returns>
        public MDataTable GetRoleTable()
        {
            string sWhere = "1=1";

            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }

        /// <summary>
        /// 功能：根据角色编码获取可提醒数据
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        /// <param name="sRoleCode">角色编号</param>
        /// <returns></returns>
        public MDataTable GetTipRoleTableByRoleCode(string sRoleCode)
        {
            string sWhere = "1=1  and rolecode=" + DbService.SetQuotesValue(sRoleCode);

            return DbService.GetTable(DecorationDb.Table_Decoration_sys_role_tiprole, 0, sWhere);
        }

    }
}
