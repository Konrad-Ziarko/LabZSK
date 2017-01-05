using LabZSK.Simulation;
using System;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK
{
    partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Opacity <= 0.05)
            {
                Close();
            }
            else
            Opacity -= 0.05;
        }
    }
}
