using System;
using System.Text;

namespace GZH.CL.Common.Encrypt
{
    public class MD5
    {
        public static string GetMD5Hash(String input)
        {
            //MD5加密
            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }
    }
}
