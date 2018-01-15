using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GZH.Agent.Manager.Models
{
    [XmlRoot("configuration")]
    public class Account
    {
        [XmlArray("Accounts"), XmlArrayItem("Account")]
        public AccountItem[] accountItem { get; set; }
    }

    [XmlRoot("Accounts")]
    public class AccountItem
    {
        [XmlAttribute(AttributeName = "name")]
        public string name { get; set; }

        [XmlAttribute(AttributeName = "pwd")]
        public string pwd { get; set; }

        [XmlAttribute(AttributeName = "status")]
        public bool status { get; set; }
    }
}