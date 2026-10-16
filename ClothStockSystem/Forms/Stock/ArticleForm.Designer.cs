namespace ClothStockSystem.Forms.Stock
{
    partial class ArticleForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            grpDetails = new GroupBox();
            tblInput = new TableLayoutPanel();
            lblNumber = new Label();
            txtNumber = new TextBox();
            lblPrice = new Label();
            txtPrice = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
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
            grpDetails.Controls.Add(tblInput);
            grpDetails.Controls.Add(flowButtons);
            grpDetails.Dock = DockStyle.Top;
            grpDetails.Font = new Font("Segoe UI", 10F);
            grpDetails.Location = new Point(0, 0);
            grpDetails.Name = "grpDetails";
            grpDetails.Padding = new Padding(12, 8, 12, 8);
            grpDetails.Size = new Size(900, 190);
            grpDetails.TabIndex = 0;
            grpDetails.TabStop = false;
            grpDetails.Text = "Article Details";
            // 
            // tblInput
            // 
            tblInput.ColumnCount = 4;
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tblInput.Controls.Add(lblNumber, 0, 0);
            tblInput.Controls.Add(txtNumber, 1, 0);
            tblInput.Controls.Add(lblPrice, 2, 0);
            tblInput.Controls.Add(txtPrice, 3, 0);
            tblInput.Controls.Add(lblName, 0, 1);
            tblInput.Controls.Add(txtName, 1, 1);
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
            // lblNumber
            // 
            lblNumber.Anchor = AnchorStyles.Left;
            lblNumber.AutoSize = true;
            lblNumber.Location = new Point(3, 0);
            lblNumber.Name = "lblNumber";
            lblNumber.Size = new Size(84, 46);
            lblNumber.TabIndex = 0;
            lblNumber.Text = "Article Number";
            // 
            // txtNumber
            // 
            txtNumber.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNumber.Location = new Point(143, 6);
            txtNumber.MaxLength = 30;
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(291, 34);
            txtNumber.TabIndex = 1;
            // 
            // lblPrice
            // 
            lblPrice.Anchor = AnchorStyles.Left;
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(440, 9);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(118, 28);
            lblPrice.TabIndex = 2;
            lblPrice.Text = "Selling Price";
            // 
            // txtPrice
            // 
            txtPrice.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPrice.Location = new Point(580, 6);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(293, 34);
            txtPrice.TabIndex = 3;
            txtPrice.TextAlign = HorizontalAlignment.Right;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Left;
            lblName.AutoSize = true;
            lblName.Location = new Point(3, 55);
            lblName.Name = "lblName";
            lblName.Size = new Size(125, 28);
            lblName.TabIndex = 4;
            lblName.Text = "Article Name";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtName.Location = new Point(143, 52);
            txtName.MaxLength = 100;
            txtName.Name = "txtName";
            txtName.Size = new Size(291, 34);
            txtName.TabIndex = 5;
            // 
            // chkActive
            // 
            chkActive.Anchor = AnchorStyles.Left;
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Location = new Point(580, 53);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(92, 32);
            chkActive.TabIndex = 6;
            chkActive.Text = "Active";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // flowButtons
            // 
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
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(750, 8);
            btnSave.Margin = new Padding(6, 8, 6, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 38);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(618, 8);
            btnNew.Margin = new Padding(6, 8, 6, 3);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(120, 38);
            btnNew.TabIndex = 8;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(200, 40, 40);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(486, 8);
            btnDelete.Margin = new Padding(6, 8, 6, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 38);
            btnDelete.TabIndex = 9;
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
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.Location = new Point(0, 190);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(900, 410);
            grid.TabIndex = 1;
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // ArticleForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(grid);
            Controls.Add(grpDetails);
            MinimumSize = new Size(780, 520);
            Name = "ArticleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Articles";
            Load += ArticleForm_Load;
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
        private Label lblNumber;
        private TextBox txtNumber;
        private Label lblPrice;
        private TextBox txtPrice;
        private Label lblName;
        private TextBox txtName;
        private CheckBox chkActive;
        private Button btnSave;
        private Button btnNew;
        private Button btnDelete;
        private DataGridView grid;
    }
}