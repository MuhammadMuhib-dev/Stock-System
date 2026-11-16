namespace ClothStockSystem.Forms.Expenses
{
    partial class ExpenseSetupForm
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
            tabs = new TabControl(); tabCat = new TabPage(); tabPay = new TabPage(); tabRec = new TabPage();
            // categories
            grpCat = new GroupBox(); gridCat = new DataGridView();
            lblCName = new Label(); txtCName = new TextBox();
            lblCGroup = new Label(); txtCGroup = new TextBox();
            lblKind = new Label(); cmbKind = new ComboBox();
            lblCCh = new Label(); cmbCCh = new ComboBox();
            chkCActive = new CheckBox();
            btnCSave = new Button(); btnCNew = new Button(); btnCDelete = new Button();
            lblKindHelp = new Label();
            // payment methods
            pnlPay = new Panel(); gridPay = new DataGridView();
            lblMName = new Label(); txtMName = new TextBox(); btnMAdd = new Button(); btnMToggle = new Button();
            // recurring
            grpRec = new GroupBox(); gridRec = new DataGridView();
            lblRCat = new Label(); cmbRCat = new ComboBox();
            lblRCh = new Label(); cmbRCh = new ComboBox();
            lblRWorker = new Label(); cmbRWorker = new ComboBox();
            lblRAmt = new Label(); numRAmt = new NumericUpDown(); chkRSalary = new CheckBox();
            lblRMethod = new Label(); cmbRMethod = new ComboBox();
            lblRNotes = new Label(); txtRNotes = new TextBox();
            chkRActive = new CheckBox();
            btnRSave = new Button(); btnRNew = new Button(); btnRDelete = new Button();
            tabs.SuspendLayout(); tabCat.SuspendLayout(); tabPay.SuspendLayout(); tabRec.SuspendLayout();
            grpCat.SuspendLayout(); pnlPay.SuspendLayout(); grpRec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCat).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridPay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridRec).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRAmt).BeginInit();
            SuspendLayout();
            // 
            tabs.Controls.Add(tabCat); tabs.Controls.Add(tabPay); tabs.Controls.Add(tabRec);
            tabs.Dock = DockStyle.Fill; tabs.Name = "tabs"; tabs.TabIndex = 0;
            tabCat.Controls.Add(gridCat); tabCat.Controls.Add(grpCat); tabCat.Name = "tabCat"; tabCat.Text = "Categories"; tabCat.UseVisualStyleBackColor = true;
            tabPay.Controls.Add(gridPay); tabPay.Controls.Add(pnlPay); tabPay.Name = "tabPay"; tabPay.Text = "Payment Methods"; tabPay.UseVisualStyleBackColor = true;
            tabRec.Text = "Fixed Expenses (rent, bills, salary)";
            // ===== Categories tab
            grpCat.Controls.Add(lblCName); grpCat.Controls.Add(txtCName);
            grpCat.Controls.Add(lblCGroup); grpCat.Controls.Add(txtCGroup);
            grpCat.Controls.Add(lblKind); grpCat.Controls.Add(cmbKind);
            grpCat.Controls.Add(lblCCh); grpCat.Controls.Add(cmbCCh); grpCat.Controls.Add(chkCActive);
            grpCat.Controls.Add(btnCSave); grpCat.Controls.Add(btnCNew); grpCat.Controls.Add(btnCDelete);
            grpCat.Controls.Add(lblKindHelp);
            grpCat.Dock = DockStyle.Top; grpCat.Name = "grpCat"; grpCat.Size = new Size(1000, 200); grpCat.TabIndex = 0; grpCat.TabStop = false; grpCat.Text = "Category";
            lblCName.AutoSize = true; lblCName.Location = new Point(12, 40); lblCName.Name = "lblCName"; lblCName.Text = "Name";
            txtCName.Location = new Point(100, 36); txtCName.MaxLength = 60; txtCName.Name = "txtCName"; txtCName.Size = new Size(220, 31); txtCName.TabIndex = 1;
            lblCGroup.AutoSize = true; lblCGroup.Location = new Point(340, 40); lblCGroup.Name = "lblCGroup"; lblCGroup.Text = "Group";
            txtCGroup.Location = new Point(410, 36); txtCGroup.MaxLength = 40; txtCGroup.Name = "txtCGroup"; txtCGroup.PlaceholderText = "Shop, Staff, Marketing"; txtCGroup.Size = new Size(190, 31); txtCGroup.TabIndex = 2;
            lblKind.AutoSize = true; lblKind.Location = new Point(620, 40); lblKind.Name = "lblKind"; lblKind.Text = "Type";
            cmbKind.DropDownStyle = ComboBoxStyle.DropDownList; cmbKind.Location = new Point(690, 36); cmbKind.Name = "cmbKind"; cmbKind.Size = new Size(130, 33); cmbKind.TabIndex = 3;
            lblCCh.AutoSize = true; lblCCh.Location = new Point(12, 92); lblCCh.Name = "lblCCh"; lblCCh.Text = "Default channel";
            cmbCCh.DropDownStyle = ComboBoxStyle.DropDownList; cmbCCh.Location = new Point(170, 88); cmbCCh.Name = "cmbCCh"; cmbCCh.Size = new Size(130, 33); cmbCCh.TabIndex = 4;
            chkCActive.AutoSize = true; chkCActive.Checked = true; chkCActive.CheckState = CheckState.Checked; chkCActive.Location = new Point(320, 90); chkCActive.Name = "chkCActive"; chkCActive.TabIndex = 5; chkCActive.Text = "Active"; chkCActive.UseVisualStyleBackColor = true;
            btnCSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215); btnCSave.FlatAppearance.BorderSize = 0; btnCSave.FlatStyle = FlatStyle.Flat;
            btnCSave.ForeColor = System.Drawing.Color.White; btnCSave.Location = new Point(430, 84); btnCSave.Name = "btnCSave"; btnCSave.Size = new Size(110, 40);
            btnCSave.TabIndex = 6; btnCSave.Text = "Save"; btnCSave.UseVisualStyleBackColor = false; btnCSave.Click += btnCSave_Click;
            btnCNew.Location = new Point(550, 84); btnCNew.Name = "btnCNew"; btnCNew.Size = new Size(110, 40); btnCNew.TabIndex = 7;
            btnCNew.Text = "New"; btnCNew.UseVisualStyleBackColor = true; btnCNew.Click += btnCNew_Click;
            btnCDelete.BackColor = System.Drawing.Color.FromArgb(200, 40, 40); btnCDelete.FlatAppearance.BorderSize = 0; btnCDelete.FlatStyle = FlatStyle.Flat;
            btnCDelete.ForeColor = System.Drawing.Color.White; btnCDelete.Location = new Point(670, 84); btnCDelete.Name = "btnCDelete"; btnCDelete.Size = new Size(110, 40);
            btnCDelete.TabIndex = 8; btnCDelete.Text = "Delete"; btnCDelete.UseVisualStyleBackColor = false; btnCDelete.Click += btnCDelete_Click;
            lblKindHelp.AutoSize = true; lblKindHelp.ForeColor = System.Drawing.Color.DimGray; lblKindHelp.Location = new Point(12, 138); lblKindHelp.Name = "lblKindHelp";
            lblKindHelp.Text = "Type: Normal = regular expense  |  Salary / Bonus / Advance = worker selected (advance deducted from salary).";
            gridCat.AllowUserToAddRows = false; gridCat.AllowUserToDeleteRows = false; gridCat.AllowUserToResizeRows = false;
            gridCat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; gridCat.BackgroundColor = System.Drawing.Color.White; gridCat.BorderStyle = BorderStyle.None;
            gridCat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; gridCat.Dock = DockStyle.Fill; gridCat.MultiSelect = false;
            gridCat.Name = "gridCat"; gridCat.ReadOnly = true; gridCat.RowHeadersVisible = false; gridCat.RowHeadersWidth = 62;
            gridCat.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridCat.TabIndex = 9;
            gridCat.SelectionChanged += gridCat_SelectionChanged;
            // ===== Payment tab
            pnlPay.Controls.Add(lblMName); pnlPay.Controls.Add(txtMName); pnlPay.Controls.Add(btnMAdd); pnlPay.Controls.Add(btnMToggle);
            pnlPay.Dock = DockStyle.Top; pnlPay.Name = "pnlPay"; pnlPay.Size = new Size(1000, 76); pnlPay.TabIndex = 0;
            lblMName.AutoSize = true; lblMName.Location = new Point(12, 26); lblMName.Name = "lblMName"; lblMName.Text = "New method";
            txtMName.Location = new Point(150, 22); txtMName.MaxLength = 30; txtMName.Name = "txtMName"; txtMName.PlaceholderText = "JazzCash, Bank..."; txtMName.Size = new Size(200, 31); txtMName.TabIndex = 1;
            btnMAdd.Location = new Point(365, 18); btnMAdd.Name = "btnMAdd"; btnMAdd.Size = new Size(110, 38); btnMAdd.TabIndex = 2;
            btnMAdd.Text = "Add"; btnMAdd.UseVisualStyleBackColor = true; btnMAdd.Click += btnMAdd_Click;
            btnMToggle.Location = new Point(490, 18); btnMToggle.Name = "btnMToggle"; btnMToggle.Size = new Size(300, 38); btnMToggle.TabIndex = 3;
            btnMToggle.Text = "Selected: Active / Disable";
            gridPay.AllowUserToAddRows = false; gridPay.AllowUserToDeleteRows = false; gridPay.AllowUserToResizeRows = false;
            gridPay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; gridPay.BackgroundColor = System.Drawing.Color.White; gridPay.BorderStyle = BorderStyle.None;
            gridPay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; gridPay.Dock = DockStyle.Fill; gridPay.MultiSelect = false;
            gridPay.Name = "gridPay"; gridPay.ReadOnly = true; gridPay.RowHeadersVisible = false; gridPay.RowHeadersWidth = 62;
            gridPay.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridPay.TabIndex = 4;
            // ===== Recurring tab
            grpRec.Controls.Add(lblRCat); grpRec.Controls.Add(cmbRCat);
            grpRec.Controls.Add(lblRCh); grpRec.Controls.Add(cmbRCh);
            grpRec.Controls.Add(lblRWorker); grpRec.Controls.Add(cmbRWorker);
            grpRec.Controls.Add(lblRAmt); grpRec.Controls.Add(numRAmt); grpRec.Controls.Add(chkRSalary);
            grpRec.Controls.Add(lblRMethod); grpRec.Controls.Add(cmbRMethod);
            grpRec.Controls.Add(lblRNotes); grpRec.Controls.Add(txtRNotes); grpRec.Controls.Add(chkRActive);
            grpRec.Controls.Add(btnRSave); grpRec.Controls.Add(btnRNew); grpRec.Controls.Add(btnRDelete);
            grpRec.Text = "Fixed expenses (created with one click at start of month)";
            lblRCat.AutoSize = true; lblRCat.Location = new Point(12, 40); lblRCat.Name = "lblRCat"; lblRCat.Text = "Category";
            cmbRCat.DropDownStyle = ComboBoxStyle.DropDownList; cmbRCat.Location = new Point(100, 36); cmbRCat.Name = "cmbRCat"; cmbRCat.Size = new Size(220, 33); cmbRCat.TabIndex = 1;
            lblRCh.AutoSize = true; lblRCh.Location = new Point(340, 40); lblRCh.Name = "lblRCh"; lblRCh.Text = "Channel";
            cmbRCh.DropDownStyle = ComboBoxStyle.DropDownList; cmbRCh.Location = new Point(420, 36); cmbRCh.Name = "cmbRCh"; cmbRCh.Size = new Size(110, 33); cmbRCh.TabIndex = 2;
            lblRWorker.AutoSize = true; lblRWorker.Location = new Point(550, 40); lblRWorker.Name = "lblRWorker"; lblRWorker.Text = "Worker";
            cmbRWorker.DropDownStyle = ComboBoxStyle.DropDownList; cmbRWorker.Location = new Point(630, 36); cmbRWorker.Name = "cmbRWorker"; cmbRWorker.Size = new Size(200, 33); cmbRWorker.TabIndex = 3;
            lblRAmt.AutoSize = true; lblRAmt.Location = new Point(12, 92); lblRAmt.Name = "lblRAmt"; lblRAmt.Text = "Amount";
            numRAmt.DecimalPlaces = 2; numRAmt.Increment = new decimal(new int[] { 100, 0, 0, 0 }); numRAmt.Location = new Point(100, 88);
            numRAmt.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 }); numRAmt.Name = "numRAmt"; numRAmt.Size = new Size(140, 31);
            numRAmt.TabIndex = 4; numRAmt.TextAlign = HorizontalAlignment.Right; numRAmt.ThousandsSeparator = true;
            chkRSalary.AutoSize = true; chkRSalary.Location = new Point(255, 90); chkRSalary.Name = "chkRSalary"; chkRSalary.TabIndex = 5;
            chkRSalary.Text = "Salary: worker ki mahana salary khud aaye"; chkRSalary.UseVisualStyleBackColor = true; chkRSalary.CheckedChanged += chkRSalary_Changed;
            lblRMethod.AutoSize = true; lblRMethod.Location = new Point(620, 92); lblRMethod.Name = "lblRMethod"; lblRMethod.Text = "Payment";
            cmbRMethod.DropDownStyle = ComboBoxStyle.DropDownList; cmbRMethod.Location = new Point(710, 88); cmbRMethod.Name = "cmbRMethod"; cmbRMethod.Size = new Size(120, 33); cmbRMethod.TabIndex = 6;
            lblRNotes.AutoSize = true; lblRNotes.Location = new Point(12, 144); lblRNotes.Name = "lblRNotes"; lblRNotes.Text = "Notes";
            txtRNotes.Location = new Point(100, 140); txtRNotes.MaxLength = 250; txtRNotes.Name = "txtRNotes"; txtRNotes.Size = new Size(350, 31); txtRNotes.TabIndex = 7;
            chkRActive.AutoSize = true; chkRActive.Checked = true; chkRActive.CheckState = CheckState.Checked; chkRActive.Location = new Point(470, 142); chkRActive.Name = "chkRActive"; chkRActive.TabIndex = 8; chkRActive.Text = "Active"; chkRActive.UseVisualStyleBackColor = true;
            btnRSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215); btnRSave.FlatAppearance.BorderSize = 0; btnRSave.FlatStyle = FlatStyle.Flat;
            btnRSave.ForeColor = System.Drawing.Color.White; btnRSave.Location = new Point(570, 136); btnRSave.Name = "btnRSave"; btnRSave.Size = new Size(100, 40);
            btnRSave.TabIndex = 9; btnRSave.Text = "Save"; btnRSave.UseVisualStyleBackColor = false; btnRSave.Click += btnRSave_Click;
            btnRNew.Location = new Point(680, 136); btnRNew.Name = "btnRNew"; btnRNew.Size = new Size(100, 40); btnRNew.TabIndex = 10;
            btnRNew.Text = "New"; btnRNew.UseVisualStyleBackColor = true; btnRNew.Click += btnRNew_Click;
            btnRDelete.BackColor = System.Drawing.Color.FromArgb(200, 40, 40); btnRDelete.FlatAppearance.BorderSize = 0; btnRDelete.FlatStyle = FlatStyle.Flat;
            btnRDelete.ForeColor = System.Drawing.Color.White; btnRDelete.Location = new Point(790, 136); btnRDelete.Name = "btnRDelete"; btnRDelete.Size = new Size(100, 40);
            btnRDelete.TabIndex = 11; btnRDelete.Text = "Delete"; btnRDelete.UseVisualStyleBackColor = false; btnRDelete.Click += btnRDelete_Click;
            gridRec.AllowUserToAddRows = false; gridRec.AllowUserToDeleteRows = false; gridRec.AllowUserToResizeRows = false;
            gridRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; gridRec.BackgroundColor = System.Drawing.Color.White; gridRec.BorderStyle = BorderStyle.None;
            gridRec.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; gridRec.Dock = DockStyle.Fill; gridRec.MultiSelect = false;
            gridRec.Name = "gridRec"; gridRec.ReadOnly = true; gridRec.RowHeadersVisible = false; gridRec.RowHeadersWidth = 62;
            gridRec.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridRec.TabIndex = 12;
            gridRec.SelectionChanged += gridRec_SelectionChanged;
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 700);
            Controls.Add(tabs);
            Name = "ExpenseSetupForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Expense Setup";
            Load += ExpenseSetupForm_Load;
            grpCat.ResumeLayout(false); grpCat.PerformLayout();
            pnlPay.ResumeLayout(false); pnlPay.PerformLayout();
            grpRec.ResumeLayout(false); grpRec.PerformLayout();
            tabCat.ResumeLayout(false); tabPay.ResumeLayout(false); tabRec.ResumeLayout(false); tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridCat).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridPay).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridRec).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRAmt).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabs; private TabPage tabCat; private TabPage tabPay; private TabPage tabRec;
        private GroupBox grpCat; private DataGridView gridCat;
        private Label lblCName; private TextBox txtCName;
        private Label lblCGroup; private TextBox txtCGroup;
        private Label lblKind; private ComboBox cmbKind;
        private Label lblCCh; private ComboBox cmbCCh;
        private CheckBox chkCActive;
        private Button btnCSave; private Button btnCNew; private Button btnCDelete;
        private Label lblKindHelp;
        private Panel pnlPay; private DataGridView gridPay;
        private Label lblMName; private TextBox txtMName; private Button btnMAdd; private Button btnMToggle;
        private GroupBox grpRec; private DataGridView gridRec;
        private Label lblRCat; private ComboBox cmbRCat;
        private Label lblRCh; private ComboBox cmbRCh;
        private Label lblRWorker; private ComboBox cmbRWorker;
        private Label lblRAmt; private NumericUpDown numRAmt; private CheckBox chkRSalary;
        private Label lblRMethod; private ComboBox cmbRMethod;
        private Label lblRNotes; private TextBox txtRNotes;
        private CheckBox chkRActive;
        private Button btnRSave; private Button btnRNew; private Button btnRDelete;
    }
}