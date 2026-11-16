using System.Data;
using System.Globalization;
using System.Text;
using ClothStock.Business.Services;
using ClothStock.Models.Expenses;
using ClothStock.Models.Orders;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Reports;

public partial class ReportsForm : Form
{
    private readonly OrderService _orders;
    private readonly StockService _stock;
    private readonly ExpenseService _expenses;

    private readonly ComboBox _cmbRange = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
    private readonly DateTimePicker _from = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 140 };
    private readonly DateTimePicker _to = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 140 };
    private readonly ComboBox _cmbGroup = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly Button _btnRefresh = new() { Text = "Refresh", Width = 100, Height = 34 };
    private readonly Button _btnExport = new() { Text = "Export (Excel)", Width = 140, Height = 34 };
    private readonly Button _btnPrint = new() { Text = "Print...", Width = 110, Height = 34 };
    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill };

    private class Rep
    {
        public TabPage Page = null!;
        public List<DataGridView> Grids = new();
        public List<string> Titles = new();
        public Label Summary = null!;
        public string SummaryTitle = "";                                              // print mein summary ka title
        public List<(string Label, string Value, bool Profit)> Items = new();         // print ke summary cards
    }
    private readonly List<Rep> _reps = new();
    private List<OrderSummary> _lastOrders = new();   // aakhri load hue orders (main muddat ke)
    private string _expDiag = "";                     // expenses load hone ki jaankari (masla pakarne ke liye)
    private bool _ready = false;
    private bool _busy = false;

    public ReportsForm()
    {
        InitializeComponent();
        _orders = Program.Services.GetRequiredService<OrderService>();
        _stock = Program.Services.GetRequiredService<StockService>();
        _expenses = Program.Services.GetRequiredService<ExpenseService>();
        BuildUi();
    }

    // ---------------------------------------------------------------- theme (sirf design)

    private static class Sky
    {
        public static readonly System.Drawing.Color Accent = System.Drawing.Color.FromArgb(79, 163, 224);
        public static readonly System.Drawing.Color Hover = System.Drawing.Color.FromArgb(59, 143, 204);
        public static readonly System.Drawing.Color Panel = System.Drawing.Color.FromArgb(240, 248, 255);
        public static readonly System.Drawing.Color RowAlt = System.Drawing.Color.FromArgb(247, 251, 255);
        public static readonly System.Drawing.Color Selected = System.Drawing.Color.FromArgb(207, 232, 250);
        public static readonly System.Drawing.Color GridLine = System.Drawing.Color.FromArgb(227, 238, 247);
        public static readonly System.Drawing.Color Text = System.Drawing.Color.FromArgb(30, 42, 54);
        public static readonly System.Drawing.Color Muted = System.Drawing.Color.FromArgb(107, 124, 140);
    }

   // ReportsForm.cs mein purane StyleGrid method ki jagah ye paste karein.
    // Sirf grid ka design badla hai; baqi code (Sky class, Bind, AddRep, logic) waisa hi rahega.
    private static void StyleGrid(DataGridView g)
    {
        var headBg = System.Drawing.Color.FromArgb(240, 249, 255);   // halka sky-blue header
        var headInk = System.Drawing.Color.FromArgb(12, 74, 110);    // gehra blue text
        var accent = System.Drawing.Color.FromArgb(2, 132, 199);     // header ke neeche line
        var rowAlt = System.Drawing.Color.FromArgb(248, 250, 252);
        var line = System.Drawing.Color.FromArgb(226, 232, 240);
        var selBg = System.Drawing.Color.FromArgb(224, 242, 254);
        var ink = System.Drawing.Color.FromArgb(30, 41, 59);

        g.EnableHeadersVisualStyles = false;
        g.BackgroundColor = System.Drawing.Color.White;
        g.BorderStyle = BorderStyle.None;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;   // sirf horizontal lines
        g.GridColor = line;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 40;
        g.RowTemplate.Height = 34;

        var h = g.ColumnHeadersDefaultCellStyle;
        h.BackColor = headBg;
        h.ForeColor = headInk;
        h.SelectionBackColor = headBg;
        h.SelectionForeColor = headInk;
        h.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
        h.Alignment = DataGridViewContentAlignment.MiddleLeft;
        h.Padding = new Padding(8, 0, 8, 0);

        g.DefaultCellStyle.BackColor = System.Drawing.Color.White;
        g.DefaultCellStyle.ForeColor = ink;
        g.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
        g.DefaultCellStyle.SelectionBackColor = selBg;
        g.DefaultCellStyle.SelectionForeColor = headInk;
        g.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
        g.AlternatingRowsDefaultCellStyle.BackColor = rowAlt;
        g.AlternatingRowsDefaultCellStyle.SelectionBackColor = selBg;
        g.AlternatingRowsDefaultCellStyle.SelectionForeColor = headInk;

        // header ke neeche sky-blue line (accent)
        g.Paint += (s, e) =>
        {
            using var pen = new System.Drawing.Pen(accent, 2F);
            var y = g.ColumnHeadersHeight - 1;
            e.Graphics.DrawLine(pen, 0, y, g.Width, y);
        };

        // TOTAL wali row: bold + halka sky-blue background (sirf nazar ka farq)
        g.CellFormatting += (s, e) =>
        {
            if (e.RowIndex < 0 || e.RowIndex >= g.Rows.Count || g.Columns.Count == 0) return;
            var first = g.Rows[e.RowIndex].Cells[0].Value as string;
            if (first == "TOTAL")
            {
                e.CellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
                e.CellStyle.BackColor = headBg;
                e.CellStyle.ForeColor = headInk;
            }
        };
    }

    private static void StyleButton(Button b, bool primary)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.Cursor = Cursors.Hand;
        b.UseVisualStyleBackColor = false;
        if (primary)
        {
            b.BackColor = Sky.Accent;
            b.ForeColor = System.Drawing.Color.White;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Sky.Hover;
            b.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        }
        else
        {
            b.BackColor = System.Drawing.Color.White;
            b.ForeColor = Sky.Hover;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Sky.Accent;
            b.FlatAppearance.MouseOverBackColor = Sky.Panel;
        }
    }

    private static void StyleCombo(ComboBox cb)
    {
        cb.FlatStyle = FlatStyle.Flat;
        cb.BackColor = System.Drawing.Color.White;
        cb.ForeColor = Sky.Text;
    }

    private void ApplyTheme(FlowLayoutPanel top)
    {
        BackColor = System.Drawing.Color.White;
        top.BackColor = Sky.Panel;

        foreach (Control c in top.Controls)
        {
            if (c is Label l) l.ForeColor = Sky.Text;
            else if (c is ComboBox cb) StyleCombo(cb);
        }
        StyleButton(_btnRefresh, true);
        StyleButton(_btnExport, false);
        StyleButton(_btnPrint, false);

        // tabs: sky blue owner-draw
        _tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
        _tabs.SizeMode = TabSizeMode.Fixed;
        _tabs.ItemSize = new System.Drawing.Size(170, 38);
        _tabs.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
        _tabs.DrawItem += Tabs_DrawItem;
    }

    private void Tabs_DrawItem(object? sender, DrawItemEventArgs e)
    {
        var tc = (TabControl)sender!;
        bool sel = e.Index == tc.SelectedIndex;
        using (var br = new SolidBrush(sel ? Sky.Accent : Sky.Panel))
            e.Graphics.FillRectangle(br, e.Bounds);
        TextRenderer.DrawText(e.Graphics, tc.TabPages[e.Index].Text, tc.Font, e.Bounds,
            sel ? System.Drawing.Color.White : Sky.Text,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
    }

    // ---------------------------------------------------------------- UI

    private void BuildUi()
    {
        _cmbRange.Items.AddRange(new object[] { "Aaj", "Is mahine", "Pichla mahina", "Is saal", "Custom" });
        _cmbRange.SelectedIndex = 1;
        _cmbGroup.Items.AddRange(new object[] { "Din wise", "Mahina wise" });
        _cmbGroup.SelectedIndex = 0;
        _from.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        _to.Value = DateTime.Today;
        _from.Enabled = _to.Enabled = false;

        var top = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 58,
            Padding = new Padding(8, 10, 8, 4),
            BackColor = Sky.Panel
        };
        top.Controls.Add(new Label { Text = "Muddat", AutoSize = true, Margin = new Padding(3, 8, 3, 0) });
        top.Controls.Add(_cmbRange);
        top.Controls.Add(_from);
        top.Controls.Add(_to);
        top.Controls.Add(new Label { Text = "Daily tab", AutoSize = true, Margin = new Padding(15, 8, 3, 0) });
        top.Controls.Add(_cmbGroup);
        top.Controls.Add(_btnRefresh);
        top.Controls.Add(_btnExport);
        top.Controls.Add(_btnPrint);

        AddRep("Daily / Monthly", "Din ya mahine ke hisab se sales");
        AddRep("Stock", "Color-wise stock", "Batch-wise stock");
        AddRep("Best Sellers", "Color-wise (sab se zyada bika)", "Article-wise");
        AddRep("Expenses", "Category-wise kharche", "Worker-wise (salary, bonus, advance)", "Ads vs Online sales");
        AddRep("Returns", "Courier-wise return", "City-wise return", "Return hue orders ki list");

        Controls.Add(_tabs);
        Controls.Add(top);
        ApplyTheme(top);

        _cmbRange.SelectedIndexChanged += async (s, e) =>
        {
            _from.Enabled = _to.Enabled = _cmbRange.SelectedIndex == 4;
            if (_ready) await LoadAllAsync();
        };
        _from.ValueChanged += async (s, e) => { if (_ready && _cmbRange.SelectedIndex == 4) await LoadAllAsync(); };
        _to.ValueChanged += async (s, e) => { if (_ready && _cmbRange.SelectedIndex == 4) await LoadAllAsync(); };
        _cmbGroup.SelectedIndexChanged += async (s, e) => { if (_ready) await LoadAllAsync(); };
        _btnRefresh.Click += async (s, e) => await LoadAllAsync();
        _btnExport.Click += (s, e) => Export();
        _btnPrint.Click += async (s, e) => await OpenPrintDialogAsync();
        Load += async (s, e) => { _ready = true; await LoadAllAsync(); };
    }

    private void AddRep(string title, params string[] gridTitles)
    {
        var rep = new Rep { Page = new TabPage(title) { BackColor = System.Drawing.Color.White } };
        var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1 };
        t.RowCount = gridTitles.Length * 2 + 1;

        foreach (var gt in gridTitles)
        {
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.Controls.Add(new Label
            {
                Text = gt,
                AutoSize = true,
                Margin = new Padding(6, 8, 3, 2),
                Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold),
                ForeColor = Sky.Text
            });
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / gridTitles.Length));
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                MultiSelect = false
            };
            StyleGrid(g);
            t.Controls.Add(g);
            rep.Grids.Add(g);
            rep.Titles.Add(gt);
        }

        rep.Summary = new Label
        {
            AutoSize = true,
            Margin = new Padding(6, 6, 3, 8),
            Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold),
            ForeColor = Sky.Hover
        };
        t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        t.Controls.Add(rep.Summary);

        rep.Page.Controls.Add(t);
        _tabs.TabPages.Add(rep.Page);
        _reps.Add(rep);
    }

    // ---------------------------------------------------------------- helpers

    private (DateTime from, DateTime to) Range()
    {
        var t = DateTime.Today;
        switch (_cmbRange.SelectedIndex)
        {
            case 0: return (t, t);
            case 2:
                var first = new DateTime(t.Year, t.Month, 1).AddMonths(-1);
                return (first, first.AddMonths(1).AddDays(-1));
            case 3: return (new DateTime(t.Year, 1, 1), t);
            case 4: return (_from.Value.Date, _to.Value.Date);
            default: return (new DateTime(t.Year, t.Month, 1), t);
        }
    }

    private static DataTable T(params (string name, Type type)[] cols)
    {
        var d = new DataTable();
        foreach (var (n, t) in cols) d.Columns.Add(n, t);
        return d;
    }

    private static void Bind(DataGridView g, DataTable dt)
    {
        g.DataSource = dt;
        foreach (DataGridViewColumn c in g.Columns)
        {
            var type = dt.Columns[c.Name]?.DataType;
            if (type == typeof(decimal) || type == typeof(int))
            {
                c.DefaultCellStyle.Format = "N0";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (type == typeof(DateTime)) c.DefaultCellStyle.Format = "dd-MMM-yyyy";
        }
        g.ClearSelection();
    }

    private static void Warn(string msg) =>
        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

    // number ko "1,234" format mein (null/object ke saath bhi safe)
    private static string N(object? v, string fmt = "N0") =>
        v is IFormattable f ? f.ToString(fmt, CultureInfo.CurrentCulture) : "0";

    private static decimal Dec(object? v) => v == null ? 0 : Convert.ToDecimal(v);

    // screen wali summary line + print ke summary cards ek hi jagah se set hote hain
    private static void SetSummary(Rep rep, string summaryTitle, string? prefix,
                                   params (string Label, string Value, bool Profit)[] items)
    {
        rep.SummaryTitle = summaryTitle;
        rep.Items = items.ToList();
        rep.Summary.Text = (string.IsNullOrWhiteSpace(prefix) ? "" : prefix + "   |   ") +
                           string.Join("   |   ", items.Select(i => $"{i.Label} {i.Value}"));
    }

    // ---------------------------------------------------------------- load

    private static bool IsActiveExpense(ExpenseItem x) =>
        string.IsNullOrWhiteSpace(x.Status) || string.Equals(x.Status, "Active", StringComparison.OrdinalIgnoreCase);

    private async Task LoadAllAsync()
    {
        if (_busy) return;
        _busy = true;
        try
        {
            var (from, to) = Range();
            var orders = (await _orders.GetOrdersAsync(new OrderFilter { From = from, To = to })).ToList();
            _lastOrders = orders;
            var lines = (await _orders.GetSalesLinesAsync(from, to)).ToList();
            var colors = (await _stock.GetColorStockAsync()).ToList();
            var batches = (await _stock.GetAllBatchesAsync()).Where(b => b.MetersRemaining > 0).ToList();

            var allExp = new List<ExpenseItem>();
            for (var m = new DateTime(from.Year, from.Month, 1); m <= to; m = m.AddMonths(1))
                allExp.AddRange(await _expenses.GetAsync(new ExpenseFilter { Month = m }));
            var exp = allExp.Where(IsActiveExpense).ToList();

            // masla pakarne ke liye: kitni entries mili, kitni Active nahi thin
            var hidden = allExp.Count - exp.Count;
            _expDiag = allExp.Count == 0
                ? "DHYAN: is muddat ki koi kharcha entry nahi mili"
                : hidden > 0
                    ? $"DHYAN: {hidden} entry Active nahi thin (status: " +
                      string.Join(", ", allExp.Where(x => !IsActiveExpense(x)).Select(x => x.Status).Distinct()) + ")"
                    : "";

            BuildDaily(orders, _cmbGroup.SelectedIndex == 1, from, to);
            BuildStock(colors, batches);
            BuildBest(lines, from, to);
            BuildExpenses(exp, orders, from, to);
            BuildReturns(orders, from, to);
        }
        catch (Exception ex) { Warn(ex.Message); }
        finally { _busy = false; }
    }

    // ---------------------------------------------------------------- 1. daily / monthly

    // Sales table ke columns (screen aur print dono yahi use karte hain)
    private static readonly (string name, Type type)[] SalesCols =
    {
        ("Tareekh", typeof(string)), ("Shop Sales", typeof(int)), ("Shop Suits", typeof(int)),
        ("Shop Amount", typeof(decimal)), ("Online Orders", typeof(int)), ("Online Suits", typeof(int)),
        ("Online Amount", typeof(decimal)), ("Returned", typeof(int)), ("COD Loss", typeof(decimal)),
        ("Profit", typeof(decimal))
    };
    private const int ProfitCol = 9;

    private static object[] SalesRow(string label, IEnumerable<OrderSummary> g)
    {
        var list = g.ToList();
        var shop = list.Where(o => o.Channel == "Shop").ToList();
        var onl = list.Where(o => o.Channel == "Online" && o.Status != "Returned").ToList();
        var ret = list.Where(o => o.Status == "Returned").ToList();
        return new object[]
        {
            label, shop.Count, shop.Sum(o => o.Suits), shop.Sum(o => o.NetSale),
            onl.Count, onl.Sum(o => o.Suits), onl.Sum(o => o.NetSale),
            ret.Count, -ret.Sum(o => o.Profit), list.Sum(o => o.Profit)
        };
    }

    // din-wise ya mahina-wise rows, aakhri row TOTAL
    private static List<object[]> SalesRows(List<OrderSummary> orders, bool monthly)
    {
        var live = orders.Where(o => o.Status != "Cancelled").ToList();
        var rows = live
            .GroupBy(o => monthly ? new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1) : o.OrderDate.Date)
            .OrderBy(g => g.Key)
            .Select(g => SalesRow(g.Key.ToString(monthly ? "MMM-yyyy" : "dd-MMM-yyyy (ddd)"), g))
            .ToList();
        rows.Add(SalesRow("TOTAL", live));
        return rows;
    }

    private void BuildDaily(List<OrderSummary> orders, bool monthly, DateTime from, DateTime to)
    {
        var rep = _reps[0];
        var dt = T(SalesCols);
        var rows = SalesRows(orders, monthly);
        foreach (var r in rows) dt.Rows.Add(r);
        Bind(rep.Grids[0], dt);

        var tot = rows[^1];
        SetSummary(rep, "Sales Summary", $"{from:dd-MMM-yyyy} se {to:dd-MMM-yyyy}",
            ("Shop Amount", N(tot[3]), false),
            ("Online Amount", N(tot[6]), false),
            ("COD Loss", N(tot[8]), false),
            ("Profit (kharchon se pehle)", N(tot[9]), true));
    }

    // ---------------------------------------------------------------- 2. stock

    private void BuildStock(List<ClothStock.Models.Stock.StockColorView> colors,
                            List<ClothStock.Models.Stock.StockBatchView> batches)
    {
        var rep = _reps[1];

        var c = T(("Article", typeof(string)), ("Color", typeof(string)), ("Batches", typeof(int)),
                  ("Meter Baqi", typeof(decimal)), ("Suits Baqi", typeof(int)), ("Stock Value", typeof(decimal)),
                  ("Alert", typeof(string)));
        foreach (var x in colors)
            c.Rows.Add(x.ArticleName, x.ColorName, x.TotalBatches, x.MetersRemaining, x.SuitsRemaining,
                       x.StockValue, x.IsLowStock ? "LOW STOCK" : "");
        Bind(rep.Grids[0], c);

        var b = T(("Article", typeof(string)), ("Color", typeof(string)), ("Batch", typeof(string)),
                  ("Received", typeof(DateTime)), ("Suits Baqi", typeof(int)), ("Tukra (m)", typeof(decimal)),
                  ("Cost/Suit", typeof(decimal)), ("Stock Value", typeof(decimal)), ("Purana (mahine)", typeof(int)),
                  ("Alert", typeof(string)));
        foreach (var x in batches)
            b.Rows.Add(x.ArticleName, x.ColorName, x.BatchNo, x.ReceivedDate, x.SuitsRemaining, x.LooseMeters,
                       x.CostPricePerSuit, x.StockValue, x.AgeMonths, x.IsOldStock ? "PURANA MAAL" : "");
        Bind(rep.Grids[1], b);
        rep.Grids[1].Columns["Tukra (m)"]!.DefaultCellStyle.Format = "0.###";

        SetSummary(rep, "Stock Summary (aaj ka stock)", "Aaj ka stock (muddat lagu nahi)",
            ("Total Suits", colors.Sum(x => x.SuitsRemaining).ToString("N0"), false),
            ("Meter Baqi", colors.Sum(x => x.MetersRemaining).ToString("0.###"), false),
            ("Stock Value", colors.Sum(x => x.StockValue).ToString("N0"), false),
            ("Low Stock Colors", colors.Count(x => x.IsLowStock).ToString("N0"), false),
            ("Purane Batches", batches.Count(x => x.IsOldStock).ToString("N0"), false));
    }

    // ---------------------------------------------------------------- 3. best sellers

    private void BuildBest(List<SalesLine> lines, DateTime from, DateTime to)
    {
        var rep = _reps[2];
        var l = lines.Where(x => x.Status != "Cancelled" && x.Status != "Returned")
            .Select(x => new
            {
                x.ArticleName,
                x.ColorName,
                x.Channel,
                Kept = x.Quantity - x.ReturnedQty,
                x.SellingPrice,
                x.CostPrice
            })
            .Where(x => x.Kept > 0).ToList();

        var colorRows = l.GroupBy(x => new { x.ArticleName, x.ColorName })
            .Select(g => new
            {
                g.Key.ArticleName,
                g.Key.ColorName,
                Suits = g.Sum(x => x.Kept),
                Shop = g.Where(x => x.Channel == "Shop").Sum(x => x.Kept),
                Online = g.Where(x => x.Channel == "Online").Sum(x => x.Kept),
                Sale = g.Sum(x => x.Kept * x.SellingPrice),
                Margin = g.Sum(x => x.Kept * (x.SellingPrice - x.CostPrice))
            })
            .OrderByDescending(x => x.Suits).ToList();

        var d1 = T(("#", typeof(int)), ("Article", typeof(string)), ("Color", typeof(string)), ("Suits", typeof(int)),
                   ("Shop Suits", typeof(int)), ("Online Suits", typeof(int)), ("Sale", typeof(decimal)),
                   ("Margin", typeof(decimal)), ("Margin/Suit", typeof(decimal)));
        var i = 0;
        foreach (var x in colorRows)
            d1.Rows.Add(++i, x.ArticleName, x.ColorName, x.Suits, x.Shop, x.Online, x.Sale, x.Margin,
                        x.Suits > 0 ? x.Margin / x.Suits : 0);
        Bind(rep.Grids[0], d1);

        var artRows = l.GroupBy(x => x.ArticleName)
            .Select(g => new
            {
                Article = g.Key,
                Suits = g.Sum(x => x.Kept),
                Sale = g.Sum(x => x.Kept * x.SellingPrice),
                Margin = g.Sum(x => x.Kept * (x.SellingPrice - x.CostPrice))
            })
            .OrderByDescending(x => x.Suits).ToList();
        var d2 = T(("#", typeof(int)), ("Article", typeof(string)), ("Suits", typeof(int)),
                   ("Sale", typeof(decimal)), ("Margin", typeof(decimal)));
        i = 0;
        foreach (var x in artRows) d2.Rows.Add(++i, x.Article, x.Suits, x.Sale, x.Margin);
        Bind(rep.Grids[1], d2);

        var top = colorRows.FirstOrDefault();
        var items = new List<(string Label, string Value, bool Profit)>
        {
            ("Suits Bike", l.Sum(x => x.Kept).ToString("N0"), false),
            ("Sale", l.Sum(x => x.Kept * x.SellingPrice).ToString("N0"), false),
            ("Margin (tax/courier se pehle)", l.Sum(x => x.Kept * (x.SellingPrice - x.CostPrice)).ToString("N0"), true)
        };
        if (top != null) items.Add(("No.1 Best Seller", $"{top.ArticleName} {top.ColorName} ({top.Suits})", false));

        SetSummary(rep, "Best Sellers Summary", $"{from:dd-MMM-yyyy} se {to:dd-MMM-yyyy}", items.ToArray());
    }

    // ---------------------------------------------------------------- 4. expenses

    private void BuildExpenses(List<ExpenseItem> exp, List<OrderSummary> orders, DateTime from, DateTime to)
    {
        var rep = _reps[3];

        var d1 = T(("Group", typeof(string)), ("Category", typeof(string)), ("Shop", typeof(decimal)),
                   ("Online", typeof(decimal)), ("Total", typeof(decimal)), ("Entries", typeof(int)));
        foreach (var g in exp.GroupBy(x => new { x.GroupName, x.CategoryName })
                     .Select(g => new {
                         g.Key,
                         Shop = g.Sum(x => x.ShopPart),
                         Online = g.Sum(x => x.OnlinePart),
                         Total = g.Sum(x => x.ExpenseAmount),
                         N = g.Count()
                     })
                     .Where(x => x.Total != 0).OrderByDescending(x => x.Total))
            d1.Rows.Add(g.Key.GroupName, g.Key.CategoryName, g.Shop, g.Online, g.Total, g.N);
        d1.Rows.Add("TOTAL", "", exp.Sum(x => x.ShopPart), exp.Sum(x => x.OnlinePart), exp.Sum(x => x.ExpenseAmount), exp.Count);
        Bind(rep.Grids[0], d1);

        // Worker-wise: WorkerId ya WorkerName dono mein se jo bhi bhara ho, Kind ka case ignore
        bool KindIs(ExpenseItem x, string k) => string.Equals(x.Kind, k, StringComparison.OrdinalIgnoreCase);
        var d2 = T(("Worker", typeof(string)), ("Salary", typeof(decimal)), ("Bonus", typeof(decimal)),
                   ("Advance diya", typeof(decimal)), ("Advance kata", typeof(decimal)), ("Kul kharcha", typeof(decimal)));
        foreach (var g in exp.Where(x => x.WorkerId.HasValue || !string.IsNullOrWhiteSpace(x.WorkerName))
                     .GroupBy(x => !string.IsNullOrWhiteSpace(x.WorkerName) ? x.WorkerName! : $"Worker #{x.WorkerId}")
                     .OrderBy(g => g.Key))
        {
            var sal = g.Where(x => KindIs(x, "Salary")).Sum(x => x.ExpenseAmount);
            var bon = g.Where(x => KindIs(x, "Bonus")).Sum(x => x.ExpenseAmount);
            d2.Rows.Add(g.Key, sal, bon, g.Where(x => KindIs(x, "Advance")).Sum(x => x.Amount),
                        g.Sum(x => x.AdvanceCut), sal + bon);
        }
        Bind(rep.Grids[1], d2);

        var onl = orders.Where(o => o.Channel == "Online" && o.Status != "Cancelled" && o.Status != "Returned").ToList();
        var onlSale = onl.Sum(o => o.NetSale);
        var ads = exp.Where(x => x.GroupName == "Marketing").Sum(x => x.OnlinePart);
        var d3 = T(("Hisab", typeof(string)), ("Value", typeof(string)));
        d3.Rows.Add("Ads kharcha (Marketing group)", ads.ToString("N0"));
        d3.Rows.Add("Online sale", onlSale.ToString("N0"));
        d3.Rows.Add("Online orders", onl.Count.ToString("N0"));
        d3.Rows.Add("Ads / Online sale", onlSale > 0 ? $"{ads / onlSale * 100:0.##} %" : "-");
        d3.Rows.Add("Ads per order", onl.Count > 0 ? (ads / onl.Count).ToString("N0") : "-");
        d3.Rows.Add("Online order ka profit (kharchon se pehle)", onl.Sum(o => o.Profit).ToString("N0"));
        d3.Rows.Add("Online profit - Ads", (onl.Sum(o => o.Profit) - ads).ToString("N0"));
        Bind(rep.Grids[2], d3);

        var prefix = $"{from:dd-MMM-yyyy} se {to:dd-MMM-yyyy} (mahine ke hisab se, advance shamil nahi)";
        if (!string.IsNullOrEmpty(_expDiag)) prefix += "   |   " + _expDiag;

        SetSummary(rep, "Expenses Summary (advance shamil nahi)", prefix,
            ("Shop Kharcha", exp.Sum(x => x.ShopPart).ToString("N0"), false),
            ("Online Kharcha", exp.Sum(x => x.OnlinePart).ToString("N0"), false),
            ("Total Kharcha", exp.Sum(x => x.ExpenseAmount).ToString("N0"), false));
    }

    // ---------------------------------------------------------------- 5. returns

    private void BuildReturns(List<OrderSummary> orders, DateTime from, DateTime to)
    {
        var rep = _reps[4];
        var online = orders.Where(o => o.Channel == "Online" && o.Status != "Cancelled").ToList();
        var ret = online.Where(o => o.Status == "Returned").ToList();

        DataTable Group(Func<OrderSummary, string> key, string title)
        {
            var d = T((title, typeof(string)), ("Kul Orders", typeof(int)), ("Return", typeof(int)),
                      ("Return %", typeof(string)), ("Suits wapas", typeof(int)), ("COD charge", typeof(decimal)),
                      ("Wapsi charge", typeof(decimal)), ("Kul Loss", typeof(decimal)));
            foreach (var g in online.GroupBy(key).OrderByDescending(g => g.Count(o => o.Status == "Returned")))
            {
                var r = g.Where(o => o.Status == "Returned").ToList();
                d.Rows.Add(g.Key, g.Count(), r.Count, g.Count() > 0 ? $"{r.Count * 100.0 / g.Count():0.#} %" : "-",
                           r.Sum(o => o.Suits), r.Sum(o => o.CourierCharge), r.Sum(o => o.ReturnCharge),
                           r.Sum(o => o.CourierCharge + o.ReturnCharge));
            }
            return d;
        }

        Bind(rep.Grids[0], Group(o => string.IsNullOrWhiteSpace(o.CourierName) ? "(courier nahi likha)" : o.CourierName!, "Courier"));
        Bind(rep.Grids[1], Group(o => string.IsNullOrWhiteSpace(o.City) ? "(city nahi likhi)" : o.City!.Trim(), "City"));

        var d3 = T(("Order", typeof(string)), ("Date", typeof(DateTime)), ("Customer", typeof(string)),
                   ("City", typeof(string)), ("Courier", typeof(string)), ("Tracking ID", typeof(string)),
                   ("Suits", typeof(int)), ("COD charge", typeof(decimal)), ("Wapsi charge", typeof(decimal)),
                   ("Loss", typeof(decimal)));
        foreach (var o in ret.OrderByDescending(o => o.OrderId))
            d3.Rows.Add(o.OrderNo, o.OrderDate, o.CustomerName ?? "", o.City ?? "", o.CourierName ?? "",
                        o.TrackingId ?? "", o.Suits, o.CourierCharge, o.ReturnCharge, o.CourierCharge + o.ReturnCharge);
        Bind(rep.Grids[2], d3);

        var pct = online.Count > 0 ? ret.Count * 100.0 / online.Count : 0;
        SetSummary(rep, "Returns Summary (order ki tareekh ke hisab se)",
            $"{from:dd-MMM-yyyy} se {to:dd-MMM-yyyy} (order ki tareekh)",
            ("Online Orders", online.Count.ToString("N0"), false),
            ("Returns", $"{ret.Count:N0} ({pct:0.#} %)", false),
            ("COD / Courier Loss", ret.Sum(o => o.CourierCharge + o.ReturnCharge).ToString("N0"), false));
    }

    // ---------------------------------------------------------------- print

    // Print dialog: content ke hisab se khud size hota hai (auto adjust)
    private class PrintDialog : Form
    {
        public readonly CheckedListBox List = new() { CheckOnClick = true, IntegralHeight = false, Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 4) };
        public readonly ComboBox Paper = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };

        // Sales: alag alag
        public readonly CheckBox SalesDaily = new() { Text = "Daily sales (din wise)  -  upar select ki hui muddat", AutoSize = true };
        public readonly CheckBox SalesMonthly = new() { Text = "Monthly sales (mahina wise)  -  upar select ki hui muddat", AutoSize = true };
        public readonly CheckBox SalesCustom = new() { Text = "Custom sales  -  apni marzi ki tareekhein", AutoSize = true };
        public readonly DateTimePicker CustomFrom = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 120 };
        public readonly DateTimePicker CustomTo = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 120 };
        public readonly ComboBox CustomGroup = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110 };

        public readonly CheckBox SaleWise = new() { Text = "Sale-wise report (har sale ki alag line)", AutoSize = true };
        public bool Preview = true;

        public PrintDialog(IList<string> items, ISet<int> preChecked, int paperIndex, bool canSaleWise,
                           bool preDaily, DateTime from, DateTime to)
        {
            Text = "Print - kya print karna hai?";
            AutoScaleMode = AutoScaleMode.Dpi;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = MinimizeBox = false;
            ShowInTaskbar = false;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            MaximumSize = Screen.PrimaryScreen?.WorkingArea.Size ?? new System.Drawing.Size(1200, 800);
            BackColor = System.Drawing.Color.White;

            // list: height items ke mutabiq (max 10 items, phir scroll), width sab se lambe naam ke mutabiq
            for (var i = 0; i < items.Count; i++) List.Items.Add(items[i], preChecked.Contains(i));
            var visible = Math.Min(Math.Max(items.Count, 2), 10);
            var longest = items.Count == 0 ? 200 :
                items.Max(s => TextRenderer.MeasureText(s, List.Font).Width);
            List.Height = visible * List.ItemHeight + 6;
            List.Width = Math.Max(420, longest + 50);
            List.MinimumSize = List.Size;
            List.BorderStyle = BorderStyle.FixedSingle;
            List.BackColor = Sky.Panel;
            List.ForeColor = Sky.Text;

            Paper.Items.AddRange(new object[]
            {
                "A4 Portrait", "A4 Landscape (zyada columns wali tables)", "Thermal (shop ki setting ke mutabiq)"
            });
            Paper.SelectedIndex = paperIndex;
            StyleCombo(Paper);
            StyleCombo(CustomGroup);

            foreach (var cb in new[] { SalesDaily, SalesMonthly, SalesCustom, SaleWise })
                cb.ForeColor = Sky.Text;

            SalesDaily.Checked = preDaily;
            SaleWise.Enabled = canSaleWise;

            CustomFrom.Value = from;
            CustomTo.Value = to;
            CustomGroup.Items.AddRange(new object[] { "Din wise", "Mahina wise" });
            CustomGroup.SelectedIndex = 0;
            void SyncCustom() => CustomFrom.Enabled = CustomTo.Enabled = CustomGroup.Enabled = SalesCustom.Checked;
            SalesCustom.CheckedChanged += (s, e) => SyncCustom();
            SyncCustom();

            var customRow = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = true,
                Margin = new Padding(22, 0, 0, 6)
            };
            customRow.Controls.Add(new Label { Text = "Se", AutoSize = true, Margin = new Padding(0, 6, 4, 0), ForeColor = Sky.Text });
            customRow.Controls.Add(CustomFrom);
            customRow.Controls.Add(new Label { Text = "Tak", AutoSize = true, Margin = new Padding(8, 6, 4, 0), ForeColor = Sky.Text });
            customRow.Controls.Add(CustomTo);
            customRow.Controls.Add(CustomGroup);

            var btnAll = new Button { Text = "Sab", AutoSize = true, MinimumSize = new System.Drawing.Size(70, 30) };
            var btnNone = new Button { Text = "Koi nahi", AutoSize = true, MinimumSize = new System.Drawing.Size(80, 30) };
            var btnPreview = new Button { Text = "Preview", AutoSize = true, MinimumSize = new System.Drawing.Size(90, 32) };
            var btnPrint = new Button { Text = "Print", AutoSize = true, MinimumSize = new System.Drawing.Size(90, 32) };
            var btnCancel = new Button { Text = "Cancel", AutoSize = true, MinimumSize = new System.Drawing.Size(80, 32), DialogResult = DialogResult.Cancel };

            StyleButton(btnAll, false);
            StyleButton(btnNone, false);
            StyleButton(btnPreview, true);
            StyleButton(btnPrint, true);
            StyleButton(btnCancel, false);

            btnAll.Click += (s, e) => { for (var i = 0; i < List.Items.Count; i++) List.SetItemChecked(i, true); };
            btnNone.Click += (s, e) => { for (var i = 0; i < List.Items.Count; i++) List.SetItemChecked(i, false); };
            btnPreview.Click += (s, e) => { Preview = true; DialogResult = DialogResult.OK; };
            btnPrint.Click += (s, e) => { Preview = false; DialogResult = DialogResult.OK; };
            CancelButton = btnCancel;
            AcceptButton = btnPreview;

            var selRow = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0, 0, 0, 4) };
            selRow.Controls.Add(btnAll);
            selRow.Controls.Add(btnNone);

            var bottom = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 0)
            };
            bottom.Controls.Add(btnCancel);
            bottom.Controls.Add(btnPrint);
            bottom.Controls.Add(btnPreview);

            Label Head(string t) => new()
            {
                Text = t,
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold),
                ForeColor = Sky.Hover,
                Margin = new Padding(0, 8, 0, 4)
            };

            var root = new TableLayoutPanel
            {
                ColumnCount = 1,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(14, 8, 14, 12),
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };
            void Add(Control c)
            {
                root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                if (c is CheckBox or Label or FlowLayoutPanel && c != bottom) c.Anchor = AnchorStyles.Left;
                root.Controls.Add(c, 0, root.RowCount++);
            }

            Add(Head("1) Sales report"));
            Add(SalesDaily);
            Add(SalesMonthly);
            Add(SalesCustom);
            Add(customRow);
            Add(SaleWise);

            Add(Head("2) Baqi tables"));
            Add(selRow);
            Add(List);

            Add(Head("3) Paper"));
            Add(Paper);
            Add(bottom);

            Controls.Add(root);
        }
    }

    private async Task OpenPrintDialogAsync()
    {
        // baqi tables (Daily tab ki table nahi, wo sales section se milti hai): (tab index, grid index)
        var map = new List<(int rep, int grid)>();
        var names = new List<string>();
        for (var r = 1; r < _reps.Count; r++)
            for (var g = 0; g < _reps[r].Grids.Count; g++)
            {
                map.Add((r, g));
                names.Add($"{_reps[r].Page.Text}  -  {_reps[r].Titles[g]}");
            }

        var cur = Math.Max(0, _tabs.SelectedIndex);
        var pre = new HashSet<int>(Enumerable.Range(0, map.Count).Where(i => map[i].rep == cur));

        ReceiptData shop;
        try { shop = await LoadShopReceiptDataAsync(); }
        catch (Exception ex) { Warn(ex.Message); return; }

        var paperIdx = shop.WidthMm >= 150 ? 1 : 2;
        var (from, to) = Range();

        using var dlg = new PrintDialog(names, pre, paperIdx, _lastOrders.Count > 0,
            preDaily: cur == 0, from, to);
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        var picked = dlg.List.CheckedIndices.Cast<int>().ToList();
        var anySales = dlg.SalesDaily.Checked || dlg.SalesMonthly.Checked || dlg.SalesCustom.Checked;
        if (picked.Count == 0 && !anySales && !dlg.SaleWise.Checked)
        {
            MessageBox.Show("Kam az kam ek cheez chunein.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var period = from == to ? from.ToString("dd-MMM-yyyy") : $"{from:dd-MMM-yyyy} se {to:dd-MMM-yyyy}";
            var mode = dlg.Paper.SelectedIndex switch
            {
                0 => PaperMode.A4Portrait,
                1 => PaperMode.A4Landscape,
                _ => PaperMode.Thermal
            };
            var showProfit = IsOwner();

            var data = new GridReportData
            {
                Shop = shop,
                Period = period,
                PrintedBy = CurrentUserName(),
                PrintedAt = DateTime.Now
            };

            // 1) sales: daily / monthly / custom (alag alag section)
            if (dlg.SalesDaily.Checked)
                AddSalesSection(data, $"Daily Sales ({period})", _lastOrders, false, showProfit);
            if (dlg.SalesMonthly.Checked)
                AddSalesSection(data, $"Monthly Sales ({period})", _lastOrders, true, showProfit);

            if (dlg.SalesCustom.Checked)
            {
                var cf = dlg.CustomFrom.Value.Date;
                var ct = dlg.CustomTo.Value.Date;
                if (ct < cf) (cf, ct) = (ct, cf);
                var cperiod = cf == ct ? cf.ToString("dd-MMM-yyyy") : $"{cf:dd-MMM-yyyy} se {ct:dd-MMM-yyyy}";
                var custom = (await _orders.GetOrdersAsync(new OrderFilter { From = cf, To = ct })).ToList();
                var cMonthly = dlg.CustomGroup.SelectedIndex == 1;
                AddSalesSection(data, $"Custom Sales ({cperiod}, {(cMonthly ? "mahina wise" : "din wise")})",
                    custom, cMonthly, showProfit);

                // sirf custom print ho raha ho to header mein bhi wahi muddat
                if (!dlg.SalesDaily.Checked && !dlg.SalesMonthly.Checked && picked.Count == 0)
                    data.Period = cperiod;
            }

            // 2) chuni hui baqi tables + unki summary cards
            var repsUsed = picked.Select(i => map[i].rep).Distinct().ToList();
            foreach (var i in picked)
                data.Sections.Add(Snapshot(_reps[map[i].rep].Grids[map[i].grid], _reps[map[i].rep].Titles[map[i].grid]));
            foreach (var ri in repsUsed)
            {
                var rep = _reps[ri];
                var items = rep.Items.Where(x => showProfit || !x.Profit)
                                     .Select(x => new PrintItem(x.Label, x.Value)).ToList();
                if (items.Count > 0)
                    data.Summaries.Add(new PrintSummary { Title = rep.SummaryTitle, Items = items });
            }

            data.Title = anySales && picked.Count == 0 ? "Sales Report"
                       : !anySales && repsUsed.Count == 1 ? _reps[repsUsed[0]].Page.Text + " Report"
                       : "Business Report";

            if (data.Sections.Count > 0)
                GridReportPrinter.Print(data, mode, this, dlg.Preview);

            // 3) sale-wise report (purana ReportPrinter)
            if (dlg.SaleWise.Checked)
            {
                ReportPrinter.Print(new SalesReportData
                {
                    Shop = shop,
                    Period = period,
                    Orders = _lastOrders,
                    ShowProfit = showProfit,
                    PrintedBy = CurrentUserName(),
                    PrintedAt = DateTime.Now
                }, this, dlg.Preview);
            }
        }
        catch (Exception ex) { Warn(ex.Message); }
    }

    // ek sales section (din ya mahina wise) + uski summary cards print data mein jorta hai
    private static void AddSalesSection(GridReportData data, string title, List<OrderSummary> orders,
                                        bool monthly, bool showProfit)
    {
        var rows = SalesRows(orders, monthly);
        var keep = Enumerable.Range(0, SalesCols.Length).Where(i => showProfit || i != ProfitCol).ToArray();

        var s = new PrintSection
        {
            Title = title,
            Headers = keep.Select(i => SalesCols[i].name).ToArray(),
            RightAlign = keep.Select(i => i > 0).ToArray(),
            LastRowIsTotal = true
        };
        foreach (var r in rows)
            s.Rows.Add(keep.Select(i => i == 0 ? Convert.ToString(r[i]) ?? "" : N(r[i])).ToArray());
        data.Sections.Add(s);

        var tot = rows[^1];
        var sm = new PrintSummary { Title = title };
        sm.Items.Add(new PrintItem("Shop Amount", N(tot[3])));
        sm.Items.Add(new PrintItem("Online Amount", N(tot[6])));
        sm.Items.Add(new PrintItem("Total Amount", N(Dec(tot[3]) + Dec(tot[6]))));
        sm.Items.Add(new PrintItem("COD Loss", N(tot[8])));
        if (showProfit) sm.Items.Add(new PrintItem("Profit (kharchon se pehle)", N(tot[9])));
        data.Summaries.Add(sm);
    }

    // grid ki wahi values print hoti hain jo screen par nazar aa rahi hain (format ke saath)
    private static PrintSection Snapshot(DataGridView g, string title)
    {
        var cols = g.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).OrderBy(c => c.DisplayIndex).ToList();
        var s = new PrintSection
        {
            Title = title,
            Headers = cols.Select(c => c.HeaderText).ToArray(),
            RightAlign = cols.Select(c => c.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleRight).ToArray()
        };
        foreach (DataGridViewRow r in g.Rows)
        {
            if (r.IsNewRow) continue;
            s.Rows.Add(cols.Select(c => r.Cells[c.Index].FormattedValue?.ToString() ?? "").ToArray());
        }
        s.LastRowIsTotal = s.Rows.Count > 0 && s.Rows[^1].Length > 0 && s.Rows[^1][0] == "TOTAL";
        return s;
    }

    // Shop ka naam, logo, address, phone aur paper ki chaurai Settings se (receipt ki tarah)
    private static async Task<ReceiptData> LoadShopReceiptDataAsync()
    {
        var settings = Program.Services.GetRequiredService<SettingsService>();
        return await ReceiptPrinter.CreateAsync(settings);
    }

    // Profit sirf Owner ko dikhta hai
    private static bool IsOwner() => SessionManager.IsOwner;

    // Print karne wale ka naam (login se)
    private static string CurrentUserName() => SessionManager.Username;

    // ---------------------------------------------------------------- export

    private static string Esc(string? s)
    {
        s ??= "";
        return s.Contains(',') || s.Contains('"') || s.Contains('\n') ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
    }

    private void Export()
    {
        var rep = _reps[_tabs.SelectedIndex];
        using var dlg = new SaveFileDialog
        {
            Filter = "Excel (CSV)|*.csv",
            FileName = $"{rep.Page.Text.Replace(" / ", "_").Replace(" ", "_")}_{DateTime.Today:yyyyMMdd}.csv"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            var sb = new StringBuilder();
            var (from, to) = Range();
            sb.AppendLine(Esc(rep.Page.Text) + "," + Esc($"{from:dd-MMM-yyyy} se {to:dd-MMM-yyyy}"));

            for (var i = 0; i < rep.Grids.Count; i++)
            {
                var g = rep.Grids[i];
                sb.AppendLine();
                sb.AppendLine(Esc(rep.Titles[i]));
                var cols = g.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).ToList();
                sb.AppendLine(string.Join(",", cols.Select(c => Esc(c.HeaderText))));
                foreach (DataGridViewRow r in g.Rows)
                    sb.AppendLine(string.Join(",", cols.Select(c => Esc(Convert.ToString(r.Cells[c.Index].Value)))));
            }
            sb.AppendLine();
            sb.AppendLine(Esc(rep.Summary.Text));

            File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
            if (MessageBox.Show("Export ho gaya. File kholni hai?", "Done", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.Yes)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
        }
        catch (Exception ex) { Warn(ex.Message); }
    }
}