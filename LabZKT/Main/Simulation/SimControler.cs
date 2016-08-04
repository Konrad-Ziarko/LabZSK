using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabZKT
{
    public class SimControler
    {
        private SimModel theModel;
        private SimView theView;

        public SimControler(SimModel simModel, SimView simView)
        {
            theModel = simModel;
            theView = simView;
            theModel.LoadLists(theView.GetDataGridMem(), theView.GetDataGridPM());
            theModel.rearrangeTextBoxes(theView.getSimPanel());

            theView.EnterEditMode += theModel.enterEditMode;
            theView.LeaveEditMode += theModel.leaveEditMode;
            theView.ClearRegisters += theModel.clearRegisters;
            theView.GetMemoryRecord += getMemoryRecord;
            theView.AddToLog += theModel.addToLog;
            theView.NewLog += theModel.NewLog;
            theView.PrepareSimulation += theModel.prepareSimulation;
            theView.NextTact += nextTact;
            theView.DrawBackground += theModel.DrawBackground;
            theView.CheckRegisters += checkRegisters;
            theView.CheckProperties += checkProperties;

            theModel.StartSim += startSim;
            theModel.StopSim += theView.stopSim;
            theModel.AddText += theView.AddText;
            theModel.ButtonNextTactSetVisible += buttonNextTactSetVisible;
            theModel.ButtonOKSetVisivle += buttonOKSetVisible;
            theModel.SetNextTact += setNextTact;
        }

        private void setNextTact(int value)
        {
            theView.SetGridInfo(value);
        }

        private void buttonOKSetVisible()
        {
            theView.buttonOkSetVisible();
        }

        private void buttonNextTactSetVisible()
        {
            theView.buttonNextTactSetVisible();
        }

        private void checkProperties()
        {
            theView.currentTact = theModel.currentTact;
            theView.isRunning = theModel.isRunning;
            theView.inMicroMode = theModel.inMicroMode;
        }

        private void startSim()
        {
            theView.isRunning = true;
        }

        private void checkRegisters()
        {
            theModel.checkRegisters();
            theModel.buttonOKClicked = false;
        }

        private void getMemoryRecord(int idxRow)
        {
            theModel.getMemoryRecord(idxRow);
            theView.selectedInstruction = theModel.selectedIntruction;
        }

        public void nextTact()
        {
            theModel.buttonNextTactClicked = true;
            theModel.nextTact();
            theView.SetGridInfo(theModel.currentTact);
        }
    }
}
