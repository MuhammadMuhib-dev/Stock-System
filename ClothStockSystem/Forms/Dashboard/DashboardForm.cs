using ClothStock.Business.Services;
using ClothStock.Models.Expenses;
using ClothStock.Models.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Dashboard;

public class DashboardForm : Form
{
    private readonly OrderService _orders;
    private readonly StockService _stock;
    private readonly ExpenseService _exp;
    private readonly FinanceService _fin;

    private readonly Dictionary<string, (Label v, Label s)> _c = new();
    private readonly FlowLayoutPanel _cards = new() { Dock = DockStyle.Top, Height = 250, Padding = new Padding(10, 8, 10, 0), WrapContents = true };
    private readonly Label _stamp = new() { AutoSize = true, ForeColor = System.Drawing.Color.DimGray, Margin = new Padding(12, 14, 3, 0) };
    private readonly DataGridView _gLow = Grid();
    private readonly DataGridView _gDue = Grid();
    private readonly DataGridView _gCod = Grid();
    private readonly DataGridView _gPend = Grid();
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 60000 };
    private bool _busy;

    private static readonly System.Drawing.Color Teal = System.Drawing.Color.FromArgb(13, 148, 136);
    private static readonly System.Drawing.Color Blue = System.Drawing.Color.FromArgb(37, 99, 235);
    private static readonly System.Drawing.Color Amber = System.Drawing.Color.FromArgb(217, 119, 6);
    private static readonly System.Drawing.Color Red = System.Drawing.Color.FromArgb(220, 38, 38);
    private static readonly System.Drawing.Color Green = System.Drawing.Color.FromArgb(22, 163, 74);
    private static readonly System.Drawing.Color Purple = System.Drawing.Color.FromArgb(124, 58, 237);

    private static DataGridView Grid() => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = System.Drawing.Color.White,
        BorderStyle = BorderStyle.None
    };

    public DashboardForm()
    {
        _orders = Program.Services.GetRequiredService<OrderService>();
        _stock = Program.Services.GetRequiredService<StockService>();
        _exp = Program.Services.GetRequiredService<ExpenseService>();
        _fin = Program.Services.GetRequiredService<FinanceService>();

        Text = "Dashboard"; Font = new System.Drawing.Font("Segoe UI", 10.5F);
        Size = new Size(1300, 800); StartPosition = FormStartPosition.CenterScreen;
        BackColor = System.Drawing.Color.FromArgb(241, 245, 249);

        // ---- cards
        _cards.Controls.Add(Card("shop", "AAJ: SHOP SALE", Teal));
        _cards.Controls.Add(Card("online", "AAJ: ONLINE ORDERS", Blue));
        _cards.Controls.Add(Card("today", "AAJ KA PROFIT", Green));
        _cards.Controls.Add(Card("month", "IS MAHINE NET PROFIT", Green));
        _cards.Controls.Add(Card("money", "CASH + BANK", Purple));
        _cards.Controls.Add(Card("lena", "LENA (customers se)", Amber));
        _cards.Controls.Add(Card("dena", "DENA (suppliers ko)", Amber));
        _cards.Controls.Add(Card("alert", "ALERTS", Red));
        var refresh = new Button { Text = "Refresh", Width = 100, Height = 34, Margin = new Padding(12, 10, 3, 3) };
        refresh.Click += async (s, e) => await LoadAsync();
        _cards.Controls.Add(refresh); _cards.Controls.Add(_stamp);

        // ---- 4 tables
        var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Padding = new Padding(10, 0, 10, 10) };
        t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 50)); t.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        t.Controls.Add(Box("Low stock colors", _gLow), 0, 0);
        t.Controls.Add(Box("Payments: guzri ya 7 din ke andar (laal = guzri)", _gDue), 1, 0);
        t.Controls.Add(Box("COD jo courier se abhi nahi mila", _gCod), 0, 1);
        t.Controls.Add(Box("Pending online orders (Placed)", _gPend), 1, 1);

        Controls.Add(t); Controls.Add(_cards);

        _timer.Tick += async (s, e) => await LoadAsync();
        Load += async (s, e) => { await LoadAsync(); _timer.Start(); };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }

    private Panel Card(string key, string title, System.Drawing.Color accent)
    {
        var p = new Panel { Width = 295, Height = 110, Margin = new Padding(8), BackColor = System.Drawing.Color.White };
        var bar = new Panel { Dock = DockStyle.Left, Width = 6, BackColor = accent };
        var tl = new Label { Text = title, AutoSize = true, Location = new Point(18, 8), ForeColor = System.Drawing.Color.Gray, Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold) };
        var v = new Label { AutoSize = true, Location = new Point(18, 30), Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold), Text = "-" };
        var s = new Label { AutoSize = false, Location = new Point(18, 72), Size = new Size(270, 34), ForeColor = System.Drawing.Color.DimGray, Font = new System.Drawing.Font("Segoe UI", 8.5F) };
        p.Controls.Add(bar); p.Controls.Add(tl); p.Controls.Add(v); p.Controls.Add(s);
        _c[key] = (v, s);
        return p;
    }

    private static Panel Box(string title, DataGridView g)
    {
        var p = new Panel { Dock = DockStyle.Fill, Margin = new Padding(6), BackColor = System.Drawing.Color.White };
        p.Controls.Add(g);
        p.Controls.Add(new Label { Text = title, Dock = DockStyle.Top, Height = 30, Padding = new Padding(8, 6, 0, 0), Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold) });
        return p;
    }

    private void Set(string key, string value, string sub, System.Drawing.Color? color = null)
    {
        var (v, s) = _c[key];
        v.Text = value; s.Text = sub;
        if (color.HasValue) v.ForeColor = color.Value;
    }

    private static void Fmt(DataGridView g, params string[] money)
    {
        foreach (var c in money)
            if (g.Columns.Contains(c)) { g.Columns[c]!.DefaultCellStyle.Format = "N0"; g.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
        g.ClearSelection();
    }

    private async Task LoadAsync()
    {
        if (_busy) return;
        _busy = true;
        try
        {
            var today = DateTime.Today;
            var m1 = new DateTime(today.Year, today.Month, 1);

            var month = (await _orders.GetOrdersAsync(new OrderFilter { From = m1, To = today })).ToList();
            var pending = (await _orders.GetOrdersAsync(new OrderFilter { Channel = "Online", Status = "Placed" })).ToList();
            var exp = (await _exp.GetAsync(new ExpenseFilter { Month = m1 })).Where(x => x.Status == "Active").ToList();
            var colors = (await _stock.GetColorStockAsync()).ToList();
            var batches = (await _stock.GetAllBatchesAsync()).Where(b => b.MetersRemaining > 0).ToList();
            var accs = (await _fin.GetAccountBalancesAsync()).Where(a => a.IsActive).ToList();
            var due = (await _fin.GetDueAsync()).ToList();
            var cod = (await _fin.GetCodPendingAsync()).ToList();

            // ---- aaj
            var td = month.Where(o => o.OrderDate.Date == today && o.Status != "Cancelled").ToList();
            var shop = td.Where(o => o.Channel == "Shop").ToList();
            var onl = td.Where(o => o.Channel == "Online" && o.Status != "Returned").ToList();
            var tProfit = td.Sum(o => o.Profit);
            Set("shop", $"{shop.Sum(o => o.NetSale):N0}", $"{shop.Count} sales, {shop.Sum(o => o.Suits)} suits");
            Set("online", $"{onl.Sum(o => o.NetSale):N0}", $"{onl.Count} orders, {onl.Sum(o => o.Suits)} suits");
            Set("today", $"{tProfit:N0}", "kharchon se pehle", tProfit >= 0 ? Green : Red);

            // ---- is mahine
            var live = month.Where(o => o.Status != "Cancelled").ToList();
            var mProfit = live.Sum(o => o.Profit);
            var mExp = exp.Sum(x => x.ShopPart + x.OnlinePart);
            var net = mProfit - mExp;
            Set("month", $"{net:N0}", $"orders profit {mProfit:N0}  -  kharche {mExp:N0}", net >= 0 ? Green : Red);

            // ---- paisa
            Set("money", $"{accs.Sum(a => a.Balance):N0}", string.Join("  |  ", accs.Select(a => $"{a.AccountName} {a.Balance:N0}")));

            // ---- udhaar
            var lena = due.Where(d => d.Kind == "Lena").ToList();
            var dena = due.Where(d => d.Kind == "Dena").ToList();
            Set("lena", $"{lena.Sum(d => d.Due):N0}", $"guzra hua {lena.Where(d => d.DaysOverdue > 0).Sum(d => d.Due):N0}");
            Set("dena", $"{dena.Sum(d => d.Due):N0}", $"guzra hua {dena.Where(d => d.DaysOverdue > 0).Sum(d => d.Due):N0}");

            // ---- alerts
            var low = colors.Where(c => c.IsLowStock).ToList();
            var old = batches.Where(b => b.IsOldStock).ToList();
            var alerts = low.Count + old.Count + cod.Count(c => c.DaysWaiting > 7);
            Set("alert", alerts.ToString(), $"low {low.Count} | purana {old.Count} | pending {pending.Count} | COD baqi {cod.Count}", alerts > 0 ? Red : Green);

            // ---- tables
            _gLow.DataSource = low.OrderBy(c => c.SuitsRemaining)
                .Select(c => new { Article = c.ArticleName, Color = c.ColorName, Suits = c.SuitsRemaining }).ToList();
            Fmt(_gLow, "Suits");

            var soon = due.Where(d => d.DaysOverdue > 0 || (d.DueDate.Date - today).TotalDays <= 7)
                .OrderBy(d => d.DueDate)
                .Select(d => new { Type = d.Kind, Party = d.PartyName, Bill = d.Ref, DueDate = d.DueDate.ToString("dd-MMM-yyyy"), Baqi = d.Due, DinLate = d.DaysOverdue }).ToList();
            _gDue.DataSource = soon;
            Fmt(_gDue, "Baqi");
            foreach (DataGridViewRow r in _gDue.Rows)
                if (Convert.ToInt32(r.Cells["DinLate"].Value) > 0) r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 220, 220);

            _gCod.DataSource = cod.OrderByDescending(c => c.DaysWaiting)
                .Select(c => new { Order = c.OrderNo, Customer = c.CustomerName, Courier = c.CourierName, Milna = c.Expected, DinSe = c.DaysWaiting }).ToList();
            Fmt(_gCod, "Milna");

            _gPend.DataSource = pending.OrderBy(o => o.OrderDate)
                .Select(o => new { Order = o.OrderNo, Date = o.OrderDate.ToString("dd-MMM-yyyy"), Customer = o.CustomerName, Courier = o.CourierName, Tracking = o.TrackingId, Amount = o.NetSale }).ToList();
            Fmt(_gPend, "Amount");

            _stamp.Text = "Update: " + DateTime.Now.ToString("hh:mm:ss tt");
        }
        catch (Exception ex)
        {
            _timer.Stop();
            MessageBox.Show(ex.Message, "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { _busy = false; }
    }
}