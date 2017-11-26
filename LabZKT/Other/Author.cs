using LabZSK.Properties;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace LabZSK.Other {
    /// <summary>
    /// Windows form for credentials
    /// </summary>
    public partial class Author : Form {
        /// <summary>
        /// Initialize instance of this form
        /// </summary>
        public Author() {
            InitializeComponent();
            setAllStrings();
        }
        internal void setAllStrings() {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);

            this.Text = Strings.AuthorTitle;
            label1.Text = Strings.instituteName;
            label2.Text = Strings.authorName;
            label3.Text = Strings.promoterName;
            label4.Text = Strings.theOtherGuy;

        }
        private void timer1_Tick(object sender, EventArgs e) {

        }

        private void panel2_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }

        private void Author_Load(object sender, EventArgs e) {
        }
    }
}
