using System.ComponentModel;
using ClothStock.Business.Services;
using ClothStock.Models.Orders;
using ClothStock.Models.Stock;
using Microsoft.Extensions.DependencyInjection;
using ClothStockSystem.Helpers;
using ClothStock.Models.Finance;

namespace ClothStockSystem.Forms.Orders;

public partial class ShopSaleForm : Form
{
    private static string CurrentUser => SessionManager.Username;   // login banne par yahan asal user aaye ga
    private const string BasisSuit = "Per Suit";
    private const string BasisBunch = "Per Bunch";
    private const string BasisTotal = "Poori line ka total";

    private readonly OrderService _orders;
    private readonly StockService _stock;
    private readonly ArticleService _articles;

    private readonly SettingsService _settings;
    private readonly FinanceService _finance;
    private readonly ExpenseSetupService _setup;
    private CustomerBalance? _customer;
    private string _checkoutKey = Guid.NewGuid().ToString("N");
    private Button btnPick = null!;
    private Label lblCustInfo = null!;

    private bool _ready = false;
    private bool _loading = false;
    private bool _salesLoading = false;
    private decimal _taxPercent = 0;
    private bool _allowBelowCost = false;
    private List<StockBatchView> _batches = new();
    private readonly BindingList<LineVm> _lines = new();

