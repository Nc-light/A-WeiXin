using GZH.CL.Config;
using GZH.CL.Config.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GZH.Agent.Api
{
    /// <summary>
    /// test 的摘要说明
    /// </summary>
    public class test : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            AgentConfig agentConfig = new AgentConfig();

            //根据记录ID读取配置内容对象
            //WeixinAgentItem item = agentConfig.GetItem(15);
            //context.Response.Write(item.name);


            //获取SN
            //string sn = agentConfig.CreateSN();
            //context.Response.Write(sn);


            //读取全部配置内容对象
            //List<WeixinAgentItem> items = agentConfig.GetItems();
            //foreach (WeixinAgentItem item in items)
            //{
            //    context.Response.Write(item.name + "\n\r");
            //}


            //获取申请用户
            //List<string> agentNames = agentConfig.GetAgentName();
            //foreach (string name in agentNames)
            //{
            //    context.Response.Write(name + "\n\r");
            //}


            //分派信息配置对象状态
            //WeixinAgentItem item = agentConfig.GetItem(15);
            //bool status = agentConfig.GetAgentStatus(item);
            //context.Response.Write(status);


            //创建记录时获取配置项ID
            //int id = agentConfig.GetIdentity();
            //context.Response.Write(id);


            //添加配置记录
            //List<WeixinAgentItem> items = agentConfig.GetItems();
            //context.Response.Write("countA:" + items.Count + "\n\r");

            //WeixinAgentItem item = new WeixinAgentItem();
            //item.id = agentConfig.GetIdentity();
            //item.agent = agentConfig.GetAgentName()[0];
            //item.name = "项目A";
            //item.authorize = true;
            //item.token = true;
            //item.signature = true;
            //item.url = "www.baidu.com";
            //item.sn = agentConfig.CreateSN();
            //item.begin = "2017-01-01";// DateTime.Parse("2017-01-01");
            //item.end = "2017-12-31";// DateTime.Parse("2017-12-31");

            //context.Response.Write("add:" + agentConfig.AddItem(agentConfig, item) + "\n\r");

            //AgentConfig agentConfig2 = new AgentConfig();
            //List<WeixinAgentItem> items2 = agentConfig2.GetItems();
            //context.Response.Write("countB:" + items2.Count + "\n\r");


            //修改配置记录
            WeixinAgentItem item = agentConfig.GetItem(995);
            if (item != null)
            {
                item.name = "更新NAME-995";

                context.Response.Write("update:" + agentConfig.UpdateItem(item) + "\r\n");
            }

            List<WeixinAgentItem> items = agentConfig.GetItems();
            foreach (WeixinAgentItem i in items)
            {
                context.Response.Write("id:" + i.id + "     ***   name:" + i.name + "\n\r");
            }


            //删除配置记录
            //context.Response.Write("del:" + agentConfig.DelItem(agentConfig, 559) + "\r\n");
            //List<WeixinAgentItem> items = agentConfig.GetItems();
            //foreach (WeixinAgentItem i in items)
            //{
            //    context.Response.Write("id:" + i.id + "     ***   name:" + i.name + "\n\r");
            //}
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