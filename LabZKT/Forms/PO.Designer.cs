﻿namespace LabZKT
{
    partial class PO
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PO));
            this.panel_View_PO = new System.Windows.Forms.Panel();
            this.Grid_PO = new System.Windows.Forms.DataGridView();
            this.Adres = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zawartosc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Typ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Edit_PO = new System.Windows.Forms.Panel();
            this.dataGridView_Decode_Complex = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_Decode_Simple = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.button_Close = new System.Windows.Forms.Button();
            this.button_Load_Table = new System.Windows.Forms.Button();
            this.button_Save_Table = new System.Windows.Forms.Button();
            this.button_Clear_Table = new System.Windows.Forms.Button();
            this.button_Clear_Row = new System.Windows.Forms.Button();
            this.panel_Left = new System.Windows.Forms.Panel();
            this.panel_Top = new System.Windows.Forms.Panel();
            this.dataGridView_Basic = new System.Windows.Forms.DataGridView();
            this.Nazwa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.save_File_Dialog = new System.Windows.Forms.SaveFileDialog();
            this.open_File_Dialog = new System.Windows.Forms.OpenFileDialog();
            this.panel_View_PO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).BeginInit();
            this.panel_Edit_PO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Decode_Complex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Decode_Simple)).BeginInit();
            this.panel_Right.SuspendLayout();
            this.panel_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Basic)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_View_PO
            // 
            this.panel_View_PO.AutoSize = true;
            this.panel_View_PO.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_View_PO.Controls.Add(this.Grid_PO);
            this.panel_View_PO.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_View_PO.Location = new System.Drawing.Point(0, 0);
            this.panel_View_PO.MinimumSize = new System.Drawing.Size(260, 0);
            this.panel_View_PO.Name = "panel_View_PO";
            this.panel_View_PO.Size = new System.Drawing.Size(271, 721);
            this.panel_View_PO.TabIndex = 0;
            // 
            // Grid_PO
            // 
            this.Grid_PO.AllowDrop = true;
            this.Grid_PO.AllowUserToAddRows = false;
            this.Grid_PO.AllowUserToDeleteRows = false;
            this.Grid_PO.AllowUserToResizeColumns = false;
            this.Grid_PO.AllowUserToResizeRows = false;
            this.Grid_PO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Grid_PO.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Adres,
            this.Zawartosc,
            this.Hex,
            this.Typ});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid_PO.DefaultCellStyle = dataGridViewCellStyle4;
            this.Grid_PO.Dock = System.Windows.Forms.DockStyle.Left;
            this.Grid_PO.Location = new System.Drawing.Point(0, 0);
            this.Grid_PO.MinimumSize = new System.Drawing.Size(260, 0);
            this.Grid_PO.MultiSelect = false;
            this.Grid_PO.Name = "Grid_PO";
            this.Grid_PO.ReadOnly = true;
            this.Grid_PO.RowHeadersVisible = false;
            this.Grid_PO.RowTemplate.Height = 24;
            this.Grid_PO.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Grid_PO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid_PO.Size = new System.Drawing.Size(271, 721);
            this.Grid_PO.TabIndex = 0;
            this.Grid_PO.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_PO_CellMouseClick);
            this.Grid_PO.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_PO_CellMouseDoubleClick);
            this.Grid_PO.SelectionChanged += new System.EventHandler(this.Grid_PO_SelectionChanged);
            this.Grid_PO.DragDrop += new System.Windows.Forms.DragEventHandler(this.Grid_PO_DragDrop);
            this.Grid_PO.DragEnter += new System.Windows.Forms.DragEventHandler(this.Grid_PO_DragEnter);
            this.Grid_PO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_PO_KeyDown);
            this.Grid_PO.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Grid_PO_MouseDown);
            this.Grid_PO.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Grid_PO_MouseMove);
            // 
            // Adres
            // 
            this.Adres.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Adres.DefaultCellStyle = dataGridViewCellStyle1;
            this.Adres.HeaderText = "Adres";
            this.Adres.Name = "Adres";
            this.Adres.ReadOnly = true;
            this.Adres.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Adres.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Adres.Width = 40;
            // 
            // Zawartosc
            // 
            this.Zawartosc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.Zawartosc.DefaultCellStyle = dataGridViewCellStyle2;
            this.Zawartosc.HeaderText = "Zawartość";
            this.Zawartosc.Name = "Zawartosc";
            this.Zawartosc.ReadOnly = true;
            this.Zawartosc.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Zawartosc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Hex
            // 
            this.Hex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.Hex.DefaultCellStyle = dataGridViewCellStyle3;
            this.Hex.HeaderText = "Hex";
            this.Hex.MinimumWidth = 65;
            this.Hex.Name = "Hex";
            this.Hex.ReadOnly = true;
            this.Hex.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Hex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Hex.Width = 65;
            // 
            // Typ
            // 
            this.Typ.HeaderText = "Typ";
            this.Typ.Name = "Typ";
            this.Typ.ReadOnly = true;
            this.Typ.Visible = false;
            // 
            // panel_Edit_PO
            // 
            this.panel_Edit_PO.Controls.Add(this.dataGridView_Decode_Complex);
            this.panel_Edit_PO.Controls.Add(this.dataGridView_Decode_Simple);
            this.panel_Edit_PO.Controls.Add(this.panel_Right);
            this.panel_Edit_PO.Controls.Add(this.panel_Left);
            this.panel_Edit_PO.Controls.Add(this.panel_Top);
            this.panel_Edit_PO.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Edit_PO.Location = new System.Drawing.Point(277, 0);
            this.panel_Edit_PO.Name = "panel_Edit_PO";
            this.panel_Edit_PO.Size = new System.Drawing.Size(729, 721);
            this.panel_Edit_PO.TabIndex = 1;
            // 
            // dataGridView_Decode_Complex
            // 
            this.dataGridView_Decode_Complex.AllowUserToAddRows = false;
            this.dataGridView_Decode_Complex.AllowUserToDeleteRows = false;
            this.dataGridView_Decode_Complex.AllowUserToResizeColumns = false;
            this.dataGridView_Decode_Complex.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Decode_Complex.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView_Decode_Complex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_Decode_Complex.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Decode_Complex.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView_Decode_Complex.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView_Decode_Complex.EnableHeadersVisualStyles = false;
            this.dataGridView_Decode_Complex.Location = new System.Drawing.Point(2, 438);
            this.dataGridView_Decode_Complex.MultiSelect = false;
            this.dataGridView_Decode_Complex.Name = "dataGridView_Decode_Complex";
            this.dataGridView_Decode_Complex.ReadOnly = true;
            this.dataGridView_Decode_Complex.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Decode_Complex.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridView_Decode_Complex.RowHeadersVisible = false;
            this.dataGridView_Decode_Complex.RowTemplate.Height = 24;
            this.dataGridView_Decode_Complex.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dataGridView_Decode_Complex.Size = new System.Drawing.Size(529, 283);
            this.dataGridView_Decode_Complex.TabIndex = 4;
            this.dataGridView_Decode_Complex.SelectionChanged += new System.EventHandler(this.dataGridView_Decode_Complex_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn3.Frozen = true;
            this.dataGridViewTextBoxColumn3.HeaderText = "Pole";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn3.Width = 52;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 90.83263F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Hex";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn4.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 144.8276F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Dec";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.FillWeight = 64.33978F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Bin";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridView_Decode_Simple
            // 
            this.dataGridView_Decode_Simple.AllowUserToAddRows = false;
            this.dataGridView_Decode_Simple.AllowUserToDeleteRows = false;
            this.dataGridView_Decode_Simple.AllowUserToResizeColumns = false;
            this.dataGridView_Decode_Simple.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Decode_Simple.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView_Decode_Simple.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_Decode_Simple.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Dec,
            this.Bin});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Decode_Simple.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView_Decode_Simple.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView_Decode_Simple.EnableHeadersVisualStyles = false;
            this.dataGridView_Decode_Simple.Location = new System.Drawing.Point(2, 179);
            this.dataGridView_Decode_Simple.MultiSelect = false;
            this.dataGridView_Decode_Simple.Name = "dataGridView_Decode_Simple";
            this.dataGridView_Decode_Simple.ReadOnly = true;
            this.dataGridView_Decode_Simple.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Decode_Simple.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView_Decode_Simple.RowHeadersVisible = false;
            this.dataGridView_Decode_Simple.RowTemplate.Height = 24;
            this.dataGridView_Decode_Simple.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dataGridView_Decode_Simple.Size = new System.Drawing.Size(529, 283);
            this.dataGridView_Decode_Simple.TabIndex = 3;
            this.dataGridView_Decode_Simple.SelectionChanged += new System.EventHandler(this.dataGridView_Decode_Simple_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "Pole";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 52;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn1.Width = 52;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Hex";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.dataGridViewTextBoxColumn2.Width = 50;
            // 
            // Dec
            // 
            this.Dec.HeaderText = "Dec";
            this.Dec.MinimumWidth = 50;
            this.Dec.Name = "Dec";
            this.Dec.ReadOnly = true;
            this.Dec.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Dec.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.Dec.Width = 50;
            // 
            // Bin
            // 
            this.Bin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Bin.HeaderText = "Bin";
            this.Bin.MinimumWidth = 50;
            this.Bin.Name = "Bin";
            this.Bin.ReadOnly = true;
            this.Bin.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Bin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // panel_Right
            // 
            this.panel_Right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Right.Controls.Add(this.button_Close);
            this.panel_Right.Controls.Add(this.button_Load_Table);
            this.panel_Right.Controls.Add(this.button_Save_Table);
            this.panel_Right.Controls.Add(this.button_Clear_Table);
            this.panel_Right.Controls.Add(this.button_Clear_Row);
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Right.Location = new System.Drawing.Point(531, 179);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Size = new System.Drawing.Size(198, 542);
            this.panel_Right.TabIndex = 2;
            // 
            // button_Close
            // 
            this.button_Close.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Close.Location = new System.Drawing.Point(15, 411);
            this.button_Close.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(167, 78);
            this.button_Close.TabIndex = 9;
            this.button_Close.Text = "Zamknij";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button_Load_Table
            // 
            this.button_Load_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Load_Table.Location = new System.Drawing.Point(15, 315);
            this.button_Load_Table.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.button_Load_Table.Name = "button_Load_Table";
            this.button_Load_Table.Size = new System.Drawing.Size(167, 78);
            this.button_Load_Table.TabIndex = 8;
            this.button_Load_Table.Text = "Wczytaj tabelę";
            this.button_Load_Table.UseVisualStyleBackColor = true;
            this.button_Load_Table.Click += new System.EventHandler(this.button_Load_Table_Click);
            // 
            // button_Save_Table
            // 
            this.button_Save_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Save_Table.Location = new System.Drawing.Point(15, 219);
            this.button_Save_Table.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.button_Save_Table.Name = "button_Save_Table";
            this.button_Save_Table.Size = new System.Drawing.Size(167, 78);
            this.button_Save_Table.TabIndex = 7;
            this.button_Save_Table.Text = "Zapisz tabelę";
            this.button_Save_Table.UseVisualStyleBackColor = true;
            this.button_Save_Table.Click += new System.EventHandler(this.button_Save_Table_Click);
            // 
            // button_Clear_Table
            // 
            this.button_Clear_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Clear_Table.Location = new System.Drawing.Point(15, 123);
            this.button_Clear_Table.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.button_Clear_Table.Name = "button_Clear_Table";
            this.button_Clear_Table.Size = new System.Drawing.Size(167, 78);
            this.button_Clear_Table.TabIndex = 6;
            this.button_Clear_Table.Text = "Wyczyść tabelę";
            this.button_Clear_Table.UseVisualStyleBackColor = true;
            this.button_Clear_Table.Click += new System.EventHandler(this.button_Clear_Table_Click);
            // 
            // button_Clear_Row
            // 
            this.button_Clear_Row.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Clear_Row.Location = new System.Drawing.Point(15, 27);
            this.button_Clear_Row.Margin = new System.Windows.Forms.Padding(15, 9, 15, 9);
            this.button_Clear_Row.Name = "button_Clear_Row";
            this.button_Clear_Row.Size = new System.Drawing.Size(167, 78);
            this.button_Clear_Row.TabIndex = 5;
            this.button_Clear_Row.Text = "Wyczyść wiersz";
            this.button_Clear_Row.UseVisualStyleBackColor = true;
            this.button_Clear_Row.Click += new System.EventHandler(this.button_Clear_Row_Click);
            // 
            // panel_Left
            // 
            this.panel_Left.AutoSize = true;
            this.panel_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_Left.Location = new System.Drawing.Point(0, 179);
            this.panel_Left.Name = "panel_Left";
            this.panel_Left.Size = new System.Drawing.Size(2, 542);
            this.panel_Left.TabIndex = 1;
            // 
            // panel_Top
            // 
            this.panel_Top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Top.Controls.Add(this.dataGridView_Basic);
            this.panel_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(729, 179);
            this.panel_Top.TabIndex = 0;
            // 
            // dataGridView_Basic
            // 
            this.dataGridView_Basic.AllowUserToAddRows = false;
            this.dataGridView_Basic.AllowUserToDeleteRows = false;
            this.dataGridView_Basic.AllowUserToResizeColumns = false;
            this.dataGridView_Basic.AllowUserToResizeRows = false;
            this.dataGridView_Basic.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Basic.ColumnHeadersVisible = false;
            this.dataGridView_Basic.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nazwa,
            this.Property});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Basic.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridView_Basic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Basic.EnableHeadersVisualStyles = false;
            this.dataGridView_Basic.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Basic.MultiSelect = false;
            this.dataGridView_Basic.Name = "dataGridView_Basic";
            this.dataGridView_Basic.ReadOnly = true;
            this.dataGridView_Basic.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridView_Basic.RowHeadersVisible = false;
            this.dataGridView_Basic.RowTemplate.Height = 24;
            this.dataGridView_Basic.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView_Basic.Size = new System.Drawing.Size(725, 175);
            this.dataGridView_Basic.TabIndex = 0;
            this.dataGridView_Basic.SelectionChanged += new System.EventHandler(this.dataGridView_Basic_SelectionChanged);
            // 
            // Nazwa
            // 
            this.Nazwa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Nazwa.DefaultCellStyle = dataGridViewCellStyle13;
            this.Nazwa.HeaderText = "Nazwa";
            this.Nazwa.Name = "Nazwa";
            this.Nazwa.ReadOnly = true;
            this.Nazwa.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Nazwa.Width = 200;
            // 
            // Property
            // 
            this.Property.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Property.HeaderText = "Property";
            this.Property.Name = "Property";
            this.Property.ReadOnly = true;
            this.Property.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // open_File_Dialog
            // 
            this.open_File_Dialog.FileName = "open_File_Dialog";
            // 
            // PO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.panel_Edit_PO);
            this.Controls.Add(this.panel_View_PO);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 650);
            this.Name = "PO";
            this.Text = "Pamięć Operacyjna";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PO_FormClosing);
            this.Load += new System.EventHandler(this.PO_Load);
            this.ResizeEnd += new System.EventHandler(this.PO_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.PO_SizeChanged);
            this.Resize += new System.EventHandler(this.PO_Resize);
            this.panel_View_PO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PO)).EndInit();
            this.panel_Edit_PO.ResumeLayout(false);
            this.panel_Edit_PO.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Decode_Complex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Decode_Simple)).EndInit();
            this.panel_Right.ResumeLayout(false);
            this.panel_Top.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Basic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_View_PO;
        private System.Windows.Forms.Panel panel_Edit_PO;
        private System.Windows.Forms.DataGridView Grid_PO;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.SaveFileDialog save_File_Dialog;
        private System.Windows.Forms.OpenFileDialog open_File_Dialog;
        private System.Windows.Forms.Panel panel_Right;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_Load_Table;
        private System.Windows.Forms.Button button_Save_Table;
        private System.Windows.Forms.Button button_Clear_Table;
        private System.Windows.Forms.Button button_Clear_Row;
        private System.Windows.Forms.Panel panel_Left;
        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.DataGridView dataGridView_Basic;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nazwa;
        private System.Windows.Forms.DataGridViewTextBoxColumn Property;
        private System.Windows.Forms.DataGridView dataGridView_Decode_Simple;
        private System.Windows.Forms.DataGridView dataGridView_Decode_Complex;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bin;
        private System.Windows.Forms.DataGridViewTextBoxColumn Adres;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zawartosc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Typ;
    }
}