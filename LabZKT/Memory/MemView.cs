﻿using LabZSK.Controls;
using LabZSK.Properties;
using LabZSK.Simulation;
using LabZSK.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK.Memory {
    /// <summary>
    /// Displays memory and allows to modify data
    /// </summary>
    public partial class MemView : Form {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK";
        internal event Action<int, string, string, int> AUpdateForm;

        /// <summary>
        /// List of MemoryRecords
        /// </summary>
        public List<MemoryRecord> List_Memory { get; set; }
        private SimView view;
        MemSubmit theSubView;
        private Rectangle dragBoxFromMouseDown;
        private object valueFromMouseDown;
        private int idxDragRow;
        private string[] copyRow = new string[3];
        /// <summary>
        /// Initialize instance of class
        /// </summary>
        public MemView(SimView view, ref List<MemoryRecord> List_Memory) {
            this.view = view;
            this.List_Memory = List_Memory;
            InitializeComponent();
            dataGridView_Basic.Rows.Add(3);
            LoadMemory();
        }
        private void MemView_Load(object sender, EventArgs e) {
            //Size = new Size(800, 650);
            float horizontalRatio = 0.4f;
            Grid_Mem.Width = panel_View_PO.Width = Convert.ToInt32(Width * horizontalRatio - 10);
            panel_Edit_PO.Width = Convert.ToInt32(Width * (1 - horizontalRatio) - 10);
            CenterToScreen();

            dataGridView_Basic.Rows[0].Cells[0].Value = Strings.recordType;
            dataGridView_Basic.Rows[1].Cells[0].Value = Strings.decimalValue;
            dataGridView_Basic.Rows[2].Cells[0].Value = Strings.mnemonicName;
            dataGridView_Basic.Rows[0].Height = 66;
            dataGridView_Basic.Rows[1].Height = 66;
            dataGridView_Basic.Rows[2].Height = 66;
            PO_SizeChanged(this, new EventArgs());
        }
        private void LoadMemory() {
            foreach (MemoryRecord row in List_Memory)
                Grid_Mem.Rows.Add(row.addr, row.value, row.hex, row.typ);
            setAllStrings();
        }
        internal void setAllStrings() {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);

            button_Clear_Row.Text = Strings.clearRowButton;
            button_Clear_Table.Text = Strings.clearTableButton;
            button_Edit.Text = Strings.editMemoryButton;
            button_Load_Table.Text = Strings.loadTableButton;
            button_Save_Table.Text = Strings.saveTableButton;
            button_Exit.Text = Strings.exitButton.ToUpper();

            Grid_Mem.Columns[0].HeaderText = Strings.cellAddressViewGrid;
            Grid_Mem.Columns[1].HeaderText = Strings.cellValueViewGrid;

            dataGridView_Basic.Rows[0].Cells[0].Value = Strings.recordType;
            dataGridView_Basic.Rows[1].Cells[0].Value = Strings.decimalValue;
            dataGridView_Basic.Rows[2].Cells[0].Value = Strings.mnemonicName;
            button1.Text = Strings.printButton;

            dataGridView_Decode_Complex.Columns[0].HeaderText = Strings.field;
            Grid_PO_SelectionChanged(this, new EventArgs());
            this.Text = Strings.MemViewTitle;
        }
        /// <summary>
        /// Save internal memoryrecords list to file
        /// </summary>
        /// <param name="fileName">String representing path to file</param>
        public void SaveTable(string fileName) {
            try {
                using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create))) {
                    bw.Write(Grid_Mem.Columns.Count);
                    bw.Write(Grid_Mem.Rows.Count);
                    foreach (DataGridViewRow row in Grid_Mem.Rows) {
                        for (int j = 0; j < Grid_Mem.Columns.Count; ++j) {
                            var val = row.Cells[j].Value;
                            bw.Write(true);
                            if (j == 3 && val.ToString() == "")
                                bw.Write("0");
                            else
                                bw.Write(val.ToString());
                        }
                    }
                }
                uint crc = CRC.ComputePAOChecksum(File.ReadAllBytes(fileName));
                using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Append))) {
                    bw.Write(crc);
                }
                for (int i = 0; i < 256; ++i) {
                    List_Memory[i] = new MemoryRecord(i, (string)Grid_Mem.Rows[i].Cells[1].Value, (string)Grid_Mem.Rows[i].Cells[2].Value,
                        Convert.ToInt16(Grid_Mem.Rows[i].Cells[3].Value));
                }
            }
            catch { MessageBox.Show("Nie można uzyskać dostępu do tego pliku"); }

        }
        /// <summary>
        /// Load memoryrecords from file
        /// </summary>
        /// <param name="fileName">String representing path to file</param>
        public void LoadTable(string fileName) {
            string[] split = fileName.Split('.');
            string extension = split[split.Length - 1];
            try {
                byte[] dataChunk = File.ReadAllBytes(fileName);
                if (dataChunk.Length >= 2974 && CRC.ComputePAOChecksum(File.ReadAllBytes(fileName)) == 0 && Regex.Match(extension, @"[pP][oO]").Success)
                    using (BinaryReader br = new BinaryReader(File.OpenRead(fileName))) {
                        bool isChanged = false;
                        string lastString;
                        int n = br.ReadInt32();
                        int m = br.ReadInt32();
                        if (m == 256 && n == 4) {
                            for (int i = 0; i < m; ++i) {
                                isChanged = false;
                                for (int j = 0; j < n; ++j) {
                                    if (br.ReadBoolean()) {
                                        lastString = br.ReadString();
                                        if (Grid_Mem.Rows[i].Cells[j].Value.ToString() != lastString) {
                                            Grid_Mem.Rows[i].Cells[j].Value = lastString;
                                            isChanged = true;
                                        }
                                    }
                                    else
                                        br.ReadBoolean();
                                }
                                if (isChanged)
                                    AUpdateForm(i, Grid_Mem.Rows[i].Cells[1].Value.ToString(), Grid_Mem.Rows[i].Cells[2].Value.ToString(), Convert.ToInt32(Grid_Mem.Rows[i].Cells[3].Value));
                            }
                        }
                        else
                            MessageBox.Show(Strings.memLoadError, Strings.memLoadErrorTitle, MessageBoxButtons.OK);
                    }
                else if (Regex.Match(extension, @"[sS][aA][gG]").Success)
                //naucz czytania plikow labsaga
                //
                {
                    ;
                }
                else
                    throw new Exception();
            }
            catch (Exception) {
                for (int i = 0; i < 256; ++i)
                    for (int j = 1; j < 4; ++j)
                        Grid_Mem.Rows[i].Cells[j].Value = "";
                MessageBox.Show(Strings.memLoadNotValid, Strings.memLoadErrorTitle, MessageBoxButtons.OK);
            }
        }

        private void grid_PO_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (!view.IsRunning) {
                Grid_Mem.EndEdit();
                if (Grid_Mem.CurrentCell.ColumnIndex > 0) {
                    using (theSubView = new MemSubmit(Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[2].Value.ToString(), Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[3].Value.ToString())) {
                        Point startPosition = Cursor.Position;
                        startPosition.Y -= theSubView.Height / 2;
                        if (startPosition.Y < 0)
                            startPosition.Y = 0;
                        startPosition.X -= theSubView.Width / 2;
                        theSubView.Location = startPosition;
                        var result = theSubView.ShowDialog();
                        if (result == DialogResult.OK) {
                            Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value = theSubView.binaryData;
                            Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[2].Value = theSubView.hexData;
                            Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[3].Value = theSubView.dataType;
                            AUpdateForm(Grid_Mem.CurrentCell.RowIndex, theSubView.binaryData, theSubView.hexData, theSubView.dataType);
                        }
                    }
                }
                Grid_PO_SelectionChanged(sender, e);
            }
        }

        private void grid_PO_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            Grid_PO_SelectionChanged(sender, e);
        }

        private void button_Clear_Row_Click(object sender, EventArgs e) {
            if (!view.IsRunning) {
                int idxRowToClear = Convert.ToInt32(Grid_Mem.CurrentCell.RowIndex);
                Grid_Mem.Rows[idxRowToClear].Cells[1].Value = "";
                Grid_Mem.Rows[idxRowToClear].Cells[2].Value = "";
                Grid_Mem.Rows[idxRowToClear].Cells[3].Value = "0";
                AUpdateForm(idxRowToClear, "", "", 0);
                Grid_PO_SelectionChanged(sender, e);
            }
        }

        private void button_Clear_Table_Click(object sender, EventArgs e) {
            if (!view.IsRunning) {
                DialogResult dr = MessageBox.Show(Strings.areYouSureClearTable, Strings.areYouSureClearTableTitle, MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK) {
                    for (int i = 0; i < 256; i++) {
                        if (Grid_Mem.Rows[i].Cells[2].Value.ToString() != "") {
                            Grid_Mem.Rows[i].Cells[1].Value = "";
                            Grid_Mem.Rows[i].Cells[2].Value = "";
                            Grid_Mem.Rows[i].Cells[3].Value = "0";
                            AUpdateForm(i, "", "", 0);
                        }
                    }
                    Grid_PO_SelectionChanged(sender, e);
                }
            }
        }

        internal void button_Save_Table_Click(object sender, EventArgs e) {
            save_File_Dialog.Filter = "Pamięć operacyjna|*.po|Wszystko|*.*";
            save_File_Dialog.Title = "Zapisz zawartość pamięci";
            if (!Directory.Exists(envPath + @"\PO\"))
                Directory.CreateDirectory(envPath + @"\PO\");
            save_File_Dialog.InitialDirectory = envPath + @"\PO\";
            DialogResult saveFileDialogResult = save_File_Dialog.ShowDialog();
            if (saveFileDialogResult == DialogResult.OK && save_File_Dialog.FileName != "") {
                SaveTable(save_File_Dialog.FileName);
                save_File_Dialog.FileName = "";
            }
        }

        internal void button_Load_Table_Click(object sender, EventArgs e) {
            if (!view.IsRunning) {
                open_File_Dialog.Filter = "Pamięć operacyjna|*.po|Wszystko|*.*";
                open_File_Dialog.Title = "Wczytaj zawartość pamięci operacyjnej";
                if (Directory.Exists(envPath + @"\PO\"))
                    open_File_Dialog.InitialDirectory = envPath + @"\PO\";
                else
                    open_File_Dialog.InitialDirectory = envPath;

                DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
                if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "") {
                    LoadTable(open_File_Dialog.FileName);
                    Show();
                    Grid_PO_SelectionChanged(sender, e);
                }
            }
        }

        private void button_Edit_Click(object sender, EventArgs e) {
            if (!view.IsRunning) {
                Grid_Mem.EndEdit();
                if (Grid_Mem.CurrentCell.ColumnIndex > 0) {
                    using (theSubView = new MemSubmit(Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[2].Value.ToString(), Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[3].Value.ToString())) {
                        theSubView.Location = Cursor.Position;
                        var result = theSubView.ShowDialog();
                        if (result == DialogResult.OK) {
                            Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value = theSubView.binaryData;
                            Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[2].Value = theSubView.hexData;
                            Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[3].Value = theSubView.dataType;
                            AUpdateForm(Grid_Mem.CurrentCell.RowIndex, theSubView.binaryData, theSubView.hexData, theSubView.dataType);
                        }
                    }
                }
                Grid_PO_SelectionChanged(sender, e);
            }
        }

        private void PO_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }

        private void dataGridView_Basic_SelectionChanged(object sender, EventArgs e) {
            dataGridView_Basic.ClearSelection();
        }

        private void Grid_PO_SelectionChanged(object sender, EventArgs e) {
            if (Grid_Mem.CurrentCell != null) {
                int idxRow = Grid_Mem.CurrentCell.RowIndex;
                string cellType = "";
                string instructionMnemo = "";
                dataGridView_Decode_Complex.Rows.Clear();
                short tmp = 0;
                try {
                    tmp = Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value);
                }
                catch {

                }
                if (tmp > 0) {
                    dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo;
                    int dataType = 0;
                    if (Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value) == 1) {
                        cellType = Strings.dataType;
                        if (Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5) == "00000")
                            dataType = 3;
                        else
                            dataType = 2;
                        dataGridView_Decode_Complex.DefaultCellStyle.ForeColor = Color.FromArgb(255, 150, 150, 150);
                    }
                    else
                        dataGridView_Decode_Complex.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    if (Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value) == 2 || dataType == 2) {
                        if (dataType != 2) {
                            cellType = Strings.simpleInstructionType;
                            instructionMnemo = Translator.DecodeInstruction(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                            dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo + " - " + Translator.GetInsrtuctionDescription(instructionMnemo);
                        }
                        //
                        dataGridView_Decode_Complex.Rows.Add("OP",
                            Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2).ToString("X"),
                            Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2), 10),
                            Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                        dataGridView_Decode_Complex.Rows.Add("XSI",
                            Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 3), 2).ToString("X"),
                            Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 3), 2), 10),
                            Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 3));
                        dataGridView_Decode_Complex.Rows.Add("DA",
                            Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8), 2).ToString("X"),
                            Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8), 2), 10),
                            Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8));
                    }
                    else if (Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value) == 3 || dataType == 3) {
                        if (dataType != 3) {
                            cellType = Strings.complexInstructionType;
                            instructionMnemo = Translator.DecodeInstruction(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4));
                            dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo + " - " + Translator.GetInsrtuctionDescription(instructionMnemo);
                        }
                        //
                        dataGridView_Decode_Complex.Rows.Add("OP",
                            Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2).ToString("X"),
                            Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2), 10),
                            Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                        dataGridView_Decode_Complex.Rows.Add("AOP",
                            Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4), 2).ToString("X"),
                            Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4), 2), 10),
                            Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4));
                        dataGridView_Decode_Complex.Rows.Add("N",
                            Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(9, 7), 2).ToString("X"),
                            Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(9, 7), 2), 10),
                            Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(9, 7));
                    }

                    dataGridView_Basic.Rows[0].Cells[1].Value = cellType;

                    if (cellType != "")
                        dataGridView_Basic.Rows[1].Cells[1].Value =
                        Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString(), 2), 10);
                }
                else {
                    dataGridView_Basic.Rows[0].Cells[1].Value = dataGridView_Basic.Rows[1].Cells[1].Value
                        = dataGridView_Basic.Rows[2].Cells[1].Value = "";
                }
                try {
                    dataGridView_Decode_Complex.ColumnHeadersHeight = dataGridView_Decode_Complex.Height / 4;
                }
                catch { }
                foreach (DataGridViewRow x in dataGridView_Decode_Complex.Rows) {
                    x.Height = dataGridView_Decode_Complex.Height / 4;
                }
                int i = 0;
                foreach (DataGridViewColumn x in dataGridView_Decode_Complex.Columns) {
                    if (i++ == 3)
                        x.Width = dataGridView_Decode_Complex.Width / 2;
                    x.Width = dataGridView_Decode_Complex.Width / 6;
                }
                Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Selected = true;
            }
        }

        private void Grid_PO_MouseMove(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
                    DragDropEffects dropEffect = Grid_Mem.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                }
            }
        }

        private void Grid_PO_MouseDown(object sender, MouseEventArgs e) {
            var hitTestInfo = Grid_Mem.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex != -1 && hitTestInfo.ColumnIndex > 0) {
                valueFromMouseDown = Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex].Value;
                idxDragRow = hitTestInfo.RowIndex;
                if (valueFromMouseDown != null) {
                    Size dragSize = SystemInformation.DragSize;
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
            }
            else
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void Grid_PO_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Copy;
        }

        private void Grid_PO_DragDrop(object sender, DragEventArgs e) {
            if (!view.IsRunning) {
                Point clientPoint = Grid_Mem.PointToClient(new Point(e.X, e.Y));

                if (e.Effect == DragDropEffects.Copy) {
                    var hitTestInfo = Grid_Mem.HitTest(clientPoint.X, clientPoint.Y);
                    if (hitTestInfo.ColumnIndex > 0 && hitTestInfo.RowIndex != -1) {
                        Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[1].Value = Grid_Mem.Rows[idxDragRow].Cells[1].Value;
                        Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[2].Value = Grid_Mem.Rows[idxDragRow].Cells[2].Value;
                        Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[3].Value = Grid_Mem.Rows[idxDragRow].Cells[3].Value;
                        AUpdateForm(hitTestInfo.RowIndex, Grid_Mem.Rows[idxDragRow].Cells[1].Value.ToString(), Grid_Mem.Rows[idxDragRow].Cells[2].Value.ToString(), Convert.ToInt16(Grid_Mem.Rows[idxDragRow].Cells[3].Value));
                    }
                }
            }
        }

        private void dataGridView_Decode_Complex_SelectionChanged(object sender, EventArgs e) {
            dataGridView_Decode_Complex.ClearSelection();
        }
        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);
        private void Grid_PO_KeyDown(object sender, KeyEventArgs e) {
            if (!view.IsRunning) {
                if (e.KeyCode == Keys.Enter) {
                    Grid_Mem.ReadOnly = true;
                    Grid_Mem.EndEdit();
                    e.Handled = true;
                }
                if (!e.Control) {
                    if (e.KeyCode == Keys.Delete)
                        button_Clear_Row_Click(sender, e);
                    else if (char.IsLetterOrDigit((char)e.KeyCode) || e.KeyCode == Keys.OemMinus) {
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

                }
                else if (e.KeyCode == Keys.C) {
                    copyRow[0] = Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value.ToString();
                    copyRow[1] = Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    copyRow[2] = Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[3].Value.ToString();

                }
                else if (e.KeyCode == Keys.V) {
                    Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[1].Value = copyRow[0];
                    Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[2].Value = copyRow[1];
                    Grid_Mem.Rows[Grid_Mem.CurrentCell.RowIndex].Cells[3].Value = copyRow[2];
                    AUpdateForm(Grid_Mem.CurrentCell.RowIndex, copyRow[0], copyRow[1], Convert.ToInt16(copyRow[2]));
                }
            }
        }

        private void PO_SizeChanged(object sender, EventArgs e) {
            float horizontalRatio = 0.35f;
            panel_View_PO.Width = Convert.ToInt32(Width * horizontalRatio);
            panel_Left.Width = Convert.ToInt32(Width - panel_View_PO.Width - 20);
            //panel_Left.Height = Convert.ToInt32(Height * 1.0);


            dataGridView_Decode_Complex.Height = panel_Left.Size.Height - 90;
            //panel_Bottom_Right.Height = panel_Right.Height - dataGridView_Decode_Complex.Height;

            panel_Bottom_Right.Height = panel_Down.Height = 90;
            foreach (DataGridViewRow x in dataGridView_Decode_Complex.Rows) {
                x.Height = dataGridView_Decode_Complex.Height / 4;
            }

            Grid_PO_SelectionChanged(this, new EventArgs());
        }

        private void Grid_Mem_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            Grid_Mem.ReadOnly = true;
            decimal tmp;
            try {
                if (decimal.TryParse(Grid_Mem.Rows[e.RowIndex].Cells[1].Value.ToString(), out tmp)) {
                    tmp = Convert.ToInt16(Grid_Mem.Rows[e.RowIndex].Cells[1].Value.ToString(), 10);
                    string bin = Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[e.RowIndex].Cells[1].Value.ToString(), 10), 2).PadLeft(16, '0');
                    string hex = Convert.ToInt16(Grid_Mem.Rows[e.RowIndex].Cells[1].Value.ToString(), 10).ToString("X").PadLeft(4, '0');
                    AUpdateForm(e.RowIndex, bin, hex, 1);
                    Grid_Mem[2, e.RowIndex].Value = hex;
                    Grid_Mem[1, e.RowIndex].Value = bin;
                    Grid_Mem[3, e.RowIndex].Value = 1;
                }
                else {
                    string txt = Grid_Mem.Rows[e.RowIndex].Cells[1].Value.ToString();
                    char[] toRemove = { 'h', 'H' };
                    txt = txt.TrimEnd(toRemove);
                    try {
                        tmp = Convert.ToInt16(txt, 16);
                        string bin = Convert.ToString(Convert.ToInt16(tmp), 2).PadLeft(16, '0');
                        string hex = Convert.ToInt16(tmp).ToString("X").PadLeft(4, '0');
                        AUpdateForm(e.RowIndex, bin, hex, 1);
                        Grid_Mem[2, e.RowIndex].Value = hex;
                        Grid_Mem[1, e.RowIndex].Value = bin;
                        Grid_Mem[3, e.RowIndex].Value = List_Memory[e.RowIndex].typ;
                    }
                    catch {
                        throw new Exception();
                    }
                }
            }
            catch {
                Grid_Mem[1, e.RowIndex].Value = List_Memory[e.RowIndex].value;
                Grid_Mem[2, e.RowIndex].Value = List_Memory[e.RowIndex].hex;
                Grid_Mem[3, e.RowIndex].Value = List_Memory[e.RowIndex].typ;
            }

        }

        internal void MemUpadateFromSimView(int row, string binary, string hex, int type) {
            AUpdateForm(row, binary, hex, type);
            Grid_Mem[2, row].Value = hex;
            Grid_Mem[1, row].Value = binary;
            Grid_Mem[3, row].Value = type;
            Grid_PO_SelectionChanged(this, new EventArgs());
        }

        private void MemView_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape)
                this.Hide();
        }

        private void button_Exit_Click(object sender, EventArgs e) {
            this.Hide();
        }

        internal void button1_Click(object sender, EventArgs e) {
            string allText = "";
            foreach (var row in List_Memory) {
                string tmp = " " + row.addr.PadRight(8, ' ');
                bool addToPrint = false;
                if (row.value != "") {
                    addToPrint = true;
                    tmp += row.value + "b".PadRight(8, ' ');
                    tmp += row.hex + "h".PadRight(8, ' ');
                    if (row.typ == 1) {
                        tmp += row.getInt16Value().ToString().PadRight(8, ' ');
                    }
                    else if (row.typ == 2) {
                        tmp += "OP=" + Convert.ToInt32(row.OP, 2).ToString().PadRight(11, ' ');
                        tmp += "XSI=" + row.XSI.PadRight(8, ' ');
                        tmp += "DA=" + Convert.ToInt32(row.DA, 2).ToString().PadRight(8, ' ');
                    }
                    else if (row.typ == 3) {
                        tmp += "AOP=" + Convert.ToInt32(row.AOP, 2).ToString().PadRight(22, ' ');
                        tmp += "N=" + Convert.ToInt32(row.N, 2).ToString().PadRight(8, ' ');
                    }
                }
                if (addToPrint)
                    allText += tmp + "\r\n";
            }


            string dirPath = envPath + @"\TMP\";
            string filePath;
            do {
                filePath = dirPath + "PAO-" + new Random().Next(1, 10024) + "-" + (new Random().Next(1, 10024) + new Random().Next(100, 160)) + ".txt";
            } while (File.Exists(filePath));


            File.WriteAllText(filePath, allText);
            System.Diagnostics.Process.Start(filePath);


            //
            /*
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

            print.Width = width + 40;
            print.Height = 600;
            print.MaximizeBox = false;
            //log.SizeGripStyle = SizeGripStyle.Hide;

            rtb.Select(0, 0);
            print.ShowDialog();*/
        }

        private void MemView_Shown(object sender, EventArgs e) {
            Grid_PO_SelectionChanged(sender, e);

        }
        internal void changeSelectedPAO(int val) {
            Grid_Mem.Rows[val].Selected = true;
            //Grid_Mem.FirstDisplayedScrollingRowIndex = val;
            Grid_Mem.CurrentCell = Grid_Mem.Rows[val].Cells[1];
            Grid_PO_SelectionChanged(this, new EventArgs());
        }
        internal void showMe(int val) {
            this.Show();
            this.BringToFront();
            changeSelectedPAO(val);
        }

        private void Grid_Mem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {

        }
    }
}
