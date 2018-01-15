using log4net;
using System;
using System.Collections;
using GZH.CL.Common;
using GZH.CL.Common.Encrypt;

namespace GZH.CL.Common
{
    public class AgentSign
    {
        public static string GetSign(Hashtable values, string sn)
        {
            //ILog logs = LogManager.GetLogger("AgentSign");

            ArrayList keys = new ArrayList(values.Keys);
            keys.Sort();

            string stringA = "";
            foreach (string key in keys)
            {
                if (values[key] != null)
                {
                    stringA += key + "=" + values[key].ToString() + "&";
                }
            }

            if (stringA.Length > 0)
                stringA += "sn=" + sn;

            //logs.Fatal("stringA:"+ stringA);
            string r = MD5.GetMD5Hash(stringA);

            return r;
        }

        public static bool CheckRequestSign(string request_sign, string nonce_str, string timestamp, int id)
        {
            bool r = false;
            string sn = new GZH.CL.Config.AgentConfig().GetItem(id).sn;

            Hashtable ht = new Hashtable();
            ht.Add("nonce_str", nonce_str);
            ht.Add("timestamp", timestamp);
            ht.Add("id", id + "");

            string svr_sign = AgentSign.GetSign(ht, sn);

            if (svr_sign == request_sign)
                r = true;

            return r;
        }

        public static void SetSign(int id, out string nonce_str, out string timestamp, out string sign)
        {
            string sn = new GZH.CL.Config.AgentConfig().GetItem(id).sn;
            nonce_str = Util.GetRandomString(32, true, true, true, false, "");
            timestamp = Util.GetTimestamp();

            Hashtable ht = new Hashtable();
            ht.Add("nonce_str", nonce_str);
            ht.Add("timestamp", timestamp);
            ht.Add("id", id + "");

            sign = GetSign(ht, sn);
        }
    }
}
