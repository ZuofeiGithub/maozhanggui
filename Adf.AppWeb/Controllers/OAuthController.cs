using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Adf.Core.Util;

namespace Adf.AppWeb.Controllers
{
    public class OAuthController : Controller
    {
        /// <summary>
        /// 测试,以下方法可以改造成js的post方法来获取AccessToken.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetToken()
        {
            String rValue = GetAccessToken();
            return Content(rValue);
        }

        /// <summary>
        /// 测试,以下方法可以改造成js的post方法来获取AccessToken.
        /// 如果是服务端则需要将accesstoken进行持久化进行处理.
        /// 在小程序中将appid与secrect写入至小程序中 的用户验证中..
        /// 同样的道理将appid与secret写入至app中.
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetAccessToken()
        {
            HttpClient httpClient = new HttpClient();

            String sUrl = "http://localhost:32206";
            Dictionary<String, String> dicParam = new Dictionary<string, string>();
            //AppId
            dicParam.Add("client_id", "wx030fcc24a9fccd0d");
            //AppSecret
            dicParam.Add("client_secret", "a716c8f08e66a33b857b3c17d0996209");
            dicParam.Add("grant_type", "client_credentials");

            //获取AccessToken

            httpClient.BaseAddress = new Uri("http://localhost:32206");
            String rValue = System.Text.Encoding.Default.GetString(httpClient.PostAsync("/token", new FormUrlEncodedContent(dicParam)).Result.Content.ReadAsByteArrayAsync().Result);

            //将rValue转换为Json获取AccessToken
            

            return rValue;
        }

        public static Stream PostJson(string url, string param)
        {



            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();//返回图片数据流

            return s;
        }
    }
}