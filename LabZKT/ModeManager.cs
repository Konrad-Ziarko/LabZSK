using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabZKT
{
    public class ModeManager
    {

        public ModeManager()
        {


        }

        public void stopSim()
        {
            toolStripMenu_Edit.Enabled = true;
            isRunning = false;
            button_Makro.Visible = true;
            button_Micro.Visible = true;
            toolStripMenu_Clear.Enabled = true;
            label_Status.Text = "Stop";
            label_Status.ForeColor = Color.Green;
        }
        public void startSim()
        {
            toolStripMenu_Edit.Enabled = false;
            isRunning = true;
            button_Makro.Visible = false;
            button_Micro.Visible = false;
            toolStripMenu_Clear.Enabled = false;
            label_Status.Text = "Start";
            label_Status.ForeColor = Color.Red;
            for (int i = 0; i < 11; i++)
                for (int j = 0; j < 8; j++)
                    cells[i, j] = true;
            foreach (var reg in registers)
                reg.Value.setActualValue(reg.Value.getInnerValue());
        }
        public void nextTact()
        {
            if (inMicroMode)
            {
                button_Next_Tact.Visible = true;
                while (!buttonNextTactClicked)
                    Application.DoEvents();
                buttonNextTactClicked = false;
            }
            else
            {
                currentTact = (currentTact + 1) % 8;
                dataGridView_Info.Rows[2].Cells[0].Value = currentTact;
            }
        }
        public void EnDisableButtons()
        {
            foreach (var reg in registers)
                reg.Value.Enabled = !reg.Value.Enabled;
        }
    }
}
