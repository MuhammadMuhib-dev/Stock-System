using System.ComponentModel;
using ClothStock.Business.Services;
using ClothStock.Models.Orders;
using ClothStock.Models.Stock;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Orders;

public partial class OnlineOrderForm : Form
{
    private static string CurrentUser => SessionManager.Username;
    private const string BasisSuit = "Per Suit";
    private const string BasisBunch = "Per Bunch";
    private const string BasisTotal = "Poori line ka total";

    private readonly OrderService _orders;
    private readonly StockService _stock;
    private readonly ArticleService _articles;
    private readonly CourierService _couriers;

    private bool _ready = false;
    private bool _loading = false;
    private bool _listLoading = false;
    private decimal _taxPercent = 0;
    private decimal _defaultPack = 0;
    private bool _allowBelowCost = false;
    private List<StockBatchView> _batches = new();
    private readonly BindingList<LineVm> _lines = new();

    private int _editOrderId = 0;
    private string _editOrderNo = "";
    private readonly Dictionary<int, int> _origByColor = new();
    private readonly Dictionary<int, int> _origByBatch = new();

    private class BatchOption
    {
        public int? BatchId { get; set; }
        public string Text { get; set; } = "";
    }

    private class LineVm
    {
        public int ColorId { get; set; }
        public int? BatchId { get; set; }
        public string Article { get; set; } = "";
        public string Color { get; set; } = "";
        public string Batch { get; set; } = "";
        public string Detail { get; set; } = "";
        public int Suits { get; set; }
        public decimal Price { get; set; }
        public decimal BuyPerSuit { get; set; }
        public decimal Total => Suits * Price;
        public decimal EstCost { get; set; }
    }

    public OnlineOrderForm()
    {
        InitializeComponent();
        _orders = Program.Services.GetRequiredService<OrderService>();
        _stock = Program.Services.GetRequiredService<StockService>();
        _articles = Program.Services.GetRequiredService<ArticleService>();
        _couriers = Program.Services.GetRequiredService<CourierService>();

        grid.DataSource = _lines;
        cmbUnit.Items.AddRange(new object[] { "Suit", "Bunch" });
        cmbUnit.SelectedIndex = 0;
        cmbBasis.Items.AddRange(new object[] { BasisSuit, BasisBunch, BasisTotal });
        cmbBasis.SelectedIndex = 0;
        cmbRange.Items.AddRange(new object[] { "Aaj", "Is mahine", "Sab" });
        cmbRange.SelectedIndex = 1;
    }

    private Article? SelectedArticle => cmbArticle.SelectedItem as Article;
    private int SelectedColorId => cmbColor.SelectedValue is int id ? id : 0;
    private OrderSummary? SelectedOrder => gridOrders.CurrentRow?.DataBoundItem as OrderSummary;

    // ---------------------------------------------------------------- load

    private async void OnlineOrderForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _taxPercent = await _orders.GetTaxPercentAsync();
            _defaultPack = await _orders.GetDefaultPackagingAsync();
            _allowBelowCost = await _orders.GetAllowBelowCostAsync();
            numPack.Value = Math.Min(_defaultPack, numPack.Maximum);

            var cl = (await _couriers.GetAllAsync(activeOnly: true)).ToList();
            cl.Insert(0, new Courier { CourierId = 0, CourierName = "-- baad mein --" });
            cmbCourier.DisplayMember = "CourierName";
            cmbCourier.ValueMember = "CourierId";
            cmbCourier.DataSource = cl;

            _loading = true;
            var list = (await _articles.GetArticlesAsync(activeOnly: true)).ToList();
            cmbArticle.DisplayMember = "ArticleName";
            cmbArticle.ValueMember = "ArticleId";
            cmbArticle.DataSource = list;
            _loading = false;

            await LoadColorsAsync();
            FormatGrid();
            _ready = true;
            SetDefaultPrice();
            RefreshHints();
            UpdateTotals();
            SetMode();
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void FormatGrid()
    {
        foreach (var c in new[] { "ColorId", "BatchId", "EstCost" })
            if (grid.Columns.Contains(c)) grid.Columns[c]!.Visible = false;
        Hdr(grid, "Detail", "Qty"); Hdr(grid, "Price", "Price/Suit");
        Hdr(grid, "BuyPerSuit", "Buy/Suit"); Hdr(grid, "Total", "Line Total");
        foreach (var c in new[] { "Price", "BuyPerSuit", "Total" })
            if (grid.Columns.Contains(c)) grid.Columns[c]!.DefaultCellStyle.Format = "N0";

        // Seller ko kharid ki cost nahi
        if (!SessionManager.CanSeeCost && grid.Columns.Contains("BuyPerSuit"))
            grid.Columns["BuyPerSuit"]!.Visible = false;
    }

