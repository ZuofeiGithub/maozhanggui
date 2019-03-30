using Decoration.Interface;
using System;
using System.Collections.Generic;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Adf.Core.Db;
using Adf.Core.Util;
using CYQ.Data;
using Decoration.Interface.Entity;


namespace Decoration.Implement.MySql
{
    public class ProjectImpl : IProject
    {
        private const String CurrentTableName = "decoration_project";

        private const String VCurrentTableName =
            @"select a1.*,a2.username as createusername,a3.companyname from decoration_project a1
	          LEFT JOIN adf_user a2 on a1.createusercode = a2.usercode
              LEFT JOIN decoration_company a3 on a1.companycode = a3.companycode";

        private const string VComanyUser = "decoration_vprojectselectuser";
        #region Implementation of IProject

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：获取实体
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByProjectCode(string projectcode)
        {
            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：列表
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="projectname">项目名称</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetList(string companycode, string projectstatus, string projectname, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount)
        {
            string sWhere = "1=1";

            if (!string.IsNullOrEmpty(companycode))
            {
                sWhere += " and companycode=" + DbService.SetQuotesValue(companycode);
            }

            if (!string.IsNullOrEmpty(projectstatus))
            {
                sWhere += " and projectstatus=" + projectstatus;
            }

            if (!string.IsNullOrEmpty(projectname))
            {
                sWhere += " and projectname like '%" + projectname.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by createdatetime desc";
            }

            string sql = @" select t1.*,GROUP_CONCAT(concat_ws(',',ifnull(t3.username,''))) as username  from (select a1.*,a2.username as createusername,a3.companyname from decoration_project a1
	          LEFT JOIN adf_user a2 on a1.createusercode = a2.usercode
              LEFT JOIN decoration_company a3 on a1.companycode = a3.companycode) t1 
left join decoration_projectteam t2 on t1.projectcode=t2.projectcode 
left join decoration_vprojectselectuser t3 on t2.usercode=t3.usercode and ifnull(t3.rolename,'')='业主' group by t1.projectcode";

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：列表
        /// </summary>
        /// <param name="companycode">企业编号</param>
        /// <param name="usercode">用户编码</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetListByUserCode(string companycode, string usercode, string projectstatus, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount)
        {
            string sWhere = "1=1";

            if (!string.IsNullOrEmpty(companycode))
            {
                sWhere += " and companycode=" + DbService.SetQuotesValue(companycode);
            }

            if (!string.IsNullOrEmpty(projectstatus))
            {
                sWhere += " and projectstatus=" + projectstatus;
            }

            if (!string.IsNullOrEmpty(usercode))
            {
                sWhere += " and   projectcode in (select DISTINCT projectcode from decoration_projectteam  where usercode='" + usercode + "') ";
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by createdatetime desc";
            }

            string sql = @" 
select a1.*,ifnull(a4.tipcount,0) tipcount,getFirstHanZiCode(a1.projectname) as pinyin_index  from (
 select t1.*,GROUP_CONCAT(concat_ws(',',t3.username)) as username  from (select a1.*,a2.username as createusername,a3.companyname from decoration_project a1
	          LEFT JOIN adf_user a2 on a1.createusercode = a2.usercode
              LEFT JOIN decoration_company a3 on a1.companycode = a3.companycode) t1 left join decoration_projectteam t2 on t1.projectcode=t2.projectcode 
left join decoration_vprojectselectuser t3 on t2.usercode=t3.usercode group by t1.projectcode)a1
LEFT JOIN
(
select count(*) as tipcount,projectcode from decoration_vtip where tipcode in ( select DISTINCT tipcode from decoration_tip_role 
where rolecode in ( select rolecode from decoration_companyuser_role where usercode='" + usercode + "')) group by projectcode)a4 on a1.projectcode=a4.projectcode ";

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        public MDataTable GetAllListByUserCode(string companycode, string usercode, string projectstatus)
        {
            string sWhere = "1=1";

            if (!string.IsNullOrEmpty(companycode))
            {
                sWhere += " and companycode=" + DbService.SetQuotesValue(companycode);
            }

            if (!string.IsNullOrEmpty(projectstatus))
            {
                sWhere += " and projectstatus=" + projectstatus;
            }

            if (!string.IsNullOrEmpty(usercode))
            {
                sWhere += " and   projectcode in (select DISTINCT projectcode from decoration_projectteam  where usercode='" + usercode + "') ";
            }
          
          

            string sql = @" select t1.*,GROUP_CONCAT(concat_ws(',',t3.username)) as username  from (" + VCurrentTableName +
                         ") t1 left join decoration_projectteam t2 on t1.projectcode=t2.projectcode left join " +
                         VComanyUser + " t3 on t2.usercode=t3.usercode group by t1.projectcode";
            return DbService.GetTable(sql, 0, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);


            string sql = @"
select (ifnull(a1.companycount,0)-ifnull(a2.t,0)) as mind from decoration_Company a1
left join 

(select count(*) as t,companycode from decoration_project  group by companycode)

 a2 on a1.companycode=a2.companycode where a1.companycode='" + dataRow.Get("companycode", "") + "'";

            MDataRow company = DbService.GetOne(sql,
                "");

            int sumcount = company.Get("mind", 0);

            if (sumcount <= 0)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "已超出最大项目数量，请联系总后台管理员！";
                return exeMsgInfo;
            }


            if (exeMsgcheck.RetStatus == 100)
            {
                string sWhere = "projectcode=" + DbService.SetQuotesValue(dataRow.Get("projectcode", ""));
                if (DbService.Exists(CurrentTableName, sWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前编号已存在";
                    return exeMsgInfo;
                }
                string sCanFilds = "projectcode,projectname,companycode,createusercode,createdatetime,projectstatus,gps,projectaddress,projectprocess,startdate,enddate,projectdays,updatedatetime";
                exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, sCanFilds, true);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
        }

        /// <summary>
        /// 创建人：侯鑫辉
        /// 时间：2018-10-22
        /// 功能：增加
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectAddress">详细地址</param>
        /// <param name="createUserCode">创建人</param>
        /// <param name="companyCode">施工单位</param>
        public ExeMsgInfo Add(string projectName, string projectAddress, string createUserCode, string companyCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (!VerificationHelper.CheckStr(projectName, 100))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目名称不能为空或字符超出最大限制";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(companyCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "施工单位编号不能为空";
                return exeMsgInfo;
            }

            MDataRow dtRow = InitDataRow();
            dtRow.Set("projectcode", Guid.NewGuid().ToString("N"));
            dtRow.Set("projectname", projectName);
            dtRow.Set("companycode", companyCode);
            dtRow.Set("createusercode", createUserCode);
            dtRow.Set("createdatetime", DateTime.Now);
            dtRow.Set("projectstatus", 0);
            dtRow.Set("projectaddress", projectAddress);
            dtRow.Set("projectprocess", 0);
            dtRow.Set("projectdays", 0);

            string sCanFilds = "projectcode,projectname,companycode,createusercode,createdatetime,projectstatus,projectaddress,projectprocess,projectdays";
            return DbService.Insert(CurrentTableName, dtRow, sCanFilds, true);

        }

        /// <summary>
        /// [急][奚德荣]外部接口
        /// </summary>
        /// <param name="projectName">项目名称</param>
        /// <param name="projectAddress">项目地址</param>
        /// <param name="createUserCode">创建人编号</param>
        /// <param name="companyCode">企业编号</param>
        /// <param name="areaName">省市区名称</param>
        /// <param name="areaCode">省市区编号</param>
        /// <param name="houseNumber">门牌号</param>
        /// <returns></returns>
        public ExeMsgInfo Add(string projectName, string projectAddress, string createUserCode, string companyCode, string areaName,
            string areaCode, string houseNumber)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (!VerificationHelper.CheckStr(projectName, 100))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目名称不能为空或字符超出最大限制";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(projectAddress))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目地址不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(createUserCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "用户编号不能为空";
                return exeMsgInfo;
            }
            if (!VerificationHelper.CheckStr(companyCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }
            string projectcode = Guid.NewGuid().ToString("N");

            MDataRow dtRow = InitDataRow();
            dtRow.Set("projectcode", projectcode);
            dtRow.Set("projectname", projectName);
            dtRow.Set("companycode", companyCode);
            dtRow.Set("createusercode", createUserCode);
            dtRow.Set("createdatetime", DateTime.Now);
            dtRow.Set("projectstatus", 0);
            dtRow.Set("projectaddress", projectAddress);
            dtRow.Set("projectprocess", 0);
            dtRow.Set("projectdays", 0);
            dtRow.Set("areaname", areaName);
            dtRow.Set("areacode", areaCode);
            dtRow.Set("housenumber", houseNumber);


          

                string sql = @"
select (ifnull(a1.companycount,0)-ifnull(a2.t,0)) as mind from decoration_Company a1
left join 

(select count(*) as t,companycode from decoration_project  group by companycode)

 a2 on a1.companycode=a2.companycode where a1.companycode='"+ companyCode + "'";

                MDataRow company = DbService.GetOne(sql,
                    "");

                int sumcount = company.Get("mind", 0);

                if (sumcount <= 0)
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "已超出最大项目数量，请联系总后台管理员！";
                    return exeMsgInfo;
                }


            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();
                    action.Data.LoadFrom(dtRow);
                    action.Insert();

