using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Adf.Core.Entity;
using Adf.Core.Util;
using Adf.SMS.ServiceReferenceSMS;

namespace Adf.SMS
{
    /// <summary>
    /// SMS发送类型辅助类
    /// </summary>
    public class SMSHelper
    {



        #region

        /// <summary>
        /// 向指定的电话发送内容
        /// 
        /// 如果后缀在后面，则当前短信平台向电信手机发送短信收不到，前缀在前面是可以收到的。
        /// 如果后缀在前面，则当前短信平台向移动手机发送短信收不到
        /// 
        /// </summary>
        /// <param name="smsCode">Adf_SMS编码,用判断短信验证码是否一致</param>
        /// <param name="sMobilePhones">手机号码,以,分隔开</param>
        /// <param name="sContent">发送的内容</param>
        /// <returns></returns>
        public static ExeMsgInfo SendSms(string smsCode, String sMobilePhones, String sContent, String suffixName)
        {

            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();

            if (String.IsNullOrEmpty(suffixName))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "短信后缀不能为空;不能缺少【公司名称或者应用名称】";
                return exeMsgInfo;
            }

            sContent = "【" + suffixName + "】" + sContent;

            #region 判断参数的正确性

            //step 1 判断内容的长度是否符合要求
            if (sContent.Length >= 140)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "字符长度超出140个字符";
                return exeMsgInfo;
            }

