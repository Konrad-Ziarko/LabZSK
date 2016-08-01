using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace LabZKT
{
    public partial class PMSubmit : Form
    {
        private int columnIdx;
        private string chosenInstruction, c1Column, selectedInstruction;

        public string SelectedInstruction
        {
            get
            {
                return selectedInstruction;
            }

            set
            {
                selectedInstruction = value;
            }
        }

        public PMSubmit(int typ, string txt, string c1)
        {
            columnIdx = typ;
            chosenInstruction = txt;
            c1Column = c1;
            InitializeComponent();
        }

        private void RadioPM_Load(object sender, EventArgs e)
        {
            AcceptButton = button_OK;
            CancelButton = button_Cancel;
            foreach(RadioButton rb in groupBox1.Controls)
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
            radioButton1.Focus();
        }
        /// <summary>
        /// Aligne buttons (OK & Cancel) below radiobuttons 
        /// </summary>
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
            loc.X = 155;
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
            PM.isChanged = true;
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void init_S1()
        {
            groupBox1.Text = Text = "S1";
            radioButton2.Text = "IXRE           RI -> LALU";
            radioButton3.Text = "OLR            LR -> BUS";
            radioButton4.Text = "ORR            RR -> BUS";
            radioButton5.Text = "ORAE           RAE -> BUS";
            radioButton6.Text = "IALU           A -> LALU";
            radioButton7.Text = "OXE            X -> RALU";
            radioButton8.Text = "OX             X -> BUS";
            hide_RadioButtons_From(9);
        }
        private void init_D1()
        {
            groupBox1.Text = Text = "D1";
            radioButton2.Text = "ILK            BUS -> LK";
            radioButton3.Text = "IRAP           BUS -> RAP";
            radioButton4.Text = "OXE            X -> RALU";
            hide_RadioButtons_From(5);
        }
        private void init_S2()
        {
            groupBox1.Text = Text = "S2";
            if (c1Column != "SHT")
            {
                radioButton2.Text = "IRAE           SUMA -> RAE";
                radioButton3.Text = "ORR            RR -> BUS";
                radioButton4.Text = "ORI            RI -> BUS";
                radioButton5.Text = "ORAE           RAE -> BUS";
                radioButton6.Text = "OA             A -> BUS";
                radioButton7.Text = "OMQ            MQ -> BUS";
                radioButton8.Text = "OX             X -> BUS";
                radioButton9.Text = "OBE            ALU -> BUS";
                radioButton10.Text = "IXRE           RI -> LALU";
                radioButton11.Text = "IALU           A -> LALU";
                radioButton12.Text = "OXE            X -> RALU";
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
                radioButton2.Text = "ILR            BUS -> LR";
                radioButton3.Text = "IX             BUS -> X";
                radioButton4.Text = "IBE            BUS -> RALU";
                radioButton5.Text = "IRI            BUS -> RI";
                radioButton6.Text = "IBI            BUS -> RAE";
                radioButton7.Text = "IA             BUS -> A";
                radioButton8.Text = "IMQ            BUS -> MQ";
                radioButton9.Text = "OXE            X -> RALU";
                radioButton10.Text = "NSI            LR+1 -> LR";
                radioButton11.Text = "IAS            A0 -> ZNAK";
                radioButton12.Text = "SGN            X0 -> ZNAK";
                hide_RadioButtons_From(13);
            }
        }
        private void init_D2_SHT()
        {
            groupBox1.Text = Text = "D2";
            radioButton1.Enabled = false;
            radioButton2.Text = "ALA            arytmetyczne A w lewo";
            radioButton3.Text = "ARA            arytmetyczne A w prawo";
            radioButton4.Text = "LRQ            logiczne A i MQ w prawo";
            radioButton5.Text = "LLQ            logiczne A i MQ w lewo";
            radioButton6.Text = "LLA            logiczne A w lewo";
            radioButton7.Text = "LRA            logiczne A w prawo";
            radioButton8.Text = "LCA            cykliczne A w lewo";
            button_Cancel.Enabled = false;
            hide_RadioButtons_From(9);
            radioButton2.Checked = true;
        }
        private void init_S3()
        {
            groupBox1.Text = Text = "S3";
            if (c1Column != "SHT")
            {
                radioButton2.Text = "ORI            RI -> BUS";
                radioButton3.Text = "OLR            LR -> BUS";
                radioButton4.Text = "OA             A -> BUS";
                radioButton5.Text = "ORAE           RAE -> BUS";
                radioButton6.Text = "OMQ            MQ -> BUS";
                radioButton7.Text = "ORB            RBP -> BUS";
                radioButton8.Text = "OXE            X -> RALU";
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
                radioButton2.Text = "ILR            BUS -> LR";
                radioButton3.Text = "IX             BUS -> X";
                radioButton4.Text = "IBE            BUS -> RALU";
                radioButton5.Text = "IRI            BUS -> RI";
                radioButton6.Text = "IBI            BUS -> RAE";
                radioButton7.Text = "IA             BUS -> A";
                radioButton8.Text = "IMQ            BUS -> MQ";
                radioButton9.Text = "OXE            X -> RALU";
                radioButton10.Text = "NSI            LR+1 -> LR";
                radioButton11.Text = "IAS            A0 -> ZNAK";
                radioButton12.Text = "SGN            X0 -> ZNAK";
                radioButton13.Text = "IRR            BUS -> RR";
                radioButton14.Text = "IRBP           BUS -> RBP";
                radioButton15.Text = "SRBP           BUS -> RBP";
                hide_RadioButtons_From(16);
            }
            else
                hide_RadioButtons_From(2);
        }
        private void init_C1()
        {
            groupBox1.Text = Text = "C1";
            radioButton2.Text = "CWC            Rozpoczęcie CWC";
            radioButton3.Text = "RRC            Rozpoczęcie RRC";
            radioButton4.Text = "MUL            16 -> LK";
            radioButton5.Text = "DIV            15 -> LK";
            radioButton6.Text = "SHT            Operacja przesunięcia";
            radioButton7.Text = "IWC            Rozpoczęcie IWC";
            radioButton8.Text = "END            Koniec mikroprogramu";
            hide_RadioButtons_From(9);
        }
        private void init_C2()
        {
            groupBox1.Text = Text = "C2";
            radioButton2.Text = "DLK            LK = [LK]-1";
            radioButton3.Text = "SOFF           OFF = 1";
            radioButton4.Text = "ROFF           OFF = 0";
            radioButton5.Text = "SXRO           XRO = 1";
            radioButton6.Text = "RXRO           XRO = 0";
            radioButton7.Text = "DRI            RI = RI-1";
            radioButton8.Text = "RA             A = 0";
            radioButton9.Text = "RMQ            MQ = 0";
            radioButton10.Text = "AQ16           NOT A0 -> MQ16";
            radioButton11.Text = "RINT           INT = 0";
            radioButton12.Text = "OPC            OP lub AOP+32 -> RAPS";
            radioButton13.Text = "CEA            Oblicz adres efektywny";
            radioButton14.Text = "ENI            Odblokuj przerwania";
            hide_RadioButtons_From(15);
        }
        private void init_Test()
        {
            groupBox1.Text = Text = "Test";
            radioButton2.Text = "UNB            Zawsze pozytywny";
            radioButton3.Text = "TINT           Brak przerwania";
            radioButton4.Text = "TIND           Adresowanie pośrednie";
            radioButton5.Text = "TAS            A >= 0";
            radioButton6.Text = "TXS            RI >= 0";
            radioButton7.Text = "TQ15           MQ15 = 0";
            radioButton8.Text = "TCR            -(LK*SHT)";
            radioButton9.Text = "TSD            ZNAK = 0";
            radioButton10.Text = "TAO            OFF = 0";
            radioButton11.Text = "TXP            RI < 0";
            radioButton12.Text = "TXZ            BXZ i RI <> 0 lub TLD i RI = 0";
            radioButton13.Text = "TXRO           XRO = 0";
            radioButton14.Text = "TAP            A < 0";
            radioButton15.Text = "TAZ            A = 0";
            hide_RadioButtons_From(16);
        }
        private void init_ALU()
        {
            groupBox1.Text = Text = "ALU";
            radioButton2.Text = "ADS            ALU = LALU + RALU";
            radioButton3.Text = "SUS            ALU = LALU - RALU";
            radioButton4.Text = "CMX            ALU = (NOT RALU)+1";
            radioButton5.Text = "CMA            ALU = (NOT LALU)+1";
            radioButton6.Text = "OR             ALU = LALU OR RALU";
            radioButton7.Text = "AND            ALU = LALU AND RALU";
            radioButton8.Text = "EOR            ALU = LALU XOR RALU";
            radioButton9.Text = "NOTL           ALU = NOT LALU";
            radioButton10.Text = "NOTR           ALU = NOT RALU";
            radioButton11.Text = "L              ALU = LALU";
            radioButton12.Text = "R              ALU = RALU";
            radioButton13.Text = "INCL           ALU = LALU + 1";
            radioButton14.Text = "INLK           ALU = RALU + 1";
            radioButton15.Text = "DECL           ALU = LALU - 1";
            radioButton16.Text = "DELK           ALU = RALU - 1";
            radioButton17.Text = "ONE            ALU = 1";
            radioButton18.Text = "ZERO           ALU = 0";
        }
        private void init_NA()
        {
            groupBox1.Text = Text = "NA";
            Label tmpLbl = new Label();
            numUpDown.Visible = true;
            groupBox1.Visible = false;
            AutoSize = false;

            tmpLbl.AutoSize = true;
            tmpLbl.Font = new System.Drawing.Font("Arial Narrow", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            tmpLbl.Location = new System.Drawing.Point(60, 23);
            tmpLbl.Name = "label1";
            tmpLbl.Size = new System.Drawing.Size(196, 23);
            tmpLbl.TabIndex = 0;
            tmpLbl.Text = "Podaj wartość 0 <= NA <= 255";

            numUpDown.Location = new System.Drawing.Point(110, 78);
            numUpDown.Size = new System.Drawing.Size(150, 22);
            numUpDown.TabIndex = 1;
            numUpDown.Text = "";

            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(270, 200);
            Controls.Add(numUpDown);
            Controls.Add(tmpLbl);
            Name = "NA";
            Text = "NA";
            ((System.ComponentModel.ISupportInitialize)(numUpDown)).EndInit();
            ResumeLayout(false);
            PerformLayout();

            numUpDown.Focus();
        }
        /// <summary>
        /// This method changes property (i.e. 'Visible' to false) of radiobuttons from given index.
        /// </summary>
        /// <param name="idxOfRadioButton">Index of first radiobutton to hide</param>
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

        void myRadioButton_MouseClick(object sender, MouseEventArgs e)
        {
            button_OK_Click(sender, (EventArgs)e);
        }
    }
}
