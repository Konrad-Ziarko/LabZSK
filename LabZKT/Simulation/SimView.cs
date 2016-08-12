﻿using LabZKT.Memory;
using LabZKT.StaticClasses;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Trinet.Core.IO.Ntfs;

namespace LabZKT.Simulation
{
    public partial class SimView : Form
    {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        public event Action EnterEditMode;
        public event Action LeaveEditMode;
        public event Action ClearRegisters;
        public event Action<int> GetMemoryRecord;
        public event Action<string> AddToLog;
        public event Action<bool> PrepareSimulation;
        public event Action NextTact;
        public event Action<Control> DrawBackground;
        public event Action NewLog;
        public event Action CheckProperties;
        public event Action ButtonOKClicked;
        public event Action<bool> SaveCurrentState;

        private DataGridViewCellStyle dgvcs1;
        public bool wasMaximized = false;

        public int currnetCycle { get; set; }
        public int mark { get; set; }
        public int mistakes { get; set; }
        public int currentTact { get; set; }
        public MemoryRecord selectedInstruction { get; set; }
        public bool isRunning { get; set; }
        public bool inEditMode { get; set; }
        public bool inMicroMode { get; set; }
        public bool buttonOKClicked { get; set; }

        public SimView(bool b)
        {
            InitializeComponent();
            nowyLogToolStripMenuItem.Enabled = b;
        }
        public Control getSimPanel()
        {
            return panel_Sim_Control;
        }
        private void RunSim_Load(object sender, EventArgs e)
        {
            Size = new Size(1024, 768);
            initAll(sender, e);
        }
        private void initAll(object sender, EventArgs e)
        {
            CenterToScreen();
            grid_PO_SelectionChanged(sender, e);
            initUserInfoArea();
            Focus();
            RunSim_ResizeEnd(sender, e);
        }
        
        public DataGridView GetDataGridPM()
        {
            return Grid_PM;
        }
        public DataGridView GetDataGridMem()
        {
            return Grid_Mem;
        }

        private void initUserInfoArea()
        {
            CheckProperties();
            dataGridView_Info.Rows.Add(mark, "Ocena");
            dataGridView_Info.Rows.Add(mistakes, "Błędy");
            dataGridView_Info.Rows.Add("0", "Takt");
            dataGridView_Info.Rows.Add(currnetCycle, "Cykl");
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
  
        private void RunSim_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
                e.Cancel = true;
            else
                SaveCurrentState(nowyLogToolStripMenuItem.Enabled);

        }
        
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_File_Dialog.Filter = "Logi symulatora|*.log|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj log";
            open_File_Dialog.InitialDirectory = envPath;

            DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
            if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
            {
                try
                {
                    uint crcFromFile;
                    byte[] arr= new byte[4];
                       FileInfo fileInfo = new FileInfo(open_File_Dialog.FileName);
                    uint crc = CRC.ComputeChecksum(File.ReadAllBytes(open_File_Dialog.FileName));
                    if (fileInfo.AlternateDataStreamExists("crc"))
                    {
                        using (FileStream fs = fileInfo.GetAlternateDataStream("crc").OpenRead())
                        {
                            fs.Read(arr, 0, 4);
                        }
                        crcFromFile = BitConverter.ToUInt32(arr, 0);
                        if (crcFromFile == crc)
                        {
                            Form log = new Form();
                            RichTextBox rtb = new RichTextBox();
                            log.Controls.Add(rtb);
                            rtb.ReadOnly = true;
                            rtb.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
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
                panel_Control.Focus();
                LeaveEditMode();
                toolStripMenu_Edit.Text = "Edytuj rejestry";
                button_Makro.Visible = true;
                button_Micro.Visible = true;
                toolStripMenu_Clear.Enabled = true;
                inEditMode = false;
                label_Status.Text = "Stop";
            }
            else
            {
                EnterEditMode();
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
            ClearRegisters();
            dgvcs1.ForeColor = Color.Green;
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
            int idxRow = Grid_Mem.CurrentCell.RowIndex;
            int idxCol = Grid_Mem.CurrentCell.ColumnIndex;

            GetMemoryRecord(idxRow);
            string instructionMnemo = "";
            label1.Text = "Komórka " + idxRow + "\n";
            if (selectedInstruction.typ == 1)
            {
                label1.Text += "DANA\n";
                label1.Text += selectedInstruction.value.ToString() + "b\n";
                label1.Text += Convert.ToInt16(selectedInstruction.value, 2) + "d\n";
                label1.Text += Convert.ToInt16(selectedInstruction.value, 2).ToString("X") + "h";
            }
            else if (selectedInstruction.typ == 2)
                instructionMnemo = Translator.DecodeInstruction(selectedInstruction.OP);
            else if (selectedInstruction.typ == 3)
                instructionMnemo = Translator.DecodeInstruction(selectedInstruction.AOP);
            string instructionDescription = Translator.GetInsrtuctionDescription(instructionMnemo, selectedInstruction.value, selectedInstruction.typ);
            label1.Text += instructionDescription;
        }

        private void grid_PM_SelectionChanged(object sender, EventArgs e)
        {
            if (Grid_PM.CurrentCell.ColumnIndex == 0)
                Grid_PM.ClearSelection();
        }
        public void AddText(string tact, string mnemo, string description)
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
            AddToLog("\t" + tact + " " + mnemo + " " + description + "\n");
        }
        
        private void button_Makro_Click(object sender, EventArgs e)
        {
            toolStripMenu_Edit.Enabled = false;
            toolStripMenu_Exit.Enabled = false;
            button_Makro.Visible = false;
            button_Micro.Visible = false;
            toolStripMenu_Clear.Enabled = false;
            label_Status.Text = "Start";
            label_Status.ForeColor = Color.Red;
            PrepareSimulation(false);
        }
        private void button_Micro_Click(object sender, EventArgs e)
        {
            toolStripMenu_Edit.Enabled = false;
            toolStripMenu_Exit.Enabled = false;
            button_Makro.Visible = false;
            button_Micro.Visible = false;
            toolStripMenu_Clear.Enabled = false;
            label_Status.Text = "Start";
            label_Status.ForeColor = Color.Red;
            PrepareSimulation(true);
        }
        public void stopSim()
        {
            isRunning = false;
            inMicroMode = false;
            toolStripMenu_Edit.Enabled = true;
            toolStripMenu_Exit.Enabled = true;
            button_Makro.Visible = true;
            button_Micro.Visible = true;
            toolStripMenu_Clear.Enabled = true;
            label_Status.Text = "Stop";
            label_Status.ForeColor = Color.Green;
            nowyLogToolStripMenuItem.Enabled = true;
        }
        private void button_Next_Tact_Click(object sender, EventArgs e)
        {
            button_Next_Tact.Visible = false;
            NextTact();
        }
        public void SetGridInfo(int value)
        {
            dataGridView_Info.Rows[2].Cells[0].Value = value;
        }
        private void button_OK_Click(object sender, EventArgs e)
        {
            ButtonOKClicked();
            button_OK.Visible = false;
            CheckProperties();
            dataGridView_Info[0, 1].Value = mistakes;
            dataGridView_Info[0, 0].Value = mark;
            if (mistakes >= 10)
                dgvcs1.ForeColor = Color.Red;
        }

        private void RunSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !inEditMode)
            {
                CheckProperties();
                if (isRunning && !inMicroMode)
                    button_OK_Click(sender, e);
                else if (isRunning && inMicroMode && currentTact != 7)
                    button_Next_Tact_Click(sender, e);
                else if (isRunning && inMicroMode && currentTact == 7)
                    button_OK_Click(sender, e);
                else
                    button_Makro_Click(sender, e);
            }
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
            DrawBackground(panel_Sim_Control);
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
            DialogResult result = MessageBox.Show("Utracisz cały postęp symulacji.\nCzy chcesz zakończyć pracę z obecnym logiem?", "Nowa symulacja", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                NewLog();
                setNewLog(false);
            }
        }

        private void RunSim_Paint(object sender, PaintEventArgs e)
        {
            DrawBackground(panel_Sim_Control);
        }
        public void buttonOkSetVisible()
        {
            button_OK.Visible = true;
        }
        public void buttonNextTactSetVisible()
        {
            button_Next_Tact.Visible = true;
        }
        public void setNewLog(bool b)
        {
            nowyLogToolStripMenuItem.Enabled = b;
        }
    }
}
