using ClothStock.Business.Services;
using ClothStock.Models.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Orders;

public partial class ShipmentForm : Form
{
    private const string CurrentUser = "owner";   // login banne par yahan asal user aaye ga

    private readonly ShipmentService _ship;
    private readonly OrderService _orders;
    private readonly CourierService _couriers;

    private int _shipmentId = 0;
    private string _status = "";
    private bool _loading = false;

    public ShipmentForm()
    {
        InitializeComponent();
        _ship = Program.Services.GetRequiredService<ShipmentService>();
        _orders = Program.Services.GetRequiredService<OrderService>();
        _couriers = Program.Services.GetRequiredService<CourierService>();
    }

    private ShipmentSummary? SelectedShipment =>
        gridShip.SelectedRows.Count > 0 ? gridShip.SelectedRows[0].DataBoundItem as ShipmentSummary : null;

    private async void ShipmentForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _loading = true;
            cmbFilter.Items.AddRange(new object[] { "Sab", "Placed", "Dispatched", "Delivered", "Returned", "Cancelled" });
            cmbFilter.SelectedIndex = 0;

            var list = (await _couriers.GetAllAsync(activeOnly: true)).ToList();
            cmbCourier.DisplayMember = "CourierName";
            cmbCourier.ValueMember = "CourierId";
            cmbCourier.DataSource = list;
            _loading = false;

            await ReloadAsync();
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void Filter_Changed(object? sender, EventArgs e)
    {
        if (_loading) return;
        await ReloadAsync();
    }

    private async void btnRefresh_Click(object? sender, EventArgs e) => await ReloadAsync(_shipmentId);

