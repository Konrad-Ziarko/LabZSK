namespace LabZSK.Memory
{
    partial class MemSubmit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemSubmit));
            this.panel_Choice = new System.Windows.Forms.Panel();
            this.button_Choice_Cancel = new System.Windows.Forms.Button();
            this.button_Choice_Complex = new System.Windows.Forms.Button();
            this.button_Choice_Simple = new System.Windows.Forms.Button();
            this.button_Choice_Data = new System.Windows.Forms.Button();
            this.panel_Data = new System.Windows.Forms.Panel();
            this.radioButton_Hex = new System.Windows.Forms.RadioButton();
            this.radioButton_Dec = new System.Windows.Forms.RadioButton();
            this.radioButton_Bin = new System.Windows.Forms.RadioButton();
            this.textBox_Data = new System.Windows.Forms.TextBox();
            this.button_Data_Cancel = new System.Windows.Forms.Button();
            this.button_Data_OK = new System.Windows.Forms.Button();
            this.panel_Simple = new System.Windows.Forms.Panel();
            this.labelOP = new System.Windows.Forms.Label();
            this.numericUpDown_Simple = new System.Windows.Forms.NumericUpDown();
            this.labelDA = new System.Windows.Forms.Label();
            this.button_Simple_Cancel = new System.Windows.Forms.Button();
            this.button_Simple_OK = new System.Windows.Forms.Button();
            this.checkBox_I = new System.Windows.Forms.CheckBox();
            this.checkBox_S = new System.Windows.Forms.CheckBox();
            this.checkBox_X = new System.Windows.Forms.CheckBox();
            this.numericUpDown_DA = new System.Windows.Forms.NumericUpDown();
            this.comboBox_Simple = new System.Windows.Forms.ComboBox();
            this.panel_Complex = new System.Windows.Forms.Panel();
            this.labelAOP = new System.Windows.Forms.Label();
            this.numericUpDown_Complex = new System.Windows.Forms.NumericUpDown();
            this.labelN = new System.Windows.Forms.Label();
            this.numericUpDown_N = new System.Windows.Forms.NumericUpDown();
            this.comboBox_Complex = new System.Windows.Forms.ComboBox();
            this.button_Complex_Cancel = new System.Windows.Forms.Button();
            this.button_Complex_OK = new System.Windows.Forms.Button();
            this.panel_Choice.SuspendLayout();
            this.panel_Data.SuspendLayout();
            this.panel_Simple.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Simple)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DA)).BeginInit();
            this.panel_Complex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Complex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_N)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Choice
            // 
            this.panel_Choice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Choice.Controls.Add(this.button_Choice_Cancel);
            this.panel_Choice.Controls.Add(this.button_Choice_Complex);
            this.panel_Choice.Controls.Add(this.button_Choice_Simple);
            this.panel_Choice.Controls.Add(this.button_Choice_Data);
            this.panel_Choice.Location = new System.Drawing.Point(9, 10);
            this.panel_Choice.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Choice.Name = "panel_Choice";
            this.panel_Choice.Size = new System.Drawing.Size(260, 206);
            this.panel_Choice.TabIndex = 0;
            // 
            // button_Choice_Cancel
            // 
            this.button_Choice_Cancel.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Choice_Cancel.Location = new System.Drawing.Point(169, 159);
            this.button_Choice_Cancel.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Choice_Cancel.Name = "button_Choice_Cancel";
            this.button_Choice_Cancel.Size = new System.Drawing.Size(80, 27);
            this.button_Choice_Cancel.TabIndex = 3;
            this.button_Choice_Cancel.Text = "Anuluj";
            this.button_Choice_Cancel.UseVisualStyleBackColor = true;
            this.button_Choice_Cancel.Click += new System.EventHandler(this.button_Choice_Cancel_Click);
            // 
            // button_Choice_Complex
            // 
            this.button_Choice_Complex.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Choice_Complex.Location = new System.Drawing.Point(52, 110);
            this.button_Choice_Complex.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Choice_Complex.Name = "button_Choice_Complex";
            this.button_Choice_Complex.Size = new System.Drawing.Size(152, 27);
            this.button_Choice_Complex.TabIndex = 2;
            this.button_Choice_Complex.Text = "Rozkaz rozszerzony";
            this.button_Choice_Complex.UseVisualStyleBackColor = true;
            this.button_Choice_Complex.Click += new System.EventHandler(this.button_Choice_Complex_Click);
            // 
            // button_Choice_Simple
            // 
            this.button_Choice_Simple.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Choice_Simple.Location = new System.Drawing.Point(52, 63);
            this.button_Choice_Simple.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Choice_Simple.Name = "button_Choice_Simple";
            this.button_Choice_Simple.Size = new System.Drawing.Size(152, 27);
            this.button_Choice_Simple.TabIndex = 1;
            this.button_Choice_Simple.Text = "Rozkaz zwykły";
            this.button_Choice_Simple.UseVisualStyleBackColor = true;
            this.button_Choice_Simple.Click += new System.EventHandler(this.button_Choice_Simple_Click);
            // 
            // button_Choice_Data
            // 
            this.button_Choice_Data.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Choice_Data.Location = new System.Drawing.Point(52, 17);
            this.button_Choice_Data.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Choice_Data.Name = "button_Choice_Data";
            this.button_Choice_Data.Size = new System.Drawing.Size(152, 27);
            this.button_Choice_Data.TabIndex = 0;
            this.button_Choice_Data.Text = "Dana";
            this.button_Choice_Data.UseVisualStyleBackColor = true;
            this.button_Choice_Data.Click += new System.EventHandler(this.button_Choice_Data_Click);
            // 
            // panel_Data
            // 
            this.panel_Data.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Data.Controls.Add(this.radioButton_Hex);
            this.panel_Data.Controls.Add(this.radioButton_Dec);
            this.panel_Data.Controls.Add(this.radioButton_Bin);
            this.panel_Data.Controls.Add(this.textBox_Data);
            this.panel_Data.Controls.Add(this.button_Data_Cancel);
            this.panel_Data.Controls.Add(this.button_Data_OK);
            this.panel_Data.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.panel_Data.Location = new System.Drawing.Point(9, 10);
            this.panel_Data.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Data.Name = "panel_Data";
            this.panel_Data.Size = new System.Drawing.Size(260, 206);
            this.panel_Data.TabIndex = 1;
            // 
            // radioButton_Hex
            // 
            this.radioButton_Hex.AutoSize = true;
            this.radioButton_Hex.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton_Hex.Location = new System.Drawing.Point(52, 67);
            this.radioButton_Hex.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_Hex.Name = "radioButton_Hex";
            this.radioButton_Hex.Size = new System.Drawing.Size(110, 21);
            this.radioButton_Hex.TabIndex = 6;
            this.radioButton_Hex.Text = "szesnastkowo";
            this.radioButton_Hex.UseVisualStyleBackColor = true;
            this.radioButton_Hex.CheckedChanged += new System.EventHandler(this.radioButton_Hex_CheckedChanged);
            // 
            // radioButton_Dec
            // 
            this.radioButton_Dec.AutoSize = true;
            this.radioButton_Dec.Checked = true;
            this.radioButton_Dec.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton_Dec.Location = new System.Drawing.Point(52, 42);
            this.radioButton_Dec.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_Dec.Name = "radioButton_Dec";
            this.radioButton_Dec.Size = new System.Drawing.Size(86, 21);
            this.radioButton_Dec.TabIndex = 5;
            this.radioButton_Dec.TabStop = true;
            this.radioButton_Dec.Text = "dziesiętnie";
            this.radioButton_Dec.UseVisualStyleBackColor = true;
            this.radioButton_Dec.CheckedChanged += new System.EventHandler(this.radioButton_Dec_CheckedChanged);
            // 
            // radioButton_Bin
            // 
            this.radioButton_Bin.AutoSize = true;
            this.radioButton_Bin.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButton_Bin.Location = new System.Drawing.Point(52, 17);
            this.radioButton_Bin.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_Bin.Name = "radioButton_Bin";
            this.radioButton_Bin.Size = new System.Drawing.Size(73, 21);
            this.radioButton_Bin.TabIndex = 4;
            this.radioButton_Bin.Text = "binarnie";
            this.radioButton_Bin.UseVisualStyleBackColor = true;
            this.radioButton_Bin.CheckedChanged += new System.EventHandler(this.radioButton_Bin_CheckedChanged);
            // 
            // textBox_Data
            // 
            this.textBox_Data.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Data.Location = new System.Drawing.Point(52, 102);
            this.textBox_Data.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_Data.Name = "textBox_Data";
            this.textBox_Data.Size = new System.Drawing.Size(112, 20);
            this.textBox_Data.TabIndex = 3;
            this.textBox_Data.Text = "0";
            this.textBox_Data.TextChanged += new System.EventHandler(this.textBox_Data_TextChanged);
            this.textBox_Data.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Data_KeyPress);
            // 
            // button_Data_Cancel
            // 
            this.button_Data_Cancel.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Data_Cancel.Location = new System.Drawing.Point(169, 159);
            this.button_Data_Cancel.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Data_Cancel.Name = "button_Data_Cancel";
            this.button_Data_Cancel.Size = new System.Drawing.Size(80, 27);
            this.button_Data_Cancel.TabIndex = 2;
            this.button_Data_Cancel.Text = "Anuluj";
            this.button_Data_Cancel.UseVisualStyleBackColor = true;
            this.button_Data_Cancel.Click += new System.EventHandler(this.button_Data_Cancel_Click);
            // 
            // button_Data_OK
            // 
            this.button_Data_OK.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Data_OK.Location = new System.Drawing.Point(9, 159);
            this.button_Data_OK.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Data_OK.Name = "button_Data_OK";
            this.button_Data_OK.Size = new System.Drawing.Size(87, 27);
            this.button_Data_OK.TabIndex = 1;
            this.button_Data_OK.Text = "Zatwierdź";
            this.button_Data_OK.UseVisualStyleBackColor = true;
            this.button_Data_OK.Click += new System.EventHandler(this.button_Data_OK_Click);
            // 
            // panel_Simple
            // 
            this.panel_Simple.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Simple.Controls.Add(this.labelOP);
            this.panel_Simple.Controls.Add(this.numericUpDown_Simple);
            this.panel_Simple.Controls.Add(this.labelDA);
            this.panel_Simple.Controls.Add(this.button_Simple_Cancel);
            this.panel_Simple.Controls.Add(this.button_Simple_OK);
            this.panel_Simple.Controls.Add(this.checkBox_I);
            this.panel_Simple.Controls.Add(this.checkBox_S);
            this.panel_Simple.Controls.Add(this.checkBox_X);
            this.panel_Simple.Controls.Add(this.numericUpDown_DA);
            this.panel_Simple.Controls.Add(this.comboBox_Simple);
            this.panel_Simple.Location = new System.Drawing.Point(9, 10);
            this.panel_Simple.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Simple.Name = "panel_Simple";
            this.panel_Simple.Size = new System.Drawing.Size(260, 206);
            this.panel_Simple.TabIndex = 1;
            // 
            // labelOP
            // 
            this.labelOP.AutoSize = true;
            this.labelOP.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelOP.Location = new System.Drawing.Point(69, 50);
            this.labelOP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelOP.Name = "labelOP";
            this.labelOP.Size = new System.Drawing.Size(30, 19);
            this.labelOP.TabIndex = 14;
            this.labelOP.Text = "OP";
            // 
            // numericUpDown_Simple
            // 
            this.numericUpDown_Simple.Location = new System.Drawing.Point(9, 50);
            this.numericUpDown_Simple.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_Simple.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numericUpDown_Simple.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Simple.Name = "numericUpDown_Simple";
            this.numericUpDown_Simple.Size = new System.Drawing.Size(51, 20);
            this.numericUpDown_Simple.TabIndex = 13;
            this.numericUpDown_Simple.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Simple.ValueChanged += new System.EventHandler(this.numericUpDown_Simple_ValueChanged);
            // 
            // labelDA
            // 
            this.labelDA.AutoSize = true;
            this.labelDA.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDA.Location = new System.Drawing.Point(69, 128);
            this.labelDA.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDA.Name = "labelDA";
            this.labelDA.Size = new System.Drawing.Size(31, 19);
            this.labelDA.TabIndex = 12;
            this.labelDA.Text = "DA";
            // 
            // button_Simple_Cancel
            // 
            this.button_Simple_Cancel.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Simple_Cancel.Location = new System.Drawing.Point(169, 159);
            this.button_Simple_Cancel.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Simple_Cancel.Name = "button_Simple_Cancel";
            this.button_Simple_Cancel.Size = new System.Drawing.Size(80, 27);
            this.button_Simple_Cancel.TabIndex = 7;
            this.button_Simple_Cancel.Text = "Anuluj";
            this.button_Simple_Cancel.UseVisualStyleBackColor = true;
            this.button_Simple_Cancel.Click += new System.EventHandler(this.button_Simple_Cancel_Click);
            // 
            // button_Simple_OK
            // 
            this.button_Simple_OK.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Simple_OK.Location = new System.Drawing.Point(9, 159);
            this.button_Simple_OK.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Simple_OK.Name = "button_Simple_OK";
            this.button_Simple_OK.Size = new System.Drawing.Size(87, 27);
            this.button_Simple_OK.TabIndex = 7;
            this.button_Simple_OK.Text = "Zatwierdź";
            this.button_Simple_OK.UseVisualStyleBackColor = true;
            this.button_Simple_OK.Click += new System.EventHandler(this.button_Simple_OK_Click);
            // 
            // checkBox_I
            // 
            this.checkBox_I.AutoSize = true;
            this.checkBox_I.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBox_I.Location = new System.Drawing.Point(174, 85);
            this.checkBox_I.Margin = new System.Windows.Forms.Padding(15, 2, 15, 2);
            this.checkBox_I.Name = "checkBox_I";
            this.checkBox_I.Size = new System.Drawing.Size(57, 21);
            this.checkBox_I.TabIndex = 4;
            this.checkBox_I.Text = "I = 0";
            this.checkBox_I.UseVisualStyleBackColor = true;
            this.checkBox_I.CheckedChanged += new System.EventHandler(this.checkBox_I_CheckedChanged);
            // 
            // checkBox_S
            // 
            this.checkBox_S.AutoSize = true;
            this.checkBox_S.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBox_S.Location = new System.Drawing.Point(92, 85);
            this.checkBox_S.Margin = new System.Windows.Forms.Padding(15, 2, 15, 2);
            this.checkBox_S.Name = "checkBox_S";
            this.checkBox_S.Size = new System.Drawing.Size(61, 21);
            this.checkBox_S.TabIndex = 3;
            this.checkBox_S.Text = "S = 0";
            this.checkBox_S.UseVisualStyleBackColor = true;
            this.checkBox_S.CheckedChanged += new System.EventHandler(this.checkBox_S_CheckedChanged);
            // 
            // checkBox_X
            // 
            this.checkBox_X.AutoSize = true;
            this.checkBox_X.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBox_X.Location = new System.Drawing.Point(10, 85);
            this.checkBox_X.Margin = new System.Windows.Forms.Padding(15, 2, 15, 2);
            this.checkBox_X.Name = "checkBox_X";
            this.checkBox_X.Size = new System.Drawing.Size(61, 21);
            this.checkBox_X.TabIndex = 2;
            this.checkBox_X.Text = "X = 0";
            this.checkBox_X.UseVisualStyleBackColor = true;
            this.checkBox_X.CheckedChanged += new System.EventHandler(this.checkBox_X_CheckedChanged);
            // 
            // numericUpDown_DA
            // 
            this.numericUpDown_DA.Location = new System.Drawing.Point(10, 128);
            this.numericUpDown_DA.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_DA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown_DA.Name = "numericUpDown_DA";
            this.numericUpDown_DA.Size = new System.Drawing.Size(52, 20);
            this.numericUpDown_DA.TabIndex = 1;
            this.numericUpDown_DA.ValueChanged += new System.EventHandler(this.numericUpDown_DA_ValueChanged);
            // 
            // comboBox_Simple
            // 
            this.comboBox_Simple.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.comboBox_Simple.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Simple.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox_Simple.FormattingEnabled = true;
            this.comboBox_Simple.Items.AddRange(new object[] {
            "ADD - Dodawanie",
            "SUB - Odejmowanie",
            "MUL - Mnożenie",
            "DIV - Dzielenie",
            "STQ - Zapamiętaj rejestr MQ",
            "STA - Zapamiętaj akumulator A",
            "STQ - Zapamiętaj modyfikator RI",
            "LDA - Ładuj akumulator A",
            "LDX - Ładuj modyfikator RI",
            "STC - Zapamiętaj LR",
            "TXA - Prześlij RI do A",
            "TMQ - Prześlij MQ do A",
            "ADX - Dodaj do RI",
            "SIO - Start operacji WE-WY",
            "LIO - Ładuj licznik słów WE-WY",
            "UNB - Skok bezwarunkowy",
            "BAO - Skocz jeśli nadmiar A",
            "BXP - Skocz jeśli RI > 0",
            "BXZ - Skocz jeśli RI = 0",
            "BXN - Skocz jeśli RI < 0",
            "TLD - RI=RI-1 i Skocz jeśli RI > 0",
            "BAP - Skocz jeśli A > 0",
            "BAZ - Skocz jeśli A = 0",
            "BAN - Skocz jeśli A < 0",
            "LOR - Suma logiczna A i komórki",
            "LAND - Iloczyn logiczny A i komórki",
            "LNG - Negacja logiczna A",
            "EOR - Różnica symetryczna A i komórki",
            "SRJ - Skok ze śladem",
            "BDN - Skok jeśli urządzenie niedostępne",
            "NOP - Nic nie rób"});
            this.comboBox_Simple.Location = new System.Drawing.Point(2, 2);
            this.comboBox_Simple.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_Simple.Name = "comboBox_Simple";
            this.comboBox_Simple.Size = new System.Drawing.Size(254, 22);
            this.comboBox_Simple.TabIndex = 0;
            this.comboBox_Simple.SelectedIndexChanged += new System.EventHandler(this.comboBox_Simple_SelectedIndexChanged);
            // 
            // panel_Complex
            // 
            this.panel_Complex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Complex.Controls.Add(this.labelAOP);
            this.panel_Complex.Controls.Add(this.numericUpDown_Complex);
            this.panel_Complex.Controls.Add(this.labelN);
            this.panel_Complex.Controls.Add(this.numericUpDown_N);
            this.panel_Complex.Controls.Add(this.comboBox_Complex);
            this.panel_Complex.Controls.Add(this.button_Complex_Cancel);
            this.panel_Complex.Controls.Add(this.button_Complex_OK);
            this.panel_Complex.Location = new System.Drawing.Point(9, 10);
            this.panel_Complex.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Complex.Name = "panel_Complex";
            this.panel_Complex.Size = new System.Drawing.Size(260, 206);
            this.panel_Complex.TabIndex = 2;
            // 
            // labelAOP
            // 
            this.labelAOP.AutoSize = true;
            this.labelAOP.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelAOP.Location = new System.Drawing.Point(69, 51);
            this.labelAOP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAOP.Name = "labelAOP";
            this.labelAOP.Size = new System.Drawing.Size(41, 19);
            this.labelAOP.TabIndex = 15;
            this.labelAOP.Text = "AOP";
            // 
            // numericUpDown_Complex
            // 
            this.numericUpDown_Complex.Location = new System.Drawing.Point(10, 50);
            this.numericUpDown_Complex.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_Complex.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown_Complex.Name = "numericUpDown_Complex";
            this.numericUpDown_Complex.Size = new System.Drawing.Size(51, 20);
            this.numericUpDown_Complex.TabIndex = 14;
            this.numericUpDown_Complex.ValueChanged += new System.EventHandler(this.numericUpDown_Complex_ValueChanged);
            // 
            // labelN
            // 
            this.labelN.AutoSize = true;
            this.labelN.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelN.Location = new System.Drawing.Point(76, 97);
            this.labelN.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelN.Name = "labelN";
            this.labelN.Size = new System.Drawing.Size(20, 19);
            this.labelN.TabIndex = 11;
            this.labelN.Text = "N";
            // 
            // numericUpDown_N
            // 
            this.numericUpDown_N.Location = new System.Drawing.Point(10, 97);
            this.numericUpDown_N.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_N.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDown_N.Name = "numericUpDown_N";
            this.numericUpDown_N.Size = new System.Drawing.Size(51, 20);
            this.numericUpDown_N.TabIndex = 10;
            this.numericUpDown_N.ValueChanged += new System.EventHandler(this.numericUpDown_N_ValueChanged);
            // 
            // comboBox_Complex
            // 
            this.comboBox_Complex.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.comboBox_Complex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Complex.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox_Complex.FormattingEnabled = true;
            this.comboBox_Complex.Items.AddRange(new object[] {
            "STP - Stop dynamiczny",
            "CMA - Uzupełnienie A do 2",
            "ALA - Przesunięcie arytmetyczne A w lewo",
            "ARA - Przesunięcie arytmetyczne A w prawo",
            "LRQ - Przesunięcie logiczne A || MQ w prawo",
            "LLQ - Przesunięcie logiczne A || MQ w lewo",
            "LLA - Przesunięcie logiczne A w lewo",
            "LRA - Przesunięcie lgoczine A w prawo",
            "LCA - Przesunięcie cykliczne A w lewo",
            "LAI - Ładuj A bezpośrednio",
            "LXI - Ładuj RI bezpośrednio",
            "INX - Zwiększ modyfikator RI",
            "DEX - Zmniejsz modyfikator RO",
            "CND - Podłącz urządzenie",
            "ENI - Zezwolenie na przerwania",
            "LDS - Podaj status"});
            this.comboBox_Complex.Location = new System.Drawing.Point(2, 2);
            this.comboBox_Complex.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_Complex.Name = "comboBox_Complex";
            this.comboBox_Complex.Size = new System.Drawing.Size(254, 22);
            this.comboBox_Complex.TabIndex = 9;
            this.comboBox_Complex.SelectedIndexChanged += new System.EventHandler(this.comboBox_Complex_SelectedIndexChanged);
            // 
            // button_Complex_Cancel
            // 
            this.button_Complex_Cancel.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Complex_Cancel.Location = new System.Drawing.Point(169, 159);
            this.button_Complex_Cancel.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Complex_Cancel.Name = "button_Complex_Cancel";
            this.button_Complex_Cancel.Size = new System.Drawing.Size(80, 27);
            this.button_Complex_Cancel.TabIndex = 8;
            this.button_Complex_Cancel.Text = "Anuluj";
            this.button_Complex_Cancel.UseVisualStyleBackColor = true;
            this.button_Complex_Cancel.Click += new System.EventHandler(this.button_Complex_Cancel_Click);
            // 
            // button_Complex_OK
            // 
            this.button_Complex_OK.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Complex_OK.Location = new System.Drawing.Point(9, 159);
            this.button_Complex_OK.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.button_Complex_OK.Name = "button_Complex_OK";
            this.button_Complex_OK.Size = new System.Drawing.Size(87, 27);
            this.button_Complex_OK.TabIndex = 8;
            this.button_Complex_OK.Text = "Zatwierdź";
            this.button_Complex_OK.UseVisualStyleBackColor = true;
            this.button_Complex_OK.Click += new System.EventHandler(this.button_Complex_OK_Click);
            // 
            // MemSubmit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 232);
            this.Controls.Add(this.panel_Complex);
            this.Controls.Add(this.panel_Choice);
            this.Controls.Add(this.panel_Simple);
            this.Controls.Add(this.panel_Data);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(296, 271);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(296, 271);
            this.Name = "MemSubmit";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Edycja komórki";
            this.Load += new System.EventHandler(this.PAOSubmit_Load);
            this.Shown += new System.EventHandler(this.MemSubmit_Shown);
            this.panel_Choice.ResumeLayout(false);
            this.panel_Data.ResumeLayout(false);
            this.panel_Data.PerformLayout();
            this.panel_Simple.ResumeLayout(false);
            this.panel_Simple.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Simple)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DA)).EndInit();
            this.panel_Complex.ResumeLayout(false);
            this.panel_Complex.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Complex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_N)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_Choice;
        private System.Windows.Forms.Button button_Choice_Cancel;
        private System.Windows.Forms.Button button_Choice_Complex;
        private System.Windows.Forms.Button button_Choice_Simple;
        private System.Windows.Forms.Button button_Choice_Data;
        private System.Windows.Forms.Panel panel_Data;
        private System.Windows.Forms.Button button_Data_Cancel;
        private System.Windows.Forms.Button button_Data_OK;
        private System.Windows.Forms.TextBox textBox_Data;
        private System.Windows.Forms.RadioButton radioButton_Hex;
        private System.Windows.Forms.RadioButton radioButton_Dec;
        private System.Windows.Forms.RadioButton radioButton_Bin;
        private System.Windows.Forms.Panel panel_Simple;
        private System.Windows.Forms.Panel panel_Complex;
        private System.Windows.Forms.ComboBox comboBox_Simple;
        private System.Windows.Forms.Button button_Simple_Cancel;
        private System.Windows.Forms.Button button_Simple_OK;
        private System.Windows.Forms.CheckBox checkBox_I;
        private System.Windows.Forms.CheckBox checkBox_S;
        private System.Windows.Forms.CheckBox checkBox_X;
        private System.Windows.Forms.NumericUpDown numericUpDown_DA;
        private System.Windows.Forms.NumericUpDown numericUpDown_N;
        private System.Windows.Forms.ComboBox comboBox_Complex;
        private System.Windows.Forms.Button button_Complex_Cancel;
        private System.Windows.Forms.Button button_Complex_OK;
        private System.Windows.Forms.Label labelDA;
        private System.Windows.Forms.Label labelN;
        private System.Windows.Forms.NumericUpDown numericUpDown_Simple;
        private System.Windows.Forms.NumericUpDown numericUpDown_Complex;
        private System.Windows.Forms.Label labelOP;
        private System.Windows.Forms.Label labelAOP;
    }
}