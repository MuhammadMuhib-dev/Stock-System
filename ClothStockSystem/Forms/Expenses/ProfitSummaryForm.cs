using ClothStock.Business.Services;
using ClothStock.Models.Expenses;
using ClothStock.Models.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Expenses;

public partial class ProfitSummaryForm : Form
{
    private readonly OrderService _orders;
    private readonly ExpenseService _expenses;
    private bool _ready = false;

    private class Row
    {
        public string Detail { get; set; } = "";
        public decimal Shop { get; set; }
        public decimal Online { get; set; }
        public decimal Total => Shop + Online;
        public bool Bold { get; set; }
    }

    public ProfitSummaryForm()
    {
        InitializeComponent();
        _orders = Program.Services.GetRequiredService<OrderService>();
        _expenses = Program.Services.GetRequiredService<ExpenseService>();

        cmbRange.Items.AddRange(new object[] { "Is mahine", "Pichla mahina", "Aaj", "Custom" });
        cmbRange.SelectedIndex = 0;
        dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpTo.Value = DateTime.Today;
        dtpFrom.Enabled = dtpTo.Enabled = false;
    }

    private (DateTime from, DateTime to) Range()
    {
        var t = DateTime.Today;
        switch (cmbRange.SelectedIndex)
        {
            case 1:
                var first = new DateTime(t.Year, t.Month, 1).AddMonths(-1);
                return (first, first.AddMonths(1).AddDays(-1));
            case 2: return (t, t);
            case 3: return (dtpFrom.Value.Date, dtpTo.Value.Date);
            default: return (new DateTime(t.Year, t.Month, 1), t);
        }
    }

    private async void ProfitSummaryForm_Load(object? sender, EventArgs e)
    {
        _ready = true;
        await LoadAsync();
    }

    private async void cmbRange_Changed(object? sender, EventArgs e)
    {
        dtpFrom.Enabled = dtpTo.Enabled = cmbRange.SelectedIndex == 3;
        if (_ready) await LoadAsync();
    }

    private async void Dates_Changed(object? sender, EventArgs e)
    {
        if (_ready && cmbRange.SelectedIndex == 3) await LoadAsync();
    }

    private async void btnRefresh_Click(object? sender, EventArgs e) => await LoadAsync();

