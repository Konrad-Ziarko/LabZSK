using LabZKT.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT.Other
{
    public partial class Options : Form
    {
        private bool suppressReload = true;
        internal event Action ACallUpdate;
        /// <summary>
        /// Initialize instance of Settings Menu
        /// </summary>
        public Options()
        {
            InitializeComponent();
            if (Settings.Default.CanEditOptions)
                groupBox1.Enabled = true;
        }

        private void Options_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = Settings.Default.SkinNum;
            comboBox2.SelectedIndex = Settings.Default.CultureIdx;
            checkBox1.Checked = Settings.Default.IsDevConsole;
            numericUpDown1.Value = Settings.Default.Delay;
            numericUpDown4.Value = Settings.Default.FirstMark;
            numericUpDown3.Value = Settings.Default.SecondMark;
            numericUpDown2.Value = Settings.Default.ThirdMark;

            setAllStrings();
        }
        internal void setAllStrings()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);

            this.Text = Strings.OptionsTitle;
            label1.Text = Strings.themeLabel; 
            label2.Text = Strings.autoModeLatencyLabel;
            label3.Text = Strings.gradingScaleLabel;
            label6.Text = Strings.languageLabel;
            label7.Text = Strings.devConsoleLabel;

            button1.Text = Strings.saveConfigButton;
            button2.Text = Strings.loadConfigButton;

            groupBox1.Text = Strings.groupBoxName;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { Strings.themeComboBox1, Strings.themeComboBox2, Strings.themeComboBox3, Strings.themeComboBox4 });

            suppressReload = true;
            comboBox1.SelectedIndex = Settings.Default.SkinNum;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.SkinNum = comboBox1.SelectedIndex;
            if (!suppressReload)
                ACallUpdate();
            suppressReload = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.IsDevConsole = checkBox1.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.Delay = Convert.ToInt32(numericUpDown1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //zapisz konfiguracje
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //odczyt konfiguracji
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.FirstMark = Convert.ToInt32(numericUpDown4.Value);
            Settings.Default.SecondMark = Convert.ToInt32(numericUpDown3.Value);
            Settings.Default.ThirdMark = Convert.ToInt32(numericUpDown2.Value);
            Settings.Default.Save();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown4.Value == numericUpDown3.Value)
                numericUpDown3.Value++;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value == numericUpDown2.Value)
                numericUpDown2.Value++;
            if (numericUpDown4.Value == numericUpDown3.Value)
                numericUpDown4.Value--;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value == numericUpDown2.Value)
            {
                numericUpDown4.Value = numericUpDown2.Value - 2;
                numericUpDown3.Value = numericUpDown2.Value - 1;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                Settings.Default.CultureIdx = 0;
                Settings.Default.Culture = "pl-PL";
            }
            else if(comboBox2.SelectedIndex == 1)
            {
                Settings.Default.Culture = "en-US";
                Settings.Default.CultureIdx = 1;
            }

            setAllStrings();
        }
    }
}
