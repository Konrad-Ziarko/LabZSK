using LabZKT.Controls;
using LabZKT.Memory;
using LabZKT.MicroOperations;
using LabZKT.Simulation;
using LabZKT.StaticClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT
{
    /// <summary>
    /// Windows form representing application main window
    /// </summary>
    public partial class MainWindow : Form
    {
        /// <summary>
        /// Strign representing default path for files
        /// </summary>
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        /// <summary>
        /// String representing temporary file path
        /// </summary>
        private string fileForPM = @"\Env\~micro.zkt", fileForPO = @"\Env\~mem.zkt";
        /// <summary>
        /// Boolean representing whether debugger is attached or not
        /// </summary>
        private bool isDebuggerPresent;
        private Author frmAuthor;
        private SimModel simModel;
        private MemoryRecord lastRecordFromRRC;//pamietaj aby nulowac po zerowaniu rejestrow albo nowej symulacji, to delete?
        /// <summary>
        /// List of loaded operating memory
        /// </summary>
        private List<MemoryRecord> List_Memory = new List<MemoryRecord>();
        /// <summary>
        /// List of loaded microoperations
        /// </summary>
        private List<MicroOperation> List_MicroOp = new List<MicroOperation>();
        /// <summary>
        /// Dictionary for register controls corresponding to their names
        /// </summary>
        private Dictionary<string, NumericTextBox> registers = new Dictionary<string, NumericTextBox>() {
            {"LK", null }, {"A", null },{"MQ", null },{"X", null },{"RAP", null },{"LALU", null },{"RALU", null },
        {"RBP", null },{"ALU", null },{"BUS", null },{"RR", null },{"LR", null },{"RI", null },
        {"RAPS", null },{"RAE", null },{"L", null },{"R", null },{"SUMA", null }};
        /// <summary>
        /// Control representing RBPS register
        /// </summary>
        private TextBox RBPS = new TextBox();
        /// <summary>
        /// Dictionary for flag controls corresponding to their names
        /// </summary>
        private Dictionary<string, BitTextBox> flags = new Dictionary<string, BitTextBox>()
        {
            {"MAV", null }, {"IA",null }, {"INT", null }, {"ZNAK",null }, {"XRO", null }, {"OFF", null }
        };
        /// <summary>
        /// Initialize main window instance
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Loads last avaliable PM state into List_MicroOP list
        /// </summary>
        private void loadLastPMState()
        {
            FileInfo fileInfo = new FileInfo(envPath + fileForPM);
            fileInfo.Attributes = FileAttributes.Normal;
            if (CRC.ComputeChecksum(File.ReadAllBytes(envPath + fileForPM)) == 0)
            {
                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(envPath + fileForPM, FileMode.Open)))
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
                            List_MicroOp[i] = tmpMicroOperation;
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
            File.Delete(envPath + fileForPM);
        }
        /// <summary>
        /// Loads last avaliable PAO state into List_Memory list
        /// </summary>
        private void loadLastPOState()
        {
            FileInfo fileInfo = new FileInfo(envPath + fileForPO);
            fileInfo.Attributes = FileAttributes.Normal;
            if (CRC.ComputeChecksum(File.ReadAllBytes(envPath + fileForPO)) == 0)
            {
                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(envPath + fileForPO, FileMode.Open)))
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
                            List_Memory[i] = tmpMemoryRecord;
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
            File.Delete(envPath + fileForPO);
        }
        
       
        /// <summary>
        /// Occurs when main window was loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            TimeSpan start = new TimeSpan(20, 0, 0);
            TimeSpan end = new TimeSpan(5, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (now >= start || now <= end)
            {
                DialogResult dr = MessageBox.Show("Pamiętaj aby pracować w oświtlonym pomieszczeniu!", "LabZKT", MessageBoxButtons.OK);
            }
            //checkIntegrity();
            //initRegisterTextBoxes();
            //initFlags();
        }
        /// <summary>
        /// Invokes close event when button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Close_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                Close();
            });
        }
        /// <summary>
        /// Occures on FormClose event. Handle Alt+F4 and close button. If necessary save data, logs, etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Zakończyć pracę z symulatorem?", "LabZKT", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //Delete temporary files
                    FileInfo fileInfoPM = new FileInfo(envPath + fileForPM);
                    FileInfo fileInfoPO = new FileInfo(envPath + fileForPO);
                    try
                    {
                        if (File.Exists(envPath + fileForPM))
                        {
                            fileInfoPM.Attributes = FileAttributes.Normal;
                            File.Delete(envPath + fileForPM);
                        }
                    }
                    catch (DirectoryNotFoundException)
                    {
                        fileInfoPO.Directory.Create();
                    }
                    try
                    {
                        if (File.Exists(envPath + fileForPO))
                        {
                            fileInfoPO.Attributes = FileAttributes.Normal;
                            File.Delete(envPath + fileForPO);
                        }
                    }
                    catch (DirectoryNotFoundException)
                    {
                        fileInfoPO.Directory.Create();
                    }

                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.ApplicationExitCall)
                e.Cancel = false;
            else
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// Show microoperations form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_PM_Click(object sender, EventArgs e)
        {
            //PMController pmControler = new PMController(ref List_MicroOp);
        }
        /// <summary>
        /// Show operating memory form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_PO_Click(object sender, EventArgs e)
        {
            //MemController memControler = new MemController(ref List_Memory);
        }
        /// <summary>
        /// Show run simulation form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Run_Click(object sender, EventArgs e)
        {
            if (simModel == null)
                simModel = new SimModel(ref List_Memory, ref List_MicroOp, ref registers, ref flags, ref RBPS, ref lastRecordFromRRC);
            SimController simControler = new SimController();
        }
        /// <summary>
        /// Show credentials form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Author_Click(object sender, EventArgs e)
        {
            frmAuthor = new Author();
            frmAuthor.ShowDialog();
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

        /// <summary>
        /// Occures on timer tick. Checks if debugger is attached.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
            if (isDebuggerPresent || System.Diagnostics.Debugger.IsAttached)
            {
                //Thread.Sleep(2000);
                //odkomentuj to potem
                //ErrorMessage tmp = new ErrorMessage();
                //tmp.Show();
                //tmp.Focus();
            }
        }
        /// <summary>
        /// Occures when main window was showne. Check if debugger is attached.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Shown(object sender, EventArgs e)
        {
            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
            if (isDebuggerPresent || System.Diagnostics.Debugger.IsAttached)
            {
                /*
                new Thread(() =>
                {

                    for (int i = 0; i < 10000; i++)
                        ;
                    var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Process.Start(psi);
                })
                { IsBackground = true }.Start();
                Application.Exit();
                */
            }
            else
            {
                timer1.Interval = new Random().Next(1000, 20000);
                timer1.Enabled = true;
            }
        }
    }
}
