using System;
using Adf.Core.Util;
using Adf.FrameWork.Interface.Entity;
using CYQ.Data.Table;
using Decoration.Interface.Entity;

namespace Decoration.Service
{
    public class EntLoginService
    {
        /// <summary>
        /// 这里需要对保存在密码进行加密处理
        /// </summary>
        /// <returns></returns>
        public static MDataRow GetLoginInfoFromCookie()
        {
            String userCode = CookieHelper.GetValueByAppKey(DecorationConstInfo.CompanyLoginUserCodeKeyForCookie);
            if (!String.IsNullOrEmpty(userCode))
            {
                return GetLoginInfoFromUserCode(userCode);
            }
            return null;
        }

        /// <summary>
        /// 根据当前用户得到登录信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <returns></returns>
        public static MDataRow GetLoginInfoFromUserCode(String userCode)
        {
            return DecorationService.Instance().CompanyUser().GetEntityWithUserCode(userCode, true);     
        }

        /// <summary>
        /// 得到用户登录信息,如果Session中存在用户信息,则从Session中取出,如果Session中没有,则从Cookie取出用户名,从数据库中进行配对
        /// </summary>
        /// <returns></returns>
        public static MDataRow GetLoginInfo()
        {
            MDataRow entity = null;
            if (SessionHelper.GetByAppKey(DecorationConstInfo.CompanyLoginInfoKeyForSession) != null)
            {
                entity = (MDataRow)SessionHelper.GetByAppKey(DecorationConstInfo.CompanyLoginInfoKeyForSession);
            }
            else
            {
                entity = GetLoginInfoFromCookie();
                if (entity != null)
                {
                    SessionHelper.SetByAppKey(DecorationConstInfo.CompanyLoginInfoKeyForSession, entity);
                }
            }
            return entity;
        }
    }
}
