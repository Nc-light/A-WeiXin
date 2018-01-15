using System.Web;
using GZH.CL.Common;

namespace GZH.Agent.Api.api.manager
{
    /// <summary>
    /// getSn 的摘要说明
    /// </summary>
    public class getSn : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(Util.GetRandomString(64, true, true, true, false, ""));
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