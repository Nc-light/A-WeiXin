using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GZH.Agent.Api.demo.authorize
{
    /// <summary>
    /// snsapi_userinfo 的摘要说明
    /// </summary>
    public class snsapi_userinfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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