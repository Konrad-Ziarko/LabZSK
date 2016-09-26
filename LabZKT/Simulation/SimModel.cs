using LabZKT.Controls;
using LabZKT.Memory;
using LabZKT.MicroOperations;
using LabZKT.Properties;
using LabZKT.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT.Simulation
{
    /// <summary>
    /// Model class
    /// </summary>
    public class SimModel
    {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        public event Action StartSim;
        public event Action StopSim;
        public event Action<string, string, string> AddText;
        public event Action ButtonOKSetVisivle;
        public event Action ButtonNextTactSetVisible;
        public event Action<int> SetNextTact;

        public bool isNewLogEnabled = false;
        public int currnetCycle { get; set; }
        public int mark { get; set; }
        public int mistakes { get; set; }
        public MemoryRecord selectedIntruction { get; set; }
        public Dictionary<string, BitTextBox> flags { get; set; }
        public MemoryRecord lastRecordFromRRC { get; set; }
        public List<MemoryRecord> List_Memory { get; set; }
        public List<MicroOperation> List_MicroOp { get; set; }
        public TextBox RBPS { get; set; }
        public Dictionary<string, NumericTextBox> registers { get; set; }
        public DataGridView Grid_Mem { get; set; }

        internal void ShowLog(string pathToLog)
        {
            if (logManager.checkLogIntegrity(pathToLog))
            {
                {
                    Form log = new Form();
                    RichTextBox rtb = new RichTextBox();
                    log.Controls.Add(rtb);
                    rtb.ReadOnly = true;
                    rtb.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
                    rtb.Text = File.ReadAllText(pathToLog, Encoding.Unicode);
                    log.AutoSize = true;
                    log.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    int width = 200;
                    Graphics g = Graphics.FromHwnd(rtb.Handle);
                    foreach (var line in rtb.Lines)
                    {
                        SizeF f = g.MeasureString(line, rtb.Font);
                        width = (int)(f.Width) > width ? (int)(f.Width) : width;
                    }
                    rtb.Width = width + 10;
                    rtb.Height = 600;
                    log.MaximizeBox = false;
                    log.SizeGripStyle = SizeGripStyle.Hide;
                    log.ShowDialog();
                }
            }
        }

        public DataGridView Grid_PM { get; set; }
        
        public bool resetBus { get; set; }
        private Dictionary<string, short> oldRegs;
        private LogManager logManager;

        internal void getMemoryRecord(int idxRow)
        {
            selectedIntruction = List_Memory[idxRow];
        }

        private Drawings draw;

        private bool[,] cells = new bool[11, 8];
        public int currentTact = 0;
        public bool isTestPositive = false, isOverflow = false, inEditMode = false,
            buttonOKClicked = false, inMicroMode = false, isRunning = false;
        public string logFile = string.Empty, microOpMnemo = string.Empty, registerToCheck = string.Empty;
        public short raps = 0, na = 0;
        public bool buttonNextTactClicked = false;
        //
        public SimModel(ref List<MemoryRecord> List_Memory, ref List<MicroOperation> List_MicroOp, ref Dictionary<string, NumericTextBox> registers, ref Dictionary<string, BitTextBox> flags, ref TextBox RBPS, ref MemoryRecord lastRecordFromRRC)
        {
            this.List_Memory = List_Memory;
            this.List_MicroOp = List_MicroOp;
            this.registers = registers;
            this.flags = flags;
            this.RBPS = RBPS;
            this.lastRecordFromRRC = lastRecordFromRRC;
            //
            currentTact = mistakes = 0;
            mark = 5;
            logManager = LogManager.Instance;
            draw = new Drawings(ref registers, ref flags, ref RBPS);
        }

        public void LoadLists(DataGridView Grid_Mem, DataGridView Grid_PM)
        {
            this.Grid_Mem = Grid_Mem;
            this.Grid_PM = Grid_PM;
            foreach (MemoryRecord row in List_Memory)
                Grid_Mem.Rows.Add(row.addr, row.value, row.hex);
            foreach (MicroOperation row in List_MicroOp)
                Grid_PM.Rows.Add(row.addr, row.S1, row.D1, row.S2, row.D2, row.S3,
                    row.D3, row.C1, row.C2, row.Test, row.ALU, row.NA);
        }
        public void initLogInformation()
        {
            string sysType;
            if (Environment.Is64BitOperatingSystem)
                sysType = "x64";
            else
                sysType = "x32";
            string ipAddrList = string.Empty;
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet && item.OperationalStatus == OperationalStatus.Up)
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            if (ipAddrList == string.Empty)
                                ipAddrList += ip.Address.ToString();
                            else
                                ipAddrList += "\n".PadRight(43, ' ') + ip.Address.ToString();
            logManager.createNewLog(logFile);
            Settings.Default.Save();
            logManager.addToMemory("Komputer w domenie: \"" + Environment.UserDomainName + "\"\nStacja \"" +
            Environment.MachineName + "\" " + sysType + " " + Environment.OSVersion + " OS\nZalogowano jako: \"" +
            Environment.UserName + "\"\n" + "Dostępne interfejsy sieciowe: " + ipAddrList + "\n\n\n");
        }
        public void rearrangeTextBoxes(Control panel_Sim_Control)
        {
            int horizontalGap = Convert.ToInt32(0.25 * panel_Sim_Control.Width);
            int verticalGap = Convert.ToInt32(0.2 * panel_Sim_Control.Height);
            var size = registers["LK"].Size;
            registers["LK"].SetXY((horizontalGap - 130) / 2, (verticalGap - 27) / 2);
            registers["A"].SetXY(horizontalGap + (horizontalGap - 130) / 2, (verticalGap - 27) / 2);
            registers["MQ"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, (verticalGap - 27) / 2);
            registers["X"].SetXY(horizontalGap * 3 + (horizontalGap - 130) / 2, (verticalGap - 27) / 2);

            registers["RAP"].SetXY((horizontalGap - 130) / 2, verticalGap + (verticalGap - 27) / 4);
            registers["LALU"].SetXY(horizontalGap + (horizontalGap - 130) / 2, verticalGap + (verticalGap - 27) / 4);
            registers["RALU"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, verticalGap + (verticalGap - 27) / 4);

            int gapFromRalu = panel_Sim_Control.Width - registers["RALU"].Location.X - registers["RALU"].Width - 60;
            flags["ZNAK"].SetXY(panel_Sim_Control.Width - gapFromRalu, verticalGap + (verticalGap - 27) / 4);
            flags["XRO"].SetXY(panel_Sim_Control.Width - gapFromRalu * 2 / 3, verticalGap + (verticalGap - 27) / 4);
            flags["OFF"].SetXY(panel_Sim_Control.Width - gapFromRalu / 3, verticalGap + (verticalGap - 27) / 4);
            flags["MAV"].SetXY(panel_Sim_Control.Width - gapFromRalu, verticalGap * 2 + (verticalGap - 27) * 1 / 4);
            flags["IA"].SetXY(panel_Sim_Control.Width - gapFromRalu * 2 / 3, verticalGap * 2 + (verticalGap - 27) * 1 / 4);
            flags["INT"].SetXY(panel_Sim_Control.Width - gapFromRalu / 3, verticalGap * 2 + (verticalGap - 27) * 1 / 4);

            registers["RBP"].SetXY((horizontalGap - 130) / 2, verticalGap * 2 + (verticalGap - 27) * 1 / 4);
            registers["ALU"].SetXY((registers["RALU"].Location.X - registers["LALU"].Location.X + 130) / 2
                - 65 + registers["LALU"].Location.X, verticalGap * 2 + (verticalGap - 27) * 1 / 4);

            registers["BUS"].SetXY((horizontalGap - 130) / 2, verticalGap * 3);

            registers["RR"].SetXY(horizontalGap + (horizontalGap - 130) / 2, verticalGap * 3 + (verticalGap - 27) / 2);
            registers["LR"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, verticalGap * 3 + (verticalGap - 27) / 2);
            registers["RI"].SetXY(horizontalGap * 3 + (horizontalGap - 130) / 2, verticalGap * 3 + (verticalGap - 27) / 2);

            var loc = RBPS.Location;
            loc.X = horizontalGap + (horizontalGap - 130) / 2;
            loc.Y = verticalGap * 4 + (verticalGap - 27) / 2;
            RBPS.Location = loc;
            registers["RAPS"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, verticalGap * 4 + (verticalGap - 27) / 2);
            registers["RAE"].SetXY(horizontalGap * 3 + (horizontalGap - 130) / 2, verticalGap * 4 + (verticalGap - 27) / 2);

            registers["L"].SetXY(horizontalGap + (horizontalGap - 130) * 3 / 2, verticalGap * 4);
            registers["R"].SetXY(horizontalGap * 2 + (horizontalGap - 130) * 3 / 2, verticalGap * 4);
            registers["SUMA"].SetXY((registers["R"].Location.X - registers["L"].Location.X + 130) / 2
                - 65 + registers["L"].Location.X, verticalGap * 4 + (verticalGap - 27) * 3 / 4);
        }

        public void switchLayOut(Control panel_Sim_Control)
        {
            if (registers["SUMA"].Visible)
            {
                registers["SUMA"].Visible = registers["L"].Visible = registers["R"].Visible = false;
                RBPS.Visible = registers["RAPS"].Visible = registers["RAE"].Visible = true;
            }
            else
            {
                registers["SUMA"].Visible = registers["L"].Visible = registers["R"].Visible = true;
                RBPS.Visible = registers["RAPS"].Visible = registers["RAE"].Visible = false;
            }
            draw.drawBackground(panel_Sim_Control);
        }

        public void leaveEditMode()
        {
            foreach (var oldReg in oldRegs)
            {
                if (registers[oldReg.Key].innerValue != oldReg.Value)
                    logManager.addToMemory("Zmiana zawartości rejestru: " + oldReg.Key.PadRight(6, ' ') +
                        oldReg.Value + "=>" + registers[oldReg.Key].innerValue + "\n");
            }
            foreach (var reg in registers)
                reg.Value.Enabled = false;
            foreach (var sig in flags)
                sig.Value.Enabled = false;
            foreach (var reg in registers)
                reg.Value.ReadOnly = true;
        }
        public void enterEditMode()
        {
            oldRegs = new Dictionary<string, short>();
            foreach (var reg in registers)
            {
                oldRegs.Add(reg.Key, reg.Value.innerValue);
            }
            foreach (var reg in registers)
            {
                if (reg.Value.registerName == "BUS" || reg.Value.registerName == "LALU" || reg.Value.registerName == "RALU" ||
                    reg.Value.registerName == "L" || reg.Value.registerName == "R" || reg.Value.registerName == "SUMA")
                    reg.Value.Enabled = false;
                else
                    reg.Value.Enabled = true;
            }
            foreach (var sig in flags)
                sig.Value.Enabled = true;
            foreach (var reg in registers)
                reg.Value.ReadOnly = false;
        }
        public void clearRegisters()
        {
            logManager.addToMemory("\n====Zerowanie rejestrów====\n");
            mark = 5;
            mistakes = currnetCycle = 0;
            foreach (var reg in registers)
                reg.Value.resetValue();
            foreach (var flg in flags)
                flg.Value.resetValue();
        }
        public void addToLog(string text)
        {
            //poprawić format logu
            logManager.addToMemory(text);
        }
        public void prepareSimulation(bool b)
        {
            inMicroMode = b;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = envPath;
            while (logFile == "")
            {
                dialog.Filter = "Log symulatora|*.log|Wszystko|*.*";
                dialog.Title = "Utwórz log";
                DialogResult saveFileDialogResult = dialog.ShowDialog();
                if (saveFileDialogResult == DialogResult.OK && dialog.FileName != "")
                {
                    logFile = dialog.FileName;
                    initLogInformation();
                    logManager.addToMemory("========Start symulacji========\n" + DateTime.Now.ToString("HH:mm:ss").PadLeft(31, ' ') + "\n======Zawartość rejestrów======\n");
                    foreach (var reg in registers.Values)
                        logManager.addToMemory((reg.registerName + "=").PadLeft(5, ' ') + reg.Text + "\n");
                }
            }
            simulateCPU();
        }
        public void DrawBackground(Control panel_Sim_Control)
        {
            rearrangeTextBoxes(panel_Sim_Control);
            draw.drawBackground(panel_Sim_Control);
        }

        public void checkRegisters()
        {
            short badValue;

            if (!registers[registerToCheck].validateRegisterValue(out badValue))
            {
                new Thread(SystemSounds.Beep.Play).Start();
                logManager.addToMemory("\tBłąd(" + (mistakes + 1) + "): " + registerToCheck + "=" + badValue +
                    "(" + registerToCheck + "=" + registers[registerToCheck].innerValue + ")\n\n");

                mistakes++;
                if (mistakes >= 2 && mistakes <= 5)
                    mark = 4;
                else if (mistakes >= 6 && mistakes <= 9)
                    mark = 3;
                else if (mistakes >= 10)
                    mark = 2;
            }
            else
            {
                logManager.addToMemory("\t" + registerToCheck + "=" + registers[registerToCheck].innerValue + "\n");
            }
        }
        public void NewLog()
        {
            logManager.addToMemory("\n" + DateTime.Now.ToString("HH:mm:ss").PadLeft(29, ' ') + "\n=======Stop  symulacji=======\n" +
                "Ocena: " + mark + "   Błędy: " + mistakes + "\n");
            logManager.clearInMemoryLog();
            logFile = string.Empty;
        }
        private void simulateCPU()
        {
            startSim();
            if (currentTact == 0)
                instructionFetch();
            while (isRunning && currentTact > 0)
                executeInstruction();
        }
        private void executeInstruction()
        {
            if (currentTact == 1 && (cells[1, 1] || cells[2, 1] || cells[4, 1] || cells[7, 1]))
                logManager.addToMemory("Takt1:\n");
            while (currentTact == 1 && (cells[1, 1] || cells[2, 1] || cells[4, 1] || cells[7, 1]))
                exeTact1();
            if (currentTact == 1)
                nextTact();
            if (currentTact == 2 && cells[10, 2])
                logManager.addToMemory("Takt2:\n");
            while (currentTact == 2 && cells[10, 2])
                exeTact2();
            while (currentTact >= 2 && currentTact <= 5)
                nextTact();
            if (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                logManager.addToMemory("Takt6:\n");
            while (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                exeTact6();
            if (currentTact == 6)
                nextTact();
            if (currentTact == 7 && (cells[5, 7] || cells[6, 7] || cells[7, 7] || cells[8, 7] || cells[9, 7]))
                logManager.addToMemory("Takt7:\n");
            while (currentTact == 7 && (cells[5, 7] || cells[6, 7] || cells[7, 7] || cells[8, 7]))
                exeTact7();
            if (currentTact == 7 && cells[9, 7])
                exeTest();
            else if (currentTact == 7 && !isTestPositive)
            {
                registers["RAPS"].setActualValue((short)(registers["RAPS"].innerValue + 1));
                registers["RAPS"].setNeedCheck(out registerToCheck);
                Grid_PM.CurrentCell = Grid_PM[11, registers["RAPS"].innerValue];
                ButtonOKSetVisivle();
                EnDisableButtons();
                registers[registerToCheck].Focus();
                while (buttonOKClicked == false)
                    Application.DoEvents();
                EnDisableButtons();
                currentTact = 0;
                SetNextTact(currentTact);
                stopSim();
                buttonOKClicked = false;
            }
            else if (currentTact == 8)
            {
                registers["LALU"].setInnerAndExpectedValue(0);
                registers["RALU"].setInnerAndExpectedValue(0);
                currentTact = 0;
                SetNextTact(currentTact);
                stopSim();
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
                    ButtonOKSetVisivle();
                    EnDisableButtons();
                    registers[registerToCheck].Focus();
                    while (buttonOKClicked == false)
                        Application.DoEvents();
                    buttonOKClicked = false;
                    EnDisableButtons();
                    registers["RAPS"].setActualValue(254);
                }
            }
            else if (microOpMnemo == "TIND")
            {
                var temp = List_Memory[registers["RAP"].valueWhichShouldBeMovedToRegister];
                Grid_Mem.Rows[registers["RAP"].valueWhichShouldBeMovedToRegister].Selected = true;
                if (temp.typ == 1)
                {
                    otherValue = true;
                    registers["RAPS"].setActualValue(Convert.ToInt16(temp.value, 2));
                }
                else if (temp.typ == 2)
                {
                    if (temp.XSI.Substring(2, 1) == "1")
                        isTestPositive = true;
                    else
                    {
                        otherValue = true;
                        registers["RAPS"].setActualValue(Convert.ToInt16(temp.OP, 2));
                    }
                }
                else if (temp.typ == 3)
                {
                    isTestPositive = true;
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
            else if (microOpMnemo == "TCR")
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
            AddText("TEST", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));

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

            ButtonOKSetVisivle();
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
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
                AddText("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[5, 7])
            {
                Grid_PM.CurrentCell = Grid_PM[5, raps];
                microOpMnemo = Grid_PM[5, raps].Value.ToString();
                if (microOpMnemo == "ORI")
                {
                    registers["BUS"].setActualValue(registers["RI"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORAE")
                {
                    registers["BUS"].setActualValue(registers["RAE"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].innerValue);
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OA")
                {
                    registers["BUS"].setActualValue(registers["A"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OMQ")
                {
                    registers["BUS"].setActualValue(registers["MQ"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OLR")
                {
                    registers["BUS"].setActualValue(registers["LR"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORBP")
                {
                    registers["BUS"].setActualValue(registers["RBP"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                cells[5, 7] = false;
                AddText("S3", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[6, 7])
            {
                Grid_PM.CurrentCell = Grid_PM[6, raps];
                microOpMnemo = Grid_PM[6, raps].Value.ToString();
                if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].innerValue);
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ILR")
                {
                    registers["LR"].setActualValue(registers["BUS"].innerValue);
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IX")
                {
                    registers["X"].setActualValue(registers["BUS"].innerValue);
                    registers["X"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBE")
                {
                    registers["RALU"].setActualValue(registers["BUS"].innerValue);
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBI")
                {
                    registers["RAE"].setActualValue(registers["BUS"].innerValue);
                    registers["RAE"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IA")
                {
                    registers["A"].setActualValue(registers["BUS"].innerValue);
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IMQ")
                {
                    registers["MQ"].setActualValue(registers["BUS"].innerValue);
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NSI")
                {
                    registers["LR"].setActualValue((short)(registers["LR"].innerValue + 1));
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
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
                {
                    registers["X"].setActualValue(registers["BUS"].innerValue);
                    registers["X"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRI")
                {
                    registers["RI"].setActualValue(registers["BUS"].innerValue);
                    registers["RI"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRR")
                {
                    registers["RR"].setActualValue(registers["BUS"].innerValue);
                    registers["RR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRBP")
                {
                    registers["RBP"].setActualValue(registers["BUS"].innerValue);
                    registers["RBP"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "SRBP")
                {
                    registers["RBP"].setActualValue(registers["BUS"].innerValue);
                    registers["RBP"].setNeedCheck(out registerToCheck);
                }
                cells[6, 7] = false;
                resetBus = true;
                AddText("D3", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[7, 7])
            {
                Grid_PM.CurrentCell = Grid_PM[7, raps];
                microOpMnemo = Grid_PM[7, raps].Value.ToString();
                if (microOpMnemo == "END")
                {
                    registers["RAPS"].setActualValue(0);
                    registers["RAPS"].setNeedCheck(out registerToCheck);
                }
                //skip TEST if END is present
                currentTact = 9;
                cells[7, 7] = false;
                AddText("C1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            ButtonOKSetVisivle();
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
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
                {
                    registers["LALU"].setActualValue(registers["RI"].innerValue);
                    registers["LALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORR")
                {
                    registers["BUS"].setActualValue(registers["RR"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORI")
                {
                    registers["BUS"].setActualValue(registers["RI"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OBE")
                {
                    registers["BUS"].setActualValue(registers["ALU"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IRAE")
                {
                    registers["RAE"].setActualValue(registers["SUMA"].innerValue);
                    registers["RAE"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ORAE")
                {
                    registers["BUS"].setActualValue(registers["RAE"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IALU")
                {
                    registers["LALU"].setActualValue(registers["A"].innerValue);
                    registers["LALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].innerValue);
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OX")
                {
                    registers["BUS"].setActualValue(registers["X"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OA")
                {
                    registers["BUS"].setActualValue(registers["A"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OMQ")
                {
                    registers["BUS"].setActualValue(registers["MQ"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                cells[3, 6] = false;
                AddText("S2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[4, 6])
            {
                Grid_PM.CurrentCell = Grid_PM[4, raps];
                microOpMnemo = Grid_PM[4, raps].Value.ToString();
                if (microOpMnemo == "ORI")
                {
                    registers["BUS"].setActualValue(registers["RI"].innerValue);
                    registers["BUS"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OXE")
                {
                    registers["RALU"].setActualValue(registers["X"].innerValue);
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "ILR")
                {
                    registers["LR"].setActualValue(registers["BUS"].innerValue);
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IX")
                {
                    registers["X"].setActualValue(registers["BUS"].innerValue);
                    registers["X"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBE")
                {
                    registers["RALU"].setActualValue(registers["BUS"].innerValue);
                    registers["RALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IBI")
                {
                    registers["RAE"].setActualValue(registers["BUS"].innerValue);
                    registers["RAE"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IA")
                {
                    registers["A"].setActualValue(registers["BUS"].innerValue);
                    registers["A"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "IMQ")
                {
                    registers["MQ"].setActualValue(registers["BUS"].innerValue);
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NSI")
                {
                    registers["LR"].setActualValue((short)(registers["LR"].innerValue + 1));
                    registers["LR"].setNeedCheck(out registerToCheck);
                }
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
                AddText("D2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[8, 6])
            {
                Grid_PM.CurrentCell = Grid_PM[8, raps];
                microOpMnemo = Grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "DLK")
                {
                    registers["LK"].setActualValue((short)(registers["LK"].innerValue - 1));
                    registers["LK"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "DRI")
                {
                    registers["RI"].setActualValue((short)(registers["RI"].innerValue - 1));
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
                    if ((registers["A"].innerValue & 0x8000) == 0x8000)
                        registers["MQ"].setActualValue((short)(registers["MQ"].innerValue & 0xFFFE));
                    else
                        registers["MQ"].setActualValue((short)(registers["MQ"].innerValue | 0x0001));
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                cells[8, 6] = false;
                AddText("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            ButtonOKSetVisivle();
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
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
                    registers["ALU"].setActualValue((short)(registers["LALU"].innerValue + registers["RALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
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
                    registers["ALU"].setActualValue((short)(registers["LALU"].innerValue - registers["RALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "CMX")
                {
                    registers["ALU"].setActualValue((short)(1 + ~registers["RALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "CMA")
                {
                    registers["ALU"].setActualValue((short)(1 + registers["LALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "OR")
                {
                    registers["ALU"].setActualValue((short)(registers["LALU"].innerValue | registers["RALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "AND")
                {
                    registers["ALU"].setActualValue((short)(registers["LALU"].innerValue & registers["RALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "EOR")
                {
                    registers["ALU"].setActualValue((short)(registers["LALU"].innerValue ^ registers["RALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NOTL")
                {
                    registers["ALU"].setActualValue((short)(~registers["LALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "NOTR")
                {
                    registers["ALU"].setActualValue((short)(~registers["RALU"].innerValue));
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "L")
                {
                    registers["ALU"].setActualValue(registers["L"].innerValue);
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "R")
                {
                    registers["ALU"].setActualValue(registers["R"].innerValue);
                    registers["ALU"].setNeedCheck(out registerToCheck);
                }
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
                    registers["ALU"].setActualValue((short)(registers["LALU"].innerValue + 1));
                    registers["ALU"].setNeedCheck(out registerToCheck);
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
                    registers["ALU"].setActualValue((short)(registers["RALU"].innerValue + 1));
                    registers["ALU"].setNeedCheck(out registerToCheck);
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
                    registers["ALU"].setActualValue((short)(registers["LALU"].innerValue - 1));
                    registers["ALU"].setNeedCheck(out registerToCheck);
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
                    registers["ALU"].setActualValue((short)(registers["RALU"].innerValue - 1));
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
                AddText("ALU", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }

            ButtonOKSetVisivle();
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
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
                {
                    testAndSet("LALU", registers["RI"].innerValue);
                }
                else if (microOpMnemo == "OLR")
                {
                    testAndSet("BUS", registers["LR"].innerValue);
                }
                else if (microOpMnemo == "ORR")
                {
                    testAndSet("BUS", registers["RR"].innerValue);
                }
                else if (microOpMnemo == "ORAE")
                {
                    testAndSet("BUS", registers["RAE"].innerValue);
                }
                else if (microOpMnemo == "IALU")
                {
                    testAndSet("LALU", registers["A"].innerValue);
                }
                else if (microOpMnemo == "OXE")
                {
                    testAndSet("RALU", registers["X"].innerValue);
                }
                else if (microOpMnemo == "OX")
                {
                    testAndSet("BUS", registers["X"].innerValue);
                }
                cells[1, 1] = false;
                AddText("S1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[2, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[2, raps];
                microOpMnemo = Grid_PM[2, raps].Value.ToString();
                if (microOpMnemo == "ILK")
                {
                    testAndSet("LK", registers["BUS"].innerValue);
                }
                else if (microOpMnemo == "IRAP")
                {
                    testAndSet("RAP", registers["BUS"].innerValue);
                }
                else if (microOpMnemo == "OXE")
                {
                    testAndSet("RALU", registers["X"].innerValue);
                }
                cells[2, 1] = false;
                resetBus = true;
                AddText("D1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[4, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[4, raps];
                microOpMnemo = Grid_PM[4, raps].Value.ToString();
                AddText("D2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
                AddText("C1", "SHT", Translator.GetMicroOpDescription("SHT"));
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
                    ButtonOKSetVisivle();
                    EnDisableButtons();
                    registers[registerToCheck].Focus();
                    while (buttonOKClicked == false)
                        Application.DoEvents();
                    EnDisableButtons();
                    buttonOKClicked = false;
                    if (LastBit)
                    {
                        short MQ = (short)(registers["MQ"].innerValue >> 1);
                        MQ = (short)(MQ | 0x8000);
                        registers["MQ"].setActualValue(MQ);
                    }
                    else
                        registers["MQ"].setActualValue((short)(registers["MQ"].innerValue >> 1));
                    registers["MQ"].setNeedCheck(out registerToCheck);
                }
                else if (microOpMnemo == "LLQ")
                {
                    A <<= 1;
                    SignBit = (registers["MQ"].innerValue & 0x8000) == 0x8000 ? true : false;
                    if (SignBit)
                        registers["A"].setActualValue((short)(A + 1));
                    else
                        registers["A"].setActualValue((short)(A));
                    registers["A"].setNeedCheck(out registerToCheck);

                    ButtonOKSetVisivle();
                    EnDisableButtons();
                    registers[registerToCheck].Focus();
                    while (buttonOKClicked == false)
                        Application.DoEvents();
                    EnDisableButtons();
                    buttonOKClicked = false;
                    testAndSet("MQ", (short)(registers["MQ"].innerValue << 1));
                }
                else if (microOpMnemo == "LLA")
                {
                    testAndSet("A", (short)(A << 1));
                }
                else if (microOpMnemo == "LRA")
                {
                    testAndSet("A", (short)(A >> 1));
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
                Grid_PM.CurrentCell = Grid_PM[7, raps];
                microOpMnemo = Grid_PM[7, raps].Value.ToString();
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
                AddText("C1", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            else if (cells[8, 1])
            {
                Grid_PM.CurrentCell = Grid_PM[8, raps];
                microOpMnemo = Grid_PM[8, raps].Value.ToString();
                if (microOpMnemo == "CEA")
                {
                    ///zwykly adresacja
                    //rozszerzony N
                    //dana
                }
                cells[8, 1] = false;
                AddText("C2", microOpMnemo, Translator.GetMicroOpDescription(microOpMnemo));
            }
            ButtonOKSetVisivle();
            if (registerToCheck != "")
            {
                EnDisableButtons();
                registers[registerToCheck].Focus();
            }
            while (buttonOKClicked == false)
                Application.DoEvents();
            if (registerToCheck != "")
                EnDisableButtons();
            if (resetBus)
            {
                registers["BUS"].setInnerAndExpectedValue(0);
                resetBus = false;
            }
            flags["IA"].setInnerValue(0);
            flags["MAV"].setInnerValue(1);
            buttonOKClicked = false;
        }
        #endregion

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

            logManager.addToMemory("===============================\n\nTakt0: RBPS=" + RBPS.Text + "\n");
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
            nextTact();
        }

        private void startSim()
        {
            StartSim();
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 8; j++)
                    cells[i, j] = true;
            foreach (var reg in registers)
                reg.Value.setActualValue(reg.Value.innerValue);
        }
        private void stopSim()
        {
            StopSim();
            isRunning = false;
            inMicroMode = false;
        }
        public void nextTact()
        {
            if (inMicroMode)
            {
                ButtonNextTactSetVisible();
                while (buttonNextTactClicked == false)
                    Application.DoEvents();
            }
            currentTact = (currentTact + 1) % 8;
            SetNextTact(currentTact);
            buttonNextTactClicked = false;
        }

        public void EnDisableButtons()
        {
            foreach (var reg in registers)
                reg.Value.Enabled = !reg.Value.Enabled;
        }
        private void testAndSet(string register, short setValue)
        {
            registers[register].setActualValue(setValue);
            registers[register].setNeedCheck(out registerToCheck);
        }
    }
}
