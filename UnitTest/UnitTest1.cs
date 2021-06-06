using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Airport.Classes;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PositiveTest()
        {
            string positivePassword = "arkov632";
            Assert.AreEqual(true, Manager.IsValidLogin(positivePassword));
        }
        [TestMethod]
        public void NegativeTest()
        {
            string negativePassword = "arkov";
            Assert.AreEqual(true, Manager.IsValidLogin(negativePassword));
        }
    }
}
