using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class ErrorMessage : Form
    {
        public ErrorMessage()
        {
            InitializeComponent();

        }

        private void ErrorMessage_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            BackgroundImageLayout = ImageLayout.Stretch;
            Cursor.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
