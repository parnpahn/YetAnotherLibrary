using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yalib;
using System.Collections.Generic;

namespace Test.Yalib
{
    [TestClass]
    public class StrHelperUnitTest
    {
        public TestContext TestContext { get; set; }

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

        [TestMethod]
        public void TestConversion()
        {
            TestContext.WriteLine("StrHelper.TrueStrings = {0}", StrHelper.TrueStrings);
            Assert.AreEqual(StrHelper.ToBoolean("y"), true);
            Assert.AreEqual(StrHelper.ToBoolean("Yes"), true);
            Assert.AreEqual(StrHelper.ToBoolean("true"), true);
            Assert.AreEqual(StrHelper.ToBoolean("1"), true);
            Assert.AreEqual(StrHelper.ToBoolean("enabled"), true);
            Assert.AreEqual(StrHelper.ToBoolean("N"), false);
            Assert.AreEqual(StrHelper.ToBoolean("qoo"), false);
        }

        [TestMethod]
        public void TestBracket()
        {
            string input = " {} {0:###} {test {nested} text} {--{{test escaped parenthesis}}--}";
            var expected = new List<string>
            {
                "",
                "0:###",
                "nested",
                "test {nested} text",
                "--{test escaped parenthesis}--"
            };

            var result = StrHelper.ExtractTextInBrackets(input, '{', '}', false, true);
            Assert.AreEqual(result.Count, expected.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(result[i], expected[i]);
            }

            var result2 = StrHelper.ExtractTextInBrackets(input, '{', '}', false, false);
            Assert.AreEqual(result2.Count, expected.Count-1);
            Assert.AreEqual(result2[2], expected[3]);
        }
    }
}
