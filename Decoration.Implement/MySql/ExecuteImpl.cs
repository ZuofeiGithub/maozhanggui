using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class ExecuteImpl: IExecute
    {
        #region Implementation of IExecute

        private const String CurrentTableName = "decoration_execute";
        private const String VCurrentTableName = "decoration_execute";


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
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：根据任务编号获取执行情况
        /// </summary>
        /// <param name="taskcode">任务编号</param>
        /// <returns></returns>
        public MDataTable GetAll(string taskcode,string usercode,string taskstatus)
        {
            string sWhere = "";
            string sqlrole = @"select a1.usercode,a2.rolename from decoration_companyuser_role a1
left join decoration_companyrole a2 on a1.rolecode=a2.rolecode";
            MDataTable dt = DbService.GetTable(sqlrole, 0, "usercode='" + usercode + "'");
            bool flag = false;

            foreach (MDataRow mDataRow in dt.Rows)
            {
                if (mDataRow.Get("rolename", "") == "监理" || mDataRow.Get("rolename", "") == "项目经理")
                {
                    flag = true;
                }
            }
            if (taskstatus == "0")
            {
                if (flag)
                {
                    sWhere = " taskcode=" + DbService.SetQuotesValue(taskcode) ;
                }
                else
                {
                sWhere = " taskcode=" + DbService.SetQuotesValue(taskcode) + " and usercode='" + usercode + "'";

                }
            }
            else
            {
             sWhere = " taskcode=" + DbService.SetQuotesValue(taskcode)+"  and ifnull(taskstatus,0)="+ taskstatus;

            }
//            string sql = @"
//select a1.*,a2.username,a4.rolename,(select taskstatus from decoration_task where taskcode=a1.taskcode) as  taskstatus  from decoration_execute a1 
//left join decoration_vcompanyuser a2 on a1.usercode=a2.usercode
//left join decoration_companyuser_role a3 on a1.usercode=a3.usercode
//left join decoration_companyrole a4 on a3.rolecode=a4.rolecode";
            string sql = @"select a1.*,a2.username,(SELECT GROUP_CONCAT(rolename) from decoration_companyuser_role b1 
LEFT JOIN decoration_companyrole b2 on b1.rolecode=b2.rolecode where usercode=a2.usercode) as rolenames,
(select taskstatus from decoration_task where taskcode=a1.taskcode) as  taskstatus  from decoration_execute a1 
left join decoration_vcompanyuser a2 on a1.usercode=a2.usercode";
            return DbService.GetTable(sql, 0, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加
        /// </summary>
        /// <param name="taskcode">任务编号</param>
        /// <param name="usercode">会员编号</param>
        /// <param name="executecontent">内容</param>
        /// <param name="executeimage">图片</param>
        /// <returns></returns>
        public ExeMsgInfo Add(string taskcode, string usercode, string executecontent, string executeimage)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (string.IsNullOrEmpty(taskcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务编号不能为空";
                return exeMsgInfo;
            }
            if (string.IsNullOrEmpty(usercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "用户编号不能为空";
                return exeMsgInfo;
            }

            MDataRow entity = this.InitDataRow();
            entity.Set("executecode", Guid.NewGuid().ToString("N"));
            entity.Set("usercode", usercode);
            entity.Set("taskcode", taskcode);
            entity.Set("executetime", DateTime.Now);
            entity.Set("executecontent", executecontent);
            entity.Set("executeimgs", executeimage);
            entity.Set("issubmit", 1);
            return DbService.Insert(CurrentTableName, entity, "executecode,usercode,taskcode,executetime,executecontent,executeimgs,issubmit", true);

        }

        /// <summary>
        /// 根据项目编码查询任务执行记录
        /// 王浩
        /// 2018-10-20
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public MDataTable GetExecuteWithProjectCode(string projectCode)
        {
            return null;
        }

        /// <summary>
        /// 查询任务执行记录详细信息
        /// 王浩
        /// 2018-10-20
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public MDataRow GetExecute(string projectCode)
        {
            return null;
        }

        /// <summary>
        /// 修改任务记录
        /// 侯鑫辉 2018.11.13
        /// </summary>
        /// <param name="dtRow"></param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByExecuteCode(MDataRow dtRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string executecode = dtRow.Get("executecode", "");
            if (String.IsNullOrEmpty(executecode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "任务执行记录不能为空";
                return exeMsgInfo;
            }

            string sWhere = " executecode=" + DbService.SetQuotesValue(executecode);
            string fields = "executecontent,executeimgs";
            exeMsgInfo = DbService.Update(CurrentTableName, dtRow, sWhere, fields, true);
            return exeMsgInfo;
        }

        #endregion

        /// <summary>
        /// 删除任务记录
        /// 侯鑫辉 2018.11.13
        /// </summary>
        /// <returns></returns>
        public ExeMsgInfo DeleteByExecuteCode(string executeCode)
        {
            string sWhere = " executecode=" + DbService.SetQuotesValue(executeCode);
            ExeMsgInfo exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
            return exeMsgInfo;
        }


    }
}