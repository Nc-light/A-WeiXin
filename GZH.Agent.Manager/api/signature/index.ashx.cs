using System;
using System.Web;
using GZH.CL.Common;
using GZH.CL.JsSDK;

namespace GZH.Agent.Manager.api.signature
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
                    if (context.Request["url"] == null)
                    {
                        context.Response.Write("abort request");
                        context.Response.End();
                    }

                    string url = context.Request["url"];

                    string encode = context.Request["encode"];
                    if (string.IsNullOrWhiteSpace(encode))
                        encode = "false";

                    Signature signature = new Signature();
                    context.Response.Write(signature.Get(url, encode));

                }
                else
                    context.Response.Write("abort signature");
            }
            else
                context.Response.Write("abort signature request");
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