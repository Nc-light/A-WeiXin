using System;
using System.Collections;
using System.Web;
using GZH.CL.Common;
using GZH.CL.Common.Encrypt;
using GZH.CL.Config;
using GZH.CL.Config.Entity;
using log4net;

namespace GZH.Agent.Manager.example
{
    /// <summary>
    /// access_token 的摘要说明
    /// </summary>
    public class access_token : IHttpHandler
    {
        //http://wxapi.light.gz.cn/example/access_token.ashx
        ILog logs = LogManager.GetLogger("authorize_example");

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int id = 99999;
            AgentConfig agentConfig = new AgentConfig();
            WeixinAgentItem weixinAgentItem = agentConfig.GetItem(id);

            if (weixinAgentItem != null)
            {
                string host = "http://" + HttpContext.Current.Request.Url.Host;
                string nonce_str = Util.GetRandomString(32, true, true, true, false, "");
                string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
                string sn = weixinAgentItem.sn;

                Hashtable ht = new Hashtable();
                ht.Add("nonce_str", nonce_str);
                ht.Add("timestamp", timestamp);
                ht.Add("id", id);

                string sign = GetSignX(ht, sn);

                string requestUrl = host + "/api/access_token/";
                requestUrl += "?nonce_str=" + nonce_str + "&timestamp=" + timestamp + "&id=" + id;
                requestUrl += "&sign=" + sign;

                context.Response.Write(HttpService.Get(requestUrl));
            }
            else
                context.Response.Write("示例配置记录不存在");
        }

        public string GetSignX(Hashtable values, string sn)
        {
            ArrayList keys = new ArrayList(values.Keys);
            keys.Sort();

            string stringA = "";
            foreach (string key in keys)
            {
                if (values[key] != null)
                {
                    stringA += key + "=" + values[key].ToString() + "&";
                }
            }

            if (stringA.Length > 0)
                stringA += "sn=" + sn;

            logs.Fatal("stringA:" + stringA);

            string r = MD5.GetMD5Hash(stringA);

            return r;
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