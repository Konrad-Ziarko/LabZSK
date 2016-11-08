using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabZSK.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void GetBytesTest()
        {
            // arrange
            string testString = "213eqwsad";
            int expectedLength = 18;
            byte[] expected = { 50, 0, 49, 0, 51, 0, 101, 0, 113, 0, 119, 0, 115, 0, 97, 0, 100, 0 };
            // act
            int actualLength;
            byte[] actual = Translator.GetBytes(testString, out actualLength);
            // assert
            Assert.AreEqual(expectedLength, actualLength);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}