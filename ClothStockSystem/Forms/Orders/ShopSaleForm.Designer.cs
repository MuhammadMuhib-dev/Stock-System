namespace ClothStockSystem.Forms.Orders
{
    partial class ShopSaleForm
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
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle altStyle = new DataGridViewCellStyle();
            tabs = new TabControl();
            tabSale = new TabPage();
            grid = new DataGridView();
            pnlBottom = new Panel();
            lblName = new Label();
            txtName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblGrand = new Label();
            lblTotals = new Label();
            lblProfit = new Label();
            lblMode = new Label();
            btnRemove = new Button();
            btnSave = new Button();
            btnClear = new Button();
            pnlItem = new GroupBox();
            lblArticle = new Label();
            cmbArticle = new ComboBox();
            lblColor = new Label();
            cmbColor = new ComboBox();
            lblBatch = new Label();
            cmbBatch = new ComboBox();
            lblUnit = new Label();
            cmbUnit = new ComboBox();
            lblQty = new Label();
            numQty = new NumericUpDown();
            lblBasis = new Label();
            cmbBasis = new ComboBox();
            lblPrice = new Label();
            numPrice = new NumericUpDown();
            lblCost = new Label();
            lblStock = new Label();
            btnAddItem = new Button();
            tabList = new TabPage();
            gridSales = new DataGridView();
            gridSaleLines = new DataGridView();
            pnlSummary = new Panel();
            lblSummary = new Label();
            pnlSearch = new Panel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblRange = new Label();
            cmbRange = new ComboBox();
            btnSearch = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            tabs.SuspendLayout();
            tabSale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            pnlBottom.SuspendLayout();
            pnlItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            tabList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridSaleLines).BeginInit();
            pnlSummary.SuspendLayout();
            pnlSearch.SuspendLayout();
            SuspendLayout();
            // 
            // shared grid styles (sky blue)
            // 
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            headerStyle.BackColor = Color.FromArgb(79, 163, 224);
            headerStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            headerStyle.ForeColor = Color.White;
            headerStyle.SelectionBackColor = Color.FromArgb(79, 163, 224);
            headerStyle.SelectionForeColor = Color.White;
            headerStyle.WrapMode = DataGridViewTriState.False;
            cellStyle.BackColor = Color.White;
            cellStyle.ForeColor = Color.FromArgb(30, 42, 54);
            cellStyle.Padding = new Padding(4, 0, 4, 0);
            cellStyle.SelectionBackColor = Color.FromArgb(207, 232, 250);
            cellStyle.SelectionForeColor = Color.FromArgb(30, 42, 54);
            altStyle.BackColor = Color.FromArgb(247, 251, 255);
            // 
            // tabs
            // 
            tabs.Controls.Add(tabSale);
            tabs.Controls.Add(tabList);
            tabs.Dock = DockStyle.Fill;
            tabs.Location = new Point(0, 0);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(1100, 760);
            tabs.TabIndex = 0;
            tabs.SelectedIndexChanged += tabs_SelectedIndexChanged;
            // 
            // tabSale
            // 
            tabSale.BackColor = Color.White;
            tabSale.Controls.Add(grid);
            tabSale.Controls.Add(pnlBottom);
            tabSale.Controls.Add(pnlItem);
            tabSale.Location = new Point(4, 34);
            tabSale.Name = "tabSale";
            tabSale.Size = new Size(1092, 722);
            tabSale.TabIndex = 0;
            tabSale.Text = "Nayi Sale / Edit";
            tabSale.UseVisualStyleBackColor = false;
            // 
            // grid
            // 
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AlternatingRowsDefaultCellStyle = (DataGridViewCellStyle)altStyle.Clone();
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle = (DataGridViewCellStyle)headerStyle.Clone();
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.DefaultCellStyle = (DataGridViewCellStyle)cellStyle.Clone();
            grid.Dock = DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = Color.FromArgb(227, 238, 247);
            grid.Location = new Point(0, 220);
            grid.MultiSelect = false;
            grid.Name = "grid";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 62;
            grid.RowTemplate.Height = 30;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Size = new Size(1092, 302);
            grid.TabIndex = 9;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.FromArgb(240, 248, 255);
            pnlBottom.Controls.Add(lblName);
            pnlBottom.Controls.Add(txtName);
            pnlBottom.Controls.Add(lblPhone);
            pnlBottom.Controls.Add(txtPhone);
            pnlBottom.Controls.Add(lblGrand);
            pnlBottom.Controls.Add(lblTotals);
            pnlBottom.Controls.Add(lblProfit);
            pnlBottom.Controls.Add(lblMode);
            pnlBottom.Controls.Add(btnRemove);
            pnlBottom.Controls.Add(btnSave);
            pnlBottom.Controls.Add(btnClear);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 522);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1092, 200);
            pnlBottom.TabIndex = 10;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.ForeColor = Color.FromArgb(30, 42, 54);
            lblName.Location = new Point(12, 16);
            lblName.Name = "lblName";
            lblName.Size = new Size(89, 25);
            lblName.TabIndex = 0;
            lblName.Text = "Customer";
            // 
            // txtName
            // 
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.FromArgb(30, 42, 54);
            txtName.Location = new Point(110, 12);
            txtName.MaxLength = 100;
            txtName.Name = "txtName";
            txtName.Size = new Size(230, 31);
            txtName.TabIndex = 11;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.ForeColor = Color.FromArgb(30, 42, 54);
            lblPhone.Location = new Point(370, 16);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(62, 25);
            lblPhone.TabIndex = 12;
            lblPhone.Text = "Phone";
            // 
            // txtPhone
            // 
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.ForeColor = Color.FromArgb(30, 42, 54);
            txtPhone.Location = new Point(440, 12);
            txtPhone.MaxLength = 15;
            txtPhone.Name = "txtPhone";
            txtPhone.PlaceholderText = "03001234567";
            txtPhone.Size = new Size(200, 31);
            txtPhone.TabIndex = 12;
            // 
            // lblGrand
            // 
            lblGrand.AutoSize = true;
            lblGrand.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblGrand.ForeColor = Color.FromArgb(59, 143, 204);
            lblGrand.Location = new Point(12, 58);
            lblGrand.Name = "lblGrand";
            lblGrand.Size = new Size(0, 38);
            lblGrand.TabIndex = 13;
            // 
            // lblTotals
            // 
            lblTotals.AutoSize = true;
            lblTotals.ForeColor = Color.FromArgb(107, 124, 140);
            lblTotals.Location = new Point(12, 102);
            lblTotals.Name = "lblTotals";
            lblTotals.Size = new Size(0, 25);
            lblTotals.TabIndex = 14;
            // 
            // lblProfit
            // 
            lblProfit.AutoSize = true;
            lblProfit.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblProfit.ForeColor = Color.FromArgb(30, 42, 54);
            lblProfit.Location = new Point(12, 130);
            lblProfit.Name = "lblProfit";
            lblProfit.Size = new Size(0, 30);
            lblProfit.TabIndex = 15;
            // 
            // lblMode
            // 
            lblMode.AutoSize = true;
            lblMode.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMode.ForeColor = Color.FromArgb(59, 143, 204);
            lblMode.Location = new Point(12, 162);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(0, 28);
            lblMode.TabIndex = 16;
            // 
            // btnRemove (secondary: flat outline)
            // 
            btnRemove.BackColor = Color.White;
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.FlatAppearance.BorderColor = Color.FromArgb(79, 163, 224);
            btnRemove.FlatAppearance.BorderSize = 1;
            btnRemove.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 248, 255);
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.ForeColor = Color.FromArgb(59, 143, 204);
            btnRemove.Location = new Point(670, 8);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(200, 38);
            btnRemove.TabIndex = 13;
            btnRemove.Text = "Remove Selected Line";
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnSave (primary)
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(79, 163, 224);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 143, 204);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(752, 110);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(220, 44);
            btnSave.TabIndex = 14;
            btnSave.Text = "Save Sale";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnClear (secondary: flat outline)
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.BackColor = Color.White;
            btnClear.Cursor = Cursors.Hand;
            btnClear.FlatAppearance.BorderColor = Color.FromArgb(79, 163, 224);
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 248, 255);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.ForeColor = Color.FromArgb(59, 143, 204);
            btnClear.Location = new Point(982, 110);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 44);
            btnClear.TabIndex = 15;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // pnlItem
            // 
            pnlItem.BackColor = Color.FromArgb(240, 248, 255);
            pnlItem.Controls.Add(lblArticle);
            pnlItem.Controls.Add(cmbArticle);
            pnlItem.Controls.Add(lblColor);
            pnlItem.Controls.Add(cmbColor);
            pnlItem.Controls.Add(lblBatch);
            pnlItem.Controls.Add(cmbBatch);
            pnlItem.Controls.Add(lblUnit);
            pnlItem.Controls.Add(cmbUnit);
            pnlItem.Controls.Add(lblQty);
            pnlItem.Controls.Add(numQty);
            pnlItem.Controls.Add(lblBasis);
            pnlItem.Controls.Add(cmbBasis);
            pnlItem.Controls.Add(lblPrice);
            pnlItem.Controls.Add(numPrice);
            pnlItem.Controls.Add(lblCost);
            pnlItem.Controls.Add(lblStock);
            pnlItem.Controls.Add(btnAddItem);
            pnlItem.Dock = DockStyle.Top;
            pnlItem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            pnlItem.ForeColor = Color.FromArgb(59, 143, 204);
            pnlItem.Location = new Point(0, 0);
            pnlItem.Name = "pnlItem";
            pnlItem.Size = new Size(1092, 220);
            pnlItem.TabIndex = 0;
            pnlItem.TabStop = false;
            pnlItem.Text = "Shop Sale: Add Item";
            // 
            // lblArticle
            // 
            lblArticle.AutoSize = true;
            lblArticle.Font = new Font("Segoe UI", 10F);
            lblArticle.ForeColor = Color.FromArgb(30, 42, 54);
            lblArticle.Location = new Point(12, 40);
            lblArticle.Name = "lblArticle";
            lblArticle.Size = new Size(61, 25);
            lblArticle.TabIndex = 0;
            lblArticle.Text = "Article";
            // 
            // cmbArticle
            // 
            cmbArticle.BackColor = Color.White;
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArticle.FlatStyle = FlatStyle.Flat;
            cmbArticle.Font = new Font("Segoe UI", 10F);
            cmbArticle.ForeColor = Color.FromArgb(30, 42, 54);
            cmbArticle.Location = new Point(90, 36);
            cmbArticle.Name = "cmbArticle";
            cmbArticle.Size = new Size(220, 33);
            cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            // 
            // lblColor
            // 
            lblColor.AutoSize = true;
            lblColor.Font = new Font("Segoe UI", 10F);
            lblColor.ForeColor = Color.FromArgb(30, 42, 54);
            lblColor.Location = new Point(330, 40);
            lblColor.Name = "lblColor";
            lblColor.Size = new Size(55, 25);
            lblColor.TabIndex = 2;
            lblColor.Text = "Color";
            // 
            // cmbColor
            // 
            cmbColor.BackColor = Color.White;
            cmbColor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbColor.FlatStyle = FlatStyle.Flat;
            cmbColor.Font = new Font("Segoe UI", 10F);
            cmbColor.ForeColor = Color.FromArgb(30, 42, 54);
            cmbColor.Location = new Point(395, 36);
            cmbColor.Name = "cmbColor";
            cmbColor.Size = new Size(200, 33);
            cmbColor.TabIndex = 2;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            // 
            // lblBatch
            // 
            lblBatch.AutoSize = true;
            lblBatch.Font = new Font("Segoe UI", 10F);
            lblBatch.ForeColor = Color.FromArgb(30, 42, 54);
            lblBatch.Location = new Point(615, 40);
            lblBatch.Name = "lblBatch";
            lblBatch.Size = new Size(55, 25);
            lblBatch.TabIndex = 3;
            lblBatch.Text = "Batch";
            // 
            // cmbBatch
            // 
            cmbBatch.BackColor = Color.White;
            cmbBatch.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBatch.FlatStyle = FlatStyle.Flat;
            cmbBatch.Font = new Font("Segoe UI", 10F);
            cmbBatch.ForeColor = Color.FromArgb(30, 42, 54);
            cmbBatch.Location = new Point(680, 36);
            cmbBatch.Name = "cmbBatch";
            cmbBatch.Size = new Size(402, 33);
            cmbBatch.TabIndex = 3;
            cmbBatch.SelectedIndexChanged += Hints_Changed;
            // 
            // lblUnit
            // 
            lblUnit.AutoSize = true;
            lblUnit.Font = new Font("Segoe UI", 10F);
            lblUnit.ForeColor = Color.FromArgb(30, 42, 54);
            lblUnit.Location = new Point(12, 92);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(44, 25);
            lblUnit.TabIndex = 4;
            lblUnit.Text = "Unit";
            // 
            // cmbUnit
            // 
            cmbUnit.BackColor = Color.White;
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.FlatStyle = FlatStyle.Flat;
            cmbUnit.Font = new Font("Segoe UI", 10F);
            cmbUnit.ForeColor = Color.FromArgb(30, 42, 54);
            cmbUnit.Location = new Point(90, 88);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(110, 33);
            cmbUnit.TabIndex = 4;
            cmbUnit.SelectedIndexChanged += cmbUnit_Changed;
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Font = new Font("Segoe UI", 10F);
            lblQty.ForeColor = Color.FromArgb(30, 42, 54);
            lblQty.Location = new Point(220, 92);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(50, 25);
            lblQty.TabIndex = 5;
            lblQty.Text = "Suits";
            // 
            // numQty
            // 
            numQty.BorderStyle = BorderStyle.FixedSingle;
            numQty.Font = new Font("Segoe UI", 10F);
            numQty.ForeColor = Color.FromArgb(30, 42, 54);
            numQty.Location = new Point(300, 88);
            numQty.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty";
            numQty.Size = new Size(90, 31);
            numQty.TabIndex = 5;
            numQty.TextAlign = HorizontalAlignment.Right;
            numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.ValueChanged += Hints_Changed;
            // 
            // lblBasis
            // 
            lblBasis.AutoSize = true;
            lblBasis.Font = new Font("Segoe UI", 10F);
            lblBasis.ForeColor = Color.FromArgb(30, 42, 54);
            lblBasis.Location = new Point(410, 92);
            lblBasis.Name = "lblBasis";
            lblBasis.Size = new Size(99, 25);
            lblBasis.TabIndex = 6;
            lblBasis.Text = "Bhav kis ka";
            // 
            // cmbBasis
            // 
            cmbBasis.BackColor = Color.White;
            cmbBasis.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBasis.FlatStyle = FlatStyle.Flat;
            cmbBasis.Font = new Font("Segoe UI", 10F);
            cmbBasis.ForeColor = Color.FromArgb(30, 42, 54);
            cmbBasis.Location = new Point(530, 88);
            cmbBasis.Name = "cmbBasis";
            cmbBasis.Size = new Size(200, 33);
            cmbBasis.TabIndex = 6;
            cmbBasis.SelectedIndexChanged += cmbBasis_Changed;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 10F);
            lblPrice.ForeColor = Color.FromArgb(30, 42, 54);
            lblPrice.Location = new Point(750, 92);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(86, 25);
            lblPrice.TabIndex = 7;
            lblPrice.Text = "Price/Suit";
            // 
            // numPrice
            // 
            numPrice.BorderStyle = BorderStyle.FixedSingle;
            numPrice.DecimalPlaces = 2;
            numPrice.Font = new Font("Segoe UI", 10F);
            numPrice.ForeColor = Color.FromArgb(30, 42, 54);
            numPrice.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numPrice.Location = new Point(860, 88);
            numPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(160, 31);
            numPrice.TabIndex = 7;
            numPrice.TextAlign = HorizontalAlignment.Right;
            numPrice.ThousandsSeparator = true;
            numPrice.ValueChanged += Hints_Changed;
            // 
            // lblCost
            // 
            lblCost.AutoSize = true;
            lblCost.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCost.ForeColor = Color.FromArgb(192, 57, 43);
            lblCost.Location = new Point(12, 138);
            lblCost.Name = "lblCost";
            lblCost.Size = new Size(0, 28);
            lblCost.TabIndex = 8;
            // 
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Font = new Font("Segoe UI", 10F);
            lblStock.ForeColor = Color.FromArgb(107, 124, 140);
            lblStock.Location = new Point(12, 176);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(0, 25);
            lblStock.TabIndex = 9;
            // 
            // btnAddItem (primary)
            // 
            btnAddItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddItem.BackColor = Color.FromArgb(79, 163, 224);
            btnAddItem.Cursor = Cursors.Hand;
            btnAddItem.FlatAppearance.BorderSize = 0;
            btnAddItem.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 143, 204);
            btnAddItem.FlatStyle = FlatStyle.Flat;
            btnAddItem.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAddItem.ForeColor = Color.White;
            btnAddItem.Location = new Point(892, 168);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(180, 40);
            btnAddItem.TabIndex = 8;
            btnAddItem.Text = "Add Item";
            btnAddItem.UseVisualStyleBackColor = false;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // tabList
            // 
            tabList.BackColor = Color.White;
            tabList.Controls.Add(gridSales);
            tabList.Controls.Add(gridSaleLines);
            tabList.Controls.Add(pnlSummary);
            tabList.Controls.Add(pnlSearch);
            tabList.Location = new Point(4, 34);
            tabList.Name = "tabList";
            tabList.Size = new Size(1092, 722);
            tabList.TabIndex = 1;
            tabList.Text = "Sales List (Search / Edit / Delete)";
            tabList.UseVisualStyleBackColor = false;
            // 
            // gridSales
            // 
            gridSales.AllowUserToAddRows = false;
            gridSales.AllowUserToDeleteRows = false;
            gridSales.AllowUserToResizeRows = false;
            gridSales.AlternatingRowsDefaultCellStyle = (DataGridViewCellStyle)altStyle.Clone();
            gridSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridSales.BackgroundColor = Color.White;
            gridSales.BorderStyle = BorderStyle.None;
            gridSales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridSales.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridSales.ColumnHeadersDefaultCellStyle = (DataGridViewCellStyle)headerStyle.Clone();
            gridSales.ColumnHeadersHeight = 36;
            gridSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridSales.DefaultCellStyle = (DataGridViewCellStyle)cellStyle.Clone();
            gridSales.Dock = DockStyle.Fill;
            gridSales.EnableHeadersVisualStyles = false;
            gridSales.GridColor = Color.FromArgb(227, 238, 247);
            gridSales.Location = new Point(0, 58);
            gridSales.MultiSelect = false;
            gridSales.Name = "gridSales";
            gridSales.ReadOnly = true;
            gridSales.RowHeadersVisible = false;
            gridSales.RowHeadersWidth = 62;
            gridSales.RowTemplate.Height = 30;
            gridSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridSales.Size = new Size(1092, 444);
            gridSales.TabIndex = 20;
            gridSales.DataBindingComplete += gridSales_DataBindingComplete;
            gridSales.SelectionChanged += gridSales_SelectionChanged;
            // 
            // gridSaleLines
            // 
            gridSaleLines.AllowUserToAddRows = false;
            gridSaleLines.AllowUserToDeleteRows = false;
            gridSaleLines.AllowUserToResizeRows = false;
            gridSaleLines.AlternatingRowsDefaultCellStyle = (DataGridViewCellStyle)altStyle.Clone();
            gridSaleLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridSaleLines.BackgroundColor = Color.White;
            gridSaleLines.BorderStyle = BorderStyle.None;
            gridSaleLines.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridSaleLines.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridSaleLines.ColumnHeadersDefaultCellStyle = (DataGridViewCellStyle)headerStyle.Clone();
            gridSaleLines.ColumnHeadersHeight = 36;
            gridSaleLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            gridSaleLines.DefaultCellStyle = (DataGridViewCellStyle)cellStyle.Clone();
            gridSaleLines.Dock = DockStyle.Bottom;
            gridSaleLines.EnableHeadersVisualStyles = false;
            gridSaleLines.GridColor = Color.FromArgb(227, 238, 247);
            gridSaleLines.Location = new Point(0, 502);
            gridSaleLines.Name = "gridSaleLines";
            gridSaleLines.ReadOnly = true;
            gridSaleLines.RowHeadersVisible = false;
            gridSaleLines.RowHeadersWidth = 62;
            gridSaleLines.RowTemplate.Height = 30;
            gridSaleLines.Size = new Size(1092, 170);
            gridSaleLines.TabIndex = 21;
            // 
            // pnlSummary
            // 
            pnlSummary.BackColor = Color.FromArgb(240, 248, 255);
            pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Dock = DockStyle.Bottom;
            pnlSummary.Location = new Point(0, 672);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(1092, 50);
            pnlSummary.TabIndex = 22;
            // 
            // lblSummary
            // 
            lblSummary.AutoSize = true;
            lblSummary.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblSummary.ForeColor = Color.FromArgb(30, 42, 54);
            lblSummary.Location = new Point(12, 12);
            lblSummary.Name = "lblSummary";
            lblSummary.Size = new Size(0, 30);
            lblSummary.TabIndex = 0;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.FromArgb(240, 248, 255);
            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(lblRange);
            pnlSearch.Controls.Add(cmbRange);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(btnEdit);
            pnlSearch.Controls.Add(btnDelete);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(0, 0);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(1092, 58);
            pnlSearch.TabIndex = 23;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.ForeColor = Color.FromArgb(30, 42, 54);
            lblSearch.Location = new Point(12, 20);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(64, 25);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Search";
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.ForeColor = Color.FromArgb(30, 42, 54);
            txtSearch.Location = new Point(85, 16);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Phone 03001234567, name or sale no";
            txtSearch.Size = new Size(320, 31);
            txtSearch.TabIndex = 24;
            txtSearch.KeyDown += txtSearch_KeyDown;
            // 
            // lblRange
            // 
            lblRange.AutoSize = true;
            lblRange.ForeColor = Color.FromArgb(30, 42, 54);
            lblRange.Location = new Point(425, 20);
            lblRange.Name = "lblRange";
            lblRange.Size = new Size(75, 25);
            lblRange.TabIndex = 25;
            lblRange.Text = "Period";
            // 
            // cmbRange
            // 
            cmbRange.BackColor = Color.White;
            cmbRange.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRange.FlatStyle = FlatStyle.Flat;
            cmbRange.ForeColor = Color.FromArgb(30, 42, 54);
            cmbRange.Location = new Point(500, 16);
            cmbRange.Name = "cmbRange";
            cmbRange.Size = new Size(150, 33);
            cmbRange.TabIndex = 25;
            cmbRange.SelectedIndexChanged += cmbRange_Changed;
            // 
            // btnSearch (primary)
            // 
            btnSearch.BackColor = Color.FromArgb(79, 163, 224);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(59, 143, 204);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(670, 12);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(110, 36);
            btnSearch.TabIndex = 26;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // btnEdit (secondary: flat outline)
            // 
            btnEdit.BackColor = Color.White;
            btnEdit.Cursor = Cursors.Hand;
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(79, 163, 224);
            btnEdit.FlatAppearance.BorderSize = 1;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 248, 255);
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.ForeColor = Color.FromArgb(59, 143, 204);
            btnEdit.Location = new Point(800, 12);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(140, 36);
            btnEdit.TabIndex = 27;
            btnEdit.Text = "Edit Sale";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete (soft red)
            // 
            btnDelete.BackColor = Color.FromArgb(224, 92, 92);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 70, 70);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(950, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(140, 36);
            btnDelete.TabIndex = 28;
            btnDelete.Text = "Delete Sale";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // ShopSaleForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1100, 760);
            Controls.Add(tabs);
            Name = "ShopSaleForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Shop Sale";
            Load += ShopSaleForm_Load;
            tabs.ResumeLayout(false);
            tabSale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            pnlItem.ResumeLayout(false);
            pnlItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            tabList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridSaleLines).EndInit();
            pnlSummary.ResumeLayout(false);
            pnlSummary.PerformLayout();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabs; private TabPage tabSale; private TabPage tabList;
        private GroupBox pnlItem;
        private Label lblArticle; private ComboBox cmbArticle;
        private Label lblColor; private ComboBox cmbColor;
        private Label lblBatch; private ComboBox cmbBatch;
        private Label lblUnit; private ComboBox cmbUnit;
        private Label lblQty; private NumericUpDown numQty;
        private Label lblBasis; private ComboBox cmbBasis;
        private Label lblPrice; private NumericUpDown numPrice;
        private Label lblCost; private Label lblStock;
        private Button btnAddItem;
        private DataGridView grid;
        private Panel pnlBottom;
        private Label lblName; private TextBox txtName;
        private Label lblPhone; private TextBox txtPhone;
        private Label lblGrand; private Label lblTotals; private Label lblProfit; private Label lblMode;
        private Button btnRemove; private Button btnSave; private Button btnClear;
        private DataGridView gridSales; private DataGridView gridSaleLines;
        private Panel pnlSearch;
        private Label lblSearch; private TextBox txtSearch;
        private Label lblRange; private ComboBox cmbRange;
        private Button btnSearch; private Button btnEdit; private Button btnDelete;
        private Panel pnlSummary; private Label lblSummary;
    }
}