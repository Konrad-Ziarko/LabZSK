using LabZKT.Memory;
using LabZKT.MicroOperations;
using LabZKT.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LabZKT.Simulation
{
    /// <summary>
    /// Displays simulation interface
    /// </summary>
    public partial class SimView : Form
    {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        internal event Action AEnterEditMode;
        internal event Action ALeaveEditMode;
        internal event Action AClearRegisters;
        internal event Action<int> AGetMemoryRecord;
        internal event Action<bool> APrepareSimulation;
        internal event Action ANextTact;
        internal event Action<Control> ADrawBackground;
        internal event Action ANewLog;
        internal event Action ACheckProperties;
        internal event Action AButtonOKClicked;
        internal event Action<bool> ASaveCurrentState;
        internal event Action<string> AShowLog;
        internal event Action ACallDevConsole;
        internal event Action AStopDevConsole;
        internal event Action AEditPM;
        internal event Action AEditPAO;
        internal event Action ALoadPM;
        internal event Action ALoadPAO;

        internal int currnetCycle { get; set; }
        internal int mark { get; set; }
        internal int mistakes { get; set; }
        internal int currentTact { get; set; }
        internal MemoryRecord selectedInstruction { get; set; }
        internal bool isRunning { get; set; }
        internal bool inMicroMode { get; set; }
        internal bool buttonOKClicked { get; set; }
        private DataGridViewCellStyle dgvcs1;
        private bool inEditMode;

        /// <summary>
        /// Initialize simulation view instance
        /// </summary>
        /// <param name="needsNewLog">Boolean representing whether log file already exists</param>
        public SimView(bool needsNewLog)
        {
            InitializeComponent();
            nowyLogToolStripMenuItem.Enabled = needsNewLog;
        }

        /// <summary>
        /// Add text to in-simulation log display
        /// </summary>
        /// <param name="tact">String representing current tact</param>
        /// <param name="mnemo">String representing mnemonic name</param>
        /// <param name="description">String representing operation description</param>
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
        }
        /// <summary>
        /// Set display for simulation stop state
        /// </summary>
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

        internal void SwitchLayOut()
        {
            ADrawBackground(panel_Sim_Control);
        }
        internal void buttonOkSetVisible()
        {
            button_OK.Visible = true;
        }
        internal void buttonNextTactSetVisible()
        {
            button_Next_Tact.Visible = true;
        }
        internal void setNewLog(bool b)
        {
            nowyLogToolStripMenuItem.Enabled = b;
        }
        internal DataGridView GetDataGridPM()
        {
            return Grid_PM;
        }
        internal void SetDataGridPM(List<MicroOperation> List_MicroOp, int row, int col)
        {
            Grid_PM[col, row].Value = List_MicroOp[row].getColumn(col);
        }
        internal DataGridView GetDataGridMem()
        {
            return Grid_Mem;
        }
        internal Control getSimPanel()
        {
            return panel_Sim_Control;
        }
        internal void SetGridInfo(int value)
        {
            dataGridView_Info.Rows[2].Cells[0].Value = value;
        }
        private void RunSim_Load(object sender, EventArgs e)
        {
            Size = new Size(1024, 768);
            CenterToScreen();
            grid_PO_SelectionChanged(sender, e);
            initUserInfoArea();
            Focus();
        }
        private void initUserInfoArea()
        {
            ACheckProperties();
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
                ASaveCurrentState(nowyLogToolStripMenuItem.Enabled);

        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open_File_Dialog.Filter = "Logi symulatora|*.log|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj log";
            open_File_Dialog.InitialDirectory = envPath;

            DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
            if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
            {
                AShowLog(open_File_Dialog.FileName);
            }
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (inEditMode)
            {
                panel_Control.Focus();
                ALeaveEditMode();
                toolStripMenu_Edit.Text = "Edytuj rejestry";
                button_Makro.Visible = true;
                button_Micro.Visible = true;
                toolStripMenu_Clear.Enabled = true;
                inEditMode = false;
                label_Status.Text = "Stop";
            }
            else
            {
                AEnterEditMode();
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
            AClearRegisters();
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

        internal void SetDataGridMem(List<MemoryRecord> list_Memory, int row)
        {
            Grid_Mem[1, row].Value = list_Memory[row].value;
            Grid_Mem[2, row].Value = list_Memory[row].hex;
        }
        internal void grid_PO_SelectionChanged(object sender, EventArgs e)
        {
            int idxRow = Grid_Mem.CurrentCell.RowIndex;
            int idxCol = Grid_Mem.CurrentCell.ColumnIndex;

            AGetMemoryRecord(idxRow);
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
        private void button_Makro_Click(object sender, EventArgs e)
        {
            toolStripMenu_Edit.Enabled = false;
            toolStripMenu_Exit.Enabled = false;
            button_Makro.Visible = false;
            button_Micro.Visible = false;
            toolStripMenu_Clear.Enabled = false;
            label_Status.Text = "Start";
            label_Status.ForeColor = Color.Red;
            APrepareSimulation(false);
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
            APrepareSimulation(true);
        }
        private void button_Next_Tact_Click(object sender, EventArgs e)
        {
            button_Next_Tact.Visible = false;
            ANextTact();
        }
        private void button_OK_Click(object sender, EventArgs e)
        {
            AButtonOKClicked();
            button_OK.Visible = false;
            ACheckProperties();
            dataGridView_Info[0, 1].Value = mistakes;
            dataGridView_Info[0, 0].Value = mark;
            if (mistakes >= 10)
                dgvcs1.ForeColor = Color.Red;
        }
        private void RunSim_KeyPress(object sender, KeyPressEventArgs e)
        {
            AStopDevConsole();
            if (e.KeyChar == (char)Keys.Enter && !inEditMode)
            {
                ACheckProperties();
                if (isRunning && !inMicroMode)
                    button_OK_Click(sender, e);
                else if (isRunning && inMicroMode && currentTact != 7)
                    button_Next_Tact_Click(sender, e);
                else if (isRunning && inMicroMode && currentTact == 7)
                    button_OK_Click(sender, e);
                else
                    button_Makro_Click(sender, e);
            }
            else if (e.KeyChar == (char)Keys.Escape && inEditMode)
            {
                panel_Control.Focus();
                ALeaveEditMode();
                toolStripMenu_Edit.Text = "Edytuj rejestry";
                button_Makro.Visible = true;
                button_Micro.Visible = true;
                toolStripMenu_Clear.Enabled = true;
                inEditMode = false;
                label_Status.Text = "Stop";
            }
            else if (e.KeyChar == (char)Keys.Escape && isRunning)
            {
                //przerwanie pracy
            }
            
        }
        private void grid_PO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
        }
        private void RunSim_SizeChanged(object sender, EventArgs e)
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
            ADrawBackground(panel_Sim_Control);
        }
        private void nowyLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Utracisz cały postęp symulacji.\nCzy chcesz zakończyć pracę z obecnym logiem?", "Nowa symulacja", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ANewLog();
                setNewLog(false);
            }
        }
        private void RunSim_Paint(object sender, PaintEventArgs e)
        {
            ADrawBackground(panel_Sim_Control);
        }
        private void SimView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                e.Handled = true;
            }
        }

        private void edytujpmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AEditPM();
        }

        private void edytujpaoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AEditPAO();
        }

        private void wczytajpaoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ALoadPAO();
        }

        private void wczytajpmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ALoadPM();
        }

        private void konsolaDevToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ACallDevConsole();
        }

        private void oAutorzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Author().ShowDialog();
        }
    }
}
