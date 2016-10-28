using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabZKT.Controls
{
    /// <summary>
    /// Class which represents single CPU register
    /// </summary>
    public class NumericTextBox : TextBox
    {
        /// <summary>
        /// String representing register name
        /// </summary>
        public string registerName { get; private set; }
        /// <summary>
        /// Boolean value representing whether register value was changed and needs to be validated
        /// </summary>
        public bool needCheck { get; set; }
        /// <summary>
        /// Value stored in register
        /// </summary>
        public short innerValue { get; private set; }
        /// <summary>
        /// Value which should be moved to this register
        /// </summary>
        public short valueWhichShouldBeMovedToRegister { get; private set; }
        private static short dragValue;
        private static Point hitTest;
        private Color customeBackColor;

        /// <summary>
        /// Initialize new instance of NumericTextBox
        /// </summary>
        /// <param name="name">String representing register name</param>
        /// <param name="x">Value representing control X position</param>
        /// <param name="y">Value representing control Y position</param>
        /// <param name="c">Parent control for this instance</param>
        public NumericTextBox(string name, int x, int y, Control c)
        {
            Color customeBackColor = SystemColors.Window;
            innerValue = valueWhichShouldBeMovedToRegister = 0;
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
        /// <summary>
        /// Occurs when key was pressed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == (char)Keys.Enter)
            {
                Parent.Focus();
            }
            else if (!Text.Contains("\t"))

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
                }
            else
                e.Handled = true;
        }
        /// <summary>
        /// Occurs when control loses focus
        /// </summary>
        /// <param name="e"></param>
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
        /// <summary>
        /// Occures when control got focus
        /// </summary>
        /// <param name="e"></param>
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
        /// <summary>
        /// Occurses when control was double clicked
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            Text = "";
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            OnGotFocus(e);
        }
        /// <summary>
        /// Check if value in textbox is still valid
        /// </summary>
        /// <param name="e">Event args</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
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
        }
        #region Drag&Drop
        /// <summary>
        /// Occures when mouse button is down
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            hitTest = new Point(e.X, e.Y);
            OnGotFocus(e);
        }
        /// <summary>
        /// Occures when mouse was moved
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && Math.Sqrt(Math.Pow(hitTest.X - e.X, 2) + Math.Pow(hitTest.Y - e.Y, 2)) > 30)
            {
                dragValue = innerValue;
                DoDragDrop(innerValue, DragDropEffects.Copy);
                setText();
                OnLostFocus(e);
            }
        }
        /// <summary>
        /// Occures when drag started
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            drgevent.Effect = DragDropEffects.Copy;
        }
        /// <summary>
        /// Occures when drag was ended
        /// </summary>
        /// <param name="drgevent"></param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            try
            {
                innerValue = dragValue;
                setText();
            }
            catch (Exception)
            {
                MessageBox.Show("Błędne dane D&D!", "LabZKT", MessageBoxButtons.OK);
            }
        }
        #endregion

        /// <summary>
        /// Set location coordinates for this control
        /// </summary>
        /// <param name="x">New X position</param>
        /// <param name="y">New Y position</param>
        public void SetXY(int x, int y)
        {
            Point loc = Location;
            loc.X = x;
            loc.Y = y;
            Location = loc;
        }
        /// <summary>
        /// Reset value stored in register to default
        /// </summary>
        public void resetValue()
        {
            innerValue = 0;
            valueWhichShouldBeMovedToRegister = 0;
            setText();
        }
        /// <summary>
        /// Set value stored in register
        /// </summary>
        /// <param name="newVal">Value to store in register</param>
        public void setInnerValue(short newVal)
        {
            innerValue = newVal;
            setText();
        }
        /// <summary>
        /// Set what value should be moved to that register
        /// </summary>
        /// <param name="newVal">Expected register value</param>
        public void setActualValue(short newVal)
        {
            valueWhichShouldBeMovedToRegister = newVal;
            clampValue();
        }
        /// <summary>
        /// Set if register needs to be validated
        /// </summary>
        /// <param name="name">String representing register name</param>
        public void setNeedCheck(out string name)
        {
            needCheck = true;
            ReadOnly = false;
            BackColor = Color.Yellow;
            name = registerName;
        }
        /// <summary>
        /// Validate value moved to register
        /// </summary>
        /// <param name="badValue">Incorrect value moved to register or 0</param>
        /// <returns>Boolean representing validation whether register was valid or not</returns>
        public bool validateRegisterValue(out short badValue)
        {
            needCheck = false;
            ReadOnly = true;
            BackColor = customeBackColor;
            setText();
            if (valueWhichShouldBeMovedToRegister != innerValue)
            {
                badValue = innerValue;
                innerValue = valueWhichShouldBeMovedToRegister;
                return false;
            }
            else
            {
                badValue = 0;
                return true;
            }
        }
        /// <summary>
        /// Set register hexadecimal and decimal text representation of stored value
        /// </summary>
        private void setText()
        {
            clampValue();
            Text = innerValue.ToString("X") + "h\t" + innerValue;
        }
        /// <summary>
        /// Set value stored and expected value for register
        /// </summary>
        /// <param name="newVal">New value to be stored</param>
        public void setInnerAndExpectedValue(short newVal)
        {
            innerValue = valueWhichShouldBeMovedToRegister = newVal;
            needCheck = false;
            setText();
        }
        public void setCustomeBackColor(Color color)
        {
            BackColor = customeBackColor = color;
        }
        /// <summary>
        /// Protect registers from overflowing by clamping values
        /// </summary>
        private void clampValue()
        {
            if (registerName == "LK")
            {
                //innerValue &= 127;
                valueWhichShouldBeMovedToRegister &= 127;
            }
            else if (registerName == "RAPS")
            {
                innerValue &= 255;
                valueWhichShouldBeMovedToRegister &= 255;
            }
            else if (registerName == "RAP")
            {
                innerValue &= 255;
                valueWhichShouldBeMovedToRegister &= 255;
            }
            else if (registerName == "LR")
            {
                innerValue &= 255;
                valueWhichShouldBeMovedToRegister &= 255;
            }
        }
    }
}
