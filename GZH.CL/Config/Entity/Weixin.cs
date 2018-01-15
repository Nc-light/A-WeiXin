using System.Xml.Serialization;

namespace GZH.CL.Config.Entity
{
    [XmlRoot("configuration")]
    public class Weixin
    {
        [XmlElement(ElementName = "AppID")]
        public string AppID { get; set; }

        [XmlElement(ElementName = "AppSecret")]
        public string AppSecret { get; set; }

        [XmlElement(ElementName = "SSLCERT_PATH")]
        public string SSLCERT_PATH { get; set; }

        [XmlElement(ElementName = "SSLCERT_PASSWORD")]
        public string SSLCERT_PASSWORD { get; set; }
    }
}
