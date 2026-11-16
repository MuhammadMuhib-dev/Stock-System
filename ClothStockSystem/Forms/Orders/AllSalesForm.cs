using ClothStock.Business.Services;
using ClothStock.Models.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Orders;

public partial class AllSalesForm : Form
{
    private readonly OrderService _orders;
    private bool _ready = false;
    private bool _loading = false;

    public AllSalesForm()
    {
        InitializeComponent();
        _orders = Program.Services.GetRequiredService<OrderService>();

        cmbRange.Items.AddRange(new object[] { "Aaj", "Is mahine", "Custom", "Sab" });
        cmbRange.SelectedIndex = 0;
        cmbChannel.Items.AddRange(new object[] { "Sab", "Shop", "Online" });
        cmbChannel.SelectedIndex = 0;
        cmbStatus.Items.AddRange(new object[] { "Sab", "Sold", "Placed", "Dispatched", "Delivered", "Returned", "Cancelled" });
        cmbStatus.SelectedIndex = 0;
        dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpTo.Value = DateTime.Today;
        dtpFrom.Enabled = dtpTo.Enabled = false;
    }

    private OrderSummary? Selected => grid.CurrentRow?.DataBoundItem as OrderSummary;

    private async void AllSalesForm_Load(object? sender, EventArgs e)
    {
        _ready = true;
        await LoadAsync();
    }

    private async void cmbRange_Changed(object? sender, EventArgs e)
    {
        dtpFrom.Enabled = dtpTo.Enabled = cmbRange.SelectedIndex == 2;
        if (_ready) await LoadAsync();
    }

    private async void Filter_Changed(object? sender, EventArgs e)
    {
        if (_ready) await LoadAsync();
    }

    private async void btnSearch_Click(object? sender, EventArgs e) => await LoadAsync();

