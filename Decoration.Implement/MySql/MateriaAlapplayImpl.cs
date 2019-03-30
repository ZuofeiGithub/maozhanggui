using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class MateriaApplayImpl : IMateriaApplay
    {
        #region Implementation of IMateriAlapply
        private const String CurrentTableName = "decoration_materiaapplay";
        private const String VCurrentTableName = "decoration_materiaapplay";
        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：获取所有
        /// </summary>
        /// <returns></returns>
        public MDataTable GetAll(string projectcode)
        {
            string sWhere = "projectcode=" + DbService.SetQuotesValue(projectcode)+ " order by   createdatettime desc";
            return DbService.GetTable(CurrentTableName, 0, sWhere);
        }

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
        /// 申请退货
        /// </summary>
        /// <param name="applycode"></param>
        /// <returns></returns>
        public ExeMsgInfo UpdateIsReturn(string applycode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (string.IsNullOrEmpty(applycode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "申请编号不能为空";
            }
    
            using (MAction action = new MAction(CurrentTableName))
            {
                if (action.Fill("applaycode=" + DbService.SetQuotesValue(applycode)))
                {
                    action.Data.SetState(0);
                    action.Data.Set("isreturned",1,2);
                    action.Data.Set("receivetime", action.Data.Get("receivetime", ""), 2);

                    if (action.Update())
                    {
                        exeMsgInfo.RetStatus = 100;
                        exeMsgInfo.RetValue ="申请成功";

                    } else
                  
                    {
                        exeMsgInfo.RetStatus = 400;
                        exeMsgInfo.RetValue = "申请失败";
                    }
                }
                   
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 创建人：顾健
        /// 时间：2018-10-19
        /// 功能：增加 
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <param name="materialtype">材料类型</param>
        /// <param name="receivetime">期望到货时间</param>
        /// <param name="receiver">接受人</param>
        /// <param name="receiverphone">接受人电话</param>
        /// <param name="applayremark">备注</param>
        /// <returns></returns>
        public ExeMsgInfo Add(string projectcode, string materialtype, string receivetime, string receiver, string receiverphone,
            string applayremark)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (string.IsNullOrEmpty(projectcode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目编号不能为空";
                return exeMsgInfo;
            }
            if (string.IsNullOrEmpty(materialtype))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "项目编号不能为空";
                return exeMsgInfo;
            }
            if (string.IsNullOrEmpty(receivetime))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "期望到货时间不能为空";
                return exeMsgInfo;
            }
            if (string.IsNullOrEmpty(receiver))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "接受人不能为空";
                return exeMsgInfo;
            }
            if (string.IsNullOrEmpty(receiverphone))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "接受人电话不能为空";
                return exeMsgInfo;
            }

            MDataRow dataRow = this.InitDataRow();
            dataRow.Set("applaycode", Guid.NewGuid().ToString("N"));
            dataRow.Set("projectcode", projectcode);
            dataRow.Set("materialtype", materialtype);
            dataRow.Set("receivetime", receivetime);
            dataRow.Set("receiver", receiver);
            dataRow.Set("receiverphone", receiverphone);
            dataRow.Set("applayremark", applayremark);
            dataRow.Set("isreturned", 0);
            dataRow.Set("createdatettime", DateTime.Now);
            
            return DbService.Insert(CurrentTableName, dataRow, "applaycode,projectcode,materialtype,receivetime,receiver,receiverphone,applayremark,isreturned,createdatettime", true);
        }

        #endregion
    }
}