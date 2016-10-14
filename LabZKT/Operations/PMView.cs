using LabZKT.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabZKT.MicroOperations
{
    /// <summary>
    /// Displays microoperations
    /// </summary>
    public partial class PMView : Form
    {
        internal event Action<int, int, string> AUpdateData;

        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
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
        }

        private void PM_Load(object sender, EventArgs e)
        {
            CancelButton = button_Close;
            Size = new Size(800, 650);
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
            uint crc = CRC.ComputeChecksum(File.ReadAllBytes(fileName));
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
                if (dataChunk.Length >= 6814 && CRC.ComputeChecksum(File.ReadAllBytes(fileName)) == 0 && Regex.Match(extension, @"[pP][mM]").Success)
                    using (BinaryReader br = new BinaryReader(File.OpenRead(fileName)))
                    {
                        int n = br.ReadInt32();
                        int m = br.ReadInt32();
                        if (m == 256 && n == 12)
                        {
                            for (int i = 0; i < m; ++i)
                            {
                                for (int j = 0; j < n; ++j)
                                {
                                    if (br.ReadBoolean())
                                    {
                                        Grid_PM[j, i].Value = br.ReadString();
                                        if (j > 0)
                                            AUpdateData(i, j, Grid_PM[j, i].Value.ToString());
                                    }
                                    else
                                        br.ReadBoolean();
                                }
                            }
                        }
                        else
                            MessageBox.Show("To nie jest plik z poprawnym mikroprogramem!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
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
                    for (int j = 0; j < 12; ++j)
                    {
                        Grid_PM[j, i].Value = "";
                        AUpdateData(i, j, "");
                    }
                MessageBox.Show("Wykryto niespójność pliku!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
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
                theSubView.StartPosition = FormStartPosition.CenterScreen;
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
        private void button_Close_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                Close();
            });
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
            for (int i = 0; i < 256; i++)
                for (int j = 1; j < 12; j++)
                {
                    Grid_PM[j, i].Value = "";
                    AUpdateData(i, j, "");
                }
        }
        private void button_Save_Table_Click(object sender, EventArgs e)
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
            using (theSubView = new PMSubmit(4, Grid_PM[4, idx].Value.ToString(), Grid_PM[7, idx].Value.ToString()))
            {
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
                    DragDropEffects dropEffect = Grid_PM.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                }
            }
        }
        #endregion

        private void PM_SizeChanged(object sender, EventArgs e)
        {
            int cos = button_Clear_Row.Size.Width;
            panel_Control.Width = 145;
            panel_View_PM.Width = Convert.ToInt32(Width - panel_Control.Width - 20);
            foreach (DataGridViewColumn c in Grid_PM.Columns)
                c.Width = panel_View_PM.Width / 12;
        }
        internal DataGridView GetDataGrid()
        {
            return Grid_PM;
        }
        internal void SetDataGrid(DataGridView Grid_PM)
        {
            this.Grid_PM = Grid_PM;
        }
    }
}
