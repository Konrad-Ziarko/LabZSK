namespace LabZKT
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.button_PM = new System.Windows.Forms.Button();
            this.button_PO = new System.Windows.Forms.Button();
            this.button_Run = new System.Windows.Forms.Button();
            this.button_Author = new System.Windows.Forms.Button();
            this.button_Close = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_nightMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_PM
            // 
            this.button_PM.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button_PM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_PM.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_PM.Location = new System.Drawing.Point(12, 60);
            this.button_PM.Name = "button_PM";
            this.button_PM.Size = new System.Drawing.Size(246, 72);
            this.button_PM.TabIndex = 0;
            this.button_PM.Text = "Pamięć Mikroprogramu";
            this.button_PM.UseVisualStyleBackColor = false;
            this.button_PM.Click += new System.EventHandler(this.button_PM_Click);
            // 
            // button_PO
            // 
            this.button_PO.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button_PO.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_PO.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_PO.Location = new System.Drawing.Point(504, 60);
            this.button_PO.Name = "button_PO";
            this.button_PO.Size = new System.Drawing.Size(246, 72);
            this.button_PO.TabIndex = 1;
            this.button_PO.Text = "Pamięć Operacyjna";
            this.button_PO.UseVisualStyleBackColor = false;
            this.button_PO.Click += new System.EventHandler(this.button_PO_Click);
            // 
            // button_Run
            // 
            this.button_Run.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Run.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Run.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Run.Location = new System.Drawing.Point(267, 180);
            this.button_Run.Name = "button_Run";
            this.button_Run.Size = new System.Drawing.Size(246, 72);
            this.button_Run.TabIndex = 2;
            this.button_Run.Text = "Uruchom Symulcję";
            this.button_Run.UseVisualStyleBackColor = false;
            this.button_Run.Click += new System.EventHandler(this.button_Run_Click);
            // 
            // button_Author
            // 
            this.button_Author.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button_Author.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Author.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Author.Location = new System.Drawing.Point(267, 314);
            this.button_Author.Name = "button_Author";
            this.button_Author.Size = new System.Drawing.Size(246, 48);
            this.button_Author.TabIndex = 3;
            this.button_Author.Text = "Autor";
            this.button_Author.UseVisualStyleBackColor = false;
            this.button_Author.Click += new System.EventHandler(this.button_Author_Click);
            // 
            // button_Close
            // 
            this.button_Close.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.button_Close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Close.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Close.ForeColor = System.Drawing.Color.Red;
            this.button_Close.Location = new System.Drawing.Point(566, 413);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(121, 48);
            this.button_Close.TabIndex = 4;
            this.button_Close.Text = "Zamknij";
            this.button_Close.UseVisualStyleBackColor = false;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_nightMode
            // 
            this.button_nightMode.BackgroundImage = global::LabZKT.Properties.Resources.moon;
            this.button_nightMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_nightMode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_nightMode.Location = new System.Drawing.Point(13, 427);
            this.button_nightMode.Name = "button_nightMode";
            this.button_nightMode.Size = new System.Drawing.Size(40, 40);
            this.button_nightMode.TabIndex = 5;
            this.button_nightMode.UseVisualStyleBackColor = true;
            this.button_nightMode.Click += new System.EventHandler(this.button_nightMode_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(762, 473);
            this.Controls.Add(this.button_nightMode);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.button_Author);
            this.Controls.Add(this.button_Run);
            this.Controls.Add(this.button_PO);
            this.Controls.Add(this.button_PM);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(780, 520);
            this.MinimumSize = new System.Drawing.Size(780, 520);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LabZKT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_PM;
        private System.Windows.Forms.Button button_PO;
        private System.Windows.Forms.Button button_Run;
        private System.Windows.Forms.Button button_Author;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_nightMode;
    }
}

