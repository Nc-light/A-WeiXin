using System.Security.Cryptography;
using System.Text;

namespace GZH.CL.Common.Encrypt
{
    public static class Sha1
    {
        public static string Encrypt(string source)
        {
            byte[] StrRes = Encoding.Default.GetBytes(source);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
    }
}
