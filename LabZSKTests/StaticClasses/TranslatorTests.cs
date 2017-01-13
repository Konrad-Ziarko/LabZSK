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
    }
}