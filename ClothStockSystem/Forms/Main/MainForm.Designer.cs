namespace ClothStockSystem
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // ------------------------------------------------ theme (Sky Blue)
        private static readonly System.Drawing.Color ClrSidebar = System.Drawing.Color.FromArgb(3, 105, 161);   // sky-700
        private static readonly System.Drawing.Color ClrSidebarDeep = System.Drawing.Color.FromArgb(7, 89, 133);    // sky-800
        private static readonly System.Drawing.Color ClrHover = System.Drawing.Color.FromArgb(2, 132, 199);   // sky-600
        private static readonly System.Drawing.Color ClrActiveBg = System.Drawing.Color.FromArgb(14, 165, 233);  // sky-500
        private static readonly System.Drawing.Color ClrAccent = System.Drawing.Color.FromArgb(224, 242, 254); // sky-100
        private static readonly System.Drawing.Color ClrAccentDark = System.Drawing.Color.FromArgb(14, 165, 233);
        private static readonly System.Drawing.Color ClrTextMuted = System.Drawing.Color.FromArgb(186, 230, 253); // sky-200
        private static readonly System.Drawing.Color ClrTextLight = System.Drawing.Color.FromArgb(240, 249, 255); // sky-50
        private static readonly System.Drawing.Color ClrDarkText = System.Drawing.Color.FromArgb(12, 74, 110);   // sky-900
        private static readonly System.Drawing.Color ClrPage = System.Drawing.Color.FromArgb(240, 249, 255);

        private const string IconFontName = "Segoe MDL2 Assets"; // Windows 10 / 11

        private Button _menuActiveButton;

        private void InitializeComponent()
        {
            pnlMenu = new Panel();
            lblTitle = new Label();

            btnGrpManage = new Button(); pnlSubManage = new Panel();
            btnGrpSales = new Button(); pnlSubSales = new Panel();
            btnGrpMoney = new Button(); pnlSubMoney = new Panel();
            btnGrpLedger = new Button(); pnlSubLedger = new Panel();

            btnArticles = new Button();
            btnColors = new Button();
            btnStockIn = new Button();
            btnPurchase = new Button();
            btnStockView = new Button();
            btnStockAdjust = new Button();
            btnShopSale = new Button();
            btnOnlineOrder = new Button();
            btnAllSales = new Button();
            btnReturns = new Button();
            btnProfit = new Button();
            btnReports = new Button();
            btnExpenses = new Button();
            btnWorkers = new Button();
            btnExpSetup = new Button();
            btnLedger = new Button();
            btnCashBook = new Button();
            btnParties = new Button();
            btnCouriers = new Button();
            btnSettings = new Button();
            pnlContent = new Panel();
            tlpHero = new TableLayoutPanel();
            pnlHeroCard = new Panel();
            pnlAccentBar = new Panel();
            lblProjectName = new Label();
            lblProjectTagline = new Label();
            flpChips = new FlowLayoutPanel();
            lblChip1 = new Label();
            lblChip2 = new Label();
            lblChip3 = new Label();
            lblChip4 = new Label();
            pnlMenu.SuspendLayout();
            pnlSubManage.SuspendLayout();
            pnlSubSales.SuspendLayout();
            pnlSubMoney.SuspendLayout();
            pnlSubLedger.SuspendLayout();
            pnlContent.SuspendLayout();
            tlpHero.SuspendLayout();
            pnlHeroCard.SuspendLayout();
            flpChips.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMenu (Dock Top: ulte order mein add hote hain)
            // Upar se neeche: Title, Inventory, Sales & Delivery, Purchase & Expenses, Accounts, Reports, Settings
            // 
            pnlMenu.AutoScroll = true;
            pnlMenu.BackColor = ClrSidebar;
            pnlMenu.Controls.Add(btnSettings);
            pnlMenu.Controls.Add(btnReports);
            pnlMenu.Controls.Add(pnlSubLedger);
            pnlMenu.Controls.Add(btnGrpLedger);
            pnlMenu.Controls.Add(pnlSubMoney);
            pnlMenu.Controls.Add(btnGrpMoney);
            pnlMenu.Controls.Add(pnlSubSales);
            pnlMenu.Controls.Add(btnGrpSales);
            pnlMenu.Controls.Add(pnlSubManage);
            pnlMenu.Controls.Add(btnGrpManage);
            pnlMenu.Controls.Add(lblTitle);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(240, 700);
            pnlMenu.TabIndex = 0;
            // 
            // lblTitle (brand header: logo + name + subtitle)
            // 
            lblTitle.BackColor = ClrSidebar;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13.5F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.White;
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(240, 84);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Cloth Stock";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.Paint += (s, e) =>
            {
                var g = e.Graphics;
                float k = lblTitle.DeviceDpi / 96f;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var full = lblTitle.ClientRectangle;
                if (full.Width <= 0 || full.Height <= 0) return;
                using (var bg = new System.Drawing.Drawing2D.LinearGradientBrush(
                    full, ClrActiveBg, ClrSidebar, 90F))
                    g.FillRectangle(bg, full);
                var tile = new System.Drawing.Rectangle((int)(16 * k), (int)(18 * k), (int)(46 * k), (int)(46 * k));
                using (var tb = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 255, 255)))
                using (var gp = RoundedPath(tile, (int)(12 * k)))
                    g.FillPath(tb, gp);
                DrawGlyph(g, "\uE8EC", 18F, ClrSidebar, tile);
                TextRenderer.DrawText(g, lblTitle.Text, lblTitle.Font,
                    new System.Drawing.Point((int)(72 * k), (int)(16 * k)), System.Drawing.Color.White,
                    TextFormatFlags.NoPadding);
                using var small = new System.Drawing.Font("Segoe UI", 8.5F);
                TextRenderer.DrawText(g, "Inventory & Accounts", small,
                    new System.Drawing.Point((int)(73 * k), (int)(44 * k)), ClrTextLight,
                    TextFormatFlags.NoPadding);
            };
            // 
            // Group 1: Inventory (setup + stock dekhna/theek karna)
            // 
            StyleGroupButton(btnGrpManage, "btnGrpManage", "Inventory", 1, "\uE7B8");
            StyleSubPanel(pnlSubManage, "pnlSubManage", 4);
            pnlSubManage.Controls.Add(btnStockAdjust);
            pnlSubManage.Controls.Add(btnStockView);
            pnlSubManage.Controls.Add(btnColors);
            pnlSubManage.Controls.Add(btnArticles);
            StyleSubButton(btnArticles, "btnArticles", "Articles", 1, "\uE8EC");
            btnArticles.Click += btnArticles_Click;
            StyleSubButton(btnColors, "btnColors", "Colors", 2, "\uE790");
            btnColors.Click += btnColors_Click;
            StyleSubButton(btnStockView, "btnStockView", "Stock View", 3, "\uE890");
            btnStockView.Click += btnStockView_Click;
            StyleSubButton(btnStockAdjust, "btnStockAdjust", "Stock Adjust", 4, "\uE70F");
            btnStockAdjust.Click += btnStockAdjust_Click;
            // 
            // Group 2: Sales & Delivery (bechna + order bhejna + wapsi)
            // 
            StyleGroupButton(btnGrpSales, "btnGrpSales", "Sales && Delivery", 2, "\uE7BF");
            StyleSubPanel(pnlSubSales, "pnlSubSales", 5);
            pnlSubSales.Controls.Add(btnReturns);
            pnlSubSales.Controls.Add(btnCouriers);
            pnlSubSales.Controls.Add(btnAllSales);
            pnlSubSales.Controls.Add(btnOnlineOrder);
            pnlSubSales.Controls.Add(btnShopSale);
            StyleSubButton(btnShopSale, "btnShopSale", "Shop Sale", 1, "\uE719");
            btnShopSale.Click += btnShopSale_Click;
            StyleSubButton(btnOnlineOrder, "btnOnlineOrder", "Online Order", 2, "\uE774");
            btnOnlineOrder.Click += btnOnlineOrder_Click;
            StyleSubButton(btnCouriers, "btnCouriers", "Couriers", 3, "\uE806");
            btnCouriers.Click += btnCouriers_Click;
            StyleSubButton(btnAllSales, "btnAllSales", "All Sales", 4, "\uE8FD");
            btnAllSales.Click += btnAllSales_Click;
            StyleSubButton(btnReturns, "btnReturns", "Returns", 5, "\uE7A7");
            btnReturns.Click += btnReturns_Click;
            // 
            // Group 3: Purchase & Expenses (jo paisa / maal bahar jata hai)
            // 
            StyleGroupButton(btnGrpMoney, "btnGrpMoney", "Purchase && Expenses", 3, "\uE8C7");
            StyleSubPanel(pnlSubMoney, "pnlSubMoney", 5);
            pnlSubMoney.Controls.Add(btnWorkers);
            pnlSubMoney.Controls.Add(btnExpSetup);
            pnlSubMoney.Controls.Add(btnExpenses);
            pnlSubMoney.Controls.Add(btnStockIn);
            pnlSubMoney.Controls.Add(btnPurchase);
            StyleSubButton(btnPurchase, "btnPurchase", "Purchase Bill", 1, "\uE8A5");
            btnPurchase.Click += btnPurchase_Click;
            StyleSubButton(btnStockIn, "btnStockIn", "Stock In", 2, "\uE896");
            btnStockIn.Click += btnStockIn_Click;
            StyleSubButton(btnExpenses, "btnExpenses", "Expenses", 3, "\uE8C7");
            btnExpenses.Click += btnExpenses_Click;
            StyleSubButton(btnExpSetup, "btnExpSetup", "Expense Setup", 4, "\uE713");
            btnExpSetup.Click += btnExpSetup_Click;
            StyleSubButton(btnWorkers, "btnWorkers", "Workers", 5, "\uE716");
            btnWorkers.Click += btnWorkers_Click;
            // 
            // Group 4: Accounts (khata, cash, parties, munafa)
            // 
            StyleGroupButton(btnGrpLedger, "btnGrpLedger", "Accounts", 4, "\uE82D");
            StyleSubPanel(pnlSubLedger, "pnlSubLedger", 4);
            pnlSubLedger.Controls.Add(btnProfit);
            pnlSubLedger.Controls.Add(btnParties);
            pnlSubLedger.Controls.Add(btnCashBook);
            pnlSubLedger.Controls.Add(btnLedger);
            StyleSubButton(btnLedger, "btnLedger", "Ledger (Khata)", 1, "\uE8F1");
            btnLedger.Click += btnLedger_Click;
            StyleSubButton(btnCashBook, "btnCashBook", "Cash Book", 2, "\uE825");
            btnCashBook.Click += btnCashBook_Click;
            StyleSubButton(btnParties, "btnParties", "Parties / Accounts", 3, "\uE716");
            btnParties.Click += btnParties_Click;
            StyleSubButton(btnProfit, "btnProfit", "Profit Summary", 4, "\uE9D2");
            btnProfit.Click += btnProfit_Click;
            // 
            // Reports (main button, group ke bahar)
            // 
            StyleMenuButton(btnReports, "btnReports", "Reports", 5, "\uE9D9");
            btnReports.Click += btnReports_Click;
            // 
            // Settings
            // 
            StyleMenuButton(btnSettings, "btnSettings", "Settings", 6, "\uE713");
            btnSettings.Click += btnSettings_Click;
            // 
            // Group click: ek time par sirf ek group khula rahe
            // 
            _groups.Add((btnGrpManage, pnlSubManage, "Inventory"));
            _groups.Add((btnGrpSales, pnlSubSales, "Sales && Delivery"));
            _groups.Add((btnGrpMoney, pnlSubMoney, "Purchase && Expenses"));
            _groups.Add((btnGrpLedger, pnlSubLedger, "Accounts"));
            foreach (var g in _groups)
            {
                var header = g.Header;
                header.Click += (s, e) => ToggleGroup(header);
                SetGroupText(g.Header, g.Title, false);
            }
            // Active (selected) button highlight
            foreach (var b in new[] { btnArticles, btnColors, btnStockIn, btnPurchase, btnStockView,
                btnStockAdjust, btnShopSale, btnOnlineOrder, btnAllSales, btnReturns, btnCouriers,
                btnExpenses, btnExpSetup, btnWorkers, btnProfit, btnLedger, btnCashBook, btnParties,
                btnReports, btnSettings })
            {
                var btn = b;
                btn.Click += (s, e) => SetMenuActiveButton(btn);
            }
            // 
            // pnlContent
            // 
            pnlContent.BackColor = ClrPage;
            pnlContent.Controls.Add(tlpHero);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Name = "pnlContent";
            pnlContent.TabIndex = 1;
            pnlContent.ControlAdded += (s, e) =>
            {
                if (e.Control != tlpHero)
                {
                    tlpHero.Visible = false;
                    e.Control.BringToFront();
                }
            };
            pnlContent.ControlRemoved += (s, e) =>
            {
                if (e.Control != tlpHero)
                    tlpHero.Visible = pnlContent.Controls.Count <= 1;
            };
            // 
            // tlpHero (card ko beech mein rakhta hai)
            // 
            tlpHero.BackColor = System.Drawing.Color.Transparent;
            tlpHero.ColumnCount = 3;
            tlpHero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpHero.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 820F));
            tlpHero.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpHero.Controls.Add(pnlHeroCard, 1, 1);
            tlpHero.Dock = DockStyle.Fill;
            tlpHero.Name = "tlpHero";
            tlpHero.RowCount = 3;
            tlpHero.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpHero.RowStyles.Add(new RowStyle(SizeType.Absolute, 360F));
            tlpHero.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpHero.TabIndex = 0;
            // 
            // pnlHeroCard (rounded white card)
            // 
            pnlHeroCard.BackColor = System.Drawing.Color.White;
            pnlHeroCard.Controls.Add(flpChips);
            pnlHeroCard.Controls.Add(lblProjectTagline);
            pnlHeroCard.Controls.Add(lblProjectName);
            pnlHeroCard.Controls.Add(pnlAccentBar);
            pnlHeroCard.Dock = DockStyle.Fill;
            pnlHeroCard.Margin = new Padding(0);
            pnlHeroCard.Name = "pnlHeroCard";
            pnlHeroCard.TabIndex = 0;
            RoundCorners(pnlHeroCard, 22);
            pnlHeroCard.Paint += (s, e) =>
            {
                using var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(186, 230, 253), 2F);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlHeroCard.Width - 1, pnlHeroCard.Height - 1);
            };
            // 
            // pnlAccentBar (gradient strip)
            // 
            pnlAccentBar.BackColor = ClrAccentDark;
            pnlAccentBar.Dock = DockStyle.Top;
            pnlAccentBar.Name = "pnlAccentBar";
            pnlAccentBar.Size = new Size(0, 10);
            pnlAccentBar.TabIndex = 0;
            pnlAccentBar.Paint += (s, e) =>
            {
                var r = pnlAccentBar.ClientRectangle;
                if (r.Width <= 0 || r.Height <= 0) return;
                using var br = new System.Drawing.Drawing2D.LinearGradientBrush(
                    r, System.Drawing.Color.FromArgb(56, 189, 248), System.Drawing.Color.FromArgb(99, 102, 241), 0F);
                e.Graphics.FillRectangle(br, r);
            };
            // 
            // lblProjectName
            // 
            lblProjectName.BackColor = System.Drawing.Color.Transparent;
            lblProjectName.Dock = DockStyle.Top;
            lblProjectName.Font = new System.Drawing.Font("Segoe UI Semibold", 30F, System.Drawing.FontStyle.Bold);
            lblProjectName.ForeColor = ClrDarkText;
            lblProjectName.Name = "lblProjectName";
            lblProjectName.Padding = new Padding(44, 26, 0, 0);
            lblProjectName.Size = new Size(0, 110);
            lblProjectName.TabIndex = 1;
            lblProjectName.Text = "Cloth Stock System";
            lblProjectName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblProjectTagline
            // 
            lblProjectTagline.BackColor = System.Drawing.Color.Transparent;
            lblProjectTagline.Dock = DockStyle.Top;
            lblProjectTagline.Font = new System.Drawing.Font("Segoe UI", 12.5F);
            lblProjectTagline.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            lblProjectTagline.Name = "lblProjectTagline";
            lblProjectTagline.Padding = new Padding(48, 0, 0, 0);
            lblProjectTagline.Size = new Size(0, 50);
            lblProjectTagline.TabIndex = 2;
            lblProjectTagline.Text = "Unstitched suits inventory, orders and accounts, all in one place";
            lblProjectTagline.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpChips (4 dashboard cards)
            // 
            flpChips.BackColor = System.Drawing.Color.Transparent;
            flpChips.Controls.Add(lblChip1);
            flpChips.Controls.Add(lblChip2);
            flpChips.Controls.Add(lblChip3);
            flpChips.Controls.Add(lblChip4);
            flpChips.Dock = DockStyle.Fill;
            flpChips.Name = "flpChips";
            flpChips.Padding = new Padding(36, 14, 16, 10);
            flpChips.TabIndex = 3;
            // 
            // chips
            // 
            StyleChip(lblChip1, "lblChip1", "Stock Control", "\uE7B8",
                System.Drawing.Color.FromArgb(240, 249, 255), System.Drawing.Color.FromArgb(14, 165, 233));
            StyleChip(lblChip2, "lblChip2", "Online Orders", "\uE774",
                System.Drawing.Color.FromArgb(238, 242, 255), System.Drawing.Color.FromArgb(99, 102, 241));
            StyleChip(lblChip3, "lblChip3", "Ledger", "\uE82D",
                System.Drawing.Color.FromArgb(236, 253, 245), System.Drawing.Color.FromArgb(16, 185, 129));
            StyleChip(lblChip4, "lblChip4", "Reports", "\uE9D9",
                System.Drawing.Color.FromArgb(255, 247, 237), System.Drawing.Color.FromArgb(249, 115, 22));
            // 
            // MainForm (Dpi scaling: icons, text aur padding aapas mein overlap nahi karte)
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = ClrPage;
            ClientSize = new Size(1300, 700);
            Controls.Add(pnlContent);
            Controls.Add(pnlMenu);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            MinimumSize = new Size(1100, 650);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cloth Stock System";
            WindowState = FormWindowState.Maximized;
            ClientSizeChanged += (s, e) => AdjustSidebar();
            pnlSubManage.ResumeLayout(false);
            pnlSubSales.ResumeLayout(false);
            pnlSubMoney.ResumeLayout(false);
            pnlSubLedger.ResumeLayout(false);
            pnlMenu.ResumeLayout(false);
            flpChips.ResumeLayout(false);
            pnlHeroCard.ResumeLayout(false);
            tlpHero.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            ResumeLayout(false);
        }

        // ------------------------------------------------ sidebar auto width

        // Window ke size ke hisaab se sidebar 230..270 dp ke darmiyan khud adjust hoti hai
        private void AdjustSidebar()
        {
            if (pnlMenu == null || ClientSize.Width <= 0) return;
            float k = DeviceDpi / 96f;
            double dp = ClientSize.Width / k * 0.17;
            int w = (int)(Math.Max(230, Math.Min(270, dp)) * k);
            if (pnlMenu.Width != w) pnlMenu.Width = w;
        }

        // ------------------------------------------------ group open / close

        private readonly List<(Button Header, Panel Body, string Title)> _groups = new();

        private void ToggleGroup(Button header)
        {
            pnlMenu.SuspendLayout();
            foreach (var g in _groups)
            {
                var open = g.Header == header && !g.Body.Visible;
                g.Body.Visible = open;
                SetGroupText(g.Header, g.Title, open);
                g.Header.BackColor = open ? ClrHover : ClrSidebar;
            }
            pnlMenu.ResumeLayout(true);
        }

        // Text sirf title; open/close state chevron icon ke liye save hoti hai
        private static void SetGroupText(Button b, string title, bool open)
        {
            b.Text = title;
            b.AccessibleDescription = open ? "open" : "closed";
            b.Invalidate();
        }

        // Selected button highlight
        private void SetMenuActiveButton(Button b)
        {
            var old = _menuActiveButton;
            _menuActiveButton = b;
            if (old != null)
            {
                bool sub = IsMenuSubButton(old);
                old.BackColor = sub ? ClrSidebarDeep : ClrSidebar;
                old.ForeColor = sub ? ClrTextMuted : ClrTextLight;
                old.Invalidate();
            }
            b.BackColor = ClrActiveBg;
            b.ForeColor = System.Drawing.Color.White;
            b.Invalidate();
        }

        private bool IsMenuSubButton(Button b) => b.Parent is Panel p && p != pnlMenu;

        // ------------------------------------------------ drawing helpers

        private static System.Drawing.Drawing2D.GraphicsPath RoundedPath(System.Drawing.Rectangle r, int radius)
        {
            int d = Math.Max(2, radius * 2);
            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }

        private static void RoundCorners(Control c, int radius)
        {
            void Apply()
            {
                if (c.Width <= 0 || c.Height <= 0) return;
                int r = (int)(radius * c.DeviceDpi / 96f);
                using var gp = RoundedPath(new System.Drawing.Rectangle(0, 0, c.Width, c.Height), r);
                c.Region = new System.Drawing.Region(gp);
            }
            c.SizeChanged += (s, e) => Apply();
            Apply();
        }

        private static void DrawGlyph(System.Drawing.Graphics g, string glyph, float size,
            System.Drawing.Color color, System.Drawing.Rectangle bounds)
        {
            using var f = new System.Drawing.Font(IconFontName, size);
            TextRenderer.DrawText(g, glyph, f, bounds, color,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
        }

        // Icon (left) + active indicator + chevron (groups only). Sab positions DPI se scale hoti hain.
        private void AttachIcon(Button b, bool isGroup)
        {
            b.Paint += (s, e) =>
            {
                float k = b.DeviceDpi / 96f;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bool active = b == _menuActiveButton;
                bool sub = !isGroup && IsMenuSubButton(b);
                var glyph = b.Tag as string;

                if (active)
                {
                    using var br = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                    using var gp = RoundedPath(new System.Drawing.Rectangle(0, (int)(9 * k), (int)(5 * k), b.Height - (int)(18 * k)), 2);
                    e.Graphics.FillPath(br, gp);
                }
                if (!string.IsNullOrEmpty(glyph))
                {
                    var col = active ? System.Drawing.Color.White : (sub ? ClrTextMuted : ClrTextLight);
                    int x = (int)((sub ? 24 : 18) * k);
                    DrawGlyph(e.Graphics, glyph, sub ? 11F : 13F, col,
                        new System.Drawing.Rectangle(x, 0, (int)(28 * k), b.Height));
                }
                if (isGroup)
                {
                    bool open = b.AccessibleDescription == "open";
                    DrawGlyph(e.Graphics, open ? "\uE70E" : "\uE70D", 8F, ClrTextMuted,
                        new System.Drawing.Rectangle(b.Width - (int)(34 * k), 0, (int)(20 * k), b.Height));
                }
            };
        }

        // ------------------------------------------------ styling helpers

        // Group ka main button
        private void StyleGroupButton(Button b, string name, string text, int tabIndex, string icon)
        {
            b.AutoEllipsis = true;
            b.Dock = DockStyle.Top;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ClrHover;
            b.FlatAppearance.MouseDownBackColor = ClrActiveBg;
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            b.ForeColor = ClrTextLight;
            b.Name = name;
            b.Size = new Size(240, 50);
            b.TabIndex = tabIndex;
            b.Text = text;
            b.Tag = icon;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(52, 0, 32, 0);
            b.UseVisualStyleBackColor = false;
            b.BackColor = ClrSidebar;
            b.Cursor = Cursors.Hand;
            AttachIcon(b, true);
        }

        // Group ke andar ka panel (shuru mein band)
        private static void StyleSubPanel(Panel p, string name, int itemCount)
        {
            p.BackColor = ClrSidebarDeep;
            p.Dock = DockStyle.Top;
            p.Name = name;
            p.Size = new Size(240, itemCount * 42);
            p.Visible = false;
        }

        // Group ke andar ke chhote buttons
        private void StyleSubButton(Button b, string name, string text, int tabIndex, string icon)
        {
            b.AutoEllipsis = true;
            b.Dock = DockStyle.Top;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ClrHover;
            b.FlatAppearance.MouseDownBackColor = ClrActiveBg;
            b.Font = new System.Drawing.Font("Segoe UI", 10F);
            b.ForeColor = ClrTextMuted;
            b.Name = name;
            b.Size = new Size(240, 42);
            b.TabIndex = tabIndex;
            b.Text = text;
            b.Tag = icon;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(60, 0, 8, 0);
            b.UseVisualStyleBackColor = false;
            b.BackColor = ClrSidebarDeep;
            b.Cursor = Cursors.Hand;
            AttachIcon(b, false);
        }

        // Main (group ke bahar wale) buttons: Reports, Settings
        private void StyleMenuButton(Button b, string name, string text, int tabIndex, string icon)
        {
            b.AutoEllipsis = true;
            b.Dock = DockStyle.Top;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ClrHover;
            b.FlatAppearance.MouseDownBackColor = ClrActiveBg;
            b.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            b.ForeColor = ClrTextLight;
            b.Name = name;
            b.Size = new Size(240, 50);
            b.TabIndex = tabIndex;
            b.Text = text;
            b.Tag = icon;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(52, 0, 8, 0);
            b.UseVisualStyleBackColor = false;
            b.BackColor = ClrSidebar;
            b.Cursor = Cursors.Hand;
            AttachIcon(b, false);
        }

        // Dashboard card: icon circle upar, title neeche
        private static void StyleChip(Label l, string name, string text, string icon,
            System.Drawing.Color back, System.Drawing.Color accent)
        {
            l.AutoSize = false;
            l.BackColor = back;
            l.Font = new System.Drawing.Font("Segoe UI Semibold", 11.5F);
            l.ForeColor = ClrDarkText;
            l.Margin = new Padding(8);
            l.Name = name;
            l.Padding = new Padding(20, 0, 0, 18);
            l.Size = new Size(176, 120);
            l.Tag = icon;
            l.Text = text;
            l.TextAlign = ContentAlignment.BottomLeft;
            RoundCorners(l, 16);
            l.Paint += (s, e) =>
            {
                float k = l.DeviceDpi / 96f;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var circle = new System.Drawing.Rectangle((int)(20 * k), (int)(20 * k), (int)(46 * k), (int)(46 * k));
                using (var br = new System.Drawing.SolidBrush(accent))
                    e.Graphics.FillEllipse(br, circle);
                DrawGlyph(e.Graphics, icon, 15F, System.Drawing.Color.White, circle);
            };
        }

        #endregion

        private Panel pnlMenu;
        private Label lblTitle;
        private Button btnGrpManage;
        private Panel pnlSubManage;
        private Button btnGrpSales;
        private Panel pnlSubSales;
        private Button btnGrpMoney;
        private Panel pnlSubMoney;
        private Button btnGrpLedger;
        private Panel pnlSubLedger;
        private Button btnArticles;
        private Button btnColors;
        private Button btnStockIn;
        private Button btnPurchase;
        private Button btnStockView;
        private Button btnStockAdjust;
        private Button btnShopSale;
        private Button btnOnlineOrder;
        private Button btnAllSales;
        private Button btnReturns;
        private Button btnProfit;
        private Button btnReports;
        private Button btnExpenses;
        private Button btnWorkers;
        private Button btnExpSetup;
        private Button btnLedger;
        private Button btnCashBook;
        private Button btnParties;
        private Button btnCouriers;
        private Button btnSettings;
        private Panel pnlContent;
        private TableLayoutPanel tlpHero;
        private Panel pnlHeroCard;
        private Panel pnlAccentBar;
        private Label lblProjectName;
        private Label lblProjectTagline;
        private FlowLayoutPanel flpChips;
        private Label lblChip1;
        private Label lblChip2;
        private Label lblChip3;
        private Label lblChip4;
    }
}