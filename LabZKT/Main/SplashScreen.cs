using System;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class SplashScreen : Form
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
            Opacity -= 0.05;
            if (Opacity == 0)
                Close();
        }
    }
}
