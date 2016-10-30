using LabZKT.Properties;
using LabZKT.StaticClasses;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace LabZKT.Memory
{
    /// <summary>
    /// Displays form responsible for submiting new MemoryRecords
    /// </summary>
    public partial class MemSubmit : Form
    {
        /// <summary>
        /// String representing memory record in binary
        /// </summary>
        public string binaryData { get; private set; }
        /// <summary>
        /// String representing memory record in hexadecimal
        /// </summary>
        public string hexData { get; private set; }
        /// <summary>
        /// Value corresponding to data type
        /// </summary>
        public int dataType { get; private set; }
        private enum DataType { Data, Simple, Complex }
        private enum NumeralBase { Hex, Binary, Decimal }
        private NumeralBase chosenDataType = NumeralBase.Decimal;
        private void turnOffTabSwitchFocus(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.GetType() == typeof(Panel))
                    turnOffTabSwitchFocus(c);
                c.TabStop = false;
            }
        }
        /// <summary>
        /// Initialize instance of class
        /// </summary>
        public MemSubmit()
        {
            InitializeComponent();
            turnOffTabSwitchFocus(this);
        }

        internal void setAllStrings()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);

            this.Text = Strings.MemSubmitTitle;
            button_Choice_Cancel.Text = Strings.cancelChoiceButton;
            button_Choice_Complex.Text = Strings.choiceComplexButton;
            button_Choice_Simple.Text = Strings.choiceSimpleButton;
            button_Choice_Data.Text = Strings.choiceDataButton;

            button_Data_OK.Text = Strings.okDataButton;
            button_Data_Cancel.Text = Strings.cancelDataButton;

            button_Simple_OK.Text = Strings.okSimpleButton;
            button_Simple_Cancel.Text = Strings.cancelSimpleButton;

            button_Complex_OK.Text = Strings.okComplexButton;
            button_Complex_Cancel.Text = Strings.cancelComplexButton;

            labelN.Text = Strings.nLabel;
            labelDA.Text = Strings.daLabel;
            labelOP.Text = Strings.opLabel;
            labelAOP.Text = Strings.aopLabel;

            radioButton_Bin.Text = Strings.binRadioButton;
            radioButton_Dec.Text = Strings.decRadioButton;
            radioButton_Hex.Text = Strings.hexRadioButton;
        }
        private void PAOSubmit_Load(object sender, EventArgs e)
        {
            CancelButton = button_Choice_Cancel;
            comboBox_Simple.SelectedIndex = 0;
            comboBox_Complex.SelectedIndex = 0;

            panel_Complex.Visible = false;
            panel_Data.Visible = false;
            panel_Simple.Visible = false;
        }
        private void setAndClose(DataType type)
        {
            string tempMemoryCell = "";
            dataType = 0;
            if (type == DataType.Data && textBox_Data.Text != "")
            {
                dataType = 1;
                if (chosenDataType == NumeralBase.Binary)
                {
                    tempMemoryCell = textBox_Data.Text;
                }
                else if (chosenDataType == NumeralBase.Decimal)
                {
                    tempMemoryCell = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 10), 2);
                }
                else if (chosenDataType == NumeralBase.Hex)
                {
                    tempMemoryCell = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 16), 2);
                }
            }
            else if (type == DataType.Simple)
            {
                dataType = 2;
                tempMemoryCell = Convert.ToString(Convert.ToInt16((comboBox_Simple.SelectedIndex + 1).ToString(), 10), 2).PadLeft(5, '0')
                    + Convert.ToInt16(checkBox_X.Checked).ToString() + Convert.ToInt16(checkBox_S.Checked).ToString()
                    + Convert.ToInt16(checkBox_I.Checked).ToString()
                    + Convert.ToString(Convert.ToInt16(numericUpDown_DA.Value.ToString(), 10), 2).PadLeft(8, '0');
            }
            else if (type == DataType.Complex)
            {
                dataType = 3;
                tempMemoryCell = "00000" +
                    Convert.ToString(Convert.ToInt16(comboBox_Complex.SelectedIndex.ToString(), 10), 2).PadLeft(4, '0')
                    + Convert.ToString(Convert.ToInt16(numericUpDown_N.Value.ToString(), 10), 2).PadLeft(7, '0');
            }
            DialogResult = DialogResult.OK;
            if (tempMemoryCell != "")
            {
                binaryData = tempMemoryCell.PadLeft(16, '0');
                hexData = Convert.ToInt16(tempMemoryCell, 2).ToString("X").PadLeft(4, '0');
            }
            else binaryData = hexData = "";
            Close();
        }
        private void textBox_Data_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !(e.KeyChar >= '0' && e.KeyChar <= '1') && chosenDataType == NumeralBase.Binary)
            {
                e.Handled = true;
            }
            else if (!char.IsControl(e.KeyChar) && !(char.IsDigit(e.KeyChar) || e.KeyChar == '-') && chosenDataType == NumeralBase.Decimal)
            {
                e.Handled = true;
            }
            else if (!char.IsControl(e.KeyChar) && !(e.KeyChar >= '0' && e.KeyChar <= '9' ||
                e.KeyChar >= 'a' && e.KeyChar <= 'f' || e.KeyChar >= 'A' && e.KeyChar <= 'F') && chosenDataType == NumeralBase.Hex)
            {
                e.Handled = true;
            }
        }
        #region Buttons
        private void button_Choice_Data_Click(object sender, EventArgs e)
        {
            panel_Choice.Visible = false;
            panel_Data.Visible = true;
            AcceptButton = button_Data_OK;
            textBox_Data.Focus();
        }
        private void button_Choice_Simple_Click(object sender, EventArgs e)
        {
            StringCollection sc = new StringCollection();
            sc.Add("ADS - " + Translator.GetInsrtuctionDescription("ADS"));
            sc.Add("SUS - " + Translator.GetInsrtuctionDescription("SUS"));
            sc.Add("MUL - " + Translator.GetInsrtuctionDescription("MUL"));
            sc.Add("DIV - " + Translator.GetInsrtuctionDescription("DIV"));
            sc.Add("STQ - " + Translator.GetInsrtuctionDescription("STQ"));
            sc.Add("STA - " + Translator.GetInsrtuctionDescription("STA"));
            sc.Add("STX - " + Translator.GetInsrtuctionDescription("STX"));
            sc.Add("LDA - " + Translator.GetInsrtuctionDescription("LDA"));
            sc.Add("LDX - " + Translator.GetInsrtuctionDescription("LDX"));
            sc.Add("STC - " + Translator.GetInsrtuctionDescription("STC"));
            sc.Add("TXA - " + Translator.GetInsrtuctionDescription("TXA"));
            sc.Add("TMQ - " + Translator.GetInsrtuctionDescription("TMQ"));
            sc.Add("ADX - " + Translator.GetInsrtuctionDescription("ADX"));
            sc.Add("SIO - " + Translator.GetInsrtuctionDescription("SIO"));
            sc.Add("LIO - " + Translator.GetInsrtuctionDescription("LIO"));
            sc.Add("UNB - " + Translator.GetInsrtuctionDescription("UNB"));
            sc.Add("BAO - " + Translator.GetInsrtuctionDescription("BAO"));
            sc.Add("BXP - " + Translator.GetInsrtuctionDescription("BXP"));
            sc.Add("BXZ - " + Translator.GetInsrtuctionDescription("BXZ"));
            sc.Add("BXN - " + Translator.GetInsrtuctionDescription("BXN"));
            sc.Add("TLD - " + Translator.GetInsrtuctionDescription("TLD"));
            sc.Add("BAP - " + Translator.GetInsrtuctionDescription("BAP"));
            sc.Add("BAZ - " + Translator.GetInsrtuctionDescription("BAZ"));
            sc.Add("BAN - " + Translator.GetInsrtuctionDescription("BAN"));
            sc.Add("LOR - " + Translator.GetInsrtuctionDescription("LOR"));
            sc.Add("LPR - " + Translator.GetInsrtuctionDescription("LPR"));
            sc.Add("LNG - " + Translator.GetInsrtuctionDescription("LNG"));
            sc.Add("EOR - " + Translator.GetInsrtuctionDescription("EOR"));
            sc.Add("SRJ - " + Translator.GetInsrtuctionDescription("SRJ"));
            sc.Add("BDN - " + Translator.GetInsrtuctionDescription("BDN"));
            sc.Add("NOP - " + Translator.GetInsrtuctionDescription("NOP"));
            if (sc.Count == 31)
            {
                comboBox_Simple.Items.Clear();
                foreach (string s in sc)
                    comboBox_Simple.Items.Add(s);
            }
            comboBox_Simple.SelectedIndex = 0;
            panel_Choice.Visible = false;
            panel_Simple.Visible = true;
            AcceptButton = button_Simple_OK;
            comboBox_Simple.Focus();
        }
        private void button_Choice_Complex_Click(object sender, EventArgs e)
        {

            StringCollection sc = new StringCollection();
            sc.Add("STP - " + Translator.GetInsrtuctionDescription("STP"));
            sc.Add("CMA - " + Translator.GetInsrtuctionDescription("CMA"));
            sc.Add("ALA - " + Translator.GetInsrtuctionDescription("ALA"));
            sc.Add("ARA - " + Translator.GetInsrtuctionDescription("ARA"));
            sc.Add("LRQ - " + Translator.GetInsrtuctionDescription("LRQ"));
            sc.Add("LLQ - " + Translator.GetInsrtuctionDescription("LLQ"));
            sc.Add("LLA - " + Translator.GetInsrtuctionDescription("LLA"));
            sc.Add("LRA - " + Translator.GetInsrtuctionDescription("LRA"));
            sc.Add("LCA - " + Translator.GetInsrtuctionDescription("LCA"));
            sc.Add("LAI - " + Translator.GetInsrtuctionDescription("LAI"));
            sc.Add("LXI - " + Translator.GetInsrtuctionDescription("LXI"));
            sc.Add("INX - " + Translator.GetInsrtuctionDescription("INX"));
            sc.Add("DEX - " + Translator.GetInsrtuctionDescription("DEX"));
            sc.Add("CND - " + Translator.GetInsrtuctionDescription("CND"));
            sc.Add("ENI - " + Translator.GetInsrtuctionDescription("ENI"));
            sc.Add("LDS - " + Translator.GetInsrtuctionDescription("LDS"));
            if (sc.Count == 16)
            {
                comboBox_Complex.Items.Clear();
                foreach(string s in sc)
                    comboBox_Complex.Items.Add(s);
            }
            comboBox_Complex.SelectedIndex = 0;
            panel_Choice.Visible = false;
            panel_Complex.Visible = true;
            AcceptButton = button_Complex_OK;
            comboBox_Complex.Focus();
        }
        private void button_Data_Cancel_Click(object sender, EventArgs e)
        {
            panel_Choice.Visible = true;
            panel_Data.Visible = false;
            AcceptButton = null;
            button_Choice_Data.Focus();
        }
        private void button_Simple_Cancel_Click(object sender, EventArgs e)
        {
            panel_Choice.Visible = true;
            panel_Simple.Visible = false;
            AcceptButton = null;
            button_Choice_Data.Focus();
        }
        private void button_Complex_Cancel_Click(object sender, EventArgs e)
        {
            panel_Choice.Visible = true;
            panel_Complex.Visible = false;
            AcceptButton = null;
            button_Choice_Data.Focus();
        }
        private void button_Choice_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void numericUpDown_DA_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_DA.Value = Convert.ToInt16(numericUpDown_DA.Value);
        }
        private void numericUpDown_N_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown_N.Value = Convert.ToInt16(numericUpDown_N.Value);
        }
        private void button_Data_OK_Click(object sender, EventArgs e)
        {
            setAndClose(DataType.Data);
        }
        private void button_Simple_OK_Click(object sender, EventArgs e)
        {
            setAndClose(DataType.Simple);
        }
        private void button_Complex_OK_Click(object sender, EventArgs e)
        {
            setAndClose(DataType.Complex);
        }
        #endregion
        private void comboBox_Simple_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown_Simple.Value = comboBox_Simple.SelectedIndex + 1;
        }
        private void numericUpDown_Simple_ValueChanged(object sender, EventArgs e)
        {
            comboBox_Simple.SelectedIndex = Convert.ToInt16(numericUpDown_Simple.Value) - 1;
        }
        private void comboBox_Complex_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown_Complex.Value = comboBox_Complex.SelectedIndex;
        }
        private void numericUpDown_Complex_ValueChanged(object sender, EventArgs e)
        {
            comboBox_Complex.SelectedIndex = Convert.ToInt16(numericUpDown_Complex.Value);
        }
        private void radioButton_Bin_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox_Data.Text != "")
                if (chosenDataType == NumeralBase.Decimal)
                    textBox_Data.Text = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 10), 2);
                else if (chosenDataType == NumeralBase.Hex)
                    textBox_Data.Text = string.Join(string.Empty,
                        textBox_Data.Text.Select(c => Convert.ToString(Convert.ToInt16(c.ToString(), 16), 2).PadLeft(4, '0')));
            chosenDataType = NumeralBase.Binary;
            validateTextBox();
        }
        private void radioButton_Dec_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox_Data.Text != "")
                if (chosenDataType == NumeralBase.Binary)
                    textBox_Data.Text = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 2), 10);
                else if (chosenDataType == NumeralBase.Hex)
                    textBox_Data.Text = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 16), 10);
            chosenDataType = NumeralBase.Decimal;
            validateTextBox();
        }
        private void radioButton_Hex_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox_Data.Text != "")
                if (chosenDataType == NumeralBase.Binary)
                    textBox_Data.Text = Convert.ToInt16(textBox_Data.Text, 2).ToString("X");
                else if (chosenDataType == NumeralBase.Decimal)
                    textBox_Data.Text = Convert.ToInt16(textBox_Data.Text, 10).ToString("X");
            chosenDataType = NumeralBase.Hex;
            validateTextBox();
        }
        private void textBox_Data_TextChanged(object sender, EventArgs e)
        {
            validateTextBox();
        }
        private void validateTextBox()
        {
            if (chosenDataType == NumeralBase.Decimal)
            {
                try
                {
                    Convert.ToInt16(textBox_Data.Text, 10);
                    button_Data_OK.Enabled = true;
                    radioButton_Bin.Enabled = true;
                    radioButton_Dec.Enabled = true;
                    radioButton_Hex.Enabled = true;
                }
                catch (Exception)
                {
                    button_Data_OK.Enabled = false;
                    radioButton_Bin.Enabled = false;
                    radioButton_Dec.Enabled = false;
                    radioButton_Hex.Enabled = false;
                }
            }
            else if (chosenDataType == NumeralBase.Binary)
            {
                try
                {
                    Convert.ToString(Convert.ToInt16(textBox_Data.Text, 2), 10);
                    button_Data_OK.Enabled = true;
                    radioButton_Bin.Enabled = true;
                    radioButton_Dec.Enabled = true;
                    radioButton_Hex.Enabled = true;
                }
                catch (Exception)
                {
                    button_Data_OK.Enabled = false;
                    radioButton_Bin.Enabled = false;
                    radioButton_Dec.Enabled = false;
                    radioButton_Hex.Enabled = false;
                }
            }
            else if (chosenDataType == NumeralBase.Hex)
            {
                try
                {
                    Convert.ToInt16(textBox_Data.Text, 16);
                    button_Data_OK.Enabled = true;
                    radioButton_Bin.Enabled = true;
                    radioButton_Dec.Enabled = true;
                    radioButton_Hex.Enabled = true;
                }
                catch (Exception)
                {
                    button_Data_OK.Enabled = false;
                    radioButton_Bin.Enabled = false;
                    radioButton_Dec.Enabled = false;
                    radioButton_Hex.Enabled = false;
                }
            }
        }

        private void MemSubmit_Shown(object sender, EventArgs e)
        {
            setAllStrings();
        }
    }
}
