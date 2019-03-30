using System;
using System.Collections.Generic;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class TipImpl : ITip
    {
        #region Implementation of ITip
        private const String CurrentTableName = "decoration_tip";
        private const String VCurrentTableName = "decoration_vtip";
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：初始化实体
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns(CurrentTableName));
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取实体
        /// </summary>
        /// <param name="tipcode">编号</param>
        /// <returns></returns>
        public MDataRow GetEntityByTipcode(string tipcode)
        {
            string sWhere = " tipcode=" + DbService.SetQuotesValue(tipcode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：列表
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectcatecode">阶段编号</param>
        /// <param name="rolecode">角色编号</param>
        /// <param name="tiptitle">标题</param>
        /// <param name="issend">是否发送</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageCount">当前页</param>
        /// <returns></returns>
        public MDataTable GetList(string projectcode, string projectcatecode, string rolecode, string tiptitle, string issend, int pageIndex, int pageSize,
            string orderBy, ref int recordCount, ref int pageCount)
        {
            string sWhere = "1=1 and projectcode=" + DbService.SetQuotesValue(projectcode);

            if (!string.IsNullOrEmpty(projectcatecode))
            {
                sWhere += " and projectcatecode=" + DbService.SetQuotesValue(projectcatecode);
            }



            if (!string.IsNullOrEmpty(tiptitle))
            {
                sWhere += " and tiptitle like '%" + tiptitle.Trim() + "%'";
            }

            if (!string.IsNullOrEmpty(rolecode))
            {
                sWhere += " and rolecode=" + DbService.SetQuotesValue(rolecode);
            }
            if (!string.IsNullOrEmpty(issend))
            {
                sWhere += " and issend=" + DbService.SetQuotesValue(issend);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sWhere += "order by " + orderBy;
            }
            else
            {
                sWhere += " order by createdatetime desc ";
            }



            return DbService.GetPageTable(VCurrentTableName, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }


        /// <summary>
        /// 获取提醒数据的重载方法
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="projectcatecode">阶段编号</param>
        /// <returns></returns>
        public MDataTable GetList(string projectcode, string projectcatecode)
        {
            string sWhere = "1=1 and projectcode=" + DbService.SetQuotesValue(projectcode);

            if (!string.IsNullOrEmpty(projectcatecode))
            {
                sWhere += " and projectcatecode=" + DbService.SetQuotesValue(projectcatecode);
            }
           
                sWhere += " order by createdatetime desc ";
          
            return DbService.GetTable(VCurrentTableName, 0, sWhere, "", false);
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
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

            if (exeMsgcheck.RetStatus == 100)
            {
                string sWhere = "tipcode=" + DbService.SetQuotesValue(dataRow.Get("tipcode", ""));
                if (DbService.Exists(CurrentTableName, sWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "当前编号已存在";
                    return exeMsgInfo;
                }

                string sCanFilds = "tipcode,rolecode,projectcatecode,tiptitle,tipcontent,senddatetime,createusercode,createdatetime,issend";
                exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, sCanFilds, true);
                return exeMsgInfo;
            }
            else
            {
                return exeMsgcheck;
            }
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-11-8
        /// 功能：增加
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <param name="rolecodes">提醒角色集</param>
        /// <returns></returns>
        public ExeMsgInfo Add( MDataRow dataRow,List<string> rolecodes)
        {

            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            using (MAction action = new MAction(CurrentTableName))
            {
                string tipcode = Guid.NewGuid().ToString("N");
                try
                {
                    action.BeginTransation();
                    if (!VerificationHelper.CheckStr(dataRow.Get("projectcatecode", "")))
                    {
                        throw new Exception("项目阶段编号不能为空");
                    }
                    if (!VerificationHelper.CheckStr(dataRow.Get("tipcontent", ""),1000))
                    {
                        throw new Exception("描述不能为空或超出字符1000限制");
                    }
                    if (!VerificationHelper.CheckStr(dataRow.Get("reminddatetime", "")))
                    {
                        throw new Exception("提醒时间不能为空");
                    }
                    if (!VerificationHelper.CheckStr(dataRow.Get("projectcode", "")))
                    {
                        throw new Exception("项目编号不能为空");
                    }

                    if (!VerificationHelper.CheckStr(dataRow.Get("taskcode", "")))
                    {
                        throw new Exception("任务编号不能为空");
                    }
                    if (!string.IsNullOrEmpty(dataRow.Get("tipimg", "")))
                    {
                        if (!VerificationHelper.CheckStr(dataRow.Get("tipimg", ""),2000))
                        {
                            throw new Exception("图片超出最大字符限制");
                        }
                    }
                  

                    action.Data.LoadFrom(dataRow);

                    action.Set("tipcode", tipcode);
                    action.Set("createdatetime", DateTime.Now);
                    action.Insert();

                    action.ResetTable("decoration_tip_role");
                    foreach (string rolecode in rolecodes)
                    {
                        action.Set("rolecode", rolecode);
                        action.Set("tipcode", tipcode);
                        action.Insert();
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
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：修改
        /// </summary>
        /// <param name="dataRow">实体</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByTipcode(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            ExeMsgInfo exeMsgcheck = new ExeMsgInfo();

            //最终验证
            exeMsgcheck = CheckDataRow(dataRow);

            if (exeMsgcheck.RetStatus == 100)
            {
                string sWhere = " tipcode=" + DbService.SetQuotesValue(dataRow.Get("tipcode", "")) + " and issend=1";

                if (DbService.Exists(CurrentTableName, sWhere))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "提醒已发送，无法修改";
                    return exeMsgInfo;
                }
                sWhere = "tipcode=" + DbService.SetQuotesValue(dataRow.Get("tipcode", ""));


                string sCanFilds = "rolecode,projectcatecode,tiptitle,tipcontent,issend";
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
        /// 时间：2018-10-19
        /// 功能：删除
        /// </summary>
        /// <param name="tipcode">编号</param>
        /// <returns></returns>
        public ExeMsgInfo DeleteByTipcode(string tipcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = " tipcode=" + DbService.SetQuotesValue(tipcode) + " ";

          
            exeMsgInfo = DbService.Delete(CurrentTableName, sWhere);
            return exeMsgInfo;
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：发送提醒
        /// </summary>
        /// <param name="tipcode">编号</param>
        /// <returns></returns>
        public ExeMsgInfo SendByTipcode(string tipcode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string sWhere = " tipcode=" + DbService.SetQuotesValue(tipcode) + " and issend=1";

            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "提醒已发送，请勿重复提醒";
                return exeMsgInfo;
            }
            sWhere = " tipcode=" + DbService.SetQuotesValue(tipcode);
            MDataRow dataRow = DbService.GetOne(CurrentTableName, sWhere);
            dataRow.Set("senddatetime", DateTime.Now);
            dataRow.Set("issend", 1);

            exeMsgInfo = DbService.Update(CurrentTableName, dataRow, sWhere, "senddatetime,issend", true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 创建人：侯鑫辉
        /// 时间：2018-10-22
        /// 功能：前端得到列表
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string userCode,string projectcode, int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {

            string sql = @"select * from " + VCurrentTableName + " where tipcode in ( select DISTINCT tipcode from decoration_tip_role where rolecode in ( select rolecode from decoration_companyuser_role where usercode ='" + userCode + "')) and projectcode="+DbService.SetQuotesValue(projectcode);

            string sWhere = " 1=1";
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = " order by createdatetime desc";
            }
            sWhere += orderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        public MDataTable GetAll(string userCode,string projectcode)
        {
            string sql = @"select * from " + VCurrentTableName + " where tipcode in ( select DISTINCT tipcode from decoration_tip_role where rolecode in ( select rolecode from decoration_companyuser_role where usercode ='" + userCode + "')) and projectcode='"+ projectcode + "'";
            return DbService.GetTable(sql, 0, "");

        }

        /// <summary>
        /// 创建人：侯鑫辉
        /// 时间：2018-10-22
        /// 功能：新增
        /// </summary>
        /// <param name="roleCode"></param>
        /// <param name="tipTitle"></param>
        /// <param name="tipContent"></param>
        /// <param name="createUserCode"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(string roleCode, string tipTitle, string tipContent, string createUserCode, string projectCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(roleCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "角色不能为空";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(tipTitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "标题不能为空";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(tipContent))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "内容不能为空";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(createUserCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建人不能为空";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(projectCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目编号不能为空";
                return exeMsgInfo;
            }

            MDataRow dtRow = InitDataRow();
            dtRow.Set("tipcode", Guid.NewGuid().ToString("N"));
            dtRow.Set("rolecode", roleCode);
            dtRow.Set("tiptitle", tipTitle);
            dtRow.Set("tipcontent", tipContent);
            dtRow.Set("createusercode", createUserCode);
            dtRow.Set("createdatetime", DateTime.Now);
            dtRow.Set("issend", 1);
            dtRow.Set("projectcode", projectCode);

            string controlFieldNames = "tipcode,rolecode,tiptitle,tipcontent,createusercode,createdatetime,issend,projectcode";
            return DbService.Insert(CurrentTableName, dtRow, controlFieldNames, true);
        }

        #endregion

        /// <summary>
        /// 获取可提醒角色
        /// </summary>
        /// <param name="rolecode"></param>
        /// <returns></returns>
        public MDataTable GetTipRole(string rolecode)
        {
            string sWhere = " rolecode="+DbService.SetQuotesValue(rolecode);
            return DbService.GetTable("decoration_role_tiprole", 0, sWhere);


        }


        public MDataTable GetAllTipRole(string usercode)
        {
            string sql = @"select * from decoration_companyrole
where rolecode in (select DISTINCT tiprolecode from decoration_role_tiprole where rolecode in(select rolecode from decoration_companyuser_role where usercode='"+ usercode + "'))";
            return DbService.GetTable(sql, 0, "");
        }

        #region 实体公共验证方法

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
                if (!VerificationHelper.CheckStr(dataRow.Get("tipcode", "")))
                {
                    throw new Exception("提醒编号不能为空");
                }

                if (!VerificationHelper.CheckStr(dataRow.Get("rolecode", "")))
                {
                    throw new Exception("角色编号不能为空");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("projectcatecode", "")))
                {
                    throw new Exception("项目阶段不能为空");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("tiptitle", ""), 100))
                {
                    throw new Exception("标题不能为空或字符超出最大限制");
                }
                if (!VerificationHelper.CheckStr(dataRow.Get("tipcontent", ""), 500))
                {
                    throw new Exception("内容不能为空或字符超出最大限制");
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