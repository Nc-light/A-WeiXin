using System;
using System.Web;
using System.Web.Configuration;

namespace GZH.CL
{
    public class ConfigSetting
    {
        //public static string configPath = @"C:\2017\Weixin\GZH.Agent.Api\config\";
        //public static string weixin = configPath + "NCD.Weixin.config";
        //public static string weixinWeb = configPath + "NCD.Weixin.Web.config";
        //public static string weixinAgent = configPath + "NCD.Weixin.Agent.config";
        //public static string weixinAgentAccount = configPath + "NCD.Weixin.Agent.Adm.config";

        private static string configPath = WebConfigurationManager.AppSettings["configPath"];
        public static string weixin = configPath + WebConfigurationManager.AppSettings["weixin"];
        public static string weixinWeb = configPath + WebConfigurationManager.AppSettings["weixinWeb"];
        public static string weixinAgent = configPath + WebConfigurationManager.AppSettings["weixinAgent"];
        public static string weixinAgentAccount = configPath + WebConfigurationManager.AppSettings["weixinAgentAccount"];
    }
}
