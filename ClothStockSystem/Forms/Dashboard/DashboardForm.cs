using System.Drawing.Drawing2D;
using ClothStock.Business.Services;
using ClothStock.Models.Expenses;
using ClothStock.Models.Orders;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;
using C = System.Drawing.Color;
using F = System.Drawing.Font;

namespace ClothStockSystem.Forms.Dashboard;

// ------------------------------------------------------------------ theme + scaling
internal static class UI
{
    // Windows display scaling (100% = 1.0, 125% = 1.25, 150% = 1.5)
    public static readonly float K = GetScale();
    private static float GetScale()
    {
        using var g = Graphics.FromHwnd(IntPtr.Zero);
        return g.DpiX / 96f;
    }
    public static int S(int px) => (int)Math.Round(px * K);

    public static readonly C Page = C.FromArgb(241, 245, 249);
    public static readonly C Navy = C.FromArgb(17, 24, 39);
    public static readonly C Teal = C.FromArgb(13, 148, 136);
    public static readonly C TealDark = C.FromArgb(15, 118, 110);
    public static readonly C TealSoft = C.FromArgb(230, 247, 245);
    public static readonly C Border = C.FromArgb(226, 232, 240);
    public static readonly C Ink = C.FromArgb(15, 23, 42);
    public static readonly C Muted = C.FromArgb(100, 116, 139);
    public static readonly C Green = C.FromArgb(22, 163, 74);
    public static readonly C Blue = C.FromArgb(37, 99, 235);
    public static readonly C Purple = C.FromArgb(124, 58, 237);
    public static readonly C Amber = C.FromArgb(217, 119, 6);
    public static readonly C Orange = C.FromArgb(234, 88, 12);
    public static readonly C Red = C.FromArgb(220, 38, 38);
    public static readonly C Indigo = C.FromArgb(79, 70, 229);

    public static readonly F Caption = new("Segoe UI Semibold", 8.5F);
    public static readonly F Value = new("Segoe UI Semibold", 20F);
    public static readonly F Pill = new("Segoe UI Semibold", 8.5F);
    public static readonly F Body = new("Segoe UI", 9.5F);
    public static readonly F Heading = new("Segoe UI Semibold", 12F);
    public static readonly F Hello = new("Segoe UI Semibold", 20F);
    public static readonly F Light = new("Segoe UI", 10.5F);

    // Rang ko safed ki taraf halka karna (pill ke background ke liye)
    public static C Mix(C c, double strength) => C.FromArgb(
        (int)(255 - (255 - c.R) * strength),
        (int)(255 - (255 - c.G) * strength),
        (int)(255 - (255 - c.B) * strength));

    public static string Compact(decimal v)
    {
        var a = Math.Abs(v);
        if (a >= 1_000_000) return (v / 1_000_000m).ToString("0.#") + "M";
        if (a >= 1_000) return (v / 1_000m).ToString("0.#") + "k";
        return v.ToString("0");
    }

    public static decimal NiceStep(decimal x)
    {
        if (x <= 0) return 1;
        var pow = (decimal)Math.Pow(10, Math.Floor(Math.Log10((double)x)));
        var n = x / pow;
        var nice = n <= 1 ? 1 : n <= 2 ? 2 : n <= 5 ? 5 : 10;
        return nice * pow;
    }
}

// ------------------------------------------------------------------ upar ka banner
internal class HeroBanner : TableLayoutPanel
{
    private readonly Label _greet = new();
    private readonly Label _date = new();
    private readonly Label _pill = new();
    private readonly Label _stamp = new();
    private readonly Button _btn = new();
    public event EventHandler? RefreshClicked;

    public HeroBanner()
    {
        DoubleBuffered = true;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        ColumnCount = 2;
        RowCount = 1;
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Padding = new Padding(UI.S(28), UI.S(22), UI.S(28), UI.S(22));

        var left = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = C.Transparent,
            Margin = new Padding(0),
            Anchor = AnchorStyles.Left
        };
        _greet.AutoSize = true; _greet.Font = UI.Hello; _greet.ForeColor = C.White;
        _greet.BackColor = C.Transparent; _greet.Margin = new Padding(0);
        _date.AutoSize = true; _date.Font = UI.Light; _date.ForeColor = C.FromArgb(190, 214, 220);
        _date.BackColor = C.Transparent; _date.Margin = new Padding(0, UI.S(2), 0, 0);
        _pill.AutoSize = true; _pill.Font = UI.Pill; _pill.ForeColor = C.White;
        _pill.BackColor = C.FromArgb(31, 92, 100); _pill.Padding = new Padding(UI.S(12), UI.S(4), UI.S(12), UI.S(4));
        _pill.Margin = new Padding(0, UI.S(12), 0, 0);
        left.Controls.Add(_greet); left.Controls.Add(_date); left.Controls.Add(_pill);

