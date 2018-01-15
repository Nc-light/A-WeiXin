using GZH.CL.Config;
using GZH.CL.Config.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GZH.Agent.Api
{
    /// <summary>
    /// test2 的摘要说明
    /// </summary>
    public class test2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            AgentConfig agentConfig2 = new AgentConfig();
            List<WeixinAgentItem> items2 = agentConfig2.GetItems();
            context.Response.Write("countB:" + items2.Count + "\n\r");
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