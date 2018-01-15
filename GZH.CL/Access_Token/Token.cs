using log4net;
using System;
using System.Web;
using System.Web.Caching;
using GZH.CL.Access_Token.Entity;
using GZH.CL.Common;
using GZH.CL.Common.Serialize;

namespace GZH.CL.Access_Token
{
    public class Token
    {
        ILog logs = LogManager.GetLogger("Token");
        public CacheItemRemovedCallback onRemove = null;

        public AccessTokenObject Get()
        {
            AccessTokenObject r = null;
            string cacheName = GZH.CL.Config.ConfigSetting.GetWeixinWeb().TokenCacheName;
            if (HttpContext.Current.Cache[cacheName] == null || HttpContext.Current.Cache[cacheName].ToString() == "")
            {
                r = this.GetFromWeixin();
                //logs.Fatal("Oauth2 Access_Token From Weixin >> " + DateTime.Now);
                //logs.Fatal(r.access_token);
                this.SetToken2Cache(cacheName, r);
            }
            else
            {
                //logs.Fatal("Oauth2 Access_Token From Cache......  ");
                r = (AccessTokenObject)HttpContext.Current.Cache.Get(cacheName);
                //logs.Fatal(r.access_token);
            }

            return r;
        }

        public void RemoveTokenCache()
        {
            string cacheName = GZH.CL.Config.ConfigSetting.GetWeixinWeb().TokenCacheName;
            HttpContext.Current.Cache.Remove(cacheName);

            //清除Ticket缓存
            RemoveTicketCache();
        }

        public void RemoveTicketCache()
        {
            string ticket_cacheName = GZH.CL.Config.ConfigSetting.GetWeixinWeb().TicketCacheName;
            HttpContext.Current.Cache.Remove(ticket_cacheName);
        }

        private void SetToken2Cache(string cacheName, AccessTokenObject token)
        {
            onRemove = new CacheItemRemovedCallback(this.RemovedCallback);
            try
            {
                HttpContext.Current.Cache.Insert(cacheName, token, null, DateTime.Now.Add(new TimeSpan(2, 0, 0)), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Low, onRemove);
                //HttpContext.Current.Cache.Add(cacheName, token, null, DateTime.Now.Add(new TimeSpan(2, 0, 0)), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority
                //logs.Fatal("SetToken2Cache >> "+ cacheName);
            }
            catch (Exception ex)
            {
                logs.Debug("SetToken2Cache", ex);
            }
        }

        private void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            //清除过期缓存
            //logs.Fatal("RemoveTokenCache >> " + DateTime.Now);
            RemoveTicketCache();
        }

        private AccessTokenObject GetFromWeixin()
        {
            string appid = GZH.CL.Config.ConfigSetting.GetWeixin().AppID;
            string secret = GZH.CL.Config.ConfigSetting.GetWeixin().AppSecret;
            
            string requestUri = GZH.CL.Config.ConfigSetting.GetWeixinWeb().TokenUrl;

            requestUri += "?grant_type=client_credential&appid="+ appid + "&secret="+ secret;

            //logs.Fatal("Token requestUri:"+ requestUri);

            AccessTokenObject r = JsonHelper.ScriptDeserialize<AccessTokenObject>(HttpService.Get(requestUri));
            r.generate = Util.GetTimestamp();

            return r;
        }
    }
}
