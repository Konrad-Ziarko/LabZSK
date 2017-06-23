namespace LabZSK.Memory
{
    partial class MemView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MemView));
            this.panel_View_PO = new System.Windows.Forms.Panel();
            this.Grid_Mem = new System.Windows.Forms.DataGridView();
            this.Adres = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zawartosc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Typ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Edit_PO = new System.Windows.Forms.Panel();
            this.panel_Down = new System.Windows.Forms.Panel();
            this.button_Clear_Row = new System.Windows.Forms.Button();
            this.button_Clear_Table = new System.Windows.Forms.Button();
            this.dataGridView_Decode_Complex = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button_Edit = new System.Windows.Forms.Button();
            this.button_Load_Table = new System.Windows.Forms.Button();
            this.panel_Bottom_Right = new System.Windows.Forms.Panel();
            this.button_Exit = new System.Windows.Forms.Button();
            this.button_Save_Table = new System.Windows.Forms.Button();
            this.panel_Left = new System.Windows.Forms.Panel();
            this.panel_Top = new System.Windows.Forms.Panel();
            this.dataGridView_Basic = new System.Windows.Forms.DataGridView();
            this.Nazwa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.save_File_Dialog = new System.Windows.Forms.SaveFileDialog();
            this.open_File_Dialog = new System.Windows.Forms.OpenFileDialog();
            this.panel_View_PO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Mem)).BeginInit();
            this.panel_Edit_PO.SuspendLayout();
            this.panel_Down.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Decode_Complex)).BeginInit();
            this.panel_Right.SuspendLayout();
            this.panel_Bottom_Right.SuspendLayout();
            this.panel_Top.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Basic)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_View_PO
            // 
            this.panel_View_PO.Controls.Add(this.Grid_Mem);
            this.panel_View_PO.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_View_PO.Location = new System.Drawing.Point(0, 0);
            this.panel_View_PO.Margin = new System.Windows.Forms.Padding(2);
            this.panel_View_PO.MinimumSize = new System.Drawing.Size(195, 0);
            this.panel_View_PO.Name = "panel_View_PO";
            this.panel_View_PO.Size = new System.Drawing.Size(206, 626);
            this.panel_View_PO.TabIndex = 0;
            // 
            // Grid_Mem
            // 
            this.Grid_Mem.AllowDrop = true;
            this.Grid_Mem.AllowUserToAddRows = false;
            this.Grid_Mem.AllowUserToDeleteRows = false;
            this.Grid_Mem.AllowUserToResizeColumns = false;
            this.Grid_Mem.AllowUserToResizeRows = false;
            this.Grid_Mem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Grid_Mem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Adres,
            this.Zawartosc,
            this.Hex,
            this.Typ});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid_Mem.DefaultCellStyle = dataGridViewCellStyle4;
            this.Grid_Mem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid_Mem.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.Grid_Mem.Location = new System.Drawing.Point(0, 0);
            this.Grid_Mem.Margin = new System.Windows.Forms.Padding(2);
            this.Grid_Mem.MinimumSize = new System.Drawing.Size(195, 0);
            this.Grid_Mem.MultiSelect = false;
            this.Grid_Mem.Name = "Grid_Mem";
            this.Grid_Mem.ReadOnly = true;
            this.Grid_Mem.RowHeadersVisible = false;
            this.Grid_Mem.RowTemplate.Height = 24;
            this.Grid_Mem.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Grid_Mem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid_Mem.Size = new System.Drawing.Size(206, 626);
            this.Grid_Mem.TabIndex = 0;
            this.Grid_Mem.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid_Mem_CellEndEdit);
            this.Grid_Mem.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_PO_CellMouseClick);
            this.Grid_Mem.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_PO_CellMouseDoubleClick);
            this.Grid_Mem.SelectionChanged += new System.EventHandler(this.Grid_PO_SelectionChanged);
            this.Grid_Mem.DragDrop += new System.Windows.Forms.DragEventHandler(this.Grid_PO_DragDrop);
            this.Grid_Mem.DragEnter += new System.Windows.Forms.DragEventHandler(this.Grid_PO_DragEnter);
            this.Grid_Mem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_PO_KeyDown);
            this.Grid_Mem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Grid_PO_MouseDown);
            this.Grid_Mem.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Grid_PO_MouseMove);
            this.Grid_Mem.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Grid_Mem_PreviewKeyDown);
            // 
            // Adres
            // 
            this.Adres.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
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
            this.panel_Edit_PO.Controls.Add(this.panel_Down);
            this.panel_Edit_PO.Controls.Add(this.dataGridView_Decode_Complex);
            this.panel_Edit_PO.Controls.Add(this.panel_Right);
            this.panel_Edit_PO.Controls.Add(this.panel_Left);
            this.panel_Edit_PO.Controls.Add(this.panel_Top);
            this.panel_Edit_PO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Edit_PO.Location = new System.Drawing.Point(206, 0);
            this.panel_Edit_PO.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Edit_PO.Name = "panel_Edit_PO";
            this.panel_Edit_PO.Size = new System.Drawing.Size(648, 626);
            this.panel_Edit_PO.TabIndex = 1;
            // 
            // panel_Down
            // 
            this.panel_Down.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Down.Controls.Add(this.button_Clear_Row);
            this.panel_Down.Controls.Add(this.button_Clear_Table);
            this.panel_Down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Down.Location = new System.Drawing.Point(2, 332);
            this.panel_Down.Name = "panel_Down";
            this.panel_Down.Size = new System.Drawing.Size(497, 294);
            this.panel_Down.TabIndex = 5;
            // 
            // button_Clear_Row
            // 
            this.button_Clear_Row.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button_Clear_Row.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Clear_Row.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Clear_Row.Location = new System.Drawing.Point(13, 9);
            this.button_Clear_Row.Margin = new System.Windows.Forms.Padding(11, 7, 11, 7);
            this.button_Clear_Row.Name = "button_Clear_Row";
            this.button_Clear_Row.Size = new System.Drawing.Size(125, 63);
            this.button_Clear_Row.TabIndex = 5;
            this.button_Clear_Row.Text = "Wyczyść wiersz";
            this.button_Clear_Row.UseVisualStyleBackColor = true;
            this.button_Clear_Row.Click += new System.EventHandler(this.button_Clear_Row_Click);
            // 
            // button_Clear_Table
            // 
            this.button_Clear_Table.BackColor = System.Drawing.Color.Red;
            this.button_Clear_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Clear_Table.ForeColor = System.Drawing.SystemColors.Window;
            this.button_Clear_Table.Location = new System.Drawing.Point(160, 7);
            this.button_Clear_Table.Margin = new System.Windows.Forms.Padding(11, 7, 11, 7);
            this.button_Clear_Table.Name = "button_Clear_Table";
            this.button_Clear_Table.Size = new System.Drawing.Size(125, 63);
            this.button_Clear_Table.TabIndex = 6;
            this.button_Clear_Table.Text = "Wyczyść pamięć";
            this.button_Clear_Table.UseVisualStyleBackColor = false;
            this.button_Clear_Table.Click += new System.EventHandler(this.button_Clear_Table_Click);
            // 
            // dataGridView_Decode_Complex
            // 
            this.dataGridView_Decode_Complex.AllowUserToAddRows = false;
            this.dataGridView_Decode_Complex.AllowUserToDeleteRows = false;
            this.dataGridView_Decode_Complex.AllowUserToResizeColumns = false;
            this.dataGridView_Decode_Complex.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
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
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Decode_Complex.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridView_Decode_Complex.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView_Decode_Complex.EnableHeadersVisualStyles = false;
            this.dataGridView_Decode_Complex.Location = new System.Drawing.Point(2, 205);
            this.dataGridView_Decode_Complex.Margin = new System.Windows.Forms.Padding(2);
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
            this.dataGridView_Decode_Complex.Size = new System.Drawing.Size(497, 127);
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
            // panel_Right
            // 
            this.panel_Right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Right.Controls.Add(this.button1);
            this.panel_Right.Controls.Add(this.button_Edit);
            this.panel_Right.Controls.Add(this.button_Load_Table);
            this.panel_Right.Controls.Add(this.panel_Bottom_Right);
            this.panel_Right.Controls.Add(this.button_Save_Table);
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Right.Location = new System.Drawing.Point(499, 205);
            this.panel_Right.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Size = new System.Drawing.Size(149, 421);
            this.panel_Right.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(11, 239);
            this.button1.Margin = new System.Windows.Forms.Padding(11, 7, 11, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 63);
            this.button1.TabIndex = 11;
            this.button1.Text = "Drukuj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_Edit.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.button_Edit.FlatAppearance.BorderSize = 2;
            this.button_Edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Edit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Edit.Location = new System.Drawing.Point(11, 162);
            this.button_Edit.Margin = new System.Windows.Forms.Padding(11, 7, 11, 7);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(125, 63);
            this.button_Edit.TabIndex = 9;
            this.button_Edit.Text = "Edytuj komórkę";
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
            this.button_Load_Table.Location = new System.Drawing.Point(11, 8);
            this.button_Load_Table.Margin = new System.Windows.Forms.Padding(11, 7, 11, 7);
            this.button_Load_Table.Name = "button_Load_Table";
            this.button_Load_Table.Size = new System.Drawing.Size(125, 63);
            this.button_Load_Table.TabIndex = 8;
            this.button_Load_Table.Text = "Wczytaj pamięć";
            this.button_Load_Table.UseVisualStyleBackColor = true;
            this.button_Load_Table.Click += new System.EventHandler(this.button_Load_Table_Click);
            // 
            // panel_Bottom_Right
            // 
            this.panel_Bottom_Right.Controls.Add(this.button_Exit);
            this.panel_Bottom_Right.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Bottom_Right.Location = new System.Drawing.Point(0, 288);
            this.panel_Bottom_Right.Name = "panel_Bottom_Right";
            this.panel_Bottom_Right.Size = new System.Drawing.Size(147, 131);
            this.panel_Bottom_Right.TabIndex = 10;
            // 
            // button_Exit
            // 
            this.button_Exit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Exit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button_Exit.FlatAppearance.BorderSize = 3;
            this.button_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Exit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Exit.Location = new System.Drawing.Point(11, 7);
            this.button_Exit.Margin = new System.Windows.Forms.Padding(11, 7, 11, 0);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(127, 63);
            this.button_Exit.TabIndex = 7;
            this.button_Exit.Text = "Exit";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // button_Save_Table
            // 
            this.button_Save_Table.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_Save_Table.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.button_Save_Table.FlatAppearance.BorderSize = 2;
            this.button_Save_Table.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Save_Table.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Save_Table.Location = new System.Drawing.Point(11, 85);
            this.button_Save_Table.Margin = new System.Windows.Forms.Padding(11, 7, 11, 7);
            this.button_Save_Table.Name = "button_Save_Table";
            this.button_Save_Table.Size = new System.Drawing.Size(125, 63);
            this.button_Save_Table.TabIndex = 7;
            this.button_Save_Table.Text = "Zapisz pamięć";
            this.button_Save_Table.UseVisualStyleBackColor = true;
            this.button_Save_Table.Click += new System.EventHandler(this.button_Save_Table_Click);
            // 
            // panel_Left
            // 
            this.panel_Left.AutoSize = true;
            this.panel_Left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_Left.Location = new System.Drawing.Point(0, 205);
            this.panel_Left.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Left.Name = "panel_Left";
            this.panel_Left.Size = new System.Drawing.Size(2, 421);
            this.panel_Left.TabIndex = 1;
            // 
            // panel_Top
            // 
            this.panel_Top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_Top.Controls.Add(this.dataGridView_Basic);
            this.panel_Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Top.Location = new System.Drawing.Point(0, 0);
            this.panel_Top.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(648, 205);
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
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Basic.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView_Basic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Basic.EnableHeadersVisualStyles = false;
            this.dataGridView_Basic.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Basic.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView_Basic.MultiSelect = false;
            this.dataGridView_Basic.Name = "dataGridView_Basic";
            this.dataGridView_Basic.ReadOnly = true;
            this.dataGridView_Basic.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridView_Basic.RowHeadersVisible = false;
            this.dataGridView_Basic.RowTemplate.Height = 24;
            this.dataGridView_Basic.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView_Basic.Size = new System.Drawing.Size(644, 201);
            this.dataGridView_Basic.TabIndex = 0;
            this.dataGridView_Basic.SelectionChanged += new System.EventHandler(this.dataGridView_Basic_SelectionChanged);
            // 
            // Nazwa
            // 
            this.Nazwa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Nazwa.DefaultCellStyle = dataGridViewCellStyle9;
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
            // open_File_Dialog
            // 
            this.open_File_Dialog.FileName = "open_File_Dialog";
            // 
            // MemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 626);
            this.Controls.Add(this.panel_Edit_PO);
            this.Controls.Add(this.panel_View_PO);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(730, 645);
            this.Name = "MemView";
            this.Text = "Pamięć Operacyjna";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PO_FormClosing);
            this.Load += new System.EventHandler(this.MemView_Load);
            this.Shown += new System.EventHandler(this.MemView_Shown);
            this.SizeChanged += new System.EventHandler(this.PO_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MemView_KeyDown);
            this.panel_View_PO.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Mem)).EndInit();
            this.panel_Edit_PO.ResumeLayout(false);
            this.panel_Edit_PO.PerformLayout();
            this.panel_Down.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Decode_Complex)).EndInit();
            this.panel_Right.ResumeLayout(false);
            this.panel_Bottom_Right.ResumeLayout(false);
            this.panel_Top.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Basic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_View_PO;
        private System.Windows.Forms.Panel panel_Edit_PO;
        private System.Windows.Forms.DataGridView Grid_Mem;
        private System.Windows.Forms.SaveFileDialog save_File_Dialog;
        private System.Windows.Forms.OpenFileDialog open_File_Dialog;
        private System.Windows.Forms.Button button_Edit;
        private System.Windows.Forms.Button button_Load_Table;
        private System.Windows.Forms.Button button_Save_Table;
        private System.Windows.Forms.Button button_Clear_Table;
        private System.Windows.Forms.Button button_Clear_Row;
        private System.Windows.Forms.Panel panel_Left;
        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.DataGridView dataGridView_Basic;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nazwa;
        private System.Windows.Forms.DataGridViewTextBoxColumn Property;
        private System.Windows.Forms.DataGridView dataGridView_Decode_Complex;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Panel panel_Down;
        private System.Windows.Forms.Panel panel_Right;
        private System.Windows.Forms.Panel panel_Bottom_Right;
        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Adres;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zawartosc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Typ;
    }
}