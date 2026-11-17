using ClothStockSystem.Forms.Auth;
using ClothStockSystem.Forms.Expenses;
using ClothStockSystem.Forms.Finance;
using ClothStockSystem.Forms.Orders;
using ClothStockSystem.Forms.Reports;
using ClothStockSystem.Forms.Settings;
using ClothStockSystem.Forms.Stock;
using ClothStockSystem.Helpers;

namespace ClothStockSystem;

public partial class MainForm : Form
{
    private Form? _current;
    private Button? _activeButton;

    private static readonly System.Drawing.Color NormalBack = System.Drawing.Color.FromArgb(17, 24, 39);
    private static readonly System.Drawing.Color ActiveBack = System.Drawing.Color.FromArgb(13, 148, 136);
    private static readonly System.Drawing.Color CardBack = System.Drawing.Color.FromArgb(31, 41, 55);

    public MainForm()
    {
        InitializeComponent();
        ApplyPermissions();
        BuildUserPanel();
    }

    // Seller ko sirf Stock View, Online Order, Returns
    private void ApplyPermissions()
    {
        var owner = SessionManager.IsOwner;
        foreach (var b in new[]
        {
            btnArticles, btnColors, btnStockIn, btnPurchase, btnStockAdjust, btnShopSale, btnAllSales,
            btnProfit, btnReports, btnExpenses, btnWorkers, btnExpSetup,
            btnLedger, btnCashBook, btnParties, btnCouriers, btnSettings
        })
            b.Visible = owner;
    }

    // ---------------------------------------------------------------- user card

    private void BuildUserPanel()
    {
        var owner = SessionManager.IsOwner;
        var name = string.IsNullOrWhiteSpace(SessionManager.FullName) ? SessionManager.Username : SessionManager.FullName;

        var card = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 150,
            BackColor = CardBack,
            Padding = new Padding(14, 12, 14, 12)
        };

