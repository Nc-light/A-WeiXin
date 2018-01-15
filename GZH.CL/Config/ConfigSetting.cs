using GZH.CL.Common.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZH.CL.Config
{
    public class ConfigSetting : GZH.CL.ConfigSetting
    {
        /// <summary>
        /// 获取公众号配置对象
        /// </summary>
        /// <returns>Weixin.config 配置对象</returns>
        public static GZH.CL.Config.Entity.Weixin GetWeixin()
        {
            string path = GZH.CL.ConfigSetting.weixin;
            //System.Console.WriteLine("Weixin path:" + path);
            GZH.CL.Config.Entity.Weixin r = XmlHelper.XmlDeserializeFromFile<GZH.CL.Config.Entity.Weixin>(path, Encoding.UTF8);

            return r;
        }

        /// <summary>
        /// 获取公众号接口地址对象
        /// </summary>
        /// <returns>Weixin.Web.config 接口地址对象</returns>
        public static GZH.CL.Config.Entity.WeixinWeb GetWeixinWeb()
        {
            string path = GZH.CL.ConfigSetting.weixinWeb;
            //System.Console.WriteLine("WeixinWeb path:" + path);
            GZH.CL.Config.Entity.WeixinWeb r = XmlHelper.XmlDeserializeFromFile<GZH.CL.Config.Entity.WeixinWeb>(path, Encoding.UTF8);

            return r;
        }

        /// <summary>
        /// 获取分派信息对象
        /// </summary>
        /// <returns>Weixin.Agent.config 分派信息对象</returns>
        public static GZH.CL.Config.Entity.WeixinAgent GetWeixinAgent()
        {
            string path = GZH.CL.ConfigSetting.weixinAgent;
            //System.Console.WriteLine("weixinAgent path:" + path);
            GZH.CL.Config.Entity.WeixinAgent r = XmlHelper.XmlDeserializeFromFile<GZH.CL.Config.Entity.WeixinAgent>(path, Encoding.UTF8);

            return r;
        }
    }
}
