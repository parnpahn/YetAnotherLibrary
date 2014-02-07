using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hlt
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 將位元陣列轉換成位元組。
        /// </summary>
        /// <param name="bits">位元陣列。</param>
        /// <returns></returns>
        public static byte BitsToByte(BitArray bits)
        {
            int value = 0;
            int mul = 1;

            for (int i = 0; i < bits.Count && i < 8; i++)
            {
                if (bits[i])
                {
                    value += mul;
                }
                mul *= 2;
            }
            return (byte)value;
        }

        /// <summary> 
        /// 將 byte 陣列轉換成 16 進位格式的字串。
        /// </summary> 
        public static string BytesToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return "";
            }
            const string HexFormat = "{0:X2}";
            StringBuilder sb = new StringBuilder();
            foreach (byte aByte in bytes)
            {
                sb.Append(string.Format(HexFormat, aByte));
            }
            return sb.ToString();
        }

        /// <summary> 
        /// 將 16 進位格式的字串轉換成 byte 陣列。 
        /// </summary> 
        public static byte[] HexStringToBytes(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
            {
                return null;
            }
            try
            {
                int l = Convert.ToInt32(hexStr.Length / 2);
                byte[] b = new byte[l];
                for (int i = 0; i <= l - 1; i++)
                {
                    b[i] = Convert.ToByte(hexStr.Substring(i * 2, 2), 16);
                }
                return b;
            }
            catch (Exception ex)
            {
                throw new System.FormatException("The provided string does not appear to be Hex encoded:" + Environment.NewLine + hexStr + Environment.NewLine, ex);
            }
        }

        /// <summary> 
        /// 將 Base64 格式的字串轉換成 byte 陣列。 
        /// </summary> 
        public static byte[] Base64StringToBytes(string base64Encoded)
        {
            if (string.IsNullOrEmpty(base64Encoded))
            {
                return null;
            }
            try
            {
                return Convert.FromBase64String(base64Encoded);
            }
            catch (System.FormatException ex)
            {
                throw new System.FormatException("字串不是 Base64 編碼:" + Environment.NewLine + base64Encoded + Environment.NewLine, ex);
            }
        }

        /// <summary> 
        /// 將 byte 陣列轉換成 Base64 編碼格式的字串。 
        /// </summary> 
        public static string BytesToBase64String(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return "";
            }
            return Convert.ToBase64String(bytes);
        }


        /// <summary>
        /// 將字串轉成位元組陣列。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToBytes(string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>
        /// 將字串轉成位元組陣列。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToBytes(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static int ToInt32(object obj, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static bool ToBoolean(object obj, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ToString(object obj, string defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                return Convert.ToString(obj);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ToDateTimeString(object obj, string format, string defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            try
            {
                if (format == null)
                {
                    return Convert.ToDateTime(obj).ToString();
                }
                return Convert.ToDateTime(obj).ToString(format);
            }
            catch
            {
                return defaultValue;
            }
        }
    }

}
