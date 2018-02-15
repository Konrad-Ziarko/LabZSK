namespace LabZSK.Simulation
{
    partial class DevConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevConsole));
            this.registerName = new System.Windows.Forms.ComboBox();
            this.registerValue = new System.Windows.Forms.NumericUpDown();
            this.buttonStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.registerValue)).BeginInit();
            this.SuspendLayout();
            // 
            // registerName
            // 
            this.registerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.registerName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.registerName.FormattingEnabled = true;
            this.registerName.Items.AddRange(new object[] {
            "Cykle+>",
            "LK",
            "A",
            "MQ",
            "X",
            "RAP",
            "LALU",
            "RALU",
            "RBP",
            "ALU",
            "BUS",
            "RR",
            "LR",
            "RI",
            "RAPS",
            "RAE",
            "L",
            "R",
            "SUMA"});
            this.registerName.Location = new System.Drawing.Point(12, 24);
            this.registerName.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.registerName.Name = "registerName";
            this.registerName.Size = new System.Drawing.Size(101, 26);
            this.registerName.TabIndex = 0;
            // 
            // registerValue
            // 
            this.registerValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.registerValue.Location = new System.Drawing.Point(145, 24);
            this.registerValue.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.registerValue.Maximum = new decimal(new int[] {
            40000,
            0,
            0,
            0});
            this.registerValue.Minimum = new decimal(new int[] {
            40000,
            0,
            0,
            -2147483648});
            this.registerValue.Name = "registerValue";
            this.registerValue.Size = new System.Drawing.Size(101, 26);
            this.registerValue.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonStart.Location = new System.Drawing.Point(74, 81);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(70, 3, 70, 15);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(105, 33);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            this.buttonStart.MouseEnter += new System.EventHandler(this.buttonStart_MouseEnter);
            // 
            // DevConsole
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(258, 138);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.registerValue);
            this.Controls.Add(this.registerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "DevConsole";
            this.Text = "Tryb automatyczny";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DevConsole_FormClosing);
            this.Load += new System.EventHandler(this.DevConsole_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DevConsole_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.registerValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox registerName;
        private System.Windows.Forms.NumericUpDown registerValue;
        private System.Windows.Forms.Button buttonStart;
    }
}