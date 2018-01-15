using log4net;
using System;
using System.Collections;
using System.Web;
using System.Web.SessionState;
using GZH.CL.Common;
using GZH.CL.Config;
using GZH.CL.Config.Entity;

namespace GZH.Agent.Manager.api.authorize
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //ILog logs = LogManager.GetLogger("index");
            //logs.Fatal("encode:" + context.Request["encode"]);
            //logs.Fatal("redirectA:" + context.Request["redirect"]);

            //签名验证
            if (context.Request["id"] != null && context.Request["sign"] != null && context.Request["nonce_str"] != null)
            {
                string request_sign = context.Request["sign"];

                string nonce_str = context.Request["nonce_str"];
                string timestamp = context.Request["timestamp"];

                int id = int.Parse(context.Request["id"]);
                string sn = new GZH.CL.Config.AgentConfig().GetItem(id).sn;

                if (AgentSign.CheckRequestSign(request_sign, nonce_str, timestamp, id))
                {
                    if (context.Request["scope"] == null || context.Request["redirect"] == null)
                    {
                        context.Response.Write("abort request");
                        context.Response.End();
                    }

                    if (!CheckConfig(id))
                    {
                        context.Response.Write("abort config");
                        context.Response.End();
                    }

                    string scope = context.Request["scope"];
                    string callback_url = context.Request["redirect"];

                    if (!string.IsNullOrWhiteSpace(scope) && callback_url.IndexOf("http://") != -1)
                    {
                        string state = scope == "snsapi_userinfo" ? "1" : "0";
                        state += "|" + id;
                        string redirect_uri = "http://" + HttpContext.Current.Request.Url.Host + "/api/authorize/redirect_uri.ashx";
                        string requestUri = ConfigSetting.GetWeixinWeb().AuthorizeUrl;

                        callback_url = context.Server.UrlEncode(callback_url);
                        redirect_uri += "?callback_url=" + callback_url;

                        requestUri += "?appid=" + ConfigSetting.GetWeixin().AppID;
                        requestUri += "&redirect_uri=" + context.Server.UrlEncode(redirect_uri);
                        requestUri += "&response_type=code&scope=" + scope + "&state=" + state + "#wechat_redirect";
                        
                        context.Response.Redirect(requestUri);
                    }
                }
                else
                    context.Response.Write("abort signature");
            }
            else
                context.Response.Write("abort signature request");
        }

        private bool CheckConfig(int checkid)
        {
            bool check = false;

            AgentConfig agentConfig = new AgentConfig();
            WeixinAgent weixinAgent = agentConfig.GetConfig();
            foreach (WeixinAgentItem item in weixinAgent.agentItem)
            {
                int id = item.id;
                bool authorize = item.authorize;
                string begin = item.begin;//.ToString("yyyy-MM-dd");
                string end = item.end;//.ToString("yyyy-MM-dd");

                //logs.Fatal("url:"+ url + "   checkurl:"+ checkurl+ "   begin:"+ begin+ "   end:"+ end);

                if (checkid == id && authorize)
                {
                    check = DateTimeManger.Availability(DateTime.Parse(begin), DateTime.Parse(end), DateTime.Now);
                    //logs.Fatal("check:" + check);

                    if (check)
                        break;
                }
            }

            return check;
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