using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT.Simulation
{
    internal partial class DevConsole : Form
    {
        SimModel modelRef;
        internal DevConsole(SimModel f)
        {
            this.modelRef = f;
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            modelRef.DEVMODE = true;
            modelRef.DEVREGISTER = registerName.Text;
            modelRef.DEVVALUE = Convert.ToInt16(registerValue.Value);
        }

        private void DevConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            modelRef.devConsole = null;
        }

        private void DevConsole_Load(object sender, EventArgs e)
        {

        }

        internal void deactivate()
        {
            modelRef.DEVMODE = false;
            modelRef.DEVREGISTER = null;
            modelRef.DEVVALUE = 0;
        }
    }
}
