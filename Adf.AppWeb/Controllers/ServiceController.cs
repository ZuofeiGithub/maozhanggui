using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using CYQ.Data.Table;
using Decoration.Service;
using Adf.Core.WebChat;
using Adf.Core.WebChat.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using CYQ.Data.Tool;
using System.Collections;
namespace Adf.AppWeb.Controllers
{
    /// <summary>
    /// 微信公众号小程序api接口
    /// king
    /// 2018-10-19
    /// 
    /// </summary>
    public class ServiceController : ApiController
    {
        #region 登录
        /// <summary>
        /// 登录验证-企业用户
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="value">参数uid userpassword</param>
        /// <returns></returns>
        public ExeMsgInfo CheckLogin([FromBody]dynamic value)
        {
            String uid = value.uid;
            String userpassword = value.userpassword;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().CompanyUser().CheckLoginWithUid(uid, userpassword);
            return exeMsgInfo;
        }

        /// <summary>
        /// 移除企业团队
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="value">参数uid userpassword</param>
        /// <returns></returns>
        public ExeMsgInfo RemoveTeam([FromBody]dynamic value)
        {
            String teamcode = value.teamcode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ProjectTeam().DeleteByTeamCode(teamcode);
            return exeMsgInfo;
        }

        /// <summary>
        /// 根据帐号获得对应的企业用户的信息-企业信息包括企业基础资料
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="uid">帐号</param>
        /// <returns></returns>
        public dynamic GetCompanyUserInfo(String uid)
        {
            MDataRow drCompanyUser = DecorationService.Instance().CompanyUser().GetEntityWithUid(uid);
            return drCompanyUser.ToJson(true);
        }

        /// <summary>
        /// 根据帐号获得对应的企业用户的信息-企业信息包括企业基础资料
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="uid">帐号</param>
        /// <returns></returns>
        public dynamic GetCompanyUserRole(String usercode)
        {
            MDataTable drCompanyUser = DecorationService.Instance().CompanyRole().GetRoleList(usercode);
            return drCompanyUser.ToJson(true);
        }
        #endregion

        #region 轮播图
        /// <summary>
        /// 根据帐号获得对应的企业用户的信息-企业信息包括企业基础资料
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public dynamic GetBanner(String companyCode)
        {
            MDataTable dtBanner = DecorationService.Instance().Banner().GetBannerByCompanyCode(companyCode);
            return dtBanner.ToJson(true);
        }

        /// <summary>
        /// 根据帐号获得对应的企业用户所属的所有角色信息获取企业轮播图信息
        /// 顾健
        /// 2018-11-08
        /// </summary>
        /// <param name="userCode">企业用户编号</param>
        /// <returns></returns>
        public dynamic GetBannerByUserCode(String userCode, int num = 0)
        {
            MDataTable dtBanner = DecorationService.Instance().Banner().GetBannerByUserCode(userCode, num);
            return dtBanner.ToJson(true);
        }



        #endregion

        #region 项目信息

        #region 项目资料
        /// <summary>
        /// 获得项目状态
        /// king
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public dynamic GetProjectCate(string usercode)
        {

            MDataTable dtProjectCate = DecorationService.Instance().FrontModule().GetAllStatusForUserCode(usercode);
            return dtProjectCate.ToJson(true);

        }
        /// <summary>
        /// 获得项目列表-根据状态获得和用户编号
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="userCode">用户编号</param>
        /// <param name="projectstatus">项目状态</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetProject(String companyCode, string userCode, String projectstatus, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            string searchUserCode = userCode;
            //MDataTable userRole = DecorationService.Instance().CompanyUser().GetRoleWithUserCode(userCode);
            //foreach (MDataRow dataRow in userRole.Rows)
            //{
            //    MDataRow role = DecorationService.Instance().CompanyRole()
            //        .GetEntityWithRoleCode(dataRow.Get("rolecode", ""));
            //    if (dataRow.Get("rolecode", "") == "003")
            //    {
            //        searchUserCode = userCode;
            //        break;
            //    }
            //    else
            //    {
            //        searchUserCode = "";
            //    }
            //}

            MDataTable dtProjectInfo = DecorationService.Instance().Project().GetListByUserCode(companyCode, searchUserCode, projectstatus, p, ps, sOrderBy, ref recordCount, ref pageCount);


            Dictionary<string, string> ht = new Dictionary<string, string>();

            List<string> list = new List<string>();
            list.Add("A");
            list.Add("B");
            list.Add("C");
            list.Add("D");
            list.Add("E");
            list.Add("F");
            list.Add("G");
            list.Add("H");
            list.Add("I");
            list.Add("J");
            list.Add("K");
            list.Add("L");
            list.Add("M");
            list.Add("N");
            list.Add("O");
            list.Add("P");
            list.Add("Q");
            list.Add("R");
            list.Add("S");
            list.Add("T");
            list.Add("U");
            list.Add("V");
            list.Add("W");
            list.Add("X");
            list.Add("Y");
            list.Add("Z");


