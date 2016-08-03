using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class PO : Form
    {
        /// <summary>
        /// Used to determin whether program should ask about saving changes or not
        /// </summary>
        public static bool isChanged = false;
        private string fileForPO = @"\Env\~mem.zkt";
        public List<MemoryRecord> List_Memory { get; private set; }
        public PO(ref List<MemoryRecord> mem)
        {
            List_Memory = mem;
            InitializeComponent();
            foreach (MemoryRecord row in List_Memory)
                Grid_PO.Rows.Add(row.addr, row.value, row.hex, row.typ);
            dataGridView_Basic.Rows.Add(3);
        }

        private void PO_Load(object sender, EventArgs e)
        {
            CancelButton = button_Close;

            Size = new Size(800, 650);
            float horizontalRatio = 0.4f;
            Grid_PO.Width = panel_View_PO.Width = Convert.ToInt32(Width * horizontalRatio - 10);
            panel_Edit_PO.Width = Convert.ToInt32(Width * (1 - horizontalRatio) - 10);
            CenterToScreen();

            dataGridView_Basic.Rows[0].Cells[0].Value = "Typ Komórki";
            dataGridView_Basic.Rows[1].Cells[0].Value = "Wartość dziesiętna";
            dataGridView_Basic.Rows[2].Cells[0].Value = "Mnemonik";
            dataGridView_Basic.Rows[0].Height = 46;
            dataGridView_Basic.Rows[1].Height = 46;
            dataGridView_Basic.Rows[2].Height = 46;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                FileInfo fileInfo = new FileInfo(MainWindow.envPath + fileForPO);
                try
                {
                    if (File.Exists(MainWindow.envPath + fileForPO))
                        fileInfo.Attributes = FileAttributes.Normal;
                    fileInfo.Directory.Create();
                }
                catch (Exception)
                {
                    fileInfo.Directory.Create();
                    File.Delete(MainWindow.envPath + fileForPO);
                }
                finally
                {
                    using (BinaryWriter bw = new BinaryWriter(File.Open(MainWindow.envPath + fileForPO, FileMode.Create)))
                    {
                        bw.Write(Grid_PO.Columns.Count);
                        bw.Write(Grid_PO.Rows.Count);
                        foreach (DataGridViewRow row in Grid_PO.Rows)
                        {
                            for (int j = 0; j < Grid_PO.Columns.Count; ++j)
                            {
                                var val = row.Cells[j].Value;
                                bw.Write(true);
                                if (j == 3 && val.ToString() == "")
                                    bw.Write("0");
                                else
                                    bw.Write(val.ToString());
                            }
                        }
                    }
                    uint crc = CRC.ComputeChecksum(File.ReadAllBytes(MainWindow.envPath + fileForPO));
                    using (BinaryWriter bw = new BinaryWriter(File.Open(MainWindow.envPath + fileForPO, FileMode.Append)))
                    {
                        bw.Write(crc);
                    }
                    fileInfo.Attributes = FileAttributes.Hidden;
                }
            }).Start();
        }

        private void grid_PO_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Grid_PO.CurrentCell.ColumnIndex > 0)
            {
                using (PAOSubmit form_Pao = new PAOSubmit())
                {
                    var result = form_Pao.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Grid_PO.Rows[Grid_PO.CurrentCell.RowIndex].Cells[1].Value = form_Pao.binaryData;
                        Grid_PO.Rows[Grid_PO.CurrentCell.RowIndex].Cells[2].Value = form_Pao.hexData;
                        Grid_PO.Rows[Grid_PO.CurrentCell.RowIndex].Cells[3].Value = form_Pao.dataType;
                    }
                }
            }
            Grid_PO_SelectionChanged(sender, (EventArgs)e);
        }

        private void grid_PO_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Grid_PO_SelectionChanged(sender, (EventArgs)e);
        }

        private void button_Clear_Row_Click(object sender, EventArgs e)
        {
            int idxRowToClear = Convert.ToInt32(Grid_PO.CurrentCell.RowIndex);
            Grid_PO.Rows[idxRowToClear].Cells[1].Value = "";
            Grid_PO.Rows[idxRowToClear].Cells[2].Value = "";
            Grid_PO.Rows[idxRowToClear].Cells[3].Value = "0";
            isChanged = true;
            Grid_PO_SelectionChanged(sender, e);
        }

        private void button_Clear_Table_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 256; i++)
            {
                Grid_PO.Rows[i].Cells[1].Value = "";
                Grid_PO.Rows[i].Cells[2].Value = "";
                Grid_PO.Rows[i].Cells[3].Value = "0";
            }
            isChanged = true;
            Grid_PO_SelectionChanged(sender, e);
        }

        private void button_Save_Table_Click(object sender, EventArgs e)
        {
            save_File_Dialog.Filter = "Pamięć operacyjna|*.po|Wszystko|*.*";
            save_File_Dialog.Title = "Zapisz zawartość pamięci";
            if (!Directory.Exists(MainWindow.envPath + @"\PO\"))
                Directory.CreateDirectory(MainWindow.envPath + @"\PO\");
            save_File_Dialog.InitialDirectory = MainWindow.envPath + @"\PO\";
            DialogResult saveFileDialogResult = save_File_Dialog.ShowDialog();
            if (saveFileDialogResult == DialogResult.OK && save_File_Dialog.FileName != "")
            {
                using (BinaryWriter bw = new BinaryWriter(save_File_Dialog.OpenFile()))
                {
                    bw.Write(Grid_PO.Columns.Count);
                    bw.Write(Grid_PO.Rows.Count);
                    foreach (DataGridViewRow row in Grid_PO.Rows)
                    {
                        for (int j = 0; j < Grid_PO.Columns.Count; ++j)
                        {
                            var val = row.Cells[j].Value;
                            bw.Write(true);
                            if (j == 3 && val.ToString() == "")
                                bw.Write("0");
                            else
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
                List_Memory[i] = new MemoryRecord(i, (string)Grid_PO.Rows[i].Cells[1].Value, (string)Grid_PO.Rows[i].Cells[2].Value,
                    Convert.ToInt16(Grid_PO.Rows[i].Cells[3].Value));
            }
            isChanged = false;
        }

        private void button_Load_Table_Click(object sender, EventArgs e)
        {
            DialogResult askUnsavedChanges = DialogResult.Yes;
            if (isChanged)
                askUnsavedChanges = MessageBox.Show("Wprowadziłeś nie zapisane zmiany.\nNapewno chcesz wczytać plik z pamięcią operacyjną?", "LabZKT", MessageBoxButtons.YesNo);

            if (askUnsavedChanges == DialogResult.Yes)
            {
                open_File_Dialog.Filter = "Pamięć operacyjna|*.po|Wszystko|*.*";
                open_File_Dialog.Title = "Wczytaj zawartość pamięci operacyjnej";
                if (Directory.Exists(MainWindow.envPath + @"\PO\"))
                    open_File_Dialog.InitialDirectory = MainWindow.envPath + @"\PO\";
                else
                    open_File_Dialog.InitialDirectory = MainWindow.envPath;

                DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
                if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
                {
                    string[] split = open_File_Dialog.FileName.Split('.');
                    string extension = split[split.Length - 1];
                    try
                    {
                        byte[] dataChunk = File.ReadAllBytes(open_File_Dialog.FileName);
                        if (dataChunk.Length >= 2974 && CRC.ComputeChecksum(File.ReadAllBytes(open_File_Dialog.FileName)) == 0)
                            using (BinaryReader br = new BinaryReader(open_File_Dialog.OpenFile()))
                            {
                                int n = br.ReadInt32();
                                int m = br.ReadInt32();
                                if (m == 256 && n == 4)
                                {
                                    MemoryRecord tmpMemory;
                                    string singleMemoryRecord = "";
                                    string tmpString = "";
                                    for (int i = 0; i < m; ++i)
                                    {
                                        for (int j = 0; j < n; ++j)
                                        {
                                            if (br.ReadBoolean())
                                            {
                                                tmpString = br.ReadString();
                                                singleMemoryRecord += tmpString + " ";
                                                Grid_PO.Rows[i].Cells[j].Value = tmpString;
                                            }
                                            else
                                                br.ReadBoolean();
                                        }
                                        string[] attributes = singleMemoryRecord.Split(' ');
                                        singleMemoryRecord = "";
                                        tmpMemory = new MemoryRecord(Convert.ToInt16(attributes[0]), attributes[1], attributes[2], Convert.ToInt16(attributes[3]));
                                        List_Memory[i] = tmpMemory;
                                    }
                                    isChanged = true;
                                }
                                else
                                    MessageBox.Show("To nie jest plik z poprawnym mikroprogramem!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
                            }
                        else if (Regex.Match(extension, @"[sS][aA][gG]").Success)
                            //naucz czytania plikow labsaga
                            //
                            ;
                        else
                            MessageBox.Show("Wykryto niespójność pliku!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Wykryto niespójność pliku!", "Ładowanie mikroprogramu przerwane", MessageBoxButtons.OK);
                    }


                }

            }
            Grid_PO_SelectionChanged(sender, e);
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate ()
            {
                Close();
            });
        }

        private void PO_Resize(object sender, EventArgs e)
        {
            float horizontalRatio = 0.2f;
            //float verticalRatio = 0.2f;
            int tempViewPo = Convert.ToInt32(Width * horizontalRatio);

            panel_Left.Height = Convert.ToInt32(Height * 1.0);
            if (tempViewPo > 280)
            {
                panel_View_PO.Width = Convert.ToInt32(280);
            }
            else panel_View_PO.Width = tempViewPo;

            panel_Left.Width = Convert.ToInt32(Width - panel_View_PO.Width - 20);
            panel_Left.Height = Convert.ToInt32(Height * 1.0);

            dataGridView_Decode_Simple.Height = panel_Left.Size.Height * 6 / 10;
            dataGridView_Decode_Complex.Height = panel_Left.Size.Height * 4 / 10;
            dataGridView_Decode_Simple.ColumnHeadersHeight = dataGridView_Decode_Simple.Height / 6;
            foreach (DataGridViewRow x in dataGridView_Decode_Simple.Rows)
            {
                x.Height = dataGridView_Decode_Simple.Height / 6;
            }
            dataGridView_Decode_Complex.ColumnHeadersHeight = dataGridView_Decode_Complex.Height / 4;
            foreach (DataGridViewRow x in dataGridView_Decode_Complex.Rows)
            {
                x.Height = dataGridView_Decode_Complex.Height / 4;
            }
        }

        private void PO_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<MemoryRecord> tmpList = new List<MemoryRecord>();
            try
            {
                for (int i = 0; i < 256; ++i)
                {
                    tmpList.Add(new MemoryRecord(i, Grid_PO.Rows[i].Cells[1].Value.ToString(), Grid_PO.Rows[i].Cells[2].Value.ToString(),
                        Convert.ToInt16(Grid_PO.Rows[i].Cells[3].Value)));
                }

                if (isChanged)
                {
                    DialogResult result = MessageBox.Show("Zapamiętać zmiany w pamięci?", "LabZKT", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        for (int i = 0; i < 256; ++i)
                        {
                            List_Memory[i] = new MemoryRecord(i, Grid_PO.Rows[i].Cells[1].Value.ToString(),
                                Grid_PO.Rows[i].Cells[2].Value.ToString(), Convert.ToInt16(Grid_PO.Rows[i].Cells[3].Value));
                        }
                    }
                    isChanged = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Napotkano nie rozpoznany błąd", "Ups!");
                Application.Exit();
            }
        }

        private void dataGridView_Basic_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView_Basic.ClearSelection();
        }

        private void Grid_PO_SelectionChanged(object sender, EventArgs e)
        {
            int idxRow = Grid_PO.CurrentCell.RowIndex;
            string cellType = "";
            string instructionMnemo = "";
            dataGridView_Decode_Simple.Rows.Clear();
            dataGridView_Decode_Complex.Rows.Clear();
            if (Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[3].Value) > 0)
            {
                dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo;
                if (Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[3].Value) == 1)
                    cellType = "Dana";
                else if (Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[3].Value) == 2)
                {
                    cellType = "Rozkaz prosty";
                    instructionMnemo = Translator.DecodeInstruction(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                    dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo + " - " + Translator.GetInsrtuctionDescription(instructionMnemo);
                    //
                    dataGridView_Decode_Simple.Rows.Add("OP",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                    dataGridView_Decode_Simple.Rows.Add("X",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 1), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 1), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 1));
                    dataGridView_Decode_Simple.Rows.Add("S",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(6, 1), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(6, 1), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(6, 1));
                    dataGridView_Decode_Simple.Rows.Add("I",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(7, 1), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(7, 1), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(7, 1));
                    dataGridView_Decode_Simple.Rows.Add("DA",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8));
                }
                else if (Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[3].Value) == 3)
                {
                    cellType = "Rozkaz rozszerzony";
                    instructionMnemo = Translator.DecodeInstruction(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4));
                    dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo + " - " + Translator.GetInsrtuctionDescription(instructionMnemo);
                    //
                    dataGridView_Decode_Complex.Rows.Add("OP",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                    dataGridView_Decode_Complex.Rows.Add("AOP",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4));
                    dataGridView_Decode_Complex.Rows.Add("N",
                        Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(9, 7), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(9, 7), 2), 10),
                        Grid_PO.Rows[idxRow].Cells[1].Value.ToString().Substring(9, 7));
                }

                dataGridView_Basic.Rows[0].Cells[1].Value = cellType;

                if (cellType != "")
                    dataGridView_Basic.Rows[1].Cells[1].Value =
                    Convert.ToString(Convert.ToInt16(Grid_PO.Rows[idxRow].Cells[1].Value.ToString(), 2), 10);
            }
            else
            {
                dataGridView_Basic.Rows[0].Cells[1].Value = dataGridView_Basic.Rows[1].Cells[1].Value
                    = dataGridView_Basic.Rows[2].Cells[1].Value = "";
            }

            dataGridView_Decode_Simple.Height = panel_Left.Size.Height * 6 / 10;
            dataGridView_Decode_Complex.Height = panel_Left.Size.Height * 4 / 10;
            dataGridView_Decode_Simple.ColumnHeadersHeight = dataGridView_Decode_Simple.Height / 6;
            foreach (DataGridViewRow x in dataGridView_Decode_Simple.Rows)
            {
                x.Height = dataGridView_Decode_Simple.Height / 6;
            }
            dataGridView_Decode_Complex.ColumnHeadersHeight = dataGridView_Decode_Complex.Height / 4;
            foreach (DataGridViewRow x in dataGridView_Decode_Complex.Rows)
            {
                x.Height = dataGridView_Decode_Complex.Height / 4;
            }
        }

        /// Drag & Drop on dataGridView (copy insted of move)
        private Rectangle dragBoxFromMouseDown;
        private object valueFromMouseDown;
        private int idxDragRow;
        private bool wasMaximized;

        private void Grid_PO_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    DragDropEffects dropEffect = Grid_PO.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                }
            }
        }

        private void Grid_PO_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTestInfo = Grid_PO.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex != -1 && hitTestInfo.ColumnIndex > 0)
            {
                valueFromMouseDown = Grid_PO.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex].Value;
                idxDragRow = hitTestInfo.RowIndex;
                if (valueFromMouseDown != null)
                {
                    Size dragSize = SystemInformation.DragSize;
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
            }
            else
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void Grid_PO_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Grid_PO_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = Grid_PO.PointToClient(new Point(e.X, e.Y));

            if (e.Effect == DragDropEffects.Copy)
            {
                var hitTestInfo = Grid_PO.HitTest(clientPoint.X, clientPoint.Y);
                if (hitTestInfo.ColumnIndex > 0 && hitTestInfo.RowIndex != -1)
                {
                    Grid_PO.Rows[hitTestInfo.RowIndex].Cells[1].Value = Grid_PO.Rows[idxDragRow].Cells[1].Value;
                    Grid_PO.Rows[hitTestInfo.RowIndex].Cells[2].Value = Grid_PO.Rows[idxDragRow].Cells[2].Value;
                    Grid_PO.Rows[hitTestInfo.RowIndex].Cells[3].Value = Grid_PO.Rows[idxDragRow].Cells[3].Value;
                }

                isChanged = true;
            }
        }

        private void dataGridView_Decode_Simple_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView_Decode_Simple.ClearSelection();
        }

        private void dataGridView_Decode_Complex_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView_Decode_Complex.ClearSelection();
        }

        private void Grid_PO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                button_Clear_Row_Click(sender, (EventArgs)e);
        }

        private void PO_ResizeEnd(object sender, EventArgs e)
        {
            float horizontalRatio = 0.4f;
            Grid_PO.Width = panel_View_PO.Width = Convert.ToInt32(Width * horizontalRatio - 10);
            panel_Edit_PO.Width = Convert.ToInt32(Width * (1 - horizontalRatio) - 10);

            int startLocation = 12;
            int diffSize = (panel_Right.Height - startLocation - (5 * button_Clear_Row.Size.Height)) / 5;
            diffSize += button_Clear_Row.Size.Height;
            var bLocation = button_Clear_Row.Location;
            bLocation.Y = startLocation;
            bLocation.X = 15;
            button_Clear_Row.Location = bLocation;

            startLocation += diffSize;
            bLocation.Y = startLocation;
            button_Clear_Table.Location = bLocation;

            startLocation += diffSize;
            bLocation.Y = startLocation;
            button_Save_Table.Location = bLocation;

            startLocation += diffSize;
            bLocation.Y = startLocation;
            button_Load_Table.Location = bLocation;

            startLocation += diffSize;
            bLocation.Y = startLocation;
            button_Close.Location = bLocation;
        }
        private void PO_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized && !wasMaximized)
            {
                wasMaximized = true;
                PO_ResizeEnd(sender, e);
            }
            else if (wasMaximized)
            {
                wasMaximized = false;
                PO_ResizeEnd(sender, e);
            }
        }
    }
}