    private async void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await LoadAsync();
        }
    }

    private static void Hdr(DataGridView g, string col, string text)
    {
        if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text;
    }

    private async Task LoadAsync()
    {
        try
        {
            var f = new OrderFilter();
            if (cmbChannel.SelectedIndex > 0) f.Channel = cmbChannel.Text;
            if (cmbStatus.SelectedIndex > 0) f.Status = cmbStatus.Text;

            var s = txtSearch.Text.Trim();
            var searching = s.Length > 0;
            if (searching)
            {
                if (s.All(c => char.IsDigit(c) || c == '-' || c == ' ' || c == '+'))
                    s = new string(s.Where(char.IsDigit).ToArray());
                f.Search = s;      // search mein sab tareekhon mein dhoondhein
            }
            else
            {
                var today = DateTime.Today;
                switch (cmbRange.SelectedIndex)
                {
                    case 0: f.From = today; f.To = today; break;
                    case 1: f.From = new DateTime(today.Year, today.Month, 1); f.To = today; break;
                    case 2: f.From = dtpFrom.Value.Date; f.To = dtpTo.Value.Date; break;
                }
            }

            var list = (await _orders.GetOrdersAsync(f)).ToList();

            _loading = true;
            grid.DataSource = list;
            string[] keep = { "OrderNo", "OrderDate", "Channel", "CustomerName", "Phone", "City",
                              "CourierName", "TrackingId", "Status", "Suits", "NetSale", "Profit" };
            foreach (DataGridViewColumn c in grid.Columns) c.Visible = keep.Contains(c.Name);
            Hdr(grid, "OrderNo", "Order / Sale No"); Hdr(grid, "OrderDate", "Date");
            Hdr(grid, "CustomerName", "Customer"); Hdr(grid, "CourierName", "Courier");
            Hdr(grid, "TrackingId", "Tracking ID"); Hdr(grid, "NetSale", "Amount");
            Hdr(grid, "Profit", "Profit / Loss");
            if (grid.Columns.Contains("OrderDate")) grid.Columns["OrderDate"]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            foreach (var c in new[] { "NetSale", "Profit" })
                if (grid.Columns.Contains(c)) grid.Columns[c]!.DefaultCellStyle.Format = "N0";
            _loading = false;

            ShowSummary(list, searching);
            await LoadLinesAsync();
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowSummary(List<OrderSummary> list, bool searching)
    {
        var shop = list.Where(x => x.Channel == "Shop" && x.Status != "Cancelled").ToList();
        var online = list.Where(x => x.Channel == "Online" && x.Status != "Cancelled" && x.Status != "Returned").ToList();
        var returned = list.Where(x => x.Status == "Returned").ToList();
        var cancelled = list.Count(x => x.Status == "Cancelled");

        var shopAmt = shop.Sum(x => x.NetSale);
        var onlineAmt = online.Sum(x => x.NetSale);
        var shopProfit = shop.Sum(x => x.Profit);
        var onlineProfit = online.Sum(x => x.Profit);
        var codLoss = -returned.Sum(x => x.Profit);

        lblShop.Text = $"SHOP:     Sales {shop.Count:N0}   |   Suits {shop.Sum(x => x.Suits):N0}   |   Amount {shopAmt:N0}   |   Profit {shopProfit:N0}";
        lblOnline.Text = $"ONLINE:   Orders {online.Count:N0}   |   Suits {online.Sum(x => x.Suits):N0}   |   Amount {onlineAmt:N0}   |   Profit {onlineProfit:N0}";
        lblReturn.Text = $"RETURNED: Orders {returned.Count:N0}   |   COD / courier LOSS {codLoss:N0}" +
                         (cancelled > 0 ? $"        (Cancelled {cancelled}: ginti mein nahi)" : "");
        var net = shopProfit + onlineProfit - codLoss;
        lblGrand.Text = (searching ? "SEARCH KA " : "") +
                        $"GRAND TOTAL:  Amount {shopAmt + onlineAmt:N0}   |   Suits {shop.Sum(x => x.Suits) + online.Sum(x => x.Suits):N0}   |   Net Profit {net:N0}";
        lblGrand.ForeColor = net >= 0 ? System.Drawing.Color.FromArgb(0, 110, 50) : System.Drawing.Color.Firebrick;
    }

    private void grid_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow r in grid.Rows)
        {
            if (r.DataBoundItem is not OrderSummary o) continue;
            if (o.Status == "Cancelled") r.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
            else if (o.Status == "Returned") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 225, 225);
            else if (o.Status == "Delivered") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(225, 245, 225);
            else if (o.Channel == "Shop") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(235, 242, 255);
        }
    }

    private async void grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        await LoadLinesAsync();
    }

    private async Task LoadLinesAsync()
    {
        var o = Selected;
        if (o == null) { gridLines.DataSource = null; return; }

        var lines = (await _orders.GetLinesAsync(o.OrderId)).ToList();
        gridLines.DataSource = lines;
        string[] keep = { "ArticleName", "ColorName", "BatchNo", "Quantity", "ReturnedQty", "SellingPrice", "CostPrice", "LineSale" };
        foreach (DataGridViewColumn c in gridLines.Columns) c.Visible = keep.Contains(c.Name);
        Hdr(gridLines, "ArticleName", "Article"); Hdr(gridLines, "ColorName", "Color");
        Hdr(gridLines, "BatchNo", "Batch"); Hdr(gridLines, "Quantity", "Suits");
        Hdr(gridLines, "ReturnedQty", "Wapas"); Hdr(gridLines, "SellingPrice", "Price/Suit");
        Hdr(gridLines, "CostPrice", "Buy/Suit"); Hdr(gridLines, "LineSale", "Line Total");
        foreach (var c in new[] { "SellingPrice", "CostPrice", "LineSale" })
            if (gridLines.Columns.Contains(c)) gridLines.Columns[c]!.DefaultCellStyle.Format = "N0";
    }
}