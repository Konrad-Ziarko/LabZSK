using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT.MicroOperations
{
    /// <summary>
    /// Displays microoperations
    /// </summary>
    public partial class PMView : Form
    {
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        internal event Action<DataGridView> TimerTick;
        internal event Action<string> LoadTable;
        internal event Action<string> SaveTable;
        internal event Action CloseForm;
        internal event Action NewMicroOperation;
        internal event Action<int> CallSubView;

        private Rectangle dragBoxFromMouseDown;
        private object valueFromMouseDown;
        private int idxDragColumn;
        /// <summary>
        /// Boolean representing whether view was changed
        /// </summary>
        public bool isChanged { get; set; }
        /// <summary>
        /// Initialize instance of class
        /// </summary>
        public PMView()
        {
            InitializeComponent();
        }

        private void PM_Load(object sender, EventArgs e)
        {
            CancelButton = button_Close;
            Size = new Size(800, 650);
            CenterToScreen();
        }

        private void grid_PM_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Grid_PM.CurrentCell.ColumnIndex > 0)
            {
                NewMicroOperation();
            }
        }

        private void PM_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isChanged)
            {
                DialogResult result = MessageBox.Show("Zapamiętać zmiany w mikroprogramie?", "LabZKT", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    CloseForm();
                }
            }
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
        private void button_Load_Table_Click(object sender, EventArgs e)
        {
            DialogResult askUnsavedChanges = DialogResult.Yes;
            if (isChanged)
                askUnsavedChanges = MessageBox.Show("Wprowadziłeś nie zapisane zmiany.\nNapewno chcesz wczytać inny mikroprogram?", "LabZKT", MessageBoxButtons.YesNo);

            if (askUnsavedChanges == DialogResult.Yes)
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
                }
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
                    }
                    else if (Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT" && hitTestInfo.ColumnIndex==7 && valueInCell != "SHT")
                    {
                        Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = valueInCell;
                        Grid_PM[4, hitTestInfo.RowIndex].Value = "";
                    }
                    else if (Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT" && hitTestInfo.ColumnIndex == 4 )
                    {
                        Grid_PM[4, hitTestInfo.RowIndex].Value = Grid_PM[7, hitTestInfo.RowIndex].Value = "";
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
                        CallSubView(hitTestInfo.RowIndex);                        
                    }
                    //else if ((hitTestInfo.ColumnIndex == 3 || hitTestInfo.ColumnIndex == 5 || hitTestInfo.ColumnIndex == 6
                    //    || hitTestInfo.ColumnIndex == 8) && Grid_PM[7, hitTestInfo.RowIndex].Value.ToString() == "SHT")
                    //{
                    //    //Grid_PM[hitTestInfo.ColumnIndex, hitTestInfo.RowIndex].Value = "";
                    //}
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
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                TimerTick(Grid_PM);
            }).Start();
        }

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
