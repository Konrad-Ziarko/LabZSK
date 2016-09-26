using LabZKT.StaticClasses;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT.Memory
{
    /// <summary>
    /// Displays memory and allows to modify data
    /// </summary>
    public partial class MemView : Form
    {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        internal event Action<DataGridView> TimerTick;
        internal event Action<string> SaveTable;
        internal event Action<string> LoadTable;
        internal event Action CloseForm;
        internal event Action NewMemoryRecord;
        /// <summary>
        /// Boolean representing whether view was changed
        /// </summary>
        public bool isChanged { get; set; }

        private Rectangle dragBoxFromMouseDown;
        private object valueFromMouseDown;
        private int idxDragRow;
        /// <summary>
        /// Initialize instance of class
        /// </summary>
        public MemView()
        {
            InitializeComponent();
            dataGridView_Basic.Rows.Add(3);
        }

        internal DataGridView GetDataGrid()
        {
            return Grid_Mem;
        }
        internal void SetDataGrid(DataGridView Grid_Mem)
        {
            this.Grid_Mem = Grid_Mem;
        }

        private void MemView_Load(object sender, EventArgs e)
        {
            CancelButton = button_Close;

            Size = new Size(800, 650);
            float horizontalRatio = 0.4f;
            Grid_Mem.Width = panel_View_PO.Width = Convert.ToInt32(Width * horizontalRatio - 10);
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
                TimerTick(Grid_Mem);
            }).Start();
        }

        private void grid_PO_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Grid_Mem.CurrentCell.ColumnIndex > 0)
            {
                NewMemoryRecord();
            }
            Grid_PO_SelectionChanged(sender, e);
        }

        private void grid_PO_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Grid_PO_SelectionChanged(sender, e);
        }

        private void button_Clear_Row_Click(object sender, EventArgs e)
        {
            int idxRowToClear = Convert.ToInt32(Grid_Mem.CurrentCell.RowIndex);
            Grid_Mem.Rows[idxRowToClear].Cells[1].Value = "";
            Grid_Mem.Rows[idxRowToClear].Cells[2].Value = "";
            Grid_Mem.Rows[idxRowToClear].Cells[3].Value = "0";
            isChanged = true;
            Grid_PO_SelectionChanged(sender, e);
        }

        private void button_Clear_Table_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 256; i++)
            {
                Grid_Mem.Rows[i].Cells[1].Value = "";
                Grid_Mem.Rows[i].Cells[2].Value = "";
                Grid_Mem.Rows[i].Cells[3].Value = "0";
            }
            isChanged = true;
            Grid_PO_SelectionChanged(sender, e);
        }

        private void button_Save_Table_Click(object sender, EventArgs e)
        {
            save_File_Dialog.Filter = "Pamięć operacyjna|*.po|Wszystko|*.*";
            save_File_Dialog.Title = "Zapisz zawartość pamięci";
            if (!Directory.Exists(envPath + @"\PO\"))
                Directory.CreateDirectory(envPath + @"\PO\");
            save_File_Dialog.InitialDirectory = envPath + @"\PO\";
            DialogResult saveFileDialogResult = save_File_Dialog.ShowDialog();
            if (saveFileDialogResult == DialogResult.OK && save_File_Dialog.FileName != "")
            {
                SaveTable(save_File_Dialog.FileName);
                save_File_Dialog.FileName = "";
            }
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
                if (Directory.Exists(envPath + @"\PO\"))
                    open_File_Dialog.InitialDirectory = envPath + @"\PO\";
                else
                    open_File_Dialog.InitialDirectory = envPath;

                DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
                if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
                {
                    LoadTable(open_File_Dialog.FileName);
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

        private void PO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Zapamiętać zmiany w pamięci?", "LabZKT", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    CloseForm();
                }
            }
        }

        private void dataGridView_Basic_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView_Basic.ClearSelection();
        }

        private void Grid_PO_SelectionChanged(object sender, EventArgs e)
        {
            int idxRow = Grid_Mem.CurrentCell.RowIndex;
            string cellType = "";
            string instructionMnemo = "";
            dataGridView_Decode_Simple.Rows.Clear();
            dataGridView_Decode_Complex.Rows.Clear();
            if (Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value) > 0)
            {
                dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo;
                if (Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value) == 1)
                    cellType = "Dana";
                else if (Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value) == 2)
                {
                    cellType = "Rozkaz prosty";
                    instructionMnemo = Translator.DecodeInstruction(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                    dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo + " - " + Translator.GetInsrtuctionDescription(instructionMnemo);
                    //
                    dataGridView_Decode_Simple.Rows.Add("OP",
                        Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5), 2), 10),
                        Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(0, 5));
                    dataGridView_Decode_Simple.Rows.Add("X",
                        Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 1), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 1), 2), 10),
                        Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 1));
                    dataGridView_Decode_Simple.Rows.Add("S",
                        Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(6, 1), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(6, 1), 2), 10),
                        Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(6, 1));
                    dataGridView_Decode_Simple.Rows.Add("I",
                        Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(7, 1), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(7, 1), 2), 10),
                        Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(7, 1));
                    dataGridView_Decode_Simple.Rows.Add("DA",
                        Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8), 2).ToString("X"),
                        Convert.ToString(Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8), 2), 10),
                        Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(8, 8));
                }
                else if (Convert.ToInt16(Grid_Mem.Rows[idxRow].Cells[3].Value) == 3)
                {
                    cellType = "Rozkaz rozszerzony";
                    instructionMnemo = Translator.DecodeInstruction(Grid_Mem.Rows[idxRow].Cells[1].Value.ToString().Substring(5, 4));
                    dataGridView_Basic.Rows[2].Cells[1].Value = instructionMnemo + " - " + Translator.GetInsrtuctionDescription(instructionMnemo);
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

        private void Grid_PO_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    DragDropEffects dropEffect = Grid_Mem.DoDragDrop(valueFromMouseDown, DragDropEffects.Copy);
                }
            }
        }

        private void Grid_PO_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTestInfo = Grid_Mem.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex != -1 && hitTestInfo.ColumnIndex > 0)
            {
                valueFromMouseDown = Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex].Value;
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
            Point clientPoint = Grid_Mem.PointToClient(new Point(e.X, e.Y));

            if (e.Effect == DragDropEffects.Copy)
            {
                var hitTestInfo = Grid_Mem.HitTest(clientPoint.X, clientPoint.Y);
                if (hitTestInfo.ColumnIndex > 0 && hitTestInfo.RowIndex != -1)
                {
                    Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[1].Value = Grid_Mem.Rows[idxDragRow].Cells[1].Value;
                    Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[2].Value = Grid_Mem.Rows[idxDragRow].Cells[2].Value;
                    Grid_Mem.Rows[hitTestInfo.RowIndex].Cells[3].Value = Grid_Mem.Rows[idxDragRow].Cells[3].Value;
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
                button_Clear_Row_Click(sender, e);
        }

        private void PO_SizeChanged(object sender, EventArgs e)
        {
            float horizontalRatio = 0.4f;
            panel_View_PO.Width = Convert.ToInt32(Width * horizontalRatio);
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
    }
}
