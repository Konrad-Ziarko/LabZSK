using LabZSK.Memory;
using LabZSK.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZSK.Simulation
{
    partial class SimView
    {
        private bool getRRRegisterOP(out short a)
        {
            string tmp = Convert.ToString(registers["RR"].innerValue, 2).PadLeft(16, '0');
            if (tmp.Substring(0, 5) == "00000")
            {
                a = (short)(Convert.ToInt16(tmp.Substring(5, 4), 2) + 32);
                return true;
            }
            else if (tmp.Substring(0, 5) != "00000" && tmp.Substring(7, 1) == "1")
            {
                a = Convert.ToInt16(tmp.Substring(0, 5), 2);
                return true;
            }
            else
            {
                a = Convert.ToInt16(tmp.Substring(0, 5), 2);
                return false;
            }
        }
        #region Instruction Execution
        private void exeTest()
        {
            Grid_PM.CurrentCell = Grid_PM[9, raps];
            microOpMnemo = Grid_PM[9, raps].Value.ToString();
            bool otherValue = false;
            if (microOpMnemo == "UNB")
                isTestPositive = true;
            else if (microOpMnemo == "TINT")
            {
                if (flags["INT"].innerValue == 0)
                    isTestPositive = true;
                else
                {
                    otherValue = true;
                    flags["INT"].setInnerValue(0);
                    //czy tak test TINT?
                    registers["RAP"].setActualValue(255);
                    registers["RAP"].setNeedCheck(out registerToCheck);
                    button_OK.Visible = true;
                    EnDisableButtons();
                    registers[registerToCheck].Focus();
                    waitForButton();
                    buttonOKClicked = false;
                    EnDisableButtons();
                    registers["RAPS"].setActualValue(254);
                }
            }
            else if (microOpMnemo == "TIND")
            {
                short a;
                bool indirectAdresation = getRRRegisterOP(out a);
                if (indirectAdresation)
                {
                    isTestPositive = true;
                }
                else
                {
                    otherValue = true;
                    registers["RAPS"].setActualValue(a);
                }
            }
            else if (microOpMnemo == "TAS")
            {
                if (registers["A"].innerValue >= 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TXS")
            {
                if (registers["RI"].innerValue >= 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TQ15")
            {
                if ((registers["MQ"].innerValue & 0x0001) != 0x0001)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TLK")
            {
                short lk = registers["LK"].innerValue;
                if (Grid_PM.Rows[raps].Cells[7].Value.ToString() == "SHT")
                {
                    if (registers["LK"].innerValue == 0)
                        isTestPositive = true;
                }
                else
                {
                    if (registers["LK"].innerValue != 0)
                        isTestPositive = true;
                }
            }
            else if (microOpMnemo == "TSD")
            {
                if (flags["ZNAK"].innerValue == 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TAO")
            {
                if (flags["OFF"].innerValue == 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TXP")
            {
                if (registers["RI"].innerValue < 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TXZ")
            {
                //co tu porównać?
            }
            else if (microOpMnemo == "TXRO")
            {
                if (flags["XRO"].innerValue == 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TAP")
            {
                if (registers["A"].innerValue < 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TAZ")
            {
                if (registers["A"].innerValue == 0)
                    isTestPositive = true;
            }
            cells[9, 7] = false;
            AddToLogAndMiniLog("TEST", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));

            if (isTestPositive)
            {
                registers["RAPS"].setActualValue(na);
                isTestPositive = false;
            }
            else if (!otherValue)
            {
                //czy zerować raps jak overflow?
                registers["RAPS"].setActualValue((short)((registers["RAPS"].innerValue + 1) & 255));
            }
            registers["RAPS"].setNeedCheck(out registerToCheck);

            Grid_PM.CurrentCell = Grid_PM[11, registers["RAPS"].innerValue];

            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            waitForButton();
            buttonOKClicked = false;
            if (registerToCheck != "")
                EnDisableButtons();
            currentTact = 9;
        }
        private void exeTact7()
        {
            if (cells[8, 7])
            {
                Grid_PM.CurrentCell = Grid_PM[8, raps];
                microOpMnemo = Grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "RA")
                    testAndSet("A", 0);
                else if (microOpMnemo == "RMQ")
                    testAndSet("MQ", 0);
                else if (microOpMnemo == "RINT")
                    flags["INT"].setInnerValue(0);
                else if (microOpMnemo == "ENI")
                    flags["INT"].setInnerValue(1);
                else if (microOpMnemo == "OPC")
                {
                    short a;
                    bool isIndirect = getRRRegisterOP(out a);
                    testAndSet("RAPS", a);
                    currentTact = 8;
                }
                cells[8, 7] = false;
                AddToLogAndMiniLog("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[5, 7])
            {
                Grid_PM.CurrentCell = Grid_PM[5, raps];
                microOpMnemo = Grid_PM[5, raps].Value.ToString();
                if (microOpMnemo == "ORI")
                    testAndSet("BUS", registers["RI"].innerValue);
                else if (microOpMnemo == "ORAE")
                    testAndSet("BUS", registers["RAE"].innerValue);
                else if (microOpMnemo == "OXE")
                    testAndSet("RALU", registers["X"].innerValue);
                else if (microOpMnemo == "OA")
                    testAndSet("BUS", registers["A"].innerValue);
                else if (microOpMnemo == "OMQ")
                    testAndSet("BUS", registers["MQ"].innerValue);
                else if (microOpMnemo == "OLR")
                    testAndSet("BUS", registers["LR"].innerValue);
                else if (microOpMnemo == "ORBP")
                    testAndSet("BUS", registers["RBP"].innerValue);
                cells[5, 7] = false;
                AddToLogAndMiniLog("S3", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[6, 7])
            {
                Grid_PM.CurrentCell = Grid_PM[6, raps];
                microOpMnemo = Grid_PM[6, raps].Value.ToString();
                if (microOpMnemo == "OXE")
                    testAndSet("RALU", registers["X"].innerValue);
                else if (microOpMnemo == "ILR")
                    testAndSet("LR", registers["BUS"].innerValue);
                else if (microOpMnemo == "IX")
                    testAndSet("X", registers["BUS"].innerValue);
                else if (microOpMnemo == "IBE")
                    testAndSet("RALU", registers["BUS"].innerValue);
                else if (microOpMnemo == "IBI")
                    testAndSet("RAE", registers["BUS"].innerValue);
                else if (microOpMnemo == "IA")
                    testAndSet("A", registers["BUS"].innerValue);
                else if (microOpMnemo == "IMQ")
                    testAndSet("MQ", registers["BUS"].innerValue);
                else if (microOpMnemo == "NSI")
                    testAndSet("LR", (short)(registers["LR"].innerValue + 1));
                else if (microOpMnemo == "IAS")
                {
                    if ((registers["A"].innerValue & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                else if (microOpMnemo == "SGN")
                {
                    if ((registers["X"].innerValue & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                else if (microOpMnemo == "IX")
                    testAndSet("X", registers["BUS"].innerValue);
                else if (microOpMnemo == "IRI")
                    testAndSet("RI", registers["BUS"].innerValue);
                else if (microOpMnemo == "IRR")
                    testAndSet("RR", registers["BUS"].innerValue);
                else if (microOpMnemo == "IRBP")
                    testAndSet("RBP", registers["BUS"].innerValue);
                else if (microOpMnemo == "SRBP")
                    testAndSet("RBP", registers["BUS"].innerValue);
                cells[6, 7] = false;
                resetBus = true;
                AddToLogAndMiniLog("D3", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[7, 7])
            {
                Grid_PM.CurrentCell = Grid_PM[7, raps];
                microOpMnemo = Grid_PM[7, raps].Value.ToString();
                if (microOpMnemo == "END")
                    testAndSet("RAPS", 0);
                else if (microOpMnemo == "CWC")
                {
                    flags["MAV"].setInnerValue(0);
                    flags["IA"].setInnerValue(1);
                    MemoryRecord mr = new MemoryRecord(registers["RAP"].innerValue, Convert.ToString(registers["RBP"].innerValue, 2).PadLeft(16, '0'), Convert.ToString(registers["RBP"].innerValue, 16).PadLeft(4, '0'), 1);
                    List_Memory[registers["RAP"].innerValue] = mr;
                    Grid_Mem.Rows[registers["RAP"].innerValue].Cells[0].Value = mr.addr;
                    Grid_Mem.Rows[registers["RAP"].innerValue].Cells[1].Value = mr.value;
                    Grid_Mem.Rows[registers["RAP"].innerValue].Cells[2].Value = mr.hex;
                    Grid_Mem.FirstDisplayedScrollingRowIndex = registers["RAP"].innerValue;
                }
                else if (microOpMnemo == "IWC")
                {
                    flags["MAV"].setInnerValue(0);
                    flags["IA"].setInnerValue(1);
                    registers["RAP"].setInnerAndExpectedValue(255);
                    MemoryRecord mr = new MemoryRecord(255, Convert.ToString(registers["LR"].innerValue, 2).PadLeft(16, '0'), Convert.ToString(registers["LR"].innerValue, 16).PadLeft(4, '0'), 1);
                    List_Memory[255] = mr;
                    Grid_Mem.Rows[255].Cells[0].Value = mr.addr;
                    Grid_Mem.Rows[255].Cells[1].Value = mr.value;
                    Grid_Mem.Rows[255].Cells[2].Value = mr.hex;
                    Grid_Mem.FirstDisplayedScrollingRowIndex = 255;
                    registerToCheck = "RAP";
                }

                //skip TEST if END is present
                if (microOpMnemo == "END")
                    currentTact = 9;
                cells[7, 7] = false;
                AddToLogAndMiniLog("C1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            waitForButton();
            buttonOKClicked = false;
            if (resetBus)
            {
                registers["BUS"].setInnerAndExpectedValue(0);
                resetBus = false;
            }
            if (registerToCheck != "")
                EnDisableButtons();
        }
        private void exeTact6()
        {
            if (cells[3, 6])
            {
                Grid_PM.CurrentCell = Grid_PM[3, raps];
                microOpMnemo = Grid_PM[3, raps].Value.ToString();
                if (microOpMnemo == "IXRE")
                    testAndSet("LALU", registers["RI"].innerValue);
                else if (microOpMnemo == "ORR")
                    testAndSet("BUS", registers["RR"].innerValue);
                else if (microOpMnemo == "ORI")
                    testAndSet("BUS", registers["RI"].innerValue);
                else if (microOpMnemo == "OBE")
                    testAndSet("BUS", registers["ALU"].innerValue);
                else if (microOpMnemo == "IRAE")
                    testAndSet("RAE", registers["SUMA"].innerValue);
                else if (microOpMnemo == "ORAE")
                    testAndSet("BUS", registers["RAE"].innerValue);
                else if (microOpMnemo == "IALU")
                    testAndSet("LALU", registers["A"].innerValue);
                else if (microOpMnemo == "OXE")
                    testAndSet("RALU", registers["X"].innerValue);
                else if (microOpMnemo == "OX")
                    testAndSet("BUS", registers["X"].innerValue);
                else if (microOpMnemo == "OA")
                    testAndSet("BUS", registers["A"].innerValue);
                else if (microOpMnemo == "OMQ")
                    testAndSet("BUS", registers["MQ"].innerValue);
                cells[3, 6] = false;
                AddToLogAndMiniLog("S2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[4, 6])
            {
                Grid_PM.CurrentCell = Grid_PM[4, raps];
                microOpMnemo = Grid_PM[4, raps].Value.ToString();
                if (microOpMnemo == "ORI")
                    testAndSet("BUS", registers["RI"].innerValue);
                else if (microOpMnemo == "OXE")
                    testAndSet("RALU", registers["X"].innerValue);
                else if (microOpMnemo == "ILR")
                    testAndSet("LR", registers["BUS"].innerValue);
                else if (microOpMnemo == "IRI")
                    testAndSet("RI", registers["BUS"].innerValue);
                else if (microOpMnemo == "IX")
                    testAndSet("X", registers["BUS"].innerValue);
                else if (microOpMnemo == "IBE")
                    testAndSet("RALU", registers["BUS"].innerValue);
                else if (microOpMnemo == "IBI")
                    testAndSet("RAE", registers["BUS"].innerValue);
                else if (microOpMnemo == "IA")
                    testAndSet("A", registers["BUS"].innerValue);
                else if (microOpMnemo == "IMQ")
                    testAndSet("MQ", registers["BUS"].innerValue);
                else if (microOpMnemo == "NSI")
                    testAndSet("LR", (short)(registers["LR"].innerValue + 1));
                else if (microOpMnemo == "IAS")
                {
                    if ((registers["A"].innerValue & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                else if (microOpMnemo == "SGN")
                {
                    if ((registers["X"].innerValue & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                cells[4, 6] = false;
                resetBus = true;
                AddToLogAndMiniLog("D2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[8, 6])
            {
                Grid_PM.CurrentCell = Grid_PM[8, raps];
                microOpMnemo = Grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "DLK")
                    testAndSet("LK", (short)(registers["LK"].innerValue - 1));
                else if (microOpMnemo == "DRI")
                    testAndSet("RI", (short)(registers["RI"].innerValue - 1));
                else if (microOpMnemo == "SOFF")
                    flags["OFF"].setInnerValue(1);
                else if (microOpMnemo == "ROFF")
                    flags["OFF"].setInnerValue(0);
                else if (microOpMnemo == "SXRO")
                    flags["XRO"].setInnerValue(1);
                else if (microOpMnemo == "RXRO")
                    flags["XRO"].setInnerValue(0);
                else if (microOpMnemo == "AQ15")
                {
                    if ((registers["A"].innerValue & 0x8000) == 0x8000)
                        testAndSet("MQ", (short)(registers["MQ"].innerValue & 0xFFFE));
                    else
                        testAndSet("MQ", (short)(registers["MQ"].innerValue | 0x0001));
                }
                cells[8, 6] = false;
                AddToLogAndMiniLog("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            waitForButton();
            if (resetBus)
            {
                registers["BUS"].setInnerAndExpectedValue(0);
                resetBus = false;
            }
            buttonOKClicked = false;
            if (registerToCheck != "")
                EnDisableButtons();
        }
        private void exeTact2()
        {
            if (cells[10, 2])
            {
                Grid_PM.CurrentCell = Grid_PM[10, raps];
                microOpMnemo = Grid_PM[10, raps].Value.ToString();
                isOverflow = false;
                if (microOpMnemo == "ADS")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].innerValue + registers["RALU"].innerValue);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    testAndSet("ALU", (short)(registers["LALU"].innerValue + registers["RALU"].innerValue));
                }
                else if (microOpMnemo == "SUS")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].innerValue - registers["RALU"].innerValue);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    testAndSet("ALU", (short)(registers["LALU"].innerValue - registers["RALU"].innerValue));
                }
                else if (microOpMnemo == "CMX")
                    testAndSet("ALU", (short)(1 + ~registers["RALU"].innerValue));
                else if (microOpMnemo == "CMA")
                    testAndSet("ALU", (short)(1 + ~registers["LALU"].innerValue));
                else if (microOpMnemo == "OR")
                    testAndSet("ALU", (short)(registers["LALU"].innerValue | registers["RALU"].innerValue));
                else if (microOpMnemo == "AND")
                    testAndSet("ALU", (short)(registers["LALU"].innerValue & registers["RALU"].innerValue));
                else if (microOpMnemo == "EOR")
                    testAndSet("ALU", (short)(registers["LALU"].innerValue ^ registers["RALU"].innerValue));
                else if (microOpMnemo == "NOTL")
                    testAndSet("ALU", (short)(~registers["LALU"].innerValue));
                else if (microOpMnemo == "NOTR")
                    testAndSet("ALU", (short)(~registers["RALU"].innerValue));
                else if (microOpMnemo == "L")
                    testAndSet("ALU", registers["LALU"].innerValue);
                else if (microOpMnemo == "R")
                    testAndSet("ALU", registers["RALU"].innerValue);
                else if (microOpMnemo == "INCL")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].innerValue + 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    testAndSet("ALU", (short)(registers["LALU"].innerValue + 1));
                }
                else if (microOpMnemo == "INCR")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["RALU"].innerValue + 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    testAndSet("ALU", (short)(registers["RALU"].innerValue + 1));
                }
                else if (microOpMnemo == "DECL")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].innerValue - 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    testAndSet("ALU", (short)(registers["LALU"].innerValue - 1));
                }
                else if (microOpMnemo == "DECR")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["RALU"].innerValue - 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    testAndSet("ALU", (short)(registers["RALU"].innerValue - 1));
                }
                else if (microOpMnemo == "ONE")
                    testAndSet("ALU", 1);
                else if (microOpMnemo == "ZERO")
                    testAndSet("ALU", 0);
                cells[10, 2] = false;
                AddToLogAndMiniLog("ALU", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }

            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            waitForButton();
            if (registerToCheck != "")
                EnDisableButtons();
            if ((registers["ALU"].valueWhichShouldBeMovedToRegister & 0x8000) == 0x8000)
                flags["ZNAK"].setInnerValue(1);
            else
                flags["ZNAK"].setInnerValue(0);
            if (isOverflow)
            {
                flags["OFF"].setInnerValue(1);
                isOverflow = false;
            }
            else
                flags["OFF"].setInnerValue(0);
            registers["LALU"].setInnerValue(0);
            registers["RALU"].setInnerValue(0);
            buttonOKClicked = false;
        }
        private void exeTact1()
        {
            if (cells[1, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[1, raps];
                microOpMnemo = Grid_PM[1, raps].Value.ToString();
                if (microOpMnemo == "IXRE")
                    testAndSet("LALU", registers["RI"].innerValue);
                else if (microOpMnemo == "OLR")
                    testAndSet("BUS", registers["LR"].innerValue);
                else if (microOpMnemo == "ORR")
                    testAndSet("BUS", registers["RR"].innerValue);
                else if (microOpMnemo == "ORAE")
                    testAndSet("BUS", registers["RAE"].innerValue);
                else if (microOpMnemo == "IALU")
                    testAndSet("LALU", registers["A"].innerValue);
                else if (microOpMnemo == "OXE")
                    testAndSet("RALU", registers["X"].innerValue);
                else if (microOpMnemo == "OX")
                    testAndSet("BUS", registers["X"].innerValue);
                cells[1, 1] = false;
                AddToLogAndMiniLog("S1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[2, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[2, raps];
                microOpMnemo = Grid_PM[2, raps].Value.ToString();
                if (microOpMnemo == "ILK")
                    testAndSet("LK", registers["BUS"].innerValue);
                else if (microOpMnemo == "IRAP")
                    testAndSet("RAP", registers["BUS"].innerValue);
                else if (microOpMnemo == "OXE")
                    testAndSet("RALU", registers["X"].innerValue);
                cells[2, 1] = false;
                resetBus = true;
                AddToLogAndMiniLog("D1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[4, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[4, raps];
                microOpMnemo = Grid_PM[4, raps].Value.ToString();
                AddToLogAndMiniLog("D2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
                AddToLogAndMiniLog("C1", "SHT", Translator.GetMicroOpDescription("SHT"));
                int A = registers["A"].innerValue;
                bool SignBit = (A & 0x8000) == 0x8000 ? true : false;
                bool LastBit = (A & 0x0001) == 0x0001 ? true : false;
                if (microOpMnemo == "ALA")
                {
                    A <<= 1;
                    if (SignBit)
                        A |= 0x8000;
                    else
                        A &= 0x7FFF;
                    testAndSet("A", (short)(A));
                }
                else if (microOpMnemo == "ARA")
                {
                    A >>= 1;
                    if (SignBit)
                        A |= 0x8000;
                    testAndSet("A", (short)(A));
                }
                else if (microOpMnemo == "LRQ")
                {
                    A >>= 1;
                    testAndSet("A", (short)(A));
                    validateRegister();
                    if (LastBit)
                    {
                        short MQ = (short)(registers["MQ"].innerValue >> 1);
                        MQ = (short)(MQ | 0x8000);
                        testAndSet("MQ", MQ);
                    }
                    else
                        testAndSet("MQ", (short)(registers["MQ"].innerValue >> 1));
                }
                else if (microOpMnemo == "LLQ")
                {
                    A <<= 1;
                    SignBit = (registers["MQ"].innerValue & 0x8000) == 0x8000 ? true : false;
                    if (SignBit)
                        testAndSet("A", (short)(A + 1));
                    else
                        testAndSet("A", (short)(A));

                    validateRegister();
                    testAndSet("MQ", (short)(registers["MQ"].innerValue << 1));
                }
                else if (microOpMnemo == "LLA")
                    testAndSet("A", (short)(A << 1));
                else if (microOpMnemo == "LRA")
                    testAndSet("A", (short)(A >> 1));
                else if (microOpMnemo == "LCA")
                {
                    if (SignBit)
                        testAndSet("A", (short)((A << 1) + 1));
                    else
                        testAndSet("A", (short)(A << 1));
                }
                cells[4, 1] = false;
            }
            else if (cells[7, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[7, raps];
                microOpMnemo = Grid_PM[7, raps].Value.ToString();
                if (microOpMnemo == "RRC")
                {
                    Grid_Mem.Rows[registers["RAP"].innerValue].Selected = true;
                    Grid_Mem.FirstDisplayedScrollingRowIndex = registers["RAP"].innerValue;
                    Grid_Mem.CurrentCell = Grid_Mem.Rows[registers["RAP"].innerValue].Cells[0];
                    Grid_Mem_SelectionChanged(this, new EventArgs());
                    try
                    {
                        testAndSet("RBP", List_Memory[registers["RAP"].innerValue].getInt16Value());
                    }
                    catch
                    {
                        testAndSet("RBP", 0);
                    }
                    flags["MAV"].setInnerValue(0);
                    flags["IA"].setInnerValue(1);
                }
                else if (microOpMnemo == "MUL")
                {
                    testAndSet("LK", 16);
                }
                else if (microOpMnemo == "DIV")
                {
                    testAndSet("LK", 15);
                }
                cells[7, 1] = false;
                AddToLogAndMiniLog("C1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[8, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[8, raps];
                microOpMnemo = Grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "CEA")
                {
                    layoutChange = true;
                    switchLayOut();
                    short leftValue = 0, rightValue = 0;
                    short a;

                    string tmp = Convert.ToString(registers["RR"].innerValue, 2).PadLeft(16, '0');
                    if (tmp.Substring(0, 5) != "00000")
                    {
                        string xsi = tmp.Substring(5, 3);
                        if (xsi == "110" || xsi == "111")
                            leftValue = rightValue = 0;
                        else if (xsi == "010" || xsi == "011")
                            rightValue = registers["LR"].innerValue;
                        else if (xsi == "100" || xsi == "101")
                            rightValue = registers["RI"].innerValue;
                        leftValue = Convert.ToInt16(tmp.Substring(8, 8), 2);
                    }
                    else
                    {
                        leftValue = Convert.ToInt16(tmp.Substring(9, 7), 2);
                    }

                    testAndSet("L", leftValue);
                    validateRegister();
                    testAndSet("R", rightValue);
                    validateRegister();
                    testAndSet("SUMA", (short)(leftValue + rightValue));
                }
                cells[8, 1] = false;
                AddToLogAndMiniLog("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            button_OK.Visible = true;
            EnDisableButtons();
            registers[registerToCheck].Focus();
            waitForButton();
            EnDisableButtons();
            if (resetBus)
            {
                registers["BUS"].setInnerAndExpectedValue(0);
                resetBus = false;
            }
            flags["IA"].setInnerValue(0);
            flags["MAV"].setInnerValue(1);
            buttonOKClicked = false;
            switchLayOut();
        }
        #endregion
        #region StartSim
        public void prepareSimulation(bool b)
        {
            inMicroMode = b;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = _environmentPath;
            string internetTime = string.Empty;
            while (logFile == "")
            {
                dialog.Filter = Strings.simLog + "|*.log|" + Strings.all + "|*.*";
                dialog.Title = Strings.createLog;
                if (Directory.Exists(_environmentPath + @"\Log\"))
                    dialog.InitialDirectory = _environmentPath + @"\Log\";
                else
                    dialog.InitialDirectory = _environmentPath;
                DialogResult saveFileDialogResult = dialog.ShowDialog();
                if (saveFileDialogResult == DialogResult.OK && dialog.FileName != "")
                {
                    logFile = dialog.FileName;
                    initLogInformation();

                    CancellationTokenSource cts = new CancellationTokenSource();
                    Task loop = Task.Factory.StartNew(() => GetNISTDate(cts.Token, out internetTime));
                    if (Task.WaitAll(new Task[] { loop }, nistTimeTimeout))
                    {
                    }
                    else
                    {
                        cts.Cancel();
                    }
                    addTextToLog(Strings.startingSimulation + "\n" + DateTime.Now.ToString("HH:mm:ss").PadLeft(20, ' ') + "\n" + internetTime.PadLeft(20, ' ') + "\n" + Strings.registersContent + "\n");
                    foreach (var reg in registers.Values)
                        addTextToLog(reg.registerName.PadRight(6, ' ') + " = " + reg.Text + "\n");
                    addTextToLog("\n");
                }
            }
            if (DEVMODE)
                addTextToLog("\nAuto: " + DEVREGISTER + " ?= " + DEVVALUE + "\n");
            else
            {
                if (inMicroMode)
                    addTextToLog("Micro\n");
                else
                    addTextToLog("Makro\n");
            }
            simulateCPU();
        }
        private void simulateCPU()
        {
            startSim();
            if (currentTact == 0)
                instructionFetch();
            switchLayOut();
            while (isRunning && currentTact > 0)
                executeInstruction();
        }
        private void executeInstruction()
        {
            if (currentTact == 1 && (cells[1, 1] || cells[2, 1] || cells[4, 1] || cells[7, 1]))
                addTextToLog(Strings.tact + "1:\n");
            while (currentTact == 1 && (cells[1, 1] || cells[2, 1] || cells[4, 1] || cells[7, 1] || cells[8, 1]))
                exeTact1();
            if (currentTact == 1)
                nextTact();
            if (currentTact == 2 && cells[10, 2])
                addTextToLog(Strings.tact + "2:\n");
            while (currentTact == 2 && cells[10, 2])
                exeTact2();
            while (currentTact >= 2 && currentTact <= 5)
                nextTact();
            if (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                addTextToLog(Strings.tact + "6:\n");
            while (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                exeTact6();
            if (currentTact == 6)
                nextTact();
            if (currentTact == 7 && (cells[5, 7] || cells[6, 7] || cells[7, 7] || cells[8, 7] || cells[9, 7]))
                addTextToLog(Strings.tact + "7:\n");
            while (currentTact == 7 && (cells[5, 7] || cells[6, 7] || cells[7, 7] || cells[8, 7]))
                exeTact7();
            if (currentTact == 7 && cells[9, 7])
                exeTest();
            else if (currentTact == 7 && !isTestPositive)
            {
                testAndSet("RAPS", (short)(registers["RAPS"].innerValue + 1));
                Grid_PM.CurrentCell = Grid_PM[11, registers["RAPS"].innerValue];
                button_OK.Visible = true;
                EnDisableButtons();
                registers[registerToCheck].Focus();
                waitForButton();
                EnDisableButtons();
                currentTact = 0;
                dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
                if (!DEVMODE)
                {
                    stopSim();
                    buttonOKClicked = false;
                }
                else if (DEVMODE && registers[DEVREGISTER].valueWhichShouldBeMovedToRegister == DEVVALUE)
                {
                    DEVMODE = false;
                    registers[registerToCheck].setInnerValue(registers[registerToCheck].valueWhichShouldBeMovedToRegister);
                    stopSim();
                    buttonOKClicked = false;
                }
                else if (DEVMODE)
                {
                    prepareSimulation(false);
                }
            }
            else if (currentTact == 8)
            {
                registers["LALU"].setInnerAndExpectedValue(0);
                registers["RALU"].setInnerAndExpectedValue(0);
                currentTact = 0;
                dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
                if (!DEVMODE)
                {
                    stopSim();
                    buttonOKClicked = false;
                }
                else if (DEVMODE && registers[DEVREGISTER].valueWhichShouldBeMovedToRegister == DEVVALUE)
                {
                    DEVMODE = false;
                    registers[registerToCheck].setInnerValue(registers[registerToCheck].valueWhichShouldBeMovedToRegister);
                    stopSim();
                    buttonOKClicked = false;
                }
                else if (DEVMODE)
                {
                    prepareSimulation(false);
                }
            }
            else if (currentTact == 9)
            {
                currentTact = 0;
                dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
                if (!DEVMODE)
                {
                    stopSim();
                    buttonOKClicked = false;
                }
                else if (DEVMODE && registers[DEVREGISTER].valueWhichShouldBeMovedToRegister == DEVVALUE)
                {
                    DEVMODE = false;
                    registers[registerToCheck].setInnerValue(registers[registerToCheck].valueWhichShouldBeMovedToRegister);
                    stopSim();
                    buttonOKClicked = false;
                }
                else if (DEVMODE)
                {
                    prepareSimulation(false);
                }
            }
        }
        private void instructionFetch()
        {
            for (int i = 0; i < 8; i++)
                cells[0, i] = false;
            for (int i = 0; i < 11; i++)
                cells[i, 0] = false;

            raps = registers["RAPS"].innerValue;

            var row = Grid_PM.Rows[raps];
            na = 0;
            Grid_PM.CurrentCell = Grid_PM[1, raps];
            if ((string)row.Cells[11].Value == "")
                na = 0;
            else
                try
                {
                    na = Convert.ToInt16(row.Cells[11].Value);
                }
                catch (Exception)
                {
                    na = 0;
                }
            long rbps = Translator.GetRbpsValue(Grid_PM.Rows[raps]) + na;
            RBPS.Text = rbps.ToString("X").PadLeft(12, '0');

            addTextToLog("===============================\n" + Strings.tact + "0: RBPS=" + RBPS.Text + "\n");
            for (int i = 1; i < 11; i++)
                for (int j = 1; j < 8; j++)
                    cells[i, j] = row.Cells[i].Value.ToString() == "" ? false : true;

            //sht powoduje zajętość, tylko dokładnie których
            if (row.Cells[7].Value.ToString() == "SHT")
            {
                for (int j = 1; j < 8; j++)
                    cells[3, j] = false;
                //if SHT present - ALU disabled
                for (int j = 1; j < 8; j++)
                    cells[10, j] = false;
            }
            if (cells[1, 1])
            {
                for (int j = 2; j < 8; j++)
                    cells[1, j] = false;
            }
            if (cells[2, 1])
            {
                for (int j = 2; j < 8; j++)
                    cells[2, j] = false;
            }
            if (cells[3, 1])
            {
                for (int j = 1; j < 8; j++)
                    cells[3, j] = false;
                cells[3, 6] = true;
            }
            if (cells[4, 1])
            {
                for (int j = 1; j < 8; j++)
                    cells[4, j] = false;
                if (row.Cells[4].Value.ToString() == "ALA" || row.Cells[4].Value.ToString() == "ARA" ||
                    row.Cells[4].Value.ToString() == "LRQ" || row.Cells[4].Value.ToString() == "LLQ" ||
                    row.Cells[4].Value.ToString() == "LLA" || row.Cells[4].Value.ToString() == "LRA" ||
                    row.Cells[4].Value.ToString() == "LCA")
                {
                    cells[4, 1] = true;
                }
                else
                {
                    cells[4, 6] = true;
                }
            }
            if (cells[5, 1])
            {
                for (int j = 1; j < 7; j++)
                    cells[5, j] = false;
            }
            if (cells[6, 1])
            {
                for (int j = 1; j < 7; j++)
                    cells[6, j] = false;
            }
            if (cells[7, 1])
            {
                for (int j = 1; j < 8; j++)
                    cells[7, j] = false;
                cells[7, 1] = true;
                if (row.Cells[7].Value.ToString() == "CWC" || row.Cells[7].Value.ToString() == "IWC" || row.Cells[7].Value.ToString() == "END")
                {
                    cells[7, 7] = true;
                    cells[7, 1] = false;
                }
                if (row.Cells[7].Value.ToString() == "RRC")
                    cells[7, 6] = true;
                if (row.Cells[7].Value.ToString() == "SHT")
                {
                    cells[7, 1] = false;
                }

            }
            if (cells[8, 1])
            {
                for (int j = 1; j < 8; j++)
                    cells[8, j] = false;
                if (row.Cells[8].Value.ToString() == "RINT" || row.Cells[8].Value.ToString() == "ENI")
                    cells[8, 7] = true;
                else if (row.Cells[8].Value.ToString() == "OPC")
                {
                    cells[8, 7] = true;
                    for (int j = 1; j < 8; j++)
                        cells[9, j] = false;
                }
                else if (row.Cells[8].Value.ToString() == "CEA")
                    cells[8, 1] = true;
                else
                    cells[8, 6] = true;
            }
            if (cells[9, 1])
            {
                for (int j = 1; j < 8; j++)
                    cells[9, j] = false;
                cells[9, 7] = true;
            }
            if (cells[10, 1])
            {
                for (int j = 1; j < 8; j++)
                    cells[10, j] = false;
                cells[10, 2] = true;
            }
            nextTact();
        }
        private void startSim()
        {
            isRunning = true;
            closeLogToolStripMenuItem.Enabled = false;
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 8; j++)
                    cells[i, j] = true;
            foreach (var reg in registers)
                reg.Value.setActualValue(reg.Value.innerValue);
        }
        internal void stopSim()
        {
            button_Show_Log.Visible = true;
            richTextBox_Log.Clear();
            isRunning = false;
            inMicroMode = false;
            toolStripMenu_Edit.Enabled = true;
            toolStripMenu_Exit.Enabled = true;
            button_Makro.Visible = true;
            button_Micro.Visible = true;
            button_Next_Tact.Visible = false;
            toolStripMenu_Clear.Enabled = true;
            label_Status.Text = Strings.stopMode;
            label_Status.ForeColor = Color.Green;
            closeLogToolStripMenuItem.Enabled = true;
        }
        private void nextTact()
        {
            if (inMicroMode)
            {
                button_Makro.Visible = false;
                button_Micro.Visible = false;
                button_Next_Tact.Visible = true;
                if (!DEVMODE)
                    while (buttonNextTactClicked == false)
                        Application.DoEvents();
            }
            currentTact = (currentTact + 1) % 8;
            dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
            buttonNextTactClicked = false;
        }
        #endregion
    }
}
