using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GZH.CL.Common
{
    public class Util
    {
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">目标字符串的长度</param>
        /// <param name="useNum">是否包含数字，1=包含，默认为包含</param>
        /// <param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        /// <param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        /// <param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        /// <param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        /// <returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;

            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            return s;
        }

        /// <summary>
        /// stream转string
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public string Stream2String(Stream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// string转stream
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Stream String2Stream(string text)
        {
            MemoryStream r = new MemoryStream();
            StreamWriter writer = new StreamWriter(text);
            writer.Write(r);
            writer.Flush();

            return r;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
        }

        public static string GetMD5Hash(String input)
        {
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        public static string GetSignX(Hashtable values, string sn)
        {
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

            string r = Util.GetMD5Hash(stringA);

            return r;
        }
    }
}
