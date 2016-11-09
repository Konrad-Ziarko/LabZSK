using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LabZSK.Controls
{
    public class NumericTextBox : TextBox
    {
        public string registerName { get; private set; }
        public bool needCheck { get; set; }
        public short innerValue { get; private set; }
        public short valueWhichShouldBeMovedToRegister { get; private set; }
        private static short dragValue;
        private static Point hitTest;
        private Color customeBackColor, customeForeColor;

        public NumericTextBox(string name, int x, int y, Control c)
        {
            customeBackColor = SystemColors.Window;
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
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
        }
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
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            Text = "";
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            OnGotFocus(e);
        }
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
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (!Enabled)
            BackColor = customeBackColor;
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.Show(innerValue.ToString("X") + "h\t" + innerValue, this, 0, 30, 1000);
        }

        #region Drag&Drop
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            hitTest = new Point(e.X, e.Y);
            OnGotFocus(e);
        }
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
                innerValue = dragValue;
                setText();
            }
            catch (Exception)
            {
                MessageBox.Show(Strings.dragError, "LabZSK", MessageBoxButtons.OK);
            }
        }
        #endregion

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
            valueWhichShouldBeMovedToRegister = 0;
            setText();
        }
        public void setInnerValue(short newVal)
        {
            innerValue = newVal;
            setText();
        }
        public void setActualValue(short newVal)
        {
            valueWhichShouldBeMovedToRegister = newVal;
            clampValue();
        }
        public void setNeedCheck(out string name)
        {
            needCheck = true;
            ReadOnly = false;
            BackColor = Color.Yellow;
            name = registerName;
        }
        public bool validateRegisterValue(out short badValue)
        {
            needCheck = false;
            ReadOnly = true;
            BackColor = customeBackColor;
            ForeColor = customeForeColor;
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
        private void setText()
        {
            clampValue();
            Text = innerValue.ToString("X") + "h\t" + innerValue;
        }
        public void setInnerAndExpectedValue(short newVal)
        {
            innerValue = valueWhichShouldBeMovedToRegister = newVal;
            needCheck = false;
            setText();
        }
        public void setCustomeColor(Color colorBack, Color colorFore)
        {
            BackColor = customeBackColor = colorBack;
            this.ForeColor = customeForeColor = colorFore;
        }
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
