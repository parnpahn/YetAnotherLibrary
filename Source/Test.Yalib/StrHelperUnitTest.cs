using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yalib;

namespace Test.Yalib
{
    [TestClass]
    public class StrHelperUnitTest
    {
        [TestMethod]
        public void TestUrlEncode()
        {
            string input = "http://xyz.com/test?tel=+1732123456&name=M. Tsai";
            string expected = "http://xyz.com/test?tel=%2B1732123456&name=M.%20Tsai";
            string encoded = StrHelper.UrlEncode(input);

            Assert.AreEqual(encoded, expected, true);

            // decode
            string decoded = StrHelper.UrlDecode(encoded);
            Assert.AreEqual(decoded, input, true);

            // test decoding '+' to space characters.
            decoded = StrHelper.UrlDecode("How+are+you");
            Assert.AreEqual(decoded, "How are you", true);

            // test double encoding then decoding.
            encoded = StrHelper.UrlEncode(StrHelper.UrlEncode(input));
            decoded = StrHelper.UrlDecode(StrHelper.UrlDecode(encoded));
            Assert.AreEqual(decoded, input);
        }
    }
}
