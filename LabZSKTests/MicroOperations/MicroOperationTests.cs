using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabZSK.MicroOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabZSK.MicroOperations.Tests
{
    [TestClass()]
    public class MicroOperationTests
    {
        [TestMethod()]
        public void getColumnTest()
        {
            MicroOperation microOp = new MicroOperation(0);
            Assert.IsTrue(microOp.addr == "0" && microOp.getColumn(1) == "");
        }
        [TestMethod()]
        public void getColumnTest2()
        {
            MicroOperation microOp = new MicroOperation(0);
            Assert.IsTrue(microOp.getColumn(99) == "");
        }
        [TestMethod()]
        public void getColumnTest3()
        {
            MicroOperation microOp = new MicroOperation(5, "IXRE", "IRAP","","","","","","","","","255");
            Assert.IsTrue(microOp.addr == "5" && microOp.getColumn(1) == "IXRE" && microOp.getColumn(5) == "");
        }
        [TestMethod()]
        public void getColumnTest4()
        {
            MicroOperation microOp = new MicroOperation(99, "", "IRAP", "IRAE", "", "", "", "", "", "", "", "1");
            Assert.IsTrue(microOp.addr == "99" && microOp.getColumn(2) == "IRAP" && microOp.getColumn(3) == "IRAE");
        }
        [TestMethod()]
        public void getColumnNameTest()
        {
            MicroOperation microOp = new MicroOperation(1, "IXRE", "IRAP", "IRAE", "", "", "", "", "", "", "", "5");
            Assert.IsTrue(microOp.addr == "1" && microOp.getColumnName(1) == "S1");
        }
        [TestMethod()]
        public void getColumnNameTest2()
        {
            MicroOperation microOp = new MicroOperation(8);
            Assert.IsTrue(microOp.getColumnName(3) == "S2" && microOp.getColumnName(4) == "D2");
        }
        [TestMethod()]
        public void getColumnTest6()
        {
            MicroOperation microOp = new MicroOperation(0);
            Assert.IsTrue(microOp.addr == "0" && microOp.getColumn(1) == "");
        }
        [TestMethod()]
        public void getColumnTest7()
        {
            MicroOperation microOp = new MicroOperation(0);
            Assert.IsTrue(microOp.getColumn(99) == "");
        }
        [TestMethod()]
        public void getColumnTest5()
        {
            MicroOperation microOp = new MicroOperation(5, "IXRE", "IRAP", "", "", "", "", "", "", "", "", "255");
            Assert.IsTrue(microOp.addr == "5" && microOp.getColumn(1) == "IXRE" && microOp.getColumn(5) == "");
        }
        [TestMethod()]
        public void getColumnTest1()
        {
            MicroOperation microOp = new MicroOperation(99, "", "IRAP", "IRAE", "", "", "", "", "", "", "", "1");
            Assert.IsTrue(microOp.addr == "99" && microOp.getColumn(2) == "IRAP" && microOp.getColumn(3) == "IRAE");
        }
        [TestMethod()]
        public void getColumnNameTest3()
        {
            MicroOperation microOp = new MicroOperation(1, "IXRE", "IRAP", "IRAE", "", "", "", "", "", "", "", "5");
            Assert.IsTrue(microOp.addr == "1" && microOp.getColumnName(1) == "S1");
        }
        [TestMethod()]
        public void getColumnNameTest4()
        {
            MicroOperation microOp = new MicroOperation(8);
            Assert.IsTrue(microOp.getColumnName(3) == "S2" && microOp.getColumnName(4) == "D2");
        }
    }
}