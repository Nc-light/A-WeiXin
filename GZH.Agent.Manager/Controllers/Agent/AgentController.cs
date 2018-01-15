using System;
using System.Collections.Generic;
using System.Web.Http;
using GZH.CL.Config;
using GZH.CL.Config.Entity;
using log4net;
using System.Web.Configuration;
using System.Web;
using GZH.CL.Common.Serialize;
using GZH.Agent.Manager.Models;
using GZH.Agent.Manager.App_Code;

namespace GZH.Agent.Manager.Controllers.Agent
{
    public class AgentController : ApiController
    {
        AgentConfig agentConfig;
        ILog logs = LogManager.GetLogger("AgentController");

        public AgentController()
        {
            agentConfig = new AgentConfig();
            string sessionName = WebConfigurationManager.AppSettings["loginSessionName"];

            if (HttpContext.Current.Session[sessionName] == null)
            {
                MsgEntity r;

                ResponseMsg.SetEntity(out r, 4102);

                HttpContext.Current.Response.Write(JsonHelper.ScriptSerialize(r, false));
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 分派信息邮件发送
        /// </summary>
        /// <param name="mailContent">邮件对象</param>
        /// <returns></returns>
        [Route("adm/agent/mail/")]
        [HttpPost]
        public MsgEntity SendMail(MailContent mailContent)
        {
            MsgEntity r;

            string sessionName = WebConfigurationManager.AppSettings["loginSessionName"];
            if (HttpContext.Current.Session[sessionName] != null)
            {
                SendMail sendMail = new SendMail();

                string toEmail = mailContent.toEmail;
                string[] cc = mailContent.cc;
                string title = mailContent.title;
                string content = mailContent.content;

                if (sendMail.Do(toEmail, cc, title, content))
                    r = ResponseMsg.SetEntity(out r, 5000);
                else
                    r = ResponseMsg.SetEntity(out r, 5101);
            }
            else
                ResponseMsg.SetEntity(out r, 4102);

            return r;
        }

        /// <summary>
        /// 获取SN
        /// </summary>
        /// <returns></returns>
        [Route("adm/agent/sn/")]
        public string GetSn()
        {
            return agentConfig.CreateSN();
        }

        /// <summary>
        /// 获取全部分派记录
        /// </summary>
        /// <returns>全部分派记录列表</returns>
        [Route("adm/agents/")]
        public List<WeixinAgentItem> GetItems()
        {
            return agentConfig.GetItems();
        }

        /// <summary>
        /// 获取单条分派记录
        /// </summary>
        /// <param name="id">记录id</param>
        /// <returns>分派记录对象</returns>
        [Route("adm/agent/{id}")]
        public WeixinAgentItem GetItem(int id)
        {
            return agentConfig.GetItem(id);
        }

        /// <summary>
        /// 获取申请方列表
        /// </summary>
        /// <returns>申请方列表</returns>
        [Route("adm/agents/applyers")]
        public List<string> GetAgentName()
        {
            return agentConfig.GetAgentName();
        }

        /// <summary>
        /// 添加分派记录
        /// </summary>
        /// <param name="item">待添加分派记录对象(id:0)</param>
        /// <returns>处理结果</returns>
        [Route("adm/agent/add/")]
        [HttpPost]
        public MsgEntity PostAgent(WeixinAgentItem item)
        {
            MsgEntity r;

            if (item.id == 0)
            {
                if (!agentConfig.IsExist(item.id))
                {
                    item = agentConfig.AddItem(item);

                    if (item != null && item.id > 0)
                        r = ResponseMsg.SetEntity(out r, 1000);
                    else
                        r = ResponseMsg.SetEntity(out r, 1101);
                }
                else
                    r = ResponseMsg.SetEntity(out r, 1100);
            }
            else
                r = ResponseMsg.SetEntity(out r, 1102);

            return r;
        }

        /// <summary>
        /// 更新分派记录
        /// </summary>
        /// <param name="item">待更新分派记录对象</param>
        /// <returns>处理结果</returns>
        [Route("adm/agent/update/")]
        [HttpPost]
        public MsgEntity PutAgent(WeixinAgentItem item)
        {
            MsgEntity r;

            if (agentConfig.IsExist(item.id))
            {
                if (agentConfig.UpdateItem(item))
                    r = ResponseMsg.SetEntity(out r, 2000);
                else
                    r = ResponseMsg.SetEntity(out r, 2101);
            }
            else
                r = ResponseMsg.SetEntity(out r, 2100);

            return r;
        }

        /// <summary>
        /// 单条删除分派记录
        /// </summary>
        /// <param name="id">待删除分派记录对象id</param>
        /// <returns>处理结果</returns>
        [Route("adm/agent/del/{id}")]
        [HttpPost]
        public MsgEntity DelAgent(int id)
        {
            MsgEntity r;

            if (agentConfig.IsExist(id))
            {
                if (agentConfig.DelItem(id))
                    r = ResponseMsg.SetEntity(out r, 3000);
                else
                    r = ResponseMsg.SetEntity(out r, 3101);
            }
            else
                r = ResponseMsg.SetEntity(out r, 3100);

            return r;
        }

        /// <summary>
        /// 批量删除分派记录
        /// </summary>
        /// <param name="ids">待删除分派记录对象id数组</param>
        /// <returns>处理结果</returns>
        [Route("adm/agent/del/")]
        [HttpPost]
        public MsgEntity DelAgents(int[] ids)
        {
            MsgEntity r;

            int delNum = agentConfig.DelItem(ids);

            if (delNum > 0)
            {
                r = ResponseMsg.SetEntity(out r, 3000);
            }
            else if (delNum == 0)
                r = ResponseMsg.SetEntity(out r, 3100);
            else
                r = ResponseMsg.SetEntity(out r, 3101);

            return r;
        }
    }
}
