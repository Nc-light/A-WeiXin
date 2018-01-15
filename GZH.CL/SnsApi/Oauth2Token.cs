using SnsApi.Entity;
using GZH.CL.Common.Serialize;
using GZH.CL.Common;
using System.Web;
using System.Web.Caching;
using System;
using log4net;
using GZH.CL.JsSDK;

namespace GZH.CL.SnsApi
{
    public class Oauth2Token
    {
        ILog logs = LogManager.GetLogger("Oauth2Token");
        public CacheItemRemovedCallback onRemove = null;

        public Oauth2 Get(string code, string state)
        {
            Oauth2 r = this.GetFromWeixin(code);

            return r;
        }

        private void SetToken2Cache(string cacheName, Oauth2 oauth2)
        {
            onRemove = new CacheItemRemovedCallback(this.RemovedCallback);
            try
            {
                HttpContext.Current.Cache.Add(cacheName, oauth2, null, DateTime.Now.Add(new TimeSpan(1, 50, 0)), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, onRemove);
                //logs.Fatal("SetToken2Cache >> "+ cacheName);
            }
            catch (Exception ex)
            {
                logs.Debug("SetToken2Cache", ex);
                //NCD.WebLog.SysWebLog.WriteLog("error:" + e.ToString());
            }
        }

        public void RemoveCache(string scope)   
        {
            try
            {
                string cacheName = GZH.CL.Config.ConfigSetting.GetWeixinWeb().SnsTokenCacheName + "_" + scope;
                if (HttpContext.Current.Cache[cacheName] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheName);
                }
            }
            catch (Exception ex) {
                logs.Debug("RemoveCache", ex);
            }
        }

        private void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            JsApiTicket jsApiTicket = new JsApiTicket();
            jsApiTicket.RemoveCache();
            //logs.Fatal("token RemovedCallback to remove ticket");
            //HttpContext.Current.Cache.Remove(WeixinConfig.TicketCacheName);
        }

        private Oauth2 GetFromWeixin(string code)
        {
            string appid = GZH.CL.Config.ConfigSetting.GetWeixin().AppID;
            string secret = GZH.CL.Config.ConfigSetting.GetWeixin().AppSecret;

            string requestUri = GZH.CL.Config.ConfigSetting.GetWeixinWeb().Oauth2Url;
            requestUri += "?appid=" + appid + "&secret=" + secret;
            requestUri += "&code="+ code + "&grant_type=authorization_code";

            //logs.Fatal("requestUri:" + requestUri);
            string result = HttpService.Get(requestUri);
            //logs.Fatal("result:" + result);

            Oauth2 r = JsonHelper.ScriptDeserialize<Oauth2>(result);

            return r;
        }
    }
}