                    action.ResetTable("decoration_projectteam");
                    action.Set("teamcode", Guid.NewGuid().ToString("N"));
                    action.Set("projectcode", dtRow.Get("projectcode", ""));
                    action.Set("usercode", createUserCode);
                    action.Insert();
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
            //string sCanFilds = "projectcode,projectname,companycode,createusercode,createdatetime,projectstatus,projectaddress,projectprocess,projectdays,areacode,areaname,housenumber";
            //return DbService.Insert(CurrentTableName, dtRow, sCanFilds, true);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByProjectCode(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                string sWhere = "projectcode=" + DbService.SetQuotesValue(dataRow.Get("projectcode", ""));
                //if (VerificationHelper.CheckProjectIsExecute(dataRow.Get("projectcode", "")))
                //{
                //    exeMsgInfo.RetValue = "当前项目正在执行，无法修改！";
                //    exeMsgInfo.RetStatus = 400;
                //    return exeMsgInfo;
                //}

                string sCanFilds = "projectname,projectstatus,gps,projectaddress,startdate,enddate,projectdays,updatedatetime";
                exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, sCanFilds, true);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：修改
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectname">项目名称</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="projectaddress">位置</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByProjectCode(string projectcode, string projectname, string projectstatus, string startdate,
            string enddate, string projectaddress,string areaName,string areaCode,string houseNumber)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();
            if (string.IsNullOrEmpty(projectcode))
            {
                exeMsgInfo.RetValue = "未找到信息";
                exeMsgInfo.RetStatus = 400;
                return exeMsgInfo;
            }
            string sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode);
            MDataRow dataRow = DbService.GetOne(CurrentTableName, sWhere);
            dataRow.Set("projectname", projectname);
            dataRow.Set("projectstatus", projectstatus);
            dataRow.Set("startdate", startdate);
            dataRow.Set("enddate", enddate);
            dataRow.Set("projectaddress", projectaddress);
            dataRow.Set("updatedatetime", DateTime.Now);
            dataRow.Set("areaname", areaName);
            dataRow.Set("areacode", areaCode);
            dataRow.Set("housenumber", houseNumber);
            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                //if (VerificationHelper.CheckProjectIsExecute(dataRow.Get("projectcode", "")))
                //{
                //    exeMsgInfo.RetValue = "当前项目正在执行，无法修改！";
                //    exeMsgInfo.RetStatus = 400;
                //    return exeMsgInfo;
                //}

                string sCanFilds = "projectname,projectstatus,projectaddress,startdate,enddate,updatedatetime,areaname,areacode,housenumber";
                exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, sCanFilds, true);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
        }

        public ExeMsgInfo UpdateByProjectCode1(string projectcode, string projectname,  string projectaddress, string areaName, string areaCode, string houseNumber)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();
            if (string.IsNullOrEmpty(projectcode))
            {
                exeMsgInfo.RetValue = "未找到信息";
                exeMsgInfo.RetStatus = 400;
                return exeMsgInfo;
            }
            string sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode);
            MDataRow dataRow = DbService.GetOne(CurrentTableName, sWhere);
            dataRow.Set("projectname", projectname);
            dataRow.Set("projectaddress", projectaddress);
            dataRow.Set("updatedatetime", DateTime.Now);
            dataRow.Set("areaname", areaName);
            dataRow.Set("areacode", areaCode);
            dataRow.Set("housenumber", houseNumber);
            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                //if (VerificationHelper.CheckProjectIsExecute(dataRow.Get("projectcode", "")))
                //{
                //    exeMsgInfo.RetValue = "当前项目正在执行，无法修改！";
                //    exeMsgInfo.RetStatus = 400;
                //    return exeMsgInfo;
                //}

                string sCanFilds = "projectname,projectaddress,updatedatetime,areaname,areacode,housenumber";
                exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, sCanFilds, true);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：修改
        /// [急][奚哥]
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectname">项目名称</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="startdate">开始时间</param>
        /// <param name="enddate">结束时间</param>
        /// <param name="projectaddress">位置</param>
        /// <param name="houseNumber">门牌号</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByProjectCode(string projectcode, string projectname, string projectstatus, string startdate,
            string enddate, string projectaddress, string houseNumber)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();
            if (string.IsNullOrEmpty(projectcode))
            {
                exeMsgInfo.RetValue = "未找到信息";
                exeMsgInfo.RetStatus = 400;
                return exeMsgInfo;
            }
            string sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode);
            MDataRow dataRow = DbService.GetOne(CurrentTableName, sWhere);
            dataRow.Set("projectname", projectname);
            dataRow.Set("projectstatus", projectstatus);
            dataRow.Set("startdate", startdate);
            dataRow.Set("enddate", enddate);
            dataRow.Set("projectaddress", projectaddress);
            dataRow.Set("updatedatetime", DateTime.Now);
            dataRow.Set("housenumber", houseNumber);
            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                //if (VerificationHelper.CheckProjectIsExecute(dataRow.Get("projectcode", "")))
                //{
                //    exeMsgInfo.RetValue = "当前项目正在执行，无法修改！";
                //    exeMsgInfo.RetStatus = 400;
                //    return exeMsgInfo;
                //}

                string sCanFilds = "projectname,projectstatus,projectaddress,startdate,enddate,updatedatetime,housenumber";
                exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, sCanFilds, true);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
        }


        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：删除
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteByProjectCode(string projectcode)
        {

            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();
            Dictionary<string, string> curDic = new Dictionary<string, string>();
            curDic.Add("decoration_projectteam", "【服务团队】中正在使用，无法删除");
            curDic.Add("decoration_taskcate", "【任务阶段】中正在使用，无法删除");
            curDic.Add("decoration_task", "【施工任务】中正在使用，无法删除");
            curDic.Add("decoration_doc", "【设计档案】中正在使用，无法删除");
            curDic.Add("decoration_case", "【精选案例】中正在使用，无法删除");
            curDic.Add("decoration_materiaapplay", "【工程材料申请】中正在使用，无法删除");

            string sWhere = " projectcode=" + DbService.SetQuotesValue(projectcode);
            exeMsgcheck = VerificationHelper.CheckIsDelete(curDic, sWhere);

            if (exeMsgcheck.RetStatus == 100)
            {
                exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
            //最终验证
            // exeMsgcheck = VerificationHelper.CheckIsDelete(curDic, sWhere);

            //exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
            //return exeMsgInfo;

        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-18
        /// 功能：总数
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public int GetRemindCount(string companyCode)
        {

            MDataTable dt = DbService.GetTable(CurrentTableName, 0,
                "companycode=" + DbService.SetQuotesValue(companyCode));
            int count = 0;
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows.Count;
            }
            return count;

        }

        /// <summary>
        /// [急][奚哥]导入模板 为当前项目引入阶段以及任务
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="templateCode">模板编号</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public ExeMsgInfo ImportTemplate(string projectCode, string templateCode, string startDate, string endDate)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (!VerificationHelper.CheckStr(projectCode))
                {
                    throw new Exception("项目编号不能为空");
                }
                if (!VerificationHelper.CheckStr(templateCode))
                {
                    throw new Exception("模板编号不能为空");
                }
                if (!VerificationHelper.CheckStr(startDate))
                {
                    throw new Exception("开始时间不能为空");
                }

                using (MAction action = new MAction(DecorationDb.Table_Decoration_TaskTemplate))
                {
                    action.BeginTransation();
                    try
                    {
                        //根据模板编号获取要导入的模板信息
                        String sWhereTaskTemplate = "templatecode=" + DbService.SetQuotesValue(templateCode);
                        bool flag11 = action.Fill(sWhereTaskTemplate);
                        if (!flag11) { throw new Exception("模板不存在，导入失败"); }

                        string templatename = action.Get("templatename", ""); //模板名称
                        int totaldays = action.Get<int>("totaldays", 0);
                        if (totaldays < 1) { throw new Exception("模板中的总天数小于1天，导入失败"); }
                        DateTime projectStartDate = ConvertHelper.ObjectToT(startDate, DateTime.Today);
                        DateTime projectEndDate = projectStartDate.AddDays(totaldays - 1);

                        //切换项目表
                        action.ResetTable(DecorationDb.Table_Decoration_Project);
                        string sWhereProject = "projectcode=" + DbService.SetQuotesValue(projectCode);
                        bool flag21 = action.Fill(sWhereProject);
                        if (!flag21) { throw new Exception("项目不存在，导入失败"); }
                        string templatecode = action.Get("templatecode", ""); //项目中的模板编号
                        string projectname = action.Get("projectname", ""); //项目名称
                        if (!String.IsNullOrEmpty(templatecode)) { throw new Exception("【" + projectname + "】该项目已经导入过模板【" + templatename + "】，导入失败"); }
                        action.Data.SetState(0);
                        action.Data.Set("startdate", projectStartDate, 2);
                        action.Data.Set("enddate", projectEndDate, 2);
                        action.Data.Set("projectdays", totaldays, 2);
                        action.Data.Set("templatecode", templateCode, 2);
                        action.Data.Set("projectstatus", 1, 2);
                        action.Data.Set("projectcatename", "进行中", 2);
                        action.Data.Set("projectprocess", 0, 2);
                        bool flag22 = action.Update(sWhereProject);
                        if (!flag22) { throw new Exception("项目更新异常，导入失败"); }

                        //切换企业任务分类（阶段）
                        action.ResetTable(DecorationDb.Table_Decoration_Companytaskcate);
                        string sWhereCompanyTaskCate = "templatecode=" + DbService.SetQuotesValue(templateCode) + " order by cateorder desc";
                        MDataTable dtCompanyTaskCate = action.Select(sWhereCompanyTaskCate);
                        if (dtCompanyTaskCate == null || dtCompanyTaskCate.Rows.Count == 0)
                        {
                            throw new Exception("当前模板【" + templatename + "】的任务分类没有数据，导入失败");
                        }

                        //清空项目任务阶段
                        action.ResetTable(DecorationDb.Table_Decoration_Taskcate);
                        string sWhereTaskCateRemove = "projectcode = " + DbService.SetQuotesValue(projectCode);
                        if (action.Exists(sWhereTaskCateRemove))
                        {
                            bool flag31 = action.Delete(sWhereTaskCateRemove);
                            if (!flag31) { throw new Exception("当前项目【" + projectname + "】中的任务阶段清除失败"); }
                        }

                        //清空项目任务阶段下的所有相关任务
                        action.ResetTable(DecorationDb.Table_Decoration_Task);
                        string sWhereTaskRemove = "projectcode=" + DbService.SetQuotesValue(projectCode);
                        if (action.Exists(sWhereTaskRemove))
                        {
                            bool flag41 = action.Delete(sWhereTaskRemove);
                            if (!flag41) { throw new Exception("当前项目【" + projectname + "】中的相关任务清除失败"); }
                        }

                        DateTime tmpdate = projectStartDate; //记录任务开始时间
                        DateTime tmpdate2 = projectStartDate; //记录阶段开始时间
                        foreach (MDataRow drTaskCate in dtCompanyTaskCate.Rows)
                        {
                            string catecode = drTaskCate.Get("catecode", "");
                            string catename = drTaskCate.Get("catename", "");
                            int cateorder = drTaskCate.Get<int>("cateorder", 0);
                            string projectcatecode = Guid.NewGuid().ToString("N");

                            action.ResetTable(DecorationDb.Table_Decoration_Taskcate); //切换项目任务阶段表
                            action.Set("projectcatecode", projectcatecode);
                            action.Set("projectcode", projectCode);
                            action.Set("catecode", catecode);
                            action.Set("catename", catename);
                            action.Set("cateorder", cateorder);

                            bool flag32 = action.Insert(InsertOp.None);
                            if (!flag32)
                            {
                                throw new Exception("项目任务阶段【" + catename + "】导入失败");
                            }

                            action.ResetTable(DecorationDb.Table_Decoration_Companytask); //切换企业施工任务表
                            string sWhereCompanyTask = "catecode=" + DbService.SetQuotesValue(catecode) + " order by taskorder desc ";
                            MDataTable dtCompanyTask = action.Select(sWhereCompanyTask) ?? new MDataTable();

                            int totaltaskdays = 0;
                            DateTime taskcateStartDate = tmpdate2; //项目任务阶段的开始时间
                            foreach (MDataRow drCompanyTask in dtCompanyTask.Rows)
                            {
                               action.ResetTable(DecorationDb.Table_Decoration_Task); //切换项目施工任务表
                                string taskname = drCompanyTask.Get("taskname", "");
                                string taskremark = drCompanyTask.Get("taskremark", "");
                                int taskdays = drCompanyTask.Get<int>("taskdays", 1); //默认1天
                                int taskorder = drCompanyTask.Get<int>("taskorder", 0);
                                string taskcode = Guid.NewGuid().ToString("N");

                                //--重点（任务开始时间及结束时间）---
                                DateTime taskstartdate = tmpdate;
                                if (taskdays == 0) { taskdays = 1; } //排序等于0的情况
                                DateTime taskenddate = tmpdate.AddDays(taskdays - 1);
                                //---------------
                                action.Data.Set("taskcode", taskcode);
                                action.Data.Set("projectcode", projectCode);
                                action.Data.Set("taskname", taskname);
                                action.Data.Set("taskstatus", 0); //待完成
                                action.Data.Set("taskdes", taskremark);
                                action.Data.Set("catecode", catecode);
                                action.Data.Set("taskstartdate", taskstartdate);
                                action.Data.Set("taskenddate", taskenddate);
                                action.Data.Set("taskdays", taskdays);
                                action.Data.Set("taskorder", taskorder); //排序
                                action.Data.Set("issendtocustomer", 0); //是否需要业主评价(默认不需要）

                                bool flag51 = action.Insert(InsertOp.None);
                                if (!flag51)
                                {
                                    throw new Exception("项目施工任务【" + taskname + "】导入失败");
                                }
                                tmpdate = taskenddate.AddDays(1); //结束时间向后推移1天，作为下一任务的开始时间

                                totaltaskdays += taskdays; //当前阶段下的总天数

                                action.ResetTable("decoration_companytaskrole");

                                string sWhereTaskRole = " taskcode=" + DbService.SetQuotesValue(taskcode);
                                MDataRow taskrole = new MDataRow();
                                if (action.Fill(sWhereTaskRole))
                                {
                                    taskrole = action.Data;
                                }

                                action.ResetTable("decoration_taskrole");
                                action.Delete("taskcode=" + DbService.SetQuotesValue(taskrole.Get("taskcode", "")));
                                action.Set("taskcode", taskrole.Get("taskcode", ""));
                                action.Set("rolecode", taskrole.Get("rolecode", ""));
                                if (!action.Insert())
                                {
                                    throw new Exception("项目施工任务【" + taskname + "所属角色】导入失败");
                                }

                            }
                            if (totaltaskdays < 1) { throw new Exception("导入失败,【" + catename + "】的总天数不能小于1天"); }
                            DateTime taskcateEndDate = tmpdate2.AddDays(totaltaskdays - 1);//项目任务阶段的结束时间
                            tmpdate2 = taskcateEndDate.AddDays(1);//结束时间向后推移1天，作为下一阶段的开始时间

                            //返填项目任务阶段中的时间及天数
                            action.ResetTable(DecorationDb.Table_Decoration_Taskcate);
                            string sWhereTaskCate = "projectcatecode=" + DbService.SetQuotesValue(projectcatecode);
                            if (action.Fill(sWhereTaskCate))
                            {
                                action.Set("startdate", taskcateStartDate);
                                action.Set("enddate", taskcateEndDate);
                                action.Set("days", totaltaskdays);
                                bool flag61 = action.Update(sWhereTaskCate);
                                if (!flag61)
                                {
                                    throw new Exception("返填写项目任务阶段失败");
                                }
                            }

                         
                        }

                        exeMsgInfo.RetStatus = 200;
                        exeMsgInfo.RetValue = templatename + " | 模板导入成功";

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
                exeMsgInfo.RetValue = ex.Message;
                exeMsgInfo.RetStatus = 400;
            }

            return exeMsgInfo;
        }

        /// <summary>
        /// 获取所有项目 排除自己
        /// </summary>
        /// <param name="projectcode"></param>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public MDataTable GetAll(string projectcode, string companycode)
        {
            string sWhere = " companycode=" + DbService.SetQuotesValue(companycode) + " and projectcode <>" +
                            DbService.SetQuotesValue(projectcode);
            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }


        ///// <summary>
        ///// 项目同步，同步导入模板中的数据
        ///// </summary>
        ///// <param name="projectcode"></param>
        ///// <returns></returns>
        //public ExeMsgInfo Synchronization(string projectcode)
        //{
        //    ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
        //    MDataRow project = DbService.GetOne(CurrentTableName, " projectcode=" + DbService.SetQuotesValue(projectcode));

        //    if (project == null)
        //    {
        //        exeMsgInfo.RetStatus = 400;
        //        exeMsgInfo.RetValue = "项目信息发生错误";
        //        return exeMsgInfo;
        //    }

        //    string templatecode = project.Get("templatecode", "");
        //    string companycode = project.Get("companycode", "");

        //    using(MAction action=new MAction("decoration_companytaskcate"))
        //    {
        //        try
        //        {
        //            string sWhere = "companycode=" + DbService.SetQuotesValue(companycode) + " and templatecode=" + DbService.SetQuotesValue(templatecode);
        //            MDataTable taskCate = action.Select(sWhere);


        //            action.ResetTable("decoration_taskcate");
        //            sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode);
        //            MDataTable projectTaskCate = action.Select(sWhere);
        //            foreach (MDataRow projecttaskcate in projectTaskCate.Rows)
        //            {
        //                foreach (MDataRow taskcate in taskCate.Rows)
        //                {
        //                    string catecode = projecttaskcate.Get("catecode", "");
        //                    if (taskcate.Get("catename", "") == projecttaskcate.Get("catename", ""))
        //                    {

        //                    }
        //                    else {
                               
        //                        action.Set("projectcatecode", Guid.NewGuid().ToString("N"));
        //                        action.Set("projectcode", projectcode);
        //                        action.Set("catecode",catecode);
        //                        action.Set("catename", projecttaskcate.Get("catename",""));
        //                        action.Set("cateorder", projecttaskcate.Get("cateorder", ""));
        //                    }

        //                }
        //            }




        //            action.ResetTable("decoration_companytask");



        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //    }
        //}

        #endregion


        #region 项目实体公共验证方法

        /// <summary>
        /// 验证数据是否通过
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public static ExeMsgInfo CheckDataRow(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                if (!VerificationHelper.CheckStr(dataRow.Get("projectcode", "")))
                {
                    throw new Exception("项目编号不能为空");
                }

                if (!VerificationHelper.CheckStr(dataRow.Get("projectname", ""), 100))
                {
                    throw new Exception("项目名称不能为空或字符超出最大限制");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("companycode", "")))
                {
                    throw new Exception("施工单位编号不能为空");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("projectstatus", "")))
                {
                    throw new Exception("项目状态不能为空");
                }
                if (VerificationHelper.CheckStr(dataRow.Get("gps", "")))
                {
                    if (dataRow.Get("gps", "").Split(',').Length != 2)
                    {
                        throw new Exception("地理位置格式有误，请使用英文下逗号分开");
                    }
                }

                exeMsgInfo.RetStatus = 100;
            }
            catch (Exception ex)
            {

                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }
            return exeMsgInfo;
        }
        #endregion
    }
}
