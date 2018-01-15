using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using GZH.CL.Config.Entity;
using GZH.CL.SnsApi;
using GZH.CL.Common;
using System.Collections;
using GZH.CL.Common.Serialize;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace GZH.CL.Config
{
    public class AgentConfig
    {
        ILog logs = LogManager.GetLogger("AgentConfig");
        public CacheItemRemovedCallback onRemove = null;
        private string configPath = GZH.CL.ConfigSetting.weixinAgent;

        /// <summary>
        /// 获取SN
        /// </summary>
        /// <returns></returns>
        public string CreateSN()
        {
            string now = DateTime.Now.Ticks.ToString();
            return Util.GetRandomString(64, true, true, true, false, now);
        }

        /// <summary>
        /// 配置记录记录是否存在
        /// </summary>
        /// <param name="id">配置记录ID</param>
        /// <returns></returns>
        public bool IsExist(int id)
        {
            List<WeixinAgentItem> items = this.GetItems();

            return items.Exists(v => v.id == id);
        }

        /// <summary>
        /// 根据记录ID读取配置内容对象
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>WeixinAgentItem对象</returns>
        public WeixinAgentItem GetItem(int id)
        {
            WeixinAgentItem r = null;
            WeixinAgent weixinAgent = GetConfig();

            Array arr =
                (from weixinAgentItem in weixinAgent.agentItem
                 where (weixinAgentItem.id == id)
                 select weixinAgentItem).ToArray<WeixinAgentItem>();

            if (arr.Length > 0)
                r = (WeixinAgentItem)arr.GetValue(0);

            return r;
        }

        /// <summary>
        /// 读取全部配置内容对象
        /// </summary>
        /// <returns>WeixinAgentItem对象</returns>
        public List<WeixinAgentItem> GetItems()
        {
            List<WeixinAgentItem> r = null;
            WeixinAgent weixinAgent = GetConfig();

            List<WeixinAgentItem> list =
                (from weixinAgentItem in weixinAgent.agentItem
                 select weixinAgentItem).ToList<WeixinAgentItem>();

            if (list.Count > 0)
                r = list;

            return r;
        }

        /// <summary>
        /// 获取申请用户
        /// </summary>
        /// <returns></returns>
        public List<string> GetAgentName()
        {
            List<string> r = null;
            WeixinAgent weixinAgent = GetConfig();

            List<string> list =
                (from weixinAgentItem in weixinAgent.agentItem
                 select weixinAgentItem.agent).ToList<string>();

            if (list.Count > 0)
                r = new HashSet<string>(list).ToList<string>();
            
            return r; 
        }

        /// <summary>
        /// 分派信息配置对象状态
        /// </summary>
        /// <returns></returns>
        public bool GetAgentStatus(WeixinAgentItem item)
        {
            string begin = item.begin;//.ToString("yyyy-MM-dd");
            string end = item.end;//.ToString("yyyy-MM-dd");

            return DateTimeManger.Availability(DateTime.Parse(begin), DateTime.Parse(end), DateTime.Now); ;
        }

        /// <summary>
        /// 创建记录时获取配置项ID
        /// </summary>
        /// <returns></returns>
        private int GetIdentity()
        {
            WeixinAgent weixinAgent = GetConfig();
            List<int> list = (from weixinAgentItem in weixinAgent.agentItem select weixinAgentItem.id).ToList<int>();
             
            return this.CreateRandom(list);
        }

        /// <summary>
        /// 获取配置项唯一ID
        /// </summary>
        /// <param name="list">已有ID列表</param>
        /// <returns></returns>
        private int CreateRandom(List<int> list)
        {
            Random rdm = new Random();
            int r = rdm.Next(0, 1000);

            if (list.Contains(r))
                r = CreateRandom(list);

            return r;
        }

        /// <summary>
        /// 添加配置记录
        /// </summary>
        /// <param name="item">WeixinAgentItem对象</param>
        /// <returns>处理结果</returns>
        public WeixinAgentItem AddItem(WeixinAgentItem item)
        {
            item.id = this.GetIdentity();

            List<WeixinAgentItem> items = this.GetItems();
            items.Add(item);

            WeixinAgent weixinAgent = new WeixinAgent();
            weixinAgent.agentItem = items.ToArray();
            
            try
            {
                string path = configPath;
                XmlHelper.XmlSerializeToFile(weixinAgent, path, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                item = null;
                logs.Debug("AddItem", ex);
            }
            

            return item;
        }

        /// <summary>
        /// 修改配置记录
        /// </summary>
        /// <param name="item">WeixinAgentItem对象</param>
        /// <returns>处理结果</returns>
        public bool UpdateItem(WeixinAgentItem item)
        {
            bool r = false;

            if (item != null && this.IsExist(item.id) )
            {
                List<WeixinAgentItem> items = this.GetItems();

                WeixinAgent weixinAgent = new WeixinAgent();

                List<WeixinAgentItem> newItems = new List<WeixinAgentItem>();
                foreach (WeixinAgentItem i in items)
                {
                    if (i.id == item.id)
                        newItems.Add(item);
                    else
                        newItems.Add(i);
                }
                weixinAgent.agentItem = newItems.ToArray();

                try
                {
                    string path = configPath;
                    XmlHelper.XmlSerializeToFile(weixinAgent, path, System.Text.Encoding.UTF8);
                    r = true;
                }
                catch (Exception ex)
                {
                    r = false;
                    logs.Debug("UpdateItem Exception", ex);
                }
            }

            return r;
        }

        /// <summary>
        /// 删除配置记录
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <returns>处理结果</returns>
        public bool DelItem(int id)
        {
            bool r = false;
            WeixinAgentItem item = this.GetItem(id);

            if (item != null)
            {
                List<WeixinAgentItem> items = this.GetItems();

                WeixinAgent weixinAgent = new WeixinAgent();
                items.Remove(item);
                weixinAgent.agentItem = items.ToArray();

                try
                {
                    string path = configPath;
                    XmlHelper.XmlSerializeToFile(weixinAgent, path, System.Text.Encoding.UTF8);
                    r = true;
                }
                catch (Exception ex)
                {
                    r = false;
                    logs.Debug("DelItem", ex);
                }
            }

            return r;
        }

        public int DelItem(int[] ids)
        {
            int r = 0;
            List<WeixinAgentItem> items = this.GetItems();
            
            r =  items.RemoveAll(v => ids.Contains(v.id));

            WeixinAgent weixinAgent = new WeixinAgent();
            weixinAgent.agentItem = items.ToArray();

            try
            {
                string path = configPath;
                XmlHelper.XmlSerializeToFile(weixinAgent, path, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                r = -1;
                logs.Debug("DelItem", ex);
            }

            return r;
        }

        //############################################



        //############################################

        /// <summary>
        /// 从缓存获取分派信息配置对象（验证分派记录有效性）
        /// </summary>
        /// <returns></returns>
        public WeixinAgent GetConfig()
        {
            WeixinAgent r = null;
            string cacheName = GZH.CL.Config.ConfigSetting.GetWeixinWeb().AgentCacheName;
            if (HttpContext.Current.Cache[cacheName] == null || HttpContext.Current.Cache[cacheName].ToString() == "")
            {
                r = GetFromFile();
                SetWeixinAgent2Cache(cacheName, r);
            }
            else
            {
                r = (WeixinAgent)HttpContext.Current.Cache.Get(cacheName);
            }
            return r;
        }

        private WeixinAgent GetFromFile()
        {
            return GZH.CL.Config.ConfigSetting.GetWeixinAgent();
        }

        private void SetWeixinAgent2Cache(string cacheName, WeixinAgent weixinAgent)
        {
            onRemove = new CacheItemRemovedCallback(RemovedCallback);
            try
            {
                CacheDependency dep = new CacheDependency(configPath);

                HttpContext.Current.Cache.Add(cacheName, weixinAgent, dep, DateTime.Now.Add(new TimeSpan(30, 0, 0, 0)), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, onRemove);
                //logs.Fatal("SetToken2Cache >> "+ cacheName);
            }
            catch (Exception ex)
            {
                logs.Debug("SetWeixinAgent2Cache", ex);
            }
        }

        private void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            Oauth2Token oauth2Token = new Oauth2Token();
            string[] scopes = new string[] { "snsapi_base", "snsapi_userinfo" };
            foreach (string scope in scopes)
            {
                oauth2Token.RemoveCache(scope);
            }

            logs.Info("AgentConfig RemovedCallback to remove token");
        }

    }
}
