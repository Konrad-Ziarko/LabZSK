using LabZSK.Controls;
using LabZSK.Properties;
using LabZSK.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK.MicroOperations
{
    /// <summary>
    /// Displays microoperations
    /// </summary>
    public partial class PMView : Form
    {
        internal event Action<int, int, string> AUpdateData;

        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK";
        internal PMSubmit theSubView;
        private Rectangle dragBoxFromMouseDown;
        private object valueFromMouseDown;
        private int idxDragColumn;

        /// <summary>
        /// String representing path to file
        /// </summary>
        /// <summary>
        /// Represents current row in microoperations
        /// </summary>
        public int idxRow { get; set; }
        /// <summary>
        /// List for microoperations used in class
        /// </summary>
        public List<MicroOperation> List_MicroOps { get; set; }
        /// <summary>
        /// Initialize instance of class
        /// </summary>
        public PMView(ref List<MicroOperation> List_MicroOps)
        {
            InitializeComponent();
            this.List_MicroOps = List_MicroOps;
            LoadMicroOperations();
            setAllStrings();
        }
        internal void setAllStrings()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);
            button_Clear_Row.Text = Strings.clearRowButton;
            button_Clear_Table.Text = Strings.clearTableButton;
            button_Edit.Text = Strings.editMemoryButton;
            button_Load_Table.Text = Strings.loadTableButton;
            button_Save_Table.Text = Strings.saveTableButton;
            button_Exit.Text = Strings.exitButton.ToUpper();
            Grid_PM.Columns[0].HeaderText = Strings.cellAddressViewGrid;
            button1.Text = Strings.printButton;

            this.Text = Strings.PMViewTitle;
        }
        private void PM_Load(object sender, EventArgs e)
        {
            Size = new Size(820, 650);
            CenterToScreen();
        }
        internal void CloseForm()
        {
            for (int i = 0; i < 256; ++i)
            {
                List_MicroOps[i] = new MicroOperation(i, Grid_PM[1, i].Value.ToString(), Grid_PM[2, i].Value.ToString(),
            Grid_PM[3, i].Value.ToString(), Grid_PM[4, i].Value.ToString(),
            Grid_PM[5, i].Value.ToString(), Grid_PM[6, i].Value.ToString(),
            Grid_PM[7, i].Value.ToString(), Grid_PM[8, i].Value.ToString(),
            Grid_PM[9, i].Value.ToString(), Grid_PM[10, i].Value.ToString(),
            Grid_PM[11, i].Value.ToString());
            }
        }
        /// <summary>
        /// Replace microoperation in microoperation
        /// </summary>
        /// <param name="newMicroInstruction">String representing new microinstruction name</param>
        /// <param name="currentMicroInstruction">String representing current microinstruction nam</param>
        public void NewMicroInstruction(string newMicroInstruction, string currentMicroInstruction)
        {
            Grid_PM.CurrentCell.Value = newMicroInstruction;
            AUpdateData(Grid_PM.CurrentCell.RowIndex, Grid_PM.CurrentCell.ColumnIndex, newMicroInstruction);
            if (Grid_PM.CurrentCell.ColumnIndex == 11 && (Grid_PM.CurrentCell.Value.ToString() == "" || Convert.ToInt32(Grid_PM.CurrentCell.Value) == 0))
            {
                Grid_PM.CurrentCell.Value = "";
                AUpdateData(Grid_PM.CurrentCell.RowIndex, Grid_PM.CurrentCell.ColumnIndex, "");
            }
            if (newMicroInstruction == "" && Grid_PM.CurrentCell.ColumnIndex == 7)
            {
                string tmp = Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value.ToString();
                if (tmp == "ALA" || tmp == "ARA" || tmp == "LRQ" || tmp == "LLQ" || tmp == "LLA" || tmp == "LRA" || tmp == "LCA")
                {
                    Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value = "";
                    AUpdateData(Grid_PM.CurrentCell.RowIndex, 4, "");
                }
            }
            else if (newMicroInstruction == "SHT")
            {
                idxRow = Grid_PM.CurrentCell.RowIndex;
                int idxCol = Grid_PM.CurrentCell.ColumnIndex;
                Grid_PM[3, idxRow].Value = "";
                AUpdateData(idxRow, 3, "");
                Grid_PM[5, idxRow].Value = "";
                AUpdateData(idxRow, 5, "");
                Grid_PM[6, idxRow].Value = "";
                AUpdateData(idxRow, 6, "");
            }
        }
        /// <summary>
        /// Save all mircrooperations to file
        /// </summary>
        /// <param name="fileName">String representing path to file</param>
        public void SaveTable(string fileName)
        {
            try
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    bw.Write(Grid_PM.Columns.Count);
                    bw.Write(Grid_PM.Rows.Count);
                    foreach (DataGridViewRow row in Grid_PM.Rows)
                    {
                        for (int j = 0; j < Grid_PM.Columns.Count; ++j)
                        {
                            var val = row.Cells[j].Value;
                            bw.Write(true);
                            bw.Write(val.ToString());
                        }
                    }
                }
                uint crc = CRC.ComputePMChecksum(File.ReadAllBytes(fileName));
                using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Append)))
                {
                    bw.Write(crc);
                }
                for (int i = 0; i < 256; ++i)
                {
                    List_MicroOps[i] = new MicroOperation(i, Grid_PM[1, i].Value.ToString(), Grid_PM[2, i].Value.ToString(),
                        Grid_PM[3, i].Value.ToString(), Grid_PM[4, i].Value.ToString(),
                        Grid_PM[5, i].Value.ToString(), Grid_PM[6, i].Value.ToString(),
                        Grid_PM[7, i].Value.ToString(), Grid_PM[8, i].Value.ToString(),
                        Grid_PM[9, i].Value.ToString(), Grid_PM[10, i].Value.ToString(),
                        Grid_PM[11, i].Value.ToString());
                }
            }
            catch { MessageBox.Show("Nie można uzyskać dostępu do tego pliku"); }
            
        }
        /// <summary>
        /// Load microoperations from file
        /// </summary>
        /// <param name="fileName">String representing path to file</param>
        public void LoadTable(string fileName)
        {
            string[] split = fileName.Split('.');
            string extension = split[split.Length - 1];

            try
            {
                byte[] dataChunk = File.ReadAllBytes(fileName);
                if (dataChunk.Length >= 6814 && CRC.ComputePMChecksum(File.ReadAllBytes(fileName)) == 0 && Regex.Match(extension, @"[pP][mM]").Success)
                    using (BinaryReader br = new BinaryReader(File.OpenRead(fileName)))
                    {
                        int n = br.ReadInt32();
                        int m = br.ReadInt32();
                        string lastString;
                        if (m == 256 && n == 12)
                        {
                            for (int i = 0; i < m; ++i)
                            {
                                for (int j = 0; j < n; ++j)
                                {
                                    if (br.ReadBoolean())
                                    {
                                        lastString = br.ReadString();
                                        if (j > 0)
                                            if (Grid_PM[j, i].Value.ToString() != lastString)
                                            {
                                                Grid_PM[j, i].Value = lastString;
                                                AUpdateData(i, j, Grid_PM[j, i].Value.ToString());
                                            }
                                    }
                                    else
                                        br.ReadBoolean();
                                }
                            }
                        }
                        else
                            MessageBox.Show(Strings.notValidMicrocodeFile, Strings.notValidMicrocodeFileTitle, MessageBoxButtons.OK);
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
            catch (Exception)
            {
                for (int i = 0; i < 256; ++i)
                    for (int j = 1; j < 12; ++j)
                    {
                        Grid_PM[j, i].Value = "";
                        AUpdateData(i, j, "");
                    }
                MessageBox.Show(Strings.inconsistentMicrocodeFile, Strings.notValidMicrocodeFileTitle, MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Initialize GridView with default data
        /// </summary>
        public void LoadMicroOperations()
        {
            foreach (MicroOperation row in List_MicroOps)
                Grid_PM.Rows.Add(row.addr, row.S1, row.D1, row.S2, row.D2, row.S3, row.D3, row.C1, row.C2, row.Test, row.ALU, row.NA);
        }
        private void grid_PM_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Grid_PM.CurrentCell.ColumnIndex > 0)
            {
                NewMicroOperation();
            }
        }
        private void NewMicroOperation()
        {
            string newMicroInstruction = "";
            string currentRadioButtonText = (string)Grid_PM.CurrentCell.Value;
            string currentMicroInstruction = currentRadioButtonText.Split()[0];

            

            using (theSubView = new PMSubmit(Convert.ToInt32(Grid_PM.CurrentCell.ColumnIndex),
                currentMicroInstruction, Grid_PM[7, Grid_PM.CurrentCell.RowIndex].Value.ToString()))
            {
                //theSubView.StartPosition = FormStartPosition.CenterScreen;
                theSubView.startPosition = Cursor.Position;
                var result = theSubView.ShowDialog();
                if (result == DialogResult.OK)
                {
                    newMicroInstruction = theSubView.SelectedInstruction;
                    if (currentMicroInstruction == "SHT" && newMicroInstruction != "SHT")
                    {
                        Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value = "";
                        AUpdateData(Grid_PM.CurrentCell.RowIndex, 4, "");
                    }
                }
                else
                    newMicroInstruction = currentMicroInstruction;

            }
            NewMicroInstruction(newMicroInstruction, currentMicroInstruction);
            if (newMicroInstruction == "SHT")
            {
                using (theSubView = new PMSubmit(4, Grid_PM[4, idxRow].Value.ToString(), Grid_PM[7, idxRow].Value.ToString()))
                {
                    theSubView.startPosition = Cursor.Position;
                    var result = theSubView.ShowDialog();
                    if (result == DialogResult.OK)
                        newMicroInstruction = theSubView.SelectedInstruction;
                    else
                        newMicroInstruction = currentMicroInstruction;
                    Grid_PM[4, idxRow].Value = newMicroInstruction;
                    AUpdateData(idxRow, 4, newMicroInstruction);
                }
            }
        }
        private void PM_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        #region Buttons
        private void button_Edit_Click(object sender, EventArgs e)
        {
            if (Grid_PM.CurrentCell.ColumnIndex > 0)
            {
                NewMicroOperation();
            }
        }
        private void button_Clear_Row_Click(object sender, EventArgs e)
        {
            int idxRowToClear = Convert.ToInt32(Grid_PM.CurrentCell.RowIndex);
            for (int i = 1; i < 12; i++)
            {
                Grid_PM[i, idxRowToClear].Value = "";
                AUpdateData(idxRowToClear, i, "");
            }

        }
        private void button_Clear_Table_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(Strings.areYouSureClearTable, Strings.areYouSureClearTableTitle, MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
                for (int i = 0; i < 256; i++)
                    for (int j = 1; j < 12; j++)
                    {
                        Grid_PM[j, i].Value = "";
                        AUpdateData(i, j, "");
                    }
        }
        internal void button_Save_Table_Click(object sender, EventArgs e)
        {
            save_File_Dialog.Filter = "Pamięć mikroprogramu|*.pm|Wszystko|*.*";
            save_File_Dialog.Title = "Zapisz mikroprogram";
            if (!Directory.Exists(envPath + @"\PM\"))
                Directory.CreateDirectory(envPath + @"\PM\");
            save_File_Dialog.InitialDirectory = envPath + @"\PM\";
            DialogResult saveFileDialogResult = save_File_Dialog.ShowDialog();
            if (saveFileDialogResult == DialogResult.OK && save_File_Dialog.FileName != "")
            {
                SaveTable(save_File_Dialog.FileName);
                save_File_Dialog.FileName = "";
            }
        }
        internal void button_Load_Table_Click(object sender, EventArgs e)
        {
            open_File_Dialog.Filter = "Pamięć Mikroprogramu|*.pm|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj mikroprogram";
            if (Directory.Exists(envPath + @"\PM\"))
                open_File_Dialog.InitialDirectory = envPath + @"\PM\";
            else
                open_File_Dialog.InitialDirectory = envPath;

            DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
            if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
            {
                LoadTable(open_File_Dialog.FileName);
                Show();
            }
        }
        #endregion
        #region Grid methods+DragAndDrop
        private void grid_PM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int idxRow = Convert.ToInt32(Grid_PM.CurrentCell.RowIndex);
                int idxColumn = Convert.ToInt32(Grid_PM.CurrentCell.ColumnIndex);

                Grid_PM[idxColumn, idxRow].Value = "";
                AUpdateData(idxRow, idxColumn, "");
            }
        }
        private void grid_PM_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = Grid_PM.PointToClient(new Point(e.X, e.Y));

            if (e.Effect == DragDropEffects.Copy)
            {
                string valueInCell = e.Data.GetData(typeof(string)) as string;
                var hitTestInfo = Grid_PM.HitTest(clientPoint.X, clientPoint.Y);
                if (hitTestInfo.ColumnIndex > 0 && hitTestInfo.RowIndex != -1 && hitTestInfo.ColumnIndex == idxDragColumn)
                {
                    if (Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT" && hitTestInfo.ColumnIndex == 7 && valueInCell == "")
                    {
                        Grid_PM[4, hitTestInfo.RowIndex].Value = Grid_PM[7, hitTestInfo.RowIndex].Value = "";
                        AUpdateData(hitTestInfo.RowIndex, 7, "");
                        AUpdateData(hitTestInfo.RowIndex, 4, "");
                    }
                    else if (Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT" && hitTestInfo.ColumnIndex == 7 && valueInCell != "SHT")
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = valueInCell;
                        Grid_PM[4, hitTestInfo.RowIndex].Value = "";
                        AUpdateData(hitTestInfo.RowIndex, hitTestInfo.ColumnIndex, valueInCell);
                        AUpdateData(hitTestInfo.RowIndex, 4, "");
                    }
                    else if (Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT" && hitTestInfo.ColumnIndex == 4)
                    {
                        Grid_PM[4, hitTestInfo.RowIndex].Value = Grid_PM[7, hitTestInfo.RowIndex].Value = "";
                        AUpdateData(hitTestInfo.RowIndex, 7, "");
                        AUpdateData(hitTestInfo.RowIndex, 4, "");
                    }
                    else
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = valueInCell;
                        AUpdateData(hitTestInfo.RowIndex, hitTestInfo.ColumnIndex, valueInCell);
                    }

                    if (valueInCell == "SHT")
                    {
                        Grid_PM[3, hitTestInfo.RowIndex].Value = "";
                        AUpdateData(hitTestInfo.RowIndex, 3, "");
                        Grid_PM[5, hitTestInfo.RowIndex].Value = "";
                        AUpdateData(hitTestInfo.RowIndex, 5, "");
                        Grid_PM[6, hitTestInfo.RowIndex].Value = "";
                        AUpdateData(hitTestInfo.RowIndex, 6, "");
                        CallSubView(hitTestInfo.RowIndex);
                    }
                    else if ((valueInCell == "ARA" || valueInCell == "ALA" || valueInCell == "LRQ" || valueInCell == "LLQ"
                       || valueInCell == "LLA" || valueInCell == "LRA" || valueInCell == "LCA") && Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() != "SHT")
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = "";
                        AUpdateData(hitTestInfo.RowIndex, hitTestInfo.ColumnIndex, "");
                    }
                }
            }
        }
        private void CallSubView(int idx)
        {
            Point startPosition = Cursor.Position;
            startPosition.Y -= theSubView.Height / 2;
            startPosition.X -= theSubView.Width / 2;
            using (theSubView = new PMSubmit(4, Grid_PM[4, idx].Value.ToString(), Grid_PM[7, idx].Value.ToString()))
            {
                theSubView.startPosition = Cursor.Position;
                var result = theSubView.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Grid_PM[4, idx].Value = theSubView.SelectedInstruction;
                    AUpdateData(idx, 4, theSubView.SelectedInstruction);
                }
            }
        }
        private void grid_PM_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void grid_PM_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTestInfo = Grid_PM.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex != -1 && hitTestInfo.ColumnIndex > 0)
            {
                valueFromMouseDown = Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value;
                idxDragColumn = hitTestInfo.ColumnIndex;
                if (valueFromMouseDown != null)
                {
                    Size dragSize = SystemInformation.DragSize;
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
            }
            else
                dragBoxFromMouseDown = Rectangle.Empty;
        }
        private void grid_PM_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    try {
                        DragDropEffects dropEffect = Grid_PM.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                    }
                    catch { }
                }
            }
        }
        #endregion

        private void PM_SizeChanged(object sender, EventArgs e)
        {
            /*
            int cos = button_Clear_Row.Size.Width;
            panel_Control.Width = 145;
            panel_View_PM.Width = Convert.ToInt32(Width - panel_Control.Width - 20);
            foreach (DataGridViewColumn c in Grid_PM.Columns)
                c.Width = panel_View_PM.Width / 12;

            
            var bLocation = button_Clear_Row.Location;
            int startLocation = panel_Control.Height - button_Clear_Table.Size.Height - 15;
            bLocation.Y = startLocation;
            button_Exit.Location = bLocation;
            bLocation.Y -= button_Clear_Table.Size.Height + 15;
            button_Clear_Table.Location = bLocation;
            */
        }
        internal DataGridView GetDataGrid()
        {
            return Grid_PM;
        }
        internal void SetDataGrid(DataGridView Grid_PM)
        {
            this.Grid_PM = Grid_PM;
        }

        private void PMView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Hide();
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        internal void button1_Click(object sender, EventArgs e)
        {
            string allText = "";
            foreach(var row in List_MicroOps)
            {
                string tmp = " " + row.addr.PadRight(8, ' ');
                bool addToPrint = false;
                if (row.S1 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 1);
                }
                 if (row.D1 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 2);
                }
                 if (row.S2 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 3);
                }
                 if (row.D2 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 4);
                }
                 if (row.S3 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 5);
                }
                 if (row.D3 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 6);
                }
                 if (row.C1 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 7);
                }
                 if (row.C2 != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 8);
                }
                if (row.Test != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 9);
                }
                if (row.ALU != "")
                {
                    addToPrint = AddToPrint(addToPrint, row, ref tmp, 10);
                }
                if (row.NA != "")
                {
                    if (addToPrint)
                        tmp += "".PadRight(9, ' ');
                    addToPrint = true;
                    tmp += "NA".PadRight(8, ' ');
                    tmp += "___" + row.NA.PadRight(8, ' ') + "\r\n";
                }
                if (addToPrint)
                    allText += tmp + "\r\n";
            }
            string dirPath = envPath + @"\TMP\";
            string filePath;
            do
            {
                filePath = dirPath + "PM-"+new Random().Next(1, 10024) + "-" + (new Random().Next(1, 10024) + new Random().Next(50, 80))+ ".txt";
            } while (File.Exists(filePath));


            File.WriteAllText(filePath, allText);
            System.Diagnostics.Process.Start(filePath);
        }

        private static bool AddToPrint(bool add, MicroOperation row, ref string tmp, int idx)
        {
            if (add)
                tmp += "".PadRight(9, ' ');
            bool addToPrint = true;
            tmp += row.getColumnName(idx).PadRight(8, ' ');
            //tmp += "___" + row.getColumn(idx).PadRight(8, ' ');
            tmp += "___" + Translator.GetMicroOpExtendedDescription(row.getColumn(idx)).PadRight(8, ' ') + "\r\n";
            return addToPrint;
        }

        private void Grid_PM_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_PM_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Grid_PM[e.ColumnIndex, e.RowIndex].Value = 255 & Convert.ToInt32(Grid_PM[e.ColumnIndex, e.RowIndex].Value);
            AUpdateData(e.RowIndex, e.ColumnIndex, Grid_PM[e.ColumnIndex, e.RowIndex].Value.ToString());
        }
    }
}
