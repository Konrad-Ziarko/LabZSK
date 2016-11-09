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
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!viewRef.isRunning)
            {
                viewRef.DEVMODE = true;
                viewRef.DEVREGISTER = registerName.Text;
                viewRef.DEVVALUE = Convert.ToInt16(registerValue.Value);
                viewRef.button_Makro_Click(this, new EventArgs());
            }
        }

        private void DevConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void DevConsole_Load(object sender, EventArgs e)
        {

        }
    }
}
