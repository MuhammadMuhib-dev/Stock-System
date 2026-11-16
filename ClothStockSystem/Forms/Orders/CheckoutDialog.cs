using ClothStock.Models.Finance;

namespace ClothStockSystem.Forms.Orders;

public class CheckoutDialog : Form
{
    private readonly decimal _total;
    private readonly CustomerBalance? _cust;
    private readonly bool _canCredit;
    private readonly List<FinancialAccount> _accounts;

    private readonly NumericUpDown _num = new();
    private readonly Label _change = new();
    private readonly Label _note = new();
    private readonly Button _ok = new();
    private readonly CheckBox _print = new();
    private readonly ComboBox _acc = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
    private readonly ComboBox _method = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
    private readonly DateTimePicker _due = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 220 };
    private readonly Label _dueLbl = new();

    public decimal AmountPaid => _num.Value;
    public int AccountId => (_acc.SelectedItem as FinancialAccount)?.AccountId ?? 0;
    public string Method => _method.SelectedItem as string ?? "";
    public DateTime? DueDate => _num.Value < _total ? _due.Value.Date : null;
    public bool PrintReceipt => _print.Checked;

    private static string Fmt(decimal v) => v % 1 == 0 ? v.ToString("N0") : v.ToString("N2");

    public CheckoutDialog(decimal total, int suits, CustomerBalance? customer,
                          List<FinancialAccount> accounts, List<string> methods,
                          int defaultCreditDays, bool canGiveCredit)
    {
        _total = total; _cust = customer; _accounts = accounts; _canCredit = canGiveCredit;

        Text = "Checkout";
        Font = new System.Drawing.Font("Segoe UI", 11F);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        AutoSize = true; AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var t = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(20) };

        Label L(string text, bool bold = false, float size = 11F, System.Drawing.Color? color = null) => new()
        {
            Text = text,
            AutoSize = true,
            Margin = new Padding(3, 8, 3, 8),
            Font = new System.Drawing.Font("Segoe UI", size, bold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular),
            ForeColor = color ?? System.Drawing.Color.Black
        };

        var r = 0;
        t.Controls.Add(L("Customer"), 0, r);
        t.Controls.Add(L(customer == null ? "(walk-in)" : $"{customer.CustomerName}  |  baqi {Fmt(customer.Balance)}", true), 1, r++);
        t.Controls.Add(L("Total Suits"), 0, r);
        t.Controls.Add(L(suits.ToString("N0"), true), 1, r++);
        t.Controls.Add(L("GRAND TOTAL", true, 13F), 0, r);
        t.Controls.Add(L(Fmt(total), true, 18F, System.Drawing.Color.FromArgb(0, 70, 140)), 1, r++);

        t.Controls.Add(L("Customer ne diye (Paid)"), 0, r);
        _num.DecimalPlaces = 2; _num.Maximum = 1000000000; _num.ThousandsSeparator = true;
        _num.TextAlign = HorizontalAlignment.Right;
        _num.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
        _num.Width = 220;
        _num.Value = Math.Min(total, _num.Maximum);
        _num.ValueChanged += (s, e) => UpdateState();
        _num.Enter += (s, e) => _num.Select(0, _num.Text.Length);
        t.Controls.Add(_num, 1, r++);

        var exact = new Button { Text = "Exact (poore paise diye)", AutoSize = true, Margin = new Padding(3, 6, 3, 6) };
        exact.Click += (s, e) => { _num.Value = Math.Min(_total, _num.Maximum); _num.Focus(); };
        t.Controls.Add(exact, 1, r++);

        t.Controls.Add(L("Change / Udhaar", true, 13F), 0, r);
        _change.AutoSize = true; _change.Margin = new Padding(3, 8, 3, 8);
        _change.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
        t.Controls.Add(_change, 1, r++);

        _note.AutoSize = false; _note.Width = 420; _note.Height = 50;
        _note.ForeColor = System.Drawing.Color.DimGray;
        t.Controls.Add(_note, 0, r); t.SetColumnSpan(_note, 2); r++;

        _dueLbl.Text = "Due date (baqi kab dega)"; _dueLbl.AutoSize = true; _dueLbl.Margin = new Padding(3, 8, 3, 8);
        _due.MinDate = DateTime.Today;
        _due.Value = DateTime.Today.AddDays(Math.Max(defaultCreditDays, 0));
        t.Controls.Add(_dueLbl, 0, r); t.Controls.Add(_due, 1, r++);

        // Items seedha add karte hain (DataSource nahi), taake foran bhar jayen
        t.Controls.Add(L("Payment method"), 0, r);
        _method.Items.AddRange(methods.Cast<object>().ToArray());
        t.Controls.Add(_method, 1, r++);

        t.Controls.Add(L("Account"), 0, r);
        _acc.DisplayMember = "AccountName";
        _acc.Items.AddRange(_accounts.Cast<object>().ToArray());
        t.Controls.Add(_acc, 1, r++);

        _print.Text = "Receipt print karein"; _print.Checked = true; _print.AutoSize = true;
        t.Controls.Add(_print, 1, r++);

        var flow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
        _ok.Text = "Confirm Sale"; _ok.AutoSize = true; _ok.Padding = new Padding(14, 6, 14, 6);
        _ok.BackColor = System.Drawing.Color.FromArgb(13, 148, 136); _ok.ForeColor = System.Drawing.Color.White;
        _ok.FlatStyle = FlatStyle.Flat; _ok.DialogResult = DialogResult.OK;
        var cancel = new Button { Text = "Cancel", AutoSize = true, Padding = new Padding(10, 6, 10, 6), DialogResult = DialogResult.Cancel };
        flow.Controls.Add(_ok); flow.Controls.Add(cancel);
        t.Controls.Add(flow, 0, r); t.SetColumnSpan(flow, 2);

        Controls.Add(t);
        AcceptButton = _ok; CancelButton = cancel;

        // Cash method ho to Cash account, warna pehla Bank/Wallet account
        var cashIdx = methods.FindIndex(m => m.Equals("Cash", StringComparison.OrdinalIgnoreCase));
        if (_method.Items.Count > 0)
            _method.SelectedIndex = cashIdx >= 0 ? cashIdx : 0;
        PickAccountForMethod();
        _method.SelectedIndexChanged += (s, e) => PickAccountForMethod();

        Shown += (s, e) => { _num.Focus(); _num.Select(0, _num.Text.Length); };
        UpdateState();
    }

    private void PickAccountForMethod()
    {
        if (_accounts.Count == 0 || _acc.Items.Count == 0) return;
        var cash = Method.Equals("Cash", StringComparison.OrdinalIgnoreCase);
        var idx = _accounts.FindIndex(x => cash ? x.AccountType == "Cash" : x.AccountType != "Cash");
        _acc.SelectedIndex = idx >= 0 ? idx : 0;
    }

    private void UpdateState()
    {
        var diff = _num.Value - _total;
        _dueLbl.Visible = _due.Visible = diff < 0;

        if (diff >= 0)
        {
            _change.Text = Fmt(diff);
            _change.ForeColor = System.Drawing.Color.FromArgb(0, 110, 50);
            _note.Text = "";
            _ok.Enabled = true;
            return;
        }

        var due = -diff;
        string? problem = null;
        if (_cust == null) problem = "For credit, please select a customer first (walk-in cannot use credit).";
        else if (!_canCredit) problem = "Udhaar dene ka haq sirf Owner ka hai.";
        else if (_cust.CreditLimit <= 0) problem = "Is customer ki credit limit 0 hai.";

        _change.Text = "UDHAAR: " + Fmt(due);
        _change.ForeColor = problem == null ? System.Drawing.Color.DarkOrange : System.Drawing.Color.Firebrick;
        _note.Text = problem ??
            $"Baqi {Fmt(due)} customer ke khate mein jayega. Abhi baqi {Fmt(_cust!.Balance)}, naye udhaar ke baad " +
            $"{Fmt(_cust.Balance + due)} (limit {Fmt(_cust.CreditLimit)}). Limit ka aakhri faisla save par hota hai.";
        _ok.Enabled = problem == null;
    }
}