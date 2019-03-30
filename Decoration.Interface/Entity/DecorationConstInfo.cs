using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decoration.Interface.Entity
{
    /// <summary>
    /// 装企通静态文件
    /// </summary>
    public class DecorationConstInfo
    {
        #region PC企业用户登录部分

        /// <summary>
        /// 保存在客户端的Cookie信息
        /// </summary>
        public const String CompanyLoginUserCodeKeyForCookie = "ZhuangQiTong-AdfManageLoginUserCode";

        /// <summary>
        /// 用户登录信息Key
        /// </summary>
        public const String CompanyLoginInfoKeyForSession = "ZhuangQiTong-AdfManageLoginUser";

        /// <summary>
        /// 保存在服务器端的Cache信息
        /// </summary>
        public const String CompanyLoginUserCodeKeyForCache = "ZhuangQiTong-AdfManageLoginUserCode";


        #endregion
    }
}
