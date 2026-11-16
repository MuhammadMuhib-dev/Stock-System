using ClothStock.Models.Finance;

namespace ClothStockSystem.Forms.Finance;

public class CodRemitDialog : Form
{
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = System.Drawing.Color.White
    };
    private readonly NumericUpDown _amt = new() { DecimalPlaces = 2, Maximum = 1000000000, ThousandsSeparator = true, Width = 160, TextAlign = HorizontalAlignment.Right };
    private readonly DateTimePicker _date = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 140, MaxDate = DateTime.Today };
    private readonly ComboBox _method = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly ComboBox _acc = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, DisplayMember = "AccountName", ValueMember = "AccountId" };
    private readonly TextBox _notes = new() { Width = 300, MaxLength = 120, PlaceholderText = "Courier slip ya reference (optional)" };
    private readonly Label _info = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(0, 70, 140) };
    private readonly List<CodPending> _items;
    private readonly List<FinancialAccount> _accounts;

    public List<int> OrderIds => _grid.SelectedRows.Cast<DataGridViewRow>()
        .Select(r => r.DataBoundItem).OfType<CodPending>().Select(x => x.OrderId).ToList();
    public decimal Amount => _amt.Value;
    public DateTime Date => _date.Value.Date;
    public string Method => _method.Text;
    public int AccountId => _acc.SelectedValue is int i ? i : 0;
    public string? Notes => string.IsNullOrWhiteSpace(_notes.Text) ? null : _notes.Text.Trim();
    public string Key { get; } = Guid.NewGuid().ToString("N");

    public CodRemitDialog(List<CodPending> items, List<FinancialAccount> accounts, List<string> methods)
    {
        _items = items; _accounts = accounts;
        Text = "COD mila (courier se paisa)";
        Font = new System.Drawing.Font("Segoe UI", 10.5F);
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(1100, 700);

        _grid.DataSource = items;
        foreach (var c in new[] { "OrderId", "NetSale" }) if (_grid.Columns.Contains(c)) _grid.Columns[c]!.Visible = false;
        void H(string c, string t) { if (_grid.Columns.Contains(c)) _grid.Columns[c]!.HeaderText = t; }
        H("OrderNo", "Order"); H("OrderDate", "Date"); H("CustomerName", "Customer"); H("CourierName", "Courier");
        H("TrackingId", "Tracking ID"); H("CourierCharge", "Courier charge"); H("Expected", "Courier se milna chahiye"); H("DaysWaiting", "Kitne din se");
        if (_grid.Columns.Contains("OrderDate")) _grid.Columns["OrderDate"]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
        foreach (var c in new[] { "CourierCharge", "Expected" })
            if (_grid.Columns.Contains(c)) { _grid.Columns[c]!.DefaultCellStyle.Format = "N0"; _grid.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
        _grid.ClearSelection();

        var top = new Label
        {
            Dock = DockStyle.Top,
            Height = 40,
            Padding = new Padding(8, 10, 0, 0),
            Text = "Delivered orders jinka COD abhi nahi mila. Jo orders courier ne ek saath diye wo Ctrl+click se chunen."
        };

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 150, Padding = new Padding(8), WrapContents = true };
        bottom.Controls.Add(_info); bottom.SetFlowBreak(_info, true);
        Label L(string t) => new() { Text = t, AutoSize = true, Margin = new Padding(8, 8, 3, 0) };
        bottom.Controls.Add(L("Asal mila")); bottom.Controls.Add(_amt);
        bottom.Controls.Add(L("Tareekh")); bottom.Controls.Add(_date);
        _method.DataSource = methods;
        bottom.Controls.Add(L("Method")); bottom.Controls.Add(_method);
        _acc.DataSource = accounts.ToList();
        bottom.Controls.Add(L("Account")); bottom.Controls.Add(_acc);
        bottom.Controls.Add(L("Notes")); bottom.Controls.Add(_notes);
        var ok = new Button { Text = "Save (COD mila)", Width = 170, Height = 34, BackColor = System.Drawing.Color.FromArgb(13, 148, 136), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };
        var cancel = new Button { Text = "Cancel", Width = 90, Height = 34, DialogResult = DialogResult.Cancel };
        ok.Click += (s, e) =>
        {
            if (OrderIds.Count == 0) { MessageBox.Show("Kam az kam ek order chunen.", "Check"); return; }
            if (_amt.Value <= 0) { MessageBox.Show("Amount 0 se zyada likhein.", "Check"); return; }
            DialogResult = DialogResult.OK;
        };
        bottom.Controls.Add(ok); bottom.Controls.Add(cancel);

        Controls.Add(_grid); Controls.Add(bottom); Controls.Add(top);
        AcceptButton = ok; CancelButton = cancel;

        _grid.SelectionChanged += (s, e) => Update();
        _amt.ValueChanged += (s, e) => ShowInfo();
        _method.SelectedIndexChanged += (s, e) => PickAccount();
        var ci = methods.FindIndex(m => m.Equals("Cash", StringComparison.OrdinalIgnoreCase));
        if (ci >= 0) _method.SelectedIndex = ci;
        PickAccount();
        ShowInfo();
    }

    private decimal Expected => _grid.SelectedRows.Cast<DataGridViewRow>()
        .Select(r => r.DataBoundItem).OfType<CodPending>().Sum(x => x.Expected);

    private void Update() { _amt.Value = Math.Min(Expected, _amt.Maximum); ShowInfo(); }

    private void ShowInfo()
    {
        var exp = Expected;
        var diff = _amt.Value - exp;
        _info.Text = $"Orders: {OrderIds.Count}   |   Milna chahiye: {exp:N0}   |   Asal mila: {_amt.Value:N0}   |   " +
                     (diff == 0 ? "Farq nahi" : diff < 0 ? $"KAM mila: {-diff:N0}" : $"ZYADA mila: {diff:N0}");
        _info.ForeColor = diff == 0 ? System.Drawing.Color.FromArgb(0, 110, 50) : System.Drawing.Color.Firebrick;
    }

    private void PickAccount()
    {
        var cash = _method.Text.Equals("Cash", StringComparison.OrdinalIgnoreCase);
        var a = _accounts.FirstOrDefault(x => cash ? x.AccountType == "Cash" : x.AccountType != "Cash");
        if (a != null) _acc.SelectedValue = a.AccountId;
    }
}