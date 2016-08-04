using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabZKT
{
    /// <summary>
    /// Class used as CPU flag
    /// </summary>
    public class BitTextBox : TextBox
    {
        private short innerValue = 0;
        public string flagName { get; private set; }
        public BitTextBox(Size s)
        {
            Height = s.Height;
            Width = s.Width;
        }
        public BitTextBox(int x, int y, Control c, string name)
        {
            flagName = name;
            if (name == "MAV")
            {
                innerValue = 1;
                Text = "1";
            }
            else
                Text = "0";
            ReadOnly = false;
            AllowDrop = true;
            Size = new Size(20, 20);
            Parent = c;
            var loc = Location;
            loc.X = x;
            loc.Y = y;
            Location = loc;
            Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == (char)Keys.Enter)
            {
                Parent.Focus();
            }

            if (e.KeyChar == '0' || e.KeyChar == '1' || e.KeyChar==(char)Keys.Back)
            {
                Text = "";
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (Text!="")
                try
                {
                    short tmp = Convert.ToInt16(Text);
                    if (tmp==1 || tmp == 0)
                    {
                        innerValue = tmp;
                    }
                }
                catch (Exception)
                {

                }
            
            Text = innerValue.ToString();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            Text = "";
        }

        /// Drag & Drop on dataGridView (copy insted of move)
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            SimView.hitTest = new Size(e.X, e.Y);
        }
        /// <summary>
        /// Set current position of this control
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        public void SetXY(int x, int y)
        {
            var loc = Location;
            loc.X = x;
            loc.Y = y;
            setLocation(loc);
        }
        delegate void SetCallback(Point loc);
        private void setLocation(Point loc)
        {
            if (InvokeRequired)
            {
                SetCallback d = new SetCallback(setLocation);
                this.Invoke(d, new object[] { loc });
            }
            else
            {
                Location = loc;
            }
        }
        /// <summary>
        /// Reset flag inner value
        /// </summary>
        public void resetValue()
        {
            if (flagName != "MAV")
            {
                innerValue = 0;
                Text = "0";
            }
            else
            {
                innerValue = 1;
                Text = "1";
            }
        }
        public short getInnerValue()
        {
            return innerValue;
        }
        public void setInnerValue(short newVal)
        {
            innerValue = newVal;
            Text = innerValue.ToString();
        }
    }
}