        var right = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            BackColor = C.Transparent,
            Margin = new Padding(0),
            Anchor = AnchorStyles.Right
        };
        _btn.Text = "Refresh"; _btn.Width = UI.S(120); _btn.Height = UI.S(40);
        _btn.FlatStyle = FlatStyle.Flat; _btn.FlatAppearance.BorderSize = 0; _btn.Cursor = Cursors.Hand;
        _btn.BackColor = C.White; _btn.ForeColor = UI.TealDark; _btn.Font = new F("Segoe UI Semibold", 9.5F);
        _btn.Margin = new Padding(0);
        _btn.Click += (s, e) => RefreshClicked?.Invoke(this, e);
        _stamp.AutoSize = true; _stamp.Font = UI.Caption; _stamp.ForeColor = C.FromArgb(190, 214, 220);
        _stamp.BackColor = C.Transparent; _stamp.Margin = new Padding(0, UI.S(8), 0, 0);
        right.Controls.Add(_btn); right.Controls.Add(_stamp);

        Controls.Add(left, 0, 0);
        Controls.Add(right, 1, 0);
    }

    public void SetText(string greet, string date, string summary, string stamp)
    {
        _greet.Text = greet; _date.Text = date; _pill.Text = summary; _stamp.Text = stamp;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var rc = ClientRectangle;
        if (rc.Width < 2 || rc.Height < 2) return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (var br = new LinearGradientBrush(rc, UI.Navy, UI.TealDark, LinearGradientMode.Horizontal))
            g.FillRectangle(br, rc);
        using var b = new SolidBrush(C.FromArgb(18, 255, 255, 255));
        g.FillEllipse(b, rc.Right - UI.S(240), -UI.S(100), UI.S(300), UI.S(300));
        g.FillEllipse(b, rc.Right - UI.S(380), UI.S(40), UI.S(200), UI.S(200));
    }
}

// ------------------------------------------------------------------ KPI card
internal class KpiCard : TableLayoutPanel
{
    private readonly Label _t = new();
    private readonly Label _v = new();
    private readonly Label _p = new();
    private readonly C _accent;

    public KpiCard(string title, C accent)
    {
        _accent = accent;
        DoubleBuffered = true;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        BackColor = C.White;
        ColumnCount = 1;
        RowCount = 4;
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (var i = 0; i < 4; i++) RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var bar = new Panel { Dock = DockStyle.Fill, Height = UI.S(5), BackColor = accent, Margin = new Padding(0) };

        _t.Text = title; _t.AutoSize = true; _t.Font = UI.Caption; _t.ForeColor = UI.Muted;
        _t.Margin = new Padding(UI.S(18), UI.S(14), UI.S(12), 0);

        _v.Text = "-"; _v.AutoSize = true; _v.Font = UI.Value; _v.ForeColor = UI.Ink;
        _v.Margin = new Padding(UI.S(18), UI.S(4), UI.S(12), 0);

        _p.AutoSize = true; _p.Font = UI.Pill; _p.Padding = new Padding(UI.S(10), UI.S(3), UI.S(10), UI.S(3));
        _p.MaximumSize = new Size(UI.S(300), 0);
        _p.Margin = new Padding(UI.S(18), UI.S(10), UI.S(12), UI.S(16));
        _p.Visible = false;

        Controls.Add(bar, 0, 0);
        Controls.Add(_t, 0, 1);
        Controls.Add(_v, 0, 2);
        Controls.Add(_p, 0, 3);
    }

