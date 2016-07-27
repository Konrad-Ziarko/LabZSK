using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class PAOSubmit : Form
    {
        private enum Type { Data, Simple, Complex }
        private enum DataType { Hex, Binary, Decimal }
        private DataType chosenDataType = DataType.Decimal;
        public string binaryData { get; private set; }
        public string hexData { get; private set; }
        public int dataType { get; private set; }
        private void turnOffTabSwitchFocus(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.GetType() == typeof(Panel))
                    turnOffTabSwitchFocus(c);
                c.TabStop = false;
            }
        }
        public PAOSubmit()
        {
            InitializeComponent();
            turnOffTabSwitchFocus(this);
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
        private void setAndClose(Type type)
        {
            string tempMemoryCell = "";
            dataType = 0;
            if (type == Type.Data && textBox_Data.Text != "")
            {
                dataType = 1;
                if (chosenDataType == DataType.Binary)
                {
                    tempMemoryCell = textBox_Data.Text;
                }
                else if (chosenDataType == DataType.Decimal)
                {
                    tempMemoryCell = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 10), 2);
                }
                else if (chosenDataType == DataType.Hex)
                {
                    tempMemoryCell = String.Join(String.Empty,
                            textBox_Data.Text.Select(c => Convert.ToString(Convert.ToInt16(c.ToString(), 16), 2).PadLeft(4, '0')));
                }
            }
            else if (type == Type.Simple)
            {
                dataType = 2;
                tempMemoryCell = Convert.ToString(Convert.ToInt16((comboBox_Simple.SelectedIndex + 1).ToString(), 10), 2).PadLeft(5, '0')
                    + Convert.ToInt16(checkBox_X.Checked).ToString() + Convert.ToInt16(checkBox_S.Checked).ToString()
                    + Convert.ToInt16(checkBox_I.Checked).ToString()
                    + Convert.ToString(Convert.ToInt16(numericUpDown_DA.Value.ToString(), 10), 2).PadLeft(8, '0');
            }
            else if (type == Type.Complex)
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
            PO.isChanged = true;
            Close();
        }

        private void textBox_Data_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !(e.KeyChar >= '0' && e.KeyChar <= '1') && chosenDataType == DataType.Binary)
            {
                e.Handled = true;
            }
            else if (!char.IsControl(e.KeyChar) && !(char.IsDigit(e.KeyChar) || e.KeyChar == '-') && chosenDataType == DataType.Decimal)
            {
                e.Handled = true;
            }
            else if (!char.IsControl(e.KeyChar) && !(e.KeyChar >= '0' && e.KeyChar <= '9' ||
                e.KeyChar >= 'a' && e.KeyChar <= 'f' || e.KeyChar >= 'A' && e.KeyChar <= 'F') && chosenDataType == DataType.Hex)
            {
                e.Handled = true;
            }
        }

        private void button_Choice_Data_Click(object sender, EventArgs e)
        {
                panel_Choice.Visible = false;
                panel_Data.Visible = true;
                AcceptButton = button_Data_OK;
                textBox_Data.Focus();
        }
        private void button_Choice_Simple_Click(object sender, EventArgs e)
        {
                panel_Choice.Visible = false;
                panel_Simple.Visible = true;
                AcceptButton = button_Simple_OK;
                comboBox_Simple.Focus();
        }
        private void button_Choice_Complex_Click(object sender, EventArgs e)
        {
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
        private void checkBox_X_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void checkBox_S_CheckedChanged(object sender, EventArgs e)
        {


        }
        private void checkBox_I_CheckedChanged(object sender, EventArgs e)
        {

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
            setAndClose(Type.Data);
        }
        private void button_Simple_OK_Click(object sender, EventArgs e)
        {
            setAndClose(Type.Simple);
        }
        private void button_Complex_OK_Click(object sender, EventArgs e)
        {
            setAndClose(Type.Complex);
        }

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
                if (chosenDataType == DataType.Decimal)
                    textBox_Data.Text = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 10), 2);
                else if (chosenDataType == DataType.Hex)
                    textBox_Data.Text = String.Join(String.Empty,
                        textBox_Data.Text.Select(c => Convert.ToString(Convert.ToInt16(c.ToString(), 16), 2).PadLeft(4, '0')));
            chosenDataType = DataType.Binary;
            validateTextBox();
        }
        private void radioButton_Dec_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox_Data.Text != "")
                if (chosenDataType == DataType.Binary)
                    textBox_Data.Text = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 2), 10);
                else if (chosenDataType == DataType.Hex)
                    textBox_Data.Text = Convert.ToString(Convert.ToInt16(textBox_Data.Text, 16), 10);
            chosenDataType = DataType.Decimal;
            validateTextBox();
        }
        private void radioButton_Hex_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox_Data.Text != "")
                if (chosenDataType == DataType.Binary)
                    textBox_Data.Text = Convert.ToInt16(textBox_Data.Text, 2).ToString("X");
                else if (chosenDataType == DataType.Decimal)
                    textBox_Data.Text = Convert.ToInt16(textBox_Data.Text, 10).ToString("X");
            chosenDataType = DataType.Hex;
            validateTextBox();
        }

        private void textBox_Data_TextChanged(object sender, EventArgs e)
        {
            validateTextBox();
        }

        private void validateTextBox()
        {
            if (chosenDataType == DataType.Decimal)
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
            else if (chosenDataType == DataType.Binary)
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
            else if (chosenDataType == DataType.Hex)
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
    }
}
