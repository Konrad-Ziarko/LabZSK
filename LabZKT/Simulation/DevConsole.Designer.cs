﻿namespace LabZKT.Simulation
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
            this.registerName = new System.Windows.Forms.ComboBox();
            this.registerValue = new System.Windows.Forms.NumericUpDown();
            this.buttonStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.registerValue)).BeginInit();
            this.SuspendLayout();
            // 
            // registerName
            // 
            this.registerName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.registerName.FormattingEnabled = true;
            this.registerName.Items.AddRange(new object[] {
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
            this.registerName.Location = new System.Drawing.Point(12, 12);
            this.registerName.Name = "registerName";
            this.registerName.Size = new System.Drawing.Size(121, 26);
            this.registerName.TabIndex = 0;
            // 
            // registerValue
            // 
            this.registerValue.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.registerValue.Location = new System.Drawing.Point(170, 12);
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
            this.registerValue.Size = new System.Drawing.Size(89, 26);
            this.registerValue.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonStart.Location = new System.Drawing.Point(84, 83);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(105, 33);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // DevConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 142);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.registerValue);
            this.Controls.Add(this.registerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DevConsole";
            this.Text = "DevConsole";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DevConsole_FormClosing);
            this.Load += new System.EventHandler(this.DevConsole_Load);
            ((System.ComponentModel.ISupportInitialize)(this.registerValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox registerName;
        private System.Windows.Forms.NumericUpDown registerValue;
        private System.Windows.Forms.Button buttonStart;
    }
}