            //step 2 判断手机号准确性
            if (String.IsNullOrEmpty(sMobilePhones))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "手机号码不能为空";
                return exeMsgInfo;
            }

            String[] arrMobilePhone = sMobilePhones.Split(',');
            Boolean isOk = true;
            foreach (string phone in arrMobilePhone)
            {
                if (!ValidateHelper.IsMobilePhone(phone))
                {
                    isOk = false;
                    break;
                }
            }

            if (isOk == false)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "手机号码存在不正确信息.";
                return exeMsgInfo;
            }


            #endregion

            //构造请求的格式
            sendSMSRequest request = new sendSMSRequest();
            request.arg0 = "8SDK-EMY-6699-RGSST";
            request.arg1 = "nantonganjiesoft170625";

            //实时发送，如果填写需要是时间格式。指定时间点进行发送，这里使用实时发送
            request.arg2 = "";
            request.arg3 = arrMobilePhone;

            //内容
            request.arg4 = sContent;

            //
            request.arg5 = "";

            //消息编码
            request.arg6 = "GBK";

            //优先级5 为最高
            request.arg7 = 5;

            //时间戳
            request.arg8 = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff"));

            //请求   
            try
            {
                SDKClient sdkClient = new SDKClientClient();
                sendSMSResponse response = sdkClient.sendSMS(request);

                //错误代码与错误信息对应表.
                Dictionary<int, string> lstDictionary = new Dictionary<int, string>();
                lstDictionary.Add(0, "发送成功");
                lstDictionary.Add(-1, "系统异常");
                lstDictionary.Add(-2, "客户端异常");
                lstDictionary.Add(-101, "命令不被支持");
                lstDictionary.Add(-102, "RegistryTransInfo删除信息失败");
                lstDictionary.Add(-103, "RegistryInfo更新信息失败");
                lstDictionary.Add(-104, "请求超过限制");
                lstDictionary.Add(-110, "号码注册激活失败");
                lstDictionary.Add(-111, "企业注册失败");
                lstDictionary.Add(-113, "充值失败");
                lstDictionary.Add(-117, "发送短信失败");
                lstDictionary.Add(-118, "接收MO失败");
                lstDictionary.Add(-119, "接收Report失败");
                lstDictionary.Add(-120, "修改密码失败");
                lstDictionary.Add(-122, "号码注销激活失败");
                lstDictionary.Add(-123, "查询单价失败");
                lstDictionary.Add(-124, "查询余额失败");
                lstDictionary.Add(-125, "设置MO转发失败");
                lstDictionary.Add(-126, "路由信息失败");
                lstDictionary.Add(-127, "计费失败0余额");
                lstDictionary.Add(-128, "计费失败余额不足");
                lstDictionary.Add(-190, "数据操作失败");
                lstDictionary.Add(-1100, "序列号错误,序列号不存在内存中,或尝试攻击的用户");
                lstDictionary.Add(-1102, "序列号密码错误");
                lstDictionary.Add(-1103, "序列号Key错误");
                lstDictionary.Add(-1104, "路由失败，请联系系统管理员");
                lstDictionary.Add(-1105, "注册号状态异常, 未用 1");
                lstDictionary.Add(-1107, "注册号状态异常, 停用 3");
                lstDictionary.Add(-1108, "注册号状态异常, 停止 5");
                lstDictionary.Add(-1131, "充值卡无效");
                lstDictionary.Add(-1132, "充值密码无效");
                lstDictionary.Add(-1133, "充值卡绑定异常");
                lstDictionary.Add(-1134, "充值状态无效");
                lstDictionary.Add(-1135, "充值金额无效");
                lstDictionary.Add(-1901, "数据库插入操作失败");
                lstDictionary.Add(-1902, "数据库更新操作失败");
                lstDictionary.Add(-1903, "数据库删除操作失败");
                lstDictionary.Add(-9000, "数据格式错误,数据超出数据库允许范围");
                lstDictionary.Add(-9001, "序列号格式错误");
                lstDictionary.Add(-9002, "密码格式错误");
                lstDictionary.Add(-9003, "客户端Key格式错误");
                lstDictionary.Add(-9004, "设置转发格式错误");
                lstDictionary.Add(-9005, "公司地址格式错误");
                lstDictionary.Add(-9006, "企业中文名格式错误");
                lstDictionary.Add(-9007, "企业中文名简称格式错误");
                lstDictionary.Add(-9008, "邮件地址格式错误");
                lstDictionary.Add(-9009, "企业英文名格式错误");
                lstDictionary.Add(-9010, "企业英文名简称格式错误");
                lstDictionary.Add(-9011, "传真格式错误");
                lstDictionary.Add(-9012, "联系人格式错误");
                lstDictionary.Add(-9013, "联系电话");
                lstDictionary.Add(-9014, "邮编格式错误");
                lstDictionary.Add(-9015, "新密码格式错误");
                lstDictionary.Add(-9016, "发送短信包大小超出范围");
                lstDictionary.Add(-9017, "发送短信内容格式错误");
                lstDictionary.Add(-9018, "发送短信扩展号格式错误");
                lstDictionary.Add(-9019, "发送短信优先级格式错误");
                lstDictionary.Add(-9020, "发送短信手机号格式错误");
                lstDictionary.Add(-9021, "发送短信定时时间格式错误");
                lstDictionary.Add(-9022, "发送短信唯一序列值错误");
                lstDictionary.Add(-9023, "充值卡号格式错误");
                lstDictionary.Add(-9024, "充值密码格式错误");
                lstDictionary.Add(-9025, "客户端请求sdk5超时");

                int responseStatus = response.@return;

                if (responseStatus == 0)
                {
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = smsCode;
                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    if (lstDictionary.ContainsKey(responseStatus))
                    {
                        exeMsgInfo.RetValue = lstDictionary[responseStatus];
                    }
                    else
                    {
                        exeMsgInfo.RetValue = "未知错误." + responseStatus;
                    }

                }
            }
            catch (Exception ex)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = ex.Message;
            }

            return exeMsgInfo;
        }



        #endregion
        /// <summary>
        /// 用于海学的短信平台
        /// </summary>
        /// <param name="sMobilePhones">电话号码</param>
        /// <param name="sContent">验证码不要超过6位数。</param>
        /// <returns></returns>
        public static ExeMsgInfo SendSms2(String sMobilePhones, String sContent)
        {
            string ret = "";
            CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            bool isInit = api.init("appsms.cloopen.com", "8883");
            api.setAccount("8a216da85cce7c54015ce376b96a0611", "52a584ceb352491f9dcec7b2780fac55");
            api.setAppId("8a216da85cce7c54015ce376b9bc0617");
            try
            {
                if (isInit)
                {
                    Dictionary<string, object> retData = api.SendTemplateSMS(sMobilePhones, "185544", new string[] { sContent });
                    ret = GetDictionaryData(retData);

                    
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {

            }

            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (ret.Contains("statusCode") && ret.Contains("000000"))
            {
                exeMsgInfo.RetStatus = 100;
                exeMsgInfo.RetValue = "发送成功";
            }
            else
            {
                LogHelper.WriteLog("短信返回结果是：" + ret);
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "失败";
            }

            return exeMsgInfo;
        }


        private static string GetDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += GetDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }

    }

}
