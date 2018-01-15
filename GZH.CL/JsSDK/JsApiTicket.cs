using log4net;
using System;
using System.Web;
using System.Web.Caching;
using GZH.CL.Access_Token;
using GZH.CL.Access_Token.Entity;
using GZH.CL.Common;
using GZH.CL.Common.Serialize;
using GZH.CL.JsSDK.Entity;

namespace GZH.CL.JsSDK
{
    public class JsApiTicket
    {
        ILog logs = LogManager.GetLogger("JsApiTicket");
        public CacheItemRemovedCallback onRemove = null;

        public TicketEntity Get()
        {
            TicketEntity r = null;
            string cacheName = GZH.CL.Config.ConfigSetting.GetWeixinWeb().TicketCacheName;
            //logs.Fatal("JsApiTicket run:"+ cacheName);
            if (HttpContext.Current.Cache[cacheName] == null || HttpContext.Current.Cache[cacheName].ToString() == "")
            {
                r = this.GetFromWeixin();
                this.SetTicket2Cache(cacheName, r);
            }
            else
            {
                //logs.Fatal("Oauth2 Access_Token From Cache......");
                r = (TicketEntity)HttpContext.Current.Cache.Get(cacheName);
                //logs.Fatal("TicketEntity GetFrom Cache:" + r.ticket);
            }

            return r;
        }

        public void RemoveCache()
        {
            string cacheName = GZH.CL.Config.ConfigSetting.GetWeixinWeb().TicketCacheName;
            HttpContext.Current.Cache.Remove(cacheName);
        }

        private void SetTicket2Cache(string cacheName, TicketEntity token)
        {
            onRemove = new CacheItemRemovedCallback(this.RemovedCallback);
            try
            {
                HttpContext.Current.Cache.Insert(cacheName, token, null, DateTime.Now.Add(new TimeSpan(1, 0, 0)), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, onRemove);
                //logs.Fatal("SetTicket2Cache >> " + cacheName);
            }
            catch (Exception ex)
            {
                logs.Debug("SetTicket2Cache", ex);
                //NCD.WebLog.SysWebLog.WriteLog("error:" + e.ToString());
            }
        }

        private void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
           
        }

        private TicketEntity GetFromWeixin()
        {
            AccessTokenObject tokenObject = new Token().Get(); 
            string requestUri = GZH.CL.Config.ConfigSetting.GetWeixinWeb().TicketUrl;

            requestUri += "?access_token=" + tokenObject.access_token + "&type=jsapi";

            //logs.Fatal("TicketUrl requestUri:"+ requestUri);

            TicketEntity r = JsonHelper.ScriptDeserialize<TicketEntity>(HttpService.Get(requestUri));

            //logs.Fatal("TicketEntity GetFromWeixin():" + r.ticket);

            return r;
        }

    }
}
