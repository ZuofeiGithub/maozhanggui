using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;

namespace Decoration.Implement.MySql
{
    public static class VerificationHelper
    {

        /// <summary>
        /// 创建人: 秦白驹
        /// 时间2018-10-10
        /// 功能:检查字符串字节长度是否超出限制
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="len">最大长度</param>
        /// <returns></returns>
        public static bool CheckTextBytesLength(string text, int len)
        {
            if (System.Text.Encoding.Default.GetBytes(text).Length > len)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 创建人: 秦白驹
        /// 时间2018-10-10
        /// 功能:数值文本转换
        /// </summary>
        /// <param name="val">以|分割</param>
        /// <param name="text">以|分割</param>
        /// <param name="thisval">当前数值</param>
        /// <returns></returns>
        public static string GetDicName(string val, string text, string thisval)
        {
            string thistext = "";
            String[] arryVal = val.Split('|');
            String[] arryText = text.Split('|');

            for (int i = 0; i < arryVal.Length; i++)
            {
                if (thisval == arryVal[i])
                {
                    thistext = arryText[i];
                    break;
                }
            }

            return thistext;
        }

        /// <summary>
        /// 是否整数 且大于等于0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsPositiveIntege(this string obj)
        {
            Int64 temp = 0;
            return Int64.TryParse(obj, out temp) && (temp >= 0);
        }

        /// <summary>
        /// 转换为Int
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static int ToInt(this string obj, int defVal = 0)
        {
            int temp;
            return int.TryParse(obj, out temp) ? temp : defVal;
        }


        /// <summary>
        /// 验证是否数字
        /// </summary>
        /// <param name="number">数字内容</param>
        /// <returns>true 验证成功 false 验证失败</returns>
        public static bool CheckNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }
            Regex regex = new Regex(@"^(-)?\d+(\.\d+)?$");
            if (regex.IsMatch(number))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证字符是否为空，并验证字符长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strlength">默认不验证字符长度</param>
        /// <returns></returns>
        public static bool CheckStr(string str, int strlength = 0)
        {
            if (strlength == 0)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(str) || !CheckTextBytesLength(str,strlength))
                {
                    return false;
                }
                else
                {
                    return true;
                } 
            }

           
        }

        /// <summary>
        /// 验证编号格式是否正确（格式为当前父级编码加3位） 顾健
        /// </summary>
        /// <param name="code">当前主键</param>
        /// <param name="parentcode">上级编号</param>
        /// <returns></returns>
        public static bool CheckCode(string code, string parentcode)
        {
            if (parentcode == "root")
            {
                if (code.Length == 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((parentcode.Length + 3) == code.Length && code.Substring(0,code.Length-3)==parentcode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建人: 秦白驹
        /// 时间2018-10-11
        /// 功能:给MAction中的值设置操作状态
        /// </summary>
        /// <param name="action"></param>
        /// <param name="fields">以逗号分割的字段字符串</param>
        /// <param name="status"></param>
        public static void SetActionDataFieldStatus(MAction action, String fields, int status)
        {
            if (String.IsNullOrEmpty(fields))
            {
                return;
            }

            String[] arrField = fields.Split(',');

            foreach (string fieldName in arrField)
            {
                if (String.IsNullOrEmpty(fieldName))
                {
                    continue;
                }
                action.Data[fieldName.Trim()].State = status;
            }

        }

        /// <summary>
        /// 创建人: 顾健
        /// 时间：2018-10-18
        /// 功能:检测项目是否正在执行。（可用范围为：与项目相关的所有涉及流程的模块修改功能）
        /// </summary>
        /// <param name="projectcode">项目编号</param>
        /// <returns></returns>
        public static bool CheckProjectIsExecute(string projectcode)
        {
            string sWhere = "projectcode="+DbService.SetQuotesValue(projectcode)+ " and projectstatus=1";
            ;
            return DbService.Exists("decoration_project", sWhere);
        }

        /// <summary>
        /// 创建人: 顾健
        /// 时间：2018-10-18
        /// 功能:检测是否允许删除
        /// </summary>
        /// <param name="curDic">检测范围数据集 key=tablename，value=message</param>
        /// <param name="sWhere">检测条件</param>
        /// <returns></returns>
        public static ExeMsgInfo CheckIsDelete(Dictionary<string, string> curDic,string sWhere)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            try
            {
                foreach (KeyValuePair<string, string> dic in curDic)
                {
                    if (DbService.Exists(dic.Key, sWhere))
                    {

                        throw new Exception(dic.Value);
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

        /// <summary>
        /// 创建人: 顾健
        /// 时间：2018-10-18
        /// 功能:检测时间大小
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool CheckTime(DateTime t1, DateTime t2)
        {
            bool flag = false;
            try
            {
                TimeSpan ts = (t1 - t2);
                int temp = Convert.ToInt32(ts.TotalDays);
                if (temp >= 0)
                {
                    flag = true;

                }
            }
            catch
            {
            }
            return flag;
        }

        /// <summary>
        /// 创建人: 顾健
        /// 时间：2018-10-18
        /// 功能:检测时间大小
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool CheckTime1(DateTime t1, DateTime t2)
        {
            bool flag = false;
            try
            {
                TimeSpan ts = (t1 - t2);
                int temp = Convert.ToInt32(ts.TotalDays);
                if (temp > 0)
                {
                    flag = true;

                }
            }
            catch
            {
            }
            return flag;
        }
    }
}
