using System;
using System.Web.Http;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace GZH.Agent.Manager
{
        public class WebApiApplication : System.Web.HttpApplication
        {
            public override void Init()
            {
                this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
                base.Init();
            }

            protected void Application_Start()
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/config/Log4Net.config"));

                GlobalConfiguration.Configure(WebApiConfig.Register);

                GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            }
        }
}
