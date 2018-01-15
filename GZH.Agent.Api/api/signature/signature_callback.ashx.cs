using System;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GZH.CL.Common;
using log4net;
using GZH.CL.JsSDK;

namespace GZH.Agent.Api.api.signature
{
    /// <summary>
    /// signature_callback 的摘要说明
    /// </summary>
    public class signature_callback : IHttpHandler
    {
        ILog logs = LogManager.GetLogger("Project");

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            if (!string.IsNullOrWhiteSpace(context.Request["url"]) && !string.IsNullOrWhiteSpace(context.Request["pn"])
                && !string.IsNullOrWhiteSpace(context.Request["sn"]) && !string.IsNullOrWhiteSpace(context.Request["id"]))
            {
                string url = context.Request["url"];
                url = HttpUtility.UrlEncode(url);

                logs.Fatal("url::" + url);

                string pn = context.Request["pn"];
                string sn = context.Request["sn"];
                int id = int.Parse(context.Request["id"]);

                Util util = new Util();

                string nonce_str = Util.GetRandomString(32, true, true, true, false, "");
                string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();

                Hashtable ht = new Hashtable();
                ht.Add("nonce_str", nonce_str);
                ht.Add("timestamp", timestamp);
                ht.Add("id", id);

                string sign = GetSignX(ht, sn);

                string requestUrl = "http://"+ HttpContext.Current.Request.Url.Host + "/api/signature/";
                requestUrl += "?url=" + url;
                requestUrl += "&nonce_str=" + nonce_str + "&timestamp=" + timestamp + "&id=" + id;
                requestUrl += "&sign=" + sign;

                //context.Response.Redirect(requestUrl);

                if (!string.IsNullOrWhiteSpace(pn))
                {
                    WebClient client = new WebClient();
                    string result = Encoding.Default.GetString(client.DownloadData(requestUrl));

                    context.Response.Write(pn + "(" + result + ")");
                }
            }
        }

        private static void Verification(HttpContext context)
        {
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

            string r = Util.GetMD5Hash(stringA);

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