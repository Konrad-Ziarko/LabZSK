using LabZSK.Controls;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LabZSK.Simulation
{
    /// <summary>
    /// Class used to draw background circuit between registers
    /// </summary>
    public class Drawings
    {
        private Dictionary<string, NumericTextBox> registers;
        private Dictionary<string, BitTextBox> flags;
        private Control controlToDrawOn;
        private TextBox RBPS;
        /// <summary>
        /// Initialize instance of drawing class
        /// </summary>
        /// <param name="regs">Dictionary with CPU registers</param>
        /// <param name="flgs">Dictionary with CPU flags</param>
        /// <param name="rbps">TextBox representing RBPS register</param>
        public Drawings(ref Dictionary<string, NumericTextBox> regs, ref Dictionary<string, BitTextBox> flgs, ref TextBox rbps)
        {
            registers = regs;
            flags = flgs;
            RBPS = rbps;
        }
        internal void addControlToDrawOn(Control c)
        {
            controlToDrawOn = c;
        }
        /// <summary>
        /// Draw background for registers in passed control object
        /// </summary>
        public void drawBackground()
        {
            Bitmap skin;
            switch (Properties.Settings.Default.SkinNum)
            {
                case 0:
                    skin = drawSkin(Color.FromArgb(255, 27, 96, 51), Color.FromArgb(255, 133, 198, 72), Color.FromArgb(255, 249, 66, 239));
                    break;
                case 1:
                    skin = drawSkin(Color.FromArgb(255, 31, 18, 158), Color.FromArgb(255, 2, 112, 223), Color.FromArgb(255, 249, 239, 66));
                    break;
                case 2:
                    skin = drawSkin(Color.FromArgb(255, 177, 1, 5), Color.FromArgb(255, 254, 65, 69), Color.FromArgb(255, 5, 254, 177));
                    break;
                case 3:
                    skin = drawSkin(Color.FromArgb(255, 5, 5, 5), Color.FromArgb(255, 120, 120, 120), Color.FromArgb(255, 249, 239, 245));
                    break;
                default:
                    skin = drawSkin(Color.FromArgb(255, 27, 96, 51), Color.FromArgb(255, 133, 198, 72), Color.FromArgb(255, 249, 66, 239));
                    break;
            }
            controlToDrawOn.BackgroundImage = skin;
        }

        private Bitmap drawSkin(Color backgroundColor, Color busColor, Color textColor)
        {
            Bitmap background = new Bitmap(controlToDrawOn.Width, controlToDrawOn.Height);
            Graphics g = Graphics.FromImage(background);
            g.Clear(backgroundColor);
            //NumericTextBox ntb;
            //if (registers.TryGetValue("BUS", out ntb))
                //ntb.setCustomeColor(busColor, Color.FromArgb(busColor.ToArgb() ^ 0xffffff));

            GraphicsPath path = new GraphicsPath();
            Pen pen = new Pen(busColor, 10);
            Pen pen2 = new Pen(busColor, 5);
            Pen pen3 = new Pen(busColor, 80);
            pen.LineJoin = LineJoin.Bevel;
            Point p1 = registers["BUS"].Location;
            Point p2 = new Point();
            p1.X += registers["BUS"].Size.Width / 2;
            p2.Y = p1.Y += registers["BUS"].Size.Height / 2;
            p2.X = controlToDrawOn.Width;
            g.DrawLine(pen, p1, p2);
            //
            drawJoint(ref p1, ref p2, "RR");
            g.DrawLine(pen, p2, p1);
            //
            drawJoint(ref p1, ref p2, "LR");
            g.DrawLine(pen, p2, p1);
            //
            drawJoint(ref p1, ref p2, "RI");
            g.DrawLine(pen, p2, p1);
            //
            p2.X = p1.X = controlToDrawOn.Location.X + (registers["BUS"].Location.X / 2);
            p2.Y = registers["LK"].Location.Y * 3 / 4 + 5;
            p1.Y = controlToDrawOn.Size.Height;
            //
            path.StartFigure();
            path.AddLine(p1, p2);
            //
            p2.X = registers["LK"].Location.X * 3 / 4 + 5;
            p1.Y = p2.Y = (int)pen.Width;
            p1.X = controlToDrawOn.Size.Width;
            //
            path.AddLine(p2, p1);
            g.DrawPath(pen, path);
            //
            drawJoint(ref p1, ref p2, "LK");
            g.DrawLine(pen, p2, p1);
            //
            drawJoint(ref p1, ref p2, "A");
            g.DrawLine(pen, p2, p1);
            //
            drawJoint(ref p1, ref p2, "MQ");
            g.DrawLine(pen, p2, p1);
            //
            drawJoint(ref p1, ref p2, "X");
            g.DrawLine(pen, p2, p1);
            //
            p1.X = registers["RAP"].Location.X + 10;
            p1.Y = registers["RAP"].Location.Y + registers["RAP"].Size.Height / 2;
            p2.X = registers["RAP"].Location.X / 2;
            p2.Y = p1.Y;
            g.DrawLine(pen, p2, p1);
            //
            p1.X = registers["RBP"].Location.X + 10;
            p1.Y = registers["RBP"].Location.Y + registers["RBP"].Size.Height / 2;
            p2.X = registers["RBP"].Location.X / 2;
            p2.Y = p1.Y;
            g.DrawLine(pen, p2, p1);
            //
            p1.X = registers["BUS"].Location.X + 10;
            p1.Y = registers["BUS"].Location.Y + registers["BUS"].Size.Height / 2;
            p2.X = registers["BUS"].Location.X / 2;
            p2.Y = p1.Y;
            g.DrawLine(pen, p2, p1);
            //
            p1.X = registers["A"].Location.X + registers["A"].Size.Width / 2;
            p1.Y = registers["A"].Location.Y + registers["A"].Size.Height / 2;
            p2.X = registers["LALU"].Location.X + registers["A"].Size.Width / 2;
            p2.Y = registers["LALU"].Location.Y + registers["LALU"].Size.Height / 2;
            g.DrawLine(pen, p2, p1);
            //
            p1.X = registers["ALU"].Location.X + registers["ALU"].Size.Width / 2;
            p1.Y = registers["ALU"].Location.Y + registers["ALU"].Size.Height / 2;
            //ALU interior
            SolidBrush aluBrush = new SolidBrush(busColor);
            g.FillPolygon(aluBrush, new Point[]
            {
                new Point(registers["LALU"].Location.X, registers["LALU"].Location.Y + registers["LALU"].Size.Height),
                new Point(registers["LALU"].Location.X + registers["LALU"].Size.Width, registers["LALU"].Location.Y + registers["LALU"].Size.Height),
                new Point(registers["ALU"].Location.X + registers["ALU"].Size.Width, registers["ALU"].Location.Y),
                new Point(registers["ALU"].Location.X, registers["ALU"].Location.Y)
            });
            g.FillPolygon(aluBrush, new Point[]
            {
                new Point(registers["RALU"].Location.X, registers["RALU"].Location.Y + registers["RALU"].Size.Height),
                new Point(registers["RALU"].Location.X + registers["RALU"].Size.Width, registers["RALU"].Location.Y + registers["RALU"].Size.Height),
                new Point(registers["ALU"].Location.X + registers["ALU"].Size.Width, registers["ALU"].Location.Y),
                new Point(registers["ALU"].Location.X, registers["ALU"].Location.Y)
            });
            //
            p2.X = p1.X;
            p2.Y = registers["BUS"].Location.Y + registers["BUS"].Size.Height / 2;
            g.DrawLine(pen, p2, p1);
            //
            p1.X = registers["A"].Location.X + registers["A"].Size.Width / 2;
            p1.Y = registers["A"].Location.Y + registers["A"].Size.Height / 2;
            p2.X = registers["MQ"].Location.X + registers["MQ"].Size.Width / 2;
            p2.Y = registers["MQ"].Location.Y + registers["MQ"].Size.Height / 2;
            g.DrawLine(pen2, p2, p1);
            //
            p1.X = registers["RALU"].Location.X + registers["RALU"].Size.Width / 2;
            p1.Y = registers["RALU"].Location.Y + registers["RALU"].Size.Height / 2;
            p2.Y = p1.Y - (registers["RALU"].Location.Y - registers["X"].Location.Y) / 2;
            p2.X = p1.X + (registers["RALU"].Location.Y - registers["X"].Location.Y) / 2;
            path.Reset();
            path.StartFigure();
            path.AddLine(p1, new Point(p1.X, p2.Y));
            path.AddLine(new Point(p1.X, p2.Y), p2);
            //
            p1.X = registers["X"].Location.X + registers["X"].Size.Width / 2;
            p1.Y = p2.Y;
            Point p3 = p1;
            p2.X = registers["X"].Location.X + registers["X"].Size.Width / 2;
            p2.Y = registers["X"].Location.Y + registers["X"].Size.Height / 2;
            path.AddLine(p1, p2);
            g.DrawPath(pen, path);
            //
            path.Reset();
            path.StartFigure();
            p2.Y = p1.Y;
            p1.X = p2.X = controlToDrawOn.Width;
            path.AddLine(p2, calculateTriangle(p2, p1, false));
            path.AddLine(p2, p3);
            g.DrawPath(pen, path);
            //
            controlToDrawOn.Visible = true;
            if (registers["SUMA"].Visible)
            {
                path.Reset();
                path.StartFigure();
                p2 = registers["RR"].Location;
                p2.X += registers["RR"].Size.Width / 2;
                p2.Y += registers["RR"].Size.Height / 2;

                p1 = registers["L"].Location;
                p1.X = p2.X;
                p1.Y += registers["L"].Size.Height / 2;
                path.AddLine(p2, p1);

                p2.Y = p1.Y;
                p2.X = registers["L"].Location.X + registers["L"].Size.Height / 2;
                path.AddLine(p1, p2);
                g.DrawPath(pen, path);
                
                p2 = registers["SUMA"].Location;
                p2.X += registers["SUMA"].Size.Width / 2;
                p2.Y += registers["SUMA"].Size.Height / 2;

                p1.X = registers["R"].Location.X + registers["R"].Size.Width / 2;
                p1.Y = registers["R"].Location.Y + registers["R"].Size.Height / 2;

                path.Reset();
                path.StartFigure();
                p2 = registers["RI"].Location;
                p2.X += registers["RI"].Size.Width / 2;
                p2.Y += registers["RI"].Size.Height / 2;

                p3 = p2;
                p3.Y = p1.Y;
                path.AddLine(p2, p3);
                path.AddLine(p3, p1);
                g.DrawPath(pen, path);
                path.Reset();
                path.StartFigure();

                p2 = registers["LR"].Location;
                p2.X += registers["LR"].Size.Width / 2;
                p2.Y += registers["LR"].Size.Height / 2;
                p3 = p2;
                p3.Y = p1.Y;
                path.AddLine(p2, p3);
                path.AddLine(p3, p1);
                g.DrawPath(pen, path);
                path.Reset();
                path.StartFigure();
                //
                SolidBrush sumBrush = new SolidBrush(busColor);
                g.FillPolygon(aluBrush, new Point[]
                {
                    new Point(registers["L"].Location.X, registers["L"].Location.Y + registers["L"].Size.Height),
                    new Point(registers["L"].Location.X + registers["L"].Size.Width, registers["L"].Location.Y + registers["L"].Size.Height),
                    new Point(registers["SUMA"].Location.X + registers["SUMA"].Size.Width, registers["SUMA"].Location.Y),
                    new Point(registers["SUMA"].Location.X, registers["SUMA"].Location.Y)
                });
                g.FillPolygon(aluBrush, new Point[]
                {
                    new Point(registers["R"].Location.X, registers["R"].Location.Y + registers["R"].Size.Height),
                    new Point(registers["R"].Location.X + registers["R"].Size.Width, registers["R"].Location.Y + registers["R"].Size.Height),
                    new Point(registers["SUMA"].Location.X + registers["SUMA"].Size.Width, registers["SUMA"].Location.Y),
                    new Point(registers["SUMA"].Location.X, registers["SUMA"].Location.Y)
                });
                //
                p2 = registers["SUMA"].Location;
                p1.X = p2.X += registers["SUMA"].Size.Width / 2;
                p2.Y += registers["SUMA"].Size.Height / 2;
                p3.Y = p1.Y = controlToDrawOn.Size.Height - (int)pen.Width;
                path.AddLine(p2, p1);
                p3.X = controlToDrawOn.Size.Width;
                path.AddLine(p1, p3);
                g.DrawPath(pen, path);
            }
            else
            {
                path.Reset();
                path.StartFigure();
                p1 = registers["RAE"].Location;
                p1.X += registers["RAE"].Size.Width / 2;
                p1.Y += registers["RAE"].Size.Height / 2;
                p2.Y = p1.Y + (controlToDrawOn.Height - registers["RAE"].Location.Y) / 2;
                p2.X = p1.X;
                path.AddLine(p1, p2);

                p1.X = controlToDrawOn.Location.X + (registers["BUS"].Location.X / 2);
                p1.Y = p2.Y;
                path.AddLine(p2, p1);
                g.DrawPath(pen, path);
            }
            Font fnt = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 238);
            SolidBrush sb = new SolidBrush(textColor);
            foreach (var flg in flags.Values)
                g.DrawString(flg.flagName, fnt, sb, flg.Location.X + flg.Width/2 - g.MeasureString(flg.flagName, fnt).Width/2, flg.Location.Y - 18);
            foreach (var reg in registers.Values)
                if (reg.Visible)
                    if (reg.registerName == "L" || reg.registerName == "R" || reg.registerName == "ALU" || reg.registerName == "SUMA")
                        g.DrawString(reg.registerName, fnt, sb, reg.Location.X + reg.Width / 2 - g.MeasureString(reg.registerName, fnt).Width / 2, reg.Location.Y - 18);
                    else
                        g.DrawString(reg.registerName, fnt, sb, reg.Location.X, reg.Location.Y - 18);
            if (!registers["SUMA"].Visible)
                g.DrawString("RBPS", fnt, sb, RBPS.Location.X, RBPS.Location.Y - 18);

            return background;
        }
        private Point calculateTriangle(Point bottom, Point top, bool leftSide)
        {
            if (leftSide)
                return new Point(top.X - (bottom.Y - top.Y), top.Y);
            else
                return new Point(top.X + (bottom.Y - top.Y), top.Y);
        }
        private void drawJoint(ref Point p1, ref Point p2, string v)
        {
            p1.X = p2.X = registers[v].Location.X + registers[v].Size.Width / 2;
            p2.Y = registers[v].Location.Y + registers[v].Size.Height / 2;
        }
    }
}
