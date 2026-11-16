namespace ClothStockSystem.Forms.Expenses
{
    partial class WorkersForm
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
            grpDetails = new GroupBox();
            lblName = new Label(); txtName = new TextBox();
            lblRole = new Label(); txtRole = new TextBox();
            lblPhone = new Label(); txtPhone = new TextBox();
            lblSalary = new Label(); numSalary = new NumericUpDown();
            chkActive = new CheckBox();
            btnSave = new Button(); btnNew = new Button(); btnDelete = new Button();
            lblInfo = new Label();
            grid = new DataGridView();
            grpDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSalary).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            grpDetails.Controls.Add(lblName); grpDetails.Controls.Add(txtName);
            grpDetails.Controls.Add(lblRole); grpDetails.Controls.Add(txtRole);
            grpDetails.Controls.Add(lblPhone); grpDetails.Controls.Add(txtPhone);
            grpDetails.Controls.Add(lblSalary); grpDetails.Controls.Add(numSalary);
            grpDetails.Controls.Add(chkActive);
            grpDetails.Controls.Add(btnSave); grpDetails.Controls.Add(btnNew); grpDetails.Controls.Add(btnDelete);
            grpDetails.Controls.Add(lblInfo);
            grpDetails.Dock = DockStyle.Top; grpDetails.Name = "grpDetails"; grpDetails.Size = new Size(1000, 190);
            grpDetails.TabIndex = 0; grpDetails.TabStop = false; grpDetails.Text = "Worker";
            lblName.AutoSize = true; lblName.Location = new Point(12, 40); lblName.Name = "lblName"; lblName.Text = "Name";
            txtName.Location = new Point(90, 36); txtName.MaxLength = 100; txtName.Name = "txtName"; txtName.Size = new Size(220, 31); txtName.TabIndex = 1;
            lblRole.AutoSize = true; lblRole.Location = new Point(330, 40); lblRole.Name = "lblRole"; lblRole.Text = "Job";
            txtRole.Location = new Point(400, 36); txtRole.MaxLength = 60; txtRole.Name = "txtRole"; txtRole.Size = new Size(170, 31); txtRole.TabIndex = 2;
            lblPhone.AutoSize = true; lblPhone.Location = new Point(590, 40); lblPhone.Name = "lblPhone"; lblPhone.Text = "Phone";
            txtPhone.Location = new Point(660, 36); txtPhone.MaxLength = 15; txtPhone.Name = "txtPhone"; txtPhone.PlaceholderText = "03001234567"; txtPhone.Size = new Size(180, 31); txtPhone.TabIndex = 3;
            lblSalary.AutoSize = true; lblSalary.Location = new Point(12, 92); lblSalary.Name = "lblSalary"; lblSalary.Text = "Monthly Salary";
            numSalary.DecimalPlaces = 2; numSalary.Increment = new decimal(new int[] { 500, 0, 0, 0 }); numSalary.Location = new Point(160, 88);
            numSalary.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 }); numSalary.Name = "numSalary"; numSalary.Size = new Size(150, 31);
            numSalary.TabIndex = 4; numSalary.TextAlign = HorizontalAlignment.Right; numSalary.ThousandsSeparator = true;
            chkActive.AutoSize = true; chkActive.Checked = true; chkActive.CheckState = CheckState.Checked; chkActive.Location = new Point(335, 90); chkActive.Name = "chkActive"; chkActive.TabIndex = 5; chkActive.Text = "Active"; chkActive.UseVisualStyleBackColor = true;
            btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215); btnSave.FlatAppearance.BorderSize = 0; btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = System.Drawing.Color.White; btnSave.Location = new Point(450, 84); btnSave.Name = "btnSave"; btnSave.Size = new Size(120, 40);
            btnSave.TabIndex = 6; btnSave.Text = "Save"; btnSave.UseVisualStyleBackColor = false; btnSave.Click += btnSave_Click;
            btnNew.Location = new Point(580, 84); btnNew.Name = "btnNew"; btnNew.Size = new Size(120, 40); btnNew.TabIndex = 7;
            btnNew.Text = "New"; btnNew.UseVisualStyleBackColor = true; btnNew.Click += btnNew_Click;
            btnDelete.BackColor = System.Drawing.Color.FromArgb(200, 40, 40); btnDelete.FlatAppearance.BorderSize = 0; btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.ForeColor = System.Drawing.Color.White; btnDelete.Location = new Point(710, 84); btnDelete.Name = "btnDelete"; btnDelete.Size = new Size(120, 40);
            btnDelete.TabIndex = 8; btnDelete.Text = "Delete"; btnDelete.UseVisualStyleBackColor = false; btnDelete.Click += btnDelete_Click;
            lblInfo.AutoSize = true; lblInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold); lblInfo.Location = new Point(12, 144); lblInfo.Name = "lblInfo"; lblInfo.Text = "";
            // 
            grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; grid.BackgroundColor = System.Drawing.Color.White; grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize; grid.Dock = DockStyle.Fill; grid.MultiSelect = false;
            grid.Name = "grid"; grid.ReadOnly = true; grid.RowHeadersVisible = false; grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.TabIndex = 9;
            grid.SelectionChanged += grid_SelectionChanged;
            grid.DataBindingComplete += grid_DataBindingComplete;
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 650);
            Controls.Add(grid); Controls.Add(grpDetails);
            Name = "WorkersForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Workers";
            Load += WorkersForm_Load;
            grpDetails.ResumeLayout(false); grpDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSalary).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpDetails;
        private Label lblName; private TextBox txtName;
        private Label lblRole; private TextBox txtRole;
        private Label lblPhone; private TextBox txtPhone;
        private Label lblSalary; private NumericUpDown numSalary;
        private CheckBox chkActive;
        private Button btnSave; private Button btnNew; private Button btnDelete;
        private Label lblInfo;
        private DataGridView grid;
    }
}