using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace GZH.Agent.Manager.App_Code
{
    public class Log4NetLogger
    {
        static log4net.ILog securityLogger = log4net.LogManager.GetLogger("SecurityLog");
        static log4net.ILog sysLogger = log4net.LogManager.GetLogger("SystemLog");

       
    }
}