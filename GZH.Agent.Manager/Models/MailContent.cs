using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GZH.Agent.Manager.Models
{
    public class MailContent
    {
        public string toEmail { get; set; }

        public string[] cc { get; set; }

        public string title { get; set; }

        public string content { get; set; }
    }
}