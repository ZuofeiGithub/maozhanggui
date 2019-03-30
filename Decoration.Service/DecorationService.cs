using System;
using System.Configuration;
using Adf.Core.Util;
using Decoration.Implement.MySql;
using Decoration.Interface;

namespace Decoration.Service
{
    public class DecorationService
    {
        /// <summary>
        /// 创建系统接口
        /// </summary>
        /// <returns></returns>
        public static IAppFactory Instance()
        {
            String dbType = ConfigurationManager.AppSettings["DbType"];
            if (String.IsNullOrEmpty(dbType))
            {
                throw new Exception("数据库类型不能为空");
            }
            if (dbType.Contains("MySql"))
            {
                return new AppFactoryImpl();
            }

            throw new Exception("框架未实现");
        }


        /// <summary>
        /// 初始化日期格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string Initdate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                return ConvertHelper.ObjectToT(date, DateTime.Now).ToString("yyyy-MM-dd");
            }
            else
            {
                return "";
            }
        }

    }
}
