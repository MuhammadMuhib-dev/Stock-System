namespace ClothStockSystem.Forms.Expenses
{
    partial class ExpenseForm
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
            grpEntry = new GroupBox();
            lblDate = new Label(); dtpDate = new DateTimePicker();
            lblCategory = new Label(); cmbCategory = new ComboBox();
            lblChannel = new Label(); cmbChannel = new ComboBox();
            lblAmount = new Label(); numAmount = new NumericUpDown();
            lblWorker = new Label(); cmbWorker = new ComboBox();
            lblForMonth = new Label(); dtpForMonth = new DateTimePicker();
            lblCut = new Label(); numCut = new NumericUpDown();
            lblMethod = new Label(); cmbMethod = new ComboBox();
            lblNotes = new Label(); txtNotes = new TextBox();
            lblMode = new Label(); lblHint = new Label();
            btnSave = new Button(); btnNew = new Button();
            pnlFilter = new Panel();
            lblMonth = new Label(); dtpMonth = new DateTimePicker(); chkAllMonths = new CheckBox();
            lblChFilter = new Label(); cmbChFilter = new ComboBox(); chkVoid = new CheckBox();
            btnSearch = new Button();
            lblSearch = new Label(); txtSearch = new TextBox();
            btnEdit = new Button(); btnVoid = new Button(); btnGenerate = new Button();
            grid = new DataGridView();
            pnlSummary = new Panel(); lblSum1 = new Label(); lblSum2 = new Label();
            grpEntry.SuspendLayout(); pnlFilter.SuspendLayout(); pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCut).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            // grpEntry
            // 
            grpEntry.Controls.Add(lblDate); grpEntry.Controls.Add(dtpDate);
            grpEntry.Controls.Add(lblCategory); grpEntry.Controls.Add(cmbCategory);
            grpEntry.Controls.Add(lblChannel); grpEntry.Controls.Add(cmbChannel);
            grpEntry.Controls.Add(lblAmount); grpEntry.Controls.Add(numAmount);
            grpEntry.Controls.Add(lblWorker); grpEntry.Controls.Add(cmbWorker);
            grpEntry.Controls.Add(lblForMonth); grpEntry.Controls.Add(dtpForMonth);
            grpEntry.Controls.Add(lblCut); grpEntry.Controls.Add(numCut);
            grpEntry.Controls.Add(lblMethod); grpEntry.Controls.Add(cmbMethod);
            grpEntry.Controls.Add(lblNotes); grpEntry.Controls.Add(txtNotes);
            grpEntry.Controls.Add(btnSave); grpEntry.Controls.Add(btnNew);
            grpEntry.Controls.Add(lblMode); grpEntry.Controls.Add(lblHint);
            grpEntry.Dock = DockStyle.Top; grpEntry.Name = "grpEntry"; grpEntry.Size = new Size(1100, 250);
            grpEntry.TabIndex = 0; grpEntry.TabStop = false; grpEntry.Text = "Enter Expense";
            // Row 1
            lblDate.AutoSize = true; lblDate.Location = new Point(12, 40); lblDate.Name = "lblDate"; lblDate.Text = "Tareekh";
            dtpDate.Format = DateTimePickerFormat.Custom; dtpDate.CustomFormat = "dd-MMM-yyyy"; dtpDate.Location = new Point(80, 36); dtpDate.Name = "dtpDate"; dtpDate.Size = new Size(140, 31); dtpDate.TabIndex = 1;
            dtpDate.ValueChanged += dtpDate_Changed;
            lblCategory.AutoSize = true; lblCategory.Location = new Point(240, 40); lblCategory.Name = "lblCategory"; lblCategory.Text = "Category";
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList; cmbCategory.Location = new Point(330, 36); cmbCategory.Name = "cmbCategory"; cmbCategory.Size = new Size(220, 33); cmbCategory.TabIndex = 2;
            cmbCategory.SelectedIndexChanged += cmbCategory_Changed;
            lblChannel.AutoSize = true; lblChannel.Location = new Point(570, 40); lblChannel.Name = "lblChannel"; lblChannel.Text = "Channel";
            cmbChannel.DropDownStyle = ComboBoxStyle.DropDownList; cmbChannel.Location = new Point(650, 36); cmbChannel.Name = "cmbChannel"; cmbChannel.Size = new Size(120, 33); cmbChannel.TabIndex = 3;
            lblAmount.AutoSize = true; lblAmount.Location = new Point(790, 40); lblAmount.Name = "lblAmount"; lblAmount.Text = "Amount";
            numAmount.DecimalPlaces = 2; numAmount.Increment = new decimal(new int[] { 100, 0, 0, 0 }); numAmount.Location = new Point(870, 36);
            numAmount.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 }); numAmount.Name = "numAmount"; numAmount.Size = new Size(150, 31);
            numAmount.TabIndex = 4; numAmount.TextAlign = HorizontalAlignment.Right; numAmount.ThousandsSeparator = true;
            numAmount.ValueChanged += Hint_Changed;
            // Row 2
            lblWorker.AutoSize = true; lblWorker.Location = new Point(12, 90); lblWorker.Name = "lblWorker"; lblWorker.Text = "Worker";
            cmbWorker.DropDownStyle = ComboBoxStyle.DropDownList; cmbWorker.Location = new Point(80, 86); cmbWorker.Name = "cmbWorker"; cmbWorker.Size = new Size(170, 33); cmbWorker.TabIndex = 5;
            cmbWorker.SelectedIndexChanged += cmbWorker_Changed;
            lblForMonth.AutoSize = true; lblForMonth.Location = new Point(270, 90); lblForMonth.Name = "lblForMonth"; lblForMonth.Text = "Kis mahine ka";
            dtpForMonth.Format = DateTimePickerFormat.Custom; dtpForMonth.CustomFormat = "MMM-yyyy"; dtpForMonth.ShowUpDown = true; dtpForMonth.Location = new Point(400, 86); dtpForMonth.Name = "dtpForMonth"; dtpForMonth.Size = new Size(120, 31); dtpForMonth.TabIndex = 6;
            lblCut.AutoSize = true; lblCut.Location = new Point(540, 90); lblCut.Name = "lblCut"; lblCut.Text = "Advance kata";
            numCut.DecimalPlaces = 2; numCut.Increment = new decimal(new int[] { 100, 0, 0, 0 }); numCut.Location = new Point(660, 86);
            numCut.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 }); numCut.Name = "numCut"; numCut.Size = new Size(120, 31);
            numCut.TabIndex = 7; numCut.TextAlign = HorizontalAlignment.Right; numCut.ThousandsSeparator = true;
            numCut.ValueChanged += Hint_Changed;
            lblMethod.AutoSize = true; lblMethod.Location = new Point(800, 90); lblMethod.Name = "lblMethod"; lblMethod.Text = "Payment";
            cmbMethod.DropDownStyle = ComboBoxStyle.DropDownList; cmbMethod.Location = new Point(880, 86); cmbMethod.Name = "cmbMethod"; cmbMethod.Size = new Size(140, 33); cmbMethod.TabIndex = 8;
            // Row 3
            lblNotes.AutoSize = true; lblNotes.Location = new Point(12, 140); lblNotes.Name = "lblNotes"; lblNotes.Text = "Notes";
            txtNotes.Location = new Point(80, 136); txtNotes.MaxLength = 250; txtNotes.Name = "txtNotes"; txtNotes.Size = new Size(700, 31); txtNotes.TabIndex = 9;
            btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215); btnSave.FlatAppearance.BorderSize = 0; btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = System.Drawing.Color.White; btnSave.Location = new Point(800, 132); btnSave.Name = "btnSave"; btnSave.Size = new Size(130, 40);
            btnSave.TabIndex = 10; btnSave.Text = "Save"; btnSave.UseVisualStyleBackColor = false; btnSave.Click += btnSave_Click;
            btnNew.Location = new Point(940, 132); btnNew.Name = "btnNew"; btnNew.Size = new Size(80, 40); btnNew.TabIndex = 11;
            btnNew.Text = "New"; btnNew.UseVisualStyleBackColor = true; btnNew.Click += btnNew_Click;
            // Row 4
            lblMode.AutoSize = true; lblMode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold); lblMode.Location = new Point(12, 188); lblMode.Name = "lblMode"; lblMode.Text = "";
            lblHint.AutoSize = true; lblHint.ForeColor = System.Drawing.Color.DimGray; lblHint.Location = new Point(12, 214); lblHint.Name = "lblHint"; lblHint.Text = "";
            // 
            // pnlFilter
            // 
            pnlFilter.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            pnlFilter.Controls.Add(lblMonth); pnlFilter.Controls.Add(dtpMonth); pnlFilter.Controls.Add(chkAllMonths);
            pnlFilter.Controls.Add(lblChFilter); pnlFilter.Controls.Add(cmbChFilter); pnlFilter.Controls.Add(chkVoid); pnlFilter.Controls.Add(btnSearch);
            pnlFilter.Controls.Add(lblSearch); pnlFilter.Controls.Add(txtSearch);
            pnlFilter.Controls.Add(btnEdit); pnlFilter.Controls.Add(btnVoid); pnlFilter.Controls.Add(btnGenerate);
            pnlFilter.Dock = DockStyle.Top; pnlFilter.Name = "pnlFilter"; pnlFilter.Size = new Size(1100, 104); pnlFilter.TabIndex = 12;
            lblMonth.AutoSize = true; lblMonth.Location = new Point(12, 16); lblMonth.Name = "lblMonth"; lblMonth.Text = "Mahina";
            dtpMonth.Format = DateTimePickerFormat.Custom; dtpMonth.CustomFormat = "MMM-yyyy"; dtpMonth.ShowUpDown = true; dtpMonth.Location = new Point(85, 12); dtpMonth.Name = "dtpMonth"; dtpMonth.Size = new Size(120, 31); dtpMonth.TabIndex = 13;
            dtpMonth.ValueChanged += Filter_Changed;
            chkAllMonths.AutoSize = true; chkAllMonths.Location = new Point(220, 14); chkAllMonths.Name = "chkAllMonths"; chkAllMonths.TabIndex = 14; chkAllMonths.Text = "Sab mahine"; chkAllMonths.UseVisualStyleBackColor = true;
            chkAllMonths.CheckedChanged += chkAllMonths_Changed;
            lblChFilter.AutoSize = true; lblChFilter.Location = new Point(380, 16); lblChFilter.Name = "lblChFilter"; lblChFilter.Text = "Channel";
            cmbChFilter.DropDownStyle = ComboBoxStyle.DropDownList; cmbChFilter.Location = new Point(460, 12); cmbChFilter.Name = "cmbChFilter"; cmbChFilter.Size = new Size(120, 33); cmbChFilter.TabIndex = 15;
            cmbChFilter.SelectedIndexChanged += Filter_Changed;
            chkVoid.AutoSize = true; chkVoid.Location = new Point(600, 14); chkVoid.Name = "chkVoid"; chkVoid.TabIndex = 16; chkVoid.Text = "Delete hui bhi dikhayen"; chkVoid.UseVisualStyleBackColor = true;
            chkVoid.CheckedChanged += Filter_Changed;
            btnSearch.Location = new Point(880, 10); btnSearch.Name = "btnSearch"; btnSearch.Size = new Size(120, 36); btnSearch.TabIndex = 17;
            btnSearch.Text = "Refresh"; btnSearch.UseVisualStyleBackColor = true; btnSearch.Click += btnSearch_Click;
            lblSearch.AutoSize = true; lblSearch.Location = new Point(12, 62); lblSearch.Name = "lblSearch"; lblSearch.Text = "Search";
            txtSearch.Location = new Point(85, 58); txtSearch.Name = "txtSearch"; txtSearch.PlaceholderText = "Category, worker ya notes"; txtSearch.Size = new Size(250, 31); txtSearch.TabIndex = 18;
            txtSearch.KeyDown += txtSearch_KeyDown;
            btnEdit.Location = new Point(350, 54); btnEdit.Name = "btnEdit"; btnEdit.Size = new Size(140, 38); btnEdit.TabIndex = 19;
            btnEdit.Text = "Edit Selected"; btnEdit.UseVisualStyleBackColor = true; btnEdit.Click += btnEdit_Click;
            btnVoid.BackColor = System.Drawing.Color.FromArgb(200, 40, 40); btnVoid.FlatAppearance.BorderSize = 0; btnVoid.FlatStyle = FlatStyle.Flat;
            btnVoid.ForeColor = System.Drawing.Color.White; btnVoid.Location = new Point(500, 54); btnVoid.Name = "btnVoid"; btnVoid.Size = new Size(150, 38);
            btnVoid.TabIndex = 20; btnVoid.Text = "Delete Selected"; btnVoid.UseVisualStyleBackColor = false; btnVoid.Click += btnVoid_Click;
            btnGenerate.Location = new Point(670, 54); btnGenerate.Name = "btnGenerate"; btnGenerate.Size = new Size(350, 38); btnGenerate.TabIndex = 21;
            btnGenerate.Text = "Is mahine ke fixed kharche banao"; btnGenerate.UseVisualStyleBackColor = true; btnGenerate.Click += btnGenerate_Click;
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; grid.BackgroundColor = System.Drawing.Color.White; grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; grid.Dock = DockStyle.Fill; grid.MultiSelect = false;
            grid.Name = "grid"; grid.ReadOnly = true; grid.RowHeadersVisible = false; grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.TabIndex = 22;
            grid.DataBindingComplete += grid_DataBindingComplete;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = System.Drawing.Color.FromArgb(240, 246, 255);
            pnlSummary.Controls.Add(lblSum1); pnlSummary.Controls.Add(lblSum2);
            pnlSummary.Dock = DockStyle.Bottom; pnlSummary.Name = "pnlSummary"; pnlSummary.Size = new Size(1100, 90); pnlSummary.TabIndex = 23;
            lblSum1.AutoSize = true; lblSum1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold); lblSum1.ForeColor = System.Drawing.Color.FromArgb(0, 70, 140); lblSum1.Location = new Point(12, 10); lblSum1.Name = "lblSum1"; lblSum1.Text = "";
            lblSum2.AutoSize = true; lblSum2.Location = new Point(12, 52); lblSum2.Name = "lblSum2"; lblSum2.Text = "";
            // 
            // ExpenseForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 800);
            Controls.Add(grid); Controls.Add(pnlSummary); Controls.Add(pnlFilter); Controls.Add(grpEntry);
            Name = "ExpenseForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Expenses";
            Load += ExpenseForm_Load;
            grpEntry.ResumeLayout(false); grpEntry.PerformLayout();
            pnlFilter.ResumeLayout(false); pnlFilter.PerformLayout();
            pnlSummary.ResumeLayout(false); pnlSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAmount).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCut).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpEntry;
        private Label lblDate; private DateTimePicker dtpDate;
        private Label lblCategory; private ComboBox cmbCategory;
        private Label lblChannel; private ComboBox cmbChannel;
        private Label lblAmount; private NumericUpDown numAmount;
        private Label lblWorker; private ComboBox cmbWorker;
        private Label lblForMonth; private DateTimePicker dtpForMonth;
        private Label lblCut; private NumericUpDown numCut;
        private Label lblMethod; private ComboBox cmbMethod;
        private Label lblNotes; private TextBox txtNotes;
        private Label lblMode; private Label lblHint;
        private Button btnSave; private Button btnNew;
        private Panel pnlFilter;
        private Label lblMonth; private DateTimePicker dtpMonth; private CheckBox chkAllMonths;
        private Label lblChFilter; private ComboBox cmbChFilter; private CheckBox chkVoid;
        private Button btnSearch;
        private Label lblSearch; private TextBox txtSearch;
        private Button btnEdit; private Button btnVoid; private Button btnGenerate;
        private DataGridView grid;
        private Panel pnlSummary; private Label lblSum1; private Label lblSum2;
    }
}