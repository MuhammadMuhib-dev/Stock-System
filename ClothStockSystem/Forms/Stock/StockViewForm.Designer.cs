namespace ClothStockSystem.Forms.Stock
{
    partial class StockViewForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // theme colors are LOCAL on purpose: no static fields/methods that could clash
            // with names already defined in StockViewForm.cs
            var cWindow = System.Drawing.Color.White;
            var cBar = System.Drawing.Color.FromArgb(246, 247, 249);
            var cBorder = System.Drawing.Color.FromArgb(218, 222, 228);
            var cRule = System.Drawing.Color.FromArgb(234, 237, 241);
            var cText = System.Drawing.Color.FromArgb(33, 37, 41);
            var cCaption = System.Drawing.Color.FromArgb(108, 117, 125);
            var cAccent = System.Drawing.Color.FromArgb(15, 118, 110);
            var cAccentDk = System.Drawing.Color.FromArgb(11, 92, 86);
            var cSelect = System.Drawing.Color.FromArgb(214, 236, 233);

            components = new System.ComponentModel.Container();
            pnlTop = new Panel();
            flowFilters = new FlowLayoutPanel();
            pnlArticle = new Panel();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            pnlView = new Panel();
            lblView = new Label();
            cmbView = new ComboBox();
            chkAlertsOnly = new CheckBox();
            btnRefresh = new Button();
            grid = new DataGridView();
            pnlBottom = new Panel();
            tlpStatus = new TableLayoutPanel();
            lblSummary = new Label();
            lblLegend = new Label();
            tmrRefresh = new System.Windows.Forms.Timer(components);
            pnlTop.SuspendLayout();
            flowFilters.SuspendLayout();
            pnlArticle.SuspendLayout();
            pnlView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlBottom.SuspendLayout();
            tlpStatus.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop (toolbar)
            // 
            pnlTop.BackColor = cWindow;
            pnlTop.Controls.Add(flowFilters);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(16, 10, 16, 10);
            pnlTop.Size = new Size(1100, 74);
            pnlTop.TabIndex = 0;
            pnlTop.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(cBorder);
                e.Graphics.DrawLine(pen, 0, pnlTop.Height - 1, pnlTop.Width, pnlTop.Height - 1);
            };
            // 
            // flowFilters
            // 
            flowFilters.BackColor = cWindow;
            flowFilters.Controls.Add(pnlArticle);
            flowFilters.Controls.Add(pnlView);
            flowFilters.Controls.Add(chkAlertsOnly);
            flowFilters.Controls.Add(btnRefresh);
            flowFilters.Dock = DockStyle.Fill;
            flowFilters.Name = "flowFilters";
            flowFilters.TabIndex = 0;
            flowFilters.WrapContents = false;
            // 
            // pnlArticle
            // 
            pnlArticle.Controls.Add(lblArticle);
            pnlArticle.Controls.Add(cmbArticle);
            pnlArticle.Margin = new Padding(0, 0, 20, 0);
            pnlArticle.Name = "pnlArticle";
            pnlArticle.Size = new Size(240, 52);
            lblArticle.AutoSize = true;
            lblArticle.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblArticle.ForeColor = cCaption;
            lblArticle.Location = new Point(0, 0);
            lblArticle.Name = "lblArticle";
            lblArticle.Text = "Article";
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbArticle.ForeColor = cText;
            cmbArticle.Location = new Point(0, 20);
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(240, 25);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += Filter_Changed;
            // 
            // pnlView
            // 
            pnlView.Controls.Add(lblView);
            pnlView.Controls.Add(cmbView);
            pnlView.Margin = new Padding(0, 0, 24, 0);
            pnlView.Name = "pnlView";
            pnlView.Size = new Size(200, 52);
            lblView.AutoSize = true;
            lblView.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            lblView.ForeColor = cCaption;
            lblView.Location = new Point(0, 0);
            lblView.Name = "lblView";
            lblView.Text = "View";
            cmbView.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbView.Font = new System.Drawing.Font("Segoe UI", 10F);
            cmbView.ForeColor = cText;
            cmbView.Location = new Point(0, 20);
            cmbView.Name = "cmbView";
            cmbView.Size = new Size(200, 25);
            cmbView.TabIndex = 2;
            cmbView.SelectedIndexChanged += Filter_Changed;
            // 
            // chkAlertsOnly
            // 
            chkAlertsOnly.AutoSize = true;
            chkAlertsOnly.BackColor = cWindow;
            chkAlertsOnly.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            chkAlertsOnly.ForeColor = cText;
            chkAlertsOnly.Margin = new Padding(0, 22, 24, 0);
            chkAlertsOnly.Name = "chkAlertsOnly";
            chkAlertsOnly.TabIndex = 3;
            chkAlertsOnly.Text = "Alerts only";
            chkAlertsOnly.UseVisualStyleBackColor = false;
            chkAlertsOnly.CheckedChanged += Filter_Changed;
            // 
            // btnRefresh
            // 
            btnRefresh.BackColor = cAccent;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = cAccentDk;
            btnRefresh.FlatAppearance.MouseDownBackColor = cAccentDk;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            btnRefresh.ForeColor = System.Drawing.Color.White;
            btnRefresh.Margin = new Padding(0, 19, 0, 0);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 29);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = cWindow;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = cBar,
                Font = new System.Drawing.Font("Segoe UI Semibold", 9F),
                ForeColor = cText,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = cBar,
                SelectionForeColor = cText,
                WrapMode = DataGridViewTriState.False
            };
            grid.ColumnHeadersHeight = 34;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = cWindow,
                Font = new System.Drawing.Font("Segoe UI", 9.5F),
                ForeColor = cText,
                Padding = new Padding(8, 0, 4, 0),
                SelectionBackColor = cSelect,
                SelectionForeColor = cText
            };
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = cRule;
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.RowTemplate.Height = 30;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.TabIndex = 5;
            grid.DataBindingComplete += grid_DataBindingComplete;
            // 
            // pnlBottom (status bar)
            // 
            pnlBottom.BackColor = cBar;
            pnlBottom.Controls.Add(tlpStatus);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1100, 40);
            pnlBottom.TabIndex = 6;
            pnlBottom.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(cBorder);
                e.Graphics.DrawLine(pen, 0, 0, pnlBottom.Width, 0);
            };
            tlpStatus.ColumnCount = 2;
            tlpStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpStatus.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpStatus.Controls.Add(lblSummary, 0, 0);
            tlpStatus.Controls.Add(lblLegend, 1, 0);
            tlpStatus.Dock = DockStyle.Fill;
            tlpStatus.Name = "tlpStatus";
            tlpStatus.Padding = new Padding(16, 0, 16, 0);
            tlpStatus.RowCount = 1;
            tlpStatus.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // 
            // lblSummary
            // 
            lblSummary.Anchor = AnchorStyles.Left;
            lblSummary.AutoSize = true;
            lblSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            lblSummary.ForeColor = cText;
            lblSummary.Name = "lblSummary";
            lblSummary.Text = "Total: 0";
            // 
            // lblLegend
            // 
            lblLegend.Anchor = AnchorStyles.Right;
            lblLegend.AutoSize = true;
            lblLegend.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblLegend.ForeColor = cCaption;
            lblLegend.Name = "lblLegend";
            lblLegend.Text = "Red = low stock, Orange = old stock";
            // 
            // tmrRefresh
            // 
            tmrRefresh.Tick += tmrRefresh_Tick;
            // 
            // StockViewForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = cWindow;
            ClientSize = new Size(1100, 680);
            Controls.Add(grid);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            MinimumSize = new Size(900, 520);
            Name = "StockViewForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock View";
            Load += StockViewForm_Load;
            pnlArticle.ResumeLayout(false);
            pnlArticle.PerformLayout();
            pnlView.ResumeLayout(false);
            pnlView.PerformLayout();
            flowFilters.ResumeLayout(false);
            flowFilters.PerformLayout();
            pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            tlpStatus.ResumeLayout(false);
            tlpStatus.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private FlowLayoutPanel flowFilters;
        private Panel pnlArticle;
        private Panel pnlView;
        private Label lblArticle;
        private ComboBox cmbArticle;
        private Label lblView;
        private ComboBox cmbView;
        private CheckBox chkAlertsOnly;
        private Button btnRefresh;
        private DataGridView grid;
        private Panel pnlBottom;
        private TableLayoutPanel tlpStatus;
        private Label lblSummary;
        private Label lblLegend;
        private System.Windows.Forms.Timer tmrRefresh;
    }
}