using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using log4net;

namespace GZH.Agent.Manager.App_Code
{
    public class SendMail
    {
        ILog logs = LogManager.GetLogger("SendMail");

        /// <summary>
        /// 邮件发送方法
        /// </summary>
        /// <param name="toEmail">接收方</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public bool Do(string toEmail, string[] cc, string title, string content)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //这里使用QQ的邮箱来发送测试，如果是其它邮箱，请根据其它邮箱POP3/IMAP/SMTP服务来设置
            client.Host = WebConfigurationManager.AppSettings["mailHost"];
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["fromAddress"], WebConfigurationManager.AppSettings["fromPwd"]);
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(WebConfigurationManager.AppSettings["fromAddress"], toEmail);
            message.Subject = title;
            message.Body = content;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            foreach (string ccitem in cc)
            {
                message.CC.Add(ccitem);
            }

            message.Attachments.Add(new System.Net.Mail.Attachment(WebConfigurationManager.AppSettings["mailAttachment"]));
            //message.Attachments.Add(new System.Net.Mail.Attachment(@"D:\webs\2017\wxapi.light.gz.cn\web\upload\apply.xlsx"));

            message.IsBodyHtml = true;
            try
            {
                client.Send(message);
                return true;
            }
            catch(Exception ex)
            {
                logs.Debug("Do", ex);
                return false;
            }
        }
    }
}