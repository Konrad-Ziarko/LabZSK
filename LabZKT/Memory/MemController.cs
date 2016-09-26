using System.Collections.Generic;
using System.Windows.Forms;

namespace LabZKT.Memory
{
    /// <summary>
    /// Controller class
    /// </summary>
    public class MemController
    {
        MemModel theModel;
        MemView theView;
        MemSubmit theSubView;

        /// <summary>
        /// Initialize instance of controller class
        /// </summary>
        /// <param name="List_Memory"></param>
        public MemController(ref List<MemoryRecord> List_Memory)
        {
            theModel = new MemModel(ref List_Memory);
            theView = new MemView();
            theModel.Grid_Mem = theView.GetDataGrid();
            theModel.LoadMemory();


            theView.SetDataGrid(theModel.Grid_Mem);

            theView.TimerTick += theModel.TimerTick;
            theView.SaveTable += SaveTable;
            theView.LoadTable += LoadTable;
            theView.CloseForm += CloseForm;
            theView.NewMemoryRecord += NewMemoryRecord;
            theView.ShowDialog();
        }
        /// <summary>
        /// Shows submit dialog form
        /// </summary>
        public void NewMemoryRecord()
        {
            using (theSubView = new MemSubmit())
            {
                theSubView.Location = Cursor.Position;
                var result = theSubView.ShowDialog();
                if (result == DialogResult.OK)
                {
                    theModel.Grid_Mem.Rows[theModel.Grid_Mem.CurrentCell.RowIndex].Cells[1].Value = theSubView.binaryData;
                    theModel.Grid_Mem.Rows[theModel.Grid_Mem.CurrentCell.RowIndex].Cells[2].Value = theSubView.hexData;
                    theModel.Grid_Mem.Rows[theModel.Grid_Mem.CurrentCell.RowIndex].Cells[3].Value = theSubView.dataType;
                    theModel.isChanged = theView.isChanged = true;
                }
            }
        }

        private void CloseForm()
        {
            theModel.Grid_Mem = theView.GetDataGrid();
            theModel.CloseForm();
        }
        /// <summary>
        /// Starts save operating memory procedure
        /// </summary>
        /// <param name="fileName">String representing name of file</param>
        public void SaveTable(string fileName)
        {
            theModel.SaveTable(fileName);
            theView.isChanged = theModel.isChanged;
        }
        /// <summary>
        /// Starts load operating memory procedure
        /// </summary>
        /// <param name="fileName">String representing name of file</param>
        public void LoadTable(string fileName)
        {
            theModel.LoadTable(fileName);
            theView.SetDataGrid(theModel.Grid_Mem);
            theView.isChanged = theModel.isChanged;
        }
    }
}
