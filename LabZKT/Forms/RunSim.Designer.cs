namespace LabZKT
{
    partial class RunSim
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunSim));
            this.panel_PO = new System.Windows.Forms.Panel();
            this.panel_View_PO = new System.Windows.Forms.Panel();
            this.grid_PO = new System.Windows.Forms.DataGridView();
            this.Adres = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zawartosc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenu_Main = new System.Windows.Forms.ToolStripMenuItem();
            this.nowyLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenu_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenu_Show_Log = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_Decode_PO = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel_Left = new System.Windows.Forms.Panel();
            this.panel_Sim = new System.Windows.Forms.Panel();
            this.panel_Sim_Control = new System.Windows.Forms.Panel();
            this.panel_User_Info = new System.Windows.Forms.Panel();
            this.panel_Control = new System.Windows.Forms.Panel();
            this.button_Micro = new System.Windows.Forms.Button();
            this.button_Makro = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.button_Next_Tact = new System.Windows.Forms.Button();
            this.label_Status = new System.Windows.Forms.Label();
            this.dataGridView_Info = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.richTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.panel_PM = new System.Windows.Forms.Panel();
            this.grid_PM = new System.Windows.Forms.DataGridView();
            this.addres = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.s1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.d1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.s2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.d2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.s3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.d3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.c2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Test = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ALU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_PO.SuspendLayout();
            this.panel_View_PO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_PO)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel_Decode_PO.SuspendLayout();
            this.panel_Left.SuspendLayout();
            this.panel_Sim.SuspendLayout();
            this.panel_User_Info.SuspendLayout();
            this.panel_Control.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Info)).BeginInit();
            this.panel_PM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_PM)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_PO
            // 
            this.panel_PO.AllowDrop = true;
            this.panel_PO.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel_PO.Controls.Add(this.panel_View_PO);
            this.panel_PO.Controls.Add(this.panel_Decode_PO);
            this.panel_PO.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_PO.Location = new System.Drawing.Point(768, 0);
            this.panel_PO.Name = "panel_PO";
            this.panel_PO.Size = new System.Drawing.Size(264, 721);
            this.panel_PO.TabIndex = 1;
            // 
            // panel_View_PO
            // 
            this.panel_View_PO.AutoScroll = true;
            this.panel_View_PO.Controls.Add(this.grid_PO);
            this.panel_View_PO.Controls.Add(this.menuStrip1);
            this.panel_View_PO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_View_PO.Location = new System.Drawing.Point(0, 0);
            this.panel_View_PO.Name = "panel_View_PO";
            this.panel_View_PO.Size = new System.Drawing.Size(264, 551);
            this.panel_View_PO.TabIndex = 2;
            // 
            // grid_PO
            // 
            this.grid_PO.AllowUserToAddRows = false;
            this.grid_PO.AllowUserToDeleteRows = false;
            this.grid_PO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.grid_PO.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid_PO.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_PO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid_PO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_PO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Adres,
            this.Zawartosc,
            this.Hex});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_PO.DefaultCellStyle = dataGridViewCellStyle3;
            this.grid_PO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_PO.Location = new System.Drawing.Point(0, 28);
            this.grid_PO.MultiSelect = false;
            this.grid_PO.Name = "grid_PO";
            this.grid_PO.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid_PO.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grid_PO.RowHeadersVisible = false;
            this.grid_PO.RowTemplate.Height = 24;
            this.grid_PO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_PO.Size = new System.Drawing.Size(264, 523);
            this.grid_PO.TabIndex = 0;
            this.grid_PO.SelectionChanged += new System.EventHandler(this.grid_PO_SelectionChanged);
            this.grid_PO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_PO_KeyDown);
            // 
            // Adres
            // 
            this.Adres.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Adres.DefaultCellStyle = dataGridViewCellStyle2;
            this.Adres.HeaderText = "Adres";
            this.Adres.MinimumWidth = 38;
            this.Adres.Name = "Adres";
            this.Adres.ReadOnly = true;
            this.Adres.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Adres.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Adres.Width = 38;
            // 
            // Zawartosc
            // 
            this.Zawartosc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Zawartosc.FillWeight = 40F;
            this.Zawartosc.HeaderText = "Zawartość";
            this.Zawartosc.MinimumWidth = 45;
            this.Zawartosc.Name = "Zawartosc";
            this.Zawartosc.ReadOnly = true;
            this.Zawartosc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Hex
            // 
            this.Hex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Hex.FillWeight = 45F;
            this.Hex.HeaderText = "Hex";
            this.Hex.MinimumWidth = 50;
            this.Hex.Name = "Hex";
            this.Hex.ReadOnly = true;
            this.Hex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Hex.Width = 50;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenu_Main});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(264, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenu_Main
            // 
            this.toolStripMenu_Main.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStripMenu_Main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.toolStripMenu_Main.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenu_Edit,
            this.toolStripMenu_Clear,
            this.toolStripMenuItem1,
            this.nowyLogToolStripMenuItem,
            this.toolStripMenu_Show_Log,
            this.toolStripMenuItem2,
            this.toolStripMenu_Exit});
            this.toolStripMenu_Main.Name = "toolStripMenu_Main";
            this.toolStripMenu_Main.Size = new System.Drawing.Size(137, 24);
            this.toolStripMenu_Main.Text = "Menu Symulatora";
            // 
            // nowyLogToolStripMenuItem
            // 
            this.nowyLogToolStripMenuItem.Name = "nowyLogToolStripMenuItem";
            this.nowyLogToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.nowyLogToolStripMenuItem.Text = "Nowy log";
            this.nowyLogToolStripMenuItem.Click += new System.EventHandler(this.nowyLogToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(178, 6);
            // 
            // toolStripMenu_Edit
            // 
            this.toolStripMenu_Edit.Name = "toolStripMenu_Edit";
            this.toolStripMenu_Edit.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenu_Edit.Text = "Edytuj rejestry";
            this.toolStripMenu_Edit.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // toolStripMenu_Clear
            // 
            this.toolStripMenu_Clear.Name = "toolStripMenu_Clear";
            this.toolStripMenu_Clear.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenu_Clear.Text = "Zeruj rejestry";
            this.toolStripMenu_Clear.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(178, 6);
            // 
            // toolStripMenu_Show_Log
            // 
            this.toolStripMenu_Show_Log.Name = "toolStripMenu_Show_Log";
            this.toolStripMenu_Show_Log.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenu_Show_Log.Text = "Pokaż log";
            this.toolStripMenu_Show_Log.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // toolStripMenu_Exit
            // 
            this.toolStripMenu_Exit.Name = "toolStripMenu_Exit";
            this.toolStripMenu_Exit.Size = new System.Drawing.Size(181, 26);
            this.toolStripMenu_Exit.Text = "Zakończ";
            this.toolStripMenu_Exit.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // panel_Decode_PO
            // 
            this.panel_Decode_PO.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Decode_PO.Controls.Add(this.label1);
            this.panel_Decode_PO.Cursor = System.Windows.Forms.Cursors.No;
            this.panel_Decode_PO.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Decode_PO.Location = new System.Drawing.Point(0, 551);
            this.panel_Decode_PO.Name = "panel_Decode_PO";
            this.panel_Decode_PO.Size = new System.Drawing.Size(264, 170);
            this.panel_Decode_PO.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 170);
            this.label1.TabIndex = 0;
            // 
            // panel_Left
            // 
            this.panel_Left.AllowDrop = true;
            this.panel_Left.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel_Left.Controls.Add(this.panel_Sim);
            this.panel_Left.Controls.Add(this.panel_PM);
            this.panel_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_Left.Location = new System.Drawing.Point(0, 0);
            this.panel_Left.Name = "panel_Left";
            this.panel_Left.Size = new System.Drawing.Size(762, 721);
            this.panel_Left.TabIndex = 3;
            // 
            // panel_Sim
            // 
            this.panel_Sim.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel_Sim.Controls.Add(this.panel_Sim_Control);
            this.panel_Sim.Controls.Add(this.panel_User_Info);
            this.panel_Sim.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Sim.Location = new System.Drawing.Point(0, 258);
            this.panel_Sim.Name = "panel_Sim";
            this.panel_Sim.Size = new System.Drawing.Size(762, 463);
            this.panel_Sim.TabIndex = 1;
            // 
            // panel_Sim_Control
            // 
            this.panel_Sim_Control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(96)))), ((int)(((byte)(51)))));
            this.panel_Sim_Control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Sim_Control.Location = new System.Drawing.Point(0, 125);
            this.panel_Sim_Control.Name = "panel_Sim_Control";
            this.panel_Sim_Control.Size = new System.Drawing.Size(762, 338);
            this.panel_Sim_Control.TabIndex = 1;
            // 
            // panel_User_Info
            // 
            this.panel_User_Info.Controls.Add(this.panel_Control);
            this.panel_User_Info.Controls.Add(this.dataGridView_Info);
            this.panel_User_Info.Controls.Add(this.richTextBox_Log);
            this.panel_User_Info.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel_User_Info.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_User_Info.Location = new System.Drawing.Point(0, 0);
            this.panel_User_Info.Name = "panel_User_Info";
            this.panel_User_Info.Size = new System.Drawing.Size(762, 125);
            this.panel_User_Info.TabIndex = 0;
            // 
            // panel_Control
            // 
            this.panel_Control.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Control.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Control.Controls.Add(this.button_Micro);
            this.panel_Control.Controls.Add(this.button_Makro);
            this.panel_Control.Controls.Add(this.button_OK);
            this.panel_Control.Controls.Add(this.button_Next_Tact);
            this.panel_Control.Controls.Add(this.label_Status);
            this.panel_Control.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Control.Location = new System.Drawing.Point(430, 0);
            this.panel_Control.Name = "panel_Control";
            this.panel_Control.Size = new System.Drawing.Size(199, 125);
            this.panel_Control.TabIndex = 2;
            // 
            // button_Micro
            // 
            this.button_Micro.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Micro.Location = new System.Drawing.Point(28, 51);
            this.button_Micro.Name = "button_Micro";
            this.button_Micro.Size = new System.Drawing.Size(155, 34);
            this.button_Micro.TabIndex = 4;
            this.button_Micro.Text = "Micro";
            this.button_Micro.UseVisualStyleBackColor = true;
            this.button_Micro.Click += new System.EventHandler(this.button_Micro_Click);
            // 
            // button_Makro
            // 
            this.button_Makro.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Makro.Location = new System.Drawing.Point(28, 13);
            this.button_Makro.Name = "button_Makro";
            this.button_Makro.Size = new System.Drawing.Size(155, 34);
            this.button_Makro.TabIndex = 3;
            this.button_Makro.Text = "Makro";
            this.button_Makro.UseVisualStyleBackColor = true;
            this.button_Makro.Click += new System.EventHandler(this.button_Makro_Click);
            // 
            // button_OK
            // 
            this.button_OK.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_OK.Location = new System.Drawing.Point(28, 13);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(155, 34);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "Zatwierdź";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Visible = false;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // button_Next_Tact
            // 
            this.button_Next_Tact.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Next_Tact.Location = new System.Drawing.Point(28, 51);
            this.button_Next_Tact.Name = "button_Next_Tact";
            this.button_Next_Tact.Size = new System.Drawing.Size(155, 34);
            this.button_Next_Tact.TabIndex = 1;
            this.button_Next_Tact.Text = "Następny takt";
            this.button_Next_Tact.UseVisualStyleBackColor = true;
            this.button_Next_Tact.Visible = false;
            this.button_Next_Tact.Click += new System.EventHandler(this.button_Next_Tact_Click);
            // 
            // label_Status
            // 
            this.label_Status.AutoSize = true;
            this.label_Status.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_Status.ForeColor = System.Drawing.Color.Green;
            this.label_Status.Location = new System.Drawing.Point(6, 88);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(60, 29);
            this.label_Status.TabIndex = 0;
            this.label_Status.Text = "Stop";
            // 
            // dataGridView_Info
            // 
            this.dataGridView_Info.AllowUserToAddRows = false;
            this.dataGridView_Info.AllowUserToDeleteRows = false;
            this.dataGridView_Info.AllowUserToResizeColumns = false;
            this.dataGridView_Info.AllowUserToResizeRows = false;
            this.dataGridView_Info.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_Info.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Info.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_Info.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Info.ColumnHeadersVisible = false;
            this.dataGridView_Info.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridView_Info.Cursor = System.Windows.Forms.Cursors.No;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Info.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView_Info.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridView_Info.Location = new System.Drawing.Point(629, 0);
            this.dataGridView_Info.MultiSelect = false;
            this.dataGridView_Info.Name = "dataGridView_Info";
            this.dataGridView_Info.ReadOnly = true;
            this.dataGridView_Info.RowHeadersVisible = false;
            this.dataGridView_Info.RowTemplate.Height = 25;
            this.dataGridView_Info.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView_Info.Size = new System.Drawing.Size(133, 125);
            this.dataGridView_Info.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.MinimumWidth = 30;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 30;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // richTextBox_Log
            // 
            this.richTextBox_Log.Cursor = System.Windows.Forms.Cursors.No;
            this.richTextBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_Log.Enabled = false;
            this.richTextBox_Log.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBox_Log.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_Log.Name = "richTextBox_Log";
            this.richTextBox_Log.Size = new System.Drawing.Size(762, 125);
            this.richTextBox_Log.TabIndex = 0;
            this.richTextBox_Log.Text = "";
            // 
            // panel_PM
            // 
            this.panel_PM.AutoSize = true;
            this.panel_PM.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel_PM.Controls.Add(this.grid_PM);
            this.panel_PM.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_PM.Location = new System.Drawing.Point(0, 0);
            this.panel_PM.MinimumSize = new System.Drawing.Size(0, 200);
            this.panel_PM.Name = "panel_PM";
            this.panel_PM.Size = new System.Drawing.Size(762, 200);
            this.panel_PM.TabIndex = 0;
            // 
            // grid_PM
            // 
            this.grid_PM.AllowUserToAddRows = false;
            this.grid_PM.AllowUserToDeleteRows = false;
            this.grid_PM.AllowUserToResizeColumns = false;
            this.grid_PM.AllowUserToResizeRows = false;
            this.grid_PM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_PM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid_PM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_PM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.addres,
            this.s1,
            this.d1,
            this.s2,
            this.d2,
            this.s3,
            this.d3,
            this.c1,
            this.c2,
            this.Test,
            this.ALU,
            this.NA});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid_PM.DefaultCellStyle = dataGridViewCellStyle8;
            this.grid_PM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_PM.Location = new System.Drawing.Point(0, 0);
            this.grid_PM.MultiSelect = false;
            this.grid_PM.Name = "grid_PM";
            this.grid_PM.ReadOnly = true;
            this.grid_PM.RowHeadersVisible = false;
            this.grid_PM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grid_PM.RowTemplate.Height = 24;
            this.grid_PM.Size = new System.Drawing.Size(762, 200);
            this.grid_PM.TabIndex = 2;
            this.grid_PM.SelectionChanged += new System.EventHandler(this.grid_PM_SelectionChanged);
            // 
            // addres
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addres.DefaultCellStyle = dataGridViewCellStyle7;
            this.addres.FillWeight = 40F;
            this.addres.HeaderText = "Adres";
            this.addres.MinimumWidth = 40;
            this.addres.Name = "addres";
            this.addres.ReadOnly = true;
            this.addres.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.addres.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // s1
            // 
            this.s1.FillWeight = 40F;
            this.s1.HeaderText = "S1";
            this.s1.MinimumWidth = 40;
            this.s1.Name = "s1";
            this.s1.ReadOnly = true;
            this.s1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.s1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // d1
            // 
            this.d1.FillWeight = 40F;
            this.d1.HeaderText = "D1";
            this.d1.MinimumWidth = 40;
            this.d1.Name = "d1";
            this.d1.ReadOnly = true;
            this.d1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.d1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // s2
            // 
            this.s2.FillWeight = 40F;
            this.s2.HeaderText = "S2";
            this.s2.MinimumWidth = 40;
            this.s2.Name = "s2";
            this.s2.ReadOnly = true;
            this.s2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.s2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // d2
            // 
            this.d2.FillWeight = 40F;
            this.d2.HeaderText = "D2";
            this.d2.MinimumWidth = 40;
            this.d2.Name = "d2";
            this.d2.ReadOnly = true;
            this.d2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.d2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // s3
            // 
            this.s3.FillWeight = 40F;
            this.s3.HeaderText = "S3";
            this.s3.MinimumWidth = 40;
            this.s3.Name = "s3";
            this.s3.ReadOnly = true;
            this.s3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.s3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // d3
            // 
            this.d3.FillWeight = 40F;
            this.d3.HeaderText = "D3";
            this.d3.MinimumWidth = 40;
            this.d3.Name = "d3";
            this.d3.ReadOnly = true;
            this.d3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.d3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // c1
            // 
            this.c1.FillWeight = 40F;
            this.c1.HeaderText = "C1";
            this.c1.MinimumWidth = 40;
            this.c1.Name = "c1";
            this.c1.ReadOnly = true;
            this.c1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // c2
            // 
            this.c2.FillWeight = 40F;
            this.c2.HeaderText = "C2";
            this.c2.MinimumWidth = 40;
            this.c2.Name = "c2";
            this.c2.ReadOnly = true;
            this.c2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Test
            // 
            this.Test.FillWeight = 40F;
            this.Test.HeaderText = "Test";
            this.Test.MinimumWidth = 40;
            this.Test.Name = "Test";
            this.Test.ReadOnly = true;
            this.Test.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Test.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ALU
            // 
            this.ALU.FillWeight = 40F;
            this.ALU.HeaderText = "ALU";
            this.ALU.MinimumWidth = 40;
            this.ALU.Name = "ALU";
            this.ALU.ReadOnly = true;
            this.ALU.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ALU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // NA
            // 
            this.NA.FillWeight = 40F;
            this.NA.HeaderText = "NA";
            this.NA.MinimumWidth = 40;
            this.NA.Name = "NA";
            this.NA.ReadOnly = true;
            this.NA.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RunSim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1032, 721);
            this.Controls.Add(this.panel_Left);
            this.Controls.Add(this.panel_PO);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1050, 760);
            this.Name = "RunSim";
            this.Text = "Praca Procesora";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RunSim_FormClosing);
            this.Load += new System.EventHandler(this.RunSim_Load);
            this.ResizeEnd += new System.EventHandler(this.RunSim_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.RunSim_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RunSim_KeyPress);
            this.panel_PO.ResumeLayout(false);
            this.panel_View_PO.ResumeLayout(false);
            this.panel_View_PO.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_PO)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel_Decode_PO.ResumeLayout(false);
            this.panel_Left.ResumeLayout(false);
            this.panel_Left.PerformLayout();
            this.panel_Sim.ResumeLayout(false);
            this.panel_User_Info.ResumeLayout(false);
            this.panel_Control.ResumeLayout(false);
            this.panel_Control.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Info)).EndInit();
            this.panel_PM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_PM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel_PO;
        private System.Windows.Forms.Panel panel_Left;
        private System.Windows.Forms.Panel panel_Sim;
        private System.Windows.Forms.Panel panel_PM;
        private System.Windows.Forms.DataGridView grid_PM;
        private System.Windows.Forms.Panel panel_Decode_PO;
        private System.Windows.Forms.Panel panel_View_PO;
        private System.Windows.Forms.DataGridView grid_PO;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Main;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Edit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Show_Log;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenu_Exit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Adres;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zawartosc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hex;
        private System.Windows.Forms.Panel panel_User_Info;
        private System.Windows.Forms.Panel panel_Sim_Control;
        private System.Windows.Forms.DataGridView dataGridView_Info;
        private System.Windows.Forms.RichTextBox richTextBox_Log;
        private System.Windows.Forms.Panel panel_Control;
        private System.Windows.Forms.Button button_OK;
        private System.Windows.Forms.Button button_Next_Tact;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn addres;
        private System.Windows.Forms.DataGridViewTextBoxColumn s1;
        private System.Windows.Forms.DataGridViewTextBoxColumn d1;
        private System.Windows.Forms.DataGridViewTextBoxColumn s2;
        private System.Windows.Forms.DataGridViewTextBoxColumn d2;
        private System.Windows.Forms.DataGridViewTextBoxColumn s3;
        private System.Windows.Forms.DataGridViewTextBoxColumn d3;
        private System.Windows.Forms.DataGridViewTextBoxColumn c1;
        private System.Windows.Forms.DataGridViewTextBoxColumn c2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Test;
        private System.Windows.Forms.DataGridViewTextBoxColumn ALU;
        private System.Windows.Forms.DataGridViewTextBoxColumn NA;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button button_Micro;
        private System.Windows.Forms.Button button_Makro;
        private System.Windows.Forms.ToolStripMenuItem nowyLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}