    private async Task LoadAsync()
    {
        try
        {
            var (from, to) = Range();

            var orders = (await _orders.GetOrdersAsync(new OrderFilter { From = from, To = to })).ToList();

            // Kharche: mahine ke hisab se (PeriodMonth), muddat ke sab mahine
            var months = new List<DateTime>();
            for (var m = new DateTime(from.Year, from.Month, 1); m <= to; m = m.AddMonths(1)) months.Add(m);
            var exp = new List<ExpenseItem>();
            foreach (var m in months)
                exp.AddRange((await _expenses.GetAsync(new ExpenseFilter { Month = m })).Where(x => x.Status == "Active"));

            Build(orders, exp, from, to);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Build(List<OrderSummary> orders, List<ExpenseItem> exp, DateTime from, DateTime to)
    {
        var shop = orders.Where(o => o.Channel == "Shop" && o.Status != "Cancelled").ToList();
        var onl = orders.Where(o => o.Channel == "Online" && o.Status != "Cancelled" && o.Status != "Returned").ToList();
        var ret = orders.Where(o => o.Channel == "Online" && o.Status == "Returned").ToList();

        decimal Sum(IEnumerable<OrderSummary> l, Func<OrderSummary, decimal> f) => l.Sum(f);

        var shopSale = Sum(shop, o => o.NetSale);
        var onlSale = Sum(onl, o => o.NetSale);
        var shopCost = Sum(shop, o => o.NetCost);
        var onlCost = Sum(onl, o => o.NetCost);
        var shopTax = Sum(shop, o => o.TaxAmount);
        var onlTax = Sum(onl, o => o.TaxAmount);
        var onlCod = Sum(onl, o => o.CourierCharge);
        var onlPack = Sum(onl, o => o.PackagingCharge);
        var shopOrderProfit = Sum(shop, o => o.Profit);
        var onlOrderProfit = Sum(onl, o => o.Profit);
        var retLoss = -Sum(ret, o => o.Profit);               // positive loss

        var shopExp = exp.Sum(x => x.ShopPart);
        var onlExp = exp.Sum(x => x.OnlinePart);

        var shopNet = shopOrderProfit - shopExp;
        var onlNet = onlOrderProfit - retLoss - onlExp;
        var net = shopNet + onlNet;

        var rows = new List<Row>
        {
            new() { Detail = $"Sale (orders: Shop {shop.Count}, Online {onl.Count})", Shop = shopSale, Online = onlSale },
            new() { Detail = "- Maal ki cost (kharid)", Shop = -shopCost, Online = -onlCost },
            new() { Detail = "- Tax", Shop = -shopTax, Online = -onlTax },
            new() { Detail = "- COD / courier charge", Shop = 0, Online = -onlCod },
            new() { Detail = "- Order packaging", Shop = 0, Online = -onlPack },
            new() { Detail = "ORDERS KA PROFIT", Shop = shopOrderProfit, Online = onlOrderProfit, Bold = true },
            new() { Detail = $"- Return loss ({ret.Count} orders: COD + wapsi charge)", Shop = 0, Online = -retLoss },
            new() { Detail = "- Kharche (Shop / Online / Shared ka hissa)", Shop = -shopExp, Online = -onlExp },
            new() { Detail = "NET PROFIT", Shop = shopNet, Online = onlNet, Bold = true },
        };

        gridSummary.DataSource = rows;
        gridSummary.Columns["Bold"]!.Visible = false;
        gridSummary.Columns["Detail"]!.HeaderText = "Hisab";
        gridSummary.Columns["Detail"]!.FillWeight = 160;
        gridSummary.Columns["Shop"]!.HeaderText = "SHOP";
        gridSummary.Columns["Online"]!.HeaderText = "ONLINE";
        gridSummary.Columns["Total"]!.HeaderText = "TOTAL";
        foreach (var c in new[] { "Shop", "Online", "Total" })
        {
            gridSummary.Columns[c]!.DefaultCellStyle.Format = "N0";
            gridSummary.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        gridSummary.ClearSelection();

        // Kharche category-wise
        var byCat = exp.GroupBy(x => new { x.GroupName, x.CategoryName })
            .Select(g => new
            {
                Group = g.Key.GroupName,
                Category = g.Key.CategoryName,
                Shop = g.Sum(x => x.ShopPart),
                Online = g.Sum(x => x.OnlinePart),
                Total = g.Sum(x => x.ExpenseAmount)
            })
            .Where(x => x.Total != 0)
            .OrderByDescending(x => x.Total).ToList();
        gridExp.DataSource = byCat;
        foreach (var c in new[] { "Shop", "Online", "Total" })
            if (gridExp.Columns.Contains(c))
            {
                gridExp.Columns[c]!.DefaultCellStyle.Format = "N0";
                gridExp.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        gridExp.ClearSelection();

        lblNet.Text = $"NET PROFIT: {net:N0}      (Shop {shopNet:N0}  |  Online {onlNet:N0})";
        lblNet.ForeColor = net >= 0 ? System.Drawing.Color.FromArgb(0, 110, 50) : System.Drawing.Color.Firebrick;

        var expected = onl.Where(o => o.IsExpected).Sum(o => o.Profit);
        var open = onl.Count(o => o.IsExpected);
        lblNote.Text = $"{from:dd-MMM-yyyy} se {to:dd-MMM-yyyy}.   Kharche mein advance shamil nahi, salary poori.   " +
                       (open > 0 ? $"Online ka {open} orders ({expected:N0} profit) abhi Delivered nahi hue, expected hain." : "");
    }

    private void gridSummary_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow r in gridSummary.Rows)
        {
            if (r.DataBoundItem is not Row x || !x.Bold) continue;
            r.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(235, 242, 255);
        }
    }
}