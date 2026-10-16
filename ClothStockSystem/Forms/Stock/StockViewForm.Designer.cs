using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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
            components = new System.ComponentModel.Container();
            pnlTop = new Panel();
            flowFilters = new FlowLayoutPanel();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            lblView = new Label();
            cmbView = new ComboBox();
            chkAlertsOnly = new CheckBox();
            btnRefresh = new Button();
            grid = new DataGridView();
            pnlBottom = new Panel();
            lblSummary = new Label();
            lblLegend = new Label();
            tmrRefresh = new System.Windows.Forms.Timer(components);
            pnlTop.SuspendLayout();
            flowFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(flowFilters);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Name = "pnlTop";
            pnlTop.Padding = new Padding(10, 8, 10, 8);
            pnlTop.Size = new Size(1100, 64);
            pnlTop.TabIndex = 0;
            // 
            // flowFilters
            // 
            flowFilters.Controls.Add(lblArticle);
            flowFilters.Controls.Add(cmbArticle);
            flowFilters.Controls.Add(lblView);
            flowFilters.Controls.Add(cmbView);
            flowFilters.Controls.Add(chkAlertsOnly);
            flowFilters.Controls.Add(btnRefresh);
            flowFilters.Dock = DockStyle.Fill;
            flowFilters.Name = "flowFilters";
            flowFilters.TabIndex = 0;
            flowFilters.WrapContents = false;
            // 
            // lblArticle
            // 
            lblArticle.Anchor = AnchorStyles.Left;
            lblArticle.AutoSize = true;
            lblArticle.Margin = new Padding(3, 8, 3, 0);
            lblArticle.Name = "lblArticle";
            lblArticle.Text = "Article:";
            // 
            // cmbArticle
            // 
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(220, 33);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += Filter_Changed;
            // 
            // lblView
            // 
            lblView.AutoSize = true;
            lblView.Margin = new Padding(20, 8, 3, 0);
            lblView.Name = "lblView";
            lblView.Text = "Dekhein:";
            // 
            // cmbView
            // 
            cmbView.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbView.Name = "cmbView";
            cmbView.Size = new Size(180, 33);
            cmbView.TabIndex = 2;
            cmbView.SelectedIndexChanged += Filter_Changed;
            // 
            // chkAlertsOnly
            // 
            chkAlertsOnly.AutoSize = true;
            chkAlertsOnly.Margin = new Padding(20, 6, 3, 0);
            chkAlertsOnly.Name = "chkAlertsOnly";
            chkAlertsOnly.TabIndex = 3;
            chkAlertsOnly.Text = "Sirf alert wale";
            chkAlertsOnly.UseVisualStyleBackColor = true;
            chkAlertsOnly.CheckedChanged += Filter_Changed;
            // 
            // btnRefresh
            // 
            btnRefresh.Margin = new Padding(20, 0, 3, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(120, 36);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.Dock = DockStyle.Fill;
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.TabIndex = 5;
            grid.DataBindingComplete += grid_DataBindingComplete;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(240, 246, 255);
            pnlBottom.Controls.Add(lblSummary);
            pnlBottom.Controls.Add(lblLegend);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1100, 80);
            pnlBottom.TabIndex = 6;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblSummary.Location = new Point(12, 10);
            lblSummary.Name = "lblSummary";
            lblSummary.Text = "Total: 0";
            // 
            // lblLegend
            // 
            lblLegend.AutoSize = true;
            lblLegend.ForeColor = System.Drawing.Color.DimGray;
            lblLegend.Location = new Point(12, 46);
            lblLegend.Name = "lblLegend";
            lblLegend.Text = "Laal = low stock, Narangi = purana maal";
            // 
            // tmrRefresh
            // 
            tmrRefresh.Tick += tmrRefresh_Tick;
            // 
            // StockViewForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 640);
            Controls.Add(grid);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            MinimumSize = new Size(900, 500);
            Name = "StockViewForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock View";
            Load += StockViewForm_Load;
            pnlTop.ResumeLayout(false);
            flowFilters.ResumeLayout(false);
            flowFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private FlowLayoutPanel flowFilters;
        private Label lblArticle;
        private ComboBox cmbArticle;
        private Label lblView;
        private ComboBox cmbView;
        private CheckBox chkAlertsOnly;
        private Button btnRefresh;
        private DataGridView grid;
        private Panel pnlBottom;
        private Label lblSummary;
        private Label lblLegend;
        private System.Windows.Forms.Timer tmrRefresh;
    }
}