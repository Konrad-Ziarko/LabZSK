using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT.Other
{
    public partial class Options : Form
    {
        internal event Action ACallUpdate;
        /// <summary>
        /// Initialize instance of Settings Menu
        /// </summary>
        public Options()
        {
            InitializeComponent();
            if (Properties.Settings.Default.CanEditOptions)
                groupBox1.Enabled = true;
        }

        private void Options_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = Properties.Settings.Default.SkinNum;
            checkBox1.Checked = Properties.Settings.Default.IsDevConsole;
            numericUpDown1.Value = Properties.Settings.Default.Delay;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SkinNum = comboBox1.SelectedIndex;
            ACallUpdate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsDevConsole = checkBox1.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Delay = Convert.ToInt32(numericUpDown1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
