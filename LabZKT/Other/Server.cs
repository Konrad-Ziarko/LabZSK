using LabZSK.Properties;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK.Other
{
    public partial class Server : Form
    {
        internal static string name="", lastName = "", group = "", ipAddress = "", remotePort = "", password = "";

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_IP_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Regex regExp = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");
            if (!regExp.IsMatch(textBox_IP.Text))
            {
                textBox_IP.BackColor = System.Drawing.Color.Red;
            }
            else
                textBox_IP.BackColor = System.Drawing.SystemColors.Window;
        }

        private void textBox_Port_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (Convert.ToUInt16(textBox_Port.Text) < 1024)
                    throw new Exception();
                textBox_Port.BackColor = System.Drawing.SystemColors.Window;
            }
            catch {
                textBox_Port.BackColor = System.Drawing.Color.Red;
            }
        }

        internal bool connect = false;
        public Server(bool isConnected = false)
        {
            InitializeComponent();
            setAllStrings();
            button1.Enabled = !isConnected;
            AcceptButton = button1;
            CancelButton = button2;

            textBox_Name.Text = name;
            textBox_LastName.Text = lastName;
            textBox_Group.Text = group;
            textBox_Port.Text = remotePort;
            textBox_IP.Text = ipAddress;
            textBox_Pass.Text = password;
        }
        internal void setAllStrings()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);

            button1.Text = Strings.connectButton;
            button2.Text = Strings.cancelButton;
            label1.Text = Strings.name;
            label2.Text = Strings.lastName;
            label3.Text = Strings.group;
            label6.Text = Strings.ipAddress;
            label5.Text = Strings.port;
            label4.Text = Strings.password;


            this.Text = Strings.serverTitle;
        }
        private void Server_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool isValid = true;
            //
            if (!isValid)
                e.Cancel = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox_Name.Text;
            lastName = textBox_LastName.Text;
            group = textBox_Group.Text;
            ipAddress = textBox_IP.Text;
            remotePort = textBox_Port.Text;
            password = textBox_Pass.Text;
            connect = true;
            Close();
        }

        

        

    }
}
