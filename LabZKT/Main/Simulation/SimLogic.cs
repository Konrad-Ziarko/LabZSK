using System;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class RunSim : Form
    {
        private void checkRegisters()
        {
            short badValue;

            if (!registers[registerToCheck].checkValue(out badValue))
            {
                new Thread(SystemSounds.Beep.Play).Start();
                //zapisac bledna i poprawna wartosc do logu
                int len = 0;
                logManager.addToMemory("\tBłąd(" + (MainWindow.mistakes + 1) + "): " + registerToCheck + "=" + badValue +
                    "(" + registerToCheck + "=" + registers[registerToCheck].getInnerValue() + ")\n", logFile);

                MainWindow.mistakes++;
                dataGridView_Info[0, 1].Value = MainWindow.mistakes;
                if (MainWindow.mistakes >= 2 && MainWindow.mistakes <= 5)
                    MainWindow.mark = 4;
                else if (MainWindow.mistakes >= 6 && MainWindow.mistakes <= 9)
                    MainWindow.mark = 3;
                else if (MainWindow.mistakes >= 10)
                {
                    dgvcs1.ForeColor = Color.Red;
                    MainWindow.mark = 2;
                }
                dataGridView_Info[0, 0].Value = MainWindow.mark;
            }
            else
            {
                int len = 0;
                logManager.addToMemory("\t" + registerToCheck + "=" + registers[registerToCheck].getInnerValue() + "\n", logFile);
            }
        }
        private void exeTest()
        {
            grid_PM.CurrentCell = grid_PM[9, raps];
            microOpMnemo = grid_PM[9, raps].Value.ToString();
            bool otherValue = false;
            if (microOpMnemo == "UNB")
                isTestPositive = true;
            else if (microOpMnemo == "TINT")
            {
                if (flags["INT"].getInnerValue() == 0)
                    isTestPositive = true;
                else
                {
                    otherValue = true;
                    flags["INT"].setInnerValue(0);
                    //czy tak test TINT?
                    registers["RAP"].setActualValue(255);
                    registers["RAP"].setNeedCheck(out registerToCheck);
                    button_OK.Visible = true;
                    modeManager.EnDisableButtons();
                    registers[registerToCheck].Focus();
                    while (buttonOKClicked == false)
                    {
                        Application.DoEvents();
                    }
                    buttonOKClicked = false;
                    modeManager.EnDisableButtons();
                    registers["RAPS"].setActualValue(254);
                }
            }
            else if (microOpMnemo == "TIND")
            {
                var temp = List_Memory[registers["RAP"].getActualValue()];
                grid_PO.Rows[registers["RAP"].getActualValue()].Selected = true;
                if (temp.XSI.Substring(2, 1) == "1")
                    isTestPositive = true;
                else
                {
                    otherValue = true;
                    registers["RAPS"].setActualValue((short)Convert.ToInt16(temp.OP, 2));
                }
            }
            else if (microOpMnemo == "TAS")
            {
                if (registers["A"].getInnerValue() >= 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TXS")
            {
                if (registers["RI"].getInnerValue() >= 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TQ15")
            {
                if ((registers["MQ"].getInnerValue() & 0x0001) != 0x0001)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TCR")
            {
                short lk = registers["LK"].getInnerValue();
                if (grid_PM.Rows[raps].Cells[7].Value.ToString() == "SHT")
                {
                    if (registers["LK"].getInnerValue() == 0)
                        isTestPositive = true;
                }
                else
                {
                    if (registers["LK"].getInnerValue() != 0)
                        isTestPositive = true;
                }
            }
            else if (microOpMnemo == "TSD")
            {
                if (flags["ZNAK"].getInnerValue() == 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TAO")
            {
                if (flags["OFF"].getInnerValue() == 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TXP")
            {
                if (registers["RI"].getInnerValue() < 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TXZ")
            {
                //co tu porównać?
            }
            else if (microOpMnemo == "TXRO")
            {
                if (flags["XRO"].getInnerValue() == 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TAP")
            {
                if (registers["A"].getInnerValue() < 0)
                    isTestPositive = true;
            }
            else if (microOpMnemo == "TAZ")
            {
                if (registers["A"].getInnerValue() == 0)
                    isTestPositive = true;
            }
            cells[9, 7] = false;
            richTextBox_addText("TEST", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));

            if (isTestPositive)
            {
                registers["RAPS"].setActualValue(na);
                isTestPositive = false;
            }
            else if (!otherValue)
            {
                //czy zerować raps jak overflow?
                registers["RAPS"].setActualValue((short)((registers["RAPS"].getInnerValue() + 1) % 256));
            }
            registers["RAPS"].setNeedCheck(out registerToCheck);

            grid_PM.CurrentCell = grid_PM[11, registers["RAPS"].getInnerValue()];

            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                modeManager.EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
            buttonOKClicked = false;
            if (registerToCheck != "")
                modeManager.EnDisableButtons();
            currentTact = 9;
        }

        private void exeTact7()
        {
            if (cells[8, 7])
            {
                grid_PM.CurrentCell = grid_PM[8, raps];
                microOpMnemo = grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "RA")
                {
                    registers["A"].setActualValue(0);
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "RMQ")
                {
                    registers["MQ"].setActualValue(0);
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "RINT")
                {
                    flags["INT"].setInnerValue(0);
                }
                else if (microOpMnemo == "ENI")
                {
                    flags["INT"].setInnerValue(1);
                }
                cells[8, 7] = false;
                richTextBox_addText("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[5, 7])
            {
                grid_PM.CurrentCell = grid_PM[5, raps];
                microOpMnemo = grid_PM[5, raps].Value.ToString();
                if (microOpMnemo == "ORI")
                {
                    registers["BUS"].setActualValue(registers["RI"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORAE")
                {
                    registers["BUS"].setActualValue(registers["RAE"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OA")
                {
                    registers["BUS"].setActualValue(registers["A"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OMQ")
                {
                    registers["BUS"].setActualValue(registers["MQ"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OLR")
                {
                    registers["BUS"].setActualValue(registers["LR"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORBP")
                {
                    registers["BUS"].setActualValue(registers["RBP"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                cells[5, 7] = false;
                richTextBox_addText("S3", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[6, 7])
            {
                grid_PM.CurrentCell = grid_PM[6, raps];
                microOpMnemo = grid_PM[6, raps].Value.ToString();
                if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ILR")
                {
                    registers["LR"].setActualValue(registers["BUS"].getInnerValue());
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IX")
                {
                    registers["X"].setActualValue(registers["BUS"].getInnerValue());
                    registers["X"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBE")
                {
                    registers["RALU"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBI")
                {
                    registers["RAE"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RAE"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IA")
                {
                    registers["A"].setActualValue(registers["BUS"].getInnerValue());
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IMQ")
                {
                    registers["MQ"].setActualValue(registers["BUS"].getInnerValue());
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NSI")
                {
                    registers["LR"].setActualValue((short)(registers["LR"].getInnerValue() + 1));
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IAS")
                {
                    if ((registers["A"].getInnerValue() & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                else if (microOpMnemo == "SGN")
                {
                    if ((registers["X"].getInnerValue() & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                else if (microOpMnemo == "IX")
                {
                    registers["X"].setActualValue(registers["BUS"].getInnerValue());
                    registers["X"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRI")
                {
                    registers["RI"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RI"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRR")
                {
                    registers["RR"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRBP")
                {
                    registers["RBP"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RBP"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "SRBP")
                {
                    registers["RBP"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RBP"].setNeedCheck(out registerToCheck);
                }
                cells[6, 7] = false;
                resetBus = true;
                richTextBox_addText("D3", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[7, 7])
            {
                grid_PM.CurrentCell = grid_PM[7, raps];
                microOpMnemo = grid_PM[7, raps].Value.ToString();
                if (microOpMnemo == "END")
                {
                    registers["RAPS"].setActualValue(0);
                    registers["RAPS"].setNeedCheck(out registerToCheck);
                }
                //skip TEST
                currentTact = 9;
                cells[7, 7] = false;
                richTextBox_addText("C1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                modeManager.EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
            buttonOKClicked = false;
            if (resetBus)
            {
                registers["BUS"].setInnerAndActual(0);
                resetBus = false;
            }
            if (registerToCheck != "")
                modeManager.EnDisableButtons();
        }

        private void exeTact6()
        {
            if (cells[3, 6])
            {
                grid_PM.CurrentCell = grid_PM[3, raps];
                microOpMnemo = grid_PM[3, raps].Value.ToString();
                if (microOpMnemo == "IXRE")
                {
                    registers["LALU"].setActualValue(registers["RI"].getInnerValue());
                    registers["LALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORR")
                {
                    registers["BUS"].setActualValue(registers["RR"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORI")
                {
                    registers["BUS"].setActualValue(registers["RI"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OBE")
                {
                    registers["BUS"].setActualValue(registers["ALU"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRAE")
                {
                    registers["RAE"].setActualValue(registers["SUMA"].getInnerValue());
                    registers["RAE"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORAE")
                {
                    registers["BUS"].setActualValue(registers["RAE"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IALU")
                {
                    registers["LALU"].setActualValue(registers["A"].getInnerValue());
                    registers["LALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OX")
                {
                    registers["BUS"].setActualValue(registers["X"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OA")
                {
                    registers["BUS"].setActualValue(registers["A"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OMQ")
                {
                    registers["BUS"].setActualValue(registers["MQ"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                cells[3, 6] = false;
                richTextBox_addText("S2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[4, 6])
            {
                grid_PM.CurrentCell = grid_PM[4, raps];
                microOpMnemo = grid_PM[4, raps].Value.ToString();
                if (microOpMnemo == "ORI")
                {
                    registers["BUS"].setActualValue(registers["RI"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ILR")
                {
                    registers["LR"].setActualValue(registers["BUS"].getInnerValue());
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IX")
                {
                    registers["X"].setActualValue(registers["BUS"].getInnerValue());
                    registers["X"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBE")
                {
                    registers["RALU"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBI")
                {
                    registers["RAE"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RAE"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IA")
                {
                    registers["A"].setActualValue(registers["BUS"].getInnerValue());
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IMQ")
                {
                    registers["MQ"].setActualValue(registers["BUS"].getInnerValue());
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NSI")
                {
                    registers["LR"].setActualValue((short)(registers["LR"].getInnerValue() + 1));
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IAS")
                {
                    if ((registers["A"].getInnerValue() & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                else if (microOpMnemo == "SGN")
                {
                    if ((registers["X"].getInnerValue() & 0x8000) == 0x8000)
                        flags["ZNAK"].setInnerValue(1);
                    else
                        flags["ZNAK"].setInnerValue(0);
                }
                cells[4, 6] = false;
                resetBus = true;
                richTextBox_addText("D2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[8, 6])
            {
                grid_PM.CurrentCell = grid_PM[8, raps];
                microOpMnemo = grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "DLK")
                {
                    registers["LK"].setActualValue((short)(registers["LK"].getInnerValue() - 1));
                    registers["LK"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "DRI")
                {
                    registers["RI"].setActualValue((short)(registers["RI"].getInnerValue() - 1));
                    registers["RI"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "SOFF")
                {
                    flags["OFF"].setInnerValue(1);
                }
                else if (microOpMnemo == "ROFF")
                {
                    flags["OFF"].setInnerValue(0);
                }
                else if (microOpMnemo == "SXRO")
                {
                    flags["XRO"].setInnerValue(1);
                }
                else if (microOpMnemo == "RXRO")
                {
                    flags["XRO"].setInnerValue(0);
                }
                else if (microOpMnemo == "AQ16")
                {
                    if ((registers["A"].getInnerValue() & 0x8000) == 0x8000)
                        registers["MQ"].setActualValue((short)(registers["MQ"].getInnerValue() & 0xFFFE));
                    else
                        registers["MQ"].setActualValue((short)(registers["MQ"].getInnerValue() | 0x0001));
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                cells[8, 6] = false;
                richTextBox_addText("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                modeManager.EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
            if (resetBus)
            {
                registers["BUS"].setInnerAndActual(0);
                resetBus = false;
            }
            buttonOKClicked = false;
            if (registerToCheck != "")
                modeManager.EnDisableButtons();
        }

        private void exeTact2()
        {
            if (cells[10, 2])
            {
                grid_PM.CurrentCell = grid_PM[10, raps];
                microOpMnemo = grid_PM[10, raps].Value.ToString();
                isOverflow = false;
                if (microOpMnemo == "ADS")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].getInnerValue() + registers["RALU"].getInnerValue());
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    registers["ALU"].setActualValue((short)(registers["LALU"].getInnerValue() + registers["RALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "SUS")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].getInnerValue() - registers["RALU"].getInnerValue());
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    registers["ALU"].setActualValue((short)(registers["LALU"].getInnerValue() - registers["RALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "CMX")
                {
                    registers["ALU"].setActualValue((short)(1 + ~registers["RALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "CMA")
                {
                    registers["ALU"].setActualValue((short)(1 + registers["LALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OR")
                {
                    registers["ALU"].setActualValue((short)(registers["LALU"].getInnerValue() | registers["RALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "AND")
                {
                    registers["ALU"].setActualValue((short)(registers["LALU"].getInnerValue() & registers["RALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "EOR")
                {
                    registers["ALU"].setActualValue((short)(registers["LALU"].getInnerValue() ^ registers["RALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NOTL")
                {
                    registers["ALU"].setActualValue((short)(~registers["LALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NOTR")
                {
                    registers["ALU"].setActualValue((short)(~registers["RALU"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "L")
                {
                    registers["ALU"].setActualValue((short)(registers["L"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "R")
                {
                    registers["ALU"].setActualValue((short)(registers["R"].getInnerValue()));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "INCL")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].getInnerValue() + 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    registers["ALU"].setActualValue((short)(registers["LALU"].getInnerValue() + 1));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "INCR")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["RALU"].getInnerValue() + 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    registers["ALU"].setActualValue((short)(registers["RALU"].getInnerValue() + 1));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "DECL")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["LALU"].getInnerValue() - 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    registers["ALU"].setActualValue((short)(registers["LALU"].getInnerValue() - 1));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "DECR")
                {
                    try
                    {
                        checked
                        {
                            short test = (short)(registers["RALU"].getInnerValue() - 1);
                        }
                    }
                    catch (OverflowException)
                    {
                        isOverflow = true;
                    }
                    registers["ALU"].setActualValue((short)(registers["RALU"].getInnerValue() - 1));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ONE")
                {
                    registers["ALU"].setActualValue(1);
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ZERO")
                {
                    registers["ALU"].setActualValue(0);
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                cells[10, 2] = false;
                richTextBox_addText("ALU", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }

            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                modeManager.EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
            if (registerToCheck != "")
                modeManager.EnDisableButtons();
            if ((registers["ALU"].getActualValue() & 0x8000) == 0x8000)
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
                grid_PM.CurrentCell = grid_PM[1, raps];
                microOpMnemo = grid_PM[1, raps].Value.ToString();
                if (microOpMnemo == "IXRE")
                {
                    registers["LALU"].setActualValue(registers["RI"].getInnerValue());
                    registers["LALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OLR")
                {
                    registers["BUS"].setActualValue(registers["LR"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORR")
                {
                    registers["BUS"].setActualValue(registers["RR"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORAE")
                {
                    registers["BUS"].setActualValue(registers["RAE"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IALU")
                {
                    registers["LALU"].setActualValue(registers["A"].getInnerValue());
                    registers["LALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OX")
                {
                    registers["BUS"].setActualValue(registers["X"].getInnerValue());
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                cells[1, 1] = false;
                richTextBox_addText("S1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[2, 1])
            {
                grid_PM.CurrentCell = grid_PM[2, raps];
                microOpMnemo = grid_PM[2, raps].Value.ToString();
                if (microOpMnemo == "ILK")
                {
                    registers["LK"].setActualValue(registers["BUS"].getInnerValue());
                    registers["LK"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRAP")
                {
                    registers["RAP"].setActualValue(registers["BUS"].getInnerValue());
                    registers["RAP"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].getInnerValue());
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                cells[2, 1] = false;
                resetBus = true;
                richTextBox_addText("D1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[4, 1])
            {
                grid_PM.CurrentCell = grid_PM[4, raps];
                microOpMnemo = grid_PM[4, raps].Value.ToString();
                richTextBox_addText("D2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
                richTextBox_addText("C1", "SHT", Translator.GetMicroOpDescription("SHT"));
                int A = registers["A"].getInnerValue();
                bool SignBit = (A & 0x8000) == 0x8000 ? true : false;
                bool LastBit = (A & 0x0001) == 0x0001 ? true : false;
                if (microOpMnemo == "ALA")
                {
                    A <<= 1;
                    if (SignBit)
                        A |= 0x8000;
                    else
                        A &= 0x7FFF;
                    registers["A"].setActualValue((short)(A));
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ARA")
                {
                    A >>= 1;
                    if (SignBit)
                        A |= 0x8000;
                    registers["A"].setActualValue((short)(A));
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "LRQ")
                {
                    A >>= 1;
                    registers["A"].setActualValue((short)(A));
                    registers["A"].setNeedCheck(out registerToCheck);

                    button_OK.Visible = true;
                    modeManager.EnDisableButtons();
                    registers[registerToCheck].Focus();
                    while (buttonOKClicked == false)
                        Application.DoEvents();
                    modeManager.EnDisableButtons();
                    buttonOKClicked = false;
                    if (LastBit)
                    {
                        short MQ = (short)(registers["MQ"].getInnerValue() >> 1);
                        MQ = (short)(MQ | 0x8000);
                        registers["MQ"].setActualValue(MQ);
                    }
                    else
                        registers["MQ"].setActualValue((short)(registers["MQ"].getInnerValue() >> 1));
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "LLQ")
                {
                    A <<= 1;
                    SignBit = (registers["MQ"].getInnerValue() & 0x8000) == 0x8000 ? true : false;
                    if (SignBit)
                        registers["A"].setActualValue((short)(A + 1));
                    else
                        registers["A"].setActualValue((short)(A));
                    registers["A"].setNeedCheck(out registerToCheck);

                    button_OK.Visible = true;
                    modeManager.EnDisableButtons();
                    registers[registerToCheck].Focus();
                    while (buttonOKClicked == false)
                        Application.DoEvents();
                    modeManager.EnDisableButtons();
                    buttonOKClicked = false;

                    registers["MQ"].setActualValue((short)(registers["MQ"].getInnerValue() << 1));
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "LLA")
                {
                    registers["A"].setActualValue((short)(A << 1));
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "LRA")
                {
                    registers["A"].setActualValue((short)(A >> 1));
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "LCA")
                {
                    if (SignBit)
                        registers["A"].setActualValue((short)((A << 1) + 1));
                    else
                        registers["A"].setActualValue((short)(A << 1));
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                cells[4, 1] = false;
            }
            else if (cells[7, 1])
            {
                grid_PM.CurrentCell = grid_PM[7, raps];
                microOpMnemo = grid_PM[7, raps].Value.ToString();
                if (microOpMnemo == "CWC")
                {
                    flags["MAV"].setInnerValue(0);
                    flags["IA"].setInnerValue(1);
                }
                else if (microOpMnemo == "RRC")
                {
                    flags["MAV"].setInnerValue(0);
                    flags["IA"].setInnerValue(1);
                }
                else if (microOpMnemo == "MUL")
                {
                    registers["LK"].setActualValue(16);
                    registers["LK"].setNeedCheck(out registerToCheck);
                    cells[7, 1] = false;
                }
                else if (microOpMnemo == "DIV")
                {
                    registers["LK"].setActualValue(15);
                    registers["LK"].setNeedCheck(out registerToCheck);
                    cells[7, 1] = false;
                }
                else if (microOpMnemo == "IWC")
                {
                    //;
                }
                cells[7, 1] = false;
                richTextBox_addText("C1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[8, 1])
            {
                grid_PM.CurrentCell = grid_PM[8, raps];
                microOpMnemo = grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "CEA")
                {
                    ///zwykly adresacja
                    //rozszerzony N
                    //dana?
                }
                cells[8, 1] = false;
                richTextBox_addText("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            button_OK.Visible = true;
            if (registerToCheck != "")
            {
                modeManager.EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
            if (registerToCheck != "")
                modeManager.EnDisableButtons();
            if (resetBus)
            {
                registers["BUS"].setInnerAndActual(0);
                resetBus = false;
            }
            flags["IA"].setInnerValue(0);
            flags["MAV"].setInnerValue(1);
            buttonOKClicked = false;
        }
        private void instructionFetch()
        {
            for (int i = 0; i < 8; i++)
                cells[0, i] = false;
            for (int i = 0; i < 11; i++)
                cells[i, 0] = false;

            raps = registers["RAPS"].getInnerValue();

            var row = grid_PM.Rows[raps];
            na = 0;
            grid_PM.CurrentCell = grid_PM[1, raps];
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
            long rbps = Translator.GetRbpsValue(grid_PM.Rows[raps]) + na;
            RBPS.Text = rbps.ToString("X").PadLeft(12, '0');
            int len = 0;
            logManager.addToMemory("===============================\n\nTakt0: RBPS=" + RBPS.Text + "\n", logFile);
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
                if (row.Cells[7].Value.ToString() == "CWC" || row.Cells[8].Value.ToString() == "IWC")
                    cells[7, 7] = true;
                if (row.Cells[7].Value.ToString() == "RRC")
                    cells[7, 6] = true;
                if (row.Cells[7].Value.ToString() == "END")
                {
                    cells[7, 1] = false;
                    cells[7, 7] = true;
                }
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
                    //if OPC is present TEST is not executed
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
            modeManager.nextTact();
        }
    }

}
