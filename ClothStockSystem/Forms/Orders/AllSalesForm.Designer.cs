namespace ClothStockSystem.Forms.Orders
{
    partial class AllSalesForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // ------------------------------------------------ theme (Sky Blue, minimal)
        private static readonly System.Drawing.Color ClrPage = System.Drawing.Color.FromArgb(248, 250, 252);
        private static readonly System.Drawing.Color ClrLine = System.Drawing.Color.FromArgb(226, 232, 240);
        private static readonly System.Drawing.Color ClrText = System.Drawing.Color.FromArgb(30, 41, 59);
        private static readonly System.Drawing.Color ClrMuted = System.Drawing.Color.FromArgb(100, 116, 139);
        private static readonly System.Drawing.Color ClrSky = System.Drawing.Color.FromArgb(2, 132, 199);
        private static readonly System.Drawing.Color ClrSkyDark = System.Drawing.Color.FromArgb(3, 105, 161);
        private static readonly System.Drawing.Color ClrSkyLight = System.Drawing.Color.FromArgb(240, 249, 255);
        private static readonly System.Drawing.Color ClrSkySel = System.Drawing.Color.FromArgb(224, 242, 254);
        private static readonly System.Drawing.Color ClrSkyInk = System.Drawing.Color.FromArgb(12, 74, 110);
        private static readonly System.Drawing.Color ClrIndigo = System.Drawing.Color.FromArgb(79, 70, 229);
        private static readonly System.Drawing.Color ClrDanger = System.Drawing.Color.FromArgb(220, 38, 38);

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            pnlTop = new Panel();
            lblRange = new Label(); cmbRange = new ComboBox();
            dtpFrom = new DateTimePicker(); dtpTo = new DateTimePicker();
            lblChannel = new Label(); cmbChannel = new ComboBox();
            lblStatus = new Label(); cmbStatus = new ComboBox();
            lblSearch = new Label(); txtSearch = new TextBox(); btnSearch = new Button();
            grid = new DataGridView(); gridLines = new DataGridView();
            pnlSummary = new Panel();
            lblShop = new Label(); lblOnline = new Label(); lblReturn = new Label(); lblGrand = new Label();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridLines).BeginInit();
            pnlSummary.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop (filter bar: row 1 = filters, row 2 = search)
            // 
            pnlTop.BackColor = System.Drawing.Color.White;
            pnlTop.Controls.Add(lblRange); pnlTop.Controls.Add(cmbRange);
            pnlTop.Controls.Add(dtpFrom); pnlTop.Controls.Add(dtpTo);
            pnlTop.Controls.Add(lblChannel); pnlTop.Controls.Add(cmbChannel);
            pnlTop.Controls.Add(lblStatus); pnlTop.Controls.Add(cmbStatus);
            pnlTop.Controls.Add(lblSearch); pnlTop.Controls.Add(txtSearch); pnlTop.Controls.Add(btnSearch);
            pnlTop.Dock = DockStyle.Top; pnlTop.Name = "pnlTop"; pnlTop.Size = new Size(1276, 110); pnlTop.TabIndex = 0;
            pnlTop.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(ClrLine);
                e.Graphics.DrawLine(pen, 0, pnlTop.Height - 1, pnlTop.Width, pnlTop.Height - 1);
            };
            // 
            // Row 1
            // 
            StyleLabel(lblRange, "lblRange", "Period", 20, 20);
            StyleCombo(cmbRange, "cmbRange", 82, 16, 140, 1);
            cmbRange.SelectedIndexChanged += cmbRange_Changed;
            StyleDate(dtpFrom, "dtpFrom", 238, 16, 2);
            dtpFrom.ValueChanged += Filter_Changed;
            StyleDate(dtpTo, "dtpTo", 398, 16, 3);
            dtpTo.ValueChanged += Filter_Changed;
            StyleLabel(lblChannel, "lblChannel", "Channel", 576, 20);
            StyleCombo(cmbChannel, "cmbChannel", 654, 16, 150, 4);
            cmbChannel.SelectedIndexChanged += Filter_Changed;
            StyleLabel(lblStatus, "lblStatus", "Status", 826, 20);
            StyleCombo(cmbStatus, "cmbStatus", 890, 16, 150, 5);
            cmbStatus.SelectedIndexChanged += Filter_Changed;
            // 
            // Row 2: search
            // 
            StyleLabel(lblSearch, "lblSearch", "Search", 20, 68);
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BackColor = System.Drawing.Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtSearch.ForeColor = ClrText;
            txtSearch.Location = new Point(82, 63);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Phone 03001234567, name, order no or tracking ID (search includes all dates)";
            txtSearch.Size = new Size(1040, 31);
            txtSearch.TabIndex = 6;
            txtSearch.KeyDown += txtSearch_KeyDown;
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.BackColor = ClrSky;
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatAppearance.MouseOverBackColor = ClrSkyDark;
            btnSearch.FlatAppearance.MouseDownBackColor = ClrSkyInk;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            btnSearch.ForeColor = System.Drawing.Color.White;
            btnSearch.Location = new Point(1140, 60);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(120, 36);
            btnSearch.TabIndex = 7;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // grid (main table)
            // 
            grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.AllowUserToResizeRows = false;
            grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.DefaultCellStyle = dataGridViewCellStyle2;
            grid.Dock = DockStyle.Fill; grid.EnableHeadersVisualStyles = false;
            grid.GridColor = ClrLine;
            grid.MultiSelect = false;
            grid.Name = "grid"; grid.ReadOnly = true; grid.RowHeadersVisible = false; grid.RowHeadersWidth = 62;
            grid.RowTemplate.Height = 38;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.TabIndex = 8;
            grid.SelectionChanged += grid_SelectionChanged;
            grid.DataBindingComplete += grid_DataBindingComplete;
            // 
            // gridLines (order ki detail lines)
            // 
            gridLines.AllowUserToAddRows = false; gridLines.AllowUserToDeleteRows = false; gridLines.AllowUserToResizeRows = false;
            gridLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridLines.BackgroundColor = System.Drawing.Color.White;
            gridLines.BorderStyle = BorderStyle.None;
            gridLines.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            gridLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridLines.DefaultCellStyle = dataGridViewCellStyle5;
            gridLines.Dock = DockStyle.Bottom; gridLines.EnableHeadersVisualStyles = false;
            gridLines.GridColor = ClrLine;
            gridLines.Name = "gridLines"; gridLines.ReadOnly = true; gridLines.RowHeadersVisible = false; gridLines.RowHeadersWidth = 62;
            gridLines.RowTemplate.Height = 34;
            gridLines.Size = new Size(1276, 150); gridLines.TabIndex = 9;
            // 
            // pnlSummary (totals bar)
            // 
            pnlSummary.BackColor = System.Drawing.Color.White;
            pnlSummary.Controls.Add(lblShop); pnlSummary.Controls.Add(lblOnline); pnlSummary.Controls.Add(lblReturn); pnlSummary.Controls.Add(lblGrand);
            pnlSummary.Dock = DockStyle.Bottom; pnlSummary.Name = "pnlSummary"; pnlSummary.Size = new Size(1276, 160); pnlSummary.TabIndex = 10;
            pnlSummary.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(ClrLine);
                e.Graphics.DrawLine(pen, 0, 0, pnlSummary.Width, 0);
            };
            StyleSummary(lblShop, "lblShop", ClrSkyDark, 20, 12);
            StyleSummary(lblOnline, "lblOnline", ClrIndigo, 20, 44);
            StyleSummary(lblReturn, "lblReturn", ClrDanger, 20, 76);
            lblGrand.AutoSize = true;
            lblGrand.Font = new System.Drawing.Font("Segoe UI Semibold", 15F);
            lblGrand.ForeColor = ClrSkyInk;
            lblGrand.Location = new Point(20, 112);
            lblGrand.Name = "lblGrand";
            lblGrand.Text = "";
            // 
            // dataGridViewCellStyle1 (grid header)
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = ClrSkyLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            dataGridViewCellStyle1.ForeColor = ClrSkyInk;
            dataGridViewCellStyle1.Padding = new Padding(8, 8, 0, 8);
            dataGridViewCellStyle1.SelectionBackColor = ClrSkyLight;
            dataGridViewCellStyle1.SelectionForeColor = ClrSkyInk;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            // 
            // dataGridViewCellStyle2 (grid rows)
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle2.ForeColor = ClrText;
            dataGridViewCellStyle2.Padding = new Padding(8, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = ClrSkySel;
            dataGridViewCellStyle2.SelectionForeColor = ClrSkyInk;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            // 
            // dataGridViewCellStyle3 (grid alternate rows)
            // 
            dataGridViewCellStyle3.BackColor = ClrPage;
            dataGridViewCellStyle3.SelectionBackColor = ClrSkySel;
            dataGridViewCellStyle3.SelectionForeColor = ClrSkyInk;
            // 
            // dataGridViewCellStyle4 (gridLines header)
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = ClrPage;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            dataGridViewCellStyle4.ForeColor = ClrMuted;
            dataGridViewCellStyle4.Padding = new Padding(8, 5, 0, 5);
            dataGridViewCellStyle4.SelectionBackColor = ClrPage;
            dataGridViewCellStyle4.SelectionForeColor = ClrMuted;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            // 
            // dataGridViewCellStyle5 (gridLines rows)
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            dataGridViewCellStyle5.ForeColor = ClrText;
            dataGridViewCellStyle5.Padding = new Padding(8, 0, 0, 0);
            dataGridViewCellStyle5.SelectionBackColor = ClrSkySel;
            dataGridViewCellStyle5.SelectionForeColor = ClrSkyInk;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            // 
            // AllSalesForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = ClrPage;
            ClientSize = new Size(1300, 800);
            Controls.Add(grid); Controls.Add(gridLines); Controls.Add(pnlSummary); Controls.Add(pnlTop);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            Name = "AllSalesForm";
            Padding = new Padding(12);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "All Sales (Shop + Online)";
            Load += AllSalesForm_Load;
            pnlTop.ResumeLayout(false); pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridLines).EndInit();
            pnlSummary.ResumeLayout(false); pnlSummary.PerformLayout();
            ResumeLayout(false);
        }

        // ------------------------------------------------ helpers

        private static void StyleLabel(Label l, string name, string text, int x, int y)
        {
            l.AutoSize = true;
            l.Font = new System.Drawing.Font("Segoe UI", 10F);
            l.ForeColor = ClrMuted;
            l.Location = new Point(x, y);
            l.Name = name;
            l.Text = text;
        }

        private static void StyleCombo(ComboBox c, string name, int x, int y, int w, int tab)
        {
            c.BackColor = System.Drawing.Color.White;
            c.DropDownStyle = ComboBoxStyle.DropDownList;
            c.FlatStyle = FlatStyle.Flat;
            c.Font = new System.Drawing.Font("Segoe UI", 10F);
            c.ForeColor = ClrText;
            c.Location = new Point(x, y);
            c.Name = name;
            c.Size = new Size(w, 33);
            c.TabIndex = tab;
        }

        private static void StyleDate(DateTimePicker d, string name, int x, int y, int tab)
        {
            d.CalendarTitleBackColor = ClrSky;
            d.CalendarTitleForeColor = System.Drawing.Color.White;
            d.CustomFormat = "dd-MMM-yyyy";
            d.Font = new System.Drawing.Font("Segoe UI", 10F);
            d.Format = DateTimePickerFormat.Custom;
            d.Location = new Point(x, y);
            d.Name = name;
            d.Size = new Size(150, 31);
            d.TabIndex = tab;
        }

        private static void StyleSummary(Label l, string name, System.Drawing.Color color, int x, int y)
        {
            l.AutoSize = true;
            l.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            l.ForeColor = color;
            l.Location = new Point(x, y);
            l.Name = name;
            l.Text = "";
        }

        #endregion

        private Panel pnlTop;
        private Label lblRange; private ComboBox cmbRange;
        private DateTimePicker dtpFrom; private DateTimePicker dtpTo;
        private Label lblChannel; private ComboBox cmbChannel;
        private Label lblStatus; private ComboBox cmbStatus;
        private Label lblSearch; private TextBox txtSearch; private Button btnSearch;
        private DataGridView grid; private DataGridView gridLines;
        private Panel pnlSummary;
        private Label lblShop; private Label lblOnline; private Label lblReturn; private Label lblGrand;
    }
}