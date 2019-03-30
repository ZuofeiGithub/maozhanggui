using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adf.SMS.Entity
{
    /// <summary>
    /// 短信请求字段
    /// </summary>
    public class SMSRequest
    {

        /// <summary>
        /// 待发送的内容
        /// </summary>
        public String Content { get; set; }

        /// <summary>
        /// 计划发送时间
        /// </summary>
        public DateTime SendDateTime { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public String MobilePhones { get; set; }


    }
}
