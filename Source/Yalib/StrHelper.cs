using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Yalib
{
    public static class StrHelper
    {
        private static HashSet<string> _trueStrings; // For method ToBoolean().

        static StrHelper()
        {
            _trueStrings = new HashSet<string>() {
                "True", "Y", "1", "Yes", "On", "Enabled"
            };
        }

        public static String TrueStrings
        {
            get
            {
                var sb = new StringBuilder();
                foreach (string s in _trueStrings)
                {
                    sb.AppendFormat("{0};", s);
                }
                return sb.ToString().TrimEnd(';');
            }
        }

        /// <summary>
        /// Append a slash character '\' to string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Obsolete("Use EnsureEndWith() or EnsureEndWithEnsureEndWithDirectorySeparator() instead.")]
        public static string AppendSlash(string input)
        {
            if (input == null)
                return @"\";
            if (input.EndsWith("/") || input.EndsWith("\\"))
                return input;
            return input + "\\";
        }

        /// <summary>
        /// Remove the last slash character.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveLastSlash(string input)
        {
            if (input == null)
                return "";
            if (input.EndsWith("/") || input.EndsWith("\\"))
                return input.Remove(input.Length - 1, 1);
            return input;
        }

        /// <summary>
        /// 取出檔案路徑名稱的路徑部份，結尾會包含斜線或反斜線。
        /// Similar to System.IO.GetDirectoryName(). Difference: for "C:\", this method returns "C:\", but GetDirectoryName() returns an empty string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ExtractFilePath(string input)
        {
            if (input == null)
                return String.Empty;
            int i = input.LastIndexOfAny(@"/\".ToCharArray());
            if (i > 0)
                return input.Substring(0, i + 1);
            return String.Empty;
        }

        /// <summary>
        /// 取出檔案路徑名稱的檔名部分。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ExtractFileName(string s)
        {
            if (s == null)
                return String.Empty;
            int i = s.LastIndexOfAny(@"/\".ToCharArray(), s.Length - 1);
            if (i > 0)
                return s.Substring(i + 1);
            return String.Empty;
        }

        /// <summary>
        /// 移除任何一個指定的字元。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="anyOf">要移除的字元。</param>
        /// <returns></returns>
        public static string RemoveAny(string input, char[] anyOf)
        {
            if (String.IsNullOrEmpty(input))
                return input;

            int i;
            while (true)
            {
                i = input.IndexOfAny(anyOf);
                if (i < 0)
                    break;
                input = input.Remove(i, 1);
            }
            return input;
        }

        /// <summary>
        /// 移除換行字元 (\n) 和 (\r)。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveNewLines(string input)
        {
            return StrHelper.RemoveNewLines(input, false);
        }

        /// <summary>
        /// 移除換行字元 (\n) 和 (\r)。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="addSpace">若為 true，則把換行符號替換成空白字元。</param>
        /// <returns></returns>
        public static string RemoveNewLines(string input,
           bool addSpace)
        {
            string replace = string.Empty;
            if (addSpace)
                replace = " ";

            string pattern = @"[\r|\n]";
            Regex regEx = new Regex(pattern, RegexOptions.Multiline);

            return regEx.Replace(input, replace);
        }

        /// <summary>
        /// 移除空白字元（包括全形空白）。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fullShapeSpaces">是否連全形空白也一併刪除。</param>
        /// <returns></returns>
        public static string RemoveSpaces(string input, bool fullShapeSpaces)
        {
            string replace = string.Empty;
            string pattern = @"[ ]";
            if (fullShapeSpaces)
            {
                pattern = @"[ |　]";
            }

            Regex regEx = new Regex(pattern, RegexOptions.Multiline);

            return regEx.Replace(input, replace);
        }


        /// <summary>
        ///   CheckIdNo: 檢驗身分證號碼的正確性
        ///		驗證規則
        ///
        ///			身份證統一編號共計有 10 位，其中第一位為英文字母，後共有九個數字；
        ///		而最後一位數字為檢查碼( Check Digit ) ，表示如下表：
        ///
        ///		L1 D2 D2 D3 D4 D5 D6 D7 D8 D9 
        ///
        ///		L1: 英文字母, 代表出生地的縣/市代號
        ///		D2: 性別,1=男, 2=女
        ///		D9: 檢查碼
        ///
        ///		L1 對照表:
        ///
        ///		  字母 A  B  C  D  E  F  G  H  J  K  L  M  N 
        ///		  代號 10 11 12 13 14 15 16 17 18 19 20 21 22 
        ///		  -------------------------------------------
        ///		  字母 P  Q  R  S  T  U  V  X  Y  W  Z  I  O
        ///		  代號 23 24 25 26 27 28 29 30 31 32 33 34 35 
        ///
        ///		令其十位數為 X1 ，個位數為 X2 ；( 如Ａ：X1=1 , X2=0 )
        ///
        ///		依其公式計算結果：
        ///		Ｙ= X1 + 9*X2 + 8*D1 + 7*D2 + 6*D3 + 5*D4 + 4*D5 + 3*D6 + 2*D7 + 
        ///		D8 + D9
        ///
        ///	  假如Ｙ能被 10 整除，則表示該身份證號碼為正確，否則為錯誤。即如以 10 為
        ///		模數，檢查號碼為 ( 10 - Ｙ - D9 ) / 10 的餘數，如餘數為 0 時，則檢查碼
        ///		為 0 。
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckIdno(string idno)
        {
            int[] letter_weight = 
            {
                // A   B   C   D   E   F   G   H   I   J   K   L   M   N   O   P   Q   R
                10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25,
                26, 27, 28, 29, 32, 30, 31, 33
                // S   T   U   V   W   X   Y   Z
            };

            int i;
            int[] D = new int[9];	 // 1..9
            int sum;

            if (idno.Length != 10)
                return false;
            idno = idno.ToUpper();
            if (!Char.IsLetter(idno[0]) || (idno[1] != '1' && idno[1] != '2'))
                return false;
            for (i = 1; i < 10; i++)
            {
                if (!Char.IsDigit(idno, i))
                    return false;
            }

            for (i = 0; i < 9; i++) // 1..9
            {
                D[i] = Int32.Parse(idno.Substring(i + 1, 1));
            }

            i = letter_weight[idno[0] - 'A'];
            sum = (i / 10) + (i % 10) * 9;
            sum = sum + 8 * D[0] + 7 * D[1] + 6 * D[2] + 5 * D[3]
                + 4 * D[4] + 3 * D[5] + 2 * D[6] + D[7] + D[8];
            return (sum % 10 == 0);
        }

        /// <summary>
        /// 判斷字串值是否為數字。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDigit(string s)
        {
            try
            {
                long num = Convert.ToInt64(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 將字串轉成位元組陣列。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [Obsolete("Use StringToBytes() instead.")]
        public static byte[] ToByteArray(string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>
        /// 將字串轉成位元組陣列。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enc">編碼。</param>
        /// <returns></returns>
        [Obsolete("Use StringToBytes() instead.")]
        public static byte[] ToByteArray(string str, Encoding enc)
        {
            return StringToBytes(str, enc);
        }

        public static byte[] StringToBytes(string str, Encoding enc = null)
        {
            if (enc == null)
            {
                enc = Encoding.UTF8;
            }
            return enc.GetBytes(str);
        }


        public static string BytesToString(byte[] bytes, Encoding enc = null)
        {
            if (enc == null)
            {
                enc = new System.Text.UTF8Encoding();
            }
            return enc.GetString(bytes);
        }

        /// <summary>
        /// Byte 陣列轉成字串。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Parse(byte[] bytes)
        {
            return BytesToString(bytes);
        }

        /// <summary>
        /// Convert a String to Boolean. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns true if input string is null, empty, or exists in the static property 'TrueStrings.'</returns>
        public static Boolean ToBoolean(string input)
        {
            return ToBoolean(input, _trueStrings);
        }

        /// <summary>
        /// Convert a String to Boolean. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns true if input string is null, empty, or exists in argument 'trueStrings.'</returns>
        public static Boolean ToBoolean(string input, HashSet<string> trueStrings)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            if (trueStrings == null)
            {
                throw new ArgumentException("Argument 'trueStrings' is NULL!");
            }

            if (trueStrings.Contains(input, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 大小寫無關的置換函式。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="newValue">要被置換的字串。</param>
        /// <param name="oldValue">置換的新字串。</param>
        /// <returns>A string</returns>
        public static string CaseInsenstiveReplace(string input,
           string oldValue, string newValue)
        {
            Regex regEx = new Regex(oldValue,
               RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(input, newValue);
        }

        /// <summary>
        /// 刪除空白以及多餘的結束字元（'\0'）。
        /// 某些 WinAPI 傳回的字元陣列轉成字串之後，後面會附加結束字元和垃圾資料，便可使用此函式。
        /// </summary>
        /// <param name="input">輸入字串</param>
        /// <returns></returns>
        public static string Trim(string s)
        {
            s = s.Trim();
            int i = s.IndexOf('\0');
            if (i >= 0)
            {
                return s.Substring(0, i);
            }
            return s;
        }

        /// <summary>
        /// 將字串中的所有全形空白轉成半形空白。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FullShapeSpaceToSpace(string input)
        {
            string replace = " ";
            string pattern = @"[　]";
            Regex regEx = new Regex(pattern, RegexOptions.Multiline);
            return regEx.Replace(input, replace);
        }

        /// <summary>
        /// 把所有空白字元轉換成 HTML 空白。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SpaceToNbsp(string input)
        {
            string space = " ";
            return input.Replace(" ", space);
        }

        /// <summary>
        /// 把所有換行符號 (\n) 和 (\r) 轉成 HTML 斷行標籤。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string NewLineToBreak(string input)
        {
            Regex regEx = new Regex(@"[\n|\r]+");
            return regEx.Replace(input, "<br />");
        }

        /// <summary>
        /// Wraps the passed string at the 
        /// at the next whitespace on or after the 
        /// total charCount has been reached
        /// for that line.  Uses the environment new line
        /// symbol for the break text.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of characters 
        /// per line.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input, int charCount)
        {
            return StrHelper.WordWrap(input, charCount,
               false, Environment.NewLine);
        }

        /// <summary>
        /// Wraps the passed string at the total 
        /// number of characters (if cuttOff is true)
        /// or at the next whitespace (if cutOff is false).
        /// Uses the environment new line
        /// symbol for the break text.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of characters 
        /// per line.</param>
        /// <param name="cutOff">If true, will break in 
        /// the middle of a word.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input,
           int charCount, bool cutOff)
        {
            return StrHelper.WordWrap(input, charCount,
               cutOff, Environment.NewLine);
        }

        /// <summary>
        /// Wraps the passed string at the total number 
        /// of characters (if cuttOff is true)
        /// or at the next whitespace (if cutOff is false).
        /// Uses the passed breakText
        /// for lineBreaks.
        /// </summary>
        /// <param name="input">The string to wrap.</param>
        /// <param name="charCount">The number of 
        /// characters per line.</param>
        /// <param name="cutOff">If true, will break in 
        /// the middle of a word.</param>
        /// <param name="breakText">The line break text to use.</param>
        /// <returns>A string.</returns>
        public static string WordWrap(string input, int charCount,
           bool cutOff, string breakText)
        {
            StringBuilder sb = new StringBuilder(input.Length + 100);
            int counter = 0;

            if (cutOff)
            {
                while (counter < input.Length)
                {
                    if (input.Length > counter + charCount)
                    {
                        sb.Append(input.Substring(counter, charCount));
                        sb.Append(breakText);
                    }
                    else
                    {
                        sb.Append(input.Substring(counter));
                    }
                    counter += charCount;
                }
            }
            else
            {
                string[] strings = input.Split(' ');
                for (int i = 0; i < strings.Length; i++)
                {
                    // added one to represent the space.
                    counter += strings[i].Length + 1;
                    if (i != 0 && counter > charCount)
                    {
                        sb.Append(breakText);
                        counter = 0;
                    }

                    sb.Append(strings[i] + ' ');
                }
            }
            // to get rid of the extra space at the end.
            return sb.ToString().TrimEnd();
        }

        /// <summary>
        /// 判斷傳入的字串是否為空字串。
        /// 注意：空白、全形空白、Tab 字元、和換行符號均視為「空」字元。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmpty(string input)
        {
            string s = StrHelper.RemoveAny(input, new char[] { '　', ' ', '\t', '\r', '\n', '\0' });
            if (String.IsNullOrEmpty(s))
                return true;
            return false;
        }

        #region 16 進制轉換

        /// <summary>
        /// 把內含 16 進位值的字串轉成 byte。
        /// </summary>
        /// <param name="input">內含 16 進位值的字串，例如：1F、AE。</param>
        /// <returns>byte 值。</returns>
        public static byte HexStrToByte(string s)
        {
            if (String.IsNullOrEmpty(s) || s.Length > 2)
                throw new ArgumentException("參數錯誤：不是合法的十六進位字串!");
            return byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// 將傳入的字串中的每個字元的數值轉成 16 進位字串，並組合成新的字串。例如: "12" => "3132"。
        /// </summary>
        /// <param name="s"></param>
        /// <returns>16 進位字串，每兩個字元代表一個 16 進位值。</returns>
        public static string ToHexString(string s)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                byte value = (byte)s[i];
                sb.Append(value.ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// 字串反轉。
        /// </summary>
        /// <param name="s"></param>
        /// <returns>反轉後的字串。</returns>
        public static string Reverse(string s)
        {
            if (String.IsNullOrEmpty(s))
                return "";
            char[] charArray = new char[s.Length];
            int len = s.Length - 1;
            for (int i = 0; i <= len; i++)
            {
                charArray[i] = s[len - i];
            }
            return new string(charArray);
        }

        /// <summary>
        /// Split input string to a list of key-value pairs. If order is not important, use SplitToDictionary() instead.
        /// </summary>
        /// <param name="input">輸入字串，常見的格式為 "key1=value1;key2=value2;..."。</param>
        /// <param name="itemSeparator">用來分隔每個 key-value 項目的字元。</param>
        /// <param name="keyValueSeparator">用來分隔 key 和 value 的字元。</param>
        /// <returns>Key-value pair 串列。</returns>
        public static List<KeyValuePair<string, string>> SplitToKeyValuePairs(string input,
            char itemSeparator, char keyValueSeparator)
        {
            List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

            string[] items = input.Split(new char[] { itemSeparator }, StringSplitOptions.RemoveEmptyEntries);
            string[] keyValue;
            KeyValuePair<string, string> pair;

            foreach (string item in items)
            {
                keyValue = item.Split(keyValueSeparator);
                if (keyValue.Length >= 2)
                {
                    pair = new KeyValuePair<string, string>(keyValue[0], keyValue[1]);
                    keyValues.Add(pair);
                }
                else if (keyValue.Length == 1)  // Just key?
                {
                    pair = new KeyValuePair<string, string>(keyValue[0], String.Empty);
                    keyValues.Add(pair);
                }
            }
            return keyValues;
        }

        /// <summary>
        /// Split input string to a list of key-value pairs. If order is important, use SplitToKeyValuePairs() instead.
        /// </summary>
        /// <param name="input">輸入字串，常見的格式為 "key1=value1;key2=value2;..."。</param>
        /// <param name="itemSeparator">用來分隔每個 key-value 項目的字元。</param>
        /// <param name="keyValueSeparator">用來分隔 key 和 value 的字元。</param>
        /// <returns>Key-value pair 串列。</returns>
        public static Dictionary<string, string> SplitToDictionary(string input, char itemSeparator, char keyValueSeparator)
        {
            var keyValues = new Dictionary<string, string>();

            string[] items = input.Split(new char[] { itemSeparator }, StringSplitOptions.RemoveEmptyEntries);
            string[] keyValue;

            foreach (string item in items)
            {
                keyValue = item.Split(keyValueSeparator);
                if (keyValue.Length >= 2)
                {
                    keyValues.Add(keyValue[0], keyValue[1]);
                }
                else if (keyValue.Length == 1)  // Just key?
                {
                    keyValues.Add(keyValue[0], String.Empty);
                }
            }
            return keyValues;
        }

        [Obsolete("Use SplitToDictionary() instead.")]
        public static Dictionary<string, string> ParseKeyValuePairs(string input, char itemSeparator, char keyValueSeparator = '=')
        {
            return SplitToDictionary(input, itemSeparator, keyValueSeparator);
        }

        /// <summary>
        /// 從指定的 XML 字串中取出特定元素。 
        /// </summary>
        /// <param name="xml">XML 字串。</param>
        /// <param name="element">元素名稱，不含左右括號（小於和大於符號）。</param>
        /// <returns></returns>
        public static string GetXmlElement(string xml, string element)
        {
            Match m;
            m = Regex.Match(xml, "<" + element + ">(?<Element>[^>]*)</" + element + ">", RegexOptions.IgnoreCase);
            if (m == null)
            {
                throw new Exception("在指定的 XML 字串中找不到 <" + element + "></" + element + "> 元素。");
            }
            return m.Groups["Element"].ToString();
        }


        /// <summary>
        /// Parses a delimited list of items into a string[].
        /// </summary>
        /// <param name="delimitedText">"1,2,3,4,5,6"</param>
        /// <param name="delimeter">分隔字元，例如：','</param>
        /// <returns></returns>
        public static string[] ToStringArray(string delimitedText, char delimeter)
        {
            if (string.IsNullOrEmpty(delimitedText))
                return null;

            string[] tokens = delimitedText.Split(delimeter);
            return tokens;
        }

        public static string EnsureEndWith(string input, string endingStr)
        {
            if (endingStr == null)
                throw new ArgumentNullException("endingStr");

            if (String.IsNullOrEmpty(input))
            {
                return endingStr;
            }

            if (input.EndsWith(endingStr))
            {
                return input;
            }
            return input + endingStr;
        }

        /// <summary>
        /// Remove the specified ending string. If the ending string contains only one character, consider using String.TrimEnd() instead.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="endingStr"></param>
        /// <returns></returns>
        public static string EnsureNotEndWith(string input, string endingStr)
        {
            if (input == null || endingStr == null)
                throw new ArgumentNullException("input or endingStr");

            if (input.EndsWith(endingStr))
            {
                int idx = input.LastIndexOf(endingStr);
                return input.Remove(idx);
            }
            return input;
        }

        public static string EnsureEndWithDirectorySeparator(string input)
        {
            return EnsureEndWith(input, System.IO.Path.DirectorySeparatorChar.ToString());
        }

        public static string Ellipsis(string input, int maxCharacters, string ellipsisText = null)
        {
            const string defaultEllipsis = "...";

            if (input == null || input.Length < maxCharacters)
            {
                return input;
            }
            if (String.IsNullOrEmpty(ellipsisText))
            {
                ellipsisText = defaultEllipsis;
            }
            return input.Substring(0, maxCharacters) + ellipsisText;
        }

        /// <summary>
        /// Encoding a URL. Basically the Path Component is encoded with Uri.EscapeUriString, and the Query Component is encoded with Uri.EscapeDataString.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlEncode(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            string uriNoQueryString = input;
            string queryString = String.Empty;
            int queryStringIndex = input.IndexOf('?');
            if (queryStringIndex >= 0)
            {
                queryString = input.Substring(queryStringIndex).TrimStart('?');
                uriNoQueryString = input.Remove(queryStringIndex);
            }

            // Parse query string to a name-value collection. The first '?' is removed and remained '?' characters will be encoded.
            var queryParams = SplitToKeyValuePairs(queryString, '&', '=');  // Do NOT use HttpUtility.ParseQueryString(aUri.Query) because it does encode.

            // Rebuilding and encoding query string.
            var sb = new StringBuilder();
            foreach (var item in queryParams)
            {
                sb.AppendFormat("{0}={1}&", Uri.EscapeDataString(item.Key), Uri.EscapeDataString(item.Value));
            }
            sb.Remove(sb.Length - 1, 1);  // Remove last '&'
            string encodedUriPath = Uri.EscapeUriString(uriNoQueryString);
            string result = String.Format("{0}?{1}", encodedUriPath, sb.ToString());
            return result;
        }

        /// <summary>
        /// Decoding a URL.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlDecode(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            // Since Uri.UnescapeDataString() does not decode plus sign ('+') to space character, we do it manually. 
            // Yes, System.Web.HttpUtility.UrlDecode() can do this, I just don't want to involve System.Web.dll here.
            return Uri.UnescapeDataString(input.Replace('+', ' '));
        }

        /// <summary>
        /// Extract text in paired brackets. Supports nested brackets and escaped bracket characters. <br />
        /// Two consecutive brackets means an escaped character.
        /// Example: <br/>
        /// "1{aa}2{bb}3" returns a list containing "aa" and "bb".
        /// "{aa{bb}cc}" returns a list containing "bb" and "aa{bb}cc".
        /// "{test \{nested\} bracket}" returns a list containing "test {nested} bracket" when argument nestedBrackets = True.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="leftBracket">Left bracket character.</param>
        /// <param name="rightBracket">Right brackey character.</param>
        /// <param name="keepBrackets">False means don't include bracket characters in the result text.</param>
        /// <param name="nestedBrackets">False means treat nested brackets as literal characters. True means also extract nested brackets.</param>
        /// <returns></returns>
        public static List<string> ExtractTextInBrackets(string input, char leftBracket = '{', char rightBracket = '}', 
            bool keepBrackets = false, bool nestedBrackets = false)
        {
            Stack<int> openingBraces = new Stack<int>();
            var result = new List<string>();

            if (String.IsNullOrWhiteSpace(input))
            {
                return result;
            }

            string leftBracketStr = leftBracket.ToString();
            string rightBracketStr = rightBracket.ToString();
            string escapedLeftBracket = new String(leftBracket, 2);
            string escapedRightBracket = new String(rightBracket, 2);

            char lastChar = '\0';
            for (int i = 0; i < input.Length; i++)
            {
                char currChar = input[i];
                char nextChar = '\0';
                if ((i + 1) < input.Length)
                {
                    nextChar = input[i + 1];
                }

                if (currChar == leftBracket && lastChar != leftBracket && nextChar != leftBracket)
                {
                    openingBraces.Push(i);
                }
                else if (currChar == rightBracket && lastChar != rightBracket && nextChar != rightBracket)
                {
                    if (openingBraces.Count > 0)
                    {
                        int beginIndex = openingBraces.Pop();
                        int endIndex = i;
                        if (!keepBrackets)
                        {
                            beginIndex++;
                            endIndex--;
                        }
                        // After Pop() is called, if there are items left in stack, then we have nested brackets. 
                        if (nestedBrackets || openingBraces.Count < 1)
                        {
                            string item = input.Substring(beginIndex, endIndex - beginIndex + 1)
                                .Replace(escapedLeftBracket, leftBracketStr)
                                .Replace(escapedRightBracket, rightBracketStr);
                            result.Add(item);
                        }
                    }
                }
                lastChar = currChar;
            }
            return result;
        }

        /// <summary>
        /// This method finds paired brackets and returns a list containting begin and end index of each bracket. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="leftBracket"></param>
        /// <param name="rightBracket"></param>
        /// <param name="nestedBrackets">False means treat nested brackets as literal characters. True means also extract nested brackets.</param>
        /// <returns>A list containting begin and end index of each bracket</returns>
        public static List<Tuple<int, int>> FindBracketPairs(string input, char leftBracket, char rightBracket, bool nestedBrackets = false)
        {
            Stack<int> openingBraces = new Stack<int>();
            var result = new List<Tuple<int, int>>();

            if (String.IsNullOrWhiteSpace(input))
            {
                return result;
            }

            string leftBracketStr = leftBracket.ToString();
            string rightBracketStr = rightBracket.ToString();
            string escapedLeftBracket = new String(leftBracket, 2);
            string escapedRightBracket = new String(rightBracket, 2);

            char lastChar = '\0';
            for (int i = 0; i < input.Length; i++)
            {
                char currChar = input[i];
                char nextChar = '\0';
                if ((i + 1) < input.Length)
                {
                    nextChar = input[i + 1];
                }

                if (currChar == leftBracket && lastChar != leftBracket && nextChar != leftBracket)
                {
                    openingBraces.Push(i);
                }
                else if (currChar == rightBracket && lastChar != rightBracket && nextChar != rightBracket)
                {
                    if (openingBraces.Count > 0)
                    {
                        int beginIndex = openingBraces.Pop();
                        int endIndex = i;
                        // After Pop() is called, if there are items left in stack, then we have nested brackets. 
                        if (nestedBrackets || openingBraces.Count < 1)
                        {
                            result.Add(new Tuple<int, int>(beginIndex, endIndex));
                        }
                    }
                }
                lastChar = currChar;
            }
            return result;
        }
    }

}
