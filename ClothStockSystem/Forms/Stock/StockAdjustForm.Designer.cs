namespace ClothStockSystem.Forms.Stock
{
    partial class StockAdjustForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // ------------------------------------------------ theme (Navy + Sky Blue)
        private static readonly System.Drawing.Color ClrPage = System.Drawing.Color.FromArgb(241, 245, 249);
        private static readonly System.Drawing.Color ClrLine = System.Drawing.Color.FromArgb(226, 232, 240);
        private static readonly System.Drawing.Color ClrText = System.Drawing.Color.FromArgb(30, 41, 59);
        private static readonly System.Drawing.Color ClrMuted = System.Drawing.Color.FromArgb(100, 116, 139);
        private static readonly System.Drawing.Color ClrSky = System.Drawing.Color.FromArgb(2, 132, 199);
        private static readonly System.Drawing.Color ClrSkyDark = System.Drawing.Color.FromArgb(3, 105, 161);
        private static readonly System.Drawing.Color ClrSkyLight = System.Drawing.Color.FromArgb(240, 249, 255);
        private static readonly System.Drawing.Color ClrSkySel = System.Drawing.Color.FromArgb(186, 230, 253);
        private static readonly System.Drawing.Color ClrSkyInk = System.Drawing.Color.FromArgb(12, 74, 110);
        private static readonly System.Drawing.Color ClrNavy = System.Drawing.Color.FromArgb(15, 23, 42);

        private void InitializeComponent()
        {
            pnlHeader = new Panel(); lblHeader = new Label(); lblHeaderSub = new Label();
            pnlBody = new Panel();
            pnlTop = new Panel(); tlpTop = new TableLayoutPanel();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            lblColor = new Label();
            cmbColor = new ComboBox();
            grpBatches = new GroupBox();
            grid = new DataGridView();
            pnlBottom = new Panel(); tlpBottom = new TableLayoutPanel();
            lblType = new Label();
            cmbType = new ComboBox();
            lblMeters = new Label();
            numMeters = new NumericUpDown();
            lblNote = new Label();
            txtNote = new TextBox();
            lblInfo = new Label();
            btnSave = new Button();
            pnlHeader.SuspendLayout();
            pnlBody.SuspendLayout();
            pnlTop.SuspendLayout(); tlpTop.SuspendLayout();
            grpBatches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlBottom.SuspendLayout(); tlpBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMeters).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader (gradient banner)
            // 
            pnlHeader.Controls.Add(lblHeader); pnlHeader.Controls.Add(lblHeaderSub);
            pnlHeader.Dock = DockStyle.Top; pnlHeader.Name = "pnlHeader"; pnlHeader.Size = new Size(1100, 62);
            pnlHeader.Paint += (s, e) =>
            {
                using var br = new System.Drawing.Drawing2D.LinearGradientBrush(
                    pnlHeader.ClientRectangle, ClrNavy, ClrSky, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(br, pnlHeader.ClientRectangle);
            };
            lblHeader.AutoSize = true; lblHeader.BackColor = System.Drawing.Color.Transparent;
            lblHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 15F);
            lblHeader.ForeColor = System.Drawing.Color.White; lblHeader.Location = new Point(20, 8);
            lblHeader.Name = "lblHeader"; lblHeader.Text = "Stock Adjust";
            lblHeaderSub.AutoSize = true; lblHeaderSub.BackColor = System.Drawing.Color.Transparent;
            lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(186, 230, 253); lblHeaderSub.Location = new Point(22, 37);
            lblHeaderSub.Name = "lblHeaderSub"; lblHeaderSub.Text = "Select article, color and batch, then record the adjustment";
            // 
            // pnlBody (page background with padding so cards don't touch the edges)
            // 
            pnlBody.BackColor = ClrPage;
            pnlBody.Controls.Add(grpBatches);
            pnlBody.Controls.Add(pnlBottom);
            pnlBody.Controls.Add(pnlTop);
            pnlBody.Dock = DockStyle.Fill; pnlBody.Name = "pnlBody"; pnlBody.Padding = new Padding(14);
            // 
            // pnlTop (article + color filter card)
            // 
            pnlTop.BackColor = System.Drawing.Color.White;
            pnlTop.Controls.Add(tlpTop);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(10, 8, 10, 8);
            pnlTop.Size = new Size(1072, 62);
            pnlTop.TabIndex = 0;
            pnlTop.Paint += (s, e) => { DrawBorder(e, pnlTop); };

            tlpTop.ColumnCount = 4; tlpTop.RowCount = 1;
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpTop.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTop.Dock = DockStyle.Fill; tlpTop.Name = "tlpTop";
            AddPair(tlpTop, lblArticle, cmbArticle, 0, 0, 1);
            AddPair(tlpTop, lblColor, cmbColor, 2, 0, 1);
            // 
            // lblArticle / cmbArticle
            // 
            lblArticle.Name = "lblArticle";
            lblArticle.Text = "Article";
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.Name = "cmbArticle";
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            // 
            // lblColor / cmbColor
            // 
            lblColor.Name = "lblColor";
            lblColor.Text = "Color";
            cmbColor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbColor.Name = "cmbColor";
            cmbColor.TabIndex = 2;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            // 
            // grpBatches
            // 
            grpBatches.BackColor = System.Drawing.Color.White;
            grpBatches.Controls.Add(grid);
            grpBatches.Dock = DockStyle.Fill;
            grpBatches.FlatStyle = FlatStyle.Flat;
            grpBatches.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            grpBatches.ForeColor = ClrSky;
            grpBatches.Name = "grpBatches";
            grpBatches.Padding = new Padding(10, 6, 10, 10);
            grpBatches.TabIndex = 1;
            grpBatches.TabStop = false;
            grpBatches.Text = "  Select a batch  ";
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Dock = DockStyle.Fill;
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.TabIndex = 3;
            grid.SelectionChanged += grid_SelectionChanged;
            StyleGrid(grid, 34);
            // 
            // pnlBottom (adjustment form card)
            // 
            pnlBottom.BackColor = System.Drawing.Color.White;
            pnlBottom.Controls.Add(tlpBottom);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Padding = new Padding(10, 10, 10, 10);
            pnlBottom.Size = new Size(1072, 210);
            pnlBottom.TabIndex = 2;
            pnlBottom.Paint += (s, e) =>
            {
                DrawBorder(e, pnlBottom);
                using var br = new System.Drawing.SolidBrush(ClrSky);
                e.Graphics.FillRectangle(br, 0, 0, 5, pnlBottom.Height);
            };

            tlpBottom.ColumnCount = 4; tlpBottom.RowCount = 3;
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpBottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBottom.Dock = DockStyle.Fill; tlpBottom.Name = "tlpBottom"; tlpBottom.Padding = new Padding(8, 0, 0, 0);
            AddPair(tlpBottom, lblType, cmbType, 0, 0, 1);
            AddPair(tlpBottom, lblMeters, numMeters, 2, 0, 1);
            AddPair(tlpBottom, lblNote, txtNote, 0, 1, 3);
            tlpBottom.Controls.Add(lblInfo, 0, 2); tlpBottom.SetColumnSpan(lblInfo, 3);
            tlpBottom.Controls.Add(btnSave, 3, 2);
            // 
            // lblType / cmbType
            // 
            lblType.Name = "lblType";
            lblType.Text = "Kya hua";
            cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbType.Name = "cmbType";
            cmbType.TabIndex = 4;
            cmbType.SelectedIndexChanged += Preview_Changed;
            // 
            // lblMeters / numMeters
            // 
            lblMeters.Name = "lblMeters";
            lblMeters.Text = "Meter";
            numMeters.DecimalPlaces = 3;
            numMeters.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numMeters.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numMeters.Name = "numMeters";
            numMeters.TabIndex = 5;
            numMeters.TextAlign = HorizontalAlignment.Right;
            numMeters.ValueChanged += Preview_Changed;
            // 
            // lblNote / txtNote
            // 
            lblNote.Name = "lblNote";
            lblNote.Text = "Wajah";
            txtNote.MaxLength = 250;
            txtNote.Name = "txtNote";
            txtNote.TabIndex = 6;
            // 
            // lblInfo (preview / status line)
            // 
            lblInfo.Anchor = AnchorStyles.Left;
            lblInfo.AutoSize = true;
            lblInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            lblInfo.ForeColor = ClrSkyInk;
            lblInfo.Margin = new Padding(3, 3, 3, 3);
            lblInfo.Name = "lblInfo";
            lblInfo.Text = "Select a batch";
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Right;
            btnSave.BackColor = ClrSky;
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = ClrSkyDark;
            btnSave.FlatAppearance.MouseDownBackColor = ClrSkyInk;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            btnSave.ForeColor = System.Drawing.Color.White;
            btnSave.Margin = new Padding(3, 3, 6, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(210, 42);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save Adjustment";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // label / input styles
            // 
            foreach (var l in new[] { lblArticle, lblColor, lblType, lblMeters, lblNote })
            {
                l.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
                l.ForeColor = ClrSkyInk;
            }
            foreach (Control c in new Control[] { cmbArticle, cmbColor, cmbType, numMeters, txtNote })
                StyleInput(c);
            // 
            // StockAdjustForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = ClrPage;
            ClientSize = new Size(1100, 760);
            Controls.Add(pnlBody);
            Controls.Add(pnlHeader);
            Font = new System.Drawing.Font("Segoe UI", 9.5F);
            MinimumSize = new Size(900, 640);
            Name = "StockAdjustForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock Adjust";
            Load += StockAdjustForm_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBody.ResumeLayout(false);
            tlpTop.ResumeLayout(false); tlpTop.PerformLayout();
            pnlTop.ResumeLayout(false);
            grpBatches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            tlpBottom.ResumeLayout(false); tlpBottom.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numMeters).EndInit();
            ResumeLayout(false);
        }

        // ------------------------------------------------ helpers

        // label + input ko table ke cell me rakhta hai (alignment automatic, overlap nahi)
        private static void AddPair(TableLayoutPanel t, Label l, Control c, int col, int row, int inputSpan)
        {
            l.AutoSize = true;
            l.Anchor = AnchorStyles.Left;
            l.Margin = new Padding(3, 3, 8, 3);
            c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            c.Margin = new Padding(3, 3, 16, 3);
            t.Controls.Add(l, col, row);
            t.Controls.Add(c, col + 1, row);
            if (inputSpan > 1) t.SetColumnSpan(c, inputSpan);
        }

        private static void DrawBorder(PaintEventArgs e, Control c)
        {
            using var pen = new System.Drawing.Pen(ClrLine);
            e.Graphics.DrawRectangle(pen, 0, 0, c.Width - 1, c.Height - 1);
        }

        private static void StyleInput(Control c)
        {
            c.Font = new System.Drawing.Font("Segoe UI", 10F);
            c.ForeColor = ClrText;
            switch (c)
            {
                case TextBox t: t.BorderStyle = BorderStyle.FixedSingle; break;
                case ComboBox cb: cb.FlatStyle = FlatStyle.Flat; cb.BackColor = System.Drawing.Color.White; break;
                case NumericUpDown n: n.BorderStyle = BorderStyle.FixedSingle; break;
            }
        }

        // Clean table: navy header, zebra rows, sky selection
        private static void StyleGrid(DataGridView g, int rowHeight)
        {
            g.BackgroundColor = System.Drawing.Color.White;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            g.EnableHeadersVisualStyles = false;
            g.GridColor = ClrLine;
            g.RowTemplate.Height = rowHeight;
            g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = ClrSkyInk,
                Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F),
                ForeColor = System.Drawing.Color.White,
                Padding = new Padding(8, 7, 0, 7),
                SelectionBackColor = ClrSkyInk,
                SelectionForeColor = System.Drawing.Color.White,
                WrapMode = DataGridViewTriState.True
            };
            g.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9.5F),
                ForeColor = ClrText,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk,
                WrapMode = DataGridViewTriState.False
            };
            g.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ClrSkyLight,
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk
            };
        }

        #endregion

        private Panel pnlHeader; private Label lblHeader; private Label lblHeaderSub;
        private Panel pnlBody;
        private TableLayoutPanel tlpTop; private TableLayoutPanel tlpBottom;

        private Panel pnlTop;
        private Label lblArticle;
        private ComboBox cmbArticle;
        private Label lblColor;
        private ComboBox cmbColor;
        private GroupBox grpBatches;
        private DataGridView grid;
        private Panel pnlBottom;
        private Label lblType;
        private ComboBox cmbType;
        private Label lblMeters;
        private NumericUpDown numMeters;
        private Label lblNote;
        private TextBox txtNote;
        private Label lblInfo;
        private Button btnSave;
    }
}