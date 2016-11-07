using LabZKT.Memory;
using LabZKT.MicroOperations;
using LabZKT.Other;
using LabZKT.Properties;
using LabZKT.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT.Simulation
{
    /// <summary>
    /// Displays simulation interface
    /// </summary>
    public partial class SimView : Form
    {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        internal event Action<int, string, string, int> AUpdateForm;
        internal event Action AEnterEditMode;
        internal event Action ALeaveEditMode;
        internal event Action AClearRegisters;
        internal event Action<int> AGetMemoryRecord;
        internal event Action<bool> APrepareSimulation;
        internal event Action ANextTact;
        internal event Action ADrawBackground;
        internal event Action ACloseLog;
        internal event Action ACheckProperties;
        internal event Action AButtonOKClicked;
        internal event Action<string> AShowLog;
        internal event Action ACallDevConsole;
        internal event Action AStopDevConsole;
        internal event Action AEditPM;
        internal event Action AEditPAO;
        internal event Action ALoadPM;
        internal event Action ALoadPAO;
        internal event Action AShowCurrentLog;
        internal event Action AChangeLanguage;
        internal event Action ABreakSimulation;

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
        private Size windowSize;
        private FormWindowState previousWindowState;
        private FormWindowState currentWindowState;
        /// <summary>
        /// Initialize simulation view instance
        /// </summary>
        /// <param name="needsNewLog">Boolean representing whether log file already exists</param>
        public SimView(bool needsNewLog)
        {
            InitializeComponent();
            devConsoleToolStripMenuItem.Enabled = Properties.Settings.Default.IsDevConsole;
            closeLogToolStripMenuItem.Enabled = needsNewLog;
            setAllStrings();
        }
        internal void setAllStrings()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);
            button_Show_Log.Text = Strings.showLogButton;
            button_End_Edit.Text = Strings.endEditRegisters;
            button_OK.Text = Strings.okButton;
            button_Next_Tact.Text = Strings.nextTactButton;

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

            Grid_PM.Columns[0].HeaderText = Grid_Mem.Columns[0].HeaderText = Strings.cellAddressViewGrid;
            Grid_Mem.Columns[1].HeaderText = Strings.cellValueViewGrid;
            this.Text = Strings.SimulationTitle;
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
            button_Show_Log.Visible = true;
            richTextBox_Log.Clear();
            isRunning = false;
            inMicroMode = false;
            toolStripMenu_Edit.Enabled = true;
            toolStripMenu_Exit.Enabled = true;
            button_Makro.Visible = true;
            button_Micro.Visible = true;
            toolStripMenu_Clear.Enabled = true;
            label_Status.Text = Strings.stopMode;
            label_Status.ForeColor = Color.Green;
            closeLogToolStripMenuItem.Enabled = true;
        }

        internal void SwitchLayOut()
        {
            ADrawBackground();
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
            closeLogToolStripMenuItem.Enabled = b;
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
            Grid_Mem_SelectionChanged(sender, e);
            initUserInfoArea();
            SimView_ResizeEnd(sender, e);
            Focus();
            windowSize = new Size(Width, Height);
        }
        private void initUserInfoArea()
        {
            ACheckProperties();
            dataGridView_Info.Rows.Add(mark, Strings.mark);
            dataGridView_Info.Rows.Add(mistakes, Strings.mistakes);
            dataGridView_Info.Rows.Add("0", Strings.tact);
            dataGridView_Info.Rows.Add(currnetCycle, Strings.cycle);
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
            {
                DialogResult dr = MessageBox.Show(Strings.areYouSureExit, Strings.areYouSureExitTitle, MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    ACloseLog();
                }
                else if (dr == DialogResult.No)
                    e.Cancel = true;
            }
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
            SwitchEditMode();
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
            /*try
            {
                Grid_Mem.Rows[row].Selected = true;
            }
            catch (NullReferenceException) { }*/
        }
        internal void Grid_Mem_SelectionChanged(object sender, EventArgs e)
        {
            int idxRow = Grid_Mem.CurrentCell.RowIndex;
            int idxCol = Grid_Mem.CurrentCell.ColumnIndex;

            AGetMemoryRecord(idxRow);
            string instructionMnemo = "";
            cellDescription.Text = "PAO[" + idxRow + "]";
            if (selectedInstruction.typ == 0)
                cellDescription.Text += "=0";
            else
                cellDescription.Text += "\n";
            if (selectedInstruction.typ == 1)
            {
                cellDescription.Text += Strings.data+"\n";
                cellDescription.Text += selectedInstruction.value.ToString() + "b\n";
                cellDescription.Text += Convert.ToUInt16(selectedInstruction.value, 2) + "d\n";
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
        internal void button_Makro_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                toolStripMenu_Edit.Enabled = false;
                toolStripMenu_Exit.Enabled = false;
                button_Makro.Visible = false;
                button_Micro.Visible = false;
                toolStripMenu_Clear.Enabled = false;
                label_Status.Text = Strings.startMode;
                label_Status.ForeColor = Color.Red;
                button_Show_Log.Visible = false;
                APrepareSimulation(false);
            }
        }
        private void button_Micro_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                toolStripMenu_Edit.Enabled = false;
                toolStripMenu_Exit.Enabled = false;
                button_Makro.Visible = false;
                button_Micro.Visible = false;
                toolStripMenu_Clear.Enabled = false;
                label_Status.Text = Strings.startMode;
                label_Status.ForeColor = Color.Red;
                button_Show_Log.Visible = false;
                APrepareSimulation(true);
            }
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
            if (mistakes >= Settings.Default.ThirdMark)
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
                SwitchEditMode();
            }
            else if (e.KeyChar == (char)Keys.Escape && isRunning)
            {
                //przerwanie pracy
                DialogResult dr = MessageBox.Show(Strings.areYouSureExit, Strings.areYouSureExitTitle, MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    button_Next_Tact.Visible = false;
                    button_Makro.Visible = false;
                    ABreakSimulation();
                }
            }

        }
        private void Grid_Mem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
            }
            else if (char.IsLetterOrDigit((char)e.KeyCode) || e.KeyCode == Keys.OemMinus)
            {
                Grid_Mem.ReadOnly = false;
                if ((char)e.KeyCode <= 90 && (char)e.KeyCode >= 65)
                    Grid_Mem.CurrentCell.Value = (char)(e.KeyCode + 32);
                else if (e.KeyCode == Keys.OemMinus)
                    Grid_Mem.CurrentCell.Value = (char)45;
                else
                    Grid_Mem.CurrentCell.Value = (char)e.KeyCode;
                Grid_Mem.BeginEdit(false);
            }
            else if (e.KeyCode == Keys.Enter)
                Grid_Mem.EndEdit();
        }
        private void RunSim_SizeChanged(object sender, EventArgs e)
        {
            previousWindowState = currentWindowState;
            currentWindowState = WindowState;
            if (previousWindowState != currentWindowState)
            {
                if (currentWindowState == FormWindowState.Maximized && previousWindowState == FormWindowState.Normal)
                    SimView_ResizeEnd(this, new EventArgs());
                else if (currentWindowState == FormWindowState.Normal && previousWindowState == FormWindowState.Maximized)
                    SimView_ResizeEnd(this, new EventArgs());
            }
        }
        private void nowyLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(Strings.areYouSureCloseLog, Strings.areYouSureCloseLogTitle, MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                ACloseLog();
                setNewLog(false);
                button_Show_Log.Visible = false;
            }
        }
        private void RunSim_Paint(object sender, PaintEventArgs e)
        {
            //ADrawBackground(panel_Sim_Control);
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

        private void opcjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Options op = new Options())
            {
                op.ACallUpdate += Op_ACallUpdate;
                op.ShowDialog();
                devConsoleToolStripMenuItem.Enabled = Settings.Default.IsDevConsole;
            }
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);
            AChangeLanguage();
        }

        private void Op_ACallUpdate()
        {
            ADrawBackground();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void SimView_ResizeBegin(object sender, EventArgs e)
        {
            //panel_Left.Visible = false;
            //panel_PO.Visible = false;
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

        private void button_Show_Log_Click(object sender, EventArgs e)
        {
            AShowCurrentLog();
        }

        private void button_End_Edit_Click(object sender, EventArgs e)
        {
            SwitchEditMode();
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
    }
}
