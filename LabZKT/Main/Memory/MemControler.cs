using System.Collections.Generic;
using System.Windows.Forms;

namespace LabZKT
{
    public class MemControler
    {
        MemModel theModel;
        MemView theView;
        MemSubmit theSubView;


        public MemControler(ref List<MemoryRecord> List_Memory)
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
        
        public void NewMemoryRecord()
        {
            using (theSubView = new MemSubmit())
            {
                var result = theSubView.ShowDialog();
                if (result == DialogResult.OK)
                {
                    theModel.Grid_Mem.Rows[theModel.Grid_Mem.CurrentCell.RowIndex].Cells[1].Value = theSubView.binaryData;
                    theModel.Grid_Mem.Rows[theModel.Grid_Mem.CurrentCell.RowIndex].Cells[2].Value = theSubView.hexData;
                    theModel.Grid_Mem.Rows[theModel.Grid_Mem.CurrentCell.RowIndex].Cells[3].Value = theSubView.dataType;
                }
                if (!theModel.isChanged)
                {
                    theModel.isChanged = theSubView.isChanged;
                }
            }
            theView.isChanged = true;
        }

        private void CloseForm()
        {
            theModel.Grid_Mem = theView.GetDataGrid();
            theModel.CloseForm();
        }

        public void SaveTable(string fileName)
        {
            theModel.SaveTable(fileName);
            theView.isChanged = theModel.isChanged;
        }

        public void LoadTable(string fileName)
        {
            theModel.LoadTable(fileName);
            theView.SetDataGrid(theModel.Grid_Mem);
            theView.isChanged = theModel.isChanged;
        }
    }
}
