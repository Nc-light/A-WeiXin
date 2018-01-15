using System;
using System.Web;
using log4net;

namespace GZH.CL.Common
{
    public class Cache
    {
        ILog logs = LogManager.GetLogger("Cache");
        public System.Web.Caching.CacheItemRemovedCallback onRemove = null;

        public bool SetObject2Cache(string key, object value)
        {
            bool r = false;
            onRemove = new System.Web.Caching.CacheItemRemovedCallback(this.RemovedCallback);
            if (!this.IsExist(key))
            {
                try
                {
                    HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.Add(new TimeSpan(48, 0, 0)), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, onRemove);
                    r = true;
                    //NCD.WebLog.SysWebLog.WriteCustomLog("cache", key+" adding cacheX....");
                }
                catch (Exception ex)
                {
                    logs.Debug("SetObject2Cache", ex);
                    //NCD.WebLog.SysWebLog.WriteCustomLog("Cache", "SetObject2Cache[" + key + "] is error:" + e.ToString());
                }
            }
            return r;
        }

        public object GetCacheObject(string key)
        {
            object r = null;

            if (this.IsExist(key))
            {
                r = HttpRuntime.Cache[key];
                //NCD.WebLog.SysWebLog.WriteCustomLog("cache", "GetCache:" + r);
            }


            return r;
        }

        public void RemovedCallback(string a, object o, System.Web.Caching.CacheItemRemovedReason reason)
        {
            //NCD.WebLog.SysWebLog.WriteLog("cacheOnRemove reason:" + reason.ToString());
        }

        public bool IsExist(string key)
        {
            bool r = false;

            if (HttpRuntime.Cache[key] != null)
                r = true;

            return r;
        }
    }
}