           for(int i=0;i<list.Count;i++)
            {
                MDataTable dt = dtProjectInfo.Select("pinyin_index='" + list[i] + "'");
                List<MDataTable> dtlist = new List<MDataTable>();
                dtlist.Add(dt);
                if (dtlist.Count > 0)
                {
                    ht.Add(list[i], dt.ToJson(true));
                }
            }



            return JsonConvert.SerializeObject(ht);
        }


        /// <summary>
        /// 获取项目统计
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public dynamic GetReport(String companyCode, string userCode)
        {
            MDataTable dt0 = DecorationService.Instance().Project().GetAllListByUserCode(companyCode, userCode, "0");
            MDataTable dt1 = DecorationService.Instance().Project().GetAllListByUserCode(companyCode, userCode, "1");
            MDataTable dt2 = DecorationService.Instance().Project().GetAllListByUserCode(companyCode, userCode, "2");
            MDataTable dt3 = DecorationService.Instance().Project().GetAllListByUserCode(companyCode, userCode, "3");

            Dictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("0", dt0.Rows.Count);
            dic.Add("1", dt1.Rows.Count);
            dic.Add("2", dt2.Rows.Count);
            dic.Add("3", dt3.Rows.Count);
            return JsonConvert.SerializeObject(dic);

        }

        /// <summary>
        /// 获得项目-详情
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        public dynamic GetProjectInfo(String projectCode)
        {
            MDataRow drProjectInfo = DecorationService.Instance().Project().GetEntityByProjectCode(projectCode);
            return drProjectInfo.ToJson(true);
        }

        /// <summary>
        /// 修改一个项目,根据项目编号
        /// king
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ExeMsgInfo UpdateProjectInfo([FromBody]dynamic value)
        {
            String projectCode = value.projectcode;
            String projectname = value.projectname;
            String projectstatus = value.projectstatus;
            String startdate = value.startdate;
            String enddate = value.enddate;
            String projectaddress = value.projectaddress;
            String areaname = value.areaname;
            String areacode = value.areacode;
            String housenumber = value.housenumber;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Project().UpdateByProjectCode(projectCode, projectname, projectstatus, startdate, enddate, projectaddress, areaname, areacode, housenumber);
            return exeMsgInfo;
        }
        public ExeMsgInfo UpdateProject([FromBody]dynamic value)
        {
            String projectCode = value.projectcode;
            String projectname = value.projectname;
            String projectaddress = value.projectaddress;
            String areaname = value.areaname;
            String areacode = value.areacode;
            String housenumber = value.housenumber;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Project().UpdateByProjectCode1(projectCode, projectname, projectaddress, areaname, areacode, housenumber);
            return exeMsgInfo;
        }

        

        /// <summary>
        /// 任务模版
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public dynamic GetTaskTemplate(String companyCode)
        {
            MDataTable dtInfo = DecorationService.Instance().TaskTemplate().GetAll(companyCode);
            return dtInfo.ToJson(true);
        }

        /// <summary>
        /// 添加一个项目
        /// king
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ExeMsgInfo AddProjectInfo([FromBody]dynamic value)
        {
            String projectname = value.projectname;
            String createusercode = value.usercode;
            String projectaddress = value.projectaddress;
            String companycode = value.companycode;
            String areaname = value.areaname;
            String areacode = value.areacode;
            String housenumber = value.housenumber;

            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Project().Add(projectname, projectaddress, createusercode, companycode, areaname, areacode, housenumber);
          
           
            return exeMsgInfo;
        }

        /// <summary>
        /// 导入模版
        /// king
        /// 2018-11-8
        /// </summary>
        /// <returns></returns>
        public ExeMsgInfo ImportTaskTemplate([FromBody]dynamic value)
        {
            String projectCode = value.projectcode;
            String templateCode = value.templatecode;
            String startDate = value.startdate;
            String endDate = value.enddate;

            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Project().ImportTemplate(projectCode, templateCode, startDate, endDate);
            return exeMsgInfo;
        }


        #endregion

        #region 设计档案
        /// <summary>
        /// 获得项目-设计档案
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="doctype">档案类型</param>
        /// <returns></returns>
        public dynamic GetDocInfo(String projectCode, String doctype)
        {
            MDataTable drActivityInfo = DecorationService.Instance().Doc().GetTable(projectCode, doctype);
            return drActivityInfo.ToJson(true);
        }

