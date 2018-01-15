using log4net;
using System;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using GZH.CL.Common;
using GZH.CL.Common.Encrypt;
using GZH.CL.Common.Serialize;
using GZH.CL.Config.Entity;
using GZH.CL.JsSDK.Entity;
using GZH.CL.Config;

namespace GZH.CL.JsSDK
{
    public class Signature
    {
        //ILog logs = LogManager.GetLogger("Signature");
        //string configPath = ConfigSetting.configPath + "NCD.SNS.Signature.config";

        //public CacheItemRemovedCallback onRemove = null;

        public string Get(string url, string encode)
        {
            string r = "";

            if (encode == "true")
                url = HttpUtility.UrlDecode(url);

            string checkurl = "";// url.Replace("http://", "");


            if(url.IndexOf("http://")!=-1)
                checkurl = url.Replace("http://", "");
            else if (url.IndexOf("https://") != -1)
                checkurl = url.Replace("https://", "");


            if (checkurl.IndexOf("/") != -1)
                checkurl = checkurl.Substring(0, checkurl.IndexOf("/"));

            if (url != "")
            {
                if (CheckConfig(checkurl))
                {
                    string ticket = new JsApiTicket().Get().ticket;
                    string timestamp = Util.GetTimestamp();//Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
                    string noncestr = Util.GetRandomString(36, true, true, true, false, "");
                    string str = "jsapi_ticket=" + ticket +
                      "&noncestr=" + noncestr +
                      "&timestamp=" + timestamp +
                      "&url=" + url;

                    //logs.Fatal("before signature str:" + str);

                    string signature = Sha1.Encrypt(str);

                    //logs.Fatal("signature:" + signature);

                    SignatureEntity signatureEntity = new SignatureEntity();
                    signatureEntity.appId = GZH.CL.Config.ConfigSetting.GetWeixin().AppID;
                    signatureEntity.timestamp = timestamp;
                    signatureEntity.nonceStr = noncestr;
                    signatureEntity.signature = signature.ToLower();
                    signatureEntity.url = url;

                    //System.Web.Script.Serialization.JavaScriptSerializer j = new System.Web.Script.Serialization.JavaScriptSerializer();

                    r = JsonHelper.ScriptSerialize(signatureEntity, false);
                }
                else
                    r = "abort config";
            }
            else
            {
                r = "abort request";
            }

            return r;
        }

        private bool CheckConfig(string checkurl)
        {
            bool check = false;

            AgentConfig agentConfig = new AgentConfig();
            WeixinAgent weixinAgent = agentConfig.GetConfig();
            foreach (WeixinAgentItem item in weixinAgent.agentItem)
            {
                string url = item.url;
                bool signature = item.signature;
                string begin = item.begin;//.ToString("yyyy-MM-dd");
                string end = item.end;//.ToString("yyyy-MM-dd");

                //logs.Fatal("url:"+ url + "   checkurl:"+ checkurl+ "   begin:"+ begin+ "   end:"+ end);

                if (checkurl == url && signature)
                {
                    check = DateTimeManger.Availability(DateTime.Parse(begin), DateTime.Parse(end), DateTime.Now);
                    //logs.Fatal("check:" + check);

                    if(check)
                        break;
                }
            }

            return check;
        }
    }
}
