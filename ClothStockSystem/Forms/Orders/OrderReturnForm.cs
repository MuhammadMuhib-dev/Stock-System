using ClothStock.Business.Services;
using ClothStock.Models.Orders;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Orders;

public partial class OrderReturnForm : Form
{
    private static string CurrentUser => SessionManager.Username;
    private const string ViewToReturn = "Return karne ke liye (Placed / Delivered)";
    private const string ViewReturned = "Return ho chuke (COD loss record)";

    private readonly OrderService _orders;
    private bool _ready = false;
    private bool _loading = false;

    public OrderReturnForm()
    {
        InitializeComponent();
        _orders = Program.Services.GetRequiredService<OrderService>();
        cmbView.Items.AddRange(new object[] { ViewToReturn, ViewReturned });
        cmbView.SelectedIndex = 0;
    }

    private bool History => cmbView.SelectedIndex == 1;

    private List<OrderSummary> SelectedOrders() =>
        grid.SelectedRows.Cast<DataGridViewRow>()
            .OrderBy(r => r.Index)
            .Select(r => r.DataBoundItem).OfType<OrderSummary>().ToList();

    private async void OrderReturnForm_Load(object? sender, EventArgs e)
    {
        _ready = true;
        await LoadAsync();
    }

    private async void cmbView_Changed(object? sender, EventArgs e)
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
            var f = new OrderFilter { Channel = "Online" };
            var s = txtSearch.Text.Trim();
            if (s.Length > 0)
            {
                if (s.All(c => char.IsDigit(c) || c == '-' || c == ' ' || c == '+'))
                    s = new string(s.Where(char.IsDigit).ToArray());
                f.Search = s;
            }
            if (History) f.Status = "Returned";

            var list = (await _orders.GetOrdersAsync(f)).ToList();
            if (!History)
                list = list.Where(x => x.Status is "Placed" or "Dispatched" or "Delivered").Take(300).ToList();

            _loading = true;
            grid.DataSource = list;

            string[] keep = History
                ? new[] { "OrderNo", "ReturnedDate", "CustomerName", "Phone", "City", "CourierName", "TrackingId", "Suits", "CourierCharge", "ReturnCharge", "Profit" }
                : new[] { "OrderNo", "OrderDate", "CustomerName", "Phone", "City", "CourierName", "TrackingId", "Status", "Suits", "NetSale", "CourierCharge" };
            foreach (DataGridViewColumn c in grid.Columns) c.Visible = keep.Contains(c.Name);

            Hdr(grid, "OrderNo", "Order No"); Hdr(grid, "OrderDate", "Date"); Hdr(grid, "ReturnedDate", "Return Date");
            Hdr(grid, "CustomerName", "Customer"); Hdr(grid, "CourierName", "Courier"); Hdr(grid, "TrackingId", "Tracking ID");
            Hdr(grid, "NetSale", "Amount"); Hdr(grid, "CourierCharge", "COD/Courier charge");
            Hdr(grid, "ReturnCharge", "Wapsi charge"); Hdr(grid, "Profit", "Loss (minus)");
            foreach (var c in new[] { "OrderDate", "ReturnedDate" })
                if (grid.Columns.Contains(c)) grid.Columns[c]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            foreach (var c in new[] { "NetSale", "CourierCharge", "ReturnCharge", "Profit" })
                if (grid.Columns.Contains(c)) grid.Columns[c]!.DefaultCellStyle.Format = "N0";
            _loading = false;

            grid.ClearSelection();

            btnReturn.Enabled = !History;
            numExtra.Enabled = !History;
            if (History)
                lblSummary.Text = $"Return ho chuke orders: {list.Count:N0}   |   Suits: {list.Sum(x => x.Suits):N0}   |   " +
                                  $"Total COD / courier loss: {-list.Sum(x => x.Profit):N0}";
            else
                lblSummary.Text = $"Return ho sakne wale orders: {list.Count:N0}";

            UpdateInfo();
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (History) { lblInfo.Text = "Ye return ho chuke orders ki list hai. Loss = COD/courier charge + wapsi charge."; return; }

        var sel = SelectedOrders();
        if (sel.Count == 0)
        {
            lblInfo.Text = "Upar list se order select karein (Ctrl + click se kai)";
            return;
        }
        var cod = sel.Sum(o => o.CourierCharge);
        lblInfo.Text = $"Selected: {sel.Count} order   |   Suits: {sel.Sum(o => o.Suits):N0}   |   " +
                       $"COD/courier charge: {cod:N0}   |   Total LOSS: {cod + numExtra.Value:N0}";
    }

    private async void btnReturn_Click(object? sender, EventArgs e)
    {
        var sel = SelectedOrders();
        if (sel.Count == 0)
        {
            MessageBox.Show("Pehle list se order select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cod = sel.Sum(o => o.CourierCharge);
        var names = string.Join("\n", sel.Take(8).Select(o => $"{o.OrderNo}  |  {o.CustomerName}  |  {o.TrackingId}  |  {o.Suits} suits"));
        if (sel.Count > 8) names += $"\n... aur {sel.Count - 8} orders";

        var confirm = MessageBox.Show(
            $"{names}\n\nMaal stock mein usi batch mein wapas aa jaye ga.\n" +
            $"COD/courier charge: {cod:N0}\nWapsi ka extra charge: {numExtra.Value:N0}\n" +
            $"Total LOSS record hoga: {cod + numExtra.Value:N0}\n\nReturn karna hai?",
            "Return confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        try
        {
            btnReturn.Enabled = false;
            var done = 0;
            foreach (var o in sel)
            {
                // Extra charge sirf pehle order par (ek hi parcel ho to double na ginay)
                decimal? extra = done == 0 ? numExtra.Value : null;
                await _orders.ReturnOrderAsync(o.OrderId, extra, CurrentUser);
                done++;
            }

            MessageBox.Show($"{done} order return ho gaye, stock wapas aa gaya.\nCOD loss record ho gaya: {cod + numExtra.Value:N0}",
                "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            numExtra.Value = 0;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Return nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            await LoadAsync();
        }
        finally { btnReturn.Enabled = !History; }
    }
}