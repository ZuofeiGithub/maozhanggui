using Decoration.Interface;
using System;
using Adf.Core.Entity;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Adf.Core.Db;
using CYQ.Data;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 王浩
    /// 2018-10-13
    /// 首页轮换图列表
    /// </summary>
    public class BannerImpl : IBanner
    {
        private const String CurrentTableName = "decoration_banner";
        private const String VCurrentTableName = "decoration_banner";


        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="bannerCode"></param>
        /// <param name="bCache"></param>
        /// <returns></returns>
        public MDataRow GetEntityWithBannerCode(string bannerCode, bool bCache = false)
        {
            String cacheKey = "Banner-GetEntity" + bannerCode;
            String strWhere = "bannercode=" + DbService.SetQuotesValue(bannerCode);

            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, bCache);
        }

        public MDataTable GetSelectRole(string bannerCode)
        {
            string sWhere = " bannerCode=" + DbService.SetQuotesValue(bannerCode);
            return DbService.GetTable("decoration_banner_role", 0, sWhere);
        }

        /// <summary>
        /// 增加一个首页轮换图信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String bannerCode = dataRow["bannercode"].ToString();

            if (String.IsNullOrEmpty(bannerCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String bannertitle = dataRow["bannertitle"].ToString();
            if (String.IsNullOrEmpty(bannertitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "标题不能为空";
                return exeMsgInfo;
            }
            String bannerimg = dataRow["bannerimg"].ToString();
            if (String.IsNullOrEmpty(bannerimg))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "图片不能为空";
                return exeMsgInfo;
            }
           
            String companycode = dataRow["companycode"].ToString();

            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }
            String createdatetime = dataRow["createdatetime"].ToString();
            if (String.IsNullOrEmpty(createdatetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建时间不能为空";
                return exeMsgInfo;
            }
            //String createusercode = dataRow["createusercode"].ToString();

            //if (String.IsNullOrEmpty(createusercode))
            //{
            //    exeMsgInfo.RetStatus = 400;
            //    exeMsgInfo.RetValue = "创建人不能为空";
            //    return exeMsgInfo;
            //}
            String controlFieldNames = "bannercode,bannertitle,bannerimg,redirecturl,isshow,createdatetime,createusercode,companycode";
            return DbService.Insert(CurrentTableName, dataRow, controlFieldNames,true);
        }

        /// <summary>
        /// 增加一个首页轮换图信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Add(MDataRow dataRow,string rolecodes)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String bannerCode = dataRow["bannercode"].ToString();

            if (String.IsNullOrEmpty(bannerCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String bannertitle = dataRow["bannertitle"].ToString();
            if (String.IsNullOrEmpty(bannertitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "标题不能为空";
                return exeMsgInfo;
            }
            String bannerimg = dataRow["bannerimg"].ToString();
            if (String.IsNullOrEmpty(bannerimg))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "图片不能为空";
                return exeMsgInfo;
            }

            String companycode = dataRow["companycode"].ToString();

            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }
            String createdatetime = dataRow["createdatetime"].ToString();
            if (String.IsNullOrEmpty(createdatetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建时间不能为空";
                return exeMsgInfo;
            }


            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();

                    action.Data.LoadFrom(dataRow);
                    action.Insert();

                    action.ResetTable("decoration_banner_role");
                    action.Delete("bannercode='"+ dataRow.Get("bannercode", "")  + "'");
                    string[] rolescode = rolecodes.Split(',');
                    foreach (string role in rolescode)
                    {
                        action.Set("bannercode", dataRow.Get("bannercode", ""));
                        action.Set("rolecode", role);
                        action.Insert();
                    }

                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "增加成功";
                }
                catch (Exception ex)
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue =ex.Message;
                }
            }
            return exeMsgInfo;
        }

        public ExeMsgInfo Update(MDataRow dataRow, string rolecodes)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String bannerCode = dataRow["bannercode"].ToString();

            if (String.IsNullOrEmpty(bannerCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String bannertitle = dataRow["bannertitle"].ToString();
            if (String.IsNullOrEmpty(bannertitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "标题不能为空";
                return exeMsgInfo;
            }
            String bannerimg = dataRow["bannerimg"].ToString();
            if (String.IsNullOrEmpty(bannerimg))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "图片不能为空";
                return exeMsgInfo;
            }

            String companycode = dataRow["companycode"].ToString();

            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }
            String createdatetime = dataRow["createdatetime"].ToString();
            if (String.IsNullOrEmpty(createdatetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "创建时间不能为空";
                return exeMsgInfo;
            }


            using (MAction action = new MAction(CurrentTableName))
            {
                try
                {
                    action.BeginTransation();

                    action.Data.LoadFrom(dataRow);
                    action.Update("bannercode='" + dataRow.Get("bannercode", "") + "'");

                    action.ResetTable("decoration_banner_role");
                    action.Delete("bannercode='" + dataRow.Get("bannercode", "") + "'");
                    string[] rolescode = rolecodes.Split(',');
                    foreach (string role in rolescode)
                    {
                        action.Set("bannercode", dataRow.Get("bannercode", ""));
                        action.Set("rolecode", role);
                        action.Insert();
                    }

                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "修改成功";
                }
                catch (Exception ex)
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = ex.Message;
                }
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 根据编码删除一条信息
        /// </summary>
        /// <param name="bannerCode">公司编码</param>
        /// <returns></returns>
        public ExeMsgInfo Delete(string bannerCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(bannerCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            String sWhere = "bannerCode=" + DbService.SetQuotesValue(bannerCode);
            return DbService.Delete(CurrentTableName, sWhere);
        }



        /// <summary>
        /// 获取首页轮换图列表
        /// </summary>
        /// <param name="bannerTitle"></param>
        /// <param name="companyCode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="recordCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public MDataTable GetList(string bannerTitle,string companyCode,  int pageIndex, int pageSize, string orderBy, ref int recordCount, ref int pageCount)
        {
            String sWhere = " 1=1 ";
            String sOrderBy = "";
            string sql = "select  *  from  decoration_banner where companycode='" + companyCode + "'";


            if (!String.IsNullOrEmpty(bannerTitle))
            {
                sWhere += " and bannertitle like '%" + bannerTitle + "%'";
            }
            if (String.IsNullOrEmpty(sOrderBy))
            {
                sOrderBy = " order by createdatetime desc";
            }
            sWhere += sOrderBy;

            return DbService.GetPageTable(sql, pageIndex, pageSize, sWhere, ref recordCount, ref pageCount);
        }

        /// <summary>
        /// 根据企业编号获取轮转图数据
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public MDataTable GetBannerByCompanyCode(string companyCode)
        {
            string sql = "select  *  from  decoration_banner where companycode='" + companyCode + "'";
            return DbService.GetTable(sql, 0);
        }

        /// <summary>
        /// 根据用户编号 获取所有轮播图（按角色）
        /// </summary>
        /// <param name="usercode">用户编号</param>
        /// <param name="num">获取数量</param>
        /// <returns></returns>
        public MDataTable GetBannerByUserCode(string usercode, int num)
        {
            string sql =
                @"select  * from decoration_banner  where bannercode in ( select DISTINCT bannercode from decoration_banner_role where rolecode in (select rolecode from decoration_companyuser_role where usercode ='" +
                usercode + "'))";
            return DbService.GetTable(sql, num, "");
        }


        /// <summary>
        /// 根据编码修改一条企业信息
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Update(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String bannerCode = dataRow["bannercode"].ToString();

            if (String.IsNullOrEmpty(bannerCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String bannertitle = dataRow["bannertitle"].ToString();
            if (String.IsNullOrEmpty(bannertitle))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "标题不能为空";
                return exeMsgInfo;
            }
            String bannerimg = dataRow["bannerimg"].ToString();
            if (String.IsNullOrEmpty(bannerimg))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "图片不能为空";
                return exeMsgInfo;
            }
            
            String companycode = dataRow["companycode"].ToString();

            if (String.IsNullOrEmpty(companycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "企业编号不能为空";
                return exeMsgInfo;
            }

            String sWhere = "bannercode=" + DbService.SetQuotesValue(bannerCode);
            
            String controlFieldNames = "bannertitle,bannerimg,redirecturl,isshow";
            return DbService.Update(CurrentTableName, dataRow, sWhere, controlFieldNames, true);
        }

        /// <summary>
        ///  初始化当前表结构，形成一个空表的带表结构的空DataRow
        /// </summary>
        /// <returns></returns>
        MDataRow IBanner.InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }
    }
}
