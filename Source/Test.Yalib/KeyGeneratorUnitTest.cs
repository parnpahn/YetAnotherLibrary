using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yalib;

namespace Test.Yalib
{
    [TestClass]
    public class KeyGeneratorUnitTest
    {
        /// <summary>
        /// When we run a unit test, we automatically get an instance of TestContext. Thus, we dob't need to explicitly instantiate it.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestRandomNumber()
        {
            TestContext.WriteLine("KeyGenerator.NewUniqueId(16)");
            for (int i = 0; i < 10; i++)
            {
                TestContext.WriteLine(KeyGenerator.NewUniqueId(16));
            }

            TestContext.WriteLine("=================================================");
            int digits = 8;            
            TestContext.WriteLine("KeyGenerator.NewRandomNumberString({0})", digits);
            string s;
            for (int i = 0; i < 10; i++)
            {
                s = KeyGenerator.NewRandomNumberString(digits);
                TestContext.WriteLine(s);
            }

            Assert.AreEqual(1, 1);
        }
    }
}
