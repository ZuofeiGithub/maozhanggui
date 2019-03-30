using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Db;
using Adf.Core.Entity;
using Adf.Core.Util;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class SystemConfigurationImpl : ISystemConfiguration
    {
        private const string CurrentTableName = "decoration_systemconfiguration";
        private const string VCurrentTableName = "decoration_systemconfiguration";

        public MDataRow InitDataRow()
        {
            return new MDataRow(DBTool.GetColumns((object)"decoration_systemconfiguration"));
        }

        public MDataRow GetEntity(int dicId, bool bCache = false)
        {
            MDataRow mdataRow = (MDataRow)null;
            string str = "DicImpl-GetEntity" + (object)dicId;
            bool flag = false;
            if (bCache)
            {
                mdataRow = CacheHelper.Get<MDataRow>(str);
                if (mdataRow != null)
                    flag = true;
            }
            if (!flag)
            {
                mdataRow = DbService.GetOne("decoration_systemconfiguration", "DicId=" + (object)dicId ?? "", str, false);
                if (mdataRow != null)
                    CacheHelper.Set(str, (object)mdataRow);
            }
            return mdataRow;
        }

        public MDataRow GetEntityByDicCode(string sDicCode, bool bCache = false)
        {
            MDataRow mdataRow = (MDataRow)null;
            string str = "DicImpl-GetEntityByDicCode" + sDicCode;
            bool flag = false;
            if (bCache)
            {
                mdataRow = CacheHelper.Get<MDataRow>(str);
                if (mdataRow != null)
                    flag = true;
            }
            if (!flag)
            {
                mdataRow = DbService.GetOne("decoration_systemconfiguration", "diccode=" + DbService.SetQuotesValue(sDicCode, true), str, false);
                if (mdataRow != null)
                    CacheHelper.Set(str, (object)mdataRow);
            }
            return mdataRow;
        }

        public MDataTable GetList(string companyCode, string sDicCode, int pageIndex, int pageSize, ref int recordCount, ref int pageCount)
        {
            string str = "";
            if (!string.IsNullOrEmpty(sDicCode))
                str = " ParentCode='" + sDicCode + "' and companycode='" + companyCode + "'";
            string sWhere = str + " order by DicOrder desc ";
            return DbService.GetPageTable("decoration_systemconfiguration", pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        public ExeMsgInfo DeleteByDicCode(string sDicCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (string.IsNullOrEmpty(sDicCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            if (!DbService.Exists("decoration_systemconfiguration", "ParentCode=" + DbService.SetQuotesValue(sDicCode, true)))
                return DbService.Delete("decoration_systemconfiguration", "DicCode=" + DbService.SetQuotesValue(sDicCode, true));
            exeMsgInfo.RetStatus = 400;
            exeMsgInfo.RetValue = "存在子级不能删除";
            return exeMsgInfo;
        }

        public MDataTable GetAll(string sDicCode, string companyCode, bool bCache = false)
        {
            string cacheKey = "DicImpl-GetAll" + sDicCode;
            if (sDicCode == "root")
            {
                return DbService.GetTable("decoration_systemconfiguration", 0, " ParentCode=" + DbService.SetQuotesValue(sDicCode, true) + " Order by DicOrder desc", cacheKey, bCache);
            }
            return DbService.GetTable("decoration_systemconfiguration", 0, " ParentCode=" + DbService.SetQuotesValue(sDicCode, true) + " and companycode='" + companyCode + "'" + " Order by DicOrder desc", cacheKey, bCache);
        }

        /// <summary>
        /// 前端
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="parentCode"></param>
        /// <returns></returns>
        public MDataTable GetWithCompanyCodeAndParentCode(string companyCode, string parentCode)
        {
            string sWhere = " companycode=" + DbService.SetQuotesValue(companyCode) + " and parentcode=" + DbService.SetQuotesValue(parentCode);
            return DbService.GetTable("decoration_systemconfiguration", 0, sWhere);
        }

        public ExeMsgInfo Insert(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            dataRow["DicKey"].ToString();
            string fldValue = dataRow.Get<string>((object)"DicCode", "");
            string str = dataRow.Get<string>((object)"ParentCode", "");
            dataRow["isparent"].Value = (object)"true";
            if (string.IsNullOrEmpty(fldValue))
            {
                fldValue = Guid.NewGuid().ToString();
                dataRow.Set((object)"DicCode", (object)fldValue);
            }
            if (string.IsNullOrEmpty(str))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "父级编码不能为空";
                return exeMsgInfo;
            }
            if (str == "root")
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "父级模块不能是root编码";
                return exeMsgInfo;
            }
            if (DbService.Exists("decoration_systemconfiguration", "DicCode=" + DbService.SetQuotesValue(fldValue, true)))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "主键已经存在";
                return exeMsgInfo;
            }
            string fields = "dicname,dicorder,parentcode,shortname,dickey,dicvalue,diccode,dicdesc,isshow,isparent,companycode,type";
            return DbService.Insert("decoration_systemconfiguration", dataRow, fields, true);
        }

        public ExeMsgInfo UpdateByDicId(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string str = dataRow["DicKey"].ToString();
            int num = dataRow.Get<int>((object)"DicId", 0);
            dataRow["isparent"].Value = (object)true;
            if (string.IsNullOrEmpty(str))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "按钮编码不能为空";
                return exeMsgInfo;
            }
            string sWhere = " DicId=" + (object)num ?? "";
            string fields = "dicname,dicorder,shortname,dickey,dicvalue,dicdesc,isshow";
            return DbService.Update("decoration_systemconfiguration", dataRow, sWhere, fields, true);
        }

        public ExeMsgInfo UpdateByDicCode(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            string fldValue = dataRow.Get<string>((object)"diccode", "");
            if (string.IsNullOrEmpty(fldValue))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "主键编码不能为空";
                return exeMsgInfo;
            }
            string sWhere = " diccode=" + DbService.SetQuotesValue(fldValue, true);
            string fields = "dicname,dicorder,shortname,dickey,dicvalue,dicdesc,isshow,companycode,type";
            return DbService.Update("decoration_systemconfiguration", dataRow, sWhere, fields, true);
        }

        public ExeMsgInfo DeleteByDicId(int dicId)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (dicId > 0)
                return DbService.Delete("decoration_systemconfiguration", " DicId=" + (object)dicId + "'");
            exeMsgInfo.RetStatus = 400;
            exeMsgInfo.RetValue = "编码不能为空";
            return exeMsgInfo;
        }
    }
}
