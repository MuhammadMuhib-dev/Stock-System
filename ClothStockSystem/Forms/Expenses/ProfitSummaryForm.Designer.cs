namespace ClothStockSystem.Forms.Expenses
{
    partial class ProfitSummaryForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlTop = new Panel();
            lblRange = new Label(); cmbRange = new ComboBox();
            dtpFrom = new DateTimePicker(); dtpTo = new DateTimePicker();
            btnRefresh = new Button();
            lblNet = new Label(); lblNote = new Label();
            gridSummary = new DataGridView();
            gridExp = new DataGridView();
            pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridSummary).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridExp).BeginInit();
            SuspendLayout();
            // 
            pnlTop.Controls.Add(lblRange); pnlTop.Controls.Add(cmbRange);
            pnlTop.Controls.Add(dtpFrom); pnlTop.Controls.Add(dtpTo); pnlTop.Controls.Add(btnRefresh);
            pnlTop.Controls.Add(lblNet); pnlTop.Controls.Add(lblNote);
            pnlTop.Dock = DockStyle.Top; pnlTop.Name = "pnlTop"; pnlTop.Size = new Size(1100, 150); pnlTop.TabIndex = 0;
            lblRange.AutoSize = true; lblRange.Location = new Point(12, 18); lblRange.Name = "lblRange"; lblRange.Text = "Period";
            cmbRange.DropDownStyle = ComboBoxStyle.DropDownList; cmbRange.Location = new Point(90, 14); cmbRange.Name = "cmbRange"; cmbRange.Size = new Size(170, 33); cmbRange.TabIndex = 1;
            cmbRange.SelectedIndexChanged += cmbRange_Changed;
            dtpFrom.Format = DateTimePickerFormat.Custom; dtpFrom.CustomFormat = "dd-MMM-yyyy"; dtpFrom.Location = new Point(280, 14); dtpFrom.Name = "dtpFrom"; dtpFrom.Size = new Size(150, 31); dtpFrom.TabIndex = 2;
            dtpFrom.ValueChanged += Dates_Changed;
            dtpTo.Format = DateTimePickerFormat.Custom; dtpTo.CustomFormat = "dd-MMM-yyyy"; dtpTo.Location = new Point(440, 14); dtpTo.Name = "dtpTo"; dtpTo.Size = new Size(150, 31); dtpTo.TabIndex = 3;
            dtpTo.ValueChanged += Dates_Changed;
            btnRefresh.Location = new Point(610, 10); btnRefresh.Name = "btnRefresh"; btnRefresh.Size = new Size(120, 38); btnRefresh.TabIndex = 4;
            btnRefresh.Text = "Refresh"; btnRefresh.UseVisualStyleBackColor = true; btnRefresh.Click += btnRefresh_Click;
            lblNet.AutoSize = true; lblNet.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold); lblNet.Location = new Point(12, 62); lblNet.Name = "lblNet"; lblNet.Text = "";
            lblNote.AutoSize = true; lblNote.ForeColor = System.Drawing.Color.DimGray; lblNote.Location = new Point(12, 114); lblNote.Name = "lblNote"; lblNote.Text = "";
            // 
            gridSummary.AllowUserToAddRows = false; gridSummary.AllowUserToDeleteRows = false; gridSummary.AllowUserToResizeRows = false;
            gridSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; gridSummary.BackgroundColor = System.Drawing.Color.White; gridSummary.BorderStyle = BorderStyle.None;
            gridSummary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; gridSummary.Dock = DockStyle.Top;
            gridSummary.Name = "gridSummary"; gridSummary.ReadOnly = true; gridSummary.RowHeadersVisible = false; gridSummary.RowHeadersWidth = 62;
            gridSummary.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridSummary.Size = new Size(1100, 380); gridSummary.TabIndex = 5;
            gridSummary.DataBindingComplete += gridSummary_DataBindingComplete;
            // 
            gridExp.AllowUserToAddRows = false; gridExp.AllowUserToDeleteRows = false; gridExp.AllowUserToResizeRows = false;
            gridExp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; gridExp.BackgroundColor = System.Drawing.Color.WhiteSmoke; gridExp.BorderStyle = BorderStyle.None;
            gridExp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; gridExp.Dock = DockStyle.Fill;
            gridExp.Name = "gridExp"; gridExp.ReadOnly = true; gridExp.RowHeadersVisible = false; gridExp.RowHeadersWidth = 62;
            gridExp.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridExp.TabIndex = 6;
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 800);
            Controls.Add(gridExp); Controls.Add(gridSummary); Controls.Add(pnlTop);
            Name = "ProfitSummaryForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Profit Summary";
            Load += ProfitSummaryForm_Load;
            pnlTop.ResumeLayout(false); pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridSummary).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridExp).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private Label lblRange; private ComboBox cmbRange;
        private DateTimePicker dtpFrom; private DateTimePicker dtpTo;
        private Button btnRefresh;
        private Label lblNet; private Label lblNote;
        private DataGridView gridSummary; private DataGridView gridExp;
    }
}