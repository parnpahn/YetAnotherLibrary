using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Hlt.Text
{
    public static class UnicodeHelper
    {        
        /// <summary>
        /// 判斷傳入的字元是否為ㄅㄆㄇㄈ（注意不含讀音符號）。
        /// </summary>
        /// <param name="aChar"></param>
        /// <returns></returns>
        public static bool IsBopomofo(string aChar)
        {            
            if (String.IsNullOrEmpty(aChar))
                return false;
            int code = Char.ConvertToUtf32(aChar, 0);
            if (code >= 0x3105 && code <= 0x3129) // ㄅㄆㄇㄈ
                return true;
            return false;
        }

        /// <summary>
        /// 判斷傳入的字元是否為中日韓字元（注意不含注音符號ㄅㄆㄇㄈ）。
        /// </summary>
        /// <param name="aChar"></param>
        /// <returns></returns>
        public static bool IsCJK(string aChar)
        {
            if (String.IsNullOrEmpty(aChar))
                return false;
            int code = Char.ConvertToUtf32(aChar, 0);
            if (code >= 0x4e00 && code <= 0x9fcb)
                return true;
            else if (code >= 0x3400 && code <= 0x4db5)
                return true;
            else if (code >= 0x20000 && code <= 0x2a2d6)
                return true;
            else if (code >= 0x2a700 && code <= 0x2b734)
                return true;
            return false;
        }
    }
}
