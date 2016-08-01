using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class PM : Form
    {
        //used to determin whether program should ask about saving changes or not
        public static bool isChanged = false;
        private string fileForPM = @"\Env\~micro.zkt";
        public List<MicroOperation> List_MicroOp;
        public PM(ref List<MicroOperation> micro)
        {
            List_MicroOp = micro;
            InitializeComponent();
            foreach (MicroOperation row in List_MicroOp)
                Grid_PM.Rows.Add(row.addr, row.S1, row.D1, row.S2, row.D2, row.S3, row.D3, row.C1, row.C2, row.Test, row.ALU, row.NA);
        }

        private void PM_Load(object sender, EventArgs e)
        {
            CancelButton = button_Close;
            Size = new Size(800, 650);
            PM_ResizeEnd(sender, e);
            CenterToScreen();
        }
        private void grid_PM_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Grid_PM.CurrentCell.ColumnIndex > 0)
            {
                string newMicroInstruction = "";
                string currentRadioButtonText = (string)Grid_PM.CurrentCell.Value;
                string currentMicroInstruction = currentRadioButtonText.Split()[0];

                using (PMSubmit form_Radio = new PMSubmit(Convert.ToInt32(Grid_PM.CurrentCell.ColumnIndex),
                    currentMicroInstruction, Grid_PM[7, Grid_PM.CurrentCell.RowIndex].Value.ToString()))
                {
                    form_Radio.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                    var result = form_Radio.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        newMicroInstruction = form_Radio.SelectedInstruction;
                        if (currentMicroInstruction == "SHT" && newMicroInstruction != "SHT")
                            Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value = "";
                    }
                    else
                        newMicroInstruction = currentMicroInstruction;

                }
                Grid_PM.CurrentCell.Value = newMicroInstruction;
                if (Grid_PM.CurrentCell.ColumnIndex == 11 && (Grid_PM.CurrentCell.Value.ToString() == "" || Convert.ToInt32(Grid_PM.CurrentCell.Value) == 0))
                    Grid_PM.CurrentCell.Value = "";
                if (newMicroInstruction == "" && Grid_PM.CurrentCell.ColumnIndex == 7)
                {
                    string tmp = Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value.ToString();
                    if (tmp == "ALA" || tmp == "ARA" || tmp == "LRQ" || tmp == "LLQ" || tmp == "LLA" || tmp == "LRA" || tmp == "LCA")
                        Grid_PM[4, Grid_PM.CurrentCell.RowIndex].Value = "";
                }
                else if (newMicroInstruction == "SHT")
                {
                    int idxRow = Grid_PM.CurrentCell.RowIndex;
                    int idxCol = Grid_PM.CurrentCell.ColumnIndex;
                    Grid_PM[3, idxRow].Value = "";
                    Grid_PM[5, idxRow].Value = "";
                    Grid_PM[6, idxRow].Value = "";
                    using (PMSubmit form_Radio = new PMSubmit(4, Grid_PM[4, idxRow].Value.ToString(), Grid_PM[7, idxRow].Value.ToString()))
                    {
                        var result = form_Radio.ShowDialog();
                        if (result == DialogResult.OK)
                            newMicroInstruction = form_Radio.SelectedInstruction;
                        else
                            newMicroInstruction = currentMicroInstruction;
                        Grid_PM[4, idxRow].Value = newMicroInstruction;
                    }
                }
            }
        }

        private void PM_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<MicroOperation> tmpList = new List<MicroOperation>();
            for (int i = 0; i < 256; ++i)
            {
                tmpList.Add(new MicroOperation(i, Grid_PM[1, i].Value.ToString(), Grid_PM[2, i].Value.ToString(),
                    Grid_PM[3, i].Value.ToString(), Grid_PM[4, i].Value.ToString(),
                    Grid_PM[5, i].Value.ToString(), Grid_PM[6, i].Value.ToString(),
                    Grid_PM[7, i].Value.ToString(), Grid_PM[8, i].Value.ToString(),
                    Grid_PM[9, i].Value.ToString(), Grid_PM[10, i].Value.ToString(),
                    Grid_PM[11, i].Value.ToString()));
            }

            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Zapamiętać zmiany w mikroprogramie?", "LabZKT", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < 256; ++i)
                    {
                        List_MicroOp[i] = new MicroOperation(i, Grid_PM[1, i].Value.ToString(), Grid_PM[2, i].Value.ToString(),
                    Grid_PM[3, i].Value.ToString(), Grid_PM[4, i].Value.ToString(),
                    Grid_PM[5, i].Value.ToString(), Grid_PM[6, i].Value.ToString(),
                    Grid_PM[7, i].Value.ToString(), Grid_PM[8, i].Value.ToString(),
                    Grid_PM[9, i].Value.ToString(), Grid_PM[10, i].Value.ToString(),
                    Grid_PM[11, i].Value.ToString());
                    }
                }
                isChanged = false;
            }

        }

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
                Grid_PM[i, idxRowToClear].Value = "";
            isChanged = true;
        }

        private void button_Clear_Table_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 256; i++)
                for (int j = 1; j < 12; j++)
                    Grid_PM[j, i].Value = "";
            isChanged = true;
        }

        private void button_Save_Table_Click(object sender, EventArgs e)
        {
            save_File_Dialog.Filter = "Pamięć mikroprogramu|*.pm|Wszystko|*.*";
            save_File_Dialog.Title = "Zapisz mikroprogram";
            if (!Directory.Exists(MainWindow.envPath + @"\PM\"))
                Directory.CreateDirectory(MainWindow.envPath + @"\PM\");
            save_File_Dialog.InitialDirectory = MainWindow.envPath + @"\PM\";
            DialogResult saveFileDialogResult = save_File_Dialog.ShowDialog();
            if (saveFileDialogResult == DialogResult.OK && save_File_Dialog.FileName != "")
            {
                using (BinaryWriter bw = new BinaryWriter(save_File_Dialog.OpenFile()))
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
                uint crc = CRC.ComputeChecksum(File.ReadAllBytes(save_File_Dialog.FileName));
                using (BinaryWriter bw = new BinaryWriter(File.Open(save_File_Dialog.FileName, FileMode.Append)))
                {
                    bw.Write(crc);
                }
                save_File_Dialog.FileName = "";
            }
            for (int i = 0; i < 256; ++i)
            {
                List_MicroOp[i] = new MicroOperation(i, Grid_PM[1, i].Value.ToString(), Grid_PM[2, i].Value.ToString(),
                    Grid_PM[3, i].Value.ToString(), Grid_PM[4, i].Value.ToString(),
                    Grid_PM[5, i].Value.ToString(), Grid_PM[6, i].Value.ToString(),
                    Grid_PM[7, i].Value.ToString(), Grid_PM[8, i].Value.ToString(),
                    Grid_PM[9, i].Value.ToString(), Grid_PM[10, i].Value.ToString(),
                    Grid_PM[11, i].Value.ToString());
            }
            isChanged = false;
        }

        private void button_Load_Table_Click(object sender, EventArgs e)
        {
            DialogResult askUnsavedChanges = DialogResult.Yes;
            if (isChanged)
                askUnsavedChanges = MessageBox.Show("Wprowadziłeś nie zapisane zmiany.\nNapewno chcesz wczytać inny mikroprogram?", "LabZKT", MessageBoxButtons.YesNo);

            if (askUnsavedChanges == DialogResult.Yes)
            {
                open_File_Dialog.Filter = "Pamięć Mikroprogramu|*.pm|Wszystko|*.*";
                open_File_Dialog.Title = "Wczytaj mikroprogram";
                if (Directory.Exists(MainWindow.envPath + @"\PM\"))
                    open_File_Dialog.InitialDirectory = MainWindow.envPath + @"\PM\";
                else
                    open_File_Dialog.InitialDirectory = MainWindow.envPath;

                DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
                if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
                {
                    //naucz czytania plikow labsaga
                    //
                    //
                    //
                    //
                    //
                    if (CRC.ComputeChecksum(File.ReadAllBytes(open_File_Dialog.FileName)) == 0)
                        using (BinaryReader br = new BinaryReader(open_File_Dialog.OpenFile()))
                        {
                            int n = br.ReadInt32();
                            int m = br.ReadInt32();
                            if (m == 256 && n == 12)
                            {
                                MicroOperation tmpMicroOperation;
                                string allMicroOpInRow = "";
                                string tmpString = "";
                                for (int i = 0; i < m; ++i)
                                {
                                    for (int j = 0; j < n; ++j)
                                    {
                                        if (br.ReadBoolean())
                                        {
                                            tmpString = br.ReadString();
                                            allMicroOpInRow += tmpString + " ";
                                            Grid_PM[j, i].Value = tmpString;
                                        }

                                        else
                                            br.ReadBoolean();
                                    }
                                    string[] attributes = allMicroOpInRow.Split(' ');
                                    allMicroOpInRow = "";
                                    tmpMicroOperation = new MicroOperation(attributes[0], attributes[1], attributes[2], attributes[3],
                                        attributes[4], attributes[5], attributes[6], attributes[7], attributes[8], attributes[9],
                                        attributes[10], attributes[11]);
                                    List_MicroOp[i] = tmpMicroOperation;
                                }
                                isChanged = true;
                            }
                            else
                                MessageBox.Show("To nie jest plik z poprawnym mikroprogramem!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
                        }
                    else
                        MessageBox.Show("Wykryto niespójność pliku!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
                }

            }
        }

        private void grid_PM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int idxRow = Convert.ToInt32(Grid_PM.CurrentCell.RowIndex);
                int idxColumn = Convert.ToInt32(Grid_PM.CurrentCell.ColumnIndex);

                Grid_PM[idxColumn, idxRow].Value = "";
            }
        }

        /// Drag & Drop on dataGridView (copy insted of move)
        private Rectangle dragBoxFromMouseDown;
        private object valueFromMouseDown;
        private int idxDragColumn;
        private bool wasMaximized;

        private void grid_PM_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = Grid_PM.PointToClient(new Point(e.X, e.Y));

            if (e.Effect == DragDropEffects.Copy)
            {
                string valueInCell = e.Data.GetData(typeof(string)) as string;
                var hitTestInfo = Grid_PM.HitTest(clientPoint.X, clientPoint.Y);
                if (hitTestInfo.ColumnIndex > 0 && hitTestInfo.RowIndex != -1 && hitTestInfo.ColumnIndex == idxDragColumn)
                {
                    if (Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT" && valueInCell == "")
                    {
                        Grid_PM[4, hitTestInfo.RowIndex].Value = Grid_PM[7, hitTestInfo.RowIndex].Value = "";
                    }
                    else if (Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT" && valueInCell != "SHT")
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = valueInCell;
                        Grid_PM[4, hitTestInfo.RowIndex].Value = "";
                    }
                    else
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = valueInCell;
                    }

                    if (valueInCell == "SHT")
                    {
                        Grid_PM[3, hitTestInfo.RowIndex].Value = "";
                        Grid_PM[5, hitTestInfo.RowIndex].Value = "";
                        Grid_PM[6, hitTestInfo.RowIndex].Value = "";
                        using (PMSubmit form_Radio = new PMSubmit(4, Grid_PM[4, hitTestInfo.RowIndex].Value.ToString(), Grid_PM[7, hitTestInfo.RowIndex].Value.ToString()))
                        {
                            var result = form_Radio.ShowDialog();
                            if (result == DialogResult.OK)
                                Grid_PM[4, hitTestInfo.RowIndex].Value = form_Radio.SelectedInstruction;
                        }
                    }
                    else if ((hitTestInfo.ColumnIndex == 3 || hitTestInfo.ColumnIndex == 5 || hitTestInfo.ColumnIndex == 6
                        || hitTestInfo.ColumnIndex == 8) && Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT")
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = "";
                    }
                    else if ((valueInCell == "ARA" || valueInCell == "ALA" || valueInCell == "LRQ" || valueInCell == "LLQ"
                       || valueInCell == "LLA" || valueInCell == "LRA" || valueInCell == "LCA") && Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() != "SHT")
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = "";
                    }


                }
                isChanged = true;
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

        //save changes every 15s in temporary file
        private void timer1_Tick(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                FileInfo fileInfo = new FileInfo(MainWindow.envPath + fileForPM);
                try
                {
                    if (File.Exists(MainWindow.envPath + fileForPM))
                        fileInfo.Attributes = FileAttributes.Normal;
                    fileInfo.Directory.Create();
                }
                catch (Exception)
                {
                    fileInfo.Directory.Create();
                    File.Delete(MainWindow.envPath + fileForPM);
                }
                finally
                {
                    using (BinaryWriter bw = new BinaryWriter(File.Open(MainWindow.envPath + fileForPM, FileMode.Create)))
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
                    uint crc = CRC.ComputeChecksum(File.ReadAllBytes(MainWindow.envPath + fileForPM));
                    using (BinaryWriter bw = new BinaryWriter(File.Open(MainWindow.envPath + fileForPM, FileMode.Append)))
                    {
                        bw.Write(crc);
                    }
                    fileInfo.Attributes = FileAttributes.Hidden;
                }
            }).Start();
        }

        private void PM_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized && !wasMaximized)
            {
                wasMaximized = true;
                PM_ResizeEnd(sender, e);
            }
            else if (wasMaximized)
            {
                wasMaximized = false;
                PM_ResizeEnd(sender, e);
            }
        }
        private void PM_ResizeEnd(object sender, EventArgs e)
        {
            int cos = button_Clear_Row.Size.Width;
            panel_Control.Width = 145;
            panel_View_PM.Width = Convert.ToInt32(Width - panel_Control.Width - 20);
            foreach (DataGridViewColumn c in Grid_PM.Columns)
                c.Width = panel_View_PM.Width / 12;
        }
    }
}