    public void Set(string value, string sub, C? tone = null)
    {
        var t = tone ?? _accent;
        _v.Text = value;
        _v.ForeColor = tone.HasValue && (tone.Value == UI.Red || tone.Value == UI.Green) ? tone.Value : UI.Ink;
        _p.Text = sub;
        _p.ForeColor = t;
        _p.BackColor = UI.Mix(t, 0.14);
        _p.Visible = !string.IsNullOrEmpty(sub);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(UI.Border);
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}

// ------------------------------------------------------------------ section card (title, badge, table ya chart)
internal class SectionCard : TableLayoutPanel
{
    private readonly Label _badge = new();
    private readonly Label _empty = new();
    private readonly Control _main;

    public SectionCard(string title, Control main, string emptyText)
    {
        _main = main;
        DoubleBuffered = true;
        BackColor = C.White;
        ColumnCount = 1;
        RowCount = 3;
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        RowStyles.Add(new RowStyle(SizeType.AutoSize));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 1));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var head = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            RowCount = 1,
            Margin = new Padding(0),
            BackColor = C.White
        };
        head.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        head.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        head.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var t = new Label
        {
            Text = title,
            AutoSize = true,
            Font = UI.Heading,
            ForeColor = UI.Ink,
            Margin = new Padding(UI.S(18), UI.S(14), 0, UI.S(12)),
            Anchor = AnchorStyles.Left
        };
        _badge.AutoSize = true; _badge.Font = UI.Pill; _badge.ForeColor = UI.TealDark; _badge.BackColor = UI.TealSoft;
        _badge.Padding = new Padding(UI.S(10), UI.S(3), UI.S(10), UI.S(3));
        _badge.Margin = new Padding(UI.S(8), UI.S(14), UI.S(18), UI.S(12)); _badge.Anchor = AnchorStyles.Right;
        head.Controls.Add(t, 0, 0);
        head.Controls.Add(_badge, 1, 0);

        var line = new Panel { Dock = DockStyle.Fill, BackColor = UI.Border, Margin = new Padding(0) };

        var host = new Panel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0),
            BackColor = C.White,
            Padding = new Padding(UI.S(14), UI.S(8), UI.S(14), UI.S(14))
        };
        _empty.Text = emptyText; _empty.Dock = DockStyle.Fill; _empty.AutoSize = false;
        _empty.TextAlign = ContentAlignment.MiddleCenter; _empty.ForeColor = UI.Muted; _empty.Font = UI.Body;
        _empty.Visible = false;
        main.Dock = DockStyle.Fill;
        host.Controls.Add(main);
        host.Controls.Add(_empty);

        Controls.Add(head, 0, 0);
        Controls.Add(line, 0, 1);
        Controls.Add(host, 0, 2);
    }

    public void SetBadge(string text) => _badge.Text = text;

    public void SetEmpty(bool empty)
    {
        _empty.Visible = empty;
        _main.Visible = !empty;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(UI.Border);
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}

// ------------------------------------------------------------------ 7 din ka chart (margins font se naap kar)
internal class SalesChart : Control
{
    private List<(string Label, decimal Shop, decimal Online)> _data = new();

