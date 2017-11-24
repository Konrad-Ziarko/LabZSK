using LabZSK.Controls;
using LabZSK.Memory;
using LabZSK.MicroOperations;
using LabZSK.Other;
using LabZSK.Properties;
using LabZSK.StaticClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Cache;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Trinet.Core.IO.Ntfs;

namespace LabZSK.Simulation {
    public partial class SimView : Form
    {
        #region ADS
        public const int GENERIC_WRITE = 1073741824;
        public const int FILE_SHARE_DELETE = 4;
        public const int FILE_SHARE_WRITE = 2;
        public const int FILE_SHARE_READ = 1;
        public const int OPEN_ALWAYS = 4;
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName,
                                                uint dwDesiredAccess,
                                                uint dwShareMode,
                                                IntPtr lpSecurityAttributes,
                                                uint dwCreationDisposition,
                                                uint dwFlagsAndAttributes,
                                                IntPtr hTemplateFile);
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);
        #endregion
        internal event Action<int, string, string, int> AUpdateForm;
        private string _environmentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK";
        private static Thread serverTimer;
        internal static int appLostFocusCounter = 0;
        //private bool isDebuggerPresent;
        //private MemoryRecord lastRecordFromRRC;//pamietaj aby nullowac po zerowaniu rejestrow albo nowej symulacji, to delete?
        #region Lists and Dictionary
        private List<MemoryRecord> List_Memory = new List<MemoryRecord>();
        private List<MicroOperation> List_MicroOp = new List<MicroOperation>();
        private Dictionary<string, NumericTextBox> registers = new Dictionary<string, NumericTextBox>() {
            {"LK", null }, {"A", null },{"MQ", null },{"X", null },{"RAP", null },{"LALU", null },{"RALU", null },
        {"RBP", null },{"ALU", null },{"BUS", null },{"RR", null },{"LR", null },{"RI", null },
        {"RAPS", null },{"RAE", null },{"L", null },{"R", null },{"SUMA", null }};
        private Dictionary<string, short> oldRegs;
        private TextBox RBPS = new TextBox();
        private Dictionary<string, BitTextBox> flags = new Dictionary<string, BitTextBox>()
        {
            {"MAV", null }, {"IA",null }, {"INT", null }, {"ZNAK",null }, {"XRO", null }, {"OFF", null }
        };
        #endregion
        #region objref
        private PMView pmView;
        private MemView memView;
        private Drawings draw;
        private LogManager logManager;
        private DevConsole devConsole;
        #endregion
        #region Simulation Vars
        private bool canSimulate = true;
        private DateTime simTime;
        private TimeSpan appStart = DateTime.Now.TimeOfDay;
        internal bool isRunning = false;
        private bool isTestPositive = false;
        private bool isOverflow = false;
        private bool buttonOKClicked = false;
        private bool inMicroMode = false;
        private bool buttonNextTactClicked = false;
        private bool resetBus;
        private bool layoutChange = false;
        private bool[,] cells = new bool[11, 8];
        private string logFile = string.Empty, microOpMnemo = string.Empty, registerToCheck = string.Empty, flagToCheck = string.Empty;
        private short raps = 0, na = 0;
        internal int currnetCycle;
        private int mark;
        private int mistakes;
        private int currentTact;
        private bool inEditMode;
        private MemoryRecord selectedInstruction;
        #endregion
        #region DEV vars
        internal bool DEVEND = false;
        internal bool DEVMODE = false;
        internal int DEVVALUE;
        internal string DEVREGISTER;
        internal int DEVCYCLE;
        internal int DEVINCCYCLE;
        internal bool DEVADDCYCLE = false;
        #endregion
        #region Form vars
        private DataGridViewCellStyle dgvcs1;
        private Size windowSize;
        private FormWindowState previousWindowState;
        private FormWindowState currentWindowState;
        #endregion

        public SimView(string filename)
        {
            InitializeComponent();
            richTextBox1.Text = string.Format("UT{0:00}:{1:00}", appStart.Hours, appStart.Minutes);
            var loc = richTextBox1.Location;
            loc.X = 203;
            richTextBox1.Location = loc;

            initLists();
            initFlags();
            initRegisterTextBoxes();
            foreach (var reg in registers)
                reg.Value.Parent = panel_Sim_Control;
            foreach (var sig in flags)
                sig.Value.Parent = panel_Sim_Control;
            RBPS.Parent = panel_Sim_Control;
            RBPS.BackColor = Color.FromArgb(255, Settings.Default.RPS, Settings.Default.GPS, Settings.Default.BPS);

            draw = new Drawings(ref registers, ref flags, ref RBPS);
            draw.addControlToDrawOn(panel_Sim_Control);
            LoadLists();
            rearrangeTextBoxes();

            currentTact = mistakes = 0;
            mark = 5;
            logManager = LogManager.Instance;
            devConsole = new DevConsole(this);

            pmView = new PMView(ref List_MicroOp);
            pmView.AUpdateData += PmView_AUpdateData;

            memView = new MemView(ref List_Memory);
            memView.AUpdateForm += MemView_AUpdateForm;

            string[] split = filename.Split(new[] { "<*>" }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (filename != string.Empty)
                {
                    if (split.Length == 1)
                    {
                        string first = split[0];
                        string[] arr = first.Split('.');

                        if (Regex.Match(arr[1], @"[pP][mM]").Success)
                            pmView.LoadTable(first);
                        else if (Regex.Match(arr[1], @"[pP][oO]").Success)
                            memView.LoadTable(first);

                    }
                    else if (split.Length == 2)
                    {
                        string first = split[0];
                        string second = split[1];

                        string[] arr = first.Split('.');

                        if (Regex.Match(arr[1], @"[pP][mM]").Success)
                            pmView.LoadTable(first);
                        else if (Regex.Match(arr[1], @"[pP][oO]").Success)
                            memView.LoadTable(first);

                        arr = second.Split('.');

                        if (Regex.Match(arr[1], @"[pP][mM]").Success)
                            pmView.LoadTable(second);
                        else if (Regex.Match(arr[1], @"[pP][oO]").Success)
                            memView.LoadTable(second);
                    }
                }
            }
            catch { }
            devConsoleToolStripMenuItem.Enabled = Settings.Default.IsDevConsole;
            closeLogToolStripMenuItem.Enabled = Settings.Default.CanCloseLog;
            setAllStrings();
        }

        private void RunSim_Load(object sender, EventArgs e) {
            Size = new Size(1024, 768);
            CenterToScreen();
            Grid_Mem_SelectionChanged(sender, e);
            initUserInfoArea();
            SimView_ResizeEnd(sender, e);
            Focus();
            windowSize = new Size(Width, Height);

            Settings.Default.CanEditOptions = false;
            Settings.Default.CanCloseLog = false;
            Settings.Default.IsDevConsole = false;
            try {
                string encryptedString = File.ReadAllText(_environmentPath + "\\LabZSK.cfg");
                string newAppSettings = StaticClasses.Encryptor.Decrypt(encryptedString);

                string[] tmp = newAppSettings.Split(new[] { "<?>" }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length != 6)
                    throw new Exception();
                if (tmp[0] != "False" && tmp[0] != "True" || tmp[1] != "False" && tmp[1] != "True" || tmp[2] != "False" && tmp[2] != "True")
                    throw new Exception();
                if (Convert.ToInt32(tmp[3]) < 0 || Convert.ToInt32(tmp[4]) < 0 || Convert.ToInt32(tmp[5]) < 0)
                    throw new Exception();

                Settings.Default.CanEditOptions = Convert.ToBoolean(tmp[0]);
                Settings.Default.CanCloseLog = Convert.ToBoolean(tmp[1]);
                Settings.Default.IsDevConsole = Convert.ToBoolean(tmp[2]);
                Settings.Default.FirstMark = Convert.ToInt32(tmp[3]);
                Settings.Default.SecondMark = Convert.ToInt32(tmp[4]);
                Settings.Default.ThirdMark = Convert.ToInt32(tmp[5]);
            }
            catch {
                MessageBox.Show("Wczytywanie konfiguracji zakończyło się niepowodzeniem");
            }

            serwerToolStripMenuItem.Visible = Settings.Default.isServerVisible;
            if (Settings.Default.simStop > DateTime.Now) {
                Settings.Default.simStart = Settings.Default.simStop;
            }
            else {
                Settings.Default.simStart = DateTime.Now;
            }
            Settings.Default.Save();
            AddNewEntryToHiddenLog();

            AUpdateForm += memView.MemUpadateFromSimView;
        }

        private static void AddNewEntryToHiddenLog() {
            FileStream fs = null;
            try {
                FileInfo fileInfo = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (!fileInfo.AlternateDataStreamExists("crc")) {
                    var stream = CreateFile(System.Reflection.Assembly.GetExecutingAssembly().Location + ":crc", GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_ALWAYS, 0, IntPtr.Zero);
                    if (stream != IntPtr.Zero) {
                        CloseHandle(stream);
                    }
                }
                fs = fileInfo.GetAlternateDataStream("crc").OpenWrite();
                using (StreamWriter sw = new StreamWriter(fs)) {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    string permType = "S";
                    if (Settings.Default.CanEditOptions || Settings.Default.IsDevConsole || Settings.Default.CanCloseLog)
                        permType = "N";
                    sw.Write(permType + "/ " + Settings.Default.simStart.ToString("yyyy-MM-dd  HH:mm:ss") + "(Czas systemowy: " + DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + ")\n");
                }
                fs.Close();
            }
            catch { }
            finally {
                if (fs != null)
                    fs.Close();
            }
        }

        private void RunSim_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
                e.Cancel = true;
            else
            {
                DialogResult dr = MessageBox.Show(new Form() { TopMost = true }, Strings.areYouSureExit, Strings.areYouSureExitTitle, MessageBoxButtons.YesNo);
                //DialogResult dr = MessageBox.Show(Strings.areYouSureExit, Strings.areYouSureExitTitle, MessageBoxButtons.YesNo) ;
                if (dr == DialogResult.Yes)
                {
                    if (Settings.Default.simStart > DateTime.Now)
                    {
                        Settings.Default.simStop = Settings.Default.simStart;
                    }
                    else
                    {
                        Settings.Default.simStop = DateTime.Now;
                    }
                    Settings.Default.Save();
                    ACloseLog();
                    string currentAppSettings = Settings.Default.CanEditOptions.ToString();
                    currentAppSettings += "<?>" + Settings.Default.CanCloseLog.ToString();
                    currentAppSettings += "<?>" + Settings.Default.IsDevConsole.ToString();
                    currentAppSettings += "<?>" + Settings.Default.FirstMark;
                    currentAppSettings += "<?>" + Settings.Default.SecondMark;
                    currentAppSettings += "<?>" + Settings.Default.ThirdMark;


                    string encryptedstring = Encryptor.Encrypt(currentAppSettings);
                    try
                    {
                        File.WriteAllText(_environmentPath + "\\LabZSK.cfg", encryptedstring);
                    }
                    catch { }
                }
                else if (dr == DialogResult.No)
                    e.Cancel = true;
            }
        }
        #region init
        private void initLists()
        {
            Directory.CreateDirectory(_environmentPath + "\\TMP");
            Directory.CreateDirectory(_environmentPath + "\\Log");
            Directory.CreateDirectory(_environmentPath + "\\PO");
            Directory.CreateDirectory(_environmentPath + "\\PM");
            for (int i = 0; i < 256; i++)
                List_MicroOp.Add(new MicroOperation(i, "", "", "", "", "", "", "", "", "", "", ""));
            for (int i = 0; i < 256; i++)
                List_Memory.Add(new MemoryRecord(i, "", "", 0));
        }
        private void initFlags()
        {
            flags["MAV"] = new BitTextBox(130, 125, null, "MAV");
            flags["IA"] = new BitTextBox(140, 125, null, "IA");
            flags["INT"] = new BitTextBox(290, 125, null, "INT");
            flags["ZNAK"] = new BitTextBox(450, 65, null, Strings.sign);
            flags["XRO"] = new BitTextBox(460, 65, null, "XRO");
            flags["OFF"] = new BitTextBox(470, 65, null, "OFF");
            foreach (var sig in flags)
                sig.Value.Enabled = false;
        }
        private void initRegisterTextBoxes()
        {
            registers["LK"] = new NumericTextBox("LK", 5, 25, null);
            registers["A"] = new NumericTextBox("A", 155, 25, null);
            registers["MQ"] = new NumericTextBox("MQ", 305, 25, null);
            registers["X"] = new NumericTextBox("X", 455, 25, null);
            registers["RAP"] = new NumericTextBox("RAP", 5, 65, null);
            registers["LALU"] = new NumericTextBox("LALU", 155, 65, null);
            registers["RALU"] = new NumericTextBox("RALU", 305, 65, null);
            registers["RBP"] = new NumericTextBox("RBP", 5, 125, null);
            registers["ALU"] = new NumericTextBox("ALU", 155, 125, null);
            registers["BUS"] = new NumericTextBox("BUS", 5, 185, null);
            registers["RR"] = new NumericTextBox("RR", 155, 225, null);
            registers["LR"] = new NumericTextBox("LR", 305, 225, null);
            registers["RI"] = new NumericTextBox("RI", 455, 225, null);
            registers["RAPS"] = new NumericTextBox("RAPS", 305, 305, null);
            registers["RAE"] = new NumericTextBox("RAE", 455, 305, null);

            registers["L"] = new NumericTextBox("L", 205, 305, null);
            registers["R"] = new NumericTextBox("R", 355, 305, null);
            registers["SUMA"] = new NumericTextBox("SUMA", 285, 355, null);
            registers["L"].Visible = false;
            registers["R"].Visible = false;
            registers["SUMA"].Visible = false;
            //registers["RBPS"].Visible = false;
            //registers["RAPS"].Visible = false;
            //registers["RAE"].Visible = false;

            foreach (var reg in registers)
                reg.Value.Enabled = false;
            RBPS.Enabled = false;
            RBPS.Size = new Size(130, 1);
            RBPS.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            RBPS.Text = "000000000000h";
        }
        private void LoadLists()
        {
            foreach (MemoryRecord row in List_Memory)
                Grid_Mem.Rows.Add(row.addr, row.value, row.hex);
            foreach (MicroOperation row in List_MicroOp)
                Grid_PM.Rows.Add(row.addr, row.S1, row.D1, row.S2, row.D2, row.S3,
                    row.D3, row.C1, row.C2, row.Test, row.ALU, row.NA);
        }
        private void rearrangeTextBoxes()
        {
            int horizontalGap = Convert.ToInt32(0.25 * panel_Sim_Control.Width);
            int verticalGap = Convert.ToInt32(0.2 * panel_Sim_Control.Height);
            var size = registers["LK"].Size;
            int locY = (verticalGap - 27) / 2;
            locY = locY < 32 ? 32 : locY;
            registers["LK"].SetXY((horizontalGap - 130) / 2, locY);
            registers["A"].SetXY(horizontalGap + (horizontalGap - 130) / 2, locY);
            registers["MQ"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, locY);
            registers["X"].SetXY(horizontalGap * 3 + (horizontalGap - 130) / 2, locY);

            locY = verticalGap + (verticalGap - 27) / 4;
            registers["RAP"].SetXY((horizontalGap - 130) / 2, locY);
            registers["LALU"].SetXY(horizontalGap + (horizontalGap - 130) / 2, locY);
            registers["RALU"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, locY);

            int gapFromRalu = panel_Sim_Control.Width - registers["RALU"].Location.X - registers["RALU"].Width - 60;
            flags["ZNAK"].SetXY(panel_Sim_Control.Width - gapFromRalu, verticalGap + (verticalGap - 27));
            flags["XRO"].SetXY(panel_Sim_Control.Width - gapFromRalu * 2 / 3, verticalGap + (verticalGap - 27));
            flags["OFF"].SetXY(panel_Sim_Control.Width - gapFromRalu / 3, verticalGap + (verticalGap - 27));
            flags["MAV"].SetXY(panel_Sim_Control.Width - gapFromRalu, verticalGap * 2 + (verticalGap - 27) * 2 / 4);
            flags["IA"].SetXY(panel_Sim_Control.Width - gapFromRalu * 2 / 3, verticalGap * 2 + (verticalGap - 27) * 2 / 4);
            flags["INT"].SetXY(panel_Sim_Control.Width - gapFromRalu / 3, verticalGap * 2 + (verticalGap - 27) * 2 / 4);

            registers["RBP"].SetXY((horizontalGap - 130) / 2, verticalGap * 2 + (verticalGap - 27) * 1 / 4);
            registers["ALU"].SetXY((registers["RALU"].Location.X - registers["LALU"].Location.X + 130) / 2
                - 65 + registers["LALU"].Location.X, verticalGap * 2 + (verticalGap - 27) * 1 / 4);

            registers["BUS"].SetXY((horizontalGap - 130) / 2, verticalGap * 3);

            locY = verticalGap * 3 + (verticalGap - 27) / 2;
            locY = locY < registers["BUS"].Location.Y + 35 ? registers["BUS"].Location.Y + 35 : locY;
            registers["RR"].SetXY(horizontalGap + (horizontalGap - 130) / 2, locY);
            registers["LR"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, locY);
            registers["RI"].SetXY(horizontalGap * 3 + (horizontalGap - 130) / 2, locY);

            var loc = RBPS.Location;
            loc.X = horizontalGap + (horizontalGap - 130) / 2;
            loc.Y = verticalGap * 4 + (verticalGap - 27) / 4;
            RBPS.Location = loc;
            locY = loc.Y;
            registers["RAPS"].SetXY(horizontalGap * 2 + (horizontalGap - 130) / 2, locY);
            registers["RAE"].SetXY(horizontalGap * 3 + (horizontalGap - 130) / 2, locY);

            locY = verticalGap * 4;
            registers["L"].SetXY(horizontalGap + (horizontalGap - 130) * 3 / 2, locY);
            registers["R"].SetXY(horizontalGap * 2 + (horizontalGap - 130) * 3 / 2, locY);
            registers["SUMA"].SetXY((registers["R"].Location.X - registers["L"].Location.X + 130) / 2
                - 65 + registers["L"].Location.X, locY + (verticalGap - 27) * 5 / 6);
        }
        private void initUserInfoArea()
        {
            dataGridView_Info.Rows.Add(Strings.mark, mark);
            dataGridView_Info.Rows.Add(Strings.mistakes, mistakes);
            dataGridView_Info.Rows.Add(Strings.tact, "0");
            dataGridView_Info.Rows.Add(Strings.cycle, currnetCycle);
            dataGridView_Info.Enabled = false;
            dataGridView_Info.ClearSelection();

            dgvcs1 = new DataGridViewCellStyle();
            dgvcs1.ForeColor = Color.Green;
            dataGridView_Info.Rows[0].DefaultCellStyle = dgvcs1;
            DataGridViewCellStyle dgvcs2 = new DataGridViewCellStyle();
            dgvcs2.ForeColor = Color.Blue;
            dataGridView_Info.Rows[2].DefaultCellStyle = dgvcs2;
            dataGridView_Info.Rows[3].DefaultCellStyle = dgvcs2;
        }
        internal void setAllStrings()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);
            button_Show_Log.Text = Strings.showLogButton;
            button_End_Edit.Text = Strings.endEditRegisters;
            button_OK.Text = Strings.okButton;
            button_Next_Tact.Text = Strings.nextTactButton;
            button_Micro.Text = Strings.micro.ToUpper();


            toolStripMenu_Main.Text = Strings.simulationToolStrip;
            toolStripMenu_Exit.Text = Strings.exitToolStrip;
            toolStripMenu_Show_Log.Text = Strings.showLogToolStrip;
            if (inEditMode)
                toolStripMenu_Edit.Text = Strings.endEditRegisters;
            else
                toolStripMenu_Edit.Text = Strings.editRegisters;
            toolStripMenu_Clear.Text = Strings.clearRegistersToolStrip;
            closeLogToolStripMenuItem.Text = Strings.closeLogToolStrip;
            devConsoleToolStripMenuItem.Text = Strings.devConsoleToolStrip;
            settingsToolStripMenuItem.Text = Strings.settingsToolStrip;

            microToolStripMenuItem.Text = Strings.microToolStrip;
            editpmToolStripMenuItem.Text = Strings.editPMToolStrip;
            loadpmToolStripMenuItem.Text = Strings.loadPMToolStrip;
            memToolStripMenuItem.Text = Strings.memToolStrip;
            editmemToolStripMenuItem.Text = Strings.editMemToolStrip;
            loadmemToolStripMenuItem.Text = Strings.loadMemToolStrip;
            aboutToolStripMenuItem.Text = Strings.authorToolStrip;
            zapiszToolStripMenuItem.Text = zapiszToolStripMenuItem2.Text = Strings.saveToolStrip;
            drukujToolStripMenuItem.Text = drukujToolStripMenuItem1.Text = Strings.printToolStrip;


            try
            {
                dataGridView_Info.Rows[0].Cells[0].Value = Strings.mark;
                dataGridView_Info.Rows[1].Cells[0].Value = Strings.mistakes;
                dataGridView_Info.Rows[2].Cells[0].Value = Strings.tact;
                dataGridView_Info.Rows[3].Cells[0].Value = Strings.cycle;
            }
            catch { }

            Grid_PM.Columns[0].HeaderText = Grid_Mem.Columns[0].HeaderText = Strings.cellAddressViewGrid;
            Grid_Mem.Columns[1].HeaderText = Strings.cellValueViewGrid;
            this.Text = Strings.SimulationTitle;
        }
        #endregion
        #region SubFormUpdate
        private void PmView_AUpdateData(int row, int col, string str)
        {
            addTextToLog("PM[".PadLeft(14, ' ')
                + row + "][" + List_MicroOp[row].getColumnName(col) + "] = \"" + List_MicroOp[row].getColumn(col)
                + "\"  -zmiana->  PM[" + row + "][" + List_MicroOp[row].getColumnName(col) + "] = \"" + str + "\"\n");
            List_MicroOp[row].setValue(col, str);
            Grid_PM[col, row].Value = List_MicroOp[row].getColumn(col);
        }
        private void MemView_AUpdateForm(int row, string binary, string hex, int type)
        {
            addTextToLog(("PAO[" + row + "]").PadLeft(17, ' ') + " = 0x" + List_Memory[row].hex.PadLeft(4, '0') + "  -zmiana->  PAO[" + row + "] = 0x" + hex + "\n");
            List_Memory[row] = new MemoryRecord(row, binary, hex, type);
            Grid_Mem[1, row].Value = List_Memory[row].value;
            Grid_Mem[2, row].Value = List_Memory[row].hex;
            Grid_Mem.ClearSelection();
            Grid_Mem.CurrentCell = Grid_Mem.Rows[row].Cells[1];
            Grid_Mem.Rows[row].Selected = true;
            //Grid_Mem.FirstDisplayedScrollingRowIndex = row;
            
        }
        #endregion
        #region Buttons
        private void DisableButtons() {
            foreach (var reg in registers)
                reg.Value.Enabled = false;
            if (registers["SUMA"].Visible)
                registers["RAE"].BackColor = Color.FromArgb(255, 170, 170, 170);
        }
        private void EnableButtons()
        {
            foreach (var reg in registers)
                reg.Value.Enabled = true;
            if (registers["SUMA"].Visible)
                registers["RAE"].BackColor = Color.FromArgb(255, 170, 170, 170);
        }
        private void waitForButton()
        {
            if (!DEVMODE)
                while (buttonOKClicked == false)
                    Application.DoEvents();
            else
            {
                Application.DoEvents();
                Thread.Sleep(Settings.Default.Delay);
                if (registerToCheck != "")
                    registers[registerToCheck].setInnerValue(registers[registerToCheck].valueWhichShouldBeMovedToRegister);
                validateRegisters();
            }
        }
        internal void button_Makro_Click(object sender, EventArgs e)
        {
            if(!inEditMode && canSimulate)
                if (!isRunning)
                {
                    bool back = false;
                    toolStripMenu_Edit.Enabled = false;
                    toolStripMenu_Exit.Enabled = false;
                    button_Makro.Visible = false;
                    button_Micro.Visible = false;
                    toolStripMenu_Clear.Enabled = false;
                    label_Status.Text = Strings.startMode;
                    label_Status.ForeColor = Color.Red;
                    button_Show_Log.Visible = false;
                    prepareSimulation(false, out back);
                    if (back)
                    {
                        richTextBox_Log.Text = "";
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
                }
        }
        private void button_Micro_Click(object sender, EventArgs e)
        {
            if(!inEditMode || canSimulate)
                if (!isRunning)
                {
                    bool back = false;
                    toolStripMenu_Edit.Enabled = false;
                    toolStripMenu_Exit.Enabled = false;
                    button_Makro.Visible = false;
                    button_Micro.Visible = false;
                    toolStripMenu_Clear.Enabled = false;
                    label_Status.Text = Strings.stopMode;
                    label_Status.ForeColor = Color.Red;
                    button_Show_Log.Visible = false;
                    prepareSimulation(true, out back);
                    if (back)
                    {
                        richTextBox_Log.Text = "";
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
                }
        }
        private void button_Next_Tact_Click(object sender, EventArgs e)
        {
            button_Next_Tact.Visible = false;
            buttonNextTactClicked = true;
            dataGridView_Info.Rows[2].Cells[1].Value = currentTact;
        }
        private void button_OK_Click(object sender, EventArgs e)
        {
            button_OK.Visible = false;
            buttonOKClicked = true;
            validateRegisters();
            dataGridView_Info[1, 1].Value = mistakes;
            dataGridView_Info[1, 0].Value = mark;
            if (mistakes >= Settings.Default.ThirdMark)
                dgvcs1.ForeColor = Color.Red;
        }
        private void button_Show_Log_Click(object sender, EventArgs e)
        {
            AShowCurrentLog();
        }
        private void button_End_Edit_Click(object sender, EventArgs e)
        {
            SwitchEditMode();
        }
        #endregion
        #region Registers
        private void validateRegister()
        {
            button_OK.Visible = true;
            EnableButtons();
            try
            {
                registers[registerToCheck].Focus();
            }
            catch { }
            waitForButton();
            DisableButtons();
            buttonOKClicked = false;
        }
        public void validateRegisters()
        {
            short badValue;
            if (registerToCheck != "")
            {
                if (!registers[registerToCheck].validateRegisterValue(out badValue))
                {
                    new Thread(SystemSounds.Beep.Play).Start();
                    if (registerToCheck == "RAPS")
                    {
                        addTextToLog("\n" + Strings.mistake.PadLeft(14, ' ') + "(" + (mistakes + 1) + "): " + registerToCheck + " = " + badValue + " / " + Convert.ToString(badValue, 16).ToUpper() +
                        "h (" + Strings.correct + " " + registerToCheck + " = " + registers[registerToCheck].innerValue + " / " +
                        Convert.ToString(registers[registerToCheck].innerValue, 16).ToUpper() + "h)\n\n");
                    }
                    else
                    {
                        addTextToLog(Strings.mistake.PadLeft(14, ' ') + "(" + (mistakes + 1) + "): " + registerToCheck + " = " + badValue + " / " + Convert.ToString(badValue, 16).ToUpper() +
                        "h (" + Strings.correct + " " + registerToCheck + " = " + registers[registerToCheck].innerValue + " / " +
                        Convert.ToString(registers[registerToCheck].innerValue, 16).ToUpper() + "h)\n\n");
                    }
                    mistakes++;
                    if (mistakes >= Settings.Default.ThirdMark)
                        mark = 2;
                    else if (mistakes >= Settings.Default.SecondMark)
                        mark = 3;
                    else if (mistakes >= Settings.Default.FirstMark)
                        mark = 4;
                    else
                        mark = 5;
                }
                else
                {
                    if (registerToCheck == "RAPS")
                    {
                        addTextToLog("\n" + registerToCheck.PadLeft(15, ' ') + " = " + registers[registerToCheck].innerValue +
                        " / " + Convert.ToString(registers[registerToCheck].innerValue, 16).ToUpper() + "h\n");
                    }
                    else
                    {
                        addTextToLog(registerToCheck.PadLeft(15, ' ') + " = " + registers[registerToCheck].innerValue +
                        " / " + Convert.ToString(registers[registerToCheck].innerValue, 16).ToUpper() + "h\n");
                    }
                }
                registerToCheck = "";
            }
            if (flagToCheck != "")
            {
                addTextToLog(flagToCheck.PadLeft(15, ' ') + " = " + flags[flagToCheck].innerValue + "\n");
                flagToCheck = "";
            }
        }
        private void testAndSet(string register, short setValue)
        {
            registers[register].setActualValue(setValue);
            registers[register].setNeedCheck(out registerToCheck);
        }
        private void SwitchEditMode()
        {
            if (inEditMode)
            {
                panel_Control.Focus();
                ALeaveEditMode();
                toolStripMenu_Edit.Text = Strings.editRegisters;
                button_Makro.Visible = true;
                button_Micro.Visible = true;
                toolStripMenu_Clear.Enabled = true;
                inEditMode = false;
                label_Status.Text = Strings.stopMode;
                button_End_Edit.Visible = false;
            }
            else
            {
                AEnterEditMode();
                toolStripMenu_Edit.Text = Strings.endEditRegisters;
                button_Makro.Visible = false;
                button_Micro.Visible = false;
                toolStripMenu_Clear.Enabled = false;
                inEditMode = true;
                label_Status.Text = Strings.editMode;
                button_End_Edit.Visible = true;
            }
        }
        public void ALeaveEditMode()
        {
            registers["LK"].setInnerAndExpectedValue((short)(registers["LK"].innerValue & 127));
            foreach (var oldReg in oldRegs)
            {
                if (registers[oldReg.Key].innerValue != oldReg.Value)
                    addTextToLog("".PadLeft(11, ' ') + oldReg.Key + " = " +
                        oldReg.Value + "  -zmiana->  "+ oldReg.Key + " = " + registers[oldReg.Key].innerValue + "\n");
            }
            foreach (var reg in registers)
                reg.Value.Enabled = false;
            foreach (var sig in flags)
                sig.Value.Enabled = false;
            foreach (var reg in registers)
                reg.Value.ReadOnly = true;
        }
        public void AEnterEditMode()
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
        #endregion
        #region Log
        public void ACloseLog()
        {
            addTextToLog("\n" + DateTime.Now.ToString("HH:mm.ss").PadLeft(20, ' ') + "\n" + Strings.simStop + "\n" +
                Strings.mark + ": " + mark + "   " + Strings.mistakes + ": " + mistakes + "\n");
            logManager.clearInMemoryLog();
            logManager.closeConnection();
            logFile = string.Empty;
            logManager.closeLog();
        }
        public void initLogInformation()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            string ipAddrList = string.Empty;
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                if (item.NetworkInterfaceType == NetworkInterfaceType.Ethernet && item.OperationalStatus == OperationalStatus.Up)
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            if (ipAddrList == string.Empty)
                                ipAddrList += ip.Address.ToString();
                            else
                                ipAddrList += "\n".PadRight(31, ' ') + ip.Address.ToString();
            logManager.createNewLog(logFile);
            Settings.Default.Save();
            addTextToLog(Strings.programStart + " " + Settings.Default.simStart + "\n");
            addTextToLog(
                //Strings.pcInDomain + ": \"" + Environment.UserDomainName + "\"\n" +
                Strings.machineName + " \"" + Environment.MachineName + "\" \n" +
                Strings.loggedAs + ": \"" + Environment.UserName + "\"\n" +
                Strings.appVersion + ": " + version + "\n" +
                Strings.networkInterfaces + ": " + ipAddrList + "\n\n");
            if (Settings.Default.CanCloseLog)
                addTextToLog(Strings.logClosingAllowed + "\n");
            if (Settings.Default.CanEditOptions)
                addTextToLog(Strings.canEditSettings + "\n");
            if (Settings.Default.IsDevConsole || Settings.Default.CanEditOptions)
            //if (!Convert.ToBoolean(ConfigurationManager.AppSettings["ApplicationForStudents"]))
                addTextToLog(Strings.notForStudents + "\n");
        }
        internal void AShowCurrentLog()
        {
            if (logFile != string.Empty && logFile != "")
                ShowLog(logFile);
        }
        internal void ShowLog(string pathToLog)
        {
            if (logManager.checkLogIntegrity(pathToLog))
            {
                {
                    Form log = new Form();
                    log.Text = Strings.viewLogFile;
                    log.Icon = Resources.Logo_WAT1;
                    RichTextBox rtb = new FastRichBox();
                    rtb.WordWrap = false;
                    log.Controls.Add(rtb);
                    rtb.ReadOnly = true;
                    rtb.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
                    rtb.Text = File.ReadAllText(pathToLog, Encoding.Unicode);
                    rtb.Text = rtb.Text.Remove(rtb.Text.Length - 2, 2);
                    //log.AutoSize = true;
                    //log.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    int width = 200;
                    Graphics g = Graphics.FromHwnd(rtb.Handle);
                    foreach (var line in rtb.Lines)
                    {
                        SizeF f = g.MeasureString(line, rtb.Font);
                        width = (int)(f.Width) > width ? (int)(f.Width) : width;
                    }
                    rtb.Dock = DockStyle.Fill;

                    log.Width = width + 40;
                    log.Height = 600;
                    log.MaximizeBox = false;
                    //log.SizeGripStyle = SizeGripStyle.Hide;

                    Regex regExp = new Regex(@"(={2}.+==|" + Strings.mistake + @".+\s|" + Strings.canEditSettings + @"\s|" + Strings.criticalError + @"\s|" + Strings.notForStudents + @"\s)");
                    foreach (Match match in regExp.Matches(rtb.Text))
                    {
                        rtb.Select(match.Index, match.Length);
                        rtb.SelectionColor = Color.Red;
                    }
                    regExp = new Regex(@"AUTO:.+\s");
                    foreach (Match match in regExp.Matches(rtb.Text))
                    {
                        rtb.Select(match.Index, match.Length);
                        rtb.SelectionColor = Color.MediumVioletRed;
                    }
                    regExp = new Regex(@"(" + Strings.registerHasChanged + @".+\s|" + "-zmiana->" + @"\s)");
                    foreach (Match match in regExp.Matches(rtb.Text))
                    {
                        rtb.Select(match.Index, match.Length);
                        rtb.SelectionColor = Color.OrangeRed;
                    }
                    regExp = new Regex(@"(={6}.+=|" + @".+CEA.+\s|" + @".+RRC.+\s|" + @".+CWC.+\s|" + @".+IWC.+\s|" + @".+END.+\s+)");
                    foreach (Match match in regExp.Matches(rtb.Text))
                    {
                        rtb.Select(match.Index, match.Length);
                        rtb.SelectionColor = Color.Blue;
                    }
                    regExp = new Regex(@"(={8}.+=|"+ Strings.macro + @"\s|" + Strings.micro + @"\s)");
                    foreach (Match match in regExp.Matches(rtb.Text))
                    {
                        rtb.Select(match.Index, match.Length);
                        rtb.SelectionColor = Color.Green;
                    }
                    rtb.Select(0, 0);
                    log.ShowDialog();
                }
            }
            else
                MessageBox.Show(Strings.logInconsistent);
        }
        private DateTime GetNISTDate(CancellationToken token, out string timeString)
        {
            DateTime dateTime = DateTime.MinValue;
            timeString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader stream = new StreamReader(response.GetResponseStream());
                    string html = stream.ReadToEnd();//<timestamp time=\"1395772696469995\" delay=\"1395772696469995\"/>
                    string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                    double milliseconds = Convert.ToInt64(time) / 1000.0;
                    dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
                }
            }
            catch
            {
                timeString = "";
            }
            if (timeString != "")
                timeString = dateTime.ToString("HH:mm.ss");
            return dateTime;
        }
        private void addTextToLog(string lineOfText)
        {
            if (logManager != null)
            {
                logManager.addToMemory(lineOfText);
            }
        }
        internal void AddToLogAndMiniLog(string tact, string mnemo, string description)
        {
            if (mnemo == "END")
                addTextToLog((tact.PadLeft(8, ' ') + " | ") + mnemo.PadLeft(4, ' ') + " : (" + Strings.cycle + " " + currnetCycle + ") " + description + " (" + DateTime.Now.ToString("HH:mm.ss") + ")\n");
            else if (mnemo == "TINT")
                addTextToLog((tact.PadLeft(8, ' ') + " | ") + mnemo.PadLeft(4, ' ') + " : " + description + "(INT ?= 0)\n");
            else
                addTextToLog((tact.PadLeft(8, ' ') + " | ") + mnemo.PadLeft(4, ' ') + " : " + description + "\n");
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
            
        }
        #endregion
        #region FormActions
        private void RunSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DEVMODE)
            {
                DEVEND = true;
            }

            if (e.KeyChar == (char)Keys.Enter && !inEditMode)
            {
                if (isRunning && !inMicroMode && button_OK.Visible)
                    button_OK_Click(sender, e);
                else if (isRunning && inMicroMode && currentTact != 7 && button_Next_Tact.Visible)
                    button_Next_Tact_Click(sender, e);
                else if (isRunning && inMicroMode && currentTact == 7 && button_OK.Visible)
                    button_OK_Click(sender, e);
                else if (button_Makro.Visible)
                    button_Makro_Click(sender, e);
            }
            else if (e.KeyChar == (char)Keys.Escape && inEditMode)
            {
                SwitchEditMode();
            }
            else if (e.KeyChar == (char)Keys.Escape && isRunning)
            {
                //przerwanie pracy
                /*
                DialogResult dr = MessageBox.Show(Strings.areYouSureExit, Strings.areYouSureExitTitle, MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    button_Next_Tact.Visible = false;
                    button_Makro.Visible = false;
                    buttonOKClicked = true;
                    //if (registers["SUMA"].Visible)
                    switchLayOut();
                    DisableButtons();
                    currentTact = 0;
                    stopSim();
                    logManager.addToMemory("\n" + Strings.simulationBreak + "\n");
                }
                */
            }

        }
        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);
        private void Grid_Mem_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (char.IsLetterOrDigit((char)e.KeyCode) || e.KeyCode == Keys.OemMinus)
            {
                Grid_Mem.ReadOnly = false;
                int nonVirtualKey = MapVirtualKey((uint)e.KeyCode, 2);
                char mappedChar = Convert.ToChar(nonVirtualKey);
                if ((mappedChar >= 'a' && mappedChar <= 'f') || (mappedChar >= 'A' && mappedChar <= 'F') || (mappedChar >= '0' && mappedChar <= '9'))
                    Grid_Mem.CurrentCell.Value = mappedChar;
                else if (e.KeyCode == Keys.OemMinus)
                    Grid_Mem.CurrentCell.Value = (char)45;
                else
                    Grid_Mem.CurrentCell.Value = "";
                Grid_Mem.BeginEdit(false);
            }
            else if (e.KeyCode == Keys.Enter)
                Grid_Mem.EndEdit();
            */
        }
        private void RunSim_SizeChanged(object sender, EventArgs e)
        {
            var tmp = panel_Sim_Control.BackgroundImage;
            panel_Sim_Control.BackgroundImage = null;
            previousWindowState = currentWindowState;
            currentWindowState = WindowState;
            if (previousWindowState != currentWindowState)
            {
                if (currentWindowState == FormWindowState.Maximized && previousWindowState == FormWindowState.Normal)
                    SimView_ResizeEnd(this, new EventArgs());
                else if (currentWindowState == FormWindowState.Normal && previousWindowState == FormWindowState.Maximized)
                    SimView_ResizeEnd(this, new EventArgs());
                else
                    panel_Sim_Control.BackgroundImage = tmp;
            }
            else
                panel_Sim_Control.BackgroundImage = tmp;
        }
        private void SimView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.None)
            {
                //e.Handled = true;
            }
            if (e.Control && e.Shift && e.KeyCode == Keys.R) {
                showAllUpTimes();
            }
            if (e.Control && e.Shift && e.KeyCode == Keys.L) {
                logManager.showInMemoryLog();
            }
            if(e.Control && e.Shift && e.KeyCode == Keys.V) {
                checkAppVersion();
            }
        }

        private void checkAppVersion() {
            new Thread(() => {
                WebClient client = new WebClient();
                try {
                    string downloadString = client.DownloadString("https://github.com/Konrad-Ziarko/LabZSK/tree/master");
                    int index = downloadString.IndexOf("LabZSK version");

                    string version = downloadString.Substring(index + 15, 7);
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                    if (version.Equals(fvi.FileVersion))
                        MessageBox.Show("OK!");
                    else
                        MessageBox.Show(version);
                }
                catch { MessageBox.Show("Connection error"); }

            }).Start();
        }

        private void showAllUpTimes() {
            FileStream fs = null;
            try {
                FileInfo fileInfo = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (!fileInfo.AlternateDataStreamExists("crc")) {
                    var stream = CreateFile(System.Reflection.Assembly.GetExecutingAssembly().Location + ":crc", GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_ALWAYS, 0, IntPtr.Zero);
                    if (stream != IntPtr.Zero) {
                        CloseHandle(stream);
                    }
                }
                fs = fileInfo.GetAlternateDataStream("crc").OpenRead();
                string txt = string.Empty;
                using (StreamReader sr = new StreamReader(fs)) {
                    txt = sr.ReadToEnd();
                }
                fs.Close();

                Form log = new Form();
                log.Text = "Uruchomienia";
                log.Icon = Resources.Logo_WAT1;
                RichTextBox rtb = new FastRichBox();
                rtb.WordWrap = false;
                log.Controls.Add(rtb);
                rtb.ReadOnly = true;
                rtb.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
                rtb.Text = txt;
                int width = 200;
                Graphics g = Graphics.FromHwnd(rtb.Handle);
                foreach (var line in rtb.Lines) {
                    SizeF f = g.MeasureString(line, rtb.Font);
                    width = (int)(f.Width) > width ? (int)(f.Width) : width;
                }
                rtb.Dock = DockStyle.Fill;

                log.Width = width + 40;
                log.Height = 600;
                log.MaximizeBox = false;
                
                
                rtb.Select(0, 0);
                log.ShowDialog();
            }
            catch { }
            finally {
                if (fs != null)
                    fs.Close();
            }
        }

        private void SimView_ResizeEnd(object sender, EventArgs e)
        {
            if (!windowSize.Equals(new Size(Width, Height)))
            {
                windowSize = new Size(Width, Height);
                float horizontalRatio = 0.2f, verticalRatio = 0.25f;

                int tempPoWidth = Convert.ToInt32(Width * horizontalRatio);
                panel_Left.Width = Convert.ToInt32(Width - panel_PO.Width - 20);

                panel_Sim.Height = Convert.ToInt32(panel_Left.Height * (1 - verticalRatio));

                panel_PM.Height = Convert.ToInt32(panel_Left.Height - panel_Sim.Height);

                int tempWidth = (Grid_PM.Width - 45) / 11;
                foreach (DataGridViewColumn col in Grid_PM.Columns)
                    if (col.Index > 0)
                        col.Width = tempWidth;

                ADrawBackground();
            }
        }
        private void panel_Sim_Control_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!isRunning)
                SwitchEditMode();
        }
        internal void Grid_Mem_SelectionChanged(object sender, EventArgs e)
        {
            int idxRow = Grid_Mem.CurrentCell.RowIndex;
            int idxCol = Grid_Mem.CurrentCell.ColumnIndex;
            memView.changeSelectedPAO(idxRow);
            selectedInstruction = List_Memory[idxRow];
            string instructionMnemo = "";
            cellDescription.Text = "PAO[" + idxRow + "]";
            if (selectedInstruction.typ == 0)
                cellDescription.Text += "=0";
            if (selectedInstruction.typ == 1)
            {
                cellDescription.Text += " - " + Strings.data + "\n";
                cellDescription.Text += selectedInstruction.value.ToString() + "b\n";
                cellDescription.Text += Convert.ToInt16(selectedInstruction.value, 2) + "d\n";
                cellDescription.Text += Convert.ToInt16(selectedInstruction.value, 2).ToString("X") + "h";
            }
            else if (selectedInstruction.typ == 2)
                instructionMnemo = Translator.DecodeInstruction(selectedInstruction.OP);
            else if (selectedInstruction.typ == 3)
                instructionMnemo = Translator.DecodeInstruction(selectedInstruction.AOP);
            string instructionDescription = Translator.GetInsrtuctionDescription(instructionMnemo, selectedInstruction.value, selectedInstruction.typ);
            cellDescription.Text += instructionDescription;
            Grid_Mem.Rows[idxRow].Cells[1].Selected = true;
        }
        private void Grid_PM_SelectionChanged(object sender, EventArgs e)
        {
            if (Grid_PM.CurrentCell.ColumnIndex == 0)
                Grid_PM.ClearSelection();
        }
        private void Grid_Mem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Grid_Mem.ReadOnly = true;
            decimal variableToTest;
            short valueAfterConvertion;
            string hexValueBefore = Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[2].Value.ToString();
            try
            {
                if (decimal.TryParse(Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value.ToString(), out variableToTest))
                {
                    valueAfterConvertion = Convert.ToInt16(Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value.ToString(), 10);

                    string bin = Convert.ToString(valueAfterConvertion, 2).PadLeft(16, '0');
                    string hex = valueAfterConvertion.ToString("X").PadLeft(4, '0');
                    AUpdateForm(Grid_Mem.CurrentCell.RowIndex, bin, hex, 1);
                    Grid_Mem[2, Grid_Mem.CurrentCell.RowIndex].Value = hex;
                    Grid_Mem[1, Grid_Mem.CurrentCell.RowIndex].Value = bin;
                }
                else
                {
                    string txt = Grid_Mem.Rows[e.RowIndex].Cells[1].Value.ToString();
                    char[] toRemove = { 'h', 'H' };
                    txt = txt.TrimEnd(toRemove);
                    try
                    {
                        valueAfterConvertion = Convert.ToInt16(txt, 16);

                        string bin = Convert.ToString(valueAfterConvertion, 2).PadLeft(16, '0');
                        string hex = valueAfterConvertion.ToString("X").PadLeft(4, '0');
                        AUpdateForm(Grid_Mem.CurrentCell.RowIndex, bin, hex, 1);
                        Grid_Mem[2, Grid_Mem.CurrentCell.RowIndex].Value = hex;
                        Grid_Mem[1, Grid_Mem.CurrentCell.RowIndex].Value = bin;
                    }
                    catch
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                try
                {
                    Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value = Convert.ToString(Convert.ToInt16(hexValueBefore, 16), 2);
                }
                catch
                {
                    Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value = "";
                }
            }
        }
        #endregion
        #region MenuStrip
        private void edytujpmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pmView.Show();
            pmView.BringToFront();
        }
        private void edytujpaoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //memView.Show();
            memView.showMe(Grid_Mem.SelectedRows[0].Index);
            
        }
        private void wczytajpaoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            memView.button_Load_Table_Click(this, new EventArgs());
        }
        private void wczytajpmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pmView.button_Load_Table_Click(this, new EventArgs());
        }
        private void konsolaDevToolStripMenuItem_Click(object sender, EventArgs e)
        {
            devConsole.Show();
            devConsole.BringToFront();
        }
        private void oAutorzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Author().ShowDialog();
        }
        private void opcjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Options op = new Options())
            {
                op.ACallUpdate += ADrawBackground;
                op.ShowDialog();
                devConsoleToolStripMenuItem.Enabled = Settings.Default.IsDevConsole;
                serwerToolStripMenuItem.Visible = Settings.Default.isServerVisible;
            }
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);
            flags["ZNAK"].flagName = Strings.sign;
            setAllStrings();
            pmView.setAllStrings();
            memView.setAllStrings();
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_File_Dialog.Filter = "Logi symulatora|*.log|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj log";
            if (Directory.Exists(_environmentPath + @"\Log\"))
                open_File_Dialog.InitialDirectory = _environmentPath + @"\Log\";
            else
                open_File_Dialog.InitialDirectory = _environmentPath;

            DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
            if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
            {
                ShowLog(open_File_Dialog.FileName);
            }
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isRunning)
                SwitchEditMode();
        }
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addTextToLog("\n" + Strings.clearingRegister + "\n");
            mark = 5;
            mistakes = currnetCycle = 0;
            foreach (var reg in registers)
                reg.Value.resetValue();
            foreach (var flg in flags)
                flg.Value.resetValue();
            dgvcs1.ForeColor = Color.Green;
            dataGridView_Info.Rows[0].Cells[1].Value = 5;
            dataGridView_Info.Rows[1].Cells[1].Value = dataGridView_Info.Rows[2].Cells[1].Value =
                dataGridView_Info.Rows[3].Cells[1].Value = 0;
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                Close();
            });
        }
        private void nowyLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Default.CanCloseLog)
            {
                DialogResult result = MessageBox.Show(Strings.areYouSureCloseLog, Strings.areYouSureCloseLogTitle, MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    ACloseLog();
                    closeLogToolStripMenuItem.Enabled = false;
                    button_Show_Log.Visible = false;
                    timer1.Enabled = false;
                    canSimulate = true;
                }
            }
            else
            {
                SystemSounds.Hand.Play();
                MessageBox.Show(Strings.cantCloseLogFile);
            }
        }
        #endregion
        #region Other
        internal void switchLayOut()
        {
            bool needRefresh = false;
            if (layoutChange)
            {
                needRefresh = RBPS.Visible;
                registers["SUMA"].Visible = registers["L"].Visible = registers["R"].Visible = true;
                RBPS.Visible = registers["RAPS"].Visible = registers["RAE"].Visible = false;
            }
            else
            {
                needRefresh = registers["SUMA"].Visible;
                registers["SUMA"].Visible = registers["L"].Visible = registers["R"].Visible = false;
                RBPS.Visible = registers["RAPS"].Visible = registers["RAE"].Visible = true;
            }
            layoutChange = false;
            if (needRefresh)
                ADrawBackground();
        }


        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pmView.button_Save_Table_Click(this, new EventArgs());
        }

        private void zapiszToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            memView.button_Save_Table_Click(this, new EventArgs());
        }

        private void Grid_Mem_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            Grid_Mem.EndEdit();
        }

        private void SimView_ResizeBegin(object sender, EventArgs e)
        {

        }

        private void SimView_MinimumSizeChanged(object sender, EventArgs e)
        {

        }

        private void SimView_Shown(object sender, EventArgs e)
        {
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new
            EventHandler(SystemEvents_DisplaySettingsChanged);
        }

        public void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            SimView_ResizeEnd(this, new EventArgs());
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_RESTORE = 0xF120;

            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_RESTORE)
            {
                //
            }

            base.WndProc(ref m);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            var loc = richTextBox1.Location;
            loc.X = 15; loc.Y = 117;
            richTextBox1.Location = loc;
            TimeSpan diff = DateTime.Now - simTime;
            TimeSpan now = simTime.TimeOfDay;
                richTextBox1.Text = string.Format("UT{0:00}:{1:00}", appStart.Hours, appStart.Minutes) + " > " + string.Format("{0:00}:{1:00}", now.Hours, now.Minutes) + " + " + string.Format("{0:00}:{1:00}.{2:00}", diff.Hours, diff.Minutes, diff.Seconds);
                Regex regExp = new Regex(@"\+");
                foreach (Match match in regExp.Matches(richTextBox1.Text))
                {
                    richTextBox1.Select(match.Index, richTextBox1.Text.Length);
                    richTextBox1.SelectionColor = Color.DarkMagenta;
                }
                richTextBox1.SelectAll();
                richTextBox1.SelectionAlignment = HorizontalAlignment.Right;
                richTextBox1.Select(0, 0);
            
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            //e.Handled = true;
            //richTextBox1.SelectionLength = 0;
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            richTextBox1.Parent.Focus();
        }

        public class TransparentPanel : Panel
        {
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                    return cp;
                }
            }
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                //base.OnPaintBackground(e);
            }
        }

        private void label1_Click(object sender, EventArgs e) {
            showAllUpTimes();
        }

        private void drukujToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            memView.button1_Click(this, new EventArgs());
        }

        private void drukujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pmView.button1_Click(this, new EventArgs());
        }

        internal void ADrawBackground()
        {
            panel_Sim_Control.Visible = false;
            rearrangeTextBoxes();
            draw.drawBackground();
        }

        #endregion
        private void serwerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name, lastName, group, ipAddress, remotePort, password;
            bool connect;
            using (Server form = new Server(LogManager.isConnected))
            {
                form.ShowDialog();
                name = Server.name;
                lastName = Server.lastName;
                group = Server.group;
                ipAddress = Server.ipAddress;
                remotePort = Server.remotePort;
                password = Server.password;
                connect = form.connect;
            }
            if (connect)
            {
                try
                {
                    logManager.addTcpClient(name, lastName, group, ipAddress, remotePort, password);
                    serverTimer = new Thread(() =>
                    {
                        bool isConnected = true;
                        try
                        {
                            while (isConnected)
                            {
                                Thread.Sleep(5000);
                                isConnected = logManager.ping();
                            }
                        }
                        catch { }
                        finally
                        {
                            logManager.disconnect();
                            serverTimer = null;
                        }
                    });
                    serverTimer.Start();
                }
                catch { }//błędne dane po stronie klienta - literowka ip:port itp
            }
        }


    }
}
