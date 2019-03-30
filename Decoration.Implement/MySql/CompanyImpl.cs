using Decoration.Interface;
using System;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Adf.Core.Db;
using CYQ.Data;
using System.Collections.Generic;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 王浩
    /// 2018-10-13
    /// 公司列表
    /// </summary>
    public class CompanyImpl : ICompany
    {
        private const String CurrentTableName = "decoration_Company";
        private const String VCurrentTableName = "decoration_Company";


        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        public MDataRow GetEntityWithCompanyCode(string companyCode, bool bCache = false)
        {
            String cacheKey = "Company-GetEntity" + companyCode;
            String strWhere = "Companycode=" + DbService.SetQuotesValue(companyCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String companyCode = dataRow["Companycode"].ToString();

            if (String.IsNullOrEmpty(companyCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String companyname = dataRow["companyname"].ToString();
            if (String.IsNullOrEmpty(companyname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目名称不能为空";
                return exeMsgInfo;
            }
            String aWhere = "companyname=" + DbService.SetQuotesValue(companyname);
            if (DbService.Exists(CurrentTableName, aWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前项目名称已经存在";
                return exeMsgInfo;
            }
            String companyaddress = dataRow["companyaddress"].ToString();
            if (String.IsNullOrEmpty(companyaddress))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "公司地址不能为空";
                return exeMsgInfo;
            }
            String companylegalperson = dataRow["companylegalperson"].ToString();

            if (String.IsNullOrEmpty(companylegalperson))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业法人不能为空";
                return exeMsgInfo;
            }
            String companystatus = dataRow["companystatus"].ToString();
            if (String.IsNullOrEmpty(companystatus))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业状态不能为空";
                return exeMsgInfo;
            }
            String parentcode = dataRow["parentcode"].ToString();
            if (String.IsNullOrEmpty(parentcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "上级企业编号不能为空";
                return exeMsgInfo;
            }
            //String createusercode = dataRow["createusercode"].ToString();
            //if (String.IsNullOrEmpty(createusercode))
            //{
            //    exeMsgInfo.RetStatus = 400;
            //    exeMsgInfo.RetValue = "创建人不能为空";
            //    return exeMsgInfo;
            //}
            String createdatetime = dataRow["createdatetime"].ToString();
            if (String.IsNullOrEmpty(createdatetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建时间不能为空";
                return exeMsgInfo;
            }
            String sWhere = "Companycode=" + DbService.SetQuotesValue(companyCode);
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前编码已经存在";
                return exeMsgInfo;
            }

            String controlFieldNames = "companycode,companyname,createusercode,createdatetime,parentcode,companyaddress,companytel,companylegalperson,founddate,companyorder,companyremark,companystatus";
            return DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
        }

        /// <summary>
        /// 根据编码删除一条企业信息
        /// </summary>
        /// <param name="companyCode">公司编码</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string companyCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(companyCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            String sWhere = "companyCode=" + DbService.SetQuotesValue(companyCode);
            return DbService.Delete(CurrentTableName, sWhere);
        }

        /// <summary>
        /// 启用或禁用一条企业信息
        /// </summary>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public ExeMsgInfo Enable(string companycode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (String.IsNullOrEmpty(companycode)) { throw new Exception("参数不正确"); }
                string sql = @"
update decoration_Company set companystatus=ABS(ifnull(companystatus,0)-1)
where companycode=@companycode;
";
                sql = sql.Replace("@companycode", DbService.SetQuotesValue(companycode));
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



        public ExeMsgInfo Restart(string companycode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            using(MAction action =new MAction("decoration_sys_template"))
            {
                //初始化模板
                try
                {
                    action.BeginTransation();
                    //平台模板
                    MDataTable template = action.Select();
                    //平台角色
                    action.ResetTable("decoration_sys_role");
                    MDataTable sysrole = action.Select();
                    //平台任务类型
                    action.ResetTable("decoration_sys_taskcate");
                    MDataTable systaskcate = action.Select();
                    //任务角色
                    action.ResetTable("decoration_sys_taskrole");
                    MDataTable taskrole = action.Select();
                    //平台施工任务decoration_sys_task

                    //系统配置
                    action.ResetTable("decoration_systemconfiguration");
                    MDataTable sysconfig = DbService.GetTable("decoration_systemconfiguration", 0, " companycode='ntjd'");



                    //平台可提醒角色decoration_sys_role_tiprole
                    action.ResetTable("decoration_sys_role_tiprole");
                    MDataTable tiprole = action.Select();

                    if (template.Rows.Count <= 0)
                    {
                        throw new Exception("平台模板数据为空！");
                    }

                    if (sysrole.Rows.Count <= 0)
                    {
                        throw new Exception("平台角色数据为空！");
                    }

                    if (systaskcate.Rows.Count <= 0)
                    {
                        throw new Exception("平台任务类型数据为空！");
                    }
                    if (taskrole.Rows.Count <= 0)
                    {
                        throw new Exception("平台任务角色数据为空！");
                    }
                    if (sysconfig.Rows.Count <= 0)
                    {
                        throw new Exception("系统配置数据为空！");
                    }


                    //重新生成模板编号
                    action.ResetTable("decoration_tasktemplate");
                    action.Delete("companycode=" + DbService.SetQuotesValue(companycode));

                    action.ResetTable("decoration_companytaskcate");
                    action.Delete("companycode=" + DbService.SetQuotesValue(companycode));
                    action.ResetTable("decoration_companytaskrole");
                    action.Delete("companycode=" + DbService.SetQuotesValue(companycode));
                    action.ResetTable("decoration_companyrole");
                    action.Delete("companycode=" + DbService.SetQuotesValue(companycode));
                    action.ResetTable("decoration_role_tiprole");
                    action.Delete("companycode=" + DbService.SetQuotesValue(companycode));
                    action.ResetTable("decoration_systemconfiguration");
                    action.Delete("companycode=" + DbService.SetQuotesValue(companycode));

                    //企业任务模板
                    foreach (MDataRow dataRow in template.Rows)
                    {
                        action.ResetTable("decoration_tasktemplate");
                        string tempmembercode = Guid.NewGuid().ToString("N");
                        action.Data.LoadFrom(dataRow);
                        action.Set("companycode", companycode);
                        action.Set("templatecode", tempmembercode);
                        action.Insert();

                        //插入企业角色信息
                        Dictionary<string, string> rolelist = new Dictionary<string, string>();
                        action.ResetTable("decoration_companyrole");

                        foreach (MDataRow role in sysrole.Rows)
                        {

                            string rolecode = Guid.NewGuid().ToString("N") + "$" + role.Get("rolecode", "");
                            rolelist.Add(role.Get("rolecode", ""), rolecode);
                            if (action.Exists("companycode=" + DbService.SetQuotesValue(companycode) +
                                              " and rolecode like '%$"+ role.Get("rolecode", "") + "'" ))
                            {
                                continue;
                            }
                            action.Data.LoadFrom(role);
                            action.Set("companycode", companycode);
                            action.Set("rolecode", rolecode);
                            action.Insert();
                        }

                        foreach (KeyValuePair<string, string> kv in rolelist)
                        {
                            action.ResetTable("decoration_sys_role_tiprole");
                            MDataTable tiproles = action.Select("rolecode=" + DbService.SetQuotesValue(kv.Key));
                            action.ResetTable("decoration_role_tiprole");

                            foreach (MDataRow tip in tiproles.Rows)
                            {
                                action.Set("rolecode", kv.Value);
                                foreach (KeyValuePair<string, string> kv2 in rolelist)
                                {
                                    if (kv2.Key == tip.Get("tiprolecode", ""))
                                    {
                                        if (action.Exists("companycode=" + DbService.SetQuotesValue(companycode) +
                                                          " and rolecode like '%$" + kv.Value + "' and tiprolecode like '%$" + kv2.Value + "'"))
                                        {
                                            continue;
                                        }
                                        action.Set("tiprolecode", kv2.Value);
                                    }
                                }
                                action.Set("companycode", companycode);
                                action.Insert();
                            }
                        }



                        //企业任务分类


                      
                        MDataTable systaskcate1 =
                            systaskcate.Select("templatecode=" +
                                               DbService.SetQuotesValue(dataRow.Get("templatecode", "")));
                        foreach (MDataRow taskcate in systaskcate1.Rows)
                        {
                            string catecode = Guid.NewGuid().ToString("N");
                            //插入任务阶段
                            action.ResetTable("decoration_companytaskcate");
                            action.Data.LoadFrom(taskcate);
                            action.Set("companycode", companycode);
                            action.Set("catecode", catecode);
                            action.Set("templatecode", tempmembercode);

                            action.Insert();

                            action.ResetTable("decoration_sys_task");

                            MDataTable cate = action.Select("catecode=" + DbService.SetQuotesValue(taskcate.Get("catecode", "")));
                            //插入企业任务
                           
                            foreach (MDataRow c in cate.Rows)
                            {
                                action.ResetTable("decoration_companytask");
                                string taskcode = c.Get("taskcode", "");
                                string newTaskcode = Guid.NewGuid().ToString("N");
                                action.Data.LoadFrom(c);
                                action.Set("taskcode", newTaskcode);
                                action.Set("catecode", catecode);
                                action.Insert();
                                action.ResetTable("decoration_sys_taskrole");

                                MDataTable taskroles = action.Select("taskcode=" + DbService.SetQuotesValue(taskcode));
                                //插入任务角色关系
                              

                                foreach (MDataRow roles in taskroles.Rows)
                                {
                                    action.ResetTable("decoration_companytaskrole");
                                    string rolecode = "";
                                    foreach (KeyValuePair<string, string> kv in rolelist)
                                    {
                                        if (kv.Key == action.Get("rolecode", ""))
                                        {
                                            rolecode = kv.Value;
                                        }
                                    }
                                    action.Set("rolecode", rolecode);
                                    action.Set("taskcode", newTaskcode);
                                    action.Set("companycode", companycode);
                                    action.Insert();
                                }
                            }


                        }
                    }


                    //系统配置
                    foreach (MDataRow item in sysconfig.Rows)
                    {
                        action.ResetTable("decoration_systemconfiguration");
                        action.Data.LoadFrom(item);
                        action.Set("companycode", companycode);
                        action.Insert();
                    }

                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "初始化成功";
                }
                catch (Exception ex)
                {

                    action.RollBack();
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "初始化失败";

                }
            }
            return exeMsgInfo;
        }
        /// <summary>
        /// 获取该企业下的子公司列表
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="companyCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string companyName, string companyCode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)

        {
            String sWhere = " 1=1 ";
            string sql = @"select  * from decoration_company 

where parentcode='" + companyCode + "' OR companycode='" + companyCode + "'";
            String sOrderBy = "";

            if (!String.IsNullOrEmpty(companyName))
            {
                sWhere += " and companyName like '%" + companyName + "%'";
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by createdatetime desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }
        /// <summary>
        /// 根据当前登录的用户编号，获取该用户的企业信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public MDataRow GetCompanyWithUserCode(string userCode)
        {
            string sql = @"select b.* from decoration_companyuser as a  
LEFT JOIN decoration_company as b on a.companycode=b.companycode
WHERE a.usercode='" + userCode + "'";
            return DbService.GetOne(sql, "");
        }




        /// <summary>
        /// 根据编码修改一条企业信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String companyCode = dataRow["Companycode"].ToString();

            if (String.IsNullOrEmpty(companyCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String companyname = dataRow["companyname"].ToString();
            if (String.IsNullOrEmpty(companyname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业名称不能为空";
                return exeMsgInfo;
            }
            if (!String.IsNullOrEmpty(companyname))
            {
                String aWhere = " companyname=" + DbService.SetQuotesValue(companyname) + " and companyCode <> " + DbService.SetQuotesValue(companyCode);
                if (DbService.Exists(CurrentTableName, aWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前服务单位已经存在";
                    return exeMsgInfo;
                }
            }
            String companyaddress = dataRow["companyaddress"].ToString();
            if (String.IsNullOrEmpty(companyaddress))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "公司地址不能为空";
                return exeMsgInfo;
            }
            String companylegalperson = dataRow["companylegalperson"].ToString();

            if (String.IsNullOrEmpty(companylegalperson))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业法人不能为空";
                return exeMsgInfo;
            }
            String companystatus = dataRow["companystatus"].ToString();
            if (String.IsNullOrEmpty(companystatus))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业状态不能为空";
                return exeMsgInfo;
            }
            String parentcode = dataRow["parentcode"].ToString();
            if (String.IsNullOrEmpty(parentcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "上级企业编号不能为空";
                return exeMsgInfo;
            }
            String sWhere = "Companycode=" + DbService.SetQuotesValue(companyCode);

            String controlFieldNames = "companyname,companyaddress,companytel,companylegalperson,founddate,companyorder,companyremark";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }



        /// <summary>
        /// 修改当前登录的企业信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo UpdateWithAdmin(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String companyCode = dataRow["Companycode"].ToString();

            if (String.IsNullOrEmpty(companyCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String companyname = dataRow["companyname"].ToString();
            if (String.IsNullOrEmpty(companyname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业名称不能为空";
                return exeMsgInfo;
            }
            if (!String.IsNullOrEmpty(companyname))
            {
                String aWhere = " companyname=" + DbService.SetQuotesValue(companyname) + " and companyCode <> " + DbService.SetQuotesValue(companyCode);
                if (DbService.Exists(CurrentTableName, aWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前服务单位已经存在";
                    return exeMsgInfo;
                }
            }
            String companyaddress = dataRow["companyaddress"].ToString();
            if (String.IsNullOrEmpty(companyaddress))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "公司地址不能为空";
                return exeMsgInfo;
            }
            String companylegalperson = dataRow["companylegalperson"].ToString();

            if (String.IsNullOrEmpty(companylegalperson))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业法人不能为空";
                return exeMsgInfo;
            }
            String companystatus = dataRow["companystatus"].ToString();
            if (String.IsNullOrEmpty(companystatus))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业状态不能为空";
                return exeMsgInfo;
            }

            String sWhere = "Companycode=" + DbService.SetQuotesValue(companyCode);

            String controlFieldNames = "companyname,companyaddress,companytel,companylegalperson,founddate,companyorder,companyremark,companylogo";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }


        /// <summary>
        ///  初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        #region 管理员账号 管理企业 2018.10.20 侯鑫辉

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public MDataRow AdminGetEntityWithCompanyCode(string companyCode)
        {
            String sWhere = " companycode=" + DbService.SetQuotesValue(companyCode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 得到企业列表
        /// </summary>
        /// <param name="companyName">企业名称</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable AdminGetList(string companyName, int pageIndex, int pageSize, string orderBy,
            ref int recordCount, ref int pageCount)
        {
            String sWhere = " (parentcode='' or parentcode is null)";
            if (!String.IsNullOrEmpty(companyName))
            {
                sWhere += " and companyname like '%" + companyName + "%'";
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by createdatetime desc ";
            }
            sWhere += orderBy;
            return DbService.GetPageTable(CurrentTableName, pageIndex, pageSize, sWhere, ref recordCount,
                ref pageCount);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        public ExeMsgInfo AdminAdd(MDataRow dtRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            String companycode = dtRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }
            String companyname = dtRow.Get("companyname", "");
            if (String.IsNullOrEmpty(companyname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写企业名称";
                return exeMsgInfo;
            }
            String aWhere = "companyname=" + DbService.SetQuotesValue(companyname);
            if (DbService.Exists(CurrentTableName, aWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前企业名称已经存在";
                return exeMsgInfo;
            }
            String companylegalperson = dtRow.Get("companylegalperson", "");
            if (String.IsNullOrEmpty(companylegalperson))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业法人不能为空";
                return exeMsgInfo;
            }
            String companystatus = dtRow.Get("companystatus", "");
            if (String.IsNullOrEmpty(companystatus))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择企业状态";
                return exeMsgInfo;
            }
            String createusercode = dtRow.Get("createusercode", "");
            if (String.IsNullOrEmpty(createusercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建人不能为空";
                return exeMsgInfo;
            }
            String createdatetime = dtRow.Get("createdatetime", "");
            if (String.IsNullOrEmpty(createdatetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建时间不能为空";
                return exeMsgInfo;
            }

            String controlFieldNames = "companycode,companyname,createusercode,createdatetime,parentcode,companyaddress,companytel,companylegalperson,founddate,companyorder,companyremark,companystatus,companylogo,companycount";
            return DbService.Insert(CurrentTableName, dtRow, controlFieldNames, true);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        public ExeMsgInfo AdminUpdate(MDataRow dtRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            String companycode = dtRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }
            String companyname = dtRow.Get("companyname", "");
            if (String.IsNullOrEmpty(companyname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请填写企业名称";
                return exeMsgInfo;
            }
            String aWhere = " companyname=" + DbService.SetQuotesValue(companyname) + " and companyCode <> " + DbService.SetQuotesValue(companycode);
            if (DbService.Exists(CurrentTableName, aWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前企业名称已经存在";
                return exeMsgInfo;
            }
            String companylegalperson = dtRow.Get("companylegalperson", "");
            if (String.IsNullOrEmpty(companylegalperson))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业法人不能为空";
                return exeMsgInfo;
            }
            String companystatus = dtRow.Get("companystatus", "");
            if (String.IsNullOrEmpty(companystatus))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "请选择企业状态";
                return exeMsgInfo;
            }

            String sWhere = "companycode=" + DbService.SetQuotesValue(companycode);

            String controlFieldNames = "companyname,companyaddress,companytel,companylegalperson,founddate,companyorder,companyremark,companystatus,companylogo,companycount";
            return DbService.Update(CurrentTableName, dtRow, sWhere, controlFieldNames, true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public ExeMsgInfo AdminDelete(string companyCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(companyCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编码不能为空";
                return exeMsgInfo;
            }

            bool flag = DbService.Exists("decoration_companyuser", " companycode=" + DbService.SetQuotesValue(companyCode));
            if (flag)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "该企业已添加企业用户，无法删除";
                return exeMsgInfo;
            }

            String sWhere = "companycode=" + DbService.SetQuotesValue(companyCode);
            return DbService.Delete(CurrentTableName, sWhere);
        }

        #endregion
    }
}
