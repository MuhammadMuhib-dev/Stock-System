namespace ClothStockSystem.Forms.Stock
{
    partial class StockInForm
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
            grpDetails = new GroupBox();
            pnlPreview = new Panel();
            lblPreviewTitle = new Label();
            lblPerBunchCaption = new Label();
            lblPerBunchValue = new Label();
            lblTotalMetersCaption = new Label();
            lblTotalMetersValue = new Label();
            lblTotalSuitsCaption = new Label();
            lblTotalSuitsValue = new Label();
            flowButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnClear = new Button();
            btnDelete = new Button();
            tblInput = new TableLayoutPanel();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            lblColor = new Label();
            cmbColor = new ComboBox();
            lblDate = new Label();
            dtpReceived = new DateTimePicker();
            lblSupplier = new Label();
            txtSupplier = new TextBox();
            lblBunches = new Label();
            numBunches = new NumericUpDown();
            lblSuitsPerBunch = new Label();
            numSuitsPerBunch = new NumericUpDown();
            lblMetersPerSuit = new Label();
            numMetersPerSuit = new NumericUpDown();
            lblCost = new Label();
            numCost = new NumericUpDown();
            lblNotes = new Label();
            txtNotes = new TextBox();
            grpBatches = new GroupBox();
            grid = new DataGridView();
            grpDetails.SuspendLayout();
            pnlPreview.SuspendLayout();
            flowButtons.SuspendLayout();
            tblInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBunches).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSuitsPerBunch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMetersPerSuit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCost).BeginInit();
            grpBatches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            // grpDetails
            // 
            grpDetails.Controls.Add(pnlPreview);
            grpDetails.Controls.Add(flowButtons);
            grpDetails.Controls.Add(tblInput);
            grpDetails.Dock = DockStyle.Top;
            grpDetails.Font = new Font("Segoe UI", 10F);
            grpDetails.Location = new Point(0, 0);
            grpDetails.Name = "grpDetails";
            grpDetails.Padding = new Padding(12, 8, 12, 8);
            grpDetails.Size = new Size(1000, 420);
            grpDetails.TabIndex = 0;
            grpDetails.TabStop = false;
            grpDetails.Text = "Stock In (Naya Maal)";
            // 
            // pnlPreview
            // 
            pnlPreview.BackColor = Color.FromArgb(240, 246, 255);
            pnlPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlPreview.Controls.Add(lblPreviewTitle);
            pnlPreview.Controls.Add(lblPerBunchCaption);
            pnlPreview.Controls.Add(lblPerBunchValue);
            pnlPreview.Controls.Add(lblTotalMetersCaption);
            pnlPreview.Controls.Add(lblTotalMetersValue);
            pnlPreview.Controls.Add(lblTotalSuitsCaption);
            pnlPreview.Controls.Add(lblTotalSuitsValue);
            pnlPreview.Dock = DockStyle.Fill;
            pnlPreview.Location = new Point(12, 265);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Size = new Size(976, 92);
            pnlPreview.TabIndex = 1;
            // 
            // lblPreviewTitle
            // 
            lblPreviewTitle.AutoSize = true;
            lblPreviewTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPreviewTitle.Location = new Point(10, 6);
            lblPreviewTitle.Name = "lblPreviewTitle";
            lblPreviewTitle.Size = new Size(264, 25);
            lblPreviewTitle.TabIndex = 0;
            lblPreviewTitle.Text = "Hisab (save se pehle preview)";
            // 
            // lblPerBunchCaption
            // 
            lblPerBunchCaption.AutoSize = true;
            lblPerBunchCaption.Location = new Point(10, 34);
            lblPerBunchCaption.Name = "lblPerBunchCaption";
            lblPerBunchCaption.Size = new Size(153, 28);
            lblPerBunchCaption.TabIndex = 1;
            lblPerBunchCaption.Text = "1 Bunch (meter):";
            // 
            // lblPerBunchValue
            // 
            lblPerBunchValue.AutoSize = true;
            lblPerBunchValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPerBunchValue.Location = new Point(169, 32);
            lblPerBunchValue.Name = "lblPerBunchValue";
            lblPerBunchValue.Size = new Size(26, 30);
            lblPerBunchValue.TabIndex = 2;
            lblPerBunchValue.Text = "0";
            // 
            // lblTotalMetersCaption
            // 
            lblTotalMetersCaption.AutoSize = true;
            lblTotalMetersCaption.Location = new Point(300, 34);
            lblTotalMetersCaption.Name = "lblTotalMetersCaption";
            lblTotalMetersCaption.Size = new Size(115, 28);
            lblTotalMetersCaption.TabIndex = 3;
            lblTotalMetersCaption.Text = "Total Meter:";
            // 
            // lblTotalMetersValue
            // 
            lblTotalMetersValue.AutoSize = true;
            lblTotalMetersValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalMetersValue.Location = new Point(420, 32);
            lblTotalMetersValue.Name = "lblTotalMetersValue";
            lblTotalMetersValue.Size = new Size(26, 30);
            lblTotalMetersValue.TabIndex = 4;
            lblTotalMetersValue.Text = "0";
            // 
            // lblTotalSuitsCaption
            // 
            lblTotalSuitsCaption.AutoSize = true;
            lblTotalSuitsCaption.Location = new Point(580, 34);
            lblTotalSuitsCaption.Name = "lblTotalSuitsCaption";
            lblTotalSuitsCaption.Size = new Size(105, 28);
            lblTotalSuitsCaption.TabIndex = 5;
            lblTotalSuitsCaption.Text = "Total Suits:";
            // 
            // lblTotalSuitsValue
            // 
            lblTotalSuitsValue.AutoSize = true;
            lblTotalSuitsValue.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotalSuitsValue.Location = new Point(690, 32);
            lblTotalSuitsValue.Name = "lblTotalSuitsValue";
            lblTotalSuitsValue.Size = new Size(26, 30);
            lblTotalSuitsValue.TabIndex = 6;
            lblTotalSuitsValue.Text = "0";
            // 
            // flowButtons
            // 
            flowButtons.Controls.Add(btnSave);
            flowButtons.Controls.Add(btnClear);
            flowButtons.Controls.Add(btnDelete);
            flowButtons.Dock = DockStyle.Bottom;
            flowButtons.FlowDirection = FlowDirection.RightToLeft;
            flowButtons.Location = new Point(12, 357);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new Size(976, 55);
            flowButtons.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(830, 8);
            btnSave.Margin = new Padding(6, 8, 6, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(140, 38);
            btnSave.TabIndex = 18;
            btnSave.Text = "Save Stock";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(698, 8);
            btnClear.Margin = new Padding(6, 8, 6, 3);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(120, 38);
            btnClear.TabIndex = 19;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(200, 40, 40);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(456, 8);
            btnDelete.Margin = new Padding(6, 8, 6, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(230, 38);
            btnDelete.TabIndex = 20;
            btnDelete.Text = "Delete Selected Batch";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // tblInput
            // 
            tblInput.ColumnCount = 4;
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18F));
            tblInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            tblInput.Controls.Add(lblArticle, 0, 0);
            tblInput.Controls.Add(cmbArticle, 1, 0);
            tblInput.Controls.Add(lblColor, 2, 0);
            tblInput.Controls.Add(cmbColor, 3, 0);
            tblInput.Controls.Add(lblDate, 0, 1);
            tblInput.Controls.Add(dtpReceived, 1, 1);
            tblInput.Controls.Add(lblSupplier, 2, 1);
            tblInput.Controls.Add(txtSupplier, 3, 1);
            tblInput.Controls.Add(lblBunches, 0, 2);
            tblInput.Controls.Add(numBunches, 1, 2);
            tblInput.Controls.Add(lblSuitsPerBunch, 2, 2);
            tblInput.Controls.Add(numSuitsPerBunch, 3, 2);
            tblInput.Controls.Add(lblMetersPerSuit, 0, 3);
            tblInput.Controls.Add(numMetersPerSuit, 1, 3);
            tblInput.Controls.Add(lblCost, 2, 3);
            tblInput.Controls.Add(numCost, 3, 3);
            tblInput.Controls.Add(lblNotes, 0, 4);
            tblInput.Controls.Add(txtNotes, 1, 4);
            tblInput.Dock = DockStyle.Top;
            tblInput.Location = new Point(12, 35);
            tblInput.Name = "tblInput";
            tblInput.RowCount = 5;
            tblInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tblInput.Size = new Size(976, 230);
            tblInput.TabIndex = 0;
            // 
            // lblArticle
            // 
            lblArticle.Anchor = AnchorStyles.Left;
            lblArticle.AutoSize = true;
            lblArticle.Location = new Point(3, 9);
            lblArticle.Name = "lblArticle";
            lblArticle.Size = new Size(68, 28);
            lblArticle.TabIndex = 0;
            lblArticle.Text = "Article";
            // 
            // cmbArticle
            // 
            cmbArticle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.Location = new Point(178, 6);
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(306, 36);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            // 
            // lblColor
            // 
            lblColor.Anchor = AnchorStyles.Left;
            lblColor.AutoSize = true;
            lblColor.Location = new Point(490, 9);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(60, 28);
            lblColor.TabIndex = 2;
            lblColor.Text = "Color";
            // 
            // cmbColor
            // 
            cmbColor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbColor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbColor.Location = new Point(665, 6);
            cmbColor.Name = "cmbColor";
            cmbColor.Size = new Size(308, 36);
            cmbColor.TabIndex = 3;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            // 
            // lblDate
            // 
            lblDate.Anchor = AnchorStyles.Left;
            lblDate.AutoSize = true;
            lblDate.Location = new Point(3, 55);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(135, 28);
            lblDate.TabIndex = 4;
            lblDate.Text = "Received Date";
            // 
            // dtpReceived
            // 
            dtpReceived.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtpReceived.CustomFormat = "dd-MMM-yyyy";
            dtpReceived.Format = DateTimePickerFormat.Custom;
            dtpReceived.Location = new Point(178, 52);
            dtpReceived.Name = "dtpReceived";
            dtpReceived.Size = new Size(306, 34);
            dtpReceived.TabIndex = 5;
            // 
            // lblSupplier
            // 
            lblSupplier.Anchor = AnchorStyles.Left;
            lblSupplier.AutoSize = true;
            lblSupplier.Location = new Point(490, 46);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(98, 46);
            lblSupplier.TabIndex = 6;
            lblSupplier.Text = "Supplier (optional)";
            // 
            // txtSupplier
            // 
            txtSupplier.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSupplier.Location = new Point(665, 52);
            txtSupplier.MaxLength = 100;
            txtSupplier.Name = "txtSupplier";
            txtSupplier.Size = new Size(308, 34);
            txtSupplier.TabIndex = 7;
            // 
            // lblBunches
            // 
            lblBunches.Anchor = AnchorStyles.Left;
            lblBunches.AutoSize = true;
            lblBunches.Location = new Point(3, 101);
            lblBunches.Name = "lblBunches";
            lblBunches.Size = new Size(83, 28);
            lblBunches.TabIndex = 8;
            lblBunches.Text = "Bunches";
            // 
            // numBunches
            // 
            numBunches.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numBunches.Location = new Point(178, 98);
            numBunches.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numBunches.Name = "numBunches";
            numBunches.Size = new Size(306, 34);
            numBunches.TabIndex = 9;
            numBunches.TextAlign = HorizontalAlignment.Right;
            numBunches.ValueChanged += Preview_Changed;
            // 
            // lblSuitsPerBunch
            // 
            lblSuitsPerBunch.Anchor = AnchorStyles.Left;
            lblSuitsPerBunch.AutoSize = true;
            lblSuitsPerBunch.Location = new Point(490, 101);
            lblSuitsPerBunch.Name = "lblSuitsPerBunch";
            lblSuitsPerBunch.Size = new Size(146, 28);
            lblSuitsPerBunch.TabIndex = 10;
            lblSuitsPerBunch.Text = "Suits per Bunch";
            // 
            // numSuitsPerBunch
            // 
            numSuitsPerBunch.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numSuitsPerBunch.Location = new Point(665, 98);
            numSuitsPerBunch.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numSuitsPerBunch.Name = "numSuitsPerBunch";
            numSuitsPerBunch.Size = new Size(308, 34);
            numSuitsPerBunch.TabIndex = 11;
            numSuitsPerBunch.TextAlign = HorizontalAlignment.Right;
            numSuitsPerBunch.ValueChanged += Preview_Changed;
            // 
            // lblMetersPerSuit
            // 
            lblMetersPerSuit.Anchor = AnchorStyles.Left;
            lblMetersPerSuit.AutoSize = true;
            lblMetersPerSuit.Location = new Point(3, 147);
            lblMetersPerSuit.Name = "lblMetersPerSuit";
            lblMetersPerSuit.Size = new Size(137, 28);
            lblMetersPerSuit.TabIndex = 12;
            lblMetersPerSuit.Text = "Meter per Suit";
            // 
            // numMetersPerSuit
            // 
            numMetersPerSuit.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numMetersPerSuit.DecimalPlaces = 3;
            numMetersPerSuit.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numMetersPerSuit.Location = new Point(178, 144);
            numMetersPerSuit.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMetersPerSuit.Name = "numMetersPerSuit";
            numMetersPerSuit.Size = new Size(306, 34);
            numMetersPerSuit.TabIndex = 13;
            numMetersPerSuit.TextAlign = HorizontalAlignment.Right;
            numMetersPerSuit.ValueChanged += Preview_Changed;
            // 
            // lblCost
            // 
            lblCost.Anchor = AnchorStyles.Left;
            lblCost.AutoSize = true;
            lblCost.Location = new Point(490, 138);
            lblCost.Name = "lblCost";
            lblCost.Size = new Size(137, 46);
            lblCost.TabIndex = 14;
            lblCost.Text = "Cost Price per Suit";
            // 
            // numCost
            // 
            numCost.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numCost.DecimalPlaces = 2;
            numCost.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numCost.Location = new Point(665, 144);
            numCost.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numCost.Name = "numCost";
            numCost.Size = new Size(308, 34);
            numCost.TabIndex = 15;
            numCost.TextAlign = HorizontalAlignment.Right;
            numCost.ThousandsSeparator = true;
            // 
            // lblNotes
            // 
            lblNotes.Anchor = AnchorStyles.Left;
            lblNotes.AutoSize = true;
            lblNotes.Location = new Point(3, 193);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(155, 28);
            lblNotes.TabIndex = 16;
            lblNotes.Text = "Notes (optional)";
            // 
            // txtNotes
            // 
            txtNotes.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tblInput.SetColumnSpan(txtNotes, 3);
            txtNotes.Location = new Point(178, 190);
            txtNotes.MaxLength = 250;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(795, 34);
            txtNotes.TabIndex = 17;
            // 
            // grpBatches
            // 
            grpBatches.Controls.Add(grid);
            grpBatches.Dock = DockStyle.Fill;
            grpBatches.Location = new Point(0, 420);
            grpBatches.Name = "grpBatches";
            grpBatches.Padding = new Padding(8);
            grpBatches.Size = new Size(1000, 360);
            grpBatches.TabIndex = 1;
            grpBatches.TabStop = false;
            grpBatches.Text = "Is color ke maujooda batches";
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.Dock = DockStyle.Fill;
            grid.Location = new Point(8, 32);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(984, 320);
            grid.TabIndex = 21;
            // 
            // StockInForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 780);
            Controls.Add(grpBatches);
            Controls.Add(grpDetails);
            MinimumSize = new Size(900, 700);
            Name = "StockInForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock In";
            Load += StockInForm_Load;
            grpDetails.ResumeLayout(false);
            pnlPreview.ResumeLayout(false);
            pnlPreview.PerformLayout();
            flowButtons.ResumeLayout(false);
            tblInput.ResumeLayout(false);
            tblInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBunches).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSuitsPerBunch).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMetersPerSuit).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCost).EndInit();
            grpBatches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grpDetails;
        private TableLayoutPanel tblInput;
        private Label lblArticle;
        private ComboBox cmbArticle;
        private Label lblColor;
        private ComboBox cmbColor;
        private Label lblDate;
        private DateTimePicker dtpReceived;
        private Label lblSupplier;
        private TextBox txtSupplier;
        private Label lblBunches;
        private NumericUpDown numBunches;
        private Label lblSuitsPerBunch;
        private NumericUpDown numSuitsPerBunch;
        private Label lblMetersPerSuit;
        private NumericUpDown numMetersPerSuit;
        private Label lblCost;
        private NumericUpDown numCost;
        private Label lblNotes;
        private TextBox txtNotes;
        private Panel pnlPreview;
        private Label lblPreviewTitle;
        private Label lblPerBunchCaption;
        private Label lblPerBunchValue;
        private Label lblTotalMetersCaption;
        private Label lblTotalMetersValue;
        private Label lblTotalSuitsCaption;
        private Label lblTotalSuitsValue;
        private FlowLayoutPanel flowButtons;
        private Button btnSave;
        private Button btnClear;
        private Button btnDelete;
        private GroupBox grpBatches;
        private DataGridView grid;
    }
}