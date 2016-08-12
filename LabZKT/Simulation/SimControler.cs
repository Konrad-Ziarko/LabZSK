﻿
using System.Threading;

namespace LabZKT.Simulation
{
    public class SimControler
    {
        private SimModel theModel;
        private SimView theView;
        public SimControler(SimModel simModel)
        {
            theModel = simModel;
            theView = new SimView(theModel.isNewLogEnabled);
            foreach (var reg in theModel.registers)
                reg.Value.Parent = theView.getSimPanel();
            foreach (var sig in theModel.flags)
                sig.Value.Parent = theView.getSimPanel();
            theModel.RBPS.Parent = theView.getSimPanel();

            theModel.LoadLists(theView.GetDataGridMem(), theView.GetDataGridPM());
            theModel.rearrangeTextBoxes(theView.getSimPanel());

            theView.AEnterEditMode += theModel.enterEditMode;
            theView.ALeaveEditMode += theModel.leaveEditMode;
            theView.AClearRegisters += theModel.clearRegisters;
            theView.AGetMemoryRecord += getMemoryRecord;
            theView.AAddToLog += theModel.addToLog;
            theView.ANewLog += theModel.NewLog;
            theView.APrepareSimulation += theModel.prepareSimulation;
            theView.ANextTact += nextTact;
            theView.ADrawBackground += theModel.DrawBackground;
            theView.ACheckProperties += checkProperties;
            theView.AButtonOKClicked += ButtonOKClicked;
            theView.ASaveCurrentState += SaveState;
            theView.AShowLog += theModel.ShowLog;

            theModel.StartSim += startSim;
            theModel.StopSim += theView.stopSim;
            theModel.AddText += theView.AddText;
            theModel.ButtonNextTactSetVisible += buttonNextTactSetVisible;
            theModel.ButtonOKSetVisivle += buttonOKSetVisible;
            theModel.SetNextTact += setNextTact;

            theView.ShowDialog();
        }
        private void SaveState(bool b)
        {
            theModel.isNewLogEnabled = b;
        }

        private void ButtonOKClicked()
        {
            theView.buttonOKClicked = theModel.buttonOKClicked = true;
            theModel.checkRegisters();
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
            theView.currnetCycle = theModel.currnetCycle;
            theView.mark = theModel.mark;
            theView.mistakes = theModel.mistakes;
        }

        private void startSim()
        {
            theView.isRunning = theModel.isRunning = true;
            theView.setNewLog(false);
        }

        private void getMemoryRecord(int idxRow)
        {
            theModel.getMemoryRecord(idxRow);
            theView.selectedInstruction = theModel.selectedIntruction;
        }

        public void nextTact()
        {
            theModel.buttonNextTactClicked = true;
            //theModel.nextTact();
            theView.SetGridInfo(theModel.currentTact);
        }
    }
}