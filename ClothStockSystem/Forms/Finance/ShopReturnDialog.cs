using ClothStock.Models.Finance;
using ClothStock.Models.Orders;

namespace ClothStockSystem.Forms.Orders;

public class ShopReturnDialog : Form
{
    private readonly OrderSummary _order;
    private readonly decimal _paid;
    private readonly CustomerBalance? _cust;
    private readonly List<FinancialAccount> _accounts;

    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.CellSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = System.Drawing.Color.White,
        EditMode = DataGridViewEditMode.EditOnEnter
    };
    private readonly NumericUpDown _refund = new() { DecimalPlaces = 2, Maximum = 0, ThousandsSeparator = true, Width = 160, TextAlign = HorizontalAlignment.Right };
    private readonly ComboBox _method = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly ComboBox _acc = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, DisplayMember = "AccountName" };
    private readonly TextBox _notes = new() { Width = 320, MaxLength = 200, PlaceholderText = "Wajah (optional)" };
    private readonly Label _info = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(0, 70, 140) };
    private readonly Label _hint = new() { AutoSize = true, ForeColor = System.Drawing.Color.DimGray };
    private readonly Button _ok = new() { Text = "Return Karein", Width = 160, Height = 36, BackColor = System.Drawing.Color.FromArgb(200, 40, 40), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };
    private bool _busy;

    public string Key { get; } = Guid.NewGuid().ToString("N");
    public decimal RefundAmount => _refund.Value;
    public int AccountId => (_acc.SelectedItem as FinancialAccount)?.AccountId ?? 0;
    public string Method => _method.SelectedItem as string ?? "";
    public string? Notes => string.IsNullOrWhiteSpace(_notes.Text) ? null : _notes.Text.Trim();

    public List<ShopReturnItem> Items => _grid.Rows.Cast<DataGridViewRow>()
        .Where(r => r.Tag is OrderLine && Qty(r) > 0)
        .Select(r => new ShopReturnItem { OrderItemId = ((OrderLine)r.Tag!).OrderItemId, Qty = Qty(r) }).ToList();

    private static int Qty(DataGridViewRow r) => int.TryParse(Convert.ToString(r.Cells["Ret"].Value), out var q) ? q : 0;

    public ShopReturnDialog(OrderSummary order, List<OrderLine> lines, decimal paid, CustomerBalance? cust,
                            List<FinancialAccount> accounts, List<string> methods)
    {
        _order = order; _paid = paid; _cust = cust; _accounts = accounts;
        Text = $"Shop sale return: {order.OrderNo}";
        Font = new System.Drawing.Font("Segoe UI", 10.5F);
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1000, 680);

        _grid.Columns.Add("Item", "Item"); _grid.Columns.Add("Batch", "Batch");
        _grid.Columns.Add("Price", "Price/suit"); _grid.Columns.Add("Kept", "Abhi mere paas bika hua");
        _grid.Columns.Add("Ret", "Wapas kitne (likhein)");
        foreach (var c in new[] { "Item", "Batch", "Price", "Kept" }) _grid.Columns[c]!.ReadOnly = true;
        _grid.Columns["Ret"]!.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 252, 230);
        foreach (var l in lines.Where(x => x.KeptQty > 0))
        {
            var i = _grid.Rows.Add($"{l.ArticleName} - {l.ColorName}", l.BatchNo, l.SellingPrice.ToString("N0"), l.KeptQty, 0);
            _grid.Rows[i].Tag = l;
        }
        _grid.CellEndEdit += (s, e) => Recalc();
        _grid.CurrentCellDirtyStateChanged += (s, e) => { if (_grid.IsCurrentCellDirty) _grid.CommitEdit(DataGridViewDataErrorContexts.Commit); };

        var top = new Label
        {
            Dock = DockStyle.Top,
            Height = 44,
            Padding = new Padding(8, 10, 0, 0),
            Text = $"{order.OrderNo}  |  {order.CustomerName ?? "walk-in"}  |  Customer ne diye: {paid:N0}.  \"Wapas kitne\" mein har item ki quantity likhein."
        };

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 170, Padding = new Padding(8), WrapContents = true };
        bottom.Controls.Add(_info); bottom.SetFlowBreak(_info, true);
        Label L(string t) => new() { Text = t, AutoSize = true, Margin = new Padding(8, 8, 3, 0) };
        bottom.Controls.Add(L("Refund (paisa wapas)")); bottom.Controls.Add(_refund);

        // Items seedha add karte hain (DataSource nahi), taake foran bhar jayen
        _method.Items.AddRange(methods.Cast<object>().ToArray());
        bottom.Controls.Add(L("Method")); bottom.Controls.Add(_method);
        _acc.Items.AddRange(accounts.Cast<object>().ToArray());
        bottom.Controls.Add(L("Account")); bottom.Controls.Add(_acc);

        bottom.Controls.Add(L("Notes")); bottom.Controls.Add(_notes); bottom.SetFlowBreak(_notes, true);
        bottom.Controls.Add(_hint); bottom.SetFlowBreak(_hint, true);
        var cancel = new Button { Text = "Cancel", Width = 90, Height = 36, DialogResult = DialogResult.Cancel };
        _ok.Click += (s, e) =>
        {
            if (_busy) return;
            if (Items.Count == 0) { MessageBox.Show("Kam az kam ek item ki wapsi likhein.", "Check"); return; }
            DialogResult = DialogResult.OK;
        };
        bottom.Controls.Add(_ok); bottom.Controls.Add(cancel);

        Controls.Add(_grid); Controls.Add(bottom); Controls.Add(top);
        CancelButton = cancel;

        var ci = methods.FindIndex(m => m.Equals("Cash", StringComparison.OrdinalIgnoreCase));
        if (_method.Items.Count > 0) _method.SelectedIndex = ci >= 0 ? ci : 0;
        PickAccount();
        _method.SelectedIndexChanged += (s, e) => PickAccount();
        Recalc();
    }

    private void PickAccount()
    {
        if (_accounts.Count == 0 || _acc.Items.Count == 0) return;
        var cash = Method.Equals("Cash", StringComparison.OrdinalIgnoreCase);
        var idx = _accounts.FindIndex(x => cash ? x.AccountType == "Cash" : x.AccountType != "Cash");
        _acc.SelectedIndex = idx >= 0 ? idx : 0;
    }

    private void Recalc()
    {
        decimal retValue = 0, curNet = 0;
        foreach (DataGridViewRow r in _grid.Rows)
        {
            if (r.Tag is not OrderLine l) continue;
            var q = Math.Clamp(Qty(r), 0, l.KeptQty);          // zyada likha to maujooda tak
            r.Cells["Ret"].Value = q;
            retValue += q * l.SellingPrice;
            curNet += l.KeptQty * l.SellingPrice;
        }

        var newTotal = curNet - retValue;
        var refundable = _paid > newTotal ? _paid - newTotal : 0;
        var maxRefund = refundable;
        if (_cust != null) maxRefund = Math.Min(refundable, Math.Max(0, -(_cust.Balance - retValue)));

        _refund.Maximum = maxRefund;
        _refund.Value = maxRefund;                                // pehle poora refund, kam karna ho to likhein

        _info.Text = $"Wapas ki value: {retValue:N0}   |   Nayi bill: {newTotal:N0}   |   Refund ho sakta hai (max): {maxRefund:N0}";
        _hint.Text = _cust == null
            ? "Walk-in: refund jitna kam likhenge, utna paisa dukan mein reh jaye ga (jaise kaat ke diya ho)."
            : $"{_cust.CustomerName}: jo refund nahi diya wo khate mein credit (advance) rahe ga ya baqi udhaar kam karega. Abhi baqi {_cust.Balance:N0}.";
        _ok.Enabled = retValue > 0;
    }
}