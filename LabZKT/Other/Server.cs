using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LabZSK.Other
{
    public partial class Server : Form
    {
        internal string name, lastName, group, ipAddress, remotePort, password;

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            name = textBox_Name.Text;
            lastName = textBox_LastName.Text;
            group = textBox_Group.Text;
            ipAddress = textBox_IP.Text;
            remotePort = textBox_Port.Text;
            password = textBox_Pass.Text;
        }

        public Server(bool isConnected=false)
        {
            InitializeComponent();
            button1.Enabled = !isConnected;
        }

    }
}