    public SalesChart()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = C.White;
    }

    public void SetData(List<(string, decimal, decimal)> data)
    {
        _data = data.Select(d => (d.Item1, d.Item2, d.Item3)).ToList();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var fh = TextRenderer.MeasureText("Ag", UI.Body).Height;
        var max = _data.Count == 0 ? 0 : _data.Max(d => d.Shop + d.Online);
        var basis = max <= 0 ? 4000 : max;
        var step = UI.NiceStep(basis / 4m);
        var top = Math.Ceiling(basis / step) * step;
        if (top <= 0) top = step * 4;

        var lblW = TextRenderer.MeasureText(UI.Compact(top), UI.Body).Width;
        int L = lblW + UI.S(14), R = UI.S(8), T = fh * 2 + UI.S(10), B = fh + UI.S(14);
        var plot = new Rectangle(L, T, Width - L - R, Height - T - B);
        if (plot.Width < 60 || plot.Height < 40) return;

        // legend (sab se upar ki line, bars ke labels se alag)
        var dot = UI.S(10);
        using (var b = new SolidBrush(UI.Teal)) g.FillEllipse(b, 2, (fh - dot) / 2, dot, dot);
        TextRenderer.DrawText(g, "Shop", UI.Body, new Point(dot + 6, 0), UI.Muted);
        var shopW = TextRenderer.MeasureText("Shop", UI.Body).Width;
        var x2 = dot + 6 + shopW + UI.S(18);
        using (var b = new SolidBrush(UI.Blue)) g.FillEllipse(b, x2, (fh - dot) / 2, dot, dot);
        TextRenderer.DrawText(g, "Online", UI.Body, new Point(x2 + dot + 6, 0), UI.Muted);

        using (var pen = new Pen(C.FromArgb(236, 240, 245)))
        {
            for (decimal v = 0; v <= top; v += step)
            {
                var y = plot.Bottom - (float)(v / top) * plot.Height;
                g.DrawLine(pen, plot.Left, y, plot.Right, y);
                TextRenderer.DrawText(g, UI.Compact(v), UI.Body, new Rectangle(0, (int)y - fh / 2, L - UI.S(8), fh), UI.Muted,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }

        var n = _data.Count;
        if (n == 0) return;
        var slot = plot.Width / (float)n;
        var barW = Math.Min(UI.S(56), slot * 0.5f);

        for (var i = 0; i < n; i++)
        {
            var d = _data[i];
            var x = plot.Left + slot * i + (slot - barW) / 2f;
            var hShop = (float)(d.Shop / top) * plot.Height;
            var hOn = (float)(d.Online / top) * plot.Height;
            var isToday = i == n - 1;

            if (hShop > 0.5f)
                using (var b = new SolidBrush(UI.Teal)) g.FillRectangle(b, x, plot.Bottom - hShop, barW, hShop);
            if (hOn > 0.5f)
                using (var b = new SolidBrush(UI.Blue)) g.FillRectangle(b, x, plot.Bottom - hShop - hOn, barW, hOn);

            var total = d.Shop + d.Online;
            if (total > 0)
                TextRenderer.DrawText(g, UI.Compact(total), UI.Caption,
                    new Rectangle((int)(x - UI.S(16)), (int)(plot.Bottom - hShop - hOn - fh - 2), (int)barW + UI.S(32), fh), UI.Ink,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);

            TextRenderer.DrawText(g, d.Label, isToday ? UI.Caption : UI.Body,
                new Rectangle((int)(plot.Left + slot * i), plot.Bottom + UI.S(6), (int)slot, fh),
                isToday ? UI.TealDark : UI.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
        }
    }
}

// ------------------------------------------------------------------ form
public class DashboardForm : Form
{
    private readonly OrderService _orders;
    private readonly StockService _stock;
    private readonly ExpenseService _exp;
    private readonly FinanceService _fin;

    private readonly HeroBanner _hero = new();
    private readonly KpiCard _kSales = new("TODAY'S SALES", UI.Teal);
    private readonly KpiCard _kToday = new("TODAY'S PROFIT", UI.Green);
    private readonly KpiCard _kMonth = new("NET PROFIT THIS MONTH", UI.Indigo);
    private readonly KpiCard _kMoney = new("CASH + BANK", UI.Purple);
    private readonly KpiCard _kLena = new("TO RECEIVE", UI.Amber);
    private readonly KpiCard _kDena = new("TO PAY SUPPLIERS", UI.Orange);
    private readonly KpiCard _kPend = new("PENDING ONLINE ORDERS", UI.Blue);
    private readonly KpiCard _kAlert = new("ALERTS", UI.Red);

    private readonly SalesChart _chart = new();
    private readonly DataGridView _gDue = NewGrid();
    private readonly DataGridView _gLow = NewGrid();
    private readonly DataGridView _gCod = NewGrid();
    private readonly DataGridView _gPend = NewGrid();
    private readonly SectionCard _cChart;
    private readonly SectionCard _cDue;
    private readonly SectionCard _cLow;
    private readonly SectionCard _cCod;
    private readonly SectionCard _cPend;

    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 60000 };
    private bool _busy;

    private static DataGridView NewGrid()
    {
        var g = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            RowHeadersVisible = false,
            MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = C.White,
            BorderStyle = BorderStyle.None,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            EnableHeadersVisualStyles = false,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = C.FromArgb(238, 242, 247),
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersHeight = UI.S(36),
            ScrollBars = ScrollBars.Vertical
        };
        g.ColumnHeadersDefaultCellStyle.BackColor = UI.TealSoft;
        g.ColumnHeadersDefaultCellStyle.ForeColor = UI.TealDark;
        g.ColumnHeadersDefaultCellStyle.Font = new F("Segoe UI Semibold", 8.5F);
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = UI.TealSoft;
        g.ColumnHeadersDefaultCellStyle.SelectionForeColor = UI.TealDark;
        g.ColumnHeadersDefaultCellStyle.Padding = new Padding(UI.S(8), 0, 0, 0);
        g.DefaultCellStyle.Font = UI.Body;
        g.DefaultCellStyle.ForeColor = UI.Ink;
        g.DefaultCellStyle.Padding = new Padding(UI.S(8), 0, 0, 0);
        g.DefaultCellStyle.SelectionBackColor = C.FromArgb(240, 253, 250);
        g.DefaultCellStyle.SelectionForeColor = UI.Ink;
        g.RowTemplate.Height = UI.S(36);
        return g;
    }

    public DashboardForm()
    {
        _orders = Program.Services.GetRequiredService<OrderService>();
        _stock = Program.Services.GetRequiredService<StockService>();
        _exp = Program.Services.GetRequiredService<ExpenseService>();
        _fin = Program.Services.GetRequiredService<FinanceService>();

        _cChart = new SectionCard("Sales, last 7 days", _chart, "No sales yet");
        _cDue = new SectionCard("Payments due", _gDue, "No payments due this week");
        _cLow = new SectionCard("Low stock", _gLow, "All colors have enough stock");
        _cPend = new SectionCard("Pending online orders", _gPend, "No pending orders");
        _cCod = new SectionCard("COD not received yet", _gCod, "No COD pending");

        Text = "Dashboard";
        Font = new F("Segoe UI", 10F);
        BackColor = UI.Page;
        Size = new Size(1300, 820);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScroll = true;

        var body = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 1,
            RowCount = 6,
            BackColor = UI.Page,
            Padding = new Padding(UI.S(26), UI.S(22), UI.S(26), UI.S(26)),
            MinimumSize = new Size(UI.S(980), 0)
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        body.RowStyles.Add(new RowStyle(SizeType.AutoSize));                  // banner
        body.RowStyles.Add(new RowStyle(SizeType.AutoSize));                  // KPI row 1
        body.RowStyles.Add(new RowStyle(SizeType.AutoSize));                  // KPI row 2
        body.RowStyles.Add(new RowStyle(SizeType.Absolute, UI.S(360)));       // chart + due
        body.RowStyles.Add(new RowStyle(SizeType.Absolute, UI.S(340)));       // low stock + pending
        body.RowStyles.Add(new RowStyle(SizeType.Absolute, UI.S(320)));       // COD

        _hero.Dock = DockStyle.Fill;
        _hero.Margin = new Padding(0, 0, 0, UI.S(18));
        body.Controls.Add(_hero, 0, 0);
        body.Controls.Add(Row(true, new[] { 25f, 25f, 25f, 25f }, _kSales, _kToday, _kMonth, _kMoney), 0, 1);
        body.Controls.Add(Row(true, new[] { 25f, 25f, 25f, 25f }, _kLena, _kDena, _kPend, _kAlert), 0, 2);
        body.Controls.Add(Row(false, new[] { 60f, 40f }, _cChart, _cDue), 0, 3);
        body.Controls.Add(Row(false, new[] { 40f, 60f }, _cLow, _cPend), 0, 4);
        body.Controls.Add(Row(false, new[] { 100f }, _cCod), 0, 5);
        Controls.Add(body);

        _hero.RefreshClicked += async (s, e) => await LoadAsync();
        _timer.Tick += async (s, e) => await LoadAsync();
        Load += async (s, e) => { await LoadAsync(); _timer.Start(); };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }

    private static TableLayoutPanel Row(bool auto, float[] widths, params Control[] items)
    {
        var t = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = items.Length,
            RowCount = 1,
            BackColor = UI.Page,
            Margin = new Padding(0, 0, 0, UI.S(18)),
            AutoSize = auto,
            AutoSizeMode = AutoSizeMode.GrowAndShrink
        };
        for (var i = 0; i < items.Length; i++) t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, widths[i]));
        t.RowStyles.Add(auto ? new RowStyle(SizeType.AutoSize) : new RowStyle(SizeType.Percent, 100));
        for (var i = 0; i < items.Length; i++)
        {
            items[i].Dock = DockStyle.Fill;
            items[i].Margin = new Padding(0, 0, i < items.Length - 1 ? UI.S(16) : 0, 0);
            t.Controls.Add(items[i], i, 0);
        }
        return t;
    }

    // ---------------------------------------------------------------- data
    private async Task LoadAsync()
    {
        if (_busy) return;
        _busy = true;
        try
        {
            var today = DateTime.Today;
            var m1 = new DateTime(today.Year, today.Month, 1);
            var from = m1 < today.AddDays(-6) ? m1 : today.AddDays(-6);

            var all = (await _orders.GetOrdersAsync(new OrderFilter { From = from, To = today })).ToList();
            var pending = (await _orders.GetOrdersAsync(new OrderFilter { Channel = "Online", Status = "Placed" })).ToList();
            var exp = (await _exp.GetAsync(new ExpenseFilter { Month = m1 })).Where(x => x.Status == "Active").ToList();
            var colors = (await _stock.GetColorStockAsync()).ToList();
            var batches = (await _stock.GetAllBatchesAsync()).Where(b => b.MetersRemaining > 0).ToList();
            var accs = (await _fin.GetAccountBalancesAsync()).Where(a => a.IsActive).ToList();
            var due = (await _fin.GetDueAsync()).ToList();
            var cod = (await _fin.GetCodPendingAsync()).ToList();

            var month = all.Where(o => o.OrderDate.Date >= m1).ToList();

            // ---- aaj
            var td = all.Where(o => o.OrderDate.Date == today && o.Status != "Cancelled").ToList();
            var shop = td.Where(o => o.Channel == "Shop").ToList();
            var onl = td.Where(o => o.Channel == "Online" && o.Status != "Returned").ToList();
            var tProfit = td.Sum(o => o.Profit);
            var tSale = shop.Sum(o => o.NetSale) + onl.Sum(o => o.NetSale);
            _kSales.Set($"{tSale:N0}", $"Shop {UI.Compact(shop.Sum(o => o.NetSale))}  |  Online {UI.Compact(onl.Sum(o => o.NetSale))}");
            _kToday.Set($"{tProfit:N0}", $"{shop.Count + onl.Count} orders, before expenses", tProfit >= 0 ? UI.Green : UI.Red);

            // ---- mahina
            var mProfit = month.Where(o => o.Status != "Cancelled").Sum(o => o.Profit);
            var mExp = exp.Sum(x => x.ShopPart + x.OnlinePart);
            var net = mProfit - mExp;
            _kMonth.Set($"{net:N0}", $"Profit {UI.Compact(mProfit)}  -  Expenses {UI.Compact(mExp)}", net >= 0 ? UI.Green : UI.Red);

            // ---- paisa
            _kMoney.Set($"{accs.Sum(a => a.Balance):N0}", string.Join("  |  ", accs.Select(a => $"{a.AccountName} {UI.Compact(a.Balance)}")));

            // ---- udhaar
            var lena = due.Where(d => d.Kind == "Lena").ToList();
            var dena = due.Where(d => d.Kind == "Dena").ToList();
            var lenaLate = lena.Where(d => d.DaysOverdue > 0).Sum(d => d.Due);
            var denaLate = dena.Where(d => d.DaysOverdue > 0).Sum(d => d.Due);
            _kLena.Set($"{lena.Sum(d => d.Due):N0}", lenaLate > 0 ? $"Overdue {UI.Compact(lenaLate)}" : "Nothing overdue", lenaLate > 0 ? UI.Red : UI.Green);
            _kDena.Set($"{dena.Sum(d => d.Due):N0}", denaLate > 0 ? $"Overdue {UI.Compact(denaLate)}" : "Nothing overdue", denaLate > 0 ? UI.Red : UI.Green);

            // ---- pending aur alerts
            _kPend.Set(pending.Count.ToString("N0"), $"Worth {UI.Compact(pending.Sum(o => o.NetSale))}");
            var low = colors.Where(c => c.IsLowStock).ToList();
            var old = batches.Where(b => b.IsOldStock).ToList();
            var lateCod = cod.Count(c => c.DaysWaiting > 7);
            var alerts = low.Count + old.Count + lateCod;
            _kAlert.Set(alerts.ToString("N0"), alerts > 0 ? $"Low {low.Count}  |  Old {old.Count}  |  COD {lateCod}" : "All clear", alerts > 0 ? UI.Red : UI.Green);

            // ---- banner
            var name = string.IsNullOrWhiteSpace(SessionManager.FullName) ? SessionManager.Username : SessionManager.FullName;
            var hr = DateTime.Now.Hour;
            _hero.SetText($"{(hr < 12 ? "Good morning" : hr < 17 ? "Good afternoon" : "Good evening")}, {name}",
                DateTime.Now.ToString("dddd, d MMMM yyyy"),
                $"Today: {shop.Count + onl.Count} orders   |   {alerts} alerts   |   {pending.Count} pending",
                "Updated " + DateTime.Now.ToString("hh:mm tt"));

            // ---- chart
            var chart = new List<(string, decimal, decimal)>();
            for (var i = 6; i >= 0; i--)
            {
                var d = today.AddDays(-i);
                var day = all.Where(o => o.OrderDate.Date == d && o.Status != "Cancelled" && o.Status != "Returned").ToList();
                chart.Add((d.ToString("ddd d"),
                    day.Where(o => o.Channel == "Shop").Sum(o => o.NetSale),
                    day.Where(o => o.Channel == "Online").Sum(o => o.NetSale)));
            }
            _chart.SetData(chart);
            _cChart.SetBadge($"7 days: {chart.Sum(c => c.Item2 + c.Item3):N0}");

            // ---- tables
            Fill(_cLow, _gLow, low.OrderBy(c => c.SuitsRemaining)
                .Select(c => new { Article = c.ArticleName, Color = c.ColorName, Suits = c.SuitsRemaining }).ToList(), "Suits");

            Fill(_cDue, _gDue, due.Where(d => d.DaysOverdue > 0 || (d.DueDate.Date - today).TotalDays <= 7)
                .OrderBy(d => d.DueDate)
                .Select(d => new {
                    Type = d.Kind == "Lena" ? "Receive" : "Pay",
                    Party = d.PartyName,
                    DueDate = d.DueDate.ToString("dd MMM"),
                    Balance = d.Due,
                    Late = d.DaysOverdue
                }).ToList(),
                "Balance");
            foreach (DataGridViewRow r in _gDue.Rows)
                if (_gDue.Columns.Contains("Late") && Convert.ToInt32(r.Cells["Late"].Value) > 0)
                {
                    r.DefaultCellStyle.BackColor = C.FromArgb(254, 242, 242);
                    r.DefaultCellStyle.ForeColor = C.FromArgb(153, 27, 27);
                }

            Fill(_cCod, _gCod, cod.OrderByDescending(c => c.DaysWaiting)
                .Select(c => new {
                    Order = c.OrderNo,
                    Customer = c.CustomerName,
                    Courier = c.CourierName,
                    Expected = c.Expected,
                    Days = c.DaysWaiting
                }).ToList(), "Expected");

            Fill(_cPend, _gPend, pending.OrderBy(o => o.OrderDate)
                .Select(o => new {
                    Order = o.OrderNo,
                    Date = o.OrderDate.ToString("dd MMM"),
                    Customer = o.CustomerName,
                    Tracking = o.TrackingId,
                    Amount = o.NetSale
                }).ToList(), "Amount");
        }
        catch (Exception ex)
        {
            _timer.Stop();
            MessageBox.Show(ex.Message, "Dashboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { _busy = false; }
    }

    private static void Fill<T>(SectionCard card, DataGridView g, List<T> rows, params string[] money)
    {
        card.SetBadge(rows.Count.ToString("N0"));
        g.DataSource = rows;
        card.SetEmpty(rows.Count == 0);
        foreach (DataGridViewColumn c in g.Columns) c.SortMode = DataGridViewColumnSortMode.NotSortable;
        foreach (var m in money)
            if (g.Columns.Contains(m))
            {
                g.Columns[m]!.DefaultCellStyle.Format = "N0";
                g.Columns[m]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                g.Columns[m]!.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        g.ClearSelection();
    }
}