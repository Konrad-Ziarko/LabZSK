using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabZSK.Controls
{
    /// <summary>
    /// Class which represents CPU flag
    /// </summary>
    public class BitTextBox : TextBox
    {
        
        /// <summary>
        /// String representing CPU flag name
        /// </summary>
        public string flagName { get; set; }
        /// <summary>
        /// Value stored in flag
        /// </summary>
        public short innerValue { get; private set; }
        /// <summary>
        /// Initialize new instance of BitTextBox
        /// </summary>
        /// <param name="x">Position in X coordinate</param>
        /// <param name="y">Position in Y coordinate</param>
        /// <param name="c">Parent control for this instance</param>
        /// <param name="name">String representing name of flag</param>
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

            if (e.KeyChar == '0' || e.KeyChar == '1' || e.KeyChar==(char)Keys.Back)
            {
                Text = "";
            }
            else
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// Occurs when control loses focus
        /// </summary>
        /// <param name="e"></param>
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
        /// <summary>
        /// Occurs when control got focus
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            Text = "";
        }

        /// <summary>
        /// Set new position for this control
        /// </summary>
        /// <param name="x">new X position</param>
        /// <param name="y">new Y position</param>
        public void SetXY(int x, int y)
        {
            var loc = Location;
            loc.X = x;
            loc.Y = y;
            Location = loc;
        }
        /// <summary>
        /// Reset flag inner value to default
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
        /// <summary>
        /// Set flag inner value and control text
        /// </summary>
        /// <param name="newVal">Value for control to store</param>
        public void setInnerValue(short newVal)
        {
            innerValue = newVal;
            Text = innerValue.ToString();
        }
    }
}
