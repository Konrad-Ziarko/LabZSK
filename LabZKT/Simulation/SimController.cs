using LabZKT.Controls;
using LabZKT.Memory;
using LabZKT.MicroOperations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT.Simulation
{
    /// <summary>
    /// Controls simulation view and model instances
    /// </summary>
    public class SimController
    {
        /// <summary>
        /// Strign representing default path for files
        /// </summary>
        private string envPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZkt";
        /// <summary>
        /// Boolean representing whether debugger is attached or not
        /// </summary>
        //private bool isDebuggerPresent;
        private MemoryRecord lastRecordFromRRC;//pamietaj aby nulowac po zerowaniu rejestrow albo nowej symulacji, to delete?
        /// <summary>
        /// List of loaded operating memory
        /// </summary>
        private List<MemoryRecord> List_Memory = new List<MemoryRecord>();
        /// <summary>
        /// List of loaded microoperations
        /// </summary>
        private List<MicroOperation> List_MicroOp = new List<MicroOperation>();
        /// <summary>
        /// Dictionary for register controls corresponding to their names
        /// </summary>
        private Dictionary<string, NumericTextBox> registers = new Dictionary<string, NumericTextBox>() {
            {"LK", null }, {"A", null },{"MQ", null },{"X", null },{"RAP", null },{"LALU", null },{"RALU", null },
        {"RBP", null },{"ALU", null },{"BUS", null },{"RR", null },{"LR", null },{"RI", null },
        {"RAPS", null },{"RAE", null },{"L", null },{"R", null },{"SUMA", null }};
        /// <summary>
        /// Control representing RBPS register
        /// </summary>
        private TextBox RBPS = new TextBox();
        /// <summary>
        /// Dictionary for flag controls corresponding to their names
        /// </summary>
        private Dictionary<string, BitTextBox> flags = new Dictionary<string, BitTextBox>()
        {
            {"MAV", null }, {"IA",null }, {"INT", null }, {"ZNAK",null }, {"XRO", null }, {"OFF", null }
        };
        private SimModel theModel;
        private SimView theView;
        private PMView pmView;
        private MemView memView;
        private DevConsole devConsole;
        /// <summary>
        /// Initialize flags with default values
        /// </summary>
        private void initFlags()
        {
            flags["MAV"] = new BitTextBox(130, 125, null, "MAV");
            flags["IA"] = new BitTextBox(140, 125, null, "IA");
            flags["INT"] = new BitTextBox(290, 125, null, "INT");
            flags["ZNAK"] = new BitTextBox(450, 65, null, "ZNAK");
            flags["XRO"] = new BitTextBox(460, 65, null, "XRO");
            flags["OFF"] = new BitTextBox(470, 65, null, "OFF");
            foreach (var sig in flags)
                sig.Value.Enabled = false;
        }
        private void initRegisterTextBoxes()
        {
            registers["LK"] = new NumericTextBox("LK", 5, 25, null);
            registers["A"] = new NumericTextBox("A", 155, 25, null);
            registers["MQ"] = new NumericTextBox("MQ", 305, 25, null);
            registers["X"] = new NumericTextBox("X", 455, 25, null);
            registers["RAP"] = new NumericTextBox("RAP", 5, 65, null);
            registers["LALU"] = new NumericTextBox("LALU", 155, 65, null);
            registers["RALU"] = new NumericTextBox("RALU", 305, 65, null);
            registers["RBP"] = new NumericTextBox("RBP", 5, 125, null);
            registers["ALU"] = new NumericTextBox("ALU", 155, 125, null);
            registers["BUS"] = new NumericTextBox("BUS", 5, 185, null);
            registers["RR"] = new NumericTextBox("RR", 155, 225, null);
            registers["LR"] = new NumericTextBox("LR", 305, 225, null);
            registers["RI"] = new NumericTextBox("RI", 455, 225, null);
            registers["RAPS"] = new NumericTextBox("RAPS", 305, 305, null);
            registers["RAE"] = new NumericTextBox("RAE", 455, 305, null);

            registers["L"] = new NumericTextBox("L", 205, 305, null);
            registers["R"] = new NumericTextBox("R", 355, 305, null);
            registers["SUMA"] = new NumericTextBox("SUMA", 285, 355, null);
            registers["L"].Visible = false;
            registers["R"].Visible = false;
            registers["SUMA"].Visible = false;
            //registers["RBPS"].Visible = false;
            //registers["RAPS"].Visible = false;
            //registers["RAE"].Visible = false;

            foreach (var reg in registers)
                reg.Value.Enabled = false;
            RBPS.Enabled = false;
            RBPS.Size = new Size(130, 1);
            RBPS.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            RBPS.Text = "000000000000";
        }
        /// <summary>
        /// Check if program was improperly closed, if so start recovering data
        /// </summary>
        private void initLists()
        {
            Directory.CreateDirectory(envPath + "\\Env");
            Directory.CreateDirectory(envPath + "\\Log");
            Directory.CreateDirectory(envPath + "\\PO");
            Directory.CreateDirectory(envPath + "\\PM");
            for (int i = 0; i < 256; i++)
                List_MicroOp.Add(new MicroOperation(i, "", "", "", "", "", "", "", "", "", "", ""));
            for (int i = 0; i < 256; i++)
                List_Memory.Add(new MemoryRecord(i, "", "", 0));
        }
        /// <summary>
        /// Initialize controller instance
        /// </summary>
        public SimController()
        {
            initLists();
            initFlags();
            initRegisterTextBoxes();
            theModel = new SimModel(ref List_Memory, ref List_MicroOp, ref registers, ref flags, ref RBPS, ref lastRecordFromRRC);
            theView = new SimView(theModel.isNewLogEnabled);
            theModel.addControlToDrawings(theView.getSimPanel());
            devConsole = new DevConsole(theModel, theView);
            foreach (var reg in theModel.registers)
                reg.Value.Parent = theView.getSimPanel();
            foreach (var sig in theModel.flags)
                sig.Value.Parent = theView.getSimPanel();
            theModel.RBPS.Parent = theView.getSimPanel();

            theModel.LoadLists(theView.GetDataGridMem(), theView.GetDataGridPM());
            theModel.rearrangeTextBoxes();

            theView.AEnterEditMode += theModel.enterEditMode;
            theView.ALeaveEditMode += theModel.leaveEditMode;
            theView.AClearRegisters += theModel.clearRegisters;
            theView.AGetMemoryRecord += getMemoryRecord;
            theView.ANewLog += theModel.CloseCurrentLogFile;
            theView.APrepareSimulation += theModel.prepareSimulation;
            theView.ANextTact += nextTact;
            theView.ADrawBackground += theModel.DrawBackground;
            theView.ACheckProperties += checkProperties;
            theView.AButtonOKClicked += ButtonOKClicked;
            theView.ASaveCurrentState += SaveState;
            theView.AShowLog += theModel.ShowLog;
            theView.ACallDevConsole += TheView_ACallDevConsole;
            theView.AStopDevConsole += TheView_AStopDevConsole;
            theView.AEditPAO += TheView_AEditPAO;
            theView.AEditPM += TheView_AEditPM;
            theView.ALoadPAO += TheView_ALoadPAO;
            theView.ALoadPM += TheView_ALoadPM;
            theView.AUpdateForm += MemView_AUpdateForm;
            theView.AShowCurrentLog += theModel.ShowCurrentLog;

            theModel.StartSim += startSim;
            theModel.StopSim += theView.stopSim;
            theModel.AddText += theView.AddText;
            theModel.ButtonNextTactSetVisible += buttonNextTactSetVisible;
            theModel.ButtonOKSetVisivle += buttonOKSetVisible;
            theModel.SetNextTact += setNextTact;
            theModel.ASwitchLayOut += theView.SwitchLayOut;
            theModel.ASelectionChanged += TheModel_ASelectionChanged;

            pmView = new PMView(ref List_MicroOp);
            pmView.AUpdateData += PmView_AUpdateData;

            memView = new MemView(ref List_Memory);
            memView.AUpdateForm += MemView_AUpdateForm;

            theView.ShowDialog();
        }

        private void TheView_ACallDevConsole()
        {
            devConsole.Show();
            devConsole.BringToFront();
        }

        private void TheModel_ASelectionChanged()
        {
            theView.Grid_Mem_SelectionChanged(this, new EventArgs());
        }

        private void MemView_AUpdateForm(int row, string binary, string hex, int type)
        {
            theModel.addTextToLogFile("\n\tZmiana zawartości pamięci operacyjnej:\n\tPAO[" 
                + row + "] = 0x" + List_Memory[row].hex.PadLeft(4, '0') + "     =>     PAO[" + row+"] = 0x" + hex + "\n\n");
            List_Memory[row] = new MemoryRecord(row, binary, hex, type);
            theView.SetDataGridMem(List_Memory, row);
        }

        private void TheView_ALoadPM()
        {
            pmView.button_Load_Table_Click(this, new EventArgs());
        }

        private void TheView_ALoadPAO()
        {
            memView.button_Load_Table_Click(this, new EventArgs());
        }

        private void TheView_AEditPM()
        {
            pmView.Show();
            pmView.BringToFront();
        }

        private void PmView_AUpdateData(int row, int col, string str)
        {
            theModel.addTextToLogFile("\n\tZmiana zawartości pamięci mikroprogramów:\n\tPM[" 
                + row + "][" + List_MicroOp[row].getColumnName(col) + "] = \"" + List_MicroOp[row].getColumn(col) 
                + "\"     =>     PM[" + row + "][" + List_MicroOp[row].getColumnName(col) + "] = \"" + str + "\"\n\n");
            List_MicroOp[row].setValue(col, str);
            theView.SetDataGridPM(List_MicroOp, row, col);
        }

        private void TheView_AEditPAO()
        {
            memView.Show();
            memView.BringToFront();
        }

        private void TheView_AStopDevConsole()
        {
            if (theModel.DEVMODE)
            {
                theModel.DEVMODE = false;
                theModel.DEVREGISTER = null;
                theModel.DEVVALUE = 0;
            }
        }

        private void SaveState(bool b)
        {
            theModel.isNewLogEnabled = b;
        }

        private void ButtonOKClicked()
        {
            theView.buttonOKClicked = theModel.buttonOKClicked = true;
            theModel.validateRegisters();
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
            theView.selectedInstruction = theModel.getMemoryRecord(idxRow);
        }

        private void nextTact()
        {
            theModel.buttonNextTactClicked = true;
            //theModel.nextTact();
            theView.SetGridInfo(theModel.currentTact);
        }
    }
}
