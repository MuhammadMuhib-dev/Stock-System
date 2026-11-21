namespace ClothStockSystem.Forms.Orders
{
    partial class OnlineOrderForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // ------------------------------------------------ theme (Navy + Sky Blue, clean cards)
        private static readonly System.Drawing.Color ClrPage = System.Drawing.Color.FromArgb(241, 245, 249);
        private static readonly System.Drawing.Color ClrLine = System.Drawing.Color.FromArgb(226, 232, 240);
        private static readonly System.Drawing.Color ClrBorder = System.Drawing.Color.FromArgb(203, 213, 225);
        private static readonly System.Drawing.Color ClrText = System.Drawing.Color.FromArgb(30, 41, 59);
        private static readonly System.Drawing.Color ClrMuted = System.Drawing.Color.FromArgb(100, 116, 139);
        private static readonly System.Drawing.Color ClrSky = System.Drawing.Color.FromArgb(2, 132, 199);
        private static readonly System.Drawing.Color ClrSkyDark = System.Drawing.Color.FromArgb(3, 105, 161);
        private static readonly System.Drawing.Color ClrSkyLight = System.Drawing.Color.FromArgb(240, 249, 255);
        private static readonly System.Drawing.Color ClrSkySel = System.Drawing.Color.FromArgb(186, 230, 253);
        private static readonly System.Drawing.Color ClrSkyInk = System.Drawing.Color.FromArgb(12, 74, 110);
        private static readonly System.Drawing.Color ClrNavy = System.Drawing.Color.FromArgb(15, 23, 42);
        private static readonly System.Drawing.Color ClrDanger = System.Drawing.Color.FromArgb(220, 38, 38);
        private static readonly System.Drawing.Color ClrDangerDk = System.Drawing.Color.FromArgb(185, 28, 28);

        private void InitializeComponent()
        {
            pnlHeader = new Panel(); lblHeader = new Label(); lblHeaderSub = new Label();
            tabs = new FlatTabControl(); tabSale = new TabPage(); tabList = new TabPage();
            pnlItem = new GroupBox(); tlpItem = new TableLayoutPanel();
            lblArticle = new Label(); cmbArticle = new ComboBox();
            lblColor = new Label(); cmbColor = new ComboBox();
            lblBatch = new Label(); cmbBatch = new ComboBox();
            lblUnit = new Label(); cmbUnit = new ComboBox();
            lblQty = new Label(); numQty = new NumericUpDown();
            lblBasis = new Label(); cmbBasis = new ComboBox();
            lblPrice = new Label(); numPrice = new NumericUpDown();
            lblCost = new Label(); lblStock = new Label();
            btnAddItem = new Button();
            grid = new DataGridView();
            pnlBottom = new Panel(); tlpBottom = new TableLayoutPanel(); tlpCust = new TableLayoutPanel();
            pnlTotals = new Panel(); tlpTotals = new TableLayoutPanel(); tlpButtons = new TableLayoutPanel();
            lblName = new Label(); txtName = new TextBox();
            lblPhone = new Label(); txtPhone = new TextBox();
            lblCity = new Label(); txtCity = new TextBox();
            lblPack = new Label(); numPack = new NumericUpDown();
            lblAddress = new Label(); txtAddress = new TextBox();
            lblCourier = new Label(); cmbCourier = new ComboBox();
            lblTrack = new Label(); txtTrack = new TextBox();
            lblCharge = new Label(); numCharge = new NumericUpDown();
            lblGrand = new Label(); lblTotals = new Label(); lblProfit = new Label(); lblMode = new Label();
            btnRemove = new Button(); btnReturns = new Button(); btnSave = new Button(); btnClear = new Button();
            gridOrders = new DataGridView(); gridLines = new DataGridView();
            pnlSearch = new Panel(); flpSearch = new FlowLayoutPanel(); flpFilter = new FlowLayoutPanel();
            lblSearch = new Label(); txtSearch = new TextBox();
            lblRange = new Label(); cmbRange = new ComboBox();
            btnSearch = new Button(); btnEdit = new Button(); btnDelivered = new Button(); btnDelete = new Button();
            pnlSummary = new Panel(); lblSummary = new Label();
            pnlHeader.SuspendLayout();
            tabs.SuspendLayout(); tabSale.SuspendLayout(); tabList.SuspendLayout();
            pnlItem.SuspendLayout(); tlpItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridLines).BeginInit();
            pnlBottom.SuspendLayout(); tlpBottom.SuspendLayout(); tlpCust.SuspendLayout();
            pnlTotals.SuspendLayout(); tlpTotals.SuspendLayout(); tlpButtons.SuspendLayout();
            pnlSearch.SuspendLayout(); flpSearch.SuspendLayout(); flpFilter.SuspendLayout(); pnlSummary.SuspendLayout();
            SuspendLayout();

            // 
            // pnlHeader (gradient banner)
            // 
            pnlHeader.Controls.Add(lblHeader); pnlHeader.Controls.Add(lblHeaderSub);
            pnlHeader.Dock = DockStyle.Top; pnlHeader.Name = "pnlHeader"; pnlHeader.Size = new Size(1100, 62);
            pnlHeader.Paint += (s, e) =>
            {
                using var br = new System.Drawing.Drawing2D.LinearGradientBrush(
                    pnlHeader.ClientRectangle, ClrNavy, ClrSky, System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(br, pnlHeader.ClientRectangle);
            };
            lblHeader.AutoSize = true; lblHeader.BackColor = System.Drawing.Color.Transparent;
            lblHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 15F);
            lblHeader.ForeColor = System.Drawing.Color.White; lblHeader.Location = new Point(20, 8);
            lblHeader.Name = "lblHeader"; lblHeader.Text = "Online Order";
            lblHeaderSub.AutoSize = true; lblHeaderSub.BackColor = System.Drawing.Color.Transparent;
            lblHeaderSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(186, 230, 253); lblHeaderSub.Location = new Point(22, 37);
            lblHeaderSub.Name = "lblHeaderSub"; lblHeaderSub.Text = "Customer order, courier & tracking in one place";

            // 
            // tabs (flat owner-drawn tabs)
            // 
            tabs.Controls.Add(tabSale); tabs.Controls.Add(tabList);
            tabs.Dock = DockStyle.Fill; tabs.Name = "tabs"; tabs.TabIndex = 0;
            tabs.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            tabs.ItemSize = new Size(190, 46);
            tabs.Padding = new Point(0, 0);
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.SelectedIndexChanged += tabs_SelectedIndexChanged;
            tabSale.BackColor = ClrPage; tabSale.Padding = new Padding(12);
            tabSale.Controls.Add(grid); tabSale.Controls.Add(pnlBottom); tabSale.Controls.Add(pnlItem);
            tabSale.Name = "tabSale"; tabSale.Text = "New Order / Edit"; tabSale.UseVisualStyleBackColor = false;
            tabList.BackColor = ClrPage; tabList.Padding = new Padding(12);
            tabList.Controls.Add(gridOrders); tabList.Controls.Add(gridLines); tabList.Controls.Add(pnlSummary); tabList.Controls.Add(pnlSearch);
            tabList.Name = "tabList"; tabList.Text = "Orders List"; tabList.UseVisualStyleBackColor = false;

            // 
            // pnlItem (add item card) - table layout, koi overlap nahi
            // 
            pnlItem.BackColor = System.Drawing.Color.White;
            pnlItem.Controls.Add(tlpItem);
            pnlItem.Dock = DockStyle.Top; pnlItem.Name = "pnlItem"; pnlItem.Size = new Size(1076, 190);
            pnlItem.Padding = new Padding(10, 6, 10, 8);
            pnlItem.FlatStyle = FlatStyle.Flat;
            pnlItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            pnlItem.ForeColor = ClrSky;
            pnlItem.TabIndex = 0; pnlItem.TabStop = false; pnlItem.Text = "  Add Item  ";

            tlpItem.Dock = DockStyle.Fill; tlpItem.Name = "tlpItem";
            tlpItem.ColumnCount = 8; tlpItem.RowCount = 4;
            for (int i = 0; i < 4; i++)
            {
                tlpItem.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                tlpItem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }
            tlpItem.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tlpItem.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpItem.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpItem.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            // Row 1: Article | Color | Batch | Unit
            AddPair(tlpItem, lblArticle, cmbArticle, 0, 0, 1);
            AddPair(tlpItem, lblColor, cmbColor, 2, 0, 1);
            AddPair(tlpItem, lblBatch, cmbBatch, 4, 0, 1);
            AddPair(tlpItem, lblUnit, cmbUnit, 6, 0, 1);
            // Row 2: Qty | Basis | Price | [Add Item]
            AddPair(tlpItem, lblQty, numQty, 0, 1, 1);
            AddPair(tlpItem, lblBasis, cmbBasis, 2, 1, 1);
            AddPair(tlpItem, lblPrice, numPrice, 4, 1, 1);
            tlpItem.Controls.Add(btnAddItem, 6, 1); tlpItem.SetColumnSpan(btnAddItem, 2);
            // Hints
            tlpItem.Controls.Add(lblCost, 0, 2); tlpItem.SetColumnSpan(lblCost, 8);
            tlpItem.Controls.Add(lblStock, 0, 3); tlpItem.SetColumnSpan(lblStock, 8);

            lblArticle.Name = "lblArticle"; lblArticle.Text = "Article";
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList; cmbArticle.Name = "cmbArticle"; cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            lblColor.Name = "lblColor"; lblColor.Text = "Color";
            cmbColor.DropDownStyle = ComboBoxStyle.DropDownList; cmbColor.Name = "cmbColor"; cmbColor.TabIndex = 2;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            lblBatch.Name = "lblBatch"; lblBatch.Text = "Batch";
            cmbBatch.DropDownStyle = ComboBoxStyle.DropDownList; cmbBatch.Name = "cmbBatch"; cmbBatch.TabIndex = 3;
            cmbBatch.SelectedIndexChanged += Hints_Changed;
            lblUnit.Name = "lblUnit"; lblUnit.Text = "Unit";
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList; cmbUnit.Name = "cmbUnit"; cmbUnit.TabIndex = 4;
            cmbUnit.SelectedIndexChanged += cmbUnit_Changed;
            lblQty.Name = "lblQty"; lblQty.Text = "Suits";
            numQty.Maximum = new decimal(new int[] { 10000, 0, 0, 0 }); numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty"; numQty.TabIndex = 5; numQty.TextAlign = HorizontalAlignment.Right; numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.ValueChanged += Hints_Changed;
            lblBasis.Name = "lblBasis"; lblBasis.Text = "Price Basis";
            cmbBasis.DropDownStyle = ComboBoxStyle.DropDownList; cmbBasis.Name = "cmbBasis"; cmbBasis.TabIndex = 6;
            cmbBasis.SelectedIndexChanged += cmbBasis_Changed;
            lblPrice.Name = "lblPrice"; lblPrice.Text = "Price/Suit";
            numPrice.DecimalPlaces = 2; numPrice.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 }); numPrice.Name = "numPrice";
            numPrice.TabIndex = 7; numPrice.TextAlign = HorizontalAlignment.Right; numPrice.ThousandsSeparator = true;
            numPrice.ValueChanged += Hints_Changed;
            lblCost.AutoSize = true; lblCost.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            lblCost.ForeColor = ClrDanger; lblCost.Margin = new Padding(3, 6, 3, 0); lblCost.Name = "lblCost"; lblCost.Text = "";
            lblStock.AutoSize = true; lblStock.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblStock.ForeColor = ClrMuted; lblStock.Margin = new Padding(3, 2, 3, 0); lblStock.Name = "lblStock"; lblStock.Text = "";
            btnAddItem.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnAddItem.Margin = new Padding(3, 3, 3, 3); btnAddItem.Height = 36;
            btnAddItem.Name = "btnAddItem"; btnAddItem.TabIndex = 8; btnAddItem.Text = "+  Add Item";
            btnAddItem.Click += btnAddItem_Click;
            StylePrimary(btnAddItem);

            // 
            // grid (order ki lines)
            // 
            grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Dock = DockStyle.Fill; grid.MultiSelect = false;
            grid.Name = "grid"; grid.ReadOnly = true; grid.RowHeadersVisible = false; grid.RowHeadersWidth = 62;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.TabIndex = 9;
            StyleGrid(grid, 34, false);

            // 
            // pnlBottom = [ customer card | totals card ]
            // 
            pnlBottom.BackColor = System.Drawing.Color.White;
            pnlBottom.Controls.Add(tlpBottom);
            pnlBottom.Dock = DockStyle.Bottom; pnlBottom.Name = "pnlBottom"; pnlBottom.Size = new Size(1076, 262); pnlBottom.TabIndex = 10;
            pnlBottom.Paint += (s, e) => TopLine(e, pnlBottom);

            tlpBottom.ColumnCount = 2; tlpBottom.RowCount = 1;
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tlpBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBottom.Dock = DockStyle.Fill; tlpBottom.Name = "tlpBottom";
            tlpBottom.Padding = new Padding(8);
            tlpBottom.Controls.Add(tlpCust, 0, 0);
            tlpBottom.Controls.Add(pnlTotals, 1, 0);

            // customer form (4 columns: label,input,label,input)
            tlpCust.ColumnCount = 4; tlpCust.RowCount = 5;
            tlpCust.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpCust.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpCust.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpCust.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 5; i++) tlpCust.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tlpCust.Dock = DockStyle.Fill; tlpCust.Name = "tlpCust"; tlpCust.Padding = new Padding(6, 0, 12, 0);
            AddPair(tlpCust, lblName, txtName, 0, 0, 1);
            AddPair(tlpCust, lblPhone, txtPhone, 2, 0, 1);
            AddPair(tlpCust, lblCity, txtCity, 0, 1, 1);
            AddPair(tlpCust, lblPack, numPack, 2, 1, 1);
            AddPair(tlpCust, lblAddress, txtAddress, 0, 2, 3);
            AddPair(tlpCust, lblCourier, cmbCourier, 0, 3, 1);
            AddPair(tlpCust, lblTrack, txtTrack, 2, 3, 1);
            AddPair(tlpCust, lblCharge, numCharge, 0, 4, 1);

            lblName.Name = "lblName"; lblName.Text = "Name";
            txtName.MaxLength = 100; txtName.Name = "txtName"; txtName.TabIndex = 11;
            lblPhone.Name = "lblPhone"; lblPhone.Text = "Phone";
            txtPhone.MaxLength = 15; txtPhone.Name = "txtPhone"; txtPhone.PlaceholderText = "03001234567"; txtPhone.TabIndex = 12;
            lblCity.Name = "lblCity"; lblCity.Text = "City";
            txtCity.MaxLength = 60; txtCity.Name = "txtCity"; txtCity.TabIndex = 13;
            lblPack.Name = "lblPack"; lblPack.Text = "Packaging";
            numPack.DecimalPlaces = 2; numPack.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numPack.Name = "numPack"; numPack.TabIndex = 14; numPack.TextAlign = HorizontalAlignment.Right;
            numPack.ValueChanged += Totals_Changed;
            lblAddress.Name = "lblAddress"; lblAddress.Text = "Address";
            txtAddress.MaxLength = 250; txtAddress.Name = "txtAddress"; txtAddress.TabIndex = 15;
            lblCourier.Name = "lblCourier"; lblCourier.Text = "Courier";
            cmbCourier.DropDownStyle = ComboBoxStyle.DropDownList; cmbCourier.Name = "cmbCourier"; cmbCourier.TabIndex = 16;
            lblTrack.Name = "lblTrack"; lblTrack.Text = "Tracking ID";
            txtTrack.MaxLength = 50; txtTrack.Name = "txtTrack"; txtTrack.TabIndex = 17;
            lblCharge.Name = "lblCharge"; lblCharge.Text = "COD / Courier";
            numCharge.DecimalPlaces = 2; numCharge.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numCharge.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 }); numCharge.Name = "numCharge";
            numCharge.TabIndex = 18; numCharge.TextAlign = HorizontalAlignment.Right; numCharge.ThousandsSeparator = true;
            numCharge.ValueChanged += Totals_Changed;

            // totals card
            pnlTotals.BackColor = ClrSkyLight;
            pnlTotals.Controls.Add(tlpTotals);
            pnlTotals.Dock = DockStyle.Fill; pnlTotals.Name = "pnlTotals"; pnlTotals.Padding = new Padding(18, 10, 18, 12);
            pnlTotals.Paint += (s, e) =>
            {
                using var br = new System.Drawing.SolidBrush(ClrSky);
                e.Graphics.FillRectangle(br, 0, 0, 5, pnlTotals.Height);
            };

            tlpTotals.ColumnCount = 1; tlpTotals.RowCount = 5;
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTotals.Dock = DockStyle.Fill; tlpTotals.Name = "tlpTotals"; tlpTotals.BackColor = System.Drawing.Color.Transparent;
            tlpTotals.Controls.Add(lblGrand, 0, 0);
            tlpTotals.Controls.Add(lblTotals, 0, 1);
            tlpTotals.Controls.Add(lblProfit, 0, 2);
            tlpTotals.Controls.Add(lblMode, 0, 3);
            tlpTotals.Controls.Add(tlpButtons, 0, 4);

            lblGrand.AutoSize = true; lblGrand.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            lblGrand.ForeColor = ClrSkyInk; lblGrand.Margin = new Padding(0, 0, 0, 2); lblGrand.Name = "lblGrand"; lblGrand.Text = "";
            lblTotals.AutoSize = true; lblTotals.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblTotals.ForeColor = ClrMuted; lblTotals.Margin = new Padding(0, 0, 0, 2); lblTotals.Name = "lblTotals"; lblTotals.Text = "";
            lblProfit.AutoSize = true; lblProfit.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            lblProfit.ForeColor = ClrText; lblProfit.Margin = new Padding(0, 0, 0, 2); lblProfit.Name = "lblProfit"; lblProfit.Text = "";
            lblMode.AutoSize = true; lblMode.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            lblMode.ForeColor = ClrSky; lblMode.Margin = new Padding(0); lblMode.Name = "lblMode"; lblMode.Text = "";

            // action buttons: 2 x 2
            tlpButtons.ColumnCount = 2; tlpButtons.RowCount = 2;
            tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpButtons.Dock = DockStyle.Fill; tlpButtons.Name = "tlpButtons"; tlpButtons.Margin = new Padding(0, 6, 0, 0);
            tlpButtons.BackColor = System.Drawing.Color.Transparent;
            tlpButtons.Controls.Add(btnRemove, 0, 0);
            tlpButtons.Controls.Add(btnReturns, 1, 0);
            tlpButtons.Controls.Add(btnClear, 0, 1);
            tlpButtons.Controls.Add(btnSave, 1, 1);
            btnRemove.Dock = DockStyle.Fill; btnRemove.Margin = new Padding(0, 0, 4, 4); btnRemove.Name = "btnRemove"; btnRemove.TabIndex = 19;
            btnRemove.Text = "Remove Line"; btnRemove.Click += btnRemove_Click;
            StyleSecondary(btnRemove);
            btnReturns.Dock = DockStyle.Fill; btnReturns.Margin = new Padding(4, 0, 0, 4); btnReturns.Name = "btnReturns"; btnReturns.TabIndex = 20;
            btnReturns.Text = "Return"; btnReturns.Click += btnReturns_Click;
            StyleDanger(btnReturns);
            btnClear.Dock = DockStyle.Fill; btnClear.Margin = new Padding(0, 4, 4, 0); btnClear.Name = "btnClear"; btnClear.TabIndex = 22;
            btnClear.Text = "Clear"; btnClear.Click += btnClear_Click;
            StyleSecondary(btnClear);
            btnSave.Dock = DockStyle.Fill; btnSave.Margin = new Padding(4, 4, 0, 0); btnSave.Name = "btnSave"; btnSave.TabIndex = 21;
            btnSave.Text = "Save Order"; btnSave.Click += btnSave_Click;
            StylePrimary(btnSave);
            btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);

            // 
            // Orders List tab
            // 
            gridOrders.AllowUserToAddRows = false; gridOrders.AllowUserToDeleteRows = false; gridOrders.AllowUserToResizeRows = false;
            gridOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridOrders.Dock = DockStyle.Fill; gridOrders.MultiSelect = false;
            gridOrders.Name = "gridOrders"; gridOrders.ReadOnly = true; gridOrders.RowHeadersVisible = false; gridOrders.RowHeadersWidth = 62;
            gridOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect; gridOrders.TabIndex = 30;
            gridOrders.SelectionChanged += gridOrders_SelectionChanged;
            gridOrders.DataBindingComplete += gridOrders_DataBindingComplete;
            StyleGrid(gridOrders, 34, false);
            gridLines.AllowUserToAddRows = false; gridLines.AllowUserToDeleteRows = false; gridLines.AllowUserToResizeRows = false;
            gridLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridLines.Dock = DockStyle.Bottom;
            gridLines.Name = "gridLines"; gridLines.ReadOnly = true; gridLines.RowHeadersVisible = false; gridLines.RowHeadersWidth = 62;
            gridLines.Size = new Size(1076, 150); gridLines.TabIndex = 31;
            StyleGrid(gridLines, 30, true);
            // pnlSummary
            pnlSummary.BackColor = ClrSkyLight; pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Dock = DockStyle.Bottom; pnlSummary.Name = "pnlSummary"; pnlSummary.Size = new Size(1076, 44); pnlSummary.TabIndex = 32;
            pnlSummary.Paint += (s, e) => TopLine(e, pnlSummary);
            lblSummary.AutoSize = true; lblSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            lblSummary.ForeColor = ClrSkyInk;
            lblSummary.Location = new Point(16, 12); lblSummary.Name = "lblSummary"; lblSummary.Text = "";

            // 
            // pnlSearch: left = filters (flow), right = buttons (flow) -> overlap nahi
            // 
            pnlSearch.BackColor = System.Drawing.Color.White;
            pnlSearch.Controls.Add(flpFilter);
            pnlSearch.Controls.Add(flpSearch);
            pnlSearch.Dock = DockStyle.Top; pnlSearch.Name = "pnlSearch"; pnlSearch.Size = new Size(1076, 58); pnlSearch.TabIndex = 33;
            pnlSearch.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(ClrLine);
                e.Graphics.DrawLine(pen, 0, pnlSearch.Height - 1, pnlSearch.Width, pnlSearch.Height - 1);
            };
            flpFilter.BackColor = System.Drawing.Color.Transparent;
            flpFilter.Controls.Add(lblSearch); flpFilter.Controls.Add(txtSearch); flpFilter.Controls.Add(lblRange); flpFilter.Controls.Add(cmbRange);
            flpFilter.Dock = DockStyle.Fill; flpFilter.FlowDirection = FlowDirection.LeftToRight;
            flpFilter.Name = "flpFilter"; flpFilter.Padding = new Padding(12, 12, 0, 0); flpFilter.WrapContents = false; flpFilter.TabIndex = 41;
            lblSearch.AutoSize = true; lblSearch.Margin = new Padding(0, 7, 6, 0); lblSearch.Name = "lblSearch"; lblSearch.Text = "Search";
            txtSearch.Margin = new Padding(0, 3, 20, 0); txtSearch.Name = "txtSearch"; txtSearch.Size = new Size(300, 25); txtSearch.TabIndex = 34;
            txtSearch.PlaceholderText = "Tracking ID, phone, name or order no";
            txtSearch.KeyDown += txtSearch_KeyDown;
            lblRange.AutoSize = true; lblRange.Margin = new Padding(0, 7, 6, 0); lblRange.Name = "lblRange"; lblRange.Text = "Period";
            cmbRange.DropDownStyle = ComboBoxStyle.DropDownList; cmbRange.Margin = new Padding(0, 3, 0, 0); cmbRange.Name = "cmbRange"; cmbRange.Size = new Size(140, 25); cmbRange.TabIndex = 35;
            cmbRange.SelectedIndexChanged += cmbRange_Changed;
            // button bar (right side)
            flpSearch.AutoSize = true;
            flpSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpSearch.BackColor = System.Drawing.Color.Transparent;
            flpSearch.Controls.Add(btnSearch); flpSearch.Controls.Add(btnEdit); flpSearch.Controls.Add(btnDelivered); flpSearch.Controls.Add(btnDelete);
            flpSearch.Dock = DockStyle.Right;
            flpSearch.FlowDirection = FlowDirection.LeftToRight;
            flpSearch.Name = "flpSearch";
            flpSearch.Padding = new Padding(0, 11, 12, 0);
            flpSearch.TabIndex = 40;
            flpSearch.WrapContents = false;
            btnSearch.Margin = new Padding(0, 0, 8, 0);
            btnSearch.Name = "btnSearch"; btnSearch.Size = new Size(90, 34); btnSearch.TabIndex = 36;
            btnSearch.Text = "Search"; btnSearch.Click += btnSearch_Click;
            StylePrimary(btnSearch);
            btnEdit.Margin = new Padding(0, 0, 8, 0);
            btnEdit.Name = "btnEdit"; btnEdit.Size = new Size(100, 34); btnEdit.TabIndex = 37;
            btnEdit.Text = "Edit Order"; btnEdit.Click += btnEdit_Click;
            StyleSecondary(btnEdit);
            btnDelivered.Margin = new Padding(0, 0, 8, 0);
            btnDelivered.Name = "btnDelivered"; btnDelivered.Size = new Size(100, 34); btnDelivered.TabIndex = 38;
            btnDelivered.Text = "Delivered"; btnDelivered.Click += btnDelivered_Click;
            StyleSecondary(btnDelivered);
            btnDelete.Margin = new Padding(0);
            btnDelete.Name = "btnDelete"; btnDelete.Size = new Size(90, 34);
            btnDelete.TabIndex = 39; btnDelete.Text = "Delete"; btnDelete.Click += btnDelete_Click;
            StyleDanger(btnDelete);

            // 
            // Label / input styles
            // 
            foreach (var l in new[] { lblArticle, lblColor, lblBatch, lblUnit, lblQty, lblBasis, lblPrice,
                lblName, lblPhone, lblCity, lblPack, lblAddress, lblCourier, lblTrack, lblCharge, lblSearch, lblRange })
            {
                l.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
                l.ForeColor = ClrSkyInk;
            }
            foreach (Control c in new Control[] { cmbArticle, cmbColor, cmbBatch, cmbUnit, numQty, cmbBasis, numPrice,
                txtName, txtPhone, txtCity, numPack, txtAddress, cmbCourier, txtTrack, numCharge, txtSearch, cmbRange })
                StyleInput(c);
            // 
            // OnlineOrderForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = ClrPage;
            ClientSize = new Size(1100, 780);
            MinimumSize = new Size(1020, 700);
            Controls.Add(tabs);
            Controls.Add(pnlHeader);
            Font = new System.Drawing.Font("Segoe UI", 9.5F);
            Name = "OnlineOrderForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Online Order";
            Load += OnlineOrderForm_Load;
            pnlHeader.ResumeLayout(false); pnlHeader.PerformLayout();
            pnlItem.ResumeLayout(false); tlpItem.ResumeLayout(false); tlpItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPack).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridLines).EndInit();
            tlpButtons.ResumeLayout(false); tlpTotals.ResumeLayout(false); tlpTotals.PerformLayout();
            pnlTotals.ResumeLayout(false); tlpCust.ResumeLayout(false); tlpCust.PerformLayout();
            tlpBottom.ResumeLayout(false); pnlBottom.ResumeLayout(false);
            flpSearch.ResumeLayout(false); flpSearch.PerformLayout();
            flpFilter.ResumeLayout(false); flpFilter.PerformLayout();
            pnlSearch.ResumeLayout(false);
            pnlSummary.ResumeLayout(false); pnlSummary.PerformLayout();
            tabSale.ResumeLayout(false); tabList.ResumeLayout(false); tabs.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ------------------------------------------------ helpers

        // label + input ko table ke cell me rakhta hai (alignment automatic, overlap nahi)
        private static void AddPair(TableLayoutPanel t, Label l, Control c, int col, int row, int inputSpan)
        {
            l.AutoSize = true;
            l.Anchor = AnchorStyles.Left;
            l.Margin = new Padding(3, 3, 8, 3);
            c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            c.Margin = new Padding(3, 3, 16, 3);
            t.Controls.Add(l, col, row);
            t.Controls.Add(c, col + 1, row);
            if (inputSpan > 1) t.SetColumnSpan(c, inputSpan);
        }

        private static void TopLine(PaintEventArgs e, Control c)
        {
            using var pen = new System.Drawing.Pen(ClrLine);
            e.Graphics.DrawLine(pen, 0, 0, c.Width, 0);
        }

        private static void StyleInput(Control c)
        {
            c.Font = new System.Drawing.Font("Segoe UI", 10F);
            c.ForeColor = ClrText;
            switch (c)
            {
                case TextBox t: t.BorderStyle = BorderStyle.FixedSingle; break;
                case ComboBox cb: cb.FlatStyle = FlatStyle.Flat; cb.BackColor = System.Drawing.Color.White; break;
                case NumericUpDown n: n.BorderStyle = BorderStyle.FixedSingle; break;
            }
        }

        // Primary action: sky blue
        private static void StylePrimary(Button b)
        {
            b.BackColor = ClrSky;
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ClrSkyDark;
            b.FlatAppearance.MouseDownBackColor = ClrSkyInk;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            b.ForeColor = System.Drawing.Color.White;
            b.UseVisualStyleBackColor = false;
        }

        // Secondary action: white with soft border
        private static void StyleSecondary(Button b)
        {
            b.BackColor = System.Drawing.Color.White;
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.BorderColor = ClrSky;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = ClrSkyLight;
            b.FlatAppearance.MouseDownBackColor = ClrSkySel;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            b.ForeColor = ClrSkyDark;
            b.UseVisualStyleBackColor = false;
        }

        // Destructive action: red
        private static void StyleDanger(Button b)
        {
            b.BackColor = ClrDanger;
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ClrDangerDk;
            b.FlatAppearance.MouseDownBackColor = ClrDangerDk;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            b.ForeColor = System.Drawing.Color.White;
            b.UseVisualStyleBackColor = false;
        }

        // Clean table: navy/sky header, zebra rows, sky selection
        private static void StyleGrid(DataGridView g, int rowHeight, bool compact)
        {
            g.BackgroundColor = System.Drawing.Color.White;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            g.EnableHeadersVisualStyles = false;
            g.GridColor = ClrLine;
            g.RowTemplate.Height = rowHeight;
            g.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = compact ? ClrSkyLight : ClrSkyInk,
                Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F),
                ForeColor = compact ? ClrSkyInk : System.Drawing.Color.White,
                Padding = new Padding(8, 7, 0, 7),
                SelectionBackColor = compact ? ClrSkyLight : ClrSkyInk,
                SelectionForeColor = compact ? ClrSkyInk : System.Drawing.Color.White,
                WrapMode = DataGridViewTriState.True
            };
            g.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9.5F),
                ForeColor = ClrText,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk,
                WrapMode = DataGridViewTriState.False
            };
            g.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ClrSkyLight,
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk
            };
        }

        #endregion

        private Panel pnlHeader; private Label lblHeader; private Label lblHeaderSub;
        private TableLayoutPanel tlpItem; private TableLayoutPanel tlpBottom; private TableLayoutPanel tlpCust;
        private Panel pnlTotals; private TableLayoutPanel tlpTotals; private TableLayoutPanel tlpButtons;
        private FlowLayoutPanel flpFilter;

        private FlatTabControl tabs; private TabPage tabSale; private TabPage tabList;
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
        private Label lblCity; private TextBox txtCity;
        private Label lblPack; private NumericUpDown numPack;
        private Label lblAddress; private TextBox txtAddress;
        private Label lblCourier; private ComboBox cmbCourier;
        private Label lblTrack; private TextBox txtTrack;
        private Label lblCharge; private NumericUpDown numCharge;
        private Label lblGrand; private Label lblTotals; private Label lblProfit; private Label lblMode;
        private Button btnRemove; private Button btnReturns; private Button btnSave; private Button btnClear;
        private DataGridView gridOrders; private DataGridView gridLines;
        private Panel pnlSearch; private FlowLayoutPanel flpSearch;
        private Label lblSearch; private TextBox txtSearch;
        private Label lblRange; private ComboBox cmbRange;
        private Button btnSearch; private Button btnEdit; private Button btnDelivered; private Button btnDelete;
        private Panel pnlSummary; private Label lblSummary;
    }

    // Modern flat "pill" tab control: poora tab strip khud paint hota hai (default grey/old look nahi)
    internal class FlatTabControl : TabControl
    {
        private int _hot = -1;

        public FlatTabControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int hot = -1;
            for (int i = 0; i < TabCount; i++)
                if (GetTabRect(i).Contains(e.Location)) { hot = i; break; }
            if (hot != _hot) { _hot = hot; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hot != -1) { _hot = -1; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(System.Drawing.Color.FromArgb(241, 245, 249));

            // tab strip background (white bar + thin bottom line)
            int stripH = ItemSize.Height + 4;
            using (var white = new System.Drawing.SolidBrush(System.Drawing.Color.White))
                g.FillRectangle(white, 0, 0, Width, stripH);
            using (var line = new System.Drawing.Pen(System.Drawing.Color.FromArgb(226, 232, 240)))
                g.DrawLine(line, 0, stripH, Width, stripH);

            for (int i = 0; i < TabCount; i++)
            {
                var r = GetTabRect(i);
                var pill = new Rectangle(r.X + 6, r.Y + 6, r.Width - 8, r.Height - 8);
                bool sel = i == SelectedIndex;
                var fill = sel ? System.Drawing.Color.FromArgb(2, 132, 199)
                         : i == _hot ? System.Drawing.Color.FromArgb(224, 242, 254)
                         : System.Drawing.Color.White;
                var fore = sel ? System.Drawing.Color.White
                         : System.Drawing.Color.FromArgb(71, 85, 105);

                using (var path = RoundRect(pill, 8))
                using (var br = new System.Drawing.SolidBrush(fill))
                    g.FillPath(br, path);

                TextRenderer.DrawText(g, TabPages[i].Text, Font, pill, fore,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new System.Drawing.Drawing2D.GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}