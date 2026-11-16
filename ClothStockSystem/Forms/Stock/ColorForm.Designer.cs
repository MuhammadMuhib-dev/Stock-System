namespace ClothStockSystem.Forms.Stock
{
    partial class ColorForm
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
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle altRowStyle = new DataGridViewCellStyle();

            grpDetails = new GroupBox();
            tblInput = new TableLayoutPanel();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            lblColor = new Label();
            txtColor = new TextBox();
            chkActive = new CheckBox();
            flowButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnNew = new Button();
            btnDelete = new Button();
            grid = new DataGridView();
            grpDetails.SuspendLayout();
            tblInput.SuspendLayout();
            flowButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            // grpDetails
            // 
            grpDetails.BackColor = Color.White;
            grpDetails.Controls.Add(tblInput);
            grpDetails.Controls.Add(flowButtons);
            grpDetails.Dock = DockStyle.Top;
            grpDetails.Font = new System.Drawing.Font("Segoe UI", 10F);
            grpDetails.ForeColor = Color.FromArgb(30, 41, 59);
            grpDetails.Location = new Point(0, 0);
            grpDetails.Name = "grpDetails";
            grpDetails.Padding = new Padding(12, 8, 12, 8);
            grpDetails.Size = new Size(900, 190);
            grpDetails.TabIndex = 0;
            grpDetails.TabStop = false;
            grpDetails.Text = "Color Details";
            // 
            // tblInput
            // 
            tblInput.BackColor = Color.White;
            tblInput.ColumnCount = 4;
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tblInput.Controls.Add(lblArticle, 0, 0);
            tblInput.Controls.Add(cmbArticle, 1, 0);
            tblInput.Controls.Add(lblColor, 0, 1);
            tblInput.Controls.Add(txtColor, 1, 1);
            tblInput.Controls.Add(chkActive, 3, 1);
            tblInput.Dock = DockStyle.Fill;
            tblInput.Location = new Point(12, 35);
            tblInput.Name = "tblInput";
            tblInput.RowCount = 2;
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblInput.Size = new Size(876, 92);
            tblInput.TabIndex = 0;
            // 
            // lblArticle
            // 
            lblArticle.Anchor = AnchorStyles.Left;
            lblArticle.AutoSize = true;
            lblArticle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            lblArticle.ForeColor = Color.FromArgb(71, 85, 105);
            lblArticle.Location = new Point(3, 9);
            lblArticle.Name = "lblArticle";
            lblArticle.Size = new Size(68, 28);
            lblArticle.TabIndex = 0;
            lblArticle.Text = "Article";
            // 
            // cmbArticle
            // 
            cmbArticle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbArticle.BackColor = Color.White;
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.FlatStyle = FlatStyle.Flat;
            cmbArticle.ForeColor = Color.FromArgb(30, 41, 59);
            cmbArticle.Location = new Point(143, 6);
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(291, 36);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            // 
            // lblColor
            // 
            lblColor.Anchor = AnchorStyles.Left;
            lblColor.AutoSize = true;
            lblColor.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            lblColor.ForeColor = Color.FromArgb(71, 85, 105);
            lblColor.Location = new Point(3, 55);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(117, 28);
            lblColor.TabIndex = 2;
            lblColor.Text = "Color Name";
            // 
            // txtColor
            // 
            txtColor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtColor.BackColor = Color.FromArgb(248, 250, 252);
            txtColor.BorderStyle = BorderStyle.FixedSingle;
            txtColor.ForeColor = Color.FromArgb(30, 41, 59);
            txtColor.Location = new Point(143, 52);
            txtColor.MaxLength = 50;
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(291, 34);
            txtColor.TabIndex = 2;
            // 
            // chkActive
            // 
            chkActive.Anchor = AnchorStyles.Left;
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Cursor = Cursors.Hand;
            chkActive.ForeColor = Color.FromArgb(30, 41, 59);
            chkActive.Location = new Point(580, 53);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(92, 32);
            chkActive.TabIndex = 3;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // flowButtons
            // 
            flowButtons.BackColor = Color.White;
            flowButtons.Controls.Add(btnSave);
            flowButtons.Controls.Add(btnNew);
            flowButtons.Controls.Add(btnDelete);
            flowButtons.Dock = DockStyle.Bottom;
            flowButtons.FlowDirection = FlowDirection.RightToLeft;
            flowButtons.Location = new Point(12, 127);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new Size(876, 55);
            flowButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(37, 99, 235);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 130, 246);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(750, 8);
            btnSave.Margin = new Padding(6, 8, 6, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 38);
            btnSave.TabIndex = 4;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.White;
            btnNew.Cursor = Cursors.Hand;
            btnNew.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
            btnNew.FlatAppearance.BorderSize = 1;
            btnNew.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 232, 240);
            btnNew.FlatAppearance.MouseOverBackColor = Color.FromArgb(241, 245, 249);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            btnNew.ForeColor = Color.FromArgb(51, 65, 85);
            btnNew.Location = new Point(618, 8);
            btnNew.Margin = new Padding(6, 8, 6, 3);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(120, 38);
            btnNew.TabIndex = 5;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = false;
            btnNew.Click += btnNew_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 38, 38);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseDownBackColor = Color.FromArgb(185, 28, 28);
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 68, 68);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(486, 8);
            btnDelete.Margin = new Padding(6, 8, 6, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 38);
            btnDelete.TabIndex = 6;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.FromArgb(243, 246, 250);
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = Color.FromArgb(30, 41, 59);
            headerStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            headerStyle.ForeColor = Color.White;
            headerStyle.Padding = new Padding(8, 6, 8, 6);
            headerStyle.SelectionBackColor = Color.FromArgb(30, 41, 59);
            headerStyle.SelectionForeColor = Color.White;
            headerStyle.WrapMode = DataGridViewTriState.True;
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            rowStyle.BackColor = Color.White;
            rowStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            rowStyle.ForeColor = Color.FromArgb(30, 41, 59);
            rowStyle.Padding = new Padding(8, 4, 8, 4);
            rowStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            rowStyle.SelectionForeColor = Color.FromArgb(30, 58, 138);
            grid.DefaultCellStyle = rowStyle;
            altRowStyle.BackColor = Color.FromArgb(248, 250, 252);
            altRowStyle.ForeColor = Color.FromArgb(30, 41, 59);
            altRowStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            altRowStyle.SelectionForeColor = Color.FromArgb(30, 58, 138);
            grid.AlternatingRowsDefaultCellStyle = altRowStyle;
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(226, 232, 240);
            grid.Location = new Point(0, 190);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(900, 410);
            grid.TabIndex = 7;
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // ColorForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 246, 250);
            ClientSize = new Size(900, 600);
            Controls.Add(grid);
            Controls.Add(grpDetails);
            MinimumSize = new Size(780, 520);
            Name = "ColorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Colors";
            Load += ColorForm_Load;
            grpDetails.ResumeLayout(false);
            tblInput.ResumeLayout(false);
            tblInput.PerformLayout();
            flowButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpDetails;
        private TableLayoutPanel tblInput;
        private FlowLayoutPanel flowButtons;
        private Label lblArticle;
        private ComboBox cmbArticle;
        private Label lblColor;
        private TextBox txtColor;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnNew;
        private Button btnDelete;
        private DataGridView grid;
    }
}