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
            MemoryRecord record = new MemoryRecord(0, "0000100000000101", "805", 1);
            Assert.IsTrue(record.DA.Equals("00000101") && record.OP.Equals("00001") && record.XSI.Equals("000"));
        }
    }
}