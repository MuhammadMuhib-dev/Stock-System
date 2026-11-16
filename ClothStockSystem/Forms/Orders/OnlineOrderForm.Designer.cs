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

        // ------------------------------------------------ theme (Sky Blue, minimal, compact)
        private static readonly System.Drawing.Color ClrPage = System.Drawing.Color.FromArgb(248, 250, 252);
        private static readonly System.Drawing.Color ClrLine = System.Drawing.Color.FromArgb(226, 232, 240);
        private static readonly System.Drawing.Color ClrBorder = System.Drawing.Color.FromArgb(203, 213, 225);
        private static readonly System.Drawing.Color ClrText = System.Drawing.Color.FromArgb(30, 41, 59);
        private static readonly System.Drawing.Color ClrMuted = System.Drawing.Color.FromArgb(100, 116, 139);
        private static readonly System.Drawing.Color ClrSky = System.Drawing.Color.FromArgb(2, 132, 199);
        private static readonly System.Drawing.Color ClrSkyDark = System.Drawing.Color.FromArgb(3, 105, 161);
        private static readonly System.Drawing.Color ClrSkyLight = System.Drawing.Color.FromArgb(240, 249, 255);
        private static readonly System.Drawing.Color ClrSkySel = System.Drawing.Color.FromArgb(224, 242, 254);
        private static readonly System.Drawing.Color ClrSkyInk = System.Drawing.Color.FromArgb(12, 74, 110);
        private static readonly System.Drawing.Color ClrDanger = System.Drawing.Color.FromArgb(220, 38, 38);
        private static readonly System.Drawing.Color ClrDangerDk = System.Drawing.Color.FromArgb(185, 28, 28);

        private void InitializeComponent()
        {
            tabs = new TabControl(); tabSale = new TabPage(); tabList = new TabPage();
            pnlItem = new GroupBox();
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
            pnlBottom = new Panel();
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
            pnlSearch = new Panel(); flpSearch = new FlowLayoutPanel();
            lblSearch = new Label(); txtSearch = new TextBox();
            lblRange = new Label(); cmbRange = new ComboBox();
            btnSearch = new Button(); btnEdit = new Button(); btnDelivered = new Button(); btnDelete = new Button();
            pnlSummary = new Panel(); lblSummary = new Label();
            tabs.SuspendLayout(); tabSale.SuspendLayout(); tabList.SuspendLayout();
            pnlItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridLines).BeginInit();
            pnlBottom.SuspendLayout(); pnlSearch.SuspendLayout(); flpSearch.SuspendLayout(); pnlSummary.SuspendLayout();
            SuspendLayout();
            // 
            // tabs (flat owner-drawn tabs)
            // 
            tabs.Controls.Add(tabSale); tabs.Controls.Add(tabList);
            tabs.Dock = DockStyle.Fill; tabs.Name = "tabs"; tabs.TabIndex = 0;
            tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabs.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            tabs.ItemSize = new Size(230, 34);
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.DrawItem += (s, e) =>
            {
                var r = tabs.GetTabRect(e.Index);
                bool sel = e.Index == tabs.SelectedIndex;
                using (var bg = new System.Drawing.SolidBrush(sel ? System.Drawing.Color.White : ClrPage))
                    e.Graphics.FillRectangle(bg, r);
                TextRenderer.DrawText(e.Graphics, tabs.TabPages[e.Index].Text, tabs.Font, r,
                    sel ? ClrSkyInk : ClrMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                if (sel)
                {
                    using var pen = new System.Drawing.Pen(ClrSky, 3F);
                    e.Graphics.DrawLine(pen, r.Left, r.Bottom - 2, r.Right, r.Bottom - 2);
                }
            };
            tabs.SelectedIndexChanged += tabs_SelectedIndexChanged;
            tabSale.BackColor = ClrPage;
            tabSale.Controls.Add(grid); tabSale.Controls.Add(pnlBottom); tabSale.Controls.Add(pnlItem);
            tabSale.Name = "tabSale"; tabSale.Text = "New Order / Edit"; tabSale.UseVisualStyleBackColor = false;
            tabList.BackColor = ClrPage;
            tabList.Controls.Add(gridOrders); tabList.Controls.Add(gridLines); tabList.Controls.Add(pnlSummary); tabList.Controls.Add(pnlSearch);
            tabList.Name = "tabList"; tabList.Text = "Orders List (Search / Edit / Delete)"; tabList.UseVisualStyleBackColor = false;
            // 
            // pnlItem (add item card, compact)
            // 
            pnlItem.BackColor = System.Drawing.Color.White;
            pnlItem.Controls.Add(lblArticle); pnlItem.Controls.Add(cmbArticle);
            pnlItem.Controls.Add(lblColor); pnlItem.Controls.Add(cmbColor);
            pnlItem.Controls.Add(lblBatch); pnlItem.Controls.Add(cmbBatch);
            pnlItem.Controls.Add(lblUnit); pnlItem.Controls.Add(cmbUnit);
            pnlItem.Controls.Add(lblQty); pnlItem.Controls.Add(numQty);
            pnlItem.Controls.Add(lblBasis); pnlItem.Controls.Add(cmbBasis);
            pnlItem.Controls.Add(lblPrice); pnlItem.Controls.Add(numPrice);
            pnlItem.Controls.Add(lblCost); pnlItem.Controls.Add(lblStock); pnlItem.Controls.Add(btnAddItem);
            pnlItem.Dock = DockStyle.Top; pnlItem.Name = "pnlItem"; pnlItem.Size = new Size(1100, 158);
            pnlItem.FlatStyle = FlatStyle.Flat;
            pnlItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            pnlItem.ForeColor = ClrSkyInk;
            pnlItem.TabIndex = 0; pnlItem.TabStop = false; pnlItem.Text = "Online Order: Add Item";
            // Row 1
            lblArticle.AutoSize = true; lblArticle.Location = new Point(16, 36); lblArticle.Name = "lblArticle"; lblArticle.Text = "Article";
            cmbArticle.DropDownStyle = ComboBoxStyle.DropDownList; cmbArticle.Location = new Point(72, 32); cmbArticle.Name = "cmbArticle"; cmbArticle.Size = new Size(210, 25); cmbArticle.TabIndex = 1;
            cmbArticle.SelectedIndexChanged += cmbArticle_SelectedIndexChanged;
            lblColor.AutoSize = true; lblColor.Location = new Point(298, 36); lblColor.Name = "lblColor"; lblColor.Text = "Color";
            cmbColor.DropDownStyle = ComboBoxStyle.DropDownList; cmbColor.Location = new Point(344, 32); cmbColor.Name = "cmbColor"; cmbColor.Size = new Size(180, 25); cmbColor.TabIndex = 2;
            cmbColor.SelectedIndexChanged += cmbColor_SelectedIndexChanged;
            lblBatch.AutoSize = true; lblBatch.Location = new Point(540, 36); lblBatch.Name = "lblBatch"; lblBatch.Text = "Batch";
            cmbBatch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbBatch.DropDownStyle = ComboBoxStyle.DropDownList; cmbBatch.Location = new Point(588, 32); cmbBatch.Name = "cmbBatch"; cmbBatch.Size = new Size(380, 25); cmbBatch.TabIndex = 3;
            cmbBatch.SelectedIndexChanged += Hints_Changed;
            // Row 2
            lblUnit.AutoSize = true; lblUnit.Location = new Point(16, 72); lblUnit.Name = "lblUnit"; lblUnit.Text = "Unit";
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList; cmbUnit.Location = new Point(72, 68); cmbUnit.Name = "cmbUnit"; cmbUnit.Size = new Size(100, 25); cmbUnit.TabIndex = 4;
            cmbUnit.SelectedIndexChanged += cmbUnit_Changed;
            lblQty.AutoSize = true; lblQty.Location = new Point(188, 72); lblQty.Name = "lblQty"; lblQty.Text = "Suits";
            numQty.Location = new Point(232, 68); numQty.Maximum = new decimal(new int[] { 10000, 0, 0, 0 }); numQty.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.Name = "numQty"; numQty.Size = new Size(70, 25); numQty.TabIndex = 5; numQty.TextAlign = HorizontalAlignment.Right; numQty.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numQty.ValueChanged += Hints_Changed;
            lblBasis.AutoSize = true; lblBasis.Location = new Point(318, 72); lblBasis.Name = "lblBasis"; lblBasis.Text = "Price Basis";
            cmbBasis.DropDownStyle = ComboBoxStyle.DropDownList; cmbBasis.Location = new Point(392, 68); cmbBasis.Name = "cmbBasis"; cmbBasis.Size = new Size(160, 25); cmbBasis.TabIndex = 6;
            cmbBasis.SelectedIndexChanged += cmbBasis_Changed;
            lblPrice.AutoSize = true; lblPrice.Location = new Point(568, 72); lblPrice.Name = "lblPrice"; lblPrice.Text = "Price/Suit";
            numPrice.DecimalPlaces = 2; numPrice.Increment = new decimal(new int[] { 10, 0, 0, 0 }); numPrice.Location = new Point(640, 68);
            numPrice.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 }); numPrice.Name = "numPrice"; numPrice.Size = new Size(120, 25);
            numPrice.TabIndex = 7; numPrice.TextAlign = HorizontalAlignment.Right; numPrice.ThousandsSeparator = true;
            numPrice.ValueChanged += Hints_Changed;
            // Hints + Add button
            lblCost.AutoSize = true; lblCost.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            lblCost.ForeColor = ClrDanger; lblCost.Location = new Point(16, 108); lblCost.Name = "lblCost"; lblCost.Text = "";
            lblStock.AutoSize = true; lblStock.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblStock.ForeColor = ClrMuted; lblStock.Location = new Point(16, 130); lblStock.Name = "lblStock"; lblStock.Text = "";
            btnAddItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddItem.Location = new Point(920, 106); btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(160, 34); btnAddItem.TabIndex = 8; btnAddItem.Text = "Add Item";
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
            StyleGrid(grid, 32, false);
            // 
            // pnlBottom (customer + totals + actions, compact)
            // 
            pnlBottom.BackColor = System.Drawing.Color.White;
            pnlBottom.Controls.Add(lblName); pnlBottom.Controls.Add(txtName);
            pnlBottom.Controls.Add(lblPhone); pnlBottom.Controls.Add(txtPhone);
            pnlBottom.Controls.Add(lblCity); pnlBottom.Controls.Add(txtCity);
            pnlBottom.Controls.Add(lblPack); pnlBottom.Controls.Add(numPack);
            pnlBottom.Controls.Add(lblAddress); pnlBottom.Controls.Add(txtAddress);
            pnlBottom.Controls.Add(lblCourier); pnlBottom.Controls.Add(cmbCourier);
            pnlBottom.Controls.Add(lblTrack); pnlBottom.Controls.Add(txtTrack);
            pnlBottom.Controls.Add(lblCharge); pnlBottom.Controls.Add(numCharge);
            pnlBottom.Controls.Add(lblGrand); pnlBottom.Controls.Add(lblTotals); pnlBottom.Controls.Add(lblProfit); pnlBottom.Controls.Add(lblMode);
            pnlBottom.Controls.Add(btnRemove); pnlBottom.Controls.Add(btnReturns); pnlBottom.Controls.Add(btnSave); pnlBottom.Controls.Add(btnClear);
            pnlBottom.Dock = DockStyle.Bottom; pnlBottom.Name = "pnlBottom"; pnlBottom.Size = new Size(1100, 232); pnlBottom.TabIndex = 10;
            pnlBottom.Paint += (s, e) => TopLine(e, pnlBottom);
            // Row A
            lblName.AutoSize = true; lblName.Location = new Point(16, 20); lblName.Name = "lblName"; lblName.Text = "Name";
            txtName.Location = new Point(86, 16); txtName.MaxLength = 100; txtName.Name = "txtName"; txtName.Size = new Size(190, 25); txtName.TabIndex = 11;
            lblPhone.AutoSize = true; lblPhone.Location = new Point(292, 20); lblPhone.Name = "lblPhone"; lblPhone.Text = "Phone";
            txtPhone.Location = new Point(344, 16); txtPhone.MaxLength = 15; txtPhone.Name = "txtPhone"; txtPhone.PlaceholderText = "03001234567"; txtPhone.Size = new Size(150, 25); txtPhone.TabIndex = 12;
            lblCity.AutoSize = true; lblCity.Location = new Point(510, 20); lblCity.Name = "lblCity"; lblCity.Text = "City";
            txtCity.Location = new Point(546, 16); txtCity.MaxLength = 60; txtCity.Name = "txtCity"; txtCity.Size = new Size(140, 25); txtCity.TabIndex = 13;
            lblPack.AutoSize = true; lblPack.Location = new Point(702, 20); lblPack.Name = "lblPack"; lblPack.Text = "Packaging";
            numPack.DecimalPlaces = 2; numPack.Location = new Point(780, 16); numPack.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numPack.Name = "numPack"; numPack.Size = new Size(110, 25); numPack.TabIndex = 14; numPack.TextAlign = HorizontalAlignment.Right;
            numPack.ValueChanged += Totals_Changed;
            // Row B
            lblAddress.AutoSize = true; lblAddress.Location = new Point(16, 56); lblAddress.Name = "lblAddress"; lblAddress.Text = "Address";
            txtAddress.Location = new Point(86, 52); txtAddress.MaxLength = 250; txtAddress.Name = "txtAddress"; txtAddress.Size = new Size(408, 25); txtAddress.TabIndex = 15;
            lblCourier.AutoSize = true; lblCourier.Location = new Point(510, 56); lblCourier.Name = "lblCourier"; lblCourier.Text = "Courier";
            cmbCourier.DropDownStyle = ComboBoxStyle.DropDownList; cmbCourier.Location = new Point(566, 52); cmbCourier.Name = "cmbCourier"; cmbCourier.Size = new Size(180, 25); cmbCourier.TabIndex = 16;
            // Row C
            lblTrack.AutoSize = true; lblTrack.Location = new Point(16, 92); lblTrack.Name = "lblTrack"; lblTrack.Text = "Tracking ID";
            txtTrack.Location = new Point(96, 88); txtTrack.MaxLength = 50; txtTrack.Name = "txtTrack"; txtTrack.Size = new Size(180, 25); txtTrack.TabIndex = 17;
            lblCharge.AutoSize = true; lblCharge.Location = new Point(292, 92); lblCharge.Name = "lblCharge"; lblCharge.Text = "COD / Courier charge";
            numCharge.DecimalPlaces = 2; numCharge.Increment = new decimal(new int[] { 10, 0, 0, 0 }); numCharge.Location = new Point(424, 88);
            numCharge.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 }); numCharge.Name = "numCharge"; numCharge.Size = new Size(110, 25);
            numCharge.TabIndex = 18; numCharge.TextAlign = HorizontalAlignment.Right; numCharge.ThousandsSeparator = true;
            numCharge.ValueChanged += Totals_Changed;
            btnRemove.Location = new Point(560, 86); btnRemove.Name = "btnRemove"; btnRemove.Size = new Size(150, 30); btnRemove.TabIndex = 19;
            btnRemove.Text = "Remove Selected Line"; btnRemove.Click += btnRemove_Click;
            StyleSecondary(btnRemove);
            btnReturns.Location = new Point(720, 86); btnReturns.Name = "btnReturns"; btnReturns.Size = new Size(90, 30);
            btnReturns.TabIndex = 20; btnReturns.Text = "Return"; btnReturns.Click += btnReturns_Click;
            StyleDanger(btnReturns);
            // Totals (left) + Save/Clear (right)
            lblGrand.AutoSize = true; lblGrand.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            lblGrand.ForeColor = ClrSkyInk; lblGrand.Location = new Point(16, 130); lblGrand.Name = "lblGrand"; lblGrand.Text = "";
            lblTotals.AutoSize = true; lblTotals.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblTotals.ForeColor = ClrMuted; lblTotals.Location = new Point(16, 162); lblTotals.Name = "lblTotals"; lblTotals.Text = "";
            lblProfit.AutoSize = true; lblProfit.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            lblProfit.ForeColor = ClrText; lblProfit.Location = new Point(16, 183); lblProfit.Name = "lblProfit"; lblProfit.Text = "";
            lblMode.AutoSize = true; lblMode.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            lblMode.ForeColor = ClrSky; lblMode.Location = new Point(16, 208); lblMode.Name = "lblMode"; lblMode.Text = "";
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(830, 176); btnSave.Name = "btnSave"; btnSave.Size = new Size(170, 38);
            btnSave.TabIndex = 21; btnSave.Text = "Save Order"; btnSave.Click += btnSave_Click;
            StylePrimary(btnSave);
            btnClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClear.Location = new Point(1010, 176); btnClear.Name = "btnClear"; btnClear.Size = new Size(70, 38); btnClear.TabIndex = 22;
            btnClear.Text = "Clear"; btnClear.Click += btnClear_Click;
            StyleSecondary(btnClear);
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
            StyleGrid(gridOrders, 32, false);
            gridLines.AllowUserToAddRows = false; gridLines.AllowUserToDeleteRows = false; gridLines.AllowUserToResizeRows = false;
            gridLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridLines.Dock = DockStyle.Bottom;
            gridLines.Name = "gridLines"; gridLines.ReadOnly = true; gridLines.RowHeadersVisible = false; gridLines.RowHeadersWidth = 62;
            gridLines.Size = new Size(1100, 150); gridLines.TabIndex = 31;
            StyleGrid(gridLines, 30, true);
            // pnlSummary
            pnlSummary.BackColor = System.Drawing.Color.White; pnlSummary.Controls.Add(lblSummary);
            pnlSummary.Dock = DockStyle.Bottom; pnlSummary.Name = "pnlSummary"; pnlSummary.Size = new Size(1100, 42); pnlSummary.TabIndex = 32;
            pnlSummary.Paint += (s, e) => TopLine(e, pnlSummary);
            lblSummary.AutoSize = true; lblSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            lblSummary.ForeColor = ClrSkyInk;
            lblSummary.Location = new Point(16, 11); lblSummary.Name = "lblSummary"; lblSummary.Text = "";
            // 
            // pnlSearch: left = search + period, right = buttons (FlowLayoutPanel, kabhi overlap nahi hota)
            // 
            pnlSearch.BackColor = System.Drawing.Color.White;
            pnlSearch.Controls.Add(flpSearch);
            pnlSearch.Controls.Add(lblSearch); pnlSearch.Controls.Add(txtSearch); pnlSearch.Controls.Add(lblRange); pnlSearch.Controls.Add(cmbRange);
            pnlSearch.Dock = DockStyle.Top; pnlSearch.Name = "pnlSearch"; pnlSearch.Size = new Size(1100, 54); pnlSearch.TabIndex = 33;
            pnlSearch.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(ClrLine);
                e.Graphics.DrawLine(pen, 0, pnlSearch.Height - 1, pnlSearch.Width, pnlSearch.Height - 1);
            };
            lblSearch.AutoSize = true; lblSearch.Location = new Point(16, 19); lblSearch.Name = "lblSearch"; lblSearch.Text = "Search";
            txtSearch.Location = new Point(70, 15); txtSearch.Name = "txtSearch"; txtSearch.Size = new Size(240, 25); txtSearch.TabIndex = 34;
            txtSearch.PlaceholderText = "Tracking ID, phone, name or order no";
            txtSearch.KeyDown += txtSearch_KeyDown;
            lblRange.AutoSize = true; lblRange.Location = new Point(328, 19); lblRange.Name = "lblRange"; lblRange.Text = "Period";
            cmbRange.DropDownStyle = ComboBoxStyle.DropDownList; cmbRange.Location = new Point(376, 15); cmbRange.Name = "cmbRange"; cmbRange.Size = new Size(120, 25); cmbRange.TabIndex = 35;
            cmbRange.SelectedIndexChanged += cmbRange_Changed;
            // button bar (right side)
            flpSearch.AutoSize = true;
            flpSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpSearch.BackColor = System.Drawing.Color.Transparent;
            flpSearch.Controls.Add(btnSearch); flpSearch.Controls.Add(btnEdit); flpSearch.Controls.Add(btnDelivered); flpSearch.Controls.Add(btnDelete);
            flpSearch.Dock = DockStyle.Right;
            flpSearch.FlowDirection = FlowDirection.LeftToRight;
            flpSearch.Name = "flpSearch";
            flpSearch.Padding = new Padding(0, 10, 12, 0);
            flpSearch.TabIndex = 40;
            flpSearch.WrapContents = false;
            btnSearch.Margin = new Padding(0, 0, 8, 0);
            btnSearch.Name = "btnSearch"; btnSearch.Size = new Size(90, 32); btnSearch.TabIndex = 36;
            btnSearch.Text = "Search"; btnSearch.Click += btnSearch_Click;
            StylePrimary(btnSearch);
            btnEdit.Margin = new Padding(0, 0, 8, 0);
            btnEdit.Name = "btnEdit"; btnEdit.Size = new Size(100, 32); btnEdit.TabIndex = 37;
            btnEdit.Text = "Edit Order"; btnEdit.Click += btnEdit_Click;
            StyleSecondary(btnEdit);
            btnDelivered.Margin = new Padding(0, 0, 8, 0);
            btnDelivered.Name = "btnDelivered"; btnDelivered.Size = new Size(100, 32); btnDelivered.TabIndex = 38;
            btnDelivered.Text = "Delivered"; btnDelivered.Click += btnDelivered_Click;
            StyleSecondary(btnDelivered);
            btnDelete.Margin = new Padding(0);
            btnDelete.Name = "btnDelete"; btnDelete.Size = new Size(90, 32);
            btnDelete.TabIndex = 39; btnDelete.Text = "Delete"; btnDelete.Click += btnDelete_Click;
            StyleDanger(btnDelete);
            // 
            // Label / input styles (ek jaisa compact look)
            // 
            foreach (var l in new[] { lblArticle, lblColor, lblBatch, lblUnit, lblQty, lblBasis, lblPrice,
                lblName, lblPhone, lblCity, lblPack, lblAddress, lblCourier, lblTrack, lblCharge, lblSearch, lblRange })
            {
                l.Font = new System.Drawing.Font("Segoe UI", 9F);
                l.ForeColor = ClrMuted;
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
            ClientSize = new Size(1100, 760);
            Controls.Add(tabs);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            Name = "OnlineOrderForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Online Order";
            Load += OnlineOrderForm_Load;
            pnlItem.ResumeLayout(false); pnlItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numQty).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPack).EndInit();
            ((System.ComponentModel.ISupportInitialize)numCharge).EndInit();
            ((System.ComponentModel.ISupportInitialize)grid).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridLines).EndInit();
            pnlBottom.ResumeLayout(false); pnlBottom.PerformLayout();
            flpSearch.ResumeLayout(false); flpSearch.PerformLayout();
            pnlSearch.ResumeLayout(false); pnlSearch.PerformLayout();
            pnlSummary.ResumeLayout(false); pnlSummary.PerformLayout();
            tabSale.ResumeLayout(false); tabList.ResumeLayout(false); tabs.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ------------------------------------------------ helpers

        private static void TopLine(PaintEventArgs e, Control c)
        {
            using var pen = new System.Drawing.Pen(ClrLine);
            e.Graphics.DrawLine(pen, 0, 0, c.Width, 0);
        }

        private static void StyleInput(Control c)
        {
            c.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            b.ForeColor = System.Drawing.Color.White;
            b.UseVisualStyleBackColor = false;
        }

        // Secondary action: white with soft border
        private static void StyleSecondary(Button b)
        {
            b.BackColor = System.Drawing.Color.White;
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.BorderColor = ClrBorder;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = ClrSkyLight;
            b.FlatAppearance.MouseDownBackColor = ClrSkySel;
            b.FlatStyle = FlatStyle.Flat;
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            b.ForeColor = ClrText;
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
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            b.ForeColor = System.Drawing.Color.White;
            b.UseVisualStyleBackColor = false;
        }

        // Clean table: sirf horizontal lines, sky-blue header/selection
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
                BackColor = compact ? ClrPage : ClrSkyLight,
                Font = new System.Drawing.Font("Segoe UI Semibold", 9F),
                ForeColor = compact ? ClrMuted : ClrSkyInk,
                Padding = new Padding(8, 5, 0, 5),
                SelectionBackColor = compact ? ClrPage : ClrSkyLight,
                SelectionForeColor = compact ? ClrMuted : ClrSkyInk,
                WrapMode = DataGridViewTriState.True
            };
            g.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Segoe UI", 9F),
                ForeColor = ClrText,
                Padding = new Padding(8, 0, 0, 0),
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk,
                WrapMode = DataGridViewTriState.False
            };
            g.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ClrPage,
                SelectionBackColor = ClrSkySel,
                SelectionForeColor = ClrSkyInk
            };
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
}