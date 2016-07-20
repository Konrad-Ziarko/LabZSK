﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT
{
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
            Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
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

            RunSim.hitTest = new Size(e.X, e.Y);
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