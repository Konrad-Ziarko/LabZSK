using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT
{
    class PMControler
    {
        PMModel theModel;
        PMView theView;
        PMSubmit theSubView;

        public PMControler(PMModel theModel, PMView theView)
        {
            this.theModel = theModel;
            this.theView = theView;
            theView.LoadMicroOperations(theModel.List_MicroOps);
            theModel.Grid_PM = theView.GetDataGrid();


            theView.TimerTick += theModel.TimerTick;
            theView.LoadTable += this.LoadTable;
            theView.SaveTable += this.SaveTable;
            theView.CloseForm += this.CloseForm;
            theView.NewMicroOperation += this.NewMicroOperation;
        }

        private void NewMicroOperation()
        {
            string newMicroInstruction = "";
            string currentRadioButtonText = (string)theModel.Grid_PM.CurrentCell.Value;
            string currentMicroInstruction = currentRadioButtonText.Split()[0];

            using (theSubView = new PMSubmit(Convert.ToInt32(theModel.Grid_PM.CurrentCell.ColumnIndex),
                currentMicroInstruction, theModel.Grid_PM[7, theModel.Grid_PM.CurrentCell.RowIndex].Value.ToString()))
            {
                theSubView.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                var result = theSubView.ShowDialog();
                if (result == DialogResult.OK)
                {
                    newMicroInstruction = theSubView.SelectedInstruction;
                    if (currentMicroInstruction == "SHT" && newMicroInstruction != "SHT")
                        theModel.Grid_PM[4, theModel.Grid_PM.CurrentCell.RowIndex].Value = "";
                    if (!theModel.isChanged)
                    {
                        theModel.isChanged = theSubView.isChanged;
                    }
                }
                else
                    newMicroInstruction = currentMicroInstruction;

            }
            theModel.NewMicroOperation(newMicroInstruction, currentMicroInstruction);
            theView.SetDataGrid(theModel.Grid_PM);

            theView.isChanged = theModel.isChanged;
        }

        private void CloseForm()
        {
            theModel.Grid_PM = theView.GetDataGrid();
            theModel.CloseForm();
        }

        private void SaveTable(string filePath)
        {
            theModel.SaveTable(filePath);
            theView.isChanged = theModel.isChanged;
        }

        private void LoadTable(string filePath)
        {
            theModel.LoadTable(filePath);
            theView.SetDataGrid(theModel.Grid_PM);
            theView.isChanged = theModel.isChanged;
        }
    }
}