        // Avatar: naam ka pehla harf, gol
        var avatar = new Panel { Size = new Size(46, 46), Location = new Point(14, 12), BackColor = CardBack };
        var initial = name.Trim().Length > 0 ? name.Trim()[0].ToString().ToUpper() : "?";
        avatar.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var br = new SolidBrush(ActiveBack);
            e.Graphics.FillEllipse(br, 0, 0, 44, 44);
            using var f = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            var sz = e.Graphics.MeasureString(initial, f);
            e.Graphics.DrawString(initial, f, Brushes.White, (46 - sz.Width) / 2, (46 - sz.Height) / 2);
        };

        var lblName = new Label
        {
            Text = name,
            ForeColor = System.Drawing.Color.White,
            Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold),
            Location = new Point(68, 12),
            Size = new Size(150, 24),
            AutoEllipsis = true
        };

        var lblRole = new Label
        {
            Text = owner ? "OWNER" : "SELLER",
            ForeColor = owner ? System.Drawing.Color.FromArgb(204, 251, 241) : System.Drawing.Color.FromArgb(254, 243, 199),
            BackColor = owner ? System.Drawing.Color.FromArgb(15, 118, 110) : System.Drawing.Color.FromArgb(180, 83, 9),
            Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Location = new Point(68, 38),
            Size = new Size(64, 20)
        };

        Button Mk(string text, System.Drawing.Color back, System.Drawing.Color hover, Point loc, Size size, EventHandler h)
        {
            var b = new Button
            {
                Text = text,
                Location = loc,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                ForeColor = System.Drawing.Color.White,
                BackColor = back,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = hover;
            b.Click += h;
            return b;
        }

        var grey = System.Drawing.Color.FromArgb(55, 65, 81);
        var greyHover = System.Drawing.Color.FromArgb(75, 85, 99);
        var red = System.Drawing.Color.FromArgb(185, 28, 28);
        var redHover = System.Drawing.Color.FromArgb(220, 38, 38);

        if (owner)
        {
            card.Controls.Add(Mk("Users", grey, greyHover, new Point(14, 72), new Size(100, 30),
                (s, e) => { using var f = new UsersForm(); f.ShowDialog(this); }));
            card.Controls.Add(Mk("Password", grey, greyHover, new Point(120, 72), new Size(100, 30),
                (s, e) => { using var f = new ChangePasswordDialog(SessionManager.UserId, forced: false); f.ShowDialog(this); }));
        }
        else
        {
            card.Controls.Add(Mk("Password", grey, greyHover, new Point(14, 72), new Size(206, 30),
                (s, e) => { using var f = new ChangePasswordDialog(SessionManager.UserId, forced: false); f.ShowDialog(this); }));
        }

        card.Controls.Add(Mk("Logout", red, redHover, new Point(14, 108), new Size(206, 32), (s, e) =>
        {
            if (MessageBox.Show("Logout karna hai?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            SessionManager.LogoutRequested = true;
            Close();
        }));

        card.Controls.Add(avatar);
        card.Controls.Add(lblName);
        card.Controls.Add(lblRole);

        var line = new Panel { Dock = DockStyle.Bottom, Height = 1, BackColor = System.Drawing.Color.FromArgb(55, 65, 81) };
        pnlMenu.Controls.Add(line);
        pnlMenu.Controls.Add(card);
        card.BringToFront();
    }

    // ---------------------------------------------------------------- forms

    private void OpenForm(Form form, Button button)
    {
        if (_current != null)
        {
            pnlContent.Controls.Remove(_current);
            _current.Dispose();
        }

        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;
        pnlContent.Controls.Add(form);
        form.Show();
        _current = form;

        if (_activeButton != null) _activeButton.BackColor = NormalBack;
        button.BackColor = ActiveBack;
        _activeButton = button;
    }

    // Online Order form ka Return button yahi bulata hai
    public void OpenReturns() => OpenForm(new OrderReturnForm(), btnReturns);

    private void btnArticles_Click(object? sender, EventArgs e) => OpenForm(new ArticleForm(), btnArticles);
    private void btnColors_Click(object? sender, EventArgs e) => OpenForm(new ColorForm(), btnColors);
    private void btnStockIn_Click(object? sender, EventArgs e) => OpenForm(new StockInForm(), btnStockIn);
    private void btnPurchase_Click(object? sender, EventArgs e) => OpenForm(new PurchaseBillForm(), btnPurchase);
    private void btnPurchaseList_Click(object? sender, EventArgs e) => OpenForm(new PurchaseHistoryForm(), btnPurchaseList);
    private void btnStockView_Click(object? sender, EventArgs e) => OpenForm(new StockViewForm(), btnStockView);
    private void btnStockAdjust_Click(object? sender, EventArgs e) => OpenForm(new StockAdjustForm(), btnStockAdjust);
    private void btnShopSale_Click(object? sender, EventArgs e) => OpenForm(new ShopSaleForm(), btnShopSale);
    private void btnOnlineOrder_Click(object? sender, EventArgs e) => OpenForm(new OnlineOrderForm(), btnOnlineOrder);
    private void btnAllSales_Click(object? sender, EventArgs e) => OpenForm(new AllSalesForm(), btnAllSales);
    private void btnReturns_Click(object? sender, EventArgs e) => OpenForm(new OrderReturnForm(), btnReturns);
    private void btnProfit_Click(object? sender, EventArgs e) => OpenForm(new ProfitSummaryForm(), btnProfit);
    private void btnReports_Click(object? sender, EventArgs e) => OpenForm(new ReportsForm(), btnReports);
    private void btnExpenses_Click(object? sender, EventArgs e) => OpenForm(new ExpenseForm(), btnExpenses);
    private void btnWorkers_Click(object? sender, EventArgs e) => OpenForm(new WorkersForm(), btnWorkers);
    private void btnExpSetup_Click(object? sender, EventArgs e) => OpenForm(new ExpenseSetupForm(), btnExpSetup);
    private void btnLedger_Click(object? sender, EventArgs e) => OpenForm(new LedgerForm(), btnLedger);
    private void btnCashBook_Click(object? sender, EventArgs e) => OpenForm(new CashBookForm(), btnCashBook);
    private void btnParties_Click(object? sender, EventArgs e) => OpenForm(new PartiesForm(), btnParties);
    private void btnCouriers_Click(object? sender, EventArgs e) => OpenForm(new CourierForm(), btnCouriers);
    private void btnSettings_Click(object? sender, EventArgs e) => OpenForm(new SettingsForm(), btnSettings);
}