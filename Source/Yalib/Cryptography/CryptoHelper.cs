using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Yalib.Cryptography
{
    public static class CryptoHelper
    {
        public static string MD5Hash(string str)
        {
            //System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile
            var md5 = MD5.Create();
            var hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var hashedStr = String.Join("", hashedBytes.Select(x => x.ToString("x2")));

            return hashedStr;
        }
    }
}
