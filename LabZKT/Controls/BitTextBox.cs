using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabZSK.Controls
{
    public class BitTextBox : TextBox
    {
        public string flagName { get; set; }
        public short innerValue { get; private set; }
        public BitTextBox(int x, int y, Control c, string name)
        {
            flagName = name;
            if (name == "MAV")
            {
                innerValue = 1;
                Text = "1";
            }
            else
            {
                innerValue = 0;
                Text = "0";
            }
                
            ReadOnly = false;
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

        public void SetXY(int x, int y)
        {
            var loc = Location;
            loc.X = x;
            loc.Y = y;
            Location = loc;
        }
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
        public void setInnerValue(short newVal)
        {
            innerValue = newVal;
            Text = innerValue.ToString();
        }
    }
}
