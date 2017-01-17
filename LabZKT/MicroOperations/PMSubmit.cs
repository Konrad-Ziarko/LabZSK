using LabZSK.StaticClasses;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LabZSK.MicroOperations
{
    /// <summary>
    /// Responsible for submiting new microoperations
    /// </summary>
    public partial class PMSubmit : Form
    {
        private int columnIdx;
        private string chosenInstruction, c1Column;
        /// <summary>
        /// String representing whether form was changed
        /// </summary>
        public bool isChanged = false;
        /// <summary>
        /// String representing microoperation name
        /// </summary>
        public string SelectedInstruction { get; set; }
        public Point startPosition { get; set; }
        /// <summary>
        /// Initialize instance of subview
        /// </summary>
        /// <param name="typ">Value representing microoperation type</param>
        /// <param name="txt">String representing microoperation name</param>
        /// <param name="c1">String representing microoperation in column C1</param>
        public PMSubmit(int typ, string txt, string c1)
        {
            columnIdx = typ;
            chosenInstruction = txt;
            c1Column = c1;
            InitializeComponent();
            button_Cancel.Text = Strings.cancelButton;
        }

        private void RadioPM_Load(object sender, EventArgs e)
        {
            AcceptButton = button_OK;
            CancelButton = button_Cancel;
            foreach (RadioButton rb in groupBox1.Controls)
            {
                rb.MouseDoubleClick += myRadioButton_MouseClick;
                MethodInfo m = typeof(RadioButton).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
                if (m != null)
                {
                    m.Invoke(rb, new object[] { ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true });

                }
            }

            if (columnIdx == 4 && c1Column == "SHT")
            {
                init_D2_SHT();
            }
            else
            {
                if (columnIdx == 1)
                    init_S1();
                else if (columnIdx == 2)
                    init_D1();
                else if (columnIdx == 3)
                    init_S2();
                else if (columnIdx == 4)
                    init_D2();
                else if (columnIdx == 5)
                    init_S3();
                else if (columnIdx == 6)
                    init_D3();
                else if (columnIdx == 7)
                    init_C1();
                else if (columnIdx == 8)
                    init_C2();
                else if (columnIdx == 9)
                    init_Test();
                else if (columnIdx == 10)
                    init_ALU();
                else if (columnIdx == 11)
                    init_NA();
            }
            aligneButtons();
            Point startLocation = startPosition;
            startLocation.Y -= this.Height / 2;
            if (startLocation.Y < 0)
                startLocation.Y = 0;
            startLocation.X -= this.Width / 2;
            Location = startLocation;
            var curLoc = Location;
            if (curLoc.Y < 0)
                curLoc.Y = 0;
            if (curLoc.X < 0)
                curLoc.X = 0;
            if (curLoc.Y + Size.Height > Screen.PrimaryScreen.Bounds.Height)
                curLoc.Y = Screen.PrimaryScreen.Bounds.Height - Size.Height;
            if (curLoc.X + Size.Width > Screen.PrimaryScreen.Bounds.Width)
                curLoc.X = Screen.PrimaryScreen.Bounds.Width - Size.Width;
            Location = curLoc;
            radioButton1.Focus();
            radioButton1.Select();
        }
        private void aligneButtons()
        {
            var loc = button_OK.Location;
            loc.Y = groupBox1.Location.Y + groupBox1.Size.Height + 10;
            if (!groupBox1.Visible)
            {
                loc.Y = 150;
            }
            loc.X = 13;
            button_OK.Location = loc;
            loc = button_Cancel.Location;
            loc.Y = groupBox1.Location.Y + groupBox1.Size.Height + 10;
            if (!groupBox1.Visible)
            {
                loc.Y = 150;
            }
            loc.X = Width-121;
            button_Cancel.Location = loc;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (groupBox1.Visible)
            {
                var checkedButton = groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
                SelectedInstruction = checkedButton.Text.Split()[0];
            }
            else
            {
                SelectedInstruction = Convert.ToInt32(numUpDown.Value).ToString();
            }
            DialogResult = DialogResult.OK;
            isChanged = true;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        #region Fields init
        private void init_S1()
        {
            groupBox1.Text = Text = "S1";
            radioButton2.Text = Translator.GetMicroOpExtendedDescription("IXRE");
            radioButton3.Text = Translator.GetMicroOpExtendedDescription("OLR");
            radioButton4.Text = Translator.GetMicroOpExtendedDescription("ORR");
            radioButton5.Text = Translator.GetMicroOpExtendedDescription("ORAE");
            radioButton6.Text = Translator.GetMicroOpExtendedDescription("IALU");
            radioButton7.Text = Translator.GetMicroOpExtendedDescription("OXE");
            radioButton8.Text = Translator.GetMicroOpExtendedDescription("OX");
            hide_RadioButtons_From(9);
        }
        private void init_D1()
        {
            groupBox1.Text = Text = "D1";
            radioButton2.Text = Translator.GetMicroOpExtendedDescription("ILK");
            radioButton3.Text = Translator.GetMicroOpExtendedDescription("IRAP");
            radioButton4.Text = Translator.GetMicroOpExtendedDescription("OXE");
            hide_RadioButtons_From(5);
        }
        private void init_S2()
        {
            groupBox1.Text = Text = "S2";
            if (c1Column != "SHT")
            {
                radioButton2.Text = Translator.GetMicroOpExtendedDescription("IRAE");
                radioButton3.Text = Translator.GetMicroOpExtendedDescription("ORR");
                radioButton4.Text = Translator.GetMicroOpExtendedDescription("ORI");
                radioButton5.Text = Translator.GetMicroOpExtendedDescription("ORAE");
                radioButton6.Text = Translator.GetMicroOpExtendedDescription("OA");
                radioButton7.Text = Translator.GetMicroOpExtendedDescription("OMQ");
                radioButton8.Text = Translator.GetMicroOpExtendedDescription("OX");
                radioButton9.Text = Translator.GetMicroOpExtendedDescription("OBE");
                radioButton10.Text = Translator.GetMicroOpExtendedDescription("IXRE");
                radioButton11.Text = Translator.GetMicroOpExtendedDescription("IALU");
                radioButton12.Text = Translator.GetMicroOpExtendedDescription("OXE");
                hide_RadioButtons_From(13);
            }
            else
            {
                hide_RadioButtons_From(2);
            }
        }
        private void init_D2()
        {
            groupBox1.Text = Text = "D2";
            if (chosenInstruction == "ALA" || chosenInstruction == "ARA" || chosenInstruction == "LRQ"
                || chosenInstruction == "LLQ" || chosenInstruction == "LLA" || chosenInstruction == "LRA" || chosenInstruction == "LCA")
            {
                init_D2_SHT();
            }
            else
            {
                radioButton2.Text = Translator.GetMicroOpExtendedDescription("ILR");
                radioButton3.Text = Translator.GetMicroOpExtendedDescription("IX");
                radioButton4.Text = Translator.GetMicroOpExtendedDescription("IBE");
                radioButton5.Text = Translator.GetMicroOpExtendedDescription("IRI");
                radioButton6.Text = Translator.GetMicroOpExtendedDescription("IBI");
                radioButton7.Text = Translator.GetMicroOpExtendedDescription("IA");
                radioButton8.Text = Translator.GetMicroOpExtendedDescription("IMQ");
                radioButton9.Text = Translator.GetMicroOpExtendedDescription("OXE");
                radioButton10.Text = Translator.GetMicroOpExtendedDescription("NSI");
                radioButton11.Text = Translator.GetMicroOpExtendedDescription("IAS");
                radioButton12.Text = Translator.GetMicroOpExtendedDescription("SGN");
                hide_RadioButtons_From(13);
            }
        }
        private void init_D2_SHT()
        {
            groupBox1.Text = Text = "D2";
            radioButton1.Enabled = false;
            radioButton2.Text = Translator.GetMicroOpExtendedDescription("ALA");
            radioButton3.Text = Translator.GetMicroOpExtendedDescription("ARA");
            radioButton4.Text = Translator.GetMicroOpExtendedDescription("LRQ");
            radioButton5.Text = Translator.GetMicroOpExtendedDescription("LLQ");
            radioButton6.Text = Translator.GetMicroOpExtendedDescription("LLA");
            radioButton7.Text = Translator.GetMicroOpExtendedDescription("LRA");
            radioButton8.Text = Translator.GetMicroOpExtendedDescription("LCA");
            button_Cancel.Enabled = false;
            hide_RadioButtons_From(9);
            radioButton2.Checked = true;
        }
        private void init_S3()
        {
            groupBox1.Text = Text = "S3";
            if (c1Column != "SHT")
            {
                radioButton2.Text = Translator.GetMicroOpExtendedDescription("ORI");
                radioButton3.Text = Translator.GetMicroOpExtendedDescription("OLR");
                radioButton4.Text = Translator.GetMicroOpExtendedDescription("OA");
                radioButton5.Text = Translator.GetMicroOpExtendedDescription("ORAE");
                radioButton6.Text = Translator.GetMicroOpExtendedDescription("OMQ");
                radioButton7.Text = Translator.GetMicroOpExtendedDescription("ORBP");
                radioButton8.Text = Translator.GetMicroOpExtendedDescription("OXE");
                hide_RadioButtons_From(9);
            }
            else
                hide_RadioButtons_From(2);
        }
        private void init_D3()
        {
            groupBox1.Text = Text = "D3";
            if (c1Column != "SHT")
            {
                radioButton2.Text = Translator.GetMicroOpExtendedDescription("ILR");
                radioButton3.Text = Translator.GetMicroOpExtendedDescription("IX");
                radioButton4.Text = Translator.GetMicroOpExtendedDescription("IBE");
                radioButton5.Text = Translator.GetMicroOpExtendedDescription("IRI");
                radioButton6.Text = Translator.GetMicroOpExtendedDescription("IBI");
                radioButton7.Text = Translator.GetMicroOpExtendedDescription("IA");
                radioButton8.Text = Translator.GetMicroOpExtendedDescription("IMQ");
                radioButton9.Text = Translator.GetMicroOpExtendedDescription("OXE");
                radioButton10.Text = Translator.GetMicroOpExtendedDescription("NSI");
                radioButton11.Text = Translator.GetMicroOpExtendedDescription("IAS");
                radioButton12.Text = Translator.GetMicroOpExtendedDescription("SGN");
                radioButton13.Text = Translator.GetMicroOpExtendedDescription("IRR");
                radioButton14.Text = Translator.GetMicroOpExtendedDescription("IRBP");
                radioButton15.Text = Translator.GetMicroOpExtendedDescription("SRBP");
                hide_RadioButtons_From(16);
            }
            else
                hide_RadioButtons_From(2);
        }
        private void init_C1()
        {
            groupBox1.Text = Text = "C1";
            radioButton2.Text = Translator.GetMicroOpExtendedDescription("CWC");
            radioButton3.Text = Translator.GetMicroOpExtendedDescription("RRC");
            radioButton4.Text = Translator.GetMicroOpExtendedDescription("MUL");
            radioButton5.Text = Translator.GetMicroOpExtendedDescription("DIV");
            radioButton6.Text = Translator.GetMicroOpExtendedDescription("SHT");
            radioButton7.Text = Translator.GetMicroOpExtendedDescription("IWC");
            radioButton8.Text = Translator.GetMicroOpExtendedDescription("END");
            hide_RadioButtons_From(9);
        }
        private void init_C2()
        {
            groupBox1.Text = Text = "C2";
            radioButton2.Text = Translator.GetMicroOpExtendedDescription("DLK");
            radioButton3.Text = Translator.GetMicroOpExtendedDescription("SOFF");
            radioButton4.Text = Translator.GetMicroOpExtendedDescription("ROFF");
            radioButton5.Text = Translator.GetMicroOpExtendedDescription("SXRO");
            radioButton6.Text = Translator.GetMicroOpExtendedDescription("RXRO");
            radioButton7.Text = Translator.GetMicroOpExtendedDescription("DRI");
            radioButton8.Text = Translator.GetMicroOpExtendedDescription("RA");
            radioButton9.Text = Translator.GetMicroOpExtendedDescription("RMQ");
            radioButton10.Text = Translator.GetMicroOpExtendedDescription("AQ15");
            radioButton11.Text = Translator.GetMicroOpExtendedDescription("RINT");
            radioButton12.Text = Translator.GetMicroOpExtendedDescription("OPC");
            radioButton13.Text = Translator.GetMicroOpExtendedDescription("CEA");
            radioButton14.Text = Translator.GetMicroOpExtendedDescription("ENI");
            hide_RadioButtons_From(15);
        }
        private void init_Test()
        {
            groupBox1.Text = Text = "Test";
            radioButton2.Text = Translator.GetMicroOpExtendedDescription("UNB");
            radioButton3.Text = Translator.GetMicroOpExtendedDescription("TINT");
            radioButton4.Text = Translator.GetMicroOpExtendedDescription("TIND");
            radioButton5.Text = Translator.GetMicroOpExtendedDescription("TAS");
            radioButton6.Text = Translator.GetMicroOpExtendedDescription("TXS");
            radioButton7.Text = Translator.GetMicroOpExtendedDescription("TQ15");
            radioButton8.Text = Translator.GetMicroOpExtendedDescription("TLK");
            radioButton9.Text = Translator.GetMicroOpExtendedDescription("TSD");
            radioButton10.Text = Translator.GetMicroOpExtendedDescription("TAO");
            radioButton11.Text = Translator.GetMicroOpExtendedDescription("TXP");
            radioButton12.Text = Translator.GetMicroOpExtendedDescription("TXZ");
            radioButton13.Text = Translator.GetMicroOpExtendedDescription("TXRO");
            radioButton14.Text = Translator.GetMicroOpExtendedDescription("TAP");
            radioButton15.Text = Translator.GetMicroOpExtendedDescription("TAZ");
            hide_RadioButtons_From(16);
        }
        private void init_ALU()
        {
            groupBox1.Text = Text = "ALU";
            radioButton2.Text = Translator.GetMicroOpExtendedDescription("ADS");
            radioButton3.Text = Translator.GetMicroOpExtendedDescription("SUS");
            radioButton4.Text = Translator.GetMicroOpExtendedDescription("CMX");
            radioButton5.Text = Translator.GetMicroOpExtendedDescription("CMA");
            radioButton6.Text = Translator.GetMicroOpExtendedDescription("OR");
            radioButton7.Text = Translator.GetMicroOpExtendedDescription("AND");
            radioButton8.Text = Translator.GetMicroOpExtendedDescription("EOR");
            radioButton9.Text = Translator.GetMicroOpExtendedDescription("NOTL");
            radioButton10.Text = Translator.GetMicroOpExtendedDescription("NOTR");
            radioButton11.Text = Translator.GetMicroOpExtendedDescription("L");
            radioButton12.Text = Translator.GetMicroOpExtendedDescription("R");
            radioButton13.Text = Translator.GetMicroOpExtendedDescription("INCL");
            radioButton14.Text = Translator.GetMicroOpExtendedDescription("INCR");
            radioButton15.Text = Translator.GetMicroOpExtendedDescription("DECL");
            radioButton16.Text = Translator.GetMicroOpExtendedDescription("DECR");
            radioButton17.Text = Translator.GetMicroOpExtendedDescription("ONE");
            radioButton18.Text = Translator.GetMicroOpExtendedDescription("ZERO");
        }
        private void init_NA()
        {
            groupBox1.Text = Text = "NA";
            Label tmpLbl = new Label();
            numUpDown.Visible = true;
            groupBox1.Visible = false;

            tmpLbl.AutoSize = true;
            tmpLbl.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 238);
            tmpLbl.Text = "0 <= NA <= 255";
            tmpLbl.Location = new System.Drawing.Point((Width-tmpLbl.Size.Width)/2, 23);
            tmpLbl.Name = "label1";
            tmpLbl.TabIndex = 0;
            Controls.Add(tmpLbl);

            numUpDown.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 238);
            //numUpDown.Size = new System.Drawing.Size(80, 22);
            numUpDown.Location = new System.Drawing.Point(tmpLbl.Location.X + (tmpLbl.Size.Width - numUpDown.Size.Width) / 2, 78);
            numUpDown.TabIndex = 1;
            numUpDown.Text = "";

            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            
            Controls.Add(numUpDown);
            
            Name = "NA";
            Text = "NA";
            ((System.ComponentModel.ISupportInitialize)(numUpDown)).EndInit();
            ResumeLayout(false);
            PerformLayout();

            numUpDown.Focus();
        }
        #endregion
        private void hide_RadioButtons_From(int idxOfRadioButton)
        {
            switch (idxOfRadioButton)
            {
                case 1:
                    radioButton1.Visible = false;
                    goto case 2;
                case 2:
                    radioButton2.Visible = false;
                    goto case 3;
                case 3:
                    radioButton3.Visible = false;
                    goto case 4;
                case 4:
                    radioButton4.Visible = false;
                    goto case 5;
                case 5:
                    radioButton5.Visible = false;
                    goto case 6;
                case 6:
                    radioButton6.Visible = false;
                    goto case 7;
                case 7:
                    radioButton7.Visible = false;
                    goto case 8;
                case 8:
                    radioButton8.Visible = false;
                    goto case 9;
                case 9:
                    radioButton9.Visible = false;
                    goto case 10;
                case 10:
                    radioButton10.Visible = false;
                    goto case 11;
                case 11:
                    radioButton11.Visible = false;
                    goto case 12;
                case 12:
                    radioButton12.Visible = false;
                    goto case 13;
                case 13:
                    radioButton13.Visible = false;
                    goto case 14;
                case 14:
                    radioButton14.Visible = false;
                    goto case 15;
                case 15:
                    radioButton15.Visible = false;
                    goto case 16;
                case 16:
                    radioButton16.Visible = false;
                    goto case 17;
                case 17:
                    radioButton17.Visible = false;
                    goto case 18;
                case 18:
                    radioButton18.Visible = false;
                    break;
            }
        }

        internal void myRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            button_OK_Click(sender, e);
        }
    }
}
