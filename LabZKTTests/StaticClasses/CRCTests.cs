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
    public class CRCTests
    {
        [TestMethod()]
        public void ComputeChecksumTest()
        {
            // arrange
            byte[] test = { 50, 21, 45, 0, 12, 5, 78, 134, 6, 246, 253, 35, 78, 34, 35, 65, 4, 36, 45, 23, 234, 8 };
            uint expected = 49726077;
            // act
            uint actual = CRC.ComputeChecksum(test);
            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ComputeChecksumTest1()
        {
            // arrange
            byte[] test = { 12, 86, 75, 4, 1, 68, 34, 250, 101, 78, 25, 99, 18, 93, 203, 84, 186, 189, 7, 33, 38, 55 };
            uint expected = 108763321;
            // act
            uint actual = CRC.ComputeChecksum(test);
            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}