using System;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK.Other
{
    /// <summary>
    /// Windows form for errors
    /// </summary>
    public partial class ErrorMessage : Form
    {
        /// <summary>
        /// Initialize instance of this class
        /// </summary>
        public ErrorMessage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Occures when form was loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorMessage_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;
            BackgroundImageLayout = ImageLayout.Stretch;
            Cursor.Hide();
            SoundPlayer audio = new SoundPlayer(LabZSK.Properties.Resources.BSoD);
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
