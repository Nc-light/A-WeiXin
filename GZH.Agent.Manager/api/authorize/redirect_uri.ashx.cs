using SnsApi.Entity;
using System.Web;
using System.Web.SessionState;
using GZH.CL.SnsApi;
using GZH.CL.Common.Serialize;
using GZH.CL.SnsApi.Entity;
using log4net;
using GZH.CL.Common;

namespace GZH.Agent.Manager.api.authorize
{
    /// <summary>
    /// redirect_uri 的摘要说明
    /// </summary>
    public class redirect_uri : IHttpHandler, IRequiresSessionState
    {
        //ILog logs = LogManager.GetLogger("redirect_uri");

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
            string code = context.Request["code"];
            string state = context.Request["state"];
            string callback_url = context.Request["callback_url"];

            if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(state) && callback_url.IndexOf("http://") != -1)
            {
                int id = int.Parse(state.Split(char.Parse("|"))[1]);
                callback_url = context.Server.UrlDecode(callback_url);

                Oauth2Token access_token = new Oauth2Token();
                
                Oauth2 oauth2 = access_token.Get(code, state);

                string paras;
                string nonce_str, timestamp, sign;
                AgentSign.SetSign(id, out nonce_str, out timestamp, out sign);

                if (oauth2.scope == "snsapi_base")
                {
                    paras = "openid=" + oauth2.openid + "&scope=" + oauth2.scope;
                    paras += "&nonce_str=" + nonce_str + "&timestamp=" + timestamp + "&sign=" + sign;
                    callback_url = SetCallbackUrl(callback_url, paras);
                }
                else if (oauth2.scope == "snsapi_userinfo")
                {
                    UserInfoApi userInfoEntity = new UserInfo().Get(oauth2.access_token, oauth2.openid);

                    paras = "&scope="+ oauth2.scope + "&openid=" + userInfoEntity.openid + "&nickname=" + userInfoEntity.nickname + "&sex=" + userInfoEntity.sex;
                    paras += "&city=" + userInfoEntity.city + "&province=" + userInfoEntity.province + "&country=" + userInfoEntity.country;
                    paras += "&headimgurl=" + userInfoEntity.headimgurl + "&unionid=" + userInfoEntity.unionid;
                    paras += "&nonce_str=" + nonce_str + "&timestamp=" + timestamp + "&sign=" + sign;

                    //logs.Fatal("paras:" + paras);

                    callback_url = SetCallbackUrl(callback_url, paras);
                }

                //logs.Fatal("callback_url:" + callback_url);

                context.Response.Redirect(callback_url);
            }
        }

        private static string SetCallbackUrl(string callback_url, string paras)
        {
            if (callback_url.IndexOf("?") != -1)
                callback_url += "&" + paras;
            else
                callback_url += "?" + paras;

            return callback_url;
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