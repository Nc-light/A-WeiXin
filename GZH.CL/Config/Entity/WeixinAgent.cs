using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GZH.CL.Config.Entity
{
    [XmlRoot("configuration")]
    public class WeixinAgent
    {
        [XmlArray("Agents"), XmlArrayItem("AgentItem")]
        public WeixinAgentItem[] agentItem { get; set; }
    }

    [XmlRoot("Agents")]
    public class WeixinAgentItem
    {
        [XmlAttribute(AttributeName ="id")]
        public int id { get; set; }

        [XmlAttribute(AttributeName = "agent")]
        public string agent { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }

        [XmlAttribute(AttributeName = "authorize")]
        public bool authorize { get; set; }

        [XmlAttribute(AttributeName = "token")]
        public bool token { get; set; }

        [XmlAttribute(AttributeName = "sn")]
        public string sn { get; set; }

        [XmlAttribute(AttributeName = "signature")]
        public bool signature { get; set; }

        [XmlAttribute(AttributeName = "url")]
        public string url { get; set; }

        [XmlAttribute(AttributeName = "begin")]
        public string begin { get; set; }

        [XmlAttribute(AttributeName = "end")]
        public string end { get; set; }

        [XmlAttribute(AttributeName = "email")]
        public string email { get; set; }
    }
}
