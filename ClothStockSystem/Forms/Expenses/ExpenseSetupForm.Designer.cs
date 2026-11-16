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
            tabs = new TabControl();
            tabCat = new TabPage();
            gridCat = new DataGridView();
            grpCat = new GroupBox();
            lblCName = new Label();
            txtCName = new TextBox();
            lblCGroup = new Label();
            txtCGroup = new TextBox();
            lblKind = new Label();
            cmbKind = new ComboBox();
            lblCCh = new Label();
            cmbCCh = new ComboBox();
            chkCActive = new CheckBox();
            btnCSave = new Button();
            btnCNew = new Button();
            btnCDelete = new Button();
            lblKindHelp = new Label();
            tabPay = new TabPage();
            gridPay = new DataGridView();
            pnlPay = new Panel();
            lblMName = new Label();
            txtMName = new TextBox();
            btnMAdd = new Button();
            btnMToggle = new Button();
            tabRec = new TabPage();
            grpRec = new GroupBox();
            lblRCat = new Label();
            cmbRCat = new ComboBox();
            lblRCh = new Label();
            cmbRCh = new ComboBox();
            lblRWorker = new Label();
            cmbRWorker = new ComboBox();
            lblRAmt = new Label();
            numRAmt = new NumericUpDown();
            chkRSalary = new CheckBox();
            lblRMethod = new Label();
            cmbRMethod = new ComboBox();
            lblRNotes = new Label();
            txtRNotes = new TextBox();
            chkRActive = new CheckBox();
            btnRSave = new Button();
            btnRNew = new Button();
            btnRDelete = new Button();
            gridRec = new DataGridView();
            tabs.SuspendLayout();
            tabCat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridCat).BeginInit();
            grpCat.SuspendLayout();
            tabPay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridPay).BeginInit();
            pnlPay.SuspendLayout();
            grpRec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRAmt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridRec).BeginInit();
            SuspendLayout();
            // 
            // tabs
            // 
            tabs.Controls.Add(tabCat);
            tabs.Controls.Add(tabPay);
            tabs.Controls.Add(tabRec);
            tabs.Dock = DockStyle.Fill;
            tabs.Location = new Point(0, 0);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(1000, 700);
            tabs.TabIndex = 0;
            // 
            // tabCat
            // 
            tabCat.Controls.Add(gridCat);
            tabCat.Controls.Add(grpCat);
            tabCat.Location = new Point(4, 34);
            tabCat.Name = "tabCat";
            tabCat.Size = new Size(992, 662);
            tabCat.TabIndex = 0;
            tabCat.Text = "Categories";
            tabCat.UseVisualStyleBackColor = true;
            // 
            // gridCat
            // 
            gridCat.AllowUserToAddRows = false;
            gridCat.AllowUserToDeleteRows = false;
            gridCat.AllowUserToResizeRows = false;
            gridCat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridCat.BackgroundColor = Color.White;
            gridCat.BorderStyle = BorderStyle.None;
            gridCat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridCat.Dock = DockStyle.Fill;
            gridCat.Location = new Point(0, 200);
            gridCat.MultiSelect = false;
            gridCat.Name = "gridCat";
            gridCat.ReadOnly = true;
            gridCat.RowHeadersVisible = false;
            gridCat.RowHeadersWidth = 62;
            gridCat.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridCat.Size = new Size(992, 462);
            gridCat.TabIndex = 9;
            gridCat.SelectionChanged += gridCat_SelectionChanged;
            // 
            // grpCat
            // 
            grpCat.Controls.Add(lblCName);
            grpCat.Controls.Add(txtCName);
            grpCat.Controls.Add(lblCGroup);
            grpCat.Controls.Add(txtCGroup);
            grpCat.Controls.Add(lblKind);
            grpCat.Controls.Add(cmbKind);
            grpCat.Controls.Add(lblCCh);
            grpCat.Controls.Add(cmbCCh);
            grpCat.Controls.Add(chkCActive);
            grpCat.Controls.Add(btnCSave);
            grpCat.Controls.Add(btnCNew);
            grpCat.Controls.Add(btnCDelete);
            grpCat.Controls.Add(lblKindHelp);
            grpCat.Dock = DockStyle.Top;
            grpCat.Location = new Point(0, 0);
            grpCat.Name = "grpCat";
            grpCat.Size = new Size(992, 200);
            grpCat.TabIndex = 0;
            grpCat.TabStop = false;
            grpCat.Text = "Category";
            // 
            // lblCName
            // 
            lblCName.AutoSize = true;
            lblCName.Location = new Point(12, 40);
            lblCName.Name = "lblCName";
            lblCName.Size = new Size(59, 25);
            lblCName.TabIndex = 0;
            lblCName.Text = "Name";
            // 
            // txtCName
            // 
            txtCName.Location = new Point(100, 36);
            txtCName.MaxLength = 60;
            txtCName.Name = "txtCName";
            txtCName.Size = new Size(220, 31);
            txtCName.TabIndex = 1;
            // 
            // lblCGroup
            // 
            lblCGroup.AutoSize = true;
            lblCGroup.Location = new Point(340, 40);
            lblCGroup.Name = "lblCGroup";
            lblCGroup.Size = new Size(62, 25);
            lblCGroup.TabIndex = 2;
            lblCGroup.Text = "Group";
            // 
            // txtCGroup
            // 
            txtCGroup.Location = new Point(410, 36);
            txtCGroup.MaxLength = 40;
            txtCGroup.Name = "txtCGroup";
            txtCGroup.PlaceholderText = "Shop, Staff, Marketing";
            txtCGroup.Size = new Size(190, 31);
            txtCGroup.TabIndex = 2;
            // 
            // lblKind
            // 
            lblKind.AutoSize = true;
            lblKind.Location = new Point(620, 40);
            lblKind.Name = "lblKind";
            lblKind.Size = new Size(49, 25);
            lblKind.TabIndex = 3;
            lblKind.Text = "Type";
            // 
            // cmbKind
            // 
            cmbKind.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKind.Location = new Point(690, 36);
            cmbKind.Name = "cmbKind";
            cmbKind.Size = new Size(130, 33);
            cmbKind.TabIndex = 3;
            // 
            // lblCCh
            // 
            lblCCh.AutoSize = true;
            lblCCh.Location = new Point(12, 92);
            lblCCh.Name = "lblCCh";
            lblCCh.Size = new Size(134, 25);
            lblCCh.TabIndex = 4;
            lblCCh.Text = "Default channel";
            // 
            // cmbCCh
            // 
            cmbCCh.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCCh.Location = new Point(170, 88);
            cmbCCh.Name = "cmbCCh";
            cmbCCh.Size = new Size(130, 33);
            cmbCCh.TabIndex = 4;
            // 
            // chkCActive
            // 
            chkCActive.AutoSize = true;
            chkCActive.Checked = true;
            chkCActive.CheckState = CheckState.Checked;
            chkCActive.Location = new Point(320, 90);
            chkCActive.Name = "chkCActive";
            chkCActive.Size = new Size(86, 29);
            chkCActive.TabIndex = 5;
            chkCActive.Text = "Active";
            chkCActive.UseVisualStyleBackColor = true;
            // 
            // btnCSave
            // 
            btnCSave.BackColor = Color.FromArgb(0, 120, 215);
            btnCSave.FlatAppearance.BorderSize = 0;
            btnCSave.FlatStyle = FlatStyle.Flat;
            btnCSave.ForeColor = Color.White;
            btnCSave.Location = new Point(430, 84);
            btnCSave.Name = "btnCSave";
            btnCSave.Size = new Size(110, 40);
            btnCSave.TabIndex = 6;
            btnCSave.Text = "Save";
            btnCSave.UseVisualStyleBackColor = false;
            btnCSave.Click += btnCSave_Click;
            // 
            // btnCNew
            // 
            btnCNew.Location = new Point(550, 84);
            btnCNew.Name = "btnCNew";
            btnCNew.Size = new Size(110, 40);
            btnCNew.TabIndex = 7;
            btnCNew.Text = "New";
            btnCNew.UseVisualStyleBackColor = true;
            btnCNew.Click += btnCNew_Click;
            // 
            // btnCDelete
            // 
            btnCDelete.BackColor = Color.FromArgb(200, 40, 40);
            btnCDelete.FlatAppearance.BorderSize = 0;
            btnCDelete.FlatStyle = FlatStyle.Flat;
            btnCDelete.ForeColor = Color.White;
            btnCDelete.Location = new Point(670, 84);
            btnCDelete.Name = "btnCDelete";
            btnCDelete.Size = new Size(110, 40);
            btnCDelete.TabIndex = 8;
            btnCDelete.Text = "Delete";
            btnCDelete.UseVisualStyleBackColor = false;
            btnCDelete.Click += btnCDelete_Click;
            // 
            // lblKindHelp
            // 
            lblKindHelp.AutoSize = true;
            lblKindHelp.ForeColor = Color.DimGray;
            lblKindHelp.Location = new Point(12, 138);
            lblKindHelp.Name = "lblKindHelp";
            lblKindHelp.Size = new Size(888, 25);
            lblKindHelp.TabIndex = 9;
            lblKindHelp.Text = "Type: Normal = regular expense  |  Salary / Bonus / Advance = worker selected (advance deducted from salary).";
            // 
            // tabPay
            // 
            tabPay.Controls.Add(gridPay);
            tabPay.Controls.Add(pnlPay);
            tabPay.Location = new Point(4, 34);
            tabPay.Name = "tabPay";
            tabPay.Size = new Size(992, 662);
            tabPay.TabIndex = 1;
            tabPay.Text = "Payment Methods";
            tabPay.UseVisualStyleBackColor = true;
            // 
            // gridPay
            // 
            gridPay.AllowUserToAddRows = false;
            gridPay.AllowUserToDeleteRows = false;
            gridPay.AllowUserToResizeRows = false;
            gridPay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridPay.BackgroundColor = Color.White;
            gridPay.BorderStyle = BorderStyle.None;
            gridPay.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridPay.Dock = DockStyle.Fill;
            gridPay.Location = new Point(0, 76);
            gridPay.MultiSelect = false;
            gridPay.Name = "gridPay";
            gridPay.ReadOnly = true;
            gridPay.RowHeadersVisible = false;
            gridPay.RowHeadersWidth = 62;
            gridPay.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridPay.Size = new Size(992, 586);
            gridPay.TabIndex = 4;
            // 
            // pnlPay
            // 
            pnlPay.Controls.Add(lblMName);
            pnlPay.Controls.Add(txtMName);
            pnlPay.Controls.Add(btnMAdd);
            pnlPay.Controls.Add(btnMToggle);
            pnlPay.Dock = DockStyle.Top;
            pnlPay.Location = new Point(0, 0);
            pnlPay.Name = "pnlPay";
            pnlPay.Size = new Size(992, 76);
            pnlPay.TabIndex = 0;
            // 
            // lblMName
            // 
            lblMName.AutoSize = true;
            lblMName.Location = new Point(12, 26);
            lblMName.Name = "lblMName";
            lblMName.Size = new Size(115, 25);
            lblMName.TabIndex = 0;
            lblMName.Text = "New method";
            // 
            // txtMName
            // 
            txtMName.Location = new Point(150, 22);
            txtMName.MaxLength = 30;
            txtMName.Name = "txtMName";
            txtMName.PlaceholderText = "JazzCash, Bank...";
            txtMName.Size = new Size(200, 31);
            txtMName.TabIndex = 1;
            // 
            // btnMAdd
            // 
            btnMAdd.Location = new Point(365, 18);
            btnMAdd.Name = "btnMAdd";
            btnMAdd.Size = new Size(110, 38);
            btnMAdd.TabIndex = 2;
            btnMAdd.Text = "Add";
            btnMAdd.UseVisualStyleBackColor = true;
            btnMAdd.Click += btnMAdd_Click;
            // 
            // btnMToggle
            // 
            btnMToggle.Location = new Point(490, 18);
            btnMToggle.Name = "btnMToggle";
            btnMToggle.Size = new Size(300, 38);
            btnMToggle.TabIndex = 3;
            btnMToggle.Text = "Selected: Active / Disable";
            // 
            // tabRec
            // 
            tabRec.Location = new Point(4, 34);
            tabRec.Name = "tabRec";
            tabRec.Size = new Size(992, 662);
            tabRec.TabIndex = 2;
            tabRec.Text = "Fixed Expenses (rent, bills, salary)";
            // 
            // grpRec
            // 
            grpRec.Controls.Add(lblRCat);
            grpRec.Controls.Add(cmbRCat);
            grpRec.Controls.Add(lblRCh);
            grpRec.Controls.Add(cmbRCh);
            grpRec.Controls.Add(lblRWorker);
            grpRec.Controls.Add(cmbRWorker);
            grpRec.Controls.Add(lblRAmt);
            grpRec.Controls.Add(numRAmt);
            grpRec.Controls.Add(chkRSalary);
            grpRec.Controls.Add(lblRMethod);
            grpRec.Controls.Add(cmbRMethod);
            grpRec.Controls.Add(lblRNotes);
            grpRec.Controls.Add(txtRNotes);
            grpRec.Controls.Add(chkRActive);
            grpRec.Controls.Add(btnRSave);
            grpRec.Controls.Add(btnRNew);
            grpRec.Controls.Add(btnRDelete);
            grpRec.Location = new Point(0, 0);
            grpRec.Name = "grpRec";
            grpRec.Size = new Size(200, 100);
            grpRec.TabIndex = 0;
            grpRec.TabStop = false;
            grpRec.Text = "Fixed expenses (created with one click at start of month)";
            // 
            // lblRCat
            // 
            lblRCat.AutoSize = true;
            lblRCat.Location = new Point(12, 40);
            lblRCat.Name = "lblRCat";
            lblRCat.Size = new Size(84, 25);
            lblRCat.TabIndex = 0;
            lblRCat.Text = "Category";
            // 
            // cmbRCat
            // 
            cmbRCat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRCat.Location = new Point(100, 36);
            cmbRCat.Name = "cmbRCat";
            cmbRCat.Size = new Size(220, 33);
            cmbRCat.TabIndex = 1;
            // 
            // lblRCh
            // 
            lblRCh.AutoSize = true;
            lblRCh.Location = new Point(340, 40);
            lblRCh.Name = "lblRCh";
            lblRCh.Size = new Size(75, 25);
            lblRCh.TabIndex = 2;
            lblRCh.Text = "Channel";
            // 
            // cmbRCh
            // 
            cmbRCh.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRCh.Location = new Point(420, 36);
            cmbRCh.Name = "cmbRCh";
            cmbRCh.Size = new Size(110, 33);
            cmbRCh.TabIndex = 2;
            // 
            // lblRWorker
            // 
            lblRWorker.AutoSize = true;
            lblRWorker.Location = new Point(550, 40);
            lblRWorker.Name = "lblRWorker";
            lblRWorker.Size = new Size(69, 25);
            lblRWorker.TabIndex = 3;
            lblRWorker.Text = "Worker";
            // 
            // cmbRWorker
            // 
            cmbRWorker.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRWorker.Location = new Point(630, 36);
            cmbRWorker.Name = "cmbRWorker";
            cmbRWorker.Size = new Size(200, 33);
            cmbRWorker.TabIndex = 3;
            // 
            // lblRAmt
            // 
            lblRAmt.AutoSize = true;
            lblRAmt.Location = new Point(12, 92);
            lblRAmt.Name = "lblRAmt";
            lblRAmt.Size = new Size(77, 25);
            lblRAmt.TabIndex = 4;
            lblRAmt.Text = "Amount";
            // 
            // numRAmt
            // 
            numRAmt.DecimalPlaces = 2;
            numRAmt.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numRAmt.Location = new Point(100, 88);
            numRAmt.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numRAmt.Name = "numRAmt";
            numRAmt.Size = new Size(140, 31);
            numRAmt.TabIndex = 4;
            numRAmt.TextAlign = HorizontalAlignment.Right;
            numRAmt.ThousandsSeparator = true;
            // 
            // chkRSalary
            // 
            chkRSalary.AutoSize = true;
            chkRSalary.Location = new Point(255, 90);
            chkRSalary.Name = "chkRSalary";
            chkRSalary.Size = new Size(370, 29);
            chkRSalary.TabIndex = 5;
            chkRSalary.Text = "Salary: worker ki mahana salary khud aaye";
            chkRSalary.UseVisualStyleBackColor = true;
            chkRSalary.CheckedChanged += chkRSalary_Changed;
            // 
            // lblRMethod
            // 
            lblRMethod.AutoSize = true;
            lblRMethod.Location = new Point(620, 92);
            lblRMethod.Name = "lblRMethod";
            lblRMethod.Size = new Size(80, 25);
            lblRMethod.TabIndex = 6;
            lblRMethod.Text = "Payment";
            // 
            // cmbRMethod
            // 
            cmbRMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRMethod.Location = new Point(710, 88);
            cmbRMethod.Name = "cmbRMethod";
            cmbRMethod.Size = new Size(120, 33);
            cmbRMethod.TabIndex = 6;
            // 
            // lblRNotes
            // 
            lblRNotes.AutoSize = true;
            lblRNotes.Location = new Point(12, 144);
            lblRNotes.Name = "lblRNotes";
            lblRNotes.Size = new Size(59, 25);
            lblRNotes.TabIndex = 7;
            lblRNotes.Text = "Notes";
            // 
            // txtRNotes
            // 
            txtRNotes.Location = new Point(100, 140);
            txtRNotes.MaxLength = 250;
            txtRNotes.Name = "txtRNotes";
            txtRNotes.Size = new Size(350, 31);
            txtRNotes.TabIndex = 7;
            // 
            // chkRActive
            // 
            chkRActive.AutoSize = true;
            chkRActive.Checked = true;
            chkRActive.CheckState = CheckState.Checked;
            chkRActive.Location = new Point(470, 142);
            chkRActive.Name = "chkRActive";
            chkRActive.Size = new Size(86, 29);
            chkRActive.TabIndex = 8;
            chkRActive.Text = "Active";
            chkRActive.UseVisualStyleBackColor = true;
            // 
            // btnRSave
            // 
            btnRSave.BackColor = Color.FromArgb(0, 120, 215);
            btnRSave.FlatAppearance.BorderSize = 0;
            btnRSave.FlatStyle = FlatStyle.Flat;
            btnRSave.ForeColor = Color.White;
            btnRSave.Location = new Point(570, 136);
            btnRSave.Name = "btnRSave";
            btnRSave.Size = new Size(100, 40);
            btnRSave.TabIndex = 9;
            btnRSave.Text = "Save";
            btnRSave.UseVisualStyleBackColor = false;
            btnRSave.Click += btnRSave_Click;
            // 
            // btnRNew
            // 
            btnRNew.Location = new Point(680, 136);
            btnRNew.Name = "btnRNew";
            btnRNew.Size = new Size(100, 40);
            btnRNew.TabIndex = 10;
            btnRNew.Text = "New";
            btnRNew.UseVisualStyleBackColor = true;
            btnRNew.Click += btnRNew_Click;
            // 
            // btnRDelete
            // 
            btnRDelete.BackColor = Color.FromArgb(200, 40, 40);
            btnRDelete.FlatAppearance.BorderSize = 0;
            btnRDelete.FlatStyle = FlatStyle.Flat;
            btnRDelete.ForeColor = Color.White;
            btnRDelete.Location = new Point(790, 136);
            btnRDelete.Name = "btnRDelete";
            btnRDelete.Size = new Size(100, 40);
            btnRDelete.TabIndex = 11;
            btnRDelete.Text = "Delete";
            btnRDelete.UseVisualStyleBackColor = false;
            btnRDelete.Click += btnRDelete_Click;
            // 
            // gridRec
            // 
            gridRec.AllowUserToAddRows = false;
            gridRec.AllowUserToDeleteRows = false;
            gridRec.AllowUserToResizeRows = false;
            gridRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridRec.BackgroundColor = Color.White;
            gridRec.BorderStyle = BorderStyle.None;
            gridRec.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridRec.Dock = DockStyle.Fill;
            gridRec.Location = new Point(0, 0);
            gridRec.MultiSelect = false;
            gridRec.Name = "gridRec";
            gridRec.ReadOnly = true;
            gridRec.RowHeadersVisible = false;
            gridRec.RowHeadersWidth = 62;
            gridRec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridRec.Size = new Size(240, 150);
            gridRec.TabIndex = 12;
            gridRec.SelectionChanged += gridRec_SelectionChanged;
            // 
            // ExpenseSetupForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 700);
            Controls.Add(tabs);
            Name = "ExpenseSetupForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Expense Setup";
            Load += ExpenseSetupForm_Load;
            tabs.ResumeLayout(false);
            tabCat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridCat).EndInit();
            grpCat.ResumeLayout(false);
            grpCat.PerformLayout();
            tabPay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridPay).EndInit();
            pnlPay.ResumeLayout(false);
            pnlPay.PerformLayout();
            grpRec.ResumeLayout(false);
            grpRec.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRAmt).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridRec).EndInit();
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