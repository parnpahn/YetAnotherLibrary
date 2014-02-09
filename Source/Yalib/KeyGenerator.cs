using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yalib
{
    public static class KeyGenerator
    {
        /// <summary>
        /// Steal from Westwind.Utilities.DataUtils class.
        /// Generates a unique Id as a string of up to 16 characters.
        /// Based on a GUID and the size takes that subset of a the
        /// Guid's 16 bytes to create a string id.
        /// 
        /// String Id contains numbers and lower case alpha chars 36 total.
        /// 
        /// Sizes: 6 gives roughly 99.97% uniqueness. 
        ///        8 gives less than 1 in a million doubles.
        ///        16 will give full GUID strength uniqueness
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <summary>
        public static string NewUniqueId(int length = 16)
        {
            string chars = "abcdefghijkmnopqrstuvwxyz1234567890";
            StringBuilder result = new StringBuilder(length);
            int count = 0;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                result.Append(chars[b % (chars.Length - 1)]);
                count++;
                if (count >= length)
                    return result.ToString();
            }
            return result.ToString();
        }

        public static long NewUniqueNumber()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(bytes, 0);
        }

        private static Random _rnd = new Random((int)DateTime.Now.Ticks);

        public static int NewRandomNumber(int min, int max)
        {
            return _rnd.Next(min, max + 1);
        }

        public static string NewRandomNumberString(int digits)
        {
            if (digits < 1 || digits > 19)  // Int64.Max is 19 digits.
            {
                throw new ArgumentException("Invalid argument value: digits=" + digits.ToString());
            }
            long num = NewUniqueNumber();
            string s = num.ToString().PadLeft(digits, '0');
            return s.Right(digits);
        }
    }
}
