using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZSK.Simulation
{
    internal partial class DevConsole : Form
    {
        SimView viewRef;
        internal DevConsole(SimView view)
        {
            this.viewRef = view;
            InitializeComponent();
            AcceptButton = buttonStart;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!viewRef.isRunning)
            {
                viewRef.DEVMODE = true;
                viewRef.DEVADDCYCLE = false;
                viewRef.DEVREGISTER = registerName.Text;
                validateValue();
                viewRef.DEVVALUE = (int)registerValue.Value;
                if (registerName.SelectedIndex == 0)
                {
                    viewRef.DEVCYCLE = viewRef.currnetCycle;
                    viewRef.DEVINCCYCLE = Convert.ToInt16(registerValue.Value);
                    viewRef.DEVVALUE = viewRef.currnetCycle + Convert.ToInt16(registerValue.Value);
                    viewRef.DEVADDCYCLE = true;
                    viewRef.DEVREGISTER = "L. Cykli";
                }
                viewRef.button_Makro_Click(this, new EventArgs());
            }
        }

        private void validateValue()
        {
            string txt = registerName.SelectedItem.ToString();
            if (txt == "RAPS" || txt == "RAP" || txt == "L" || txt == "R" || txt == "SUMA")
            {
                registerValue.Value %= 255;
                viewRef.DEVVALUE = Convert.ToInt16(registerValue.Value);
            }
            else if (txt == "LK")
            {
                registerValue.Value %= 127;
                viewRef.DEVVALUE = Convert.ToInt16(registerValue.Value);
            }
            else if (txt == "LK")
            {
                registerValue.Value %= 127;
                viewRef.DEVVALUE = Convert.ToInt16(registerValue.Value);
            }
            else if (txt == "Cykle+>")
            {
                registerValue.Value %= 201;
                viewRef.DEVVALUE = Convert.ToInt32(registerValue.Value);
            }
            else
            {
                registerValue.Value %= 0xFFFF;
                viewRef.DEVVALUE = Convert.ToInt16(registerValue.Value);
            }
        }

        private void DevConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void DevConsole_Load(object sender, EventArgs e)
        {
            registerName.SelectedIndex = 0;
        }

        private void DevConsole_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Hide();
        }
    }
}
