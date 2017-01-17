using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabZSK.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace LabZSK.Memory.Tests
{
    [TestClass()]
    public class MemoryRecordTests
    {
        [TestMethod()]
        public void MemoryRecordTest()
        {
            MemoryRecord record = new MemoryRecord(0, "0000100000000101", "805", 2);
            Assert.IsTrue(record.DA.Equals("00000101") && record.OP.Equals("00001") && record.XSI.Equals("000"));
        }
        [TestMethod()]
        public void MemoryRecordTest2()
        {
            MemoryRecord record = new MemoryRecord(0, "0000000000000000", "0", 3);
            Assert.IsTrue(record.isComplex && record.N.Equals("0000000"));
        }
        [TestMethod()]
        public void MemoryRecordTest3()
        {
            MemoryRecord record = new MemoryRecord(5, "0000000000000001", "0", 3);
            Assert.IsTrue(record.isComplex && record.N.Equals("0000001"));
        }
        [TestMethod()]
        public void MemoryRecordTest4()
        {
            MemoryRecord record = new MemoryRecord(0, "0010100001001101", "284D", 2);
            Assert.IsTrue(record.DA.Equals("01001101") && record.OP.Equals("00101") && record.XSI.Equals("000"));
        }
        [TestMethod()]
        public void MemoryRecordTest5()
        {
            MemoryRecord record = new MemoryRecord(0, "0000100000000101", "805", 2);
            Assert.IsFalse(record.isComplex);
        }
    }
}