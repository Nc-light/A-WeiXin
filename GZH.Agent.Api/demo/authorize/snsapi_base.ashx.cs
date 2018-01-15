using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using GZH.CL.Common;
using log4net;

namespace GZH.Agent.Api.demo.authorize
{
    /// <summary>
    /// snsapi_base 的摘要说明
    /// </summary>
    public class snsapi_base : IHttpHandler
    {
        //http://2017xsrunning.amway.com.cn/snsapi.ashx
        int id = 191;
        string sn = "w6fF339bDJ06vKSb7qG487t33eh4s64yDnDngpG619jJD4h0TjV98Bj7fbCErVHe";
        string host = "http://wxapi.nutrilite.digi-campaign.com";
        string scope = "snsapi_userinfo"; //snsapi_base

        public void ProcessRequest(HttpContext context)
        {
            ILog logs = LogManager.GetLogger("index");
            logs.Fatal("RUN snsapi_userinfo");

            context.Response.ContentType = "text/plain";
            string redirect = "http://2017xsrunning.amway.com.cn/index.html?parA=a&parB=b&parC=c";
            redirect = context.Server.UrlEncode(redirect);


            string nonce_str = Util.GetRandomString(32, true, true, true, false, "");
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();

            Hashtable ht = new Hashtable();
            ht.Add("nonce_str", nonce_str);
            ht.Add("timestamp", timestamp);
            ht.Add("id", id);

            string sign = Util.GetSignX(ht, sn);

            string requestUrl = host + "/api/authorize/";
            requestUrl += "?scope=" + scope + "&redirect=" + redirect;
            requestUrl += "&nonce_str=" + nonce_str + "&timestamp=" + timestamp + "&id=" + id;
            requestUrl += "&sign=" + sign;

            context.Response.Redirect(requestUrl);
        }

       


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}