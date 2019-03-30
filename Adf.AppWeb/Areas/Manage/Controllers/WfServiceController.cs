using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Db;
using Adf.Core.DB.Entity;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using Adf.FrameWork.Service;
using Adf.FrameWork.Service.Base;
using CYQ.Data;
using CYQ.Data.Table;

namespace Adf.AppWeb.Areas.Manage.Controllers
{
    /// <summary>
    /// 流程服务管理
    /// </summary>
    public class WfServiceController : BaseController
    {
        /// <summary>
        /// 得到一个表单审批历史
        /// </summary>
        /// <param name="wfCode">工作流编码</param>
        /// <param name="bokeycode">业务表单数据</param>
        /// <returns></returns>
        public ActionResult GetWfHistory(String wfCode, String bokeycode)
        {
            List<WfInstance> lstInfo = FrameWorkService.Instance().WfInstance().QueryAllByWfCode(wfCode, bokeycode);
            ViewBag.WfInstanceList = lstInfo;

            //视图
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/wfmng/wfservice/wfservice.history.cshtml");
            return View(viewName);

        }

        /// <summary>
        /// 得到下步步骤信息,动态获取.
        /// 
        /// 目前处理的，当前节点只有一个步骤
        /// 
        /// {有可能 当前节点有多个步骤，让用户选择一个步骤进行执行。 反应到前端界面上可能是多个步骤列表，让用户选择一个步骤 从而确定下一个节点以及工作人}
        /// 
        /// </summary>
        /// <param name="curNodeCode"></param>
        /// <returns></returns>
        public ActionResult GetNext(String currentnodecode, String wfcode)
        {
            
            //得到当前节点
            WfStep wfNextStepEntity = null;
            WfNode curWfNode = FrameWorkService.Instance().WfNode().GetEntityByWfNodeCode(wfcode, currentnodecode);

            /**
             1、根据当前节点来确定下一个步骤
             */
            if (curWfNode != null)
            {
                //得到下一步的步骤
                if (curWfNode.SelectNextNodeTypeCode == "01")
                {
                    //通过指定步骤来进行
                    List<WfStep> lstNextStep = FrameWorkService.Instance().WfStep().GetNexStepList(currentnodecode, wfcode);
                    if (lstNextStep.Count == 0)
                    {
                        //当前节点后面没有节点

                    } else if (lstNextStep.Count == 1)
                    {
                        //当前节点后面只有一个节点
                        wfNextStepEntity = lstNextStep[0];

                    } else if (lstNextStep.Count > 1)
                    {
                        //当前节点下面有多个节点,这里应该当用户来选择哪一个流程或者根据当前用户表单输入的相关值,来从多个下一步中挑选一个步骤
                    }
                }
                else if (curWfNode.SelectNextNodeTypeCode == "02")
                {
                    //使用存储过程来计算下一个步骤
                    //需要进行业务逻辑进行判断,目前版本采用存储过程作为逻辑解析器进行
                    String procName = curWfNode.ProcName;
                    List<DataParam> lstParams = DbService.GetProcParams(procName);

                    if (lstParams != null && lstParams.Count > 0)
                    {
                        foreach (DataParam dataParam in lstParams)
                        {
                            if (dataParam.Direction == 0)
                            {
                                dataParam.Value = Server.UrlDecode(RequestHelper.GetQueryString(dataParam.Name.Replace("@", "")));
                            }
                        }
                    }

                }

                /**
                 2、步骤确定后，根据步骤来确定人
                 */

                String receiveusercode = "";
                //MDataRow drNextDealUserEntity = FrameWorkService.Instance().User().InitDataRow();
                //如果不是最后一个节点
                if (wfNextStepEntity != null)
                {
                    String receivenodecode = wfNextStepEntity.ToNodeCode;
                    MDataRow drReceiveNodeEntity = FrameWorkService.Instance()
                        .WfNode()
                        .GetDataEntityByWfNodeCode(wfcode, receivenodecode);

                    if (receivenodecode != "end")
                    {
                        //不是最后个结点的处理方法
                        String curDeptCode = GlobalUserLogin.Get("deptcode", "");
                        String curOrgCode = GlobalUserLogin.Get("orgcode", "");
                        MDataRow drDept = FrameWorkService.Instance().Org().GetEntityByOrgCode(curDeptCode);
                        MDataRow drOrg = FrameWorkService.Instance().Org().GetEntityByOrgCode(curOrgCode);



                        String dealusertypecode = drReceiveNodeEntity.Get("dealusertypecode", "");


                        if (dealusertypecode == "01")
                        {
                            //当前用户部门主管
                            String chargeUserCode = drDept.Get("chargerusercode", "");
                            receiveusercode = chargeUserCode;
                        }
                        else if (dealusertypecode == "02")
                        {
                            //当前用户所在机构主管
                            String leaderuercode = drOrg.Get("leaderuercode", "");
                            receiveusercode = leaderuercode;
                        }
                        else if (dealusertypecode == "03")
                        {
                            //指定部门主管负责人
                            MDataRow tOrg = FrameWorkService.Instance().Org().GetEntityByOrgCode(wfNextStepEntity.ToOrgCode);
                            if (tOrg != null)
                            {
                                receiveusercode = tOrg.Get("chargerusercode", "");
                            }
                        }
                        else if (dealusertypecode == "03")
                        {

                        }
                        else if (dealusertypecode == "05")
                        {
                            //指定人
                            receiveusercode = wfNextStepEntity.ToUserCode;
                        }
                        else if (dealusertypecode == "06")
                        {
                            //当前部门分管领导
                            String leaderUsercode = drDept.Get("leaderusercode", "");
                            receiveusercode = leaderUsercode;
                        }

                        //下一处理人
                        MDataRow curDrReceiveUserEntity = FrameWorkService.Instance().User().GetEntity(receiveusercode);
                        ViewBag.DrReceiveUserEntity = curDrReceiveUserEntity;
                    }
                    

                   

                    //下一节点
                    ViewBag.DrReceiveNodeEntity = drReceiveNodeEntity;
                }
                


            }
           

            //视图
            String viewName = FrameWorkService.GetModuleViewName(GlobalAdminViewPath, "system/wfmng/wfservice/wfservice.next.cshtml");
            return PartialView(viewName);

        }



    }
}
