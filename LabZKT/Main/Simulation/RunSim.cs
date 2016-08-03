using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class RunSim : Form
    {
        public static short dragValue;
        public static Size hitTest;
        public static bool buttonNextTactClicked = false;
        private int currentTact = 0;
        private bool[,] cells = new bool[11, 8];
        private List<MemoryRecord> List_Memory { get; set; }
        private List<MicroOperation> List_MicroOp { get; set; }
        private MemoryRecord lastRecordFromRRC;
        private Dictionary<string, short> oldRegs;
        private ModeManager modeManager;
        private LogManager logManager;
        private Drawings draw;
        private string logFile = string.Empty, microOpMnemo = string.Empty, registerToCheck = string.Empty;
        private short raps = 0, na = 0;
        private bool isTestPositive = false, isOverflow = false, wasMaximized = false, inEditMode = false,
            buttonOKClicked = false, inMicroMode = false, isRunning = false;
        private Dictionary<string, NumericTextBox> registers;
        private Dictionary<string, BitTextBox> flags;
        private TextBox RBPS;
        private DataGridViewCellStyle dgvcs1;
        public RunSim(ref List<MemoryRecord> List_Memory, ref List<MicroOperation> List_MicroOp, 
            ref Dictionary<string, NumericTextBox> registers,ref Dictionary<string, BitTextBox> flags, 
            ref TextBox RBPS, ref MemoryRecord lastRecordFromRRC)
        {
            this.lastRecordFromRRC = lastRecordFromRRC;
            this.List_Memory = List_Memory;
            this.List_MicroOp = List_MicroOp;
            this.registers = registers;
            this.flags = flags;
            this.RBPS = RBPS;
            InitializeComponent();
        }
        public bool resetBus { get; private set; }
        public Control getSimPanel()
        {
            return panel_Sim_Control;
        }
        private void RunSim_Load(object sender, EventArgs e)
        {
            draw = new Drawings(ref panel_Sim_Control, ref registers, ref flags, ref RBPS);
            modeManager = ModeManager.getInstace(ref registers, ref toolStripMenu_Edit, ref toolStripMenu_Clear, ref toolStripMenu_Exit,
                ref label_Status, ref button_Makro, ref button_Micro, ref dataGridView_Info, ref button_Next_Tact);
            logManager = LogManager.Instance;
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
            Focus();
            RunSim_ResizeEnd(sender, e);
        }
        private void initLogInformation()
        {
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
            if (!isRunning)
            {
                DialogResult result = MessageBox.Show("Zamknąć okno symulacji i powrócic do głównego menu?\nPostęp symulacji nie zostanie utracony", "Symulacja została już uruchomiona", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    e.Cancel = false;
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_File_Dialog.Filter = "Logi symulatora|*.log|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj log";
            open_File_Dialog.InitialDirectory = MainWindow.envPath;

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
                        rtb.Width = width + 10;
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
                        logManager.addToMemory("Zmiana zawartości rejestru:" + oldReg.Key + " " +
                            oldReg.Value + "=>" + registers[oldReg.Key].getInnerValue() + "\n", logFile);
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
            logManager.addToMemory("\t" + tact + " " + mnemo + " " + description + "\n", logFile);
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
            dialog.InitialDirectory = MainWindow.envPath;
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
            modeManager.startSim(out isRunning, cells, out cells);
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
                modeManager.nextTact(inMicroMode, currentTact, out currentTact);
            if (currentTact == 2 && cells[10, 2])
                logManager.addToMemory("Takt2:\n", logFile);
            while (currentTact == 2 && cells[10, 2])
                exeTact2();
            while (currentTact >= 2 && currentTact <= 5)
                modeManager.nextTact(inMicroMode, currentTact, out currentTact);
            if (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                logManager.addToMemory("Takt6:\n", logFile);
            while (currentTact == 6 && (cells[3, 6] || cells[4, 6] || cells[8, 6]))
                exeTact6();
            if (currentTact == 6)
                modeManager.nextTact(inMicroMode, currentTact, out currentTact);
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
                modeManager.stopSim(out isRunning, out inMicroMode);
                buttonOKClicked = false;
            }
            else if (currentTact == 9)
            {
                registers["LALU"].setInnerAndActual(0);
                registers["RALU"].setInnerAndActual(0);
                currentTact = 0;
                dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
                modeManager.stopSim(out isRunning, out inMicroMode);
            }
            if (currentTact == 7)
                currentTact = 8;
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


            Graphics g = panel_Sim_Control.CreateGraphics();
            draw.drawBackground(ref g);
            rearrangeTextBoxes();
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
            logManager.addToMemory("\n" + DateTime.Now.ToString("HH:MM:ss:") + "========Stop  symulacji========\n", logFile);
            logManager.clearInMemoryLog();
            logFile = string.Empty;
            //zerowanie rejestrów
            DialogResult result = MessageBox.Show("Utracisz cały postęp symulacji.\nCzy chcesz zakończyć pracę z obecnym logiem?", "Nowa symulacja", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

            }

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

        }
    }
}
