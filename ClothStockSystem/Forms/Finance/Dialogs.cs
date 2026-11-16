using ClothStock.Models.Finance;

namespace ClothStockSystem.Forms.Finance;

// Paisa aaya / gaya / transfer, ek hi dialog
public class MoneyDialog : Form
{
    private readonly NumericUpDown _amt = new() { DecimalPlaces = 2, Maximum = 1000000000, ThousandsSeparator = true, Width = 240, TextAlign = HorizontalAlignment.Right };
    private readonly DateTimePicker _date = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 240, MaxDate = DateTime.Today };
    private readonly ComboBox _method = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 240 };
    private readonly ComboBox _acc = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 240, DisplayMember = "AccountName", ValueMember = "AccountId" };
    private readonly ComboBox _to = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 240, DisplayMember = "AccountName", ValueMember = "AccountId" };
    private readonly TextBox _notes = new() { Width = 240, MaxLength = 250 };
    private readonly List<FinancialAccount> _accounts;

    public decimal Amount => _amt.Value;
    public DateTime Date => _date.Value.Date;
    public string Method => _method.Text;
    public int AccountId => _acc.SelectedValue is int i ? i : 0;
    public int ToAccountId => _to.SelectedValue is int i ? i : 0;
    public string? Notes => string.IsNullOrWhiteSpace(_notes.Text) ? null : _notes.Text.Trim();
    public string Key { get; } = Guid.NewGuid().ToString("N");     // double click / retry se bachao

    public MoneyDialog(string title, string info, List<FinancialAccount> accounts, List<string> methods,
                       bool transfer = false, string accountLabel = "Account")
    {
        _accounts = accounts;
        Text = title;
        Font = new System.Drawing.Font("Segoe UI", 10.5F);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        AutoSize = true; AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var t = new TableLayoutPanel { ColumnCount = 2, AutoSize = true, Padding = new Padding(18) };
        var r = 0;
        void Row(string label, Control c) { t.Controls.Add(new Label { Text = label, AutoSize = true, Margin = new Padding(3, 8, 8, 8) }, 0, r); t.Controls.Add(c, 1, r++); }

        var lbl = new Label { Text = info, AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold), Margin = new Padding(3, 0, 3, 10) };
        t.Controls.Add(lbl, 0, r); t.SetColumnSpan(lbl, 2); r++;

        Row("Amount", _amt);
        Row("Tareekh", _date);
        _method.DataSource = methods;
        Row("Payment method", _method);
        _acc.DataSource = accounts.ToList();
        Row(accountLabel, _acc);
        if (transfer) { _to.DataSource = accounts.ToList(); if (accounts.Count > 1) _to.SelectedIndex = 1; Row("Kis account mein", _to); }
        Row("Notes", _notes);

        var ok = new Button { Text = "Save", Width = 110, Height = 34, BackColor = System.Drawing.Color.FromArgb(13, 148, 136), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };
        var cancel = new Button { Text = "Cancel", Width = 90, Height = 34, DialogResult = DialogResult.Cancel };
        ok.Click += (s, e) =>
        {
            if (_amt.Value <= 0) { MessageBox.Show("Amount 0 se zyada likhein.", "Check"); return; }
            if (transfer && AccountId == ToAccountId) { MessageBox.Show("Dono account alag chunen.", "Check"); return; }
            DialogResult = DialogResult.OK;
        };
        var flow = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Dock = DockStyle.Fill };
        flow.Controls.Add(ok); flow.Controls.Add(cancel);
        t.Controls.Add(flow, 0, r); t.SetColumnSpan(flow, 2);

        Controls.Add(t);
        AcceptButton = ok; CancelButton = cancel;

        _method.SelectedIndexChanged += (s, e) => PickAccount();
        var ci = methods.FindIndex(m => m.Equals("Cash", StringComparison.OrdinalIgnoreCase));
        if (ci >= 0) _method.SelectedIndex = ci;
        PickAccount();
        Shown += (s, e) => { _amt.Focus(); _amt.Select(0, _amt.Text.Length); };
    }

    private void PickAccount()
    {
        var cash = _method.Text.Equals("Cash", StringComparison.OrdinalIgnoreCase);
        var a = _accounts.FirstOrDefault(x => cash ? x.AccountType == "Cash" : x.AccountType != "Cash");
        if (a != null) _acc.SelectedValue = a.AccountId;
    }
}

public static class Prompt
{
    public static string? Ask(IWin32Window owner, string title, string label)
    {
        using var f = new Form
        {
            Text = title,
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Font = new System.Drawing.Font("Segoe UI", 10.5F)
        };
        var tb = new TextBox { Width = 380, MaxLength = 250 };
        var ok = new Button { Text = "OK", Width = 90, DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Cancel", Width = 90, DialogResult = DialogResult.Cancel };
        var p = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true, Padding = new Padding(16) };
        p.Controls.Add(new Label { Text = label, AutoSize = true });
        p.Controls.Add(tb);
        var b = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.RightToLeft, Width = 380 };
        b.Controls.Add(ok); b.Controls.Add(cancel);
        p.Controls.Add(b);
        f.Controls.Add(p);
        f.AcceptButton = ok; f.CancelButton = cancel;
        return f.ShowDialog(owner) == DialogResult.OK ? tb.Text.Trim() : null;
    }
}