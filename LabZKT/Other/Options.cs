﻿using LabZSK.Properties;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace LabZSK.Other {
    public partial class Options : Form {
        private bool suppressReload = true;
        private string _environmentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LabZSK";
        internal event Action ACallUpdate;
        public Options() {
            InitializeComponent();
        }

        private void loadGUI() {
            //ukryj pola do zmiany koloru RAPS i RBPS
            label16.Visible = txt5.Visible = false;
            //
            comboBox1.SelectedIndex = Settings.Default.Skin;
            label5.ForeColor = System.Drawing.Color.Blue;
            txt1.Text = "#" + Settings.Default.R1.ToString("X").PadLeft(2, '0') + Settings.Default.G1.ToString("X").PadLeft(2, '0') + Settings.Default.B1.ToString("X").PadLeft(2, '0');
            txt2.Text = "#" + Settings.Default.R2.ToString("X").PadLeft(2, '0') + Settings.Default.G2.ToString("X").PadLeft(2, '0') + Settings.Default.B2.ToString("X").PadLeft(2, '0');
            txt3.Text = "#" + Settings.Default.R3.ToString("X").PadLeft(2, '0') + Settings.Default.G3.ToString("X").PadLeft(2, '0') + Settings.Default.B3.ToString("X").PadLeft(2, '0');

            txt4.Text = "#" + Settings.Default.RPS.ToString("X").PadLeft(2, '0') + Settings.Default.GPS.ToString("X").PadLeft(2, '0') + Settings.Default.BPS.ToString("X").PadLeft(2, '0');
            txt5.Text = "#" + Settings.Default.RPAO.ToString("X").PadLeft(2, '0') + Settings.Default.GPAO.ToString("X").PadLeft(2, '0') + Settings.Default.BPAO.ToString("X").PadLeft(2, '0');

            comboBox2.SelectedIndex = Settings.Default.CultureIdx;
            numericUpDown1.Value = Settings.Default.Delay;
            numericUpDown4.Value = Settings.Default.FirstMark;
            numericUpDown3.Value = Settings.Default.SecondMark;
            numericUpDown2.Value = Settings.Default.ThirdMark;
            checkBox2.Checked = Settings.Default.CanCloseLog;
            groupBox1.Enabled = Settings.Default.CanEditOptions;
            checkBox1.Checked = Settings.Default.IsDevConsole;
            checkBox4.Checked = Settings.Default.CanEditOptions;
            checkBox3.Checked = Settings.Default.isServerVisible;
            setAllStrings();
        }

        private void Options_Load(object sender, EventArgs e) {
            loadGUI();
        }
        internal void setAllStrings() {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(Settings.Default.Culture);

            this.Text = Strings.OptionsTitle;
            label1.Text = Strings.themeLabel;
            label2.Text = Strings.autoModeLatencyLabel;
            label3.Text = Strings.gradingScaleLabel;
            label6.Text = Strings.languageLabel;
            label7.Text = Strings.devConsoleLabel;
            label8.Text = Strings.logClosing;

            label9.Text = Strings.server;
            label10.Text = Strings.background;
            label11.Text = Strings.bus;
            label12.Text = Strings.names;

            button1.Text = Strings.saveConfigButton;
            button2.Text = Strings.loadConfigButton;

            groupBox1.Text = Strings.groupBoxName;


            if (checkBox4.Checked) {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = Strings.adminMode;
            }
            else {
                label5.ForeColor = System.Drawing.Color.Blue;
                label5.Text = Strings.studentMode;
            }

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new object[] { Strings.themeComboBox1, Strings.themeComboBox2, Strings.themeComboBox3, Strings.themeComboBox4, Strings.themeComboBox5 });

            suppressReload = true;
            comboBox1.SelectedIndex = Settings.Default.Skin;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            switch (comboBox1.SelectedIndex) {
                case 0:
                    Settings.Default.Skin = 0;
                    txt1.Text = "#1B6033";
                    txt2.Text = "#85C648";
                    txt3.Text = "#F942EF";
                    break;
                case 1:
                    Settings.Default.Skin = 1;
                    txt1.Text = "#1F129E";
                    txt2.Text = "#0270DF";
                    txt3.Text = "#F9EF42";
                    break;
                case 2:
                    Settings.Default.Skin = 2;
                    txt1.Text = "#B10105";
                    txt2.Text = "#FE4145";
                    txt3.Text = "#05FEB1";
                    break;
                case 3:
                    Settings.Default.Skin = 3;
                    txt1.Text = "#050505";
                    txt2.Text = "#787878";
                    txt3.Text = "#EFFEF5";
                    break;
                case 4:
                    Settings.Default.Skin = 4;
                    txt1.Text = "#" + Settings.Default.R1.ToString("X").PadLeft(2, '0') + Settings.Default.G1.ToString("X").PadLeft(2, '0') + Settings.Default.B1.ToString("X").PadLeft(2, '0');
                    txt2.Text = "#" + Settings.Default.R2.ToString("X").PadLeft(2, '0') + Settings.Default.G2.ToString("X").PadLeft(2, '0') + Settings.Default.B2.ToString("X").PadLeft(2, '0');
                    txt3.Text = "#" + Settings.Default.R3.ToString("X").PadLeft(2, '0') + Settings.Default.G3.ToString("X").PadLeft(2, '0') + Settings.Default.B3.ToString("X").PadLeft(2, '0');

                    break;
            }
            t1chk();
            t2chk();
            t3chk();
            if (!suppressReload)
                ACallUpdate();
            suppressReload = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            Settings.Default.IsDevConsole = checkBox1.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            Settings.Default.Delay = Convert.ToInt32(numericUpDown1.Value);
        }

        private void button1_Click(object sender, EventArgs e) {
            SaveFileDialog save_File_Dialog = new SaveFileDialog();
            save_File_Dialog.Filter = "Plik konfiguracyjny|*.cfg|Wszystko|*.*";
            save_File_Dialog.Title = "Zapisz konfigurację";
            if (Directory.Exists(_environmentPath))
                save_File_Dialog.InitialDirectory = _environmentPath;
            DialogResult saveFileDialogResult = save_File_Dialog.ShowDialog();
            if (saveFileDialogResult == DialogResult.OK && save_File_Dialog.FileName != "") {
                string currentAppSettings = Settings.Default.CanEditOptions.ToString();
                currentAppSettings += "<?>" + Settings.Default.CanCloseLog.ToString();
                currentAppSettings += "<?>" + Settings.Default.IsDevConsole.ToString();
                currentAppSettings += "<?>" + Settings.Default.FirstMark;
                currentAppSettings += "<?>" + Settings.Default.SecondMark;
                currentAppSettings += "<?>" + Settings.Default.ThirdMark;

                string encryptedstring = StaticClasses.Encryptor.Encrypt(currentAppSettings);
                try {
                    File.WriteAllText(save_File_Dialog.FileName, encryptedstring);
                }
                catch {
                    MessageBox.Show("Zapisywanie konfiguracji zakończyło się niepowodzeniem");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            //odczyt konfiguracji
            OpenFileDialog open_File_Dialog = new OpenFileDialog();
            open_File_Dialog.Filter = "Plik konfiguracyjny|*.cfg|Wszystko|*.*";
            open_File_Dialog.Title = "Wczytaj konfigurację";
            if (Directory.Exists(_environmentPath + @"\PO\"))
                open_File_Dialog.InitialDirectory = _environmentPath;

            DialogResult openFileDialogResult = open_File_Dialog.ShowDialog();
            if (openFileDialogResult == DialogResult.OK && open_File_Dialog.FileName != "")
                try {
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
                    checkBox4.Checked = Settings.Default.CanEditOptions;
                }
                catch {
                    MessageBox.Show("Wczytywanie konfiguracji zakończyło się niepowodzeniem");
                }
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e) {
            Settings.Default.FirstMark = Convert.ToInt32(numericUpDown4.Value);
            Settings.Default.SecondMark = Convert.ToInt32(numericUpDown3.Value);
            Settings.Default.ThirdMark = Convert.ToInt32(numericUpDown2.Value);
            Settings.Default.IsDevConsole = checkBox1.Checked;
            Settings.Default.Save();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e) {
            if (numericUpDown4.Value == numericUpDown3.Value)
                numericUpDown3.Value++;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e) {
            if (numericUpDown3.Value == numericUpDown2.Value)
                numericUpDown2.Value++;
            if (numericUpDown4.Value == numericUpDown3.Value)
                numericUpDown4.Value--;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e) {
            if (numericUpDown3.Value == numericUpDown2.Value) {
                numericUpDown4.Value = numericUpDown2.Value - 2;
                numericUpDown3.Value = numericUpDown2.Value - 1;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBox2.SelectedIndex == 0) {
                Settings.Default.CultureIdx = 0;
                Settings.Default.Culture = "pl-PL";
            }
            else if (comboBox2.SelectedIndex == 1) {
                Settings.Default.Culture = "en-US";
                Settings.Default.CultureIdx = 1;
            }

            setAllStrings();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            Settings.Default.CanCloseLog = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            Settings.Default.isServerVisible = checkBox3.Checked;
        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void label4_Click(object sender, EventArgs e) {

        }

        private void label9_Click(object sender, EventArgs e) {

        }

        private void getSkinData() {
            Settings.Default.Skin = 4;
            Settings.Default.Save();
            if (comboBox1.SelectedIndex != 4)
                comboBox1.SelectedIndex = 4;
            if (!suppressReload)
                ACallUpdate();
            suppressReload = false;

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            Settings.Default.CanEditOptions = checkBox4.Checked;
            if (checkBox4.Checked) {
                label5.ForeColor = System.Drawing.Color.Red;
                label5.Text = Strings.adminMode;
            }
            else {
                label5.ForeColor = System.Drawing.Color.Blue;
                label5.Text = Strings.studentMode;
            }
        }

        public string HexToColor(string hexString, ref int r, ref int g, ref int b, out bool valid) {
            valid = true;
            int cr = r, cg = g, cb = b;
            if ((hexString.StartsWith("#")) && (hexString.Length == 7)) {
                try {
                    cr = Convert.ToInt32(hexString.Substring(1, 2), 16);
                    try {
                        cg = Convert.ToInt32(hexString.Substring(3, 2), 16);
                        try {
                            cb = Convert.ToInt32(hexString.Substring(5, 2), 16);
                        }
                        catch { valid = false; }
                    }
                    catch { valid = false; }
                }
                catch { valid = false; }
            }
            else if (hexString.Length == 6) {
                try {
                    cr = Convert.ToInt32(hexString.Substring(0, 2), 16);
                    try {
                        cg = Convert.ToInt32(hexString.Substring(2, 2), 16);
                        try {
                            cb = Convert.ToInt32(hexString.Substring(4, 2), 16);
                        }
                        catch { valid = false; }
                    }
                    catch { valid = false; }
                }
                catch { valid = false; }
            }
            if (valid) {
                r = cr;
                g = cg;
                b = cb;
            }
            return "#" + r.ToString("X").PadLeft(2, '0') + g.ToString("X").PadLeft(2, '0') + b.ToString("X").PadLeft(2, '0');
        }

        private void t1chk() {
            int r = Settings.Default.R1, g = Settings.Default.G1, b = Settings.Default.B1;
            bool valid;
            txt1.Text = HexToColor(txt1.Text, ref r, ref g, ref b, out valid);
            if (valid) {
                Settings.Default.R1 = r;
                Settings.Default.G1 = g;
                Settings.Default.B1 = b;
            }
        }
        private void t2chk() {
            int r = Settings.Default.R2, g = Settings.Default.G2, b = Settings.Default.B2;
            bool valid;
            txt2.Text = HexToColor(txt2.Text, ref r, ref g, ref b, out valid);
            if (valid) {
                Settings.Default.R2 = r;
                Settings.Default.G2 = g;
                Settings.Default.B2 = b;
            }
        }
        private void t3chk() {
            int r = Settings.Default.R3, g = Settings.Default.G3, b = Settings.Default.B3;
            bool valid;
            txt3.Text = HexToColor(txt3.Text, ref r, ref g, ref b, out valid);
            if (valid) {
                Settings.Default.R3 = r;
                Settings.Default.G3 = g;
                Settings.Default.B3 = b;
            }
        }
        private void txt1_Leave(object sender, EventArgs e) {
            t1chk();
            t2chk();
            t3chk();
            getSkinData();
        }

        private void txt2_Leave(object sender, EventArgs e) {
            t1chk();
            t2chk();
            t3chk();
            getSkinData();
        }

        private void txt3_Leave(object sender, EventArgs e) {
            t1chk();
            t2chk();
            t3chk();
            getSkinData();
        }

        private void txt4_Leave(object sender, EventArgs e) {
            int r = Settings.Default.RPS, g = Settings.Default.GPS, b = Settings.Default.BPS;
            bool valid;
            txt4.Text = HexToColor(txt4.Text, ref r, ref g, ref b, out valid);
            if (valid) {
                Settings.Default.RPS = r;
                Settings.Default.GPS = g;
                Settings.Default.BPS = b;
            }
            getSkinData();
        }

        private void txt5_Leave(object sender, EventArgs e) {
            int r = Settings.Default.RPAO, g = Settings.Default.GPAO, b = Settings.Default.BPAO;
            bool valid;
            txt5.Text = HexToColor(txt5.Text, ref r, ref g, ref b, out valid);
            if (valid) {
                Settings.Default.RPAO = r;
                Settings.Default.GPAO = g;
                Settings.Default.BPAO = b;
            }
            getSkinData();
        }

        private void button3_Click(object sender, EventArgs e) {
            var result = MessageBox.Show(Strings.areYouSure, "LabZSK", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                Settings.Default.Reset();
                Settings.Default.Save();
                loadGUI();
            }
        }
    }
}
