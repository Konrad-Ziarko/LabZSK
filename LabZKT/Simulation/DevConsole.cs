﻿using System;
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
        SimModel f;
        internal DevConsole(SimModel f)
        {
            this.f = f;
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            f.DEVMODE = true;
            f.DEVREGISTER = registerName.Text;
            f.DEVVALUE = Convert.ToInt16(registerValue.Value);
        }

        private void DevConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            f.devConsole = null;
        }

        private void DevConsole_Load(object sender, EventArgs e)
        {

        }

        internal void deactivate()
        {
            f.DEVMODE = false;
            f.DEVREGISTER = null;
            f.DEVVALUE = 0;
        }
    }
}