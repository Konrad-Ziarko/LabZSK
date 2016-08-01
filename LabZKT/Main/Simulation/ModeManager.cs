using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LabZKT
{
    /// <summary>
    /// Class for simulation mode management
    /// </summary>
    public class ModeManager
    {
        private Dictionary<string, NumericTextBox> registers;
        private ToolStripMenuItem toolStripMenu_Edit, toolStripMenu_Clear, toolStripMenu_Exit;
        private DataGridView dataGridView_Info;
        private Label label_Status;
        private Button button_Makro, button_Micro, button_Next_Tact;
        private static ModeManager instance;
        public static ModeManager Instance
        {
            get
            {
                return instance;
            }
        }
        public static ModeManager getInstace(ref Dictionary<string, NumericTextBox> regs, ref ToolStripMenuItem tsmie, ref ToolStripMenuItem tsmic,
            ref ToolStripMenuItem tsmiex, ref Label lStatus, ref Button bMakro, ref Button bMicro, ref DataGridView dgvInfo, ref Button bNext_Tact)
        {
            if (instance == null)
                return instance = new ModeManager(ref regs, ref tsmie, ref tsmic, ref tsmiex, ref lStatus, ref bMakro, ref bMicro, ref dgvInfo, ref bNext_Tact);
            else
                return instance;
        }
        private ModeManager(ref Dictionary<string, NumericTextBox> regs, ref ToolStripMenuItem tsmie, ref ToolStripMenuItem tsmic,
            ref ToolStripMenuItem tsmiex, ref Label lStatus, ref Button bMakro, ref Button bMicro, ref DataGridView dgvInfo, ref Button bNext_Tact)
        {
            button_Next_Tact = bNext_Tact;
            dataGridView_Info = dgvInfo;
            registers = regs;
            toolStripMenu_Edit = tsmie;
            toolStripMenu_Clear = tsmic;
            toolStripMenu_Exit = tsmiex;
            label_Status = lStatus;
            button_Makro = bMakro;
            button_Micro = bMicro;
        }

        public void stopSim(out bool oIsRunning, out bool oInMicroMode)
        {
            toolStripMenu_Edit.Enabled = true;
            toolStripMenu_Exit.Enabled = true;
            oIsRunning = false;
            oInMicroMode = false;
            button_Makro.Visible = true;
            button_Micro.Visible = true;
            toolStripMenu_Clear.Enabled = true;
            label_Status.Text = "Stop";
            label_Status.ForeColor = Color.Green;
        }
        public void startSim(out bool OutIsRunning, bool[,] InCells, out bool[,] OutCells)
        {
            OutCells = InCells;
            toolStripMenu_Edit.Enabled = false;
            toolStripMenu_Exit.Enabled = false;
            OutIsRunning = true;
            button_Makro.Visible = false;
            button_Micro.Visible = false;
            toolStripMenu_Clear.Enabled = false;
            label_Status.Text = "Start";
            label_Status.ForeColor = Color.Red;
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 8; j++)
                    OutCells[i, j] = true;
            foreach (var reg in registers)
                reg.Value.setActualValue(reg.Value.getInnerValue());
        }
        public void nextTact(bool inMicroMode, int InCurrentTack, out int OutCurrentTack)
        {
            OutCurrentTack = InCurrentTack;
            if (inMicroMode)//RunSim.inMicroMode)
            {
                button_Next_Tact.Visible = true;
                while (!RunSim.buttonNextTactClicked)
                    Application.DoEvents();
                RunSim.buttonNextTactClicked = false;
            }
            else
            {
                //RunSim.currentTact = (RunSim.currentTact + 1) % 8;
                OutCurrentTack = (InCurrentTack + 1) % 8;
                dataGridView_Info.Rows[2].Cells[0].Value = OutCurrentTack;// RunSim.currentTact;
            }
        }
        /// <summary>
        /// Change current button 'Enable' state to opposite
        /// </summary>
        public void EnDisableButtons()
        {
            foreach (var reg in registers)
                reg.Value.Enabled = !reg.Value.Enabled;
        }
    }
}
