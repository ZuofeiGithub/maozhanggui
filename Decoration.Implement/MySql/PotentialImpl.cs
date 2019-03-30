using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;
using Decoration.Interface.Entity;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 王孝为
    /// 2018-11-07
    /// 报备客户信息
    /// </summary>
    public class PotentialImpl: IPotential
    {

        private const String CurrentTableName = DecorationDb.Table_Decoration_Potential;
        private const String VCurrentTableName = DecorationDb.Table_Decoration_VPotential;

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
        /// 获取一条记录
        /// </summary>
        /// <param name="potentialcode"></param>
        /// <returns></returns>
        public MDataRow GetEntityByCode(string potentialcode)
        {
            string sWhere = " potentialcode=" + DbService.SetQuotesValue(potentialcode);
            return DbService.GetOne(VCurrentTableName, sWhere);
        }

        /// <summary>
        /// 增加一个报备
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            
            //报备编号默认为guid
            string potentialCode = Guid.NewGuid().ToString("N");
            dataRow.Set("potentialcode", potentialCode);
            //创建日期默认为当前的日期

            string createdatetime = DateTime.Now.ToString();
            dataRow.Set("createdatetime", createdatetime);

            string potentialdate = DateTime.Now.ToString();
            dataRow.Set("potentialdate", potentialdate);

            String companycode = dataRow.Get("companycode", "");
            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }

            String potentialusercode = dataRow.Get("potentialusercode", "");
            if (String.IsNullOrEmpty(potentialusercode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "报备人编号不能尾款";
                return exeMsgInfo;
            }

            String potentialname = dataRow.Get("potentialname", "");
            if (String.IsNullOrEmpty(potentialname))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "姓名不能为空";
                return exeMsgInfo;
            }

            String potentialphone = dataRow.Get("potentialphone", "");
            if (String.IsNullOrEmpty(potentialphone))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "手机号不能为空";
                return exeMsgInfo;
            }

            String potentialaddress = dataRow.Get("potentialaddress", "");
            if (String.IsNullOrEmpty(potentialaddress))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "地址不能为空";
                return exeMsgInfo;
            }

            String controlFieldNames = "potentialcode,companycode,potentialusercode,potentialdate,potentialname,potentialphone,potentialaddress,potentialstyle,potentialsln,potentialarea,potentialbudget,potentialremark,createusercode,createdatetime";
            exeMsgInfo = DbService.Insert(CurrentTableName, dataRow, controlFieldNames, true);
            return exeMsgInfo;
        }

        /// <summary>
        /// 获取所有的列表 后台使用
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="potentialusername">报备人</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string companyCode, string potentialusername, int pageIndex, int pageSize, string orderBy, ref int recordCount,
            ref int pageCount)
        {
            string sWhere = " companycode = '"+ companyCode +"' ";

            if (!string.IsNullOrEmpty(potentialusername))
            {
                sWhere += " and potentialusername like '%" + potentialusername + "%' ";
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
    }
}
