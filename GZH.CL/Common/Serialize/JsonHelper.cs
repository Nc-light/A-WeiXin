using System.Web.Script.Serialization;

namespace GZH.CL.Common.Serialize
{
    public static class JsonHelper
    {
        public static T ScriptDeserialize<T>(string strJson)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(strJson);
        }

        public static string ScriptSerialize(object customer, bool unescape)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string r = "";

            if (unescape)
                r = System.Text.RegularExpressions.Regex.Unescape(js.Serialize(customer));
            else
                r = js.Serialize(customer);

            return r;
        }
    }
}
