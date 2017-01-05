using LabZSK.Properties;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace LabZSK.Other
{
    public partial class Options : Form
    {
        private bool suppressReload = true;
        private string _environmentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK";
        internal event Action ACallUpdate;
        private bool ignore = false;
        public Options()
        {
            InitializeComponent();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = Settings.Default.Skin;
            ignore = true;
            r1.Value = Settings.Default.R1;
            g1.Value = Settings.Default.G1;
            b1.Value = Settings.Default.B1;

            r2.Value = Settings.Default.R2;
            g2.Value = Settings.Default.G2;
            b3.Value = Settings.Default.B2;

            r3.Value = Settings.Default.R3;
            g3.Value = Settings.Default.G3;
            b3.Value = Settings.Default.B3;
            ignore = false;
            comboBox2.SelectedIndex = Settings.Default.CultureIdx;
            numericUpDown1.Value = Settings.Default.Delay;
            numericUpDown4.Value = Settings.Default.FirstMark;
            numericUpDown3.Value = Settings.Default.SecondMark;
            numericUpDown2.Value = Settings.Default.ThirdMark;
            checkBox2.Checked = Settings.Default.CanCloseLog;
            groupBox1.Enabled = Settings.Default.CanEditOptions;
            checkBox1.Checked = Settings.Default.IsDevConsole;
            setAllStrings();
        }
        internal void setAllStrings()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);

            this.Text = Strings.OptionsTitle;
            label1.Text = Strings.themeLabel;
            label2.Text = Strings.autoModeLatencyLabel;
            label3.Text = Strings.gradingScaleLabel;
            label6.Text = Strings.languageLabel;
            label7.Text = Strings.devConsoleLabel;
            label8.Text = Strings.logClosing;

            button1.Text = Strings.saveConfigButton;
            button2.Text = Strings.loadConfigButton;

            groupBox1.Text = Strings.groupBoxName;

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { Strings.themeComboBox1, Strings.themeComboBox2, Strings.themeComboBox3, Strings.themeComboBox4, Strings.themeComboBox5 });

            suppressReload = true;
            comboBox1.SelectedIndex = Settings.Default.Skin;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Settings.Default.Skin = 0;
                    ignore = true;
                    r1.Value = 27;
                    g1.Value = 96;
                    b1.Value = 51;

                    r2.Value = 133;
                    g2.Value = 198;
                    b2.Value = 72;

                    r3.Value = 249;
                    g3.Value = 66;
                    b3.Value = 239;
                    ignore = false;
                    break;
                case 1:
                    Settings.Default.Skin = 1;
                    ignore = true;
                    r1.Value = 31;
                    g1.Value = 18;
                    b1.Value = 158;

                    r2.Value = 2;
                    g2.Value = 112;
                    b2.Value = 223;

                    r3.Value = 249;
                    g3.Value = 239;
                    b3.Value = 66;
                    ignore = false;
                    break;
                case 2:
                    Settings.Default.Skin = 2;
                    ignore = true;
                    r1.Value = 177;
                    g1.Value = 1;
                    b1.Value = 5;

                    r2.Value = 254;
                    g2.Value = 65;
                    b2.Value = 69;

                    r3.Value = 5;
                    g3.Value = 254;
                    b3.Value = 177;
                    ignore = false;
                    break;
                case 3:
                    Settings.Default.Skin = 3;
                    ignore = true;
                    r1.Value = 5;
                    g1.Value = 5;
                    b1.Value = 5;

                    r2.Value = 120;
                    g2.Value = 120;
                    b2.Value = 120;

                    r3.Value = 239;
                    g3.Value = 254;
                    b3.Value = 245;
                    ignore = false;
                    break;
                case 4:
                    Settings.Default.Skin = 4;
                    ignore = true;
                    r1.Value = Settings.Default.R1;
                    g1.Value = Settings.Default.G1;
                    b1.Value = Settings.Default.B1;

                    r2.Value = Settings.Default.R2;
                    g2.Value = Settings.Default.G2;
                    b2.Value = Settings.Default.B2;

                    r3.Value = Settings.Default.R3;
                    g3.Value = Settings.Default.G3;
                    b3.Value = Settings.Default.B3;
                    ignore = false;
                    break;
            }
            Settings.Default.Save();
            if (!suppressReload)
            ACallUpdate();
            suppressReload = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.IsDevConsole = checkBox1.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.Delay = Convert.ToInt32(numericUpDown1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_File_Dialog = new SaveFileDialog();
            save_File_Dialog.Filter = "To czego szukasz|*.mnie|Wszystko|*.*";
            save_File_Dialog.Title = "Zapisz konfigurację";
            if (Directory.Exists(_environmentPath))
                save_File_Dialog.InitialDirectory = _environmentPath;
            DialogResult saveFileDialogResult = save_File_Dialog.ShowDialog();
            if (saveFileDialogResult == DialogResult.OK && save_File_Dialog.FileName != "")
            {
                string currentAppSettings = Settings.Default.CanEditOptions.ToString();
                currentAppSettings += "<?>" + Settings.Default.CanCloseLog.ToString();
                currentAppSettings += "<?>" + Settings.Default.IsDevConsole.ToString();
                currentAppSettings += "<?>" + Settings.Default.FirstMark;
                currentAppSettings += "<?>" + Settings.Default.SecondMark;
                currentAppSettings += "<?>" + Settings.Default.ThirdMark;

                string encryptedstring = StaticClasses.Encryptor.Encrypt(currentAppSettings);
                try
                {
                    File.WriteAllText(save_File_Dialog.FileName, encryptedstring);
                }
                catch
                {
                    MessageBox.Show("Zapisywanie konfiguracji zakończyło się niepowodzeniem");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //odczyt konfiguracji
            OpenFileDialog open_File_Dialog = new OpenFileDialog();
            open_File_Dialog.Filter = "To czego szukasz|*.mnie|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj konfigurację";
            if (Directory.Exists(_environmentPath + @"\PO\"))
                open_File_Dialog.InitialDirectory = _environmentPath;

            DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
            if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
                try
                {
                    string encryptedString = File.ReadAllText(open_File_Dialog.FileName);
                    string newAppSettings = StaticClasses.Encryptor.Decrypt(encryptedString);

                    string[] tmp = newAppSettings.Split(new[] { "<?>" }, StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length != 6)
                        throw new Exception();
                    if (tmp[0] != "False" && tmp[0] != "True" || tmp[1] != "False" && tmp[1] != "True" || tmp[2] != "False" && tmp[2] != "True")
                        throw new Exception();
                    if (Convert.ToInt32(tmp[3]) < 0 || Convert.ToInt32(tmp[4]) < 0 || Convert.ToInt32(tmp[5]) < 0)
                        throw new Exception();

                    Settings.Default.CanEditOptions = Convert.ToBoolean(tmp[0]);
                    Settings.Default.CanCloseLog = Convert.ToBoolean(tmp[1]);
                    Settings.Default.IsDevConsole = Convert.ToBoolean(tmp[2]);
                    Settings.Default.FirstMark = Convert.ToInt32(tmp[3]);
                    Settings.Default.SecondMark = Convert.ToInt32(tmp[4]);
                    Settings.Default.ThirdMark = Convert.ToInt32(tmp[5]);

                    //comboBox1.SelectedIndex = Settings.Default.SkinNum;
                    comboBox2.SelectedIndex = Settings.Default.CultureIdx;
                    numericUpDown1.Value = Settings.Default.Delay;
                    numericUpDown4.Value = Settings.Default.FirstMark;
                    numericUpDown3.Value = Settings.Default.SecondMark;
                    numericUpDown2.Value = Settings.Default.ThirdMark;
                    checkBox2.Checked = Settings.Default.CanCloseLog;
                    groupBox1.Enabled = Settings.Default.CanEditOptions;
                    checkBox1.Checked = Settings.Default.IsDevConsole;
                }
                catch
                {
                    MessageBox.Show("Wczytywanie konfiguracji zakończyło się niepowodzeniem");
                }
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.FirstMark = Convert.ToInt32(numericUpDown4.Value);
            Settings.Default.SecondMark = Convert.ToInt32(numericUpDown3.Value);
            Settings.Default.ThirdMark = Convert.ToInt32(numericUpDown2.Value);
            Settings.Default.IsDevConsole = checkBox1.Checked;
            Settings.Default.Save();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown4.Value == numericUpDown3.Value)
                numericUpDown3.Value++;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value == numericUpDown2.Value)
                numericUpDown2.Value++;
            if (numericUpDown4.Value == numericUpDown3.Value)
                numericUpDown4.Value--;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown3.Value == numericUpDown2.Value)
            {
                numericUpDown4.Value = numericUpDown2.Value - 2;
                numericUpDown3.Value = numericUpDown2.Value - 1;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                Settings.Default.CultureIdx = 0;
                Settings.Default.Culture = "pl-PL";
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                Settings.Default.Culture = "en-US";
                Settings.Default.CultureIdx = 1;
            }

            setAllStrings();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.CanCloseLog = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.isServerVisible = checkBox3.Checked;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void getSkinData()
        {
            Settings.Default.R1 = Convert.ToInt32(r1.Value);
            Settings.Default.R2 = Convert.ToInt32(r2.Value);
            Settings.Default.B1 = Convert.ToInt32(b1.Value);
            Settings.Default.B2 = Convert.ToInt32(b2.Value);
            Settings.Default.G2 = Convert.ToInt32(g2.Value);
            Settings.Default.R3 = Convert.ToInt32(r3.Value);
            Settings.Default.B3 = Convert.ToInt32(b3.Value);
            Settings.Default.G3 = Convert.ToInt32(g3.Value);
            Settings.Default.G1 = Convert.ToInt32(g1.Value);

            Settings.Default.Skin = 4;
            if (comboBox1.SelectedIndex != 4)
                comboBox1.SelectedIndex = 4;
            if (!suppressReload)
            ACallUpdate();
            suppressReload = false;

        }
        private void r1_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void g1_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void b1_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void r2_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void g2_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void b2_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void r3_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void g3_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
        private void b3_ValueChanged(object sender, EventArgs e)
        {
            if (!ignore)
                getSkinData();
        }
    }
}
