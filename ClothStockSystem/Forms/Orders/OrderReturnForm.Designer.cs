namespace ClothStockSystem.Forms.Orders
{
    partial class OrderReturnForm
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
        private static readonly System.Drawing.Color ClrDanger = System.Drawing.Color.FromArgb(220, 38, 38);
        private static readonly System.Drawing.Color ClrDangerDk = System.Drawing.Color.FromArgb(185, 28, 28);

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblView = new Label(); cmbView = new ComboBox();
            lblSearch = new Label(); txtSearch = new TextBox(); btnSearch = new Button();
            grid = new DataGridView();
            pnlBottom = new Panel();
            lblInfo = new Label();
            lblExtra = new Label(); numExtra = new NumericUpDown();
            btnReturn = new Button();
            lblSummary = new Label();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numExtra).BeginInit();
            SuspendLayout();
            // 
            // pnlTop (filter bar)
            // 
            pnlTop.BackColor = System.Drawing.Color.White;
            pnlTop.Controls.Add(lblView); pnlTop.Controls.Add(cmbView);
            pnlTop.Controls.Add(lblSearch); pnlTop.Controls.Add(txtSearch); pnlTop.Controls.Add(btnSearch);
            pnlTop.Dock = DockStyle.Top; pnlTop.Name = "pnlTop"; pnlTop.Size = new Size(1100, 62); pnlTop.TabIndex = 0;
            pnlTop.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(ClrLine);
                e.Graphics.DrawLine(pen, 0, pnlTop.Height - 1, pnlTop.Width, pnlTop.Height - 1);
            };
            // 
            // lblView / cmbView
            // 
            StyleLabel(lblView, "lblView", "Dekhein", 20, 21);
            lblView.ForeColor = ClrMuted;
            cmbView.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbView.FlatStyle = FlatStyle.Flat;
            cmbView.BackColor = System.Drawing.Color.White;
            cmbView.ForeColor = ClrText;
            cmbView.Location = new Point(88, 17);
            cmbView.Name = "cmbView";
            cmbView.Size = new Size(300, 33);
            cmbView.TabIndex = 1;
            cmbView.SelectedIndexChanged += cmbView_Changed;
            // 
            // lblSearch / txtSearch
            // 
            StyleLabel(lblSearch, "lblSearch", "Search", 416, 21);
            lblSearch.ForeColor = ClrMuted;
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = ClrText;
            txtSearch.Location = new Point(478, 17);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tracking ID, order no, phone or name";
            txtSearch.Size = new Size(472, 31);
            txtSearch.TabIndex = 2;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // btnSearch (primary action)
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.BackColor = ClrSky;
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatAppearance.MouseOverBackColor = ClrSkyDark;
            btnSearch.FlatAppearance.MouseDownBackColor = ClrSkyInk;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            btnSearch.ForeColor = System.Drawing.Color.White;
            btnSearch.Location = new Point(970, 14);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(110, 34);
            btnSearch.TabIndex = 3;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // grid (clean table: sirf horizontal lines)
            // 
            grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 42;
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = ClrLine;
            grid.MultiSelect = true;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.RowTemplate.Height = 38;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.TabIndex = 4;
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = ClrSkyLight,
                Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F),
                ForeColor = ClrSkyInk,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = ClrSkyLight,
                SelectionForeColor = ClrSkyInk
            };
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10F),
                ForeColor = ClrText,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk
            };
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ClrPage,
                ForeColor = ClrText,
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk
            };
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // pnlBottom (action bar)
            // 
            pnlBottom.BackColor = System.Drawing.Color.White;
            pnlBottom.Controls.Add(lblInfo); pnlBottom.Controls.Add(lblExtra); pnlBottom.Controls.Add(numExtra);
            pnlBottom.Controls.Add(btnReturn); pnlBottom.Controls.Add(lblSummary);
            pnlBottom.Dock = DockStyle.Bottom; pnlBottom.Name = "pnlBottom"; pnlBottom.Size = new Size(1100, 150); pnlBottom.TabIndex = 5;
            pnlBottom.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(ClrLine);
                e.Graphics.DrawLine(pen, 0, 0, pnlBottom.Width, 0);
            };
            // 
            // lblInfo
            // 
            lblInfo.AutoSize = true;
            lblInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            lblInfo.ForeColor = ClrSkyInk;
            lblInfo.Location = new Point(20, 16);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Select order(s) from the list above (Ctrl+click for multiple)";
            // 
            // lblExtra / numExtra
            // 
            StyleLabel(lblExtra, "lblExtra", "Wapsi ka extra charge (agar laga ho)", 20, 66);
            lblExtra.ForeColor = ClrMuted;
            numExtra.BorderStyle = BorderStyle.FixedSingle;
            numExtra.DecimalPlaces = 2;
            numExtra.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numExtra.Location = new Point(300, 61);
            numExtra.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numExtra.Name = "numExtra";
            numExtra.Size = new Size(140, 31);
            numExtra.TabIndex = 6;
            numExtra.TextAlign = HorizontalAlignment.Right;
            numExtra.ThousandsSeparator = true;
            numExtra.ValueChanged += grid_SelectionChanged;
            // 
            // btnReturn (destructive action: red)
            // 
            btnReturn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReturn.BackColor = ClrDanger;
            btnReturn.Cursor = Cursors.Hand;
            btnReturn.FlatAppearance.BorderSize = 0;
            btnReturn.FlatAppearance.MouseOverBackColor = ClrDangerDk;
            btnReturn.FlatAppearance.MouseDownBackColor = ClrDangerDk;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            btnReturn.ForeColor = System.Drawing.Color.White;
            btnReturn.Location = new Point(660, 54);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(420, 44);
            btnReturn.TabIndex = 7;
            btnReturn.Text = "Process Return (Return stock + Record loss)";
            btnReturn.UseVisualStyleBackColor = false;
            btnReturn.Click += btnReturn_Click;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            lblSummary.ForeColor = ClrText;
            lblSummary.Location = new Point(20, 110);
            lblSummary.Name = "lblSummary";
            lblSummary.Text = "";
            // 
            // OrderReturnForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = ClrPage;
            ClientSize = new Size(1100, 700);
            Controls.Add(grid); Controls.Add(pnlBottom); Controls.Add(pnlTop);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            Name = "OrderReturnForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Returns";
            Load += OrderReturnForm_Load;
            pnlTop.ResumeLayout(false); pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            pnlBottom.ResumeLayout(false); pnlBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numExtra).EndInit();
            ResumeLayout(false);
        }

        // ------------------------------------------------ helpers

        private static void StyleLabel(Label l, string name, string text, int x, int y)
        {
            l.AutoSize = true;
            l.Location = new Point(x, y);
            l.Name = name;
            l.Text = text;
        }

        #endregion

        private Panel pnlTop;
        private Label lblView; private ComboBox cmbView;
        private Label lblSearch; private TextBox txtSearch; private Button btnSearch;
        private DataGridView grid;
        private Panel pnlBottom;
        private Label lblInfo; private Label lblExtra; private NumericUpDown numExtra;
        private Button btnReturn; private Label lblSummary;
    }
}