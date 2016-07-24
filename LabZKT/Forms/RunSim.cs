using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class RunSim : Form
    {
        //mem przekazac do watku symulacji bo cwc bedzie zapisywac do pamieci
        private List<MemoryRecord> List_Memory { get; set; }
        private List<MicroOperation> List_MicroOp { get; set; }
        private MemoryRecord lastRecordFromRRC;
        Dictionary<string, short> oldRegs;
        private ModeManager modeManager;
        private LogManager logManager;
        private Drawings draw;
        public bool resetBus { get; private set; }
        public static short dragValue;
        public static Size hitTest;
        private string logFile = string.Empty, microOpMnemo = string.Empty, registerToCheck = string.Empty;
        public bool inEditMode = false, buttonOKClicked = false;
        public static bool inMicroMode = false, isRunning = false, buttonNextTactClicked = false;
        public static int currentTact = 0;
        private short raps = 0, na = 0;
        private bool isTestPositive = false, isOverflow = false, wasMaximized = false;
        public static bool[,] cells = new bool[11, 8];
        private Dictionary<string, NumericTextBox> registers;
        private Dictionary<string, BitTextBox> flags;
        private TextBox RBPS;
        private DataGridViewCellStyle dgvcs1;
        public RunSim(ref List<MemoryRecord> mem, ref List<MicroOperation> op, ref Dictionary<string, NumericTextBox> reg,
            ref Dictionary<string, BitTextBox> sig, ref TextBox rbps, ref MemoryRecord lrfr)
        {
            lastRecordFromRRC = lrfr;
            List_Memory = mem;
            List_MicroOp = op;
            registers = reg;
            flags = sig;
            RBPS = rbps;
            InitializeComponent();

        }
        public Control getSimPanel()
        {
            return panel_Sim_Control;
        }
        private void RunSim_Load(object sender, EventArgs e)
        {
            draw = new Drawings(ref panel_Sim_Control, ref registers, ref flags, ref RBPS);
            modeManager = new ModeManager(ref registers, ref toolStripMenu_Edit, ref toolStripMenu_Clear, ref label_Status,
                ref button_Makro, ref button_Micro, ref dataGridView_Info, ref button_Next_Tact);
            logManager = new LogManager();
            Size = new Size(1024, 768);
            initAll(sender, e);
        }
        private void initAll(object sender, EventArgs e)
        {
            nowyLogToolStripMenuItem.Enabled = false;
            CenterToScreen();
            fillGridsWithData();
            grid_PO_SelectionChanged(sender, e);
            initUserInfoArea();
            RunSim_ResizeEnd(sender, e);
            Focus();
        }
        private void initLogInformation()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + @"\Log\");
            if (Directory.Exists(Environment.CurrentDirectory + @"\Log\"))
                foreach (FileInfo file in di.GetFiles())
                    file.Delete();
            else
                Directory.CreateDirectory(Environment.CurrentDirectory + @"\Log\");

            int len;
            string sysType;
            if (Environment.Is64BitOperatingSystem)
                sysType = "x64";
            else
                sysType = "x32";
            string ipAddrList = String.Empty;
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet && item.OperationalStatus == OperationalStatus.Up)
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            if (ipAddrList == String.Empty)
                                ipAddrList += ip.Address.ToString();
                            else
                                ipAddrList += "\n".PadRight(31, ' ') + ip.Address.ToString();
            logManager.createNewLog(logFile);
            logManager.addToMemory("Uruchomienie symulatora o " + DateTime.Now.ToString("HH:MM:ss:")
            + "\nKomputer w domenie: \"" + Environment.UserDomainName + "\"\nStacja \"" +
            Environment.MachineName + "\" " + sysType + " " + Environment.OSVersion + " OS\nZalogowano jako: \"" +
            Environment.UserName + "\"\n" + "Dostępne interfejsy sieciowe: " + ipAddrList + "\n\n\n", logFile);
        }

        private void fillGridsWithData()
        {
            foreach (MemoryRecord row in List_Memory)
                grid_PO.Rows.Add(row.addr, row.value, row.hex);
            foreach (MicroOperation row in List_MicroOp)
                grid_PM.Rows.Add(row.addr, row.S1, row.D1, row.S2, row.D2, row.S3,
                    row.D3, row.C1, row.C2, row.Test, row.ALU, row.NA);
        }
        private void initUserInfoArea()
        {
            dataGridView_Info.Rows.Add(MainWindow.mark, "Ocena");
            dataGridView_Info.Rows.Add(MainWindow.mistakes, "Błędy");
            dataGridView_Info.Rows.Add("0", "Takt");
            dataGridView_Info.Rows.Add(MainWindow.currnetCycle, "Cykl");
            dataGridView_Info.Enabled = false;
            dataGridView_Info.ClearSelection();

            dgvcs1 = new System.Windows.Forms.DataGridViewCellStyle();
            dgvcs1.ForeColor = Color.Green;
            dataGridView_Info.Rows[0].DefaultCellStyle = dgvcs1;
            DataGridViewCellStyle dgvcs2 = new System.Windows.Forms.DataGridViewCellStyle();
            dgvcs2.ForeColor = Color.Blue;
            dataGridView_Info.Rows[2].DefaultCellStyle = dgvcs2;
            dataGridView_Info.Rows[3].DefaultCellStyle = dgvcs2;
        }
        private void rearrangeTextBoxes()
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

        private void switchLayOut()
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
            new Thread(() =>
            {
                Graphics g = panel_Sim_Control.CreateGraphics();
                draw.drawBackground(ref g);
            }).Start();
        }
        private void RunSim_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
            {
                //poprawić sposób zamykania
                //może dodać przycisk do nowej symulacji?
                DialogResult result = MessageBox.Show("Jeśli przerwiesz pracę teraz stracisz cały postęp.\nNa pewno chcesz zakończyć symulację?", "Symulacja została już uruchomiona", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    foreach (var reg in registers)
                        reg.Value.Enabled = false;
                else
                    e.Cancel = true;
            }
            else
            {
                foreach (var reg in registers)
                    reg.Value.Enabled = false;
                foreach (var sig in flags)
                    sig.Value.Enabled = false;
                foreach (var reg in registers)
                    reg.Value.ReadOnly = true;
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_File_Dialog.Filter = "Logi symulatora|*.log|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj log";
            open_File_Dialog.InitialDirectory = Environment.CurrentDirectory;

            DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
            if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(open_File_Dialog.FileName + "crc");
                    fileInfo.Attributes = FileAttributes.Normal;
                    uint crcFromFile;
                    using (BinaryReader br = new BinaryReader(File.Open(open_File_Dialog.FileName + "crc", FileMode.Open)))
                    {
                        crcFromFile = br.ReadUInt32();
                    }
                    fileInfo.Attributes = FileAttributes.Hidden;
                    uint crc = CRC.ComputeChecksum(File.ReadAllBytes(open_File_Dialog.FileName));
                    if (crcFromFile == crc)
                    {
                        Form log = new Form();
                        RichTextBox rtb = new RichTextBox();
                        log.Controls.Add(rtb);
                        rtb.ReadOnly = true;
                        rtb.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                        rtb.Text = File.ReadAllText(open_File_Dialog.FileName, Encoding.Unicode);
                        log.AutoSize = true;
                        log.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        int width = 200;
                        Graphics g = Graphics.FromHwnd(rtb.Handle);
                        foreach (var line in rtb.Lines)
                        {
                            SizeF f = g.MeasureString(line, rtb.Font);
                            width = (int)(f.Width) > width ? (int)(f.Width) : width;
                        }
                        rtb.Width = width+10;
                        rtb.Height = 600;
                        log.MaximizeBox = false;
                        log.SizeGripStyle = SizeGripStyle.Hide;
                        log.ShowDialog();
                    }
                    else
                    {
                        //ktoś modyfikował log albo skasował sumę kontrolną logu
                    }
                }
                catch (FileNotFoundException)
                {

                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inEditMode)
            {
                foreach (var oldReg in oldRegs)
                {
                    if (registers[oldReg.Key].getInnerValue() != oldReg.Value)
                        logManager.addToMemory("Zmiana zawartości rejestru:"+oldReg.Key+" "+ 
                            oldReg.Value+"=>"+ registers[oldReg.Key].getInnerValue()+"\n", logFile);
                }
                foreach (var reg in registers)
                    reg.Value.Enabled = false;
                foreach (var sig in flags)
                    sig.Value.Enabled = false;
                foreach (var reg in registers)
                    reg.Value.ReadOnly = true;
                toolStripMenu_Edit.Text = "Edytuj rejestry";
                button_Makro.Visible = true;
                button_Micro.Visible = true;
                toolStripMenu_Clear.Enabled = true;
                inEditMode = false;
                label_Status.Text = "Stop";
            }
            else
            {
                oldRegs = new Dictionary<string, short>();
                foreach (var reg in registers)
                {
                    oldRegs.Add(reg.Key, reg.Value.getInnerValue());
                }
                //oldRegs = registers;
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
                toolStripMenu_Edit.Text = "Zakończ edycję";
                button_Makro.Visible = false;
                button_Micro.Visible = false;
                toolStripMenu_Clear.Enabled = false;
                inEditMode = true;
                label_Status.Text = "Edycja";
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var reg in registers)
                reg.Value.resetValue();
            foreach (var flg in flags)
                flg.Value.resetValue();
            dgvcs1.ForeColor = Color.Green;
            MainWindow.mark = 5;
            MainWindow.mistakes = MainWindow.currnetCycle = 0;
            dataGridView_Info.Rows[0].Cells[0].Value = 5;
            dataGridView_Info.Rows[1].Cells[0].Value = dataGridView_Info.Rows[2].Cells[0].Value =
                dataGridView_Info.Rows[3].Cells[0].Value = 0;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                Close();
            });
        }

        private void grid_PO_SelectionChanged(object sender, EventArgs e)
        {
            int idxRow = grid_PO.CurrentCell.RowIndex;
            int idxCol = grid_PO.CurrentCell.ColumnIndex;

            MemoryRecord instruction = List_Memory[idxRow];
            string instructionMnemo = "";
            label1.Text = "Komórka " + idxRow + "\n";
            if (instruction.typ == 1)
            {
                label1.Text += "DANA\n";
                label1.Text += instruction.value.ToString() + "b\n";
                label1.Text += Convert.ToInt16(instruction.value, 2) + "d\n";
                label1.Text += Convert.ToInt16(instruction.value, 2).ToString("X") + "h";
            }
            else if (instruction.typ == 2)
                instructionMnemo = Translator.DecodeInstruction(instruction.OP);
            else if (instruction.typ == 3)
                instructionMnemo = Translator.DecodeInstruction(instruction.AOP);
            string instructionDescription = Translator.GetInsrtuctionDescription(instructionMnemo, instruction.value, instruction.typ);
            label1.Text += instructionDescription;
        }

        private void grid_PM_SelectionChanged(object sender, EventArgs e)
        {
            if (grid_PM.CurrentCell.ColumnIndex == 0)
                grid_PM.ClearSelection();
        }
        private void richTextBox_addText(string tact, string mnemo, string description)
        {
            string text;
            if (richTextBox_Log.Lines.Count() == 0)
                text = tact.PadRight(5, ' ');
            else
                text = '\n' + tact.PadRight(5, ' ');
            int start = richTextBox_Log.TextLength;
            richTextBox_Log.AppendText(text);
            int end = richTextBox_Log.TextLength;
            richTextBox_Log.Select(start, end - start);
            richTextBox_Log.SelectionColor = Color.Red;
            richTextBox_Log.Select(end, 0);
            start = richTextBox_Log.TextLength;
            richTextBox_Log.AppendText(mnemo.PadRight(6, ' ') + description);
            end = richTextBox_Log.TextLength;
            richTextBox_Log.Select(start, end - start);
            richTextBox_Log.SelectionColor = Color.Black;
            richTextBox_Log.Select(end, 0);
            richTextBox_Log.ScrollToCaret();
            //poprawić format logu
            logManager.addToMemory("\t"+tact + " " + mnemo + " " + description + "\n", logFile);
        }
        delegate void simulateCPUCallBack();
        private void button_Makro_Click(object sender, EventArgs e)
        {
            prepareSimulation(false);
        }
        private void button_Micro_Click(object sender, EventArgs e)
        {
            prepareSimulation(true);
        }
        private void prepareSimulation(bool b)
        {
            inMicroMode = b;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.CurrentDirectory;
            while (logFile == "")
            {
                dialog.Filter = "Log symulatora|*.log|Wszystko|*.*";
                dialog.Title = "Utwórz log";
                DialogResult saveFileDialogResult = dialog.ShowDialog();
                if (saveFileDialogResult == DialogResult.OK && dialog.FileName != "")
                {
                    logFile = dialog.FileName;
                    logTimer.Enabled = true;
                    initLogInformation();
                    logManager.addToMemory("========Start symulacji========\n======Zawartość rejestrów======\n", logFile);
                    foreach (var reg in registers.Values)
                        logManager.addToMemory(reg.registerName + "=" + reg.Text + "\n", logFile);
                }
            }
            simulateCPUCallBack cb = new simulateCPUCallBack(simulateCPU);
            new Thread(() => Invoke(cb)).Start();
        }
        private void button_Next_Tact_Click(object sender, EventArgs e)
        {
            button_Next_Tact.Visible = false;
            buttonNextTactClicked = true;
            currentTact = (currentTact + 1) % 8;
            dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
        }
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
                logManager.addToMemory("\t"+registerToCheck + "=" + registers[registerToCheck].getInnerValue() + "\n", logFile);
            }
        }
        private void button_OK_Click(object sender, EventArgs e)
        {
            button_OK.Visible = false;
            checkRegisters();
            buttonOKClicked = true;
        }

        private void RunSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !inEditMode)
            {
                if (isRunning && !inMicroMode)
                    button_OK_Click(sender, (EventArgs)e);
                else if (isRunning && inMicroMode && currentTact != 7)
                    button_Next_Tact_Click(sender, (EventArgs)e);
                else if (isRunning && inMicroMode && currentTact == 7)
                    button_OK_Click(sender, (EventArgs)e);
                else
                    button_Makro_Click(sender, (EventArgs)e);
            }
        }

        private void simulateCPU()
        {
            modeManager.startSim();
            if (currentTact == 0)
                instructionFetch();
            while (isRunning && currentTact > 0)
                executeInstruction();
        }

        private void executeInstruction()
        {
            if (currentTact == 1 && (cells[1, 1] || cells[2, 1] || cells[4, 1] || cells[7, 1]))
                logManager.addToMemory("Takt1:\n", logFile);
            while (currentTact == 1 && (cells[1, 1] || cells[2, 1] || cells[4, 1] || cells[7, 1]))
                exeTact1();
            if (currentTact == 1)
                modeManager.nextTact();
            if (currentTact == 2 && cells[10, 2])
                logManager.addToMemory("Takt2:\n", logFile);
            while (currentTact == 2 && cells[10, 2])
                exeTact2();
            while (currentTact >= 2 && currentTact <= 5)
                modeManager.nextTact();
            if (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                logManager.addToMemory("Takt6:\n", logFile);
            while (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                exeTact6();
            if (currentTact == 6)
                modeManager.nextTact();
            if (currentTact == 7 && (cells[5, 7] || cells[6, 7] || cells[7, 7] || cells[8, 7] || cells[9, 7]))
                logManager.addToMemory("Takt7:\n", logFile);
            while (currentTact == 7 && (cells[5, 7] || cells[6, 7] || cells[7, 7] || cells[8, 7]))
                exeTact7();
            if (currentTact == 7 && cells[9, 7])
                exeTest();
            else if (currentTact == 7 && !isTestPositive)
                currentTact = 8;
            else if (currentTact == 8)
            {
                registers["RAPS"].setActualValue((short)(registers["RAPS"].getInnerValue() + 1));
                registers["RAPS"].setNeedCheck(out registerToCheck);
                grid_PM.CurrentCell = grid_PM[11, registers["RAPS"].getInnerValue()];
                button_OK.Visible = true;
                modeManager.EnDisableButtons();
                registers[registerToCheck].Focus();
                while (buttonOKClicked == false)
                    Application.DoEvents();
                modeManager.EnDisableButtons();
                currentTact = 0;
                dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
                modeManager.stopSim();
                buttonOKClicked = false;
            }
            else if (currentTact == 9)
            {
                registers["LALU"].setInnerAndActual(0);
                registers["RALU"].setInnerAndActual(0);
                currentTact = 0;
                dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
                modeManager.stopSim();
            }
            if (currentTact == 7)
                currentTact = 8;
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

        private void grid_PO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void RunSim_ResizeEnd(object sender, EventArgs e)
        {
            float horizontalRatio = 0.2f, verticalRatio = 0.15f;

            int tempPoWidth = Convert.ToInt32(Width * horizontalRatio);
            panel_PO.Height = Convert.ToInt32(Height * 1.0);
            if (tempPoWidth > 280)
                panel_PO.Width = 280;
            else
                panel_PO.Width = tempPoWidth;

            panel_Left.Width = Convert.ToInt32(Width - panel_PO.Width - 20);
            panel_Left.Height = Convert.ToInt32(Height * 1.0);

            int tempPmHeight = Convert.ToInt32(panel_Left.Height * verticalRatio);
            if (tempPmHeight < 400)
                panel_PM.Height = 400;
            else
                panel_PM.Height = tempPmHeight;
            panel_PM.Width = Convert.ToInt32(panel_Left.Width * 1.0);

            panel_Sim.Width = Convert.ToInt32(panel_Left.Width * 1.0);
            panel_Sim.Height = Convert.ToInt32(panel_Left.Height - panel_PM.Height);

            rearrangeTextBoxes();
            new Thread(() =>
            {
                Graphics g = panel_Sim_Control.CreateGraphics();
                draw.drawBackground(ref g);
            }).Start();

        }

        private void RunSim_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized && !wasMaximized)
            {
                wasMaximized = true;
                RunSim_ResizeEnd(sender, e);
            }
            else if (wasMaximized)
            {
                wasMaximized = false;
                RunSim_ResizeEnd(sender, e);
            }
        }
        private void nowyLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logManager.addToMemory("\n"+ DateTime.Now.ToString("HH:MM:ss:") + "========Stop  symulacji========\n", logFile);
            logManager.clearInMemoryLog();
            logFile = string.Empty;
            //zerowanie rejestrów
        }

        private void RunSim_Paint(object sender, PaintEventArgs e)
        {
            new Thread(() =>
            {
                Graphics g = panel_Sim_Control.CreateGraphics();
                draw.drawBackground(ref g);
            }).Start();
        }

        private void logTimer_Tick(object sender, EventArgs e)
        {
            //dla pliku logu tworzyc ukryty plik z crc
            //jesli symulacja ruszyla okno tylko ukrywac lub zapisywac gdzies stany rejesstrow
            //zapamietac liczbe bledow i ocene, oraz cykl
            new Thread(() =>
            {
                using (BinaryWriter bw = new BinaryWriter(File.Create(Environment.CurrentDirectory + @"\Log\Log_" + DateTime.Now.ToString("HH_mm_ss"))))
                {
                    bw.Write(logManager.GetBuffer());
                }
            }).Start();
        }
    }
}
