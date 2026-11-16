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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tblRoot = new TableLayoutPanel();
            pnlHeader = new Panel();
            lblFormSubtitle = new Label();
            lblFormTitle = new Label();
            grpDetails = new GroupBox();
            tblDetails = new TableLayoutPanel();
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
            pnlPreview = new Panel();
            tblPreview = new TableLayoutPanel();
            pnlCardBunch = new Panel();
            lblPerBunchValue = new Label();
            lblPerBunchCaption = new Label();
            pnlCardMeters = new Panel();
            lblTotalMetersValue = new Label();
            lblTotalMetersCaption = new Label();
            pnlCardSuits = new Panel();
            lblTotalSuitsValue = new Label();
            lblTotalSuitsCaption = new Label();
            lblPreviewTitle = new Label();
            flowButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnClear = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            grpBatches = new GroupBox();
            grid = new DataGridView();
            tblRoot.SuspendLayout();
            pnlHeader.SuspendLayout();
            grpDetails.SuspendLayout();
            tblDetails.SuspendLayout();
            tblInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numBunches).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSuitsPerBunch).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMetersPerSuit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCost).BeginInit();
            pnlPreview.SuspendLayout();
            tblPreview.SuspendLayout();
            pnlCardBunch.SuspendLayout();
            pnlCardMeters.SuspendLayout();
            pnlCardSuits.SuspendLayout();
            flowButtons.SuspendLayout();
            grpBatches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            SuspendLayout();
            // 
            // tblRoot
            // 
            tblRoot.ColumnCount = 1;
            tblRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblRoot.Controls.Add(pnlHeader, 0, 0);
            tblRoot.Controls.Add(grpDetails, 0, 1);
            tblRoot.Controls.Add(grpBatches, 0, 2);
            tblRoot.Dock = DockStyle.Fill;
            tblRoot.Location = new Point(0, 0);
            tblRoot.Margin = new Padding(0);
            tblRoot.Name = "tblRoot";
            tblRoot.RowCount = 3;
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 420F));
            tblRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblRoot.Size = new Size(1100, 760);
            tblRoot.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(30, 58, 138);
            pnlHeader.Controls.Add(lblFormSubtitle);
            pnlHeader.Controls.Add(lblFormTitle);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Margin = new Padding(0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1100, 60);
            pnlHeader.TabIndex = 0;
            // 
            // lblFormSubtitle
            // 
            lblFormSubtitle.AutoSize = true;
            lblFormSubtitle.Font = new Font("Segoe UI", 9F);
            lblFormSubtitle.ForeColor = Color.FromArgb(191, 219, 254);
            lblFormSubtitle.Location = new Point(23, 35);
            lblFormSubtitle.Name = "lblFormSubtitle";
            lblFormSubtitle.Size = new Size(432, 25);
            lblFormSubtitle.TabIndex = 1;
            lblFormSubtitle.Text = "Naya stock add karein ya maujooda batch edit karein";
            // 
            // lblFormTitle
            // 
            lblFormTitle.AutoSize = true;
            lblFormTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblFormTitle.ForeColor = Color.White;
            lblFormTitle.Location = new Point(20, 4);
            lblFormTitle.Name = "lblFormTitle";
            lblFormTitle.Size = new Size(118, 38);
            lblFormTitle.TabIndex = 0;
            lblFormTitle.Text = "Stock In";
            // 
            // grpDetails
            // 
            grpDetails.BackColor = Color.White;
            grpDetails.Controls.Add(tblDetails);
            grpDetails.Dock = DockStyle.Fill;
            grpDetails.Font = new Font("Segoe UI", 10F);
            grpDetails.ForeColor = Color.FromArgb(30, 58, 138);
            grpDetails.Location = new Point(12, 70);
            grpDetails.Margin = new Padding(12, 10, 12, 6);
            grpDetails.Name = "grpDetails";
            grpDetails.Padding = new Padding(14, 6, 14, 8);
            grpDetails.Size = new Size(1076, 404);
            grpDetails.TabIndex = 1;
            grpDetails.TabStop = false;
            grpDetails.Text = "Stock In (New Items)";
            // 
            // tblDetails
            // 
            tblDetails.ColumnCount = 1;
            tblDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDetails.Controls.Add(tblInput, 0, 0);
            tblDetails.Controls.Add(pnlPreview, 0, 1);
            tblDetails.Controls.Add(flowButtons, 0, 2);
            tblDetails.Dock = DockStyle.Fill;
            tblDetails.ForeColor = Color.FromArgb(31, 41, 55);
            tblDetails.Location = new Point(14, 33);
            tblDetails.Margin = new Padding(0);
            tblDetails.Name = "tblDetails";
            tblDetails.RowCount = 3;
            tblDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
            tblDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tblDetails.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblDetails.Size = new Size(1048, 363);
            tblDetails.TabIndex = 0;
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
            tblInput.Dock = DockStyle.Fill;
            tblInput.Location = new Point(0, 0);
            tblInput.Margin = new Padding(0);
            tblInput.Name = "tblInput";
            tblInput.RowCount = 5;
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblInput.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tblInput.Size = new Size(1048, 200);
            tblInput.TabIndex = 0;
            // 
            // lblArticle
            // 
            lblArticle.AutoEllipsis = true;
            lblArticle.Dock = DockStyle.Fill;
            lblArticle.Location = new Point(0, 0);
            lblArticle.Margin = new Padding(0);
            lblArticle.Name = "lblArticle";
            lblArticle.Size = new Size(188, 40);
            lblArticle.TabIndex = 0;
            lblArticle.Text = "Article";
            lblArticle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbArticle
            // 
            cmbArticle.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.FlatStyle = FlatStyle.Flat;
            cmbArticle.Location = new Point(188, 3);
            cmbArticle.Margin = new Padding(0, 0, 16, 0);
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(319, 36);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            // 
            // lblColor
            // 
            lblColor.AutoEllipsis = true;
            lblColor.Dock = DockStyle.Fill;
            lblColor.Location = new Point(523, 0);
            lblColor.Margin = new Padding(0);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(188, 40);
            lblColor.TabIndex = 2;
            lblColor.Text = "Color";
            lblColor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbColor
            // 
            cmbColor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cmbColor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbColor.FlatStyle = FlatStyle.Flat;
            cmbColor.Location = new Point(711, 3);
            cmbColor.Margin = new Padding(0);
            cmbColor.Name = "cmbColor";
            cmbColor.Size = new Size(337, 36);
            cmbColor.TabIndex = 3;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            // 
            // lblDate
            // 
            lblDate.AutoEllipsis = true;
            lblDate.Dock = DockStyle.Fill;
            lblDate.Location = new Point(0, 40);
            lblDate.Margin = new Padding(0);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(188, 40);
            lblDate.TabIndex = 4;
            lblDate.Text = "Received Date";
            lblDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpReceived
            // 
            dtpReceived.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtpReceived.CustomFormat = "dd-MMM-yyyy";
            dtpReceived.Format = DateTimePickerFormat.Custom;
            dtpReceived.Location = new Point(188, 43);
            dtpReceived.Margin = new Padding(0, 0, 16, 0);
            dtpReceived.Name = "dtpReceived";
            dtpReceived.Size = new Size(319, 34);
            dtpReceived.TabIndex = 5;
            // 
            // lblSupplier
            // 
            lblSupplier.AutoEllipsis = true;
            lblSupplier.Dock = DockStyle.Fill;
            lblSupplier.Location = new Point(523, 40);
            lblSupplier.Margin = new Padding(0);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(188, 40);
            lblSupplier.TabIndex = 6;
            lblSupplier.Text = "Supplier (optional)";
            lblSupplier.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSupplier
            // 
            txtSupplier.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSupplier.BorderStyle = BorderStyle.FixedSingle;
            txtSupplier.Location = new Point(711, 43);
            txtSupplier.Margin = new Padding(0);
            txtSupplier.MaxLength = 100;
            txtSupplier.Name = "txtSupplier";
            txtSupplier.Size = new Size(337, 34);
            txtSupplier.TabIndex = 7;
            // 
            // lblBunches
            // 
            lblBunches.AutoEllipsis = true;
            lblBunches.Dock = DockStyle.Fill;
            lblBunches.Location = new Point(0, 80);
            lblBunches.Margin = new Padding(0);
            lblBunches.Name = "lblBunches";
            lblBunches.Size = new Size(188, 40);
            lblBunches.TabIndex = 8;
            lblBunches.Text = "Bunches";
            lblBunches.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numBunches
            // 
            numBunches.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numBunches.Location = new Point(188, 83);
            numBunches.Margin = new Padding(0, 0, 16, 0);
            numBunches.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numBunches.Name = "numBunches";
            numBunches.Size = new Size(319, 34);
            numBunches.TabIndex = 9;
            numBunches.TextAlign = HorizontalAlignment.Right;
            numBunches.ValueChanged += Preview_Changed;
            // 
            // lblSuitsPerBunch
            // 
            lblSuitsPerBunch.AutoEllipsis = true;
            lblSuitsPerBunch.Dock = DockStyle.Fill;
            lblSuitsPerBunch.Location = new Point(523, 80);
            lblSuitsPerBunch.Margin = new Padding(0);
            lblSuitsPerBunch.Name = "lblSuitsPerBunch";
            lblSuitsPerBunch.Size = new Size(188, 40);
            lblSuitsPerBunch.TabIndex = 10;
            lblSuitsPerBunch.Text = "Suits per Bunch";
            lblSuitsPerBunch.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numSuitsPerBunch
            // 
            numSuitsPerBunch.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numSuitsPerBunch.Location = new Point(711, 83);
            numSuitsPerBunch.Margin = new Padding(0);
            numSuitsPerBunch.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numSuitsPerBunch.Name = "numSuitsPerBunch";
            numSuitsPerBunch.Size = new Size(337, 34);
            numSuitsPerBunch.TabIndex = 11;
            numSuitsPerBunch.TextAlign = HorizontalAlignment.Right;
            numSuitsPerBunch.ValueChanged += Preview_Changed;
            // 
            // lblMetersPerSuit
            // 
            lblMetersPerSuit.AutoEllipsis = true;
            lblMetersPerSuit.Dock = DockStyle.Fill;
            lblMetersPerSuit.Location = new Point(0, 120);
            lblMetersPerSuit.Margin = new Padding(0);
            lblMetersPerSuit.Name = "lblMetersPerSuit";
            lblMetersPerSuit.Size = new Size(188, 40);
            lblMetersPerSuit.TabIndex = 12;
            lblMetersPerSuit.Text = "Meter per Suit";
            lblMetersPerSuit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numMetersPerSuit
            // 
            numMetersPerSuit.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numMetersPerSuit.DecimalPlaces = 3;
            numMetersPerSuit.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            numMetersPerSuit.Location = new Point(188, 123);
            numMetersPerSuit.Margin = new Padding(0, 0, 16, 0);
            numMetersPerSuit.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numMetersPerSuit.Name = "numMetersPerSuit";
            numMetersPerSuit.Size = new Size(319, 34);
            numMetersPerSuit.TabIndex = 13;
            numMetersPerSuit.TextAlign = HorizontalAlignment.Right;
            numMetersPerSuit.ValueChanged += Preview_Changed;
            // 
            // lblCost
            // 
            lblCost.AutoEllipsis = true;
            lblCost.Dock = DockStyle.Fill;
            lblCost.Location = new Point(523, 120);
            lblCost.Margin = new Padding(0);
            lblCost.Name = "lblCost";
            lblCost.Size = new Size(188, 40);
            lblCost.TabIndex = 14;
            lblCost.Text = "Cost Price per Suit";
            lblCost.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // numCost
            // 
            numCost.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            numCost.DecimalPlaces = 2;
            numCost.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numCost.Location = new Point(711, 123);
            numCost.Margin = new Padding(0);
            numCost.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numCost.Name = "numCost";
            numCost.Size = new Size(337, 34);
            numCost.TabIndex = 15;
            numCost.TextAlign = HorizontalAlignment.Right;
            numCost.ThousandsSeparator = true;
            // 
            // lblNotes
            // 
            lblNotes.AutoEllipsis = true;
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.Location = new Point(0, 160);
            lblNotes.Margin = new Padding(0);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(188, 40);
            lblNotes.TabIndex = 16;
            lblNotes.Text = "Notes (optional)";
            lblNotes.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNotes
            // 
            txtNotes.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNotes.BorderStyle = BorderStyle.FixedSingle;
            tblInput.SetColumnSpan(txtNotes, 3);
            txtNotes.Location = new Point(188, 163);
            txtNotes.Margin = new Padding(0);
            txtNotes.MaxLength = 250;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(860, 34);
            txtNotes.TabIndex = 17;
            // 
            // pnlPreview
            // 
            pnlPreview.BackColor = Color.FromArgb(239, 246, 255);
            pnlPreview.Controls.Add(tblPreview);
            pnlPreview.Controls.Add(lblPreviewTitle);
            pnlPreview.Dock = DockStyle.Fill;
            pnlPreview.Location = new Point(0, 204);
            pnlPreview.Margin = new Padding(0, 4, 0, 4);
            pnlPreview.Name = "pnlPreview";
            pnlPreview.Padding = new Padding(8, 4, 8, 8);
            pnlPreview.Size = new Size(1048, 92);
            pnlPreview.TabIndex = 1;
            // 
            // tblPreview
            // 
            tblPreview.ColumnCount = 3;
            tblPreview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblPreview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tblPreview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tblPreview.Controls.Add(pnlCardBunch, 0, 0);
            tblPreview.Controls.Add(pnlCardMeters, 1, 0);
            tblPreview.Controls.Add(pnlCardSuits, 2, 0);
            tblPreview.Dock = DockStyle.Fill;
            tblPreview.Location = new Point(8, 26);
            tblPreview.Margin = new Padding(0);
            tblPreview.Name = "tblPreview";
            tblPreview.RowCount = 1;
            tblPreview.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblPreview.Size = new Size(1032, 58);
            tblPreview.TabIndex = 1;
            // 
            // pnlCardBunch
            // 
            pnlCardBunch.BackColor = Color.White;
            pnlCardBunch.Controls.Add(lblPerBunchValue);
            pnlCardBunch.Controls.Add(lblPerBunchCaption);
            pnlCardBunch.Dock = DockStyle.Fill;
            pnlCardBunch.Location = new Point(0, 0);
            pnlCardBunch.Margin = new Padding(0, 0, 8, 0);
            pnlCardBunch.Name = "pnlCardBunch";
            pnlCardBunch.Padding = new Padding(14, 2, 14, 2);
            pnlCardBunch.Size = new Size(335, 58);
            pnlCardBunch.TabIndex = 0;
            // 
            // lblPerBunchValue
            // 
            lblPerBunchValue.Dock = DockStyle.Fill;
            lblPerBunchValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblPerBunchValue.ForeColor = Color.FromArgb(30, 64, 175);
            lblPerBunchValue.Location = new Point(14, 22);
            lblPerBunchValue.Name = "lblPerBunchValue";
            lblPerBunchValue.Size = new Size(307, 34);
            lblPerBunchValue.TabIndex = 2;
            lblPerBunchValue.Text = "0";
            lblPerBunchValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPerBunchCaption
            // 
            lblPerBunchCaption.Dock = DockStyle.Top;
            lblPerBunchCaption.Font = new Font("Segoe UI", 9F);
            lblPerBunchCaption.ForeColor = Color.FromArgb(107, 114, 128);
            lblPerBunchCaption.Location = new Point(14, 2);
            lblPerBunchCaption.Name = "lblPerBunchCaption";
            lblPerBunchCaption.Size = new Size(307, 20);
            lblPerBunchCaption.TabIndex = 1;
            lblPerBunchCaption.Text = "1 Bunch (meter)";
            lblPerBunchCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlCardMeters
            // 
            pnlCardMeters.BackColor = Color.White;
            pnlCardMeters.Controls.Add(lblTotalMetersValue);
            pnlCardMeters.Controls.Add(lblTotalMetersCaption);
            pnlCardMeters.Dock = DockStyle.Fill;
            pnlCardMeters.Location = new Point(343, 0);
            pnlCardMeters.Margin = new Padding(0, 0, 8, 0);
            pnlCardMeters.Name = "pnlCardMeters";
            pnlCardMeters.Padding = new Padding(14, 2, 14, 2);
            pnlCardMeters.Size = new Size(335, 58);
            pnlCardMeters.TabIndex = 1;
            // 
            // lblTotalMetersValue
            // 
            lblTotalMetersValue.Dock = DockStyle.Fill;
            lblTotalMetersValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalMetersValue.ForeColor = Color.FromArgb(30, 64, 175);
            lblTotalMetersValue.Location = new Point(14, 22);
            lblTotalMetersValue.Name = "lblTotalMetersValue";
            lblTotalMetersValue.Size = new Size(307, 34);
            lblTotalMetersValue.TabIndex = 4;
            lblTotalMetersValue.Text = "0";
            lblTotalMetersValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalMetersCaption
            // 
            lblTotalMetersCaption.Dock = DockStyle.Top;
            lblTotalMetersCaption.Font = new Font("Segoe UI", 9F);
            lblTotalMetersCaption.ForeColor = Color.FromArgb(107, 114, 128);
            lblTotalMetersCaption.Location = new Point(14, 2);
            lblTotalMetersCaption.Name = "lblTotalMetersCaption";
            lblTotalMetersCaption.Size = new Size(307, 20);
            lblTotalMetersCaption.TabIndex = 3;
            lblTotalMetersCaption.Text = "Total Meter";
            lblTotalMetersCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlCardSuits
            // 
            pnlCardSuits.BackColor = Color.White;
            pnlCardSuits.Controls.Add(lblTotalSuitsValue);
            pnlCardSuits.Controls.Add(lblTotalSuitsCaption);
            pnlCardSuits.Dock = DockStyle.Fill;
            pnlCardSuits.Location = new Point(686, 0);
            pnlCardSuits.Margin = new Padding(0);
            pnlCardSuits.Name = "pnlCardSuits";
            pnlCardSuits.Padding = new Padding(14, 2, 14, 2);
            pnlCardSuits.Size = new Size(346, 58);
            pnlCardSuits.TabIndex = 2;
            // 
            // lblTotalSuitsValue
            // 
            lblTotalSuitsValue.Dock = DockStyle.Fill;
            lblTotalSuitsValue.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTotalSuitsValue.ForeColor = Color.FromArgb(30, 64, 175);
            lblTotalSuitsValue.Location = new Point(14, 22);
            lblTotalSuitsValue.Name = "lblTotalSuitsValue";
            lblTotalSuitsValue.Size = new Size(318, 34);
            lblTotalSuitsValue.TabIndex = 6;
            lblTotalSuitsValue.Text = "0";
            lblTotalSuitsValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTotalSuitsCaption
            // 
            lblTotalSuitsCaption.Dock = DockStyle.Top;
            lblTotalSuitsCaption.Font = new Font("Segoe UI", 9F);
            lblTotalSuitsCaption.ForeColor = Color.FromArgb(107, 114, 128);
            lblTotalSuitsCaption.Location = new Point(14, 2);
            lblTotalSuitsCaption.Name = "lblTotalSuitsCaption";
            lblTotalSuitsCaption.Size = new Size(318, 20);
            lblTotalSuitsCaption.TabIndex = 5;
            lblTotalSuitsCaption.Text = "Total Suits";
            lblTotalSuitsCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblPreviewTitle
            // 
            lblPreviewTitle.Dock = DockStyle.Top;
            lblPreviewTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPreviewTitle.ForeColor = Color.FromArgb(30, 64, 175);
            lblPreviewTitle.Location = new Point(8, 4);
            lblPreviewTitle.Name = "lblPreviewTitle";
            lblPreviewTitle.Size = new Size(1032, 22);
            lblPreviewTitle.TabIndex = 0;
            lblPreviewTitle.Text = "Details (preview before save)";
            lblPreviewTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowButtons
            // 
            flowButtons.Controls.Add(btnSave);
            flowButtons.Controls.Add(btnClear);
            flowButtons.Controls.Add(btnEdit);
            flowButtons.Controls.Add(btnDelete);
            flowButtons.Dock = DockStyle.Fill;
            flowButtons.FlowDirection = FlowDirection.RightToLeft;
            flowButtons.Location = new Point(0, 300);
            flowButtons.Margin = new Padding(0);
            flowButtons.Name = "flowButtons";
            flowButtons.Padding = new Padding(0, 6, 0, 0);
            flowButtons.Size = new Size(1048, 63);
            flowButtons.TabIndex = 2;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(37, 99, 235);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(898, 6);
            btnSave.Margin = new Padding(8, 0, 0, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 38);
            btnSave.TabIndex = 18;
            btnSave.Text = "Save Stock";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClear
            // 
            btnClear.BackColor = Color.White;
            btnClear.Cursor = Cursors.Hand;
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(156, 163, 175);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 10F);
            btnClear.ForeColor = Color.FromArgb(55, 65, 81);
            btnClear.Location = new Point(780, 6);
            btnClear.Margin = new Padding(8, 0, 0, 0);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(110, 38);
            btnClear.TabIndex = 19;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.FromArgb(217, 119, 6);
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(592, 6);
            btnEdit.Margin = new Padding(8, 0, 0, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(180, 38);
            btnEdit.TabIndex = 21;
            btnEdit.Text = "Edit Selected Batch";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(200, 40, 40);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(394, 6);
            btnDelete.Margin = new Padding(8, 0, 0, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(190, 38);
            btnDelete.TabIndex = 20;
            btnDelete.Text = "Delete Selected Batch";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // grpBatches
            // 
            grpBatches.BackColor = Color.White;
            grpBatches.Controls.Add(grid);
            grpBatches.Dock = DockStyle.Fill;
            grpBatches.Font = new Font("Segoe UI", 10F);
            grpBatches.ForeColor = Color.FromArgb(30, 58, 138);
            grpBatches.Location = new Point(12, 480);
            grpBatches.Margin = new Padding(12, 0, 12, 12);
            grpBatches.Name = "grpBatches";
            grpBatches.Padding = new Padding(12, 4, 12, 12);
            grpBatches.Size = new Size(1076, 268);
            grpBatches.TabIndex = 2;
            grpBatches.TabStop = false;
            grpBatches.Text = "Is color ke maujooda batches";
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(17, 24, 39);
            grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.Padding = new Padding(6, 0, 0, 0);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(31, 41, 55);
            dataGridViewCellStyle3.Padding = new Padding(6, 0, 0, 0);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(17, 24, 39);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            grid.DefaultCellStyle = dataGridViewCellStyle3;
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(229, 231, 235);
            grid.Location = new Point(12, 31);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.RowTemplate.Height = 32;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(1052, 225);
            grid.TabIndex = 22;
            // 
            // StockInForm
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(243, 244, 246);
            ClientSize = new Size(1100, 760);
            Controls.Add(tblRoot);
            Font = new Font("Segoe UI", 10F);
            MinimumSize = new Size(1000, 700);
            Name = "StockInForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock In";
            Load += StockInForm_Load;
            tblRoot.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            grpDetails.ResumeLayout(false);
            tblDetails.ResumeLayout(false);
            tblInput.ResumeLayout(false);
            tblInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numBunches).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSuitsPerBunch).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMetersPerSuit).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCost).EndInit();
            pnlPreview.ResumeLayout(false);
            tblPreview.ResumeLayout(false);
            pnlCardBunch.ResumeLayout(false);
            pnlCardMeters.ResumeLayout(false);
            pnlCardSuits.ResumeLayout(false);
            flowButtons.ResumeLayout(false);
            grpBatches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tblRoot;
        private TableLayoutPanel tblDetails;
        private Panel pnlHeader;
        private Label lblFormTitle;
        private Label lblFormSubtitle;
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
        private TableLayoutPanel tblPreview;
        private Panel pnlCardBunch;
        private Panel pnlCardMeters;
        private Panel pnlCardSuits;
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
        private Button btnEdit;
        private Button btnDelete;
        private GroupBox grpBatches;
        private DataGridView grid;
    }
}