    private static void Hdr(DataGridView g, string col, string text)
    {
        if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text;
    }

    private async void cmbArticle_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        await LoadColorsAsync();
    }

    private async void cmbColor_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        await LoadBatchesAsync();
    }

    private async Task LoadColorsAsync()
    {
        try
        {
            _loading = true;
            var a = SelectedArticle;
            if (a == null) { cmbColor.DataSource = null; return; }

            var colors = (await _articles.GetColorsAsync(a.ArticleId, activeOnly: true)).ToList();
            cmbColor.DisplayMember = "ColorName";
            cmbColor.ValueMember = "ColorId";
            cmbColor.DataSource = colors;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { _loading = false; }

        await LoadBatchesAsync();
    }

    private async Task LoadBatchesAsync()
    {
        try
        {
            cmbBatch.DataSource = null;
            _batches = new();
            if (SelectedColorId == 0)
            {
                lblCost.Text = "";
                lblStock.Text = "Is article ke colors nahi";
                return;
            }

            var result = await _stock.GetBatchesAsync(SelectedColorId);
            _batches = result.Batches.ToList();

            var options = new List<BatchOption>
            {
                new() { BatchId = null, Text = "FIFO (sab se purana batch)" }
            };
            options.AddRange(_batches.Select(b => new BatchOption
            {
                BatchId = b.BatchId,
                Text = $"{b.BatchNo} | {b.ReceivedDate:dd-MMM-yyyy} | {b.SuitsRemaining} suits" +
                       (SessionManager.CanSeeCost ? $" | cost {b.CostPricePerSuit:N0}" : "")
            }));

            cmbBatch.DisplayMember = "Text";
            cmbBatch.ValueMember = "BatchId";
            cmbBatch.DataSource = options;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        if (_ready) { SetDefaultPrice(); RefreshHints(); }
    }

    // ---------------------------------------------------------------- hisab

    private StockBatchView? RefBatch()
    {
        var id = cmbBatch.SelectedValue as int?;
        return id.HasValue
            ? _batches.FirstOrDefault(b => b.BatchId == id.Value)
            : _batches.OrderBy(b => b.ReceivedDate).ThenBy(b => b.BatchId).FirstOrDefault();
    }

    private bool TryGetSuits(out int suits)
    {
        suits = (int)numQty.Value;
        if (cmbUnit.SelectedIndex != 1) return true;
        var rb = RefBatch();
        if (rb == null || rb.SuitsPerBunch <= 0) return false;
        suits *= rb.SuitsPerBunch;
        return true;
    }

    private bool TryPerSuit(int suits, out decimal perSuit)
    {
        perSuit = 0;
        if (suits <= 0) return false;
        var basis = cmbBasis.SelectedItem as string;
        if (basis == BasisSuit) perSuit = numPrice.Value;
        else if (basis == BasisBunch)
        {
            var rb = RefBatch();
            if (rb == null || rb.SuitsPerBunch <= 0) return false;
            perSuit = Math.Round(numPrice.Value / rb.SuitsPerBunch, 2);
        }
        else perSuit = Math.Round(numPrice.Value / suits, 2);
        return true;
    }

    private int AvailableFor(int colorId, int? batchId)
    {
        if (batchId.HasValue)
        {
            var b = _batches.FirstOrDefault(x => x.BatchId == batchId.Value);
            return (b?.SuitsRemaining ?? 0) + _origByBatch.GetValueOrDefault(batchId.Value);
        }
        return _batches.Sum(x => x.SuitsRemaining) + _origByColor.GetValueOrDefault(colorId);
    }

    private int AlreadyUsed(int colorId, int? batchId) =>
        batchId.HasValue
            ? _lines.Where(l => l.BatchId == batchId).Sum(l => l.Suits)
            : _lines.Where(l => l.ColorId == colorId).Sum(l => l.Suits);

    private decimal EstimateCost(int? batchId, int qty)
    {
        if (batchId.HasValue)
        {
            var b = _batches.FirstOrDefault(x => x.BatchId == batchId.Value);
            return (b?.CostPricePerSuit ?? 0) * qty;
        }

        decimal cost = 0;
        var need = qty;
        var ordered = _batches.OrderBy(x => x.ReceivedDate).ThenBy(x => x.BatchId).ToList();
        foreach (var b in ordered)
        {
            if (need == 0) break;
            var take = Math.Min(need, b.SuitsRemaining);
            cost += take * b.CostPricePerSuit;
            need -= take;
        }
        if (need > 0 && ordered.Count > 0) cost += need * ordered[0].CostPricePerSuit;
        return cost;
    }

    private void SetDefaultPrice()
    {
        var a = SelectedArticle;
        if (a == null) return;
        var basis = cmbBasis.SelectedItem as string;
        decimal price = a.SellingPrice;

        if (basis == BasisBunch)
        {
            var rb = RefBatch();
            price = rb != null && rb.SuitsPerBunch > 0 ? a.SellingPrice * rb.SuitsPerBunch : a.SellingPrice;
        }
        else if (basis == BasisTotal)
        {
            price = TryGetSuits(out var s) ? a.SellingPrice * s : a.SellingPrice;
        }
        numPrice.Value = Math.Min(Math.Max(price, 0), numPrice.Maximum);
    }

    private void cmbUnit_Changed(object? sender, EventArgs e)
    {
        if (!_ready) return;
        cmbBasis.SelectedIndex = cmbUnit.SelectedIndex == 1 ? 1 : 0;
        SetDefaultPrice();
        RefreshHints();
    }

    private void cmbBasis_Changed(object? sender, EventArgs e)
    {
        if (!_ready) return;
        SetDefaultPrice();
        RefreshHints();
    }

    private void Hints_Changed(object? sender, EventArgs e) => RefreshHints();

    private void RefreshHints()
    {
        if (!_ready || _loading) return;

        var isBunch = cmbUnit.SelectedIndex == 1;
        var basis = cmbBasis.SelectedItem as string;
        lblQty.Text = isBunch ? "Bunches" : "Suits";
        lblPrice.Text = basis == BasisSuit ? "Price/Suit" : basis == BasisBunch ? "Price/Bunch" : "Total Bhav";

        var rb = RefBatch();
        var art = SelectedArticle;
        if (rb == null)
        {
            lblCost.Text = "Is color ka stock maujood nahi.";
            lblStock.Text = "";
            return;
        }

        var spb = rb.SuitsPerBunch;
        var txt = $"BUY (kharid): {rb.CostPricePerSuit:N0}/suit";
        if (spb > 0) txt += $" | {rb.CostPricePerSuit * spb:N0}/bunch ({spb} suits)";
        if (art != null)
        {
            txt += $"   ||   Article rate: {art.SellingPrice:N0}/suit";
            if (spb > 0) txt += $" | {art.SellingPrice * spb:N0}/bunch";
        }
        if (cmbBatch.SelectedValue as int? == null)
        {
            var costs = _batches.Select(b => b.CostPricePerSuit).Distinct().OrderBy(x => x).ToList();
            if (costs.Count > 1) txt += $"   ||   Batches ka cost: {costs.First():N0} se {costs.Last():N0}";
        }
        // Seller ko BUY / cost wali line nahi
        lblCost.Text = SessionManager.CanSeeCost ? txt : "";

        if (!TryGetSuits(out var suits) || !TryPerSuit(suits, out var perSuit))
        {
            lblStock.ForeColor = System.Drawing.Color.Firebrick;
            lblStock.Text = "Bunch ka hisab nahi ho sakta (batch nahi mila).";
            return;
        }

        var batchId = cmbBatch.SelectedValue as int?;
        var avail = AvailableFor(SelectedColorId, batchId);
        var used = AlreadyUsed(SelectedColorId, batchId);
        var stockOk = suits + used <= avail;
        var buy = Math.Round(EstimateCost(batchId, suits) / suits, 2);
        bool below = perSuit < buy;

        var head = isBunch ? $"{numQty.Value:N0} bunch = {suits:N0} suits" : $"{suits:N0} suits";
        var msg = $"{head}  |  Per suit: {perSuit:N0}  |  Line total: {perSuit * suits:N0}  |  Margin/suit: {perSuit - buy:N0}  |  " +
                  (stockOk ? $"Maujood: {avail:N0} suits" : $"STOCK KAM! Maujood sirf {avail:N0} suits");
        if (below) msg += "  |  LOSS! Cost se kam bhav";

        // Seller ke liye margin aur LOSS ka zikr nahi
        if (!SessionManager.CanSeeCost)
        {
            msg = $"{head}  |  Per suit: {perSuit:N0}  |  Line total: {perSuit * suits:N0}  |  " +
                  (stockOk ? $"Maujood: {avail:N0} suits" : $"STOCK KAM! Maujood sirf {avail:N0} suits");
            below = false;
        }

        lblStock.ForeColor = stockOk && !below ? System.Drawing.Color.DimGray : System.Drawing.Color.Firebrick;
        lblStock.Text = msg;
    }

    // ---------------------------------------------------------------- lines

    private void btnAddItem_Click(object? sender, EventArgs e)
    {
        var art = SelectedArticle;
        if (SelectedColorId == 0 || art == null)
        {
            MessageBox.Show("Article aur color select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!TryGetSuits(out var suits) || suits <= 0 || !TryPerSuit(suits, out var perSuit))
        {
            MessageBox.Show("Is color ka batch nahi mila, bunch ka hisab nahi ho sakta.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var batchId = cmbBatch.SelectedValue as int?;
        var avail = AvailableFor(SelectedColorId, batchId);
        var used = AlreadyUsed(SelectedColorId, batchId);
        if (suits + used > avail)
        {
            MessageBox.Show($"Itna stock nahi. Maujood {avail - used} suits hain.", "Stock kam hai",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cost = EstimateCost(batchId, suits);
        var buy = Math.Round(cost / suits, 2);

        if (perSuit < buy)
        {
            var text = SessionManager.CanSeeCost
                ? $"Is bhav par LOSS hai.\n\nKharid (cost): {buy:N0} per suit\nAap ka bhav: {perSuit:N0} per suit\n"
                : "Ye bhav minimum limit se kam hai. Owner se poochein.\n";

            if (!_allowBelowCost)
            {
                MessageBox.Show(text + "\nCost se kam bhav par order allowed nahi (minimum limit)." +
                                (SessionManager.IsOwner ? "\nIjazat ke liye Settings mein AllowSaleBelowCost = 1 karein." : ""),
                    "Minimum limit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show(text + "\nPhir bhi add karna hai?", "Loss", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) != DialogResult.Yes) return;
        }

        var isBunch = cmbUnit.SelectedIndex == 1;
        _lines.Add(new LineVm
        {
            ColorId = SelectedColorId,
            BatchId = batchId,
            Article = art.ArticleName,
            Color = cmbColor.Text,
            Batch = batchId.HasValue ? cmbBatch.Text.Split('|')[0].Trim() : "FIFO",
            Detail = isBunch ? $"{numQty.Value:N0} bunch ({suits} suits)" : $"{suits} suits",
            Suits = suits,
            Price = perSuit,
            BuyPerSuit = buy,
            EstCost = cost
        });

        numQty.Value = 1;
        UpdateTotals();
        RefreshHints();
    }

    private void btnRemove_Click(object? sender, EventArgs e)
    {
        if (grid.CurrentRow?.DataBoundItem is LineVm l)
        {
            _lines.Remove(l);
            UpdateTotals();
            RefreshHints();
        }
    }

    private void Totals_Changed(object? sender, EventArgs e) => UpdateTotals();

    private void UpdateTotals()
    {
        var sale = _lines.Sum(l => l.Total);
        var cost = _lines.Sum(l => l.EstCost);
        var tax = sale * _taxPercent / 100m;
        var profit = _orders.ExpectedProfit(sale, cost, _taxPercent, numPack.Value) - numCharge.Value;

        lblGrand.Text = $"ORDER TOTAL: {sale:N0}     ({_lines.Count} lines, {_lines.Sum(l => l.Suits):N0} suits)";
        lblTotals.Text = $"Cost (kharid): {cost:N0}   |   Tax ({_taxPercent:0.##}%): {tax:N0}   |   " +
                         $"Packaging: {numPack.Value:N0}   |   COD/Courier: {numCharge.Value:N0}";
        lblProfit.Text = $"Expected Profit: {profit:N0}";
        lblProfit.ForeColor = profit >= 0 ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Firebrick;

        // Seller ko cost, tax aur profit nahi
        if (!SessionManager.CanSeeCost)
        {
            lblTotals.Text = $"Packaging: {numPack.Value:N0}   |   COD/Courier: {numCharge.Value:N0}";
            lblProfit.Text = "";
        }
    }

    private void SetMode()
    {
        var edit = _editOrderId > 0;
        lblMode.Text = edit
            ? $"EDIT MODE: {_editOrderNo}  (update par purani lines ka maal wapas aur nayi lines ka maal kam hoga)"
            : "Naya order";
        lblMode.ForeColor = edit ? System.Drawing.Color.DarkOrange : System.Drawing.Color.DimGray;
        btnSave.Text = edit ? "Order Update Karein" : "Order Save Karein";
        btnClear.Text = edit ? "Edit Cancel" : "Clear";
    }

    // ---------------------------------------------------------------- save

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_lines.Count == 0)
            {
                MessageBox.Show("Kam az kam ek item jorein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var items = _lines.Select(l => new OrderItemRequest
            {
                ColorId = l.ColorId,
                Quantity = l.Suits,
                SellingPrice = l.Price,
                BatchId = l.BatchId
            }).ToList();

            int? courierId = cmbCourier.SelectedValue is int cid && cid > 0 ? cid : null;
            var track = string.IsNullOrWhiteSpace(txtTrack.Text) ? null : txtTrack.Text.Trim();
            var total = _lines.Sum(l => l.Total);

            btnSave.Enabled = false;

            if (_editOrderId > 0)
            {
                await _orders.UpdateOnlineOrderAsync(new OrderReplaceRequest
                {
                    OrderId = _editOrderId,
                    Channel = "Online",
                    CustomerName = txtName.Text.Trim(),
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text.Trim(),
                    City = txtCity.Text.Trim(),
                    PackagingCharge = numPack.Value,
                    CourierId = courierId,
                    TrackingId = track,
                    CourierCharge = numCharge.Value,
                    CreatedBy = CurrentUser,
                    Items = items
                });
                MessageBox.Show($"Order update ho gaya.\nOrder Total: {total:N0}", "Done",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                await _orders.CreateAsync(new OrderCreateRequest
                {
                    Channel = "Online",
                    CustomerName = txtName.Text.Trim(),
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text.Trim(),
                    City = txtCity.Text.Trim(),
                    PackagingCharge = numPack.Value,
                    CourierId = courierId,
                    TrackingId = track,
                    CourierCharge = numCharge.Value,
                    CreatedBy = CurrentUser,
                    Items = items
                });
                MessageBox.Show($"Order save ho gaya. Stock kam ho gaya.\nOrder Total: {total:N0}", "Done",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            ClearAll();
            await LoadBatchesAsync();
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("UX_Orders_TrackingId", StringComparison.OrdinalIgnoreCase)
                ? "Ye Tracking ID pehle se kisi order par lagi hai. Har order ki alag Tracking ID honi chahiye."
                : ex.Message;
            MessageBox.Show(msg, "Order save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            await LoadBatchesAsync();
        }
        finally { btnSave.Enabled = true; }
    }

    private void btnClear_Click(object? sender, EventArgs e) => ClearAll();

    private void ClearAll()
    {
        txtName.Clear(); txtPhone.Clear(); txtCity.Clear(); txtAddress.Clear(); txtTrack.Clear();
        numCharge.Value = 0;
        numPack.Value = Math.Min(_defaultPack, numPack.Maximum);
        if (cmbCourier.Items.Count > 0) cmbCourier.SelectedIndex = 0;
        _lines.Clear();
        _origByColor.Clear(); _origByBatch.Clear();
        _editOrderId = 0; _editOrderNo = "";
        numQty.Value = 1;
        SetMode();
        UpdateTotals();
        RefreshHints();
    }

    // Return wale menu mein jana
    private void btnReturns_Click(object? sender, EventArgs e)
    {
        if (ParentForm is MainForm main) main.OpenReturns();
        else MessageBox.Show("Left menu se Returns kholen.", "Returns", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ---------------------------------------------------------------- orders list

    private void tabs_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_ready && tabs.SelectedTab == tabList) _ = LoadOrdersAsync();
    }

    private async void cmbRange_Changed(object? sender, EventArgs e)
    {
        if (_ready) await LoadOrdersAsync();
    }

    private async void btnSearch_Click(object? sender, EventArgs e) => await LoadOrdersAsync();

    private async void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await LoadOrdersAsync();
        }
    }

    private async Task LoadOrdersAsync()
    {
        try
        {
            var f = new OrderFilter { Channel = "Online" };
            var s = txtSearch.Text.Trim();
            var searching = s.Length > 0;

            if (searching)
            {
                if (s.All(c => char.IsDigit(c) || c == '-' || c == ' ' || c == '+'))
                    s = new string(s.Where(char.IsDigit).ToArray());
                f.Search = s;
            }
            else
            {
                var today = DateTime.Today;
                if (cmbRange.SelectedIndex == 0) { f.From = today; f.To = today; }
                else if (cmbRange.SelectedIndex == 1) { f.From = new DateTime(today.Year, today.Month, 1); f.To = today; }
            }

            var list = (await _orders.GetOrdersAsync(f)).ToList();

            _listLoading = true;
            gridOrders.DataSource = list;
            string[] keep = { "OrderNo", "OrderDate", "CustomerName", "Phone", "City", "CourierName", "TrackingId",
                              "Status", "Suits", "NetSale", "CourierCharge", "Profit" };
            foreach (DataGridViewColumn c in gridOrders.Columns) c.Visible = keep.Contains(c.Name);

            // Seller ko Profit column nahi
            if (!SessionManager.CanSeeCost && gridOrders.Columns.Contains("Profit"))
                gridOrders.Columns["Profit"]!.Visible = false;

            Hdr(gridOrders, "OrderNo", "Order No"); Hdr(gridOrders, "OrderDate", "Date");
            Hdr(gridOrders, "CustomerName", "Customer"); Hdr(gridOrders, "CourierName", "Courier");
            Hdr(gridOrders, "TrackingId", "Tracking ID"); Hdr(gridOrders, "NetSale", "Amount");
            Hdr(gridOrders, "CourierCharge", "COD/Courier");
            if (gridOrders.Columns.Contains("OrderDate")) gridOrders.Columns["OrderDate"]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            foreach (var c in new[] { "NetSale", "CourierCharge", "Profit" })
                if (gridOrders.Columns.Contains(c)) gridOrders.Columns[c]!.DefaultCellStyle.Format = "N0";
            _listLoading = false;

            var ok = list.Where(x => x.Status != "Cancelled" && x.Status != "Returned").ToList();
            var returned = list.Where(x => x.Status == "Returned").ToList();
            var cancelled = list.Count(x => x.Status == "Cancelled");
            lblSummary.Text =
                (searching ? "Search ka total:  " : "Total:  ") +
                $"Orders: {ok.Count:N0}   |   Suits: {ok.Sum(x => x.Suits):N0}   |   Amount: {ok.Sum(x => x.NetSale):N0}" +
                (SessionManager.CanSeeCost ? $"   |   Profit: {ok.Sum(x => x.Profit):N0}" : "") +
                (returned.Count > 0
                    ? $"   |   Returned: {returned.Count}" +
                      (SessionManager.CanSeeCost ? $" (COD loss {-returned.Sum(x => x.Profit):N0})" : "")
                    : "") +
                (cancelled > 0 ? $"   |   Cancelled: {cancelled}" : "");

            await LoadLinesAsync();
        }
        catch (Exception ex)
        {
            _listLoading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void gridOrders_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow r in gridOrders.Rows)
        {
            if (r.DataBoundItem is not OrderSummary o) continue;
            if (o.Status == "Cancelled") r.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
            else if (o.Status == "Returned") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 225, 225);
            else if (o.Status == "Delivered") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(225, 245, 225);
        }
    }

    private async void gridOrders_SelectionChanged(object? sender, EventArgs e)
    {
        if (_listLoading) return;
        await LoadLinesAsync();
    }

    private async Task LoadLinesAsync()
    {
        var o = SelectedOrder;
        if (o == null) { gridLines.DataSource = null; return; }

        var lines = (await _orders.GetLinesAsync(o.OrderId)).ToList();
        gridLines.DataSource = lines;
        string[] keep = { "ArticleName", "ColorName", "BatchNo", "Quantity", "SellingPrice", "CostPrice", "LineSale" };
        foreach (DataGridViewColumn c in gridLines.Columns) c.Visible = keep.Contains(c.Name);

        // Seller ko CostPrice nahi
        if (!SessionManager.CanSeeCost && gridLines.Columns.Contains("CostPrice"))
            gridLines.Columns["CostPrice"]!.Visible = false;

        Hdr(gridLines, "ArticleName", "Article"); Hdr(gridLines, "ColorName", "Color");
        Hdr(gridLines, "BatchNo", "Batch"); Hdr(gridLines, "Quantity", "Suits");
        Hdr(gridLines, "SellingPrice", "Price/Suit"); Hdr(gridLines, "CostPrice", "Buy/Suit");
        Hdr(gridLines, "LineSale", "Line Total");
        foreach (var c in new[] { "SellingPrice", "CostPrice", "LineSale" })
            if (gridLines.Columns.Contains(c)) gridLines.Columns[c]!.DefaultCellStyle.Format = "N0";
    }

    private async void btnEdit_Click(object? sender, EventArgs e)
    {
        var o = SelectedOrder;
        if (o == null)
        {
            MessageBox.Show("Pehle list se order select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (o.Status != "Placed")
        {
            MessageBox.Show("Sirf Placed order edit ho sakta hai. Delivered, Returned ya Cancelled edit nahi hote.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_lines.Count > 0 && MessageBox.Show("Abhi jo order likh rahe hain wo hat jaye ga. Edit shuru karein?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        try
        {
            var lines = (await _orders.GetLinesAsync(o.OrderId)).ToList();

            _lines.Clear(); _origByColor.Clear(); _origByBatch.Clear();
            foreach (var l in lines)
            {
                _lines.Add(new LineVm
                {
                    ColorId = l.ColorId,
                    BatchId = l.BatchId,
                    Article = l.ArticleName,
                    Color = l.ColorName,
                    Batch = l.BatchNo,
                    Detail = $"{l.Quantity} suits",
                    Suits = l.Quantity,
                    Price = l.SellingPrice,
                    BuyPerSuit = l.CostPrice,
                    EstCost = l.CostPrice * l.Quantity
                });
                _origByColor[l.ColorId] = _origByColor.GetValueOrDefault(l.ColorId) + l.Quantity;
                _origByBatch[l.BatchId] = _origByBatch.GetValueOrDefault(l.BatchId) + l.Quantity;
            }

            txtName.Text = o.CustomerName ?? "";
            txtPhone.Text = o.Phone ?? "";
            txtCity.Text = o.City ?? "";
            txtAddress.Text = o.Address ?? "";
            txtTrack.Text = o.TrackingId ?? "";
            numPack.Value = Math.Min(o.PackagingCharge, numPack.Maximum);
            numCharge.Value = Math.Min(o.CourierCharge, numCharge.Maximum);
            cmbCourier.SelectedValue = o.CourierId ?? 0;

            _editOrderId = o.OrderId;
            _editOrderNo = o.OrderNo;

            SetMode();
            UpdateTotals();
            RefreshHints();
            tabs.SelectedTab = tabSale;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnDelivered_Click(object? sender, EventArgs e)
    {
        var o = SelectedOrder;
        if (o == null)
        {
            MessageBox.Show("Pehle list se order select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (o.Status != "Placed")
        {
            MessageBox.Show("Sirf Placed order Delivered ho sakta hai.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (MessageBox.Show($"Order {o.OrderNo} Delivered mark karna hai?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        try
        {
            await _orders.DeliverOrderAsync(o.OrderId, CurrentUser);
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        var o = SelectedOrder;
        if (o == null)
        {
            MessageBox.Show("Pehle list se order select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (o.Status != "Placed")
        {
            MessageBox.Show("Sirf Placed order delete ho sakta hai.\nParcel wapas aaya ho to Return button / Returns menu use karein (COD loss record hoga).",
                "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"Order {o.OrderNo} delete karna hai?\n\nCustomer: {o.CustomerName}\nSuits: {o.Suits}\nAmount: {o.NetSale:N0}\n\n" +
            "Stock wapas aa jaye ga. Entry list mein Cancelled ke taur par rahe gi.\n" +
            "(Parcel wapas aaya ho to Delete nahi, Return use karein, warna COD loss record nahi hoga.)",
            "Delete confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        try
        {
            btnDelete.Enabled = false;
            await _orders.CancelAsync(o.OrderId, CurrentUser);
            if (_editOrderId == o.OrderId) ClearAll();

            MessageBox.Show("Order delete ho gaya, stock wapas aa gaya.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadOrdersAsync();
            await LoadBatchesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Delete nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { btnDelete.Enabled = true; }
    }
    private void tabs_DrawItem(object sender, DrawItemEventArgs e)
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
    }
}