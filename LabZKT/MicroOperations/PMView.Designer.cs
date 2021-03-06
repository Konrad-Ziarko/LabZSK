﻿namespace LabZSK.MicroOperations
{
    partial class PMView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PMView));
            this.panel_View_PM = new System.Windows.Forms.Panel();
            this.Grid_PM = new System.Windows.Forms.DataGridView();
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
            this.panel_Control = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button_Exit = new System.Windows.Forms.Button();
            this.button_Edit = new System.Windows.Forms.Button();
            this.button_Load_Table = new System.Windows.Forms.Button();
            this.button_Save_Table = new System.Windows.Forms.Button();
            this.button_Clear_Table = new System.Windows.Forms.Button();
            this.button_Clear_Row = new System.Windows.Forms.Button();
            this.open_File_Dialog = new System.Windows.Forms.OpenFileDialog();
            this.save_File_Dialog = new System.Windows.Forms.SaveFileDialog();
            this.panel_View_PM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PM)).BeginInit();
            this.panel_Control.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_View_PM
            // 
            this.panel_View_PM.Controls.Add(this.Grid_PM);
            this.panel_View_PM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_View_PM.Location = new System.Drawing.Point(0, 0);
            this.panel_View_PM.Margin = new System.Windows.Forms.Padding(2);
            this.panel_View_PM.Name = "panel_View_PM";
            this.panel_View_PM.Size = new System.Drawing.Size(589, 605);
            this.panel_View_PM.TabIndex = 4;
            // 
            // Grid_PM
            // 
            this.Grid_PM.AllowDrop = true;
            this.Grid_PM.AllowUserToAddRows = false;
            this.Grid_PM.AllowUserToDeleteRows = false;
            this.Grid_PM.AllowUserToResizeColumns = false;
            this.Grid_PM.AllowUserToResizeRows = false;
            this.Grid_PM.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid_PM.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Grid_PM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_PM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid_PM.DefaultCellStyle = dataGridViewCellStyle3;
            this.Grid_PM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_PM.Location = new System.Drawing.Point(0, 0);
            this.Grid_PM.Margin = new System.Windows.Forms.Padding(2);
            this.Grid_PM.MultiSelect = false;
            this.Grid_PM.Name = "Grid_PM";
            this.Grid_PM.RowHeadersVisible = false;
            this.Grid_PM.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Grid_PM.RowTemplate.Height = 24;
            this.Grid_PM.Size = new System.Drawing.Size(589, 605);
            this.Grid_PM.TabIndex = 3;
            this.Grid_PM.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_PM_CellEndEdit);
            this.Grid_PM.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_PM_CellLeave);
            this.Grid_PM.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_PM_CellMouseDoubleClick);
            this.Grid_PM.DragDrop += new System.Windows.Forms.DragEventHandler(this.grid_PM_DragDrop);
            this.Grid_PM.DragEnter += new System.Windows.Forms.DragEventHandler(this.grid_PM_DragEnter);
            this.Grid_PM.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_PM_KeyDown);
            this.Grid_PM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grid_PM_MouseDown);
            this.Grid_PM.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grid_PM_MouseMove);
            // 
            // addres
            // 
            this.addres.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addres.DefaultCellStyle = dataGridViewCellStyle2;
            this.addres.FillWeight = 45F;
            this.addres.Frozen = true;
            this.addres.HeaderText = "Adres";
            this.addres.MinimumWidth = 50;
            this.addres.Name = "addres";
            this.addres.ReadOnly = true;
            this.addres.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.addres.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.addres.Width = 51;
            // 
            // s1
            // 
            this.s1.FillWeight = 55.3761F;
            this.s1.HeaderText = "S1";
            this.s1.MinimumWidth = 50;
            this.s1.Name = "s1";
            this.s1.ReadOnly = true;
            this.s1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.s1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // d1
            // 
            this.d1.FillWeight = 55.3761F;
            this.d1.HeaderText = "D1";
            this.d1.MinimumWidth = 50;
            this.d1.Name = "d1";
            this.d1.ReadOnly = true;
            this.d1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.d1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // s2
            // 
            this.s2.FillWeight = 55.3761F;
            this.s2.HeaderText = "S2";
            this.s2.MinimumWidth = 50;
            this.s2.Name = "s2";
            this.s2.ReadOnly = true;
            this.s2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.s2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // d2
            // 
            this.d2.FillWeight = 55.3761F;
            this.d2.HeaderText = "D2";
            this.d2.MinimumWidth = 50;
            this.d2.Name = "d2";
            this.d2.ReadOnly = true;
            this.d2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.d2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // s3
            // 
            this.s3.FillWeight = 55.3761F;
            this.s3.HeaderText = "S3";
            this.s3.MinimumWidth = 50;
            this.s3.Name = "s3";
            this.s3.ReadOnly = true;
            this.s3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.s3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // d3
            // 
            this.d3.FillWeight = 55.3761F;
            this.d3.HeaderText = "D3";
            this.d3.MinimumWidth = 50;
            this.d3.Name = "d3";
            this.d3.ReadOnly = true;
            this.d3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.d3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // c1
            // 
            this.c1.FillWeight = 55.3761F;
            this.c1.HeaderText = "C1";
            this.c1.MinimumWidth = 50;
            this.c1.Name = "c1";
            this.c1.ReadOnly = true;
            this.c1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // c2
            // 
            this.c2.FillWeight = 55.3761F;
            this.c2.HeaderText = "C2";
            this.c2.MinimumWidth = 50;
            this.c2.Name = "c2";
            this.c2.ReadOnly = true;
            this.c2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.c2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Test
            // 
            this.Test.FillWeight = 55.3761F;
            this.Test.HeaderText = "Test";
            this.Test.MinimumWidth = 50;
            this.Test.Name = "Test";
            this.Test.ReadOnly = true;
            this.Test.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Test.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ALU
            // 
            this.ALU.FillWeight = 55.3761F;
            this.ALU.HeaderText = "ALU";
            this.ALU.MinimumWidth = 50;
            this.ALU.Name = "ALU";
            this.ALU.ReadOnly = true;
            this.ALU.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ALU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // NA
            // 
            this.NA.FillWeight = 55.3761F;
            this.NA.HeaderText = "NA";
            this.NA.MinimumWidth = 50;
            this.NA.Name = "NA";
            this.NA.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel_Control
            // 
            this.panel_Control.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Control.Controls.Add(this.button1);
            this.panel_Control.Controls.Add(this.button_Exit);
            this.panel_Control.Controls.Add(this.button_Edit);
            this.panel_Control.Controls.Add(this.button_Load_Table);
            this.panel_Control.Controls.Add(this.button_Save_Table);
            this.panel_Control.Controls.Add(this.button_Clear_Table);
            this.panel_Control.Controls.Add(this.button_Clear_Row);
            this.panel_Control.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Control.Location = new System.Drawing.Point(589, 0);
            this.panel_Control.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Control.Name = "panel_Control";
            this.panel_Control.Size = new System.Drawing.Size(143, 605);
            this.panel_Control.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(7, 238);
            this.button1.Margin = new System.Windows.Forms.Padding(7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 63);
            this.button1.TabIndex = 6;
            this.button1.Text = "Drukuj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_Exit
            // 
            this.button_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Exit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button_Exit.FlatAppearance.BorderSize = 3;
            this.button_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Exit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Exit.Location = new System.Drawing.Point(7, 524);
            this.button_Exit.Margin = new System.Windows.Forms.Padding(7);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(125, 63);
            this.button_Exit.TabIndex = 5;
            this.button_Exit.Text = "Exit";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_Edit.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.button_Edit.FlatAppearance.BorderSize = 2;
            this.button_Edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Edit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Edit.Location = new System.Drawing.Point(7, 161);
            this.button_Edit.Margin = new System.Windows.Forms.Padding(7);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(125, 63);
            this.button_Edit.TabIndex = 4;
            this.button_Edit.Text = "Edytuj";
            this.button_Edit.UseVisualStyleBackColor = true;
            this.button_Edit.Click += new System.EventHandler(this.button_Edit_Click);
            // 
            // button_Load_Table
            // 
            this.button_Load_Table.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Load_Table.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.button_Load_Table.FlatAppearance.BorderSize = 2;
            this.button_Load_Table.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Load_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Load_Table.Location = new System.Drawing.Point(7, 7);
            this.button_Load_Table.Margin = new System.Windows.Forms.Padding(7);
            this.button_Load_Table.Name = "button_Load_Table";
            this.button_Load_Table.Size = new System.Drawing.Size(125, 63);
            this.button_Load_Table.TabIndex = 3;
            this.button_Load_Table.Text = "Wczytaj pamięć";
            this.button_Load_Table.UseVisualStyleBackColor = true;
            this.button_Load_Table.Click += new System.EventHandler(this.button_Load_Table_Click);
            // 
            // button_Save_Table
            // 
            this.button_Save_Table.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_Save_Table.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.button_Save_Table.FlatAppearance.BorderSize = 2;
            this.button_Save_Table.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Save_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Save_Table.Location = new System.Drawing.Point(7, 84);
            this.button_Save_Table.Margin = new System.Windows.Forms.Padding(7);
            this.button_Save_Table.Name = "button_Save_Table";
            this.button_Save_Table.Size = new System.Drawing.Size(125, 63);
            this.button_Save_Table.TabIndex = 2;
            this.button_Save_Table.Text = "Zapisz pamięć";
            this.button_Save_Table.UseVisualStyleBackColor = true;
            this.button_Save_Table.Click += new System.EventHandler(this.button_Save_Table_Click);
            // 
            // button_Clear_Table
            // 
            this.button_Clear_Table.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Clear_Table.BackColor = System.Drawing.Color.Red;
            this.button_Clear_Table.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.button_Clear_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Clear_Table.ForeColor = System.Drawing.SystemColors.Window;
            this.button_Clear_Table.Location = new System.Drawing.Point(7, 420);
            this.button_Clear_Table.Margin = new System.Windows.Forms.Padding(7);
            this.button_Clear_Table.Name = "button_Clear_Table";
            this.button_Clear_Table.Size = new System.Drawing.Size(125, 63);
            this.button_Clear_Table.TabIndex = 1;
            this.button_Clear_Table.Text = "Wyczyść pamięć";
            this.button_Clear_Table.UseVisualStyleBackColor = false;
            this.button_Clear_Table.Click += new System.EventHandler(this.button_Clear_Table_Click);
            // 
            // button_Clear_Row
            // 
            this.button_Clear_Row.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Clear_Row.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button_Clear_Row.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Clear_Row.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Clear_Row.Location = new System.Drawing.Point(7, 343);
            this.button_Clear_Row.Margin = new System.Windows.Forms.Padding(7);
            this.button_Clear_Row.Name = "button_Clear_Row";
            this.button_Clear_Row.Size = new System.Drawing.Size(125, 63);
            this.button_Clear_Row.TabIndex = 0;
            this.button_Clear_Row.Text = "Wyczyść wiersz";
            this.button_Clear_Row.UseVisualStyleBackColor = true;
            this.button_Clear_Row.Click += new System.EventHandler(this.button_Clear_Row_Click);
            // 
            // PMView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 605);
            this.Controls.Add(this.panel_View_PM);
            this.Controls.Add(this.panel_Control);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(394, 620);
            this.Name = "PMView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pamięć Mikroprogramów";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PM_FormClosing);
            this.Load += new System.EventHandler(this.PM_Load);
            this.SizeChanged += new System.EventHandler(this.PM_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PMView_KeyDown);
            this.panel_View_PM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_PM)).EndInit();
            this.panel_Control.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_View_PM;
        private System.Windows.Forms.DataGridView Grid_PM;
        private System.Windows.Forms.Panel panel_Control;
        private System.Windows.Forms.Button button_Edit;
        private System.Windows.Forms.Button button_Load_Table;
        private System.Windows.Forms.Button button_Save_Table;
        private System.Windows.Forms.Button button_Clear_Table;
        private System.Windows.Forms.Button button_Clear_Row;
        private System.Windows.Forms.OpenFileDialog open_File_Dialog;
        private System.Windows.Forms.SaveFileDialog save_File_Dialog;
        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.Button button1;
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
    }
}