        /// <summary>
        /// 修改-设计档案
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo UpdateDocInfo([FromBody]dynamic value)
        {
            String doccode = value.doccode;
            String doccontent = value.doccontent;
            String docimages = value.docimages;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Doc().UpdateByDocCode(doccode, doccontent, docimages);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除 设计档案
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ExeMsgInfo RemoveDocInfo([FromBody] dynamic value)
        {
            string doccode = value.doccode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Doc().DeleteByDocCode(doccode);
            return exeMsgInfo;
        }

        /// <summary>
        /// 新增-设计档案
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public dynamic AddDocInfo([FromBody]dynamic value)
        {
            String doctype = value.doctype;
            String projectCode = value.projectcode;
            String doccontent = value.doccontent;
            String docimages = value.docimages;
            String createusercode = value.createusercode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Doc().Add(doctype, projectCode, doccontent, docimages, createusercode);
            return exeMsgInfo;
        }
        #endregion

        #region 任务分类

        /// <summary>
        /// 获得项目-施工任务分类
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        public dynamic GetTaskCate(String projectCode)
        {
            MDataTable dtTaskCate = DecorationService.Instance().TaskCate().GetAll(projectCode);
            return dtTaskCate.ToJson(true);
        }

        /// <summary>
        /// 获得项目-分类施工任务
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="cateCode">任务分类编号</param>
        /// <returns></returns>
        public dynamic GetTask(String projectCode, String cateCode)
        {
            MDataTable dtTask = DecorationService.Instance().Task().GetAll(projectCode, cateCode);
            return dtTask.ToJson(true);
        }

        /// <summary>
        /// 获得项目-所有施工任务
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        public dynamic GetAllTask(String projectCode)
        {
            MDataTable dtTask = DecorationService.Instance().Task().GetAll(projectCode);
            return dtTask.ToJson(true);
        }

        /// <summary>
        /// 获得项目-施工任务明细
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="taskCode">项目编号</param>
        /// <returns></returns>
        public dynamic GetTaskInfo(String taskCode)
        {
            MDataRow drTaskInfo = DecorationService.Instance().Task().GetEntityByTaskcode(taskCode);
            return drTaskInfo.ToJson(true);
        }

        /// <summary>
        /// 获得项目-施工任务-执行记录
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="taskCode">任务编号</param>
        /// <returns></returns>
        public dynamic GetTaskExecute(String taskCode, string usercode, string taskstatus)
        {
            MDataTable drTaskExecute = DecorationService.Instance().Execute().GetAll(taskCode, usercode, taskstatus);
            return drTaskExecute.ToJson(true);
        }




        /// <summary>
        /// 获得项目-施工任务-设置任务完成
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo CompleteTask([FromBody]dynamic value)
        {
            String taskcode = value.taskcode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Task().Complete(taskcode);
            return exeMsgInfo;
        }
        /// <summary>
        /// 项目-施工任务-添加执行任务
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo AddTaskExecuteInfo([FromBody]dynamic value)
        {
            String taskcode = value.taskcode;
            String usercode = value.usercode;
            String executecontent = value.executecontent;
            String executeimgs = value.executeimgs;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Execute().Add(taskcode, usercode, executecontent, executeimgs);
            return exeMsgInfo;
        }

        #endregion

        #region 工程材料

        /// <summary>
        /// 获得项目-工程材料
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <returns></returns>
        public dynamic GetMateriaApplay(String projectCode)
        {
            MDataTable dtMateriaApplay = DecorationService.Instance().MateriaApplay().GetAll(projectCode);
            return dtMateriaApplay.ToJson(true);
        }

        /// <summary>
        /// 获得项目-工程材料分类
        /// king
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public dynamic GetMateriaApplayCate()
        {
            MDataTable dtMateriaApplay = FrameWorkService.Instance().DataDic().GetAll("materialtype");
            return dtMateriaApplay.ToJson(true);
        }

        /// <summary>
        /// 获得项目-工程材料添加
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public dynamic AddMateriaApplay([FromBody]dynamic value)
        {
            String projectcode = value.projectcode;
            String materialtype = value.materialtype;
            String receivetime = value.receivetime;
            String receiver = value.receiver;
            String receiverphone = value.receiverphone;
            String applayremark = value.applayremark;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().MateriaApplay().Add(projectcode, materialtype, receivetime, receiver, receiverphone, applayremark);
            return exeMsgInfo;
        }

        /// <summary>
        /// 申请退货-工程材料退货
        /// 顾健
        /// 2018-11-08
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ExeMsgInfo IsReturned([FromBody]dynamic value)
        {
            String applycode = value.applycode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().MateriaApplay().UpdateIsReturn(applycode);
            return exeMsgInfo;
        }

        #endregion

        #region 项目成员



        /// <summary>
        /// 获得项目成员-服务团队
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetProjectTeamUser(String projectCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtProjectTeamUserInfo = DecorationService.Instance().ProjectTeam().GetList(projectCode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtProjectTeamUserInfo.ToJson(true);
        }

        /// <summary>
        /// 获得企业用户
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetCompanyUser(String companyCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtCompanyUserInfo = DecorationService.Instance().CompanyUser().GetListWithCompanyCode(companyCode, "", "", p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtCompanyUserInfo.ToJson(true);
        }
        /// <summary>
        /// 获得企业角色
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetCompanyRole(String companyCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtCompanyRoleInfo = DecorationService.Instance().CompanyRole().GetList("", companyCode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtCompanyRoleInfo.ToJson(true);
        }

        /// <summary>
        /// 添加企业用户
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="value">参数</param>
        public ExeMsgInfo AddCompanyUser([FromBody]dynamic value)
        {
            String companycode = value.companycode;
            String username = value.username;
            String cellphone = value.cellphone;
            String remark = value.remark;
            String projectcode = value.projectcode;
            String userrole = value.userrole;
            String usercode = value.usercode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().CompanyUser().AddCompanyUser(usercode, companycode, username, cellphone, remark, userrole, projectcode);
            return exeMsgInfo;
        }

        #endregion

        #region 项目模版

        #endregion

        #region 家装商城
        /// <summary>
        /// 获得商城分类
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public dynamic GetProductCate(String companyCode,string catecode)
        {
            MDataTable dtProductCate = DecorationService.Instance().ProductCate().GetAll(companyCode, catecode);
            return dtProductCate.ToJson(true);
        }

        /// <summary>
        /// 获得商城分类-商品信息
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="cateCode">企业编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetProduct(String companyCode, String cateCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtProductInfo = DecorationService.Instance().Product().GetListByCompanyCode(companyCode, cateCode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtProductInfo.ToJson(true);
        }

        /// <summary>
        /// 产品详情
        /// 顾健
        /// </summary>
        /// <param name="productcode">产品编码</param>
        /// <returns></returns>
        public dynamic GetProductDetail(string productcode)
        {
            MDataRow detailRow = DecorationService.Instance().Product().GetEntityWithProductCode(productcode);
            return detailRow.ToJson(true);
        }

        #endregion

        #region 精选案例

        /// <summary>
        /// 获得精选案例列表
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="userCode">用户编码</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetCase(String projectcode, String userCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtCompanyRoleInfo = DecorationService.Instance().Case().GetList(userCode, projectcode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtCompanyRoleInfo.ToJson(true);
        }



        /// <summary>
        /// 获取未选择的数据
        /// </summary>
        /// <param name="projectcode"></param>
        /// <param name="companycode"></param>
        /// <returns></returns>
        public dynamic GetSelectCase(String projectcode, String companycode)
        {

            MDataTable dtInfo = DecorationService.Instance().Project().GetAll(projectcode, companycode);
            MDataTable Select = DecorationService.Instance().Case().GetAll(projectcode);
            List<string> projectcodes = new List<string>();
            foreach (MDataRow dataRow in Select.Rows)
            {
                projectcodes.Add("'" + dataRow.Get("caseprojectcode", "") + "'");

            }
            MDataTable dt = dtInfo.Select(" projectcode not in('" + string.Join(",", projectcodes) + "')");
            return dt.ToJson(true);
        }

        /// <summary>
        /// 添加精选案例
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo AddCase([FromBody] dynamic value)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String projectcode = value.projectcode;
            String createusercode = value.usercode;
            String caseprojectcode = value.caseprojectcode;
            string rolecodes = value.rolecodes;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(Guid.NewGuid().ToString("N"))), 4, 8);
            t2 = t2.Replace("-", "");

            string casecode = t2;
            exeMsgInfo = DecorationService.Instance().Case()
                .Add(casecode, projectcode, createusercode, caseprojectcode);
            if (exeMsgInfo.RetStatus == 100)
            {
                string rolecode = rolecodes;
                if (string.IsNullOrEmpty(rolecode))
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "请至少选择一个角色";
                }
                else
                {
                    List<MDataRow> roleList = new List<MDataRow>();
                    string[] roles = rolecode.Split('|');
                    foreach (string role in roles)
                    {
                        MDataRow roleRow = DecorationService.Instance().CaseRole().InitDataRow();
                        roleRow.Set("casecode", casecode);
                        roleRow.Set("rolecode", role);
                        roleList.Add(roleRow);
                    }
                    exeMsgInfo = DecorationService.Instance().CaseRole().Add(roleList, casecode);
                }
            }
            return exeMsgInfo;
        }

        #endregion

        #endregion

        #region 我的
        /// <summary>
        /// 获得活动推送-列表
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetActivity(String companyCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtActivityInfo = DecorationService.Instance().Activity().GetListWithCompanyCode(companyCode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtActivityInfo.ToJson(true);
        }
        /// <summary>
        /// 获得活动推送-详情
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="activityCode">活动编号</param>
        /// <returns></returns>
        public dynamic GetActivityEntity(String activityCode)
        {
            MDataRow drActivityInfo = DecorationService.Instance().Activity().GetEntityWithActivityCode(activityCode);
            return drActivityInfo.ToJson(true);
        }


        /// <summary>
        /// 关于
        /// king
        /// 2018-10-22
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <returns></returns>
        public dynamic GetCompany(String companyCode)
        {
            MDataRow drCompanyInfo = DecorationService.Instance().Company().GetEntityWithCompanyCode(companyCode);
            return drCompanyInfo.ToJson(true);
        }


        /// <summary>
        /// 项目消息提醒-添加
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="value">参数</param>
        public ExeMsgInfo AddTip([FromBody]dynamic value)
        {
            String rolecode = value.rolecode;
            String tiptitle = value.tiptitle;
            String tipcontent = value.tipcontent;
            String createusercode = value.createusercode;
            String projectcode = value.projectcode;
            String taskcode = value.taskcode;


            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Tip().Add(rolecode, tiptitle, tipcontent, createusercode, projectcode);
            return exeMsgInfo;
        }
        /// <summary>
        ///消息提醒-添加
        ///  顾健
        /// 2018-11-08
        /// </summary>
        /// <param name="value">参数</param>
        public ExeMsgInfo AddNewTip([FromBody]dynamic value)
        {
            String projectcatecode = value.projectcatecode;
            String tipcontent = value.tipcontent;
            String reminddatetime = value.reminddatetime;
            String projectcode = value.projectcode;
            String taskcode = value.taskcode;
            String tipimg = value.tipimg;
            String createusercode = value.createusercode;
            string rolecode = value.rolecode;
            MDataRow dataRow = DecorationService.Instance().Tip().InitDataRow();

            dataRow.Set("projectcatecode", projectcatecode);
            dataRow.Set("tipcontent", tipcontent);
            dataRow.Set("reminddatetime", reminddatetime);
            dataRow.Set("projectcode", projectcode);
            dataRow.Set("taskcode", taskcode);
            dataRow.Set("tipimg", tipimg);
            dataRow.Set("createusercode", createusercode);

            List<string> rolecodes = new List<string>();
            if (!string.IsNullOrEmpty(rolecode))
            {
                string[] s = rolecode.Split(',');
                foreach (string str in s)
                {
                    rolecodes.Add(str);
                }
            }

            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Tip().Add(dataRow, rolecodes);
            return exeMsgInfo;
        }

        /// <summary>
        /// 项目消息提醒
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetTip(String userCode,string projectcode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtTipInfo = DecorationService.Instance().Tip().GetList(userCode, projectcode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtTipInfo.ToJson(true);
        }

        /// <summary>
        /// 获取提醒总数
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public ExeMsgInfo TipCount([FromBody]dynamic value)
        {
            ExeMsgInfo exemsginfo = new ExeMsgInfo();
            string usercode = value.usercode;
            string projectcode = value.projectcode;
            MDataTable dtTipInfo = DecorationService.Instance().Tip().GetAll(usercode, projectcode);
            exemsginfo.RetStatus = 100;
            exemsginfo.RetValue = dtTipInfo.Rows.Count().ToString();
            return exemsginfo;
        }

        /// <summary>
        /// 系统公告
        /// king
        /// 2018-10-22
        /// </summary>
        /// <param name="companyCode">企业编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetSysMsg(String companyCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtSysMsgInfo = DecorationService.Instance().SysMsg().GetListWithCompanyCode(companyCode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtSysMsgInfo.ToJson(true);
        }

        /// <summary>
        /// 修改头像
        /// 顾健
        /// 2018-11-8
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo UpDateHeadImgByUserCode([FromBody]dynamic value)
        {
            String usercode = value.usercode;
            String headimg = value.headimg;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().CompanyUser().UpDateHeadImgByUserCode(usercode, headimg);
            return exeMsgInfo;
        }



        #endregion

        #region 签到

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo SignIn([FromBody]dynamic value)
        {
            String usercode = value.usercode;
            String gps = value.gps;
            String signinaddress = value.signinaddress;
            String projectcode = value.projectcode;


            ExeMsgInfo exeMsgInfo = DecorationService.Instance().SignIn().SigniInAdd(usercode, gps, signinaddress, projectcode);
            return exeMsgInfo;
        }

        /// <summary>
        /// 获得签到记录
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="usercode">用户编号</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetSignIn(String usercode, String projectCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtSignInInfo = DecorationService.Instance().SignIn().GetListWithUserCode(usercode, projectCode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtSignInInfo.ToJson(true);
        }


        #region 微信二维码专用
        /// <summary>
        /// 设置code
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo SetCode([FromBody]dynamic value)
        {
            String taskcode = value.taskcode;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(taskcode)), 4, 8);
            t2 = t2.Replace("-", "");


            t2 = t2.ToLower();

            MDataRow dr = new MDataRow(DBTool.GetColumns("WxACode"));
            dr.Set("oldcode", taskcode);
            dr.Set("newcode", t2);

            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Task().SetCode(dr);
            if (exeMsgInfo.RetStatus != 400)
            {
                exeMsgInfo.RetValue = t2;
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 获取 任务编码 以MD5加密Code获取
        /// </summary>
        /// <param name="usercode"></param>
        /// <param name="projectCode"></param>
        /// <param name="p"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public dynamic GetCode(String newcode)
        {
            MDataRow dtSignInInfo = DecorationService.Instance().Task().GetCode(newcode);
            return dtSignInInfo.ToJson(true);
        }
        #endregion



        /// <summary>
        /// 获得项目下所有签到记录
        /// 侯鑫辉
        /// 2018-11-13
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetSignInWithProjectCode(String projectCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtSignInInfo = DecorationService.Instance().SignIn().GetListWithProjectCode(projectCode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtSignInInfo.ToJson(true);
        }
        #endregion

        #region 上传图片
        [Route("api/service/uploadimage")]
        public ExeMsgInfo PostFile()
        {

            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                String docFiles = "";
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    String fileName = postedFile.FileName;
                    var fileExtension = fileName.Substring(fileName.LastIndexOf("."));//后缀
                    String newFileName = Guid.NewGuid().ToString("N") + fileExtension;

                    var filePath = HttpContext.Current.Server.MapPath("~/upload/" + newFileName);
                    postedFile.SaveAs(filePath);

                    docFiles += "/upload/" + newFileName + ",";
                }
                if (!string.IsNullOrEmpty(docFiles))
                {
                    docFiles = docFiles.Substring(0, docFiles.Length - 1);
                }
                exeMsgInfo.RetStatus = 100;
                exeMsgInfo.RetValue = docFiles;

            }
            else
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "未检测到文件";
            }
            return exeMsgInfo;
        }

        #endregion

        #region 报备客户
        /// <summary>
        /// 新增报备客户
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo AddPotential([FromBody]dynamic value)
        {
            MDataRow dataRow = DecorationService.Instance().Potential().InitDataRow();
            dataRow.Set("companycode", value.companycode);
            dataRow.Set("potentialusercode", value.potentialusercode);
            dataRow.Set("potentialname", value.potentialname);
            dataRow.Set("potentialphone", value.potentialphone);
            dataRow.Set("potentialaddress", value.potentialaddress);
            dataRow.Set("potentialstyle", value.potentialstyle);
            dataRow.Set("potentialsln", value.potentialsln);
            dataRow.Set("potentialarea", value.potentialarea);
            dataRow.Set("potentialbudget", value.potentialbudget);
            dataRow.Set("potentialremark", value.potentialremark);
            dataRow.Set("createusercode", value.createusercode);
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Potential().Add(dataRow);
            return exeMsgInfo;
        }


        #endregion

        #region 分享留言

        /// <summary>
        /// 增加留言
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ExeMsgInfo AddTaskMessage([FromBody] dynamic value)
        {
            string taskcode = value.taskcode;
            string content = value.content;
            string cellphone = value.cellphone;
            string membername = value.membername;
            string address = value.address;

            MDataRow task = DecorationService.Instance().Task().GetEntityByTaskcode(taskcode);
            MDataRow project = DecorationService.Instance().Project().GetEntityByProjectCode(task.Get("projectcode", ""));

            
            MDataRow dataRow = DecorationService.Instance().TaskMessage().InitDataRow();
            dataRow.Set("cellphone", cellphone);
            dataRow.Set("membername", membername);
            dataRow.Set("address", address);
            dataRow.Set("taskcode", taskcode);
            dataRow.Set("content", content);
            dataRow.Set("companycode", project.Get("companycode", ""));
            dataRow.Set("createdate", DateTime.Now);
            dataRow.Set("messsagecode", Guid.NewGuid().ToString());
            return DecorationService.Instance().TaskMessage().Add(dataRow);
        }

        /// <summary>
        /// 获取分享留言列表
        /// </summary>
        /// <param name="companycode"></param>
        /// <param name="taskcode"></param>
        /// <returns></returns>
        public dynamic GetTaskMessage(string companycode, string taskcode)
        {
            MDataTable dt = DecorationService.Instance().TaskMessage().GetAll(companycode, taskcode);
            return dt.ToJson();
        }

        #endregion

        #region 收藏
        /// <summary>
        /// 添加收藏
        /// king
        /// 2018-11-7
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo AddProductCollection([FromBody]dynamic value)
        {
            String createusercode = value.createusercode;
            String productcode = value.productcode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ProductCollection().Add(productcode, createusercode);
            return exeMsgInfo;
        }


        /// <summary>
        /// 获得签到记录
        /// king
        /// 2018-11-7
        /// </summary>
        /// <param name="usercode">用户编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetProductCollection(String usercode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtInfo = DecorationService.Instance().ProductCollection().GetList(usercode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtInfo.ToJson(true);
        }

        /// <summary>
        /// 删除收藏
        /// king
        /// 2018-11-7
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo RemoveProductCollection([FromBody]dynamic value)
        {
            String collectioncode = value.collectioncode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ProductCollection().Delete(collectioncode);
            return exeMsgInfo;
        }
        #endregion

        #region 购物车
        /// <summary>
        /// 添加到购物车
        /// king
        /// 2018-11-7
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo AddShopCart([FromBody]dynamic value)
        {
            String createusercode = value.createusercode;
            String productcode = value.productcode;
            string productnum = value.productnum;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ShopCart().Add(productcode, ConvertHelper.ObjectToT(productnum, 0), createusercode);
            return exeMsgInfo;
        }

        /// <summary>
        /// 获得购物车列表
        /// king
        /// 2018-11-7
        /// </summary>
        /// <param name="usercode">用户编号</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetShopCart(String usercode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtInfo = DecorationService.Instance().ShopCart().GetList(usercode, p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtInfo.ToJson(true);
        }

        /// <summary>
        /// 删除购物车
        /// king
        /// 2018-11-7
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo RemoveShopCart([FromBody]dynamic value)
        {
            String shopCartCode = value.shopcartcode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ShopCart().Delete(shopCartCode);
            return exeMsgInfo;
        }
        #endregion

        #region 我的收入

        /// <summary>
        /// 我的收入
        /// king
        /// 2018-10-20
        /// </summary>
        /// <param name="companyCode">公司编号</param>
        /// <param name="userCode">用户编码</param>
        /// <param name="p">页码</param>
        /// <param name="ps">条数</param>
        /// <returns></returns>
        public dynamic GetPay(String companyCode, String userCode, int p, int ps)
        {
            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtInfo = DecorationService.Instance().CompanyPay().GetListByUser(companyCode, userCode, "", "", p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtInfo.ToJson(true);
        }

        #endregion

        #region 处罚

        /// <summary>
        /// 添加到购物车
        /// king
        /// 2018-11-7
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public ExeMsgInfo AddPenalize([FromBody]dynamic value)
        {

            String companycode = value.companycode;
            string penalizemoney = value.penalizemoney;
            string penalizeimg = value.penalizeimg;
            string createusercode = value.createusercode;
            string penalizeremark = value.penalizeremark;
            string taskcode = value.taskcode;
            MDataRow dataRow = DecorationService.Instance().DecoraTionPenalize().InitDataRow();
            dataRow.Set("companycode", companycode);
            dataRow.Set("penalizemoney", penalizemoney);
            dataRow.Set("penalizeimg", penalizeimg);
            dataRow.Set("createusercode", createusercode);
            dataRow.Set("taskcode", taskcode);
            dataRow.Set("penalizeremark", penalizeremark);
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().DecoraTionPenalize().Add(dataRow);
            return exeMsgInfo;
        }

        #endregion

        #region 模块

        /// <summary>
        /// 获得活动推送-列表
        /// king
        /// 2018-10-19
        /// </summary>
        /// <param name="usercode">企业编号</param>

        /// <returns></returns>
        public dynamic GetModule(String usercode)
        {
            MDataTable dtActivityInfo = DecorationService.Instance().FrontModule().GetAllModuleForUserCode(usercode);
            return dtActivityInfo.ToJson(true);
        }

        /// <summary>
        /// 获取可提醒角色
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public dynamic GetTipRole(String usercode)
        {
            MDataTable dtActivityInfo = DecorationService.Instance().Tip().GetAllTipRole(usercode);
            return dtActivityInfo.ToJson(true);
        }


        /// <summary>
        /// 申请延期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ExeMsgInfo AddDelay([FromBody] dynamic value)
        {

            string projectcode = value.projectcode;
            string delayreason = value.delayreason;
            string delaydays = value.delaydays;
            string submitusercode = value.submitusercode;
            MDataRow dataRow = DecorationService.Instance().ApplyDelay().InitDataRow();
            dataRow.Set("projectcode", projectcode);
            dataRow.Set("delayreason", delayreason);
            dataRow.Set("delaydays", delaydays);
            dataRow.Set("submitusercode", submitusercode);
            dataRow.Set("delaycode", Guid.NewGuid().ToString("N"));
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ApplyDelay().Add(dataRow);
            return exeMsgInfo;
        }
        /// <summary>
        /// 工程经理审核
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ExeMsgInfo CheckDelay([FromBody] dynamic value)
        {

            string delaycode = value.delaycode;
            string checkusercode1 = value.checkusercode1;
            string ischeck1 = value.ischeck1;
            MDataRow dataRow = DecorationService.Instance().ApplyDelay().GetEntity(delaycode);
            dataRow.Set("checkusercode1", checkusercode1);
            dataRow.Set("ischeck1", ischeck1);
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ApplyDelay().Check(delaycode, checkusercode1, ischeck1);
            return exeMsgInfo;
        }

        /// <summary>
        /// 业主审核
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ExeMsgInfo CheckDelayForCustomer([FromBody] dynamic value)
        {

            string delaycode = value.delaycode;
            string checkusercode2 = value.checkusercode2;
            string ischeck2 = value.ischeck2;
            MDataRow dataRow = DecorationService.Instance().ApplyDelay().GetEntity(delaycode);
            dataRow.Set("checkusercode2", checkusercode2);
            dataRow.Set("ischeck2", ischeck2);
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().ApplyDelay().CheckByCustomer(delaycode, checkusercode2, ischeck2);
            return exeMsgInfo;
        }


        /// <summary>
        /// 待审核列表
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="delaystatus"></param>
        /// <param name="p"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public dynamic GetDelayList(String projectCode, string delaystatus, int p, int ps)
        {

            int pageCount = 1;
            int recordCount = 0;
            String sOrderBy = "";
            MDataTable dtSysMsgInfo = DecorationService.Instance().ApplyDelay().GetList(projectCode, delaystatus, "", "", p, ps, sOrderBy, ref recordCount, ref pageCount);
            return dtSysMsgInfo.ToJson(true);
        }




        #region 微信相关


        public ExeMsgInfo WxACode([FromBody] dynamic value)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                string scene = value.data;
                OAuthController OAuth = new OAuthController();
                AccessToken entityAccess = AccessTokenHelper.GetAccessTokenEntity("wx030fcc24a9fccd0d", "a716c8f08e66a33b857b3c17d0996209");
                //string token = OAuth.GetAccessToken();
                string sUrl = "https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token=" + entityAccess.access_token;



                Stream orderList = OAuthController.PostJson(sUrl, scene);

                byte[] tt = StreamToBytes(orderList);
                //将流保存在c盘test.png文件下

                string imgpath = Guid.NewGuid().ToString("N") + ".jpg";
                string fullPath = System.Web.HttpContext.Current.Server.MapPath("/Upload/image/");

                System.IO.File.WriteAllBytes(fullPath + imgpath, tt);
                exeMsgInfo.RetStatus = 100;
                exeMsgInfo.RetValue = imgpath;
                return exeMsgInfo;
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
                return exeMsgInfo;
            }
        }
        public static byte[] StreamToBytes(Stream stream)
        {
            List<byte> bytes = new List<byte>();
            int temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte)temp);
                temp = stream.ReadByte();
            }
            return bytes.ToArray();
        }


        ///编码
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }
        ///解码
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }


        #endregion

        #endregion

        #region 项目任务记录
        /// <summary>
        /// 修改一个任务记录
        /// 侯鑫辉
        /// 2018-11-13
        /// </summary>
        /// <returns></returns>
        public ExeMsgInfo UpdateExecute([FromBody]dynamic value)
        {
            String executecode = value.executecode;
            String executecontent = value.executecontent;
            String executeimgs = value.executeimgs;

            MDataRow dtRow = DecorationService.Instance().Execute().InitDataRow();
            dtRow.Set("executecode", executecode);
            dtRow.Set("executecontent", executecontent);
            dtRow.Set("executeimgs", executeimgs);

            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Execute().UpdateByExecuteCode(dtRow);
            return exeMsgInfo;
        }

        /// <summary>
        /// 删除任务记录
        /// 侯鑫辉
        /// 2018-11-13
        /// </summary>
        /// <param name="value">参数taskcode</param>
        /// <returns></returns>
        public ExeMsgInfo RemoveByExecuteCode([FromBody]dynamic value)
        {
            String executecode = value.executecode;
            ExeMsgInfo exeMsgInfo = DecorationService.Instance().Execute().DeleteByExecuteCode(executecode);
            return exeMsgInfo;
        }
        #endregion

        #region 系统配置
        /// <summary>
        /// 佣金规则、留言(佣金规则传的parentCode：yongjin，留言图片：liuyanpic，留言文字：liuyantext)
        /// 侯鑫辉
        /// 2018-11-20
        /// </summary>
        /// <param name="companyCode">公司编号</param>
        /// <param name="parentCode">数据类型</param>
        /// <returns></returns>
        public dynamic GetSystemConfiguration(String companyCode, string parentCode)
        {
            MDataTable dtInfo = DecorationService.Instance().SystemConfiguration().GetWithCompanyCodeAndParentCode(companyCode, parentCode);
            return dtInfo.ToJson(true);
        }


        /// <summary>
        /// 佣金规则、留言(佣金规则传的parentCode：yongjin，留言图片：liuyanpic，留言文字：liuyantext)
        /// 顾健
        /// </summary>
        /// <param name="taskcode">任务编号</param>
        /// <param name="parentcode">数据类型</param>
        /// <returns></returns>
        public dynamic GetSytemConfigByTaskCode(string taskcode,string parentcode)
        {
            MDataRow taskInfo = DecorationService.Instance().Task().GetEntityByTaskcode(taskcode);
            MDataRow projectcode = DecorationService.Instance().Project()
                .GetEntityByProjectCode(taskInfo.Get("projectcode", ""));


            MDataTable dtInfo = DecorationService.Instance().SystemConfiguration().GetWithCompanyCodeAndParentCode(projectcode.Get("companyCode", ""), parentcode);
            return dtInfo.ToJson(true);
        }
        #endregion
    }
}