    // Edit mode
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
        public decimal Price { get; set; }          // per suit
        public decimal BuyPerSuit { get; set; }
        public decimal Total => Suits * Price;
        public decimal EstCost { get; set; }
        public decimal ListPrice { get; set; }      // article ka asal rate (bachat dikhane ke liye)
    }

    public ShopSaleForm()
    {
        InitializeComponent();
        _orders = Program.Services.GetRequiredService<OrderService>();
        _stock = Program.Services.GetRequiredService<StockService>();
        _articles = Program.Services.GetRequiredService<ArticleService>();
        _settings = Program.Services.GetRequiredService<SettingsService>();

        // Sales List ke neeche "Receipt Print" button
        var btnPrint = new Button
        {
            Text = "Receipt Print",
            Size = new Size(170, 34),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(pnlSummary.Width - 190, 8)
        };
        btnPrint.Click += btnPrintReceipt_Click;
        pnlSummary.Controls.Add(btnPrint);

        var btnRet = new Button
        {
            Text = "Return / Refund",
            Size = new Size(170, 34),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(pnlSummary.Width - 380, 8),
            BackColor = System.Drawing.Color.FromArgb(200, 40, 40),
            ForeColor = System.Drawing.Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnRet.Click += btnReturnSale_Click;
        pnlSummary.Controls.Add(btnRet);

        var btnRep = new Button
        {
            Text = "Report Print",
            Size = new Size(170, 34),
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            Location = new Point(pnlSummary.Width - 570, 8)
        };
       // btnRep.Click += btnReportPrint_Click;
        pnlSummary.Controls.Add(btnRep);

        grid.DataSource = _lines;
        cmbUnit.Items.AddRange(new object[] { "Suit", "Bunch" });
        cmbUnit.SelectedIndex = 0;
        cmbBasis.Items.AddRange(new object[] { BasisSuit, BasisBunch, BasisTotal });
        cmbBasis.SelectedIndex = 0;
        cmbRange.Items.AddRange(new object[] { "Aaj", "Is mahine", "Sab" });
        cmbRange.SelectedIndex = 1;
        _finance = Program.Services.GetRequiredService<FinanceService>();
        _setup = Program.Services.GetRequiredService<ExpenseSetupService>();

        btnPick = new Button
        {
            Text = "Customer chunein (udhaar ke liye)",
            Location = new Point(880, 8),
            Size = new Size(210, 38),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        btnPick.Click += btnPick_Click;
        lblCustInfo = new Label
        {
            Location = new Point(880, 50),
            Size = new Size(210, 44),
            ForeColor = System.Drawing.Color.DimGray,
            Font = new System.Drawing.Font("Segoe UI", 8.5F),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        pnlBottom.Controls.Add(btnPick);
        pnlBottom.Controls.Add(lblCustInfo);
    }

    private Article? SelectedArticle => cmbArticle.SelectedItem as Article;
    private int SelectedColorId => cmbColor.SelectedValue is int id ? id : 0;
    private OrderSummary? SelectedSale => gridSales.CurrentRow?.DataBoundItem as OrderSummary;

    // ---------------------------------------------------------------- load

    private async void ShopSaleForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _taxPercent = await _orders.GetTaxPercentAsync();
            _allowBelowCost = await _orders.GetAllowBelowCostAsync();

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
            await LoadSalesAsync();
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
        Hdr(grid, "Detail", "Qty"); Hdr(grid, "Suits", "Suits"); Hdr(grid, "Price", "Price/Suit");
        Hdr(grid, "BuyPerSuit", "Buy/Suit"); Hdr(grid, "Total", "Line Total");
        foreach (var c in new[] { "Price", "BuyPerSuit", "Total" })
            if (grid.Columns.Contains(c)) grid.Columns[c]!.DefaultCellStyle.Format = "N0";
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
                lblStock.Text = "This article has no colors";
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
                Text = $"{b.BatchNo} | {b.ReceivedDate:dd-MMM-yyyy} | {b.SuitsRemaining} suits | cost {b.CostPricePerSuit:N0}"
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

    // Qty ko suits mein badalna (Bunch ho to suits per bunch se)
    private bool TryGetSuits(out int suits)
    {
        suits = (int)numQty.Value;
        if (cmbUnit.SelectedIndex != 1) return true;
        var rb = RefBatch();
        if (rb == null || rb.SuitsPerBunch <= 0) return false;
        suits *= rb.SuitsPerBunch;
        return true;
    }

    // Jo bhav likha (suit / bunch / total) us se per suit price
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

    // Article ki rate ko bhav ke hisab se default bana dena
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
        cmbBasis.SelectedIndex = cmbUnit.SelectedIndex == 1 ? 1 : 0;   // Bunch par Per Bunch bhav
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
            lblCost.Text = "Stock not available for this color.";
            lblStock.Text = "";
            return;
        }

        // Shopkeeper ke liye kharid (buy) price
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
        lblCost.Text = txt;

        if (!TryGetSuits(out var suits) || !TryPerSuit(suits, out var perSuit))
        {
            lblStock.ForeColor = System.Drawing.Color.Firebrick;
            lblStock.Text = "Bunch calculation not possible (batch not found).";
            return;
        }

        var batchId = cmbBatch.SelectedValue as int?;
        var avail = AvailableFor(SelectedColorId, batchId);
        var used = AlreadyUsed(SelectedColorId, batchId);
        var stockOk = suits + used <= avail;
        var buy = Math.Round(EstimateCost(batchId, suits) / suits, 2);
        var below = perSuit < buy;

        var head = isBunch ? $"{numQty.Value:N0} bunch = {suits:N0} suits" : $"{suits:N0} suits";
        var msg = $"{head}  |  Per suit: {perSuit:N0}  |  Line total: {perSuit * suits:N0}  |  Margin/suit: {perSuit - buy:N0}  |  " +
                  (stockOk ? $"Maujood: {avail:N0} suits" : $"STOCK KAM! Maujood sirf {avail:N0} suits");
        if (below) msg += "  |  LOSS! Cost se kam bhav";

        lblStock.ForeColor = stockOk && !below ? System.Drawing.Color.DimGray : System.Drawing.Color.Firebrick;
        lblStock.Text = msg;
    }

    // ---------------------------------------------------------------- lines

    private void btnAddItem_Click(object? sender, EventArgs e)
    {
        var art = SelectedArticle;
        if (SelectedColorId == 0 || art == null)
        {
            MessageBox.Show("Please select an article and color.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!TryGetSuits(out var suits) || suits <= 0 || !TryPerSuit(suits, out var perSuit))
        {
            MessageBox.Show("Batch not found for this color, bunch calculation not possible.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var batchId = cmbBatch.SelectedValue as int?;
        var avail = AvailableFor(SelectedColorId, batchId);
        var used = AlreadyUsed(SelectedColorId, batchId);
        if (suits + used > avail)
        {
            MessageBox.Show($"Not enough stock. Available {avail - used} suits.", "Stock insufficient",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var cost = EstimateCost(batchId, suits);
        var buy = Math.Round(cost / suits, 2);

        // Minimum limit: kharid (cost) se kam par nahi
        if (perSuit < buy)
        {
            var text = $"Is bhav par LOSS hai.\n\nKharid (cost): {buy:N0} per suit\nAap ka bhav: {perSuit:N0} per suit\n";
            if (!_allowBelowCost)
            {
                MessageBox.Show(text + "\nSale below cost is not allowed (minimum limit).\n" +
                                "Ijazat ke liye Settings mein AllowSaleBelowCost = 1 karein.",
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
            EstCost = cost,
            ListPrice = art.SellingPrice
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

    private void UpdateTotals()
    {
        var sale = _lines.Sum(l => l.Total);
        var cost = _lines.Sum(l => l.EstCost);
        var tax = sale * _taxPercent / 100m;
        var profit = _orders.ExpectedProfit(sale, cost, _taxPercent, 0);

        lblGrand.Text = $"SALE TOTAL: {sale:N0}     ({_lines.Count} lines, {_lines.Sum(l => l.Suits):N0} suits)";
        lblTotals.Text = $"Cost (kharid): {cost:N0}   |   Tax ({_taxPercent:0.##}%): {tax:N0}";
        lblProfit.Text = $"Profit: {profit:N0}";
        lblProfit.ForeColor = profit >= 0 ? System.Drawing.Color.DarkGreen : System.Drawing.Color.Firebrick;
    }

    private void SetMode()
    {
        var edit = _editOrderId > 0;
        lblMode.Text = edit
            ? $"EDIT MODE: {_editOrderNo}  (on update, old items will be returned and new items will be deducted)"
            : "Nayi sale";
        lblMode.ForeColor = edit ? System.Drawing.Color.DarkOrange : System.Drawing.Color.DimGray;
        btnSave.Text = edit ? "Checkout (Update)" : "Checkout (Sale Confirm)";
        btnClear.Text = edit ? "Edit Cancel" : "Clear";
    }

    // ---------------------------------------------------------------- customer

    private void SetCustomer(CustomerBalance? c)
    {
        _customer = c;
        if (c == null)
        {
            txtName.ReadOnly = false; txtPhone.ReadOnly = false;
            btnPick.Text = "Customer chunein (udhaar ke liye)";
            lblCustInfo.Text = "";
            return;
        }
        txtName.Text = c.CustomerName;
        txtPhone.Text = c.Phone ?? "";
        txtName.ReadOnly = true; txtPhone.ReadOnly = true;
        btnPick.Text = "Customer badlein / hatayen";
        lblCustInfo.Text = $"Baqi: {c.Balance:N0}  |  Limit: {c.CreditLimit:N0}";
    }

    private void btnPick_Click(object? sender, EventArgs e)
    {
        using var dlg = new CustomerPickerDialog(_finance);
        if (dlg.ShowDialog(this) == DialogResult.OK) SetCustomer(dlg.Selected);
    }

    // ---------------------------------------------------------------- save

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_lines.Count == 0)
            {
                MessageBox.Show("Please add at least one item.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var items = _lines.Select(l => new OrderItemRequest
            {
                ColorId = l.ColorId,
                Quantity = l.Suits,
                SellingPrice = l.Price,
                BatchId = l.BatchId
            }).ToList();

            var name = string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim();
            var phone = _customer != null ? _customer.Phone : OrderService.NormalizePhone(txtPhone.Text);
            var total = _lines.Sum(l => l.Total);
            var suits = _lines.Sum(l => l.Suits);

            var accounts = (await _finance.GetAccountsAsync()).ToList();
            var methods = (await _setup.GetMethodsAsync(true)).Select(m => m.MethodName).ToList();
            var days = await _finance.GetDefaultCreditDaysAsync();
            if (accounts.Count == 0 || methods.Count == 0)
            {
                MessageBox.Show("No active account or payment method found.", "Check",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Customer ka taaza balance (checkout mein sahi dikhane ke liye)
            if (_customer != null) _customer = await _finance.GetCustomerAsync(_customer.CustomerId);

            using var dlg = new CheckoutDialog(total, suits, _customer, accounts, methods, days, SessionManager.IsOwner);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            btnSave.Enabled = false;

            var orderId = await _finance.CheckoutShopSaleAsync(new ShopCheckoutRequest
            {
                OrderId = _editOrderId,
                CustomerId = _customer?.CustomerId,
                CustomerName = name,
                Phone = phone,
                Items = items,
                AmountPaid = dlg.AmountPaid,
                AccountId = dlg.AccountId,
                Method = dlg.Method,
                DueDate = dlg.DueDate,
                Key = _editOrderId == 0 ? _checkoutKey : null,
                User = CurrentUser
            });

            var paid = dlg.AmountPaid;

            if (dlg.PrintReceipt)
            {
                try
                {
                    var d = await ReceiptPrinter.CreateAsync(_settings);
                    d.ReceiptNo = $"ORD-{orderId:D6}";
                    d.CustomerName = name ?? "";
                    d.CustomerPhone = phone ?? "";
                    d.Paid = paid;
                    d.DueDate = dlg.DueDate;
                    d.CashierName = CurrentUser;
                    if (paid > 0)
                    {
                        d.PaymentMethod = dlg.Method;
                        d.PaymentAccount = accounts.FirstOrDefault(a => a.AccountId == dlg.AccountId)?.AccountName ?? "";
                    }
                    d.Lines = _lines.Select(l => new ReceiptLine
                    {
                        Name = $"{l.Article} - {l.Color}",
                        Qty = l.Suits,
                        Price = l.Price,
                        ListPrice = l.ListPrice,
                        Note = ItemNote(l.Detail, l.Batch)
                    }).ToList();
                    ReceiptPrinter.Print(d, this);
                }
                catch (Exception pex)
                {
                    MessageBox.Show("Sale saved, but receipt could not be printed:\n" + pex.Message +
                                    "\n\nSales List se 'Receipt Print' dobara try kar sakte hain.",
                        "Print", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            MessageBox.Show(paid >= total
                    ? $"Sale confirmed.\n\nTotal: {total:N0}\nPaid: {paid:N0}\nChange to return: {paid - total:N0}"
                    : $"Sale confirm ho gayi (udhaar).\n\nTotal: {total:N0}\nAbhi liya: {paid:N0}\nBaqi udhaar: {total - paid:N0}\nDue date: {dlg.DueDate:dd-MMM-yyyy}",
                "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearAll();
            await LoadBatchesAsync();
            await LoadSalesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Sale could not be saved", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            await LoadBatchesAsync();
        }
        finally { btnSave.Enabled = true; }
    }

    // Item ke neeche chhota detail: bunch ka hisab aur batch (FIFO ho to batch nahi likhte)
    private static string ItemNote(string detail, string batch)
    {
        var parts = new List<string>();
        if (detail.Contains("bunch", StringComparison.OrdinalIgnoreCase)) parts.Add(detail);
        if (!string.IsNullOrWhiteSpace(batch) && !batch.Equals("FIFO", StringComparison.OrdinalIgnoreCase))
            parts.Add("Batch " + batch);
        return string.Join("  |  ", parts);
    }

    private void btnClear_Click(object? sender, EventArgs e) => ClearAll();

    private void ClearAll()
    {
        SetCustomer(null);
        _checkoutKey = Guid.NewGuid().ToString("N");
        txtName.Clear(); txtPhone.Clear();
        _lines.Clear();
        _origByColor.Clear(); _origByBatch.Clear();
        _editOrderId = 0; _editOrderNo = "";
        numQty.Value = 1;
        SetMode();
        UpdateTotals();
        RefreshHints();
    }

    // ---------------------------------------------------------------- sales list

    private void tabs_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_ready && tabs.SelectedTab == tabList) _ = LoadSalesAsync();
    }

    private async void cmbRange_Changed(object? sender, EventArgs e)
    {
        if (_ready) await LoadSalesAsync();
    }

    private async void btnSearch_Click(object? sender, EventArgs e) => await LoadSalesAsync();

    private async void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await LoadSalesAsync();
        }
    }

    private async Task LoadSalesAsync()
    {
        try
        {
            var f = new OrderFilter { Channel = "Shop" };
            var s = txtSearch.Text.Trim();
            var searching = s.Length > 0;

            if (searching)
            {
                // Phone likha ho (dash ya space ke sath bhi) to sirf digits
                if (s.All(c => char.IsDigit(c) || c == '-' || c == ' ' || c == '+'))
                    s = new string(s.Where(char.IsDigit).ToArray());
                f.Search = s;                        // search mein sab tareekhon mein dhoondhein
            }
            else
            {
                var today = DateTime.Today;
                if (cmbRange.SelectedIndex == 0) { f.From = today; f.To = today; }
                else if (cmbRange.SelectedIndex == 1) { f.From = new DateTime(today.Year, today.Month, 1); f.To = today; }
            }

            var list = (await _orders.GetOrdersAsync(f)).ToList();

            _salesLoading = true;
            gridSales.DataSource = list;
            string[] keep = { "OrderNo", "OrderDate", "CustomerName", "Phone", "Status", "Suits", "NetSale", "Profit" };
            foreach (DataGridViewColumn c in gridSales.Columns) c.Visible = keep.Contains(c.Name);
            Hdr(gridSales, "OrderNo", "Sale No"); Hdr(gridSales, "OrderDate", "Date");
            Hdr(gridSales, "CustomerName", "Customer"); Hdr(gridSales, "NetSale", "Amount");
            if (gridSales.Columns.Contains("OrderDate")) gridSales.Columns["OrderDate"]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            foreach (var c in new[] { "NetSale", "Profit" })
                if (gridSales.Columns.Contains(c)) gridSales.Columns[c]!.DefaultCellStyle.Format = "N0";
            _salesLoading = false;

            var ok = list.Where(x => x.Status != "Cancelled").ToList();
            var cancelled = list.Count - ok.Count;
            lblSummary.Text =
                (searching ? "Search ka total:  " : "Total:  ") +
                $"Sales: {ok.Count:N0}   |   Suits: {ok.Sum(x => x.Suits):N0}   |   Amount: {ok.Sum(x => x.NetSale):N0}   |   Profit: {ok.Sum(x => x.Profit):N0}" +
                (cancelled > 0 ? $"   |   Cancelled (not counted): {cancelled}" : "");

            await LoadSaleLinesAsync();
        }
        catch (Exception ex)
        {
            _salesLoading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void gridSales_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow r in gridSales.Rows)
            if (r.DataBoundItem is OrderSummary o && o.Status == "Cancelled")
                r.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
    }

    private async void gridSales_SelectionChanged(object? sender, EventArgs e)
    {
        if (_salesLoading) return;
        await LoadSaleLinesAsync();
    }

    private async Task LoadSaleLinesAsync()
    {
        var o = SelectedSale;
        if (o == null) { gridSaleLines.DataSource = null; return; }

        var lines = (await _orders.GetLinesAsync(o.OrderId)).ToList();
        gridSaleLines.DataSource = lines;
        string[] keep = { "ArticleName", "ColorName", "BatchNo", "Quantity", "SellingPrice", "CostPrice", "LineSale" };
        foreach (DataGridViewColumn c in gridSaleLines.Columns) c.Visible = keep.Contains(c.Name);
        Hdr(gridSaleLines, "ArticleName", "Article"); Hdr(gridSaleLines, "ColorName", "Color");
        Hdr(gridSaleLines, "BatchNo", "Batch"); Hdr(gridSaleLines, "Quantity", "Suits");
        Hdr(gridSaleLines, "SellingPrice", "Price/Suit"); Hdr(gridSaleLines, "CostPrice", "Buy/Suit");
        Hdr(gridSaleLines, "LineSale", "Line Total");
        foreach (var c in new[] { "SellingPrice", "CostPrice", "LineSale" })
            if (gridSaleLines.Columns.Contains(c)) gridSaleLines.Columns[c]!.DefaultCellStyle.Format = "N0";
    }

    private async void btnEdit_Click(object? sender, EventArgs e)
    {
        var o = SelectedSale;
        if (o == null)
        {
            MessageBox.Show("Please select a sale from the list first.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (o.ReturnedSuits > 0) { MessageBox.Show("This sale has returns, cannot edit or delete.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (o.Status != "Sold")
        {
            MessageBox.Show("Only Sold sales can be edited. Cancelled sales cannot be edited.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (_lines.Count > 0 && MessageBox.Show("Current sale being entered will be discarded. Start editing?",
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
            SetCustomer(o.CustomerId.HasValue ? await _finance.GetCustomerAsync(o.CustomerId.Value) : null);
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

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        var o = SelectedSale;
        if (o == null)
        {
            MessageBox.Show("Pehle list se sale select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (o.ReturnedSuits > 0) { MessageBox.Show("This sale has returns, cannot edit or delete.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        var confirm = MessageBox.Show(
            $"Sale {o.OrderNo} delete karni hai?\n\nCustomer: {o.CustomerName}\nSuits: {o.Suits}\nAmount: {o.NetSale:N0}\n\n" +
            "Stock will be returned. Entry will remain in list as Cancelled.",
            "Delete confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        try
        {
            btnDelete.Enabled = false;
            await _orders.CancelAsync(o.OrderId, CurrentUser);
            if (_editOrderId == o.OrderId) ClearAll();

            MessageBox.Show("Sale deleted, stock returned.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSalesAsync();
            await LoadBatchesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Could not delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { btnDelete.Enabled = true; }
    }

    private async void btnReturnSale_Click(object? sender, EventArgs e)
    {
        var o = SelectedSale;
        if (o == null) { MessageBox.Show("Please select a sale from the list first.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (o.Status != "Sold") { MessageBox.Show("Sirf Sold sale ki return hoti hai.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        try
        {
            var lines = (await _orders.GetLinesAsync(o.OrderId)).ToList();
            var paid = await _finance.GetOrderPaidAsync(o.OrderId);
            var cust = o.CustomerId.HasValue ? await _finance.GetCustomerAsync(o.CustomerId.Value) : null;
            var accounts = (await _finance.GetAccountsAsync()).ToList();
            var methods = (await _setup.GetMethodsAsync(true)).Select(m => m.MethodName).ToList();

            using var dlg = new ShopReturnDialog(o, lines, paid, cust, accounts, methods);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            await _finance.ReturnShopSaleAsync(new ShopReturnRequest
            {
                OrderId = o.OrderId,
                Items = dlg.Items,
                RefundAmount = dlg.RefundAmount,
                AccountId = dlg.RefundAmount > 0 ? dlg.AccountId : null,
                Method = dlg.RefundAmount > 0 ? dlg.Method : null,
                Notes = dlg.Notes,
                Key = dlg.Key,
                User = CurrentUser
            });

            MessageBox.Show("Return completed, stock returned.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadSalesAsync();
            await LoadBatchesAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Return nahi hui", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    // Jo list abhi screen par hai (Aaj / Is mahine / Sab / search) usi ka summary print karta hai
    /* private async void btnReportPrint_Click(object? sender, EventArgs e)
     {
         try
         {
             if (gridSales.DataSource is not List<OrderSummary> orders)
             {
                 MessageBox.Show("Pehle Sales List kholein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
             }

             var today = DateTime.Today;
             var search = txtSearch.Text.Trim();
             var period = search.Length > 0 ? $"Search: {search}"
                 : cmbRange.SelectedIndex == 0 ? $"Aaj ({today:dd-MMM-yyyy})"
                 : cmbRange.SelectedIndex == 1 ? $"{new DateTime(today.Year, today.Month, 1):dd-MMM-yyyy} se {today:dd-MMM-yyyy}"
                 : "Sab sales";

             var shop = await ReceiptPrinter.CreateAsync(_settings);
             ReportPrinter.Print(new SalesReportData
             {
                 Shop = shop,
                 Period = period,
                 Orders = orders,
                 ShowProfit = SessionManager.IsOwner,
                 PrintedBy = CurrentUser
             }, this, shop.Preview);
         }
         catch (Exception ex)
         {
             MessageBox.Show(ex.Message, "Report print nahi hui", MessageBoxButtons.OK, MessageBoxIcon.Warning);
         }
     }*/

    private async void btnPrintReceipt_Click(object? sender, EventArgs e)
    {
        var o = SelectedSale;
        if (o == null)
        {
            MessageBox.Show("Pehle list se sale select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (o.Status == "Cancelled")
        {
            MessageBox.Show("Ye sale cancel ho chuki hai, receipt nahi banti.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var lines = (await _orders.GetLinesAsync(o.OrderId)).ToList();
            var d = await ReceiptPrinter.CreateAsync(_settings);
            d.ReceiptNo = o.OrderNo;
            d.Date = o.CreatedAt;
            d.CustomerName = o.CustomerName ?? "";
            d.CustomerPhone = o.Phone ?? "";
            d.Lines = lines.Select(l => new ReceiptLine
            {
                Name = $"{l.ArticleName} - {l.ColorName}",
                Qty = l.Quantity,
                Price = l.SellingPrice,
                Note = ItemNote("", l.BatchNo)
            }).ToList();
            d.Paid = o.AmountPaid ?? d.Total;      // purani sale (paid save nahi thi) par paid = total
            d.Duplicate = true;
            ReceiptPrinter.Print(d, this, forcePreview: true);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Print nahi hui", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

}