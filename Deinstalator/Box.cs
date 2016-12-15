using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deinstalator
{
    public partial class Box : Form
    {
        public Box()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK", true);
            }
            catch { }
            finally
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