    private async void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await ReloadAsync();
        }
    }

    private async Task ReloadAsync(int selectId = 0)
    {
        try
        {
            _loading = true;

            var status = cmbFilter.SelectedIndex <= 0 ? null : cmbFilter.Text;
            var list = (await _ship.GetAllAsync(status, txtSearch.Text)).ToList();
            gridShip.DataSource = list;

            foreach (var c in new[] { "ShipmentId", "Notes", "CreatedBy", "CreatedAt", "CustomerName", "City" })
                if (gridShip.Columns.Contains(c)) gridShip.Columns[c]!.Visible = false;
            Hdr(gridShip, "TrackingId", "Tracking ID"); Hdr(gridShip, "CourierName", "Courier");
            Hdr(gridShip, "WeightKg", "Weight (kg)"); Hdr(gridShip, "CourierCharge", "Charge");
            Hdr(gridShip, "ReturnCharge", "Return Charge"); Hdr(gridShip, "DispatchDate", "Dispatch");
            Hdr(gridShip, "DeliveredDate", "Delivered"); Hdr(gridShip, "ReturnedDate", "Returned");
            Hdr(gridShip, "OrdersCount", "Orders"); Hdr(gridShip, "Suits", "Suits");
            foreach (var c in new[] { "DispatchDate", "DeliveredDate", "ReturnedDate" })
                if (gridShip.Columns.Contains(c)) gridShip.Columns[c]!.DefaultCellStyle.Format = "dd-MMM-yyyy";

            await LoadUnshippedAsync();

            var idx = selectId > 0 ? list.FindIndex(x => x.ShipmentId == selectId) : -1;
            if (idx >= 0)
            {
                gridShip.ClearSelection();
                gridShip.Rows[idx].Selected = true;
                var cell = gridShip.Rows[idx].Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                if (cell != null) gridShip.CurrentCell = cell;
            }
            else
            {
                gridShip.ClearSelection();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { _loading = false; }

        await LoadSelectedAsync();
    }

    private async Task LoadUnshippedAsync()
    {
        var un = (await _orders.GetUnshippedAsync()).ToList();
        gridUnshipped.DataSource = un;
        string[] keep = { "OrderNo", "CustomerName", "City", "Suits", "NetSale" };
        foreach (DataGridViewColumn c in gridUnshipped.Columns)
            c.Visible = keep.Contains(c.Name);
        Hdr(gridUnshipped, "OrderNo", "Order"); Hdr(gridUnshipped, "CustomerName", "Customer");
        Hdr(gridUnshipped, "NetSale", "Amount");
        if (gridUnshipped.Columns.Contains("NetSale")) gridUnshipped.Columns["NetSale"]!.DefaultCellStyle.Format = "N0";
        gridUnshipped.ClearSelection();
    }

    private static void Hdr(DataGridView g, string col, string text)
    {
        if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text;
    }

    private async void gridShip_SelectionChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        await LoadSelectedAsync();
    }

    private async Task LoadSelectedAsync()
    {
        try
        {
            var s = SelectedShipment;
            if (s == null)
            {
                SetNewMode();
                return;
            }

            var sh = await _ship.GetByIdAsync(s.ShipmentId);
            if (sh == null) { SetNewMode(); return; }

            _shipmentId = sh.ShipmentId;
            _status = sh.Status;
            cmbCourier.SelectedValue = sh.CourierId;
            txtTracking.Text = sh.TrackingId;
            numWeight.Value = Math.Min(sh.WeightKg, numWeight.Maximum);
            numCharge.Value = Math.Min(sh.CourierCharge, numCharge.Maximum);
            numReturn.Value = Math.Min(sh.ReturnCharge, numReturn.Maximum);
            txtNotes.Text = sh.Notes ?? "";
            grpDetails.Text = $"Parcel: {sh.TrackingId}  ({sh.Status})";

            var orders = (await _ship.GetOrdersAsync(sh.ShipmentId)).ToList();
            gridOrders.DataSource = orders;
            string[] keep = { "OrderNo", "CustomerName", "City", "Status", "Suits", "NetSale", "CourierShare", "ReturnShare", "Profit" };
            foreach (DataGridViewColumn c in gridOrders.Columns) c.Visible = keep.Contains(c.Name);
            Hdr(gridOrders, "OrderNo", "Order"); Hdr(gridOrders, "CustomerName", "Customer");
            Hdr(gridOrders, "NetSale", "Amount"); Hdr(gridOrders, "CourierShare", "Courier Hissa");
            Hdr(gridOrders, "ReturnShare", "Return Hissa");
            foreach (var c in new[] { "NetSale", "CourierShare", "ReturnShare", "Profit" })
                if (gridOrders.Columns.Contains(c)) gridOrders.Columns[c]!.DefaultCellStyle.Format = "N0";
            gridOrders.ClearSelection();

            lblInfo.Text = $"Orders: {orders.Count}   |   Suits: {orders.Sum(o => o.Suits):N0}   |   " +
                           $"Amount: {orders.Sum(o => o.NetSale):N0}   |   Profit/Loss jod: {orders.Sum(o => o.Profit):N0}";
            UpdateButtons();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetNewMode()
    {
        _shipmentId = 0;
        _status = "";
        txtTracking.Clear(); txtNotes.Clear();
        numWeight.Value = 0; numCharge.Value = 0; numReturn.Value = 0;
        gridOrders.DataSource = null;
        grpDetails.Text = "Naya parcel (neeche right se orders select karein)";
        lblInfo.Text = "";
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool has = _shipmentId > 0;
        btnDispatch.Enabled = has && _status == "Placed";
        btnDeliver.Enabled = has && _status == "Dispatched";
        btnReturn.Enabled = has && (_status == "Dispatched" || _status == "Delivered");
        btnCancelShip.Enabled = has && _status == "Placed";
        btnRemove.Enabled = has && _status == "Placed";
        btnAdd.Enabled = _shipmentId == 0 || _status == "Placed";
        btnAdd.Text = _shipmentId == 0 ? "(Naya parcel: orders select karke Save Parcel dabayen)" : "Selected Orders Parcel Mein Jorein";
        btnAdd.Enabled = has && _status == "Placed";
    }

    private List<int> SelectedUnshippedIds() =>
        gridUnshipped.SelectedRows.Cast<DataGridViewRow>()
            .Select(r => r.DataBoundItem).OfType<OrderSummary>().Select(o => o.OrderId).ToList();

    private void btnNew_Click(object? sender, EventArgs e)
    {
        _loading = true;
        gridShip.ClearSelection();
        _loading = false;
        SetNewMode();
        txtTracking.Focus();
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_shipmentId > 0 && (_status == "Cancelled"))
            {
                MessageBox.Show("Cancel hui parcel badli nahi ja sakti.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var req = new ShipmentSaveRequest
            {
                ShipmentId = _shipmentId,
                CourierId = cmbCourier.SelectedValue is int id ? id : 0,
                TrackingId = txtTracking.Text,
                WeightKg = numWeight.Value,
                CourierCharge = numCharge.Value,
                ReturnCharge = numReturn.Value,
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                CreatedBy = CurrentUser,
                OrderIds = _shipmentId == 0 ? SelectedUnshippedIds() : new List<int>()
            };

            btnSave.Enabled = false;
            var savedId = await _ship.SaveAsync(req);
            MessageBox.Show("Parcel save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await ReloadAsync(savedId);
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
                ? "Is courier ki ye Tracking ID pehle se maujood hai." : ex.Message;
            MessageBox.Show(msg, "Save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { btnSave.Enabled = true; }
    }

    private async void btnAdd_Click(object? sender, EventArgs e)
    {
        try
        {
            var ids = SelectedUnshippedIds();
            if (ids.Count == 0)
            {
                MessageBox.Show("Right side se order select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            await _ship.AddOrdersAsync(_shipmentId, ids, CurrentUser);
            await ReloadAsync(_shipmentId);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Order nahi jura", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnRemove_Click(object? sender, EventArgs e)
    {
        try
        {
            if (gridOrders.CurrentRow?.DataBoundItem is not OrderSummary o)
            {
                MessageBox.Show("Pehle left list se order select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            await _ship.RemoveOrderAsync(o.OrderId, CurrentUser);
            await ReloadAsync(_shipmentId);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Order nahi hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnDispatch_Click(object? sender, EventArgs e)
    {
        if (!Confirm("Parcel Dispatch karna hai?")) return;
        await Run(() => _ship.DispatchAsync(_shipmentId, null, CurrentUser));
    }

    private async void btnDeliver_Click(object? sender, EventArgs e)
    {
        if (!Confirm("Parcel Delivered mark karna hai?")) return;
        await Run(() => _ship.DeliverAsync(_shipmentId, null, CurrentUser));
    }

    private async void btnReturn_Click(object? sender, EventArgs e)
    {
        var msg = $"Parcel wapas aa gaya?\n\nReturn Charge: {numReturn.Value:N0}\n" +
                  "Maal usi batch mein wapas ho jaye ga, aur courier ka kata hua charge nuksan likha jaye ga.\n" +
                  "(Agar maal kharab hai to baad mein Stock Adjust se 'Kharab maal' likhein.)";
        if (!Confirm(msg)) return;
        await Run(() => _ship.ReturnAsync(_shipmentId, numReturn.Value, null, CurrentUser));
    }

    private async void btnCancelShip_Click(object? sender, EventArgs e)
    {
        if (!Confirm("Parcel cancel karna hai? Orders Placed hi rahen ge, bas parcel se alag ho jayen ge.")) return;
        await Run(() => _ship.CancelAsync(_shipmentId));
    }

    private static bool Confirm(string text) =>
        MessageBox.Show(text, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

    private async Task Run(Func<Task> action)
    {
        try
        {
            await action();
            await ReloadAsync(_shipmentId);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}