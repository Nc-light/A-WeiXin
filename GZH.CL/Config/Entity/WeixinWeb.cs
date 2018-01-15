using System.Xml.Serialization;

namespace GZH.CL.Config.Entity
{
    [XmlRoot("configuration")]
    public class WeixinWeb
    {
        [XmlElement(ElementName = "AuthorizeUrl")]
        public string AuthorizeUrl { get; set; }

        [XmlElement(ElementName = "AuthorizeRedirect_uri")]
        public string AuthorizeRedirect_uri { get; set; }

        [XmlElement(ElementName = "Oauth2Url")]
        public string Oauth2Url { get; set; }

        [XmlElement(ElementName = "UserinfoUrl")]
        public string UserinfoUrl { get; set; }

        [XmlElement(ElementName = "SnsTokenCacheName")]
        public string SnsTokenCacheName { get; set; }

        [XmlElement(ElementName = "TokenUrl")]
        public string TokenUrl { get; set; }

        [XmlElement(ElementName = "TokenCacheName")]
        public string TokenCacheName { get; set; }
        
        [XmlElement(ElementName = "TicketUrl")]
        public string TicketUrl { get; set; }

        [XmlElement(ElementName = "TicketCacheName")]
        public string TicketCacheName { get; set; }

        //
        //[XmlElement(ElementName = "SignatureConfigCacheName")]
        //public string SignatureConfigCacheName { get; set; }

        [XmlElement(ElementName = "AgentCacheName")]
        public string AgentCacheName { get; set; }
    }
}
