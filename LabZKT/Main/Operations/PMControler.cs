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
            this.theView.LoadMicroOperations(this.theModel.List_MicroOps);
            this.theModel.Grid_PM = this.theView.GetDataGrid();


            this.theView.TimerTick += this.theModel.TimerTick;
            this.theView.LoadTable += LoadTable;
            this.theView.SaveTable += SaveTable;
            this.theView.CloseForm += CloseForm;
            this.theView.NewMicroOperation += NewMicroOperation;
            this.theView.CallSubView += CallSubView;
        }

        private void CallSubView(int idx)
        {
            using (theSubView = new PMSubmit(4, theModel.Grid_PM[4, idx].Value.ToString(), theModel.Grid_PM[7, idx].Value.ToString()))
            {
                var result = theSubView.ShowDialog();
                if (result == DialogResult.OK)
                    theModel.Grid_PM[4, idx].Value = theSubView.SelectedInstruction;
            }
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
            if(newMicroInstruction == "SHT")
            {
                using (theSubView = new PMSubmit(4, theModel.Grid_PM[4, theModel.idxRow].Value.ToString(), theModel.Grid_PM[7, theModel.idxRow].Value.ToString()))
                {
                    var result = theSubView.ShowDialog();
                    if (result == DialogResult.OK)
                        newMicroInstruction = theSubView.SelectedInstruction;
                    else
                        newMicroInstruction = currentMicroInstruction;
                    theModel.Grid_PM[4, theModel.idxRow].Value = newMicroInstruction;
                }
            }
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
