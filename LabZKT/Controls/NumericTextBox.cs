using LabZKT.Simulation;
using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabZKT
{
    /// <summary>
    /// Class used to hold data for single register
    /// </summary>
    public class NumericTextBox : TextBox
    {
        private short innerValue = 0, actualValue = 0;
        public string registerName { get; private set; }
        public bool needCheck { get; set; }

        public NumericTextBox(string name, int x, int y, Control c)
        {
            registerName = name;
            ReadOnly = AllowDrop = true;
            needCheck = false;
            Text = "0h\t0";
            Size = new Size(130, 1);
            Parent = c;
            var loc = Location;
            loc.X = x;
            loc.Y = y;
            Location = loc;
            BackColor = Color.White;
            Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == (char)Keys.Enter)
            {
                Parent.Focus();
            }

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar) || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F'))
            {
                if (Text.Length > 0 && Text[Text.Length - 1] == 'h')
                {
                    if (SelectionLength > 0 && SelectionStart < Text.Length - 1)
                    {
                        int lastSelectionStart = SelectionStart;
                        Text = Text.Substring(0, lastSelectionStart) + Text.Substring(lastSelectionStart + SelectionLength);
                        SelectionStart = lastSelectionStart;
                    }
                    else if (SelectionStart == Text.Length)
                    {
                        e.Handled = true;
                    }
                    else if (SelectionLength > 0 && SelectionStart == Text.Length - 1)
                    {
                        Text = Text.Substring(0, Text.Length - 1);
                        SelectionStart = Text.Length;
                    }
                }
            }
            else if (e.KeyChar == '\b')
            {
                if (Text.Length == 2 && Text[1] == 'h' && SelectionStart == 1) { Text = "0"; }
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = true;
                if (Text.Length > 0 && Text[Text.Length - 1] != 'h' && Regex.IsMatch(Text, @"^-?\d+$"))
                {
                    if (Text[0] != '-')
                        Text = '-' + Text;
                    else
                        Text = Text.Substring(1);
                }
                else if (Text.Length == 0)
                {
                    Text = "-";
                }
                SelectionStart = Text.Length;
            }
            else if (e.KeyChar == 'h')
            {
                e.Handled = true;
                if (Text.Length > 0 && Text[Text.Length - 1] != 'h')
                {
                    Text += 'h';
                    if (Text[0] == '-')
                        Text = Text.Substring(1);
                }
                SelectionStart = Text.Length;
            }
            else
            {
                e.Handled = true;
                //    MessageBeep();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            try
            {
                if (Text.Length > 0 && Text[Text.Length - 1] == 'h')
                {
                    innerValue = Convert.ToInt16(Text.Substring(0, Text.Length - 1), 16);
                }
                else if (Text.Length > 0 && Text[Text.Length - 1] != 'h')
                {
                    if (Regex.IsMatch(Text, @"^-?\d+$"))
                    {
                        innerValue = Convert.ToInt16(Text);
                    }
                    else if (Regex.IsMatch(Text, @"^[0-9a-fA-F]+h?$"))
                    {
                        innerValue = Convert.ToInt16(Text, 16);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                setText();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (ReadOnly == true)
            {

            }
            else
            {
                base.OnGotFocus(e);
                Text = "";
            }
        }

        /// Drag & Drop on dataGridView (copy insted of move)
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            SimView.hitTest = new Size(e.X, e.Y);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && Math.Sqrt(Math.Pow(SimView.hitTest.Width - e.X, 2) + Math.Pow(SimView.hitTest.Height - e.Y, 2)) > 30)
            {
                SimView.dragValue = innerValue;
                DoDragDrop(innerValue, DragDropEffects.Copy);
                setText();
            }
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            drgevent.Effect = DragDropEffects.Copy;
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            try
            {
                innerValue = SimView.dragValue;
                actualValue = innerValue;
                setText();
            }
            catch (Exception)
            {
                MessageBox.Show("Błędne dane D&D!", "LabZKT", MessageBoxButtons.OK);
            }

        }
        //zrobić delegata
        public void SetXY(int x, int y)
        {
            Point loc = Location;
            loc.X = x;
            loc.Y = y;
            Location = loc;
        }
        public void resetValue()
        {
            innerValue = 0;
            actualValue = 0;
            setText();
        }
        public short getInnerValue()
        {
            return innerValue;
        }
        public void setInnerValue(short newVal)
        {
            innerValue = newVal;
            setText();
        }
        public short getActualValue()
        {
            return actualValue;
        }
        public void setActualValue(short newVal)
        {
            actualValue = newVal;
            clampValue();
        }
        public void setNeedCheck(out string name)
        {
            needCheck = true;
            ReadOnly = false;
            BackColor = Color.Yellow;
            name = registerName;
        }
        public bool checkValue(out short badValue)
        {
            needCheck = false;
            ReadOnly = true;
            BackColor = Color.White;
            setText();
            if (actualValue != innerValue)
            {
                badValue = innerValue;
                innerValue = actualValue;
                return false;
            }
            else
            {
                badValue = 0;
                return true;
            }
        }
        public void setText()
        {
            clampValue();
            Text = innerValue.ToString("X") + "h\t" + innerValue;
        }
        public void incrementInnerValue()
        {
            innerValue = (short)(innerValue + 1);
            clampValue();
        }
        public void setInnerAndActual(short newVal)
        {
            innerValue = actualValue = newVal;
            needCheck = false;
            setText();
        }
        private void clampValue()
        {
            if (registerName == "RAE")
            {
                innerValue &= 63;
                actualValue &= 63;
            }
            else if (registerName == "LK")
            {
                innerValue &= 127;
                actualValue &= 127;
            }
            else if (registerName == "RAPS")
            {
                innerValue &= 255;
                actualValue &= 255;
            }
            else if (registerName == "RAP")
            {
                innerValue &= 255;
                actualValue &= 255;
            }
            else if (registerName == "LR")
            {
                innerValue &= 255;
                actualValue &= 255;
            }
        }
    }
}
