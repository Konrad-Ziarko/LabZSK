using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT
{//może tryb nocny?
    public partial class MainWindow : Form
    {
        private string fileForPM = @"\Env\~micro.zkt";
        private string fileForPO = @"\Env\~mem.zkt";
        private Author frmAuthor;
        private RunSim frmSimulation;
        private PM frmPM;
        private PO frmPO;
        private MemoryRecord lastRecordFromRRC;//pamietaj aby nulowac po zerowaniu rejestrow albo nowej symulacji
        private List<MemoryRecord> List_Memory = new List<MemoryRecord>();
        private List<MicroOperation> List_MicroOp = new List<MicroOperation>();
        private Dictionary<string, NumericTextBox> registers = new Dictionary<string, NumericTextBox>() {
            {"LK", null }, {"A", null },{"MQ", null },{"X", null },{"RAP", null },{"LALU", null },{"RALU", null },
        {"RBP", null },{"ALU", null },{"BUS", null },{"RR", null },{"LR", null },{"RI", null },
        {"RAPS", null },{"RAE", null },{"L", null },{"R", null },{"SUMA", null }};
        private TextBox RBPS = new TextBox();
        private Dictionary<string, BitTextBox> flags = new Dictionary<string, BitTextBox>()
        {
            {"MAV", null }, {"IA",null }, {"INT", null }, {"ZNAK",null }, {"XRO", null }, {"OFF", null }
        };

        public static int currnetCycle = 0;
        public static int mark = 5;
        public static int mistakes = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void loadLastPMState()
        {
            FileInfo fileInfo = new FileInfo(Environment.CurrentDirectory + fileForPM);
            fileInfo.Attributes = FileAttributes.Normal;
            if (CRC.ComputeChecksum(File.ReadAllBytes(Environment.CurrentDirectory + fileForPM)) == 0)
            {
                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(Environment.CurrentDirectory + fileForPM, FileMode.Open)))
                    {
                        int n = br.ReadInt32();
                        int m = br.ReadInt32();
                        MicroOperation tmpMicroOperation;
                        string tmpString = "";
                        for (int i = 0; i < m; ++i)
                        {
                            for (int j = 0; j < n; ++j)
                            {

                                if (br.ReadBoolean())
                                    tmpString += br.ReadString() + " ";
                                else
                                    br.ReadBoolean();
                            }
                            string[] attributes = tmpString.Split(' ');
                            tmpString = "";
                            tmpMicroOperation = new MicroOperation(attributes[0], attributes[1], attributes[2], attributes[3],
                                attributes[4], attributes[5], attributes[6], attributes[7], attributes[8], attributes[9],
                                attributes[10], attributes[11]);
                            List_MicroOp[i]=tmpMicroOperation;
                        }
                    }
                }
                catch (Exception)
                {
                    //plik uszkodzony ?
                }
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    List_MicroOp.Add(new MicroOperation(i, "", "", "", "", "", "", "", "", "", "", ""));
                }
                MessageBox.Show("Plik z ostatnim zapisem pamięci mikroprogramu jest niespójny!", "LabZKT", MessageBoxButtons.OK);
            }
            File.Delete(Environment.CurrentDirectory + fileForPM);
        }
        private void loadLastPOState()
        {
            FileInfo fileInfo = new FileInfo(Environment.CurrentDirectory + fileForPO);
            fileInfo.Attributes = FileAttributes.Normal;
            if (CRC.ComputeChecksum(File.ReadAllBytes(Environment.CurrentDirectory + fileForPO)) == 0)
            {
                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(Environment.CurrentDirectory + fileForPO, FileMode.Open)))
                    {
                        int n = br.ReadInt32();
                        int m = br.ReadInt32();
                        MemoryRecord tmpMemoryRecord;
                        string tmpString = "";
                        for (int i = 0; i < m; ++i)
                        {
                            for (int j = 0; j < n; ++j)
                            {

                                if (br.ReadBoolean())
                                    tmpString += br.ReadString() + " ";
                                else
                                    br.ReadBoolean();
                            }
                            string[] attributes = tmpString.Split(' ');
                            tmpString = "";
                            tmpMemoryRecord = new MemoryRecord(Convert.ToInt16(attributes[0]), attributes[1],
                                attributes[2], Convert.ToInt16(attributes[3]));
                            List_Memory[i]=tmpMemoryRecord;
                        }
                    }
                }
                catch (Exception)
                {
                    //plik uszkodzony ?
                }
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    List_Memory.Add(new MemoryRecord(i, "", "", 0));
                }
                MessageBox.Show("Plik z ostatnim zapisem pamięci operacyjnej jest niespójny!", "LabZKT", MessageBoxButtons.OK);
            }
            File.Delete(Environment.CurrentDirectory + fileForPO);
        }
        private void initFlags()
        {
            flags["MAV"] = new BitTextBox(130, 125, null, "MAV");
            flags["IA"] = new BitTextBox(140, 125, null, "IA");
            flags["INT"] = new BitTextBox(290, 125, null, "INT");
            flags["ZNAK"] = new BitTextBox(450, 65, null, "ZNAK");
            flags["XRO"] = new BitTextBox(460, 65, null, "XRO");
            flags["OFF"] = new BitTextBox(470 , 65, null, "OFF");
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
            RBPS.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            RBPS.Text = "000000000000";
        }
        private void checkIntegrity()
        {
            for (int i = 0; i < 256; i++)
                List_MicroOp.Add(new MicroOperation(i, "", "", "", "", "", "", "", "", "", "", ""));
            for (int i = 0; i < 256; i++)
                List_Memory.Add(new MemoryRecord(i, "", "", 0));
            //wykrycie niepoprawnego zamkniecia programu, pytac o wczytanie poprzenich mem i micro
            if (File.Exists(Environment.CurrentDirectory + fileForPM) || File.Exists(Environment.CurrentDirectory + fileForPO))
                MessageBox.Show("Wykryto niepoprawne zakończenie pracy symulatora.\nDo pamięci został wczytany ostatni dostępny stan.", "LabZKT", MessageBoxButtons.OK);
            //Load last available MicroOp list
            if (File.Exists(Environment.CurrentDirectory + fileForPM))
                new Thread(() => { loadLastPMState(); }).Start();

            //Load last available MemoryRecords list
            if (File.Exists(Environment.CurrentDirectory + fileForPO))
                new Thread(() => { loadLastPOState(); }).Start();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            checkIntegrity();
            initRegisterTextBoxes();
            initFlags();
        }

        /// Invoke close event when butto is clicked
        private void button_Close_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                Close();
            });
        }

        /// Handle FormClose event. Handle also Alt+F4 and 'X' button. If necessary save data, logs, etc.
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Zakończyć pracę z symulatorem?", "LabZKT", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //Delete temporary files
                    FileInfo fileInfoPM = new FileInfo(Environment.CurrentDirectory + fileForPM);
                    FileInfo fileInfoPO = new FileInfo(Environment.CurrentDirectory + fileForPO);
                    try
                    {
                        if (File.Exists(Environment.CurrentDirectory + fileForPM))
                        {
                            fileInfoPM.Attributes = FileAttributes.Normal;
                            File.Delete(Environment.CurrentDirectory + fileForPM);
                        }
                    }
                    catch (DirectoryNotFoundException)
                    {
                        fileInfoPO.Directory.Create();
                    }
                    try
                    {
                        if (File.Exists(Environment.CurrentDirectory + fileForPO))
                        {
                            fileInfoPO.Attributes = FileAttributes.Normal;
                            File.Delete(Environment.CurrentDirectory + fileForPO);
                        }
                    }
                    catch (DirectoryNotFoundException)
                    {
                        fileInfoPO.Directory.Create();
                    }

                    e.Cancel = false;
                    //Environment.Exit(0);
                    //Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void button_PM_Click(object sender, EventArgs e)
        {
            frmPM = new PM(ref List_MicroOp);
            frmPM.ShowDialog();
        }
        private void button_PO_Click(object sender, EventArgs e)
        {
            frmPO = new PO(ref List_Memory);
            frmPO.ShowDialog();
        }
        private void button_Run_Click(object sender, EventArgs e)
        {
            frmSimulation = new RunSim(ref List_Memory, ref List_MicroOp, ref registers, ref flags, ref RBPS, ref lastRecordFromRRC);
            foreach (var reg in registers)
                reg.Value.Parent = frmSimulation.getSimPanel();
            foreach (var sig in flags)
                sig.Value.Parent = frmSimulation.getSimPanel();
            RBPS.Parent = frmSimulation.getSimPanel();
            frmSimulation.ShowDialog();
        }
        private void button_Author_Click(object sender, EventArgs e)
        {
            frmAuthor = new Author();
            frmAuthor.ShowDialog();
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);
        private bool isDebuggerPresent;
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
            if (isDebuggerPresent || System.Diagnostics.Debugger.IsAttached)
            {
                //cos smiesznego
                //np okno z blue screenem
                //Environment.Exit(0);
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            //Thread.Sleep(2000);
            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
            if (isDebuggerPresent || System.Diagnostics.Debugger.IsAttached)
            {
                //odkomentuj to potem
                /*
                ErrorMessage tmp = new ErrorMessage();

                tmp.Show();
                tmp.Focus();
                this.Visible = false;
                SoundPlayer audio = new SoundPlayer(LabZKT.Properties.Resources.BSoD);
                audio.PlayLooping();
                */
            }
            else timer1.Enabled = true;
        }
    }
}
