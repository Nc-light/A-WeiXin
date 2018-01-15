using System;
using System.Web;
using GZH.CL.Access_Token;
using GZH.CL.Access_Token.Entity;
using GZH.CL.Common;
using GZH.CL.Common.Serialize;
using GZH.CL.Config;
using GZH.CL.Config.Entity;

namespace GZH.Agent.Manager.api.access_token
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            //签名验证
            if (context.Request["id"] != null && context.Request["sign"] != null && context.Request["nonce_str"] != null)
            {
                string request_sign = context.Request["sign"];

                string nonce_str = context.Request["nonce_str"];
                string timestamp = context.Request["timestamp"];

                int id = int.Parse(context.Request["id"]);

                if (AgentSign.CheckRequestSign(request_sign, nonce_str, timestamp, id))
                {
                    if (!CheckConfig(id))
                    {
                        context.Response.Write("abort config");
                        context.Response.End();
                    }

                    AccessTokenObject tokenObject = new Token().Get();
                    context.Response.Write(JsonHelper.ScriptSerialize(tokenObject, false));
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
                bool token = item.token;
                string begin = item.begin;//.ToString("yyyy-MM-dd");
                string end = item.end;//.ToString("yyyy-MM-dd");

                //logs.Fatal("url:"+ url + "   checkurl:"+ checkurl+ "   begin:"+ begin+ "   end:"+ end);

                if (checkid == id && token)
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