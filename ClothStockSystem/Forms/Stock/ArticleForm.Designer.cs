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
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
            grpDetails.BackColor = Color.White;
            grpDetails.Controls.Add(tblInput);
            grpDetails.Controls.Add(flowButtons);
            grpDetails.Dock = DockStyle.Top;
            grpDetails.Font = new Font("Segoe UI Semibold", 10.5F);
            grpDetails.ForeColor = Color.FromArgb(13, 148, 136);
            grpDetails.Location = new Point(16, 16);
            grpDetails.Name = "grpDetails";
            grpDetails.Padding = new Padding(20, 10, 20, 10);
            grpDetails.Size = new Size(1130, 200);
            grpDetails.TabIndex = 0;
            grpDetails.TabStop = false;
            grpDetails.Text = "Article Details";
            // 
            // tblInput
            // 
            tblInput.BackColor = Color.White;
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
            tblInput.Location = new Point(20, 38);
            tblInput.Name = "tblInput";
            tblInput.RowCount = 2;
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tblInput.Size = new Size(1090, 92);
            tblInput.TabIndex = 0;
            // 
            // lblNumber
            // 
            lblNumber.Anchor = AnchorStyles.Left;
            lblNumber.AutoSize = true;
            lblNumber.Font = new Font("Segoe UI", 10F);
            lblNumber.ForeColor = Color.FromArgb(71, 85, 105);
            lblNumber.Location = new Point(3, 9);
            lblNumber.Name = "lblNumber";
            lblNumber.Size = new Size(145, 28);
            lblNumber.TabIndex = 0;
            lblNumber.Text = "Article Number";
            // 
            // txtNumber
            // 
            txtNumber.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNumber.BackColor = Color.FromArgb(248, 250, 252);
            txtNumber.BorderStyle = BorderStyle.FixedSingle;
            txtNumber.Font = new Font("Segoe UI", 10.5F);
            txtNumber.ForeColor = Color.FromArgb(15, 23, 42);
            txtNumber.Location = new Point(177, 5);
            txtNumber.MaxLength = 30;
            txtNumber.Name = "txtNumber";
            txtNumber.Size = new Size(364, 35);
            txtNumber.TabIndex = 1;
            // 
            // lblPrice
            // 
            lblPrice.Anchor = AnchorStyles.Left;
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 10F);
            lblPrice.ForeColor = Color.FromArgb(71, 85, 105);
            lblPrice.Location = new Point(547, 0);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(152, 46);
            lblPrice.TabIndex = 2;
            lblPrice.Text = "Selling Price(Estimated)";
            // 
            // txtPrice
            // 
            txtPrice.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtPrice.BackColor = Color.FromArgb(248, 250, 252);
            txtPrice.BorderStyle = BorderStyle.FixedSingle;
            txtPrice.Font = new Font("Segoe UI", 10.5F);
            txtPrice.ForeColor = Color.FromArgb(15, 23, 42);
            txtPrice.Location = new Point(721, 5);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(366, 35);
            txtPrice.TabIndex = 3;
            txtPrice.TextAlign = HorizontalAlignment.Right;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Left;
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 10F);
            lblName.ForeColor = Color.FromArgb(71, 85, 105);
            lblName.Location = new Point(3, 55);
            lblName.Name = "lblName";
            lblName.Size = new Size(125, 28);
            lblName.TabIndex = 4;
            lblName.Text = "Article Name";
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtName.BackColor = Color.FromArgb(248, 250, 252);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.Font = new Font("Segoe UI", 10.5F);
            txtName.ForeColor = Color.FromArgb(15, 23, 42);
            txtName.Location = new Point(177, 51);
            txtName.MaxLength = 100;
            txtName.Name = "txtName";
            txtName.Size = new Size(364, 35);
            txtName.TabIndex = 5;
            // 
            // chkActive
            // 
            chkActive.Anchor = AnchorStyles.Left;
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Cursor = Cursors.Hand;
            chkActive.Font = new Font("Segoe UI Semibold", 10F);
            chkActive.ForeColor = Color.FromArgb(13, 148, 136);
            chkActive.Location = new Point(721, 53);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(93, 32);
            chkActive.TabIndex = 6;
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
            flowButtons.Location = new Point(20, 130);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new Size(1090, 60);
            flowButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(13, 148, 136);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(15, 118, 110);
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 184, 166);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 10F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(964, 8);
            btnSave.Margin = new Padding(6, 8, 6, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(120, 40);
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnNew
            // 
            btnNew.BackColor = Color.White;
            btnNew.Cursor = Cursors.Hand;
            btnNew.FlatAppearance.BorderColor = Color.FromArgb(13, 148, 136);
            btnNew.FlatAppearance.MouseDownBackColor = Color.FromArgb(153, 246, 228);
            btnNew.FlatAppearance.MouseOverBackColor = Color.FromArgb(204, 251, 241);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Font = new Font("Segoe UI Semibold", 10F);
            btnNew.ForeColor = Color.FromArgb(15, 118, 110);
            btnNew.Location = new Point(832, 8);
            btnNew.Margin = new Padding(6, 8, 6, 3);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(120, 40);
            btnNew.TabIndex = 8;
            btnNew.Text = "New";
            btnNew.UseVisualStyleBackColor = false;
            btnNew.Click += btnNew_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(220, 38, 38);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseDownBackColor = Color.FromArgb(153, 27, 27);
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 68, 68);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI Semibold", 10F);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(700, 8);
            btnDelete.Margin = new Padding(6, 8, 6, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 40);
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
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(204, 251, 241);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(15, 118, 110);
            grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.FromArgb(241, 245, 249);
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(17, 24, 39);
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 10F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.Padding = new Padding(8, 6, 0, 6);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(17, 24, 39);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.Padding = new Padding(8, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(204, 251, 241);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(15, 118, 110);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            grid.DefaultCellStyle = dataGridViewCellStyle3;
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(226, 232, 240);
            grid.Location = new Point(16, 216);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.RowTemplate.Height = 38;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(1130, 368);
            grid.TabIndex = 1;
            grid.SelectionChanged += grid_SelectionChanged;
            // 
            // ArticleForm
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1162, 600);
            Controls.Add(grid);
            Controls.Add(grpDetails);
            MinimumSize = new Size(780, 520);
            Name = "ArticleForm";
            Padding = new Padding(16);
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