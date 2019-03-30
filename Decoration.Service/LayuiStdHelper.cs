using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Decoration.Service
{
    public static class LayuiStdHelper
    {

        /// <summary>
        /// 必填*号
        /// 修改人：张硕
        /// 修改时间：2018-09-10
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="isTrue">默认不显示，true为显示</param>
        /// <returns></returns>
        public static MvcHtmlString LayuiStdFormRequired(this HtmlHelper htmlHelper, Boolean isTrue = false)
        {
            String htmlInfo = "<div class='layui-form-mid layui-word-aux'><span>&nbsp;</span></div>";
            if (isTrue)
            {
                htmlInfo = "<div class='layui-form-mid  ' style='color:red'><span>*</span></div>";
            }
            return MvcHtmlString.Create(htmlInfo);
        }
    }
}
