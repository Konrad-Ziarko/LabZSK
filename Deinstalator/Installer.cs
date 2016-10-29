using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Deinstalator
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
            Form frm = new Box();
            frm.ShowDialog();
            frm.BringToFront();
            frm.Focus();
        }
    }
}
