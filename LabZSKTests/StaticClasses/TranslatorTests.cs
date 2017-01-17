using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabZSK.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LabZSK.StaticClasses.Tests
{
    [TestClass()]
    public class TranslatorTests
    {
        [TestMethod()]
        public void GetMicroOpDescriptionTest()
        {
            // arrange
            string mnemo = "IRBP";
            string expected = "BUS -> RBP";
            // act
            string actual = Translator.GetMicroOpDescription(mnemo);
            // assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void GetMicroOpDescriptionTest2()
        {
            // arrange
            string mnemo = "";
            // act
            string actual = Translator.GetMicroOpDescription(mnemo);
            // assert
            Assert.IsTrue(string.IsNullOrEmpty(actual));
        }

        [TestMethod()]
        public void GetMicroOpExtendedDescriptionTest()
        {
            Assert.IsTrue(Translator.GetMicroOpExtendedDescription("ORR").Contains("RR -> BUS"));
        }
        [TestMethod()]
        public void GetMicroOpExtendedDescriptionTest2()
        {
            Assert.IsTrue(Translator.GetMicroOpExtendedDescription("ILK").Contains("BUS -> LK"));
        }
        [TestMethod()]
        public void GetInsrtuctionDescriptionTest()
        {
            Assert.IsTrue(Translator.GetInsrtuctionDescription("ADS").Equals("Dodawanie"));
        }

        [TestMethod()]
        public void GetInsrtuctionDescriptionTest1()
        {
            Assert.IsTrue(Translator.GetInsrtuctionDescription("SUS").Equals("Odejmowanie"));
        }

        [TestMethod()]
        public void GetBytesTest()
        {
            int len;
            Translator.GetBytes("abc", out len);
            Assert.IsTrue(len==6);
        }
        [TestMethod()]
        public void GetBytesTest2()
        {
            int len;
            Translator.GetBytes("abcde", out len);
            Assert.IsTrue(len == 10);
        }
        [TestMethod()]
        public void GetBytesTest3()
        {
            int len;
            Translator.GetBytes("123456789", out len);
            Assert.IsTrue(len == 18);
        }
        [TestMethod()]
        public void GetBytesTest4()
        {
            int len;
            Translator.GetBytes("", out len);
            Assert.IsTrue(len == 0);
        }

    }
}