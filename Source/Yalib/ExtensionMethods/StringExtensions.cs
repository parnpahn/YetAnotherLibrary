using System;
using System.Collections.Generic;
using System.Text;

namespace Yalib
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Reverses a string.
        /// </summary>
        /// <param name = "input">The string to be reversed.</param>
        /// <returns>The reversed string</returns>
        public static string Reverse(this string input)
        {
            if (String.IsNullOrWhiteSpace(input) || (input.Length == 1))
            {
                return input;
            }

            var chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// Returns the left part of the string.
        /// </summary>
        /// <param name="input">The original string.</param>
        /// <param name="characterCount">The character count to be returned.</param>
        /// <returns>The left part</returns>
        public static string Left(this string input, int characterCount)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (characterCount >= input.Length)
                throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
            return input.Substring(0, characterCount);
        }

        /// <summary>
        /// Returns the Right part of the string.
        /// </summary>
        /// <param name="input">The original string.</param>
        /// <param name="characterCount">The character count to be returned.</param>
        /// <returns>The right part</returns>
        public static string Right(this string input, int characterCount)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (characterCount >= input.Length)
                throw new ArgumentOutOfRangeException("characterCount", characterCount, "characterCount must be less than length of string");
            return input.Substring(input.Length - characterCount);
        }

        public static string EnsureEndWith(this string input, string endingStr)
        {
            if (endingStr == null)
                throw new ArgumentNullException("endingStr");
            if (input.EndsWith(endingStr))
            {
                return input;
            }
            return input + endingStr;
        }

        public static string EnsureEndWithDirectorySeparator(this string input)
        {
            return input.EnsureEndWith(System.IO.Path.DirectorySeparatorChar.ToString());
        }

        #region To X conversions

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="input">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string input, bool ignorecase)
        {
            if (input == null)
                throw new ArgumentNullException("Value");

            input = input.Trim();

            if (input.Length == 0)
                throw new ArgumentNullException("Must specify valid information for parsing in the string.", "value");

            Type t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            return (T)Enum.Parse(t, input, ignorecase);
        }

        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int ToInteger(this string input, int defaultvalue)
        {
            return (int)ToDouble(input, defaultvalue);
        }

        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToInteger(this string value)
        {
            return ToInteger(value, 0);
        }

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double ToDouble(this string input, double defaultvalue)
        {
            double result;
            if (double.TryParse(input, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <returns></returns>
        public static double ToDouble(this string input)
        {
            return ToDouble(input, 0);
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string input, DateTime? defaultvalue)
        {
            DateTime result;
            if (DateTime.TryParse(input, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="input">The value.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string input)
        {
            return ToDateTime(input, null);
        }

        /// <summary>
        /// Converts a string value to bool value, supports "T" and "F" conversions.
        /// </summary>
        /// <param name="input">The string value.</param>
        /// <returns>A bool based on the string value</returns>
        public static bool? ToBoolean(this string input)
        {
            if (string.Compare("T", input, true) == 0)
            {
                return true;
            }
            if (string.Compare("F", input, true) == 0)
            {
                return false;
            }
            bool result;
            if (bool.TryParse(input, out result))
            {
                return result;
            }
            else return null;
        }

        #endregion To X conversions

        #region Validation methods

        /// <summary>
        /// Determines whether it is a valid URL.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUrl(this string text)
        {
            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return rx.IsMatch(text);
        }

        /// <summary>
        /// Determines whether it is a valid email address
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmail(this string email)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }

        #endregion Validation methods

        public static string Ellipsis(this string input, int maxCharacters, string ellipsisText = null)
        {
            return StrHelper.Ellipsis(input, maxCharacters, ellipsisText);
        }

        /// <summary>
        /// This method is suited for encoding query string of a URL, but not for the entire URL. It uses Uri.EscapeDataString() to encode the input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UrlEncodeQueryString(this string queryString)
        {
            return Uri.EscapeDataString(queryString);
        }

        /// <summary>
        /// This method is suited for decoding query string of a URL, but not for the entire URL. It uses Uri.UnescapeDataString() to decode the input string.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static string UrlDecodeQueryString(this string queryString)
        {
            return Uri.UnescapeDataString(queryString);
        }
    }
}