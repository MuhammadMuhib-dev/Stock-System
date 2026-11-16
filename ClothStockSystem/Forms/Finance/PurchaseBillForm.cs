using System.ComponentModel;
using ClothStock.Business.Services;
using ClothStock.Models.Finance;
using ClothStock.Models.Stock;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Finance;

public class PurchaseBillForm : Form
{
    private static string CurrentUser => SessionManager.Username;

    private readonly FinanceService _fin;
    private readonly ArticleService _articles;
    private readonly ExpenseSetupService _setup;

    // bill
    private readonly ComboBox _sup = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220, DisplayMember = "SupplierName", ValueMember = "SupplierId" };
    private readonly TextBox _no = new() { Width = 140, MaxLength = 40 };
    private readonly DateTimePicker _date = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 140, MaxDate = DateTime.Today };
    private readonly DateTimePicker _due = new() { Format = DateTimePickerFormat.Custom, CustomFormat = "dd-MMM-yyyy", Width = 140 };
    private readonly TextBox _notes = new() { Width = 300, MaxLength = 250 };

    // line
    private readonly ComboBox _art = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200, DisplayMember = "ArticleName", ValueMember = "ArticleId" };
    private readonly ComboBox _col = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180, DisplayMember = "ColorName", ValueMember = "ColorId" };
    private readonly NumericUpDown _bunch = new() { Maximum = 1000000, Width = 90, TextAlign = HorizontalAlignment.Right };
    private readonly NumericUpDown _spb = new() { Maximum = 100000, Width = 90, TextAlign = HorizontalAlignment.Right };
    private readonly NumericUpDown _mps = new() { DecimalPlaces = 3, Maximum = 1000, Width = 100, TextAlign = HorizontalAlignment.Right, Increment = 0.5m };
    private readonly NumericUpDown _cost = new() { DecimalPlaces = 2, Maximum = 100000000, Width = 120, TextAlign = HorizontalAlignment.Right, ThousandsSeparator = true };
    private readonly Label _preview = new() { AutoSize = true, ForeColor = System.Drawing.Color.DimGray, Margin = new Padding(10, 8, 3, 3) };

    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = System.Drawing.Color.White,
        BorderStyle = BorderStyle.None
    };

    // payment
    private readonly Label _total = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(0, 70, 140) };
    private readonly NumericUpDown _paid = new() { DecimalPlaces = 2, Maximum = 1000000000, Width = 150, TextAlign = HorizontalAlignment.Right, ThousandsSeparator = true };
    private readonly ComboBox _method = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly ComboBox _acc = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150, DisplayMember = "AccountName", ValueMember = "AccountId" };
    private readonly Label _due2 = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold), Margin = new Padding(10, 8, 3, 3) };
    private readonly Button _save = new() { Text = "Bill Save Karein", Width = 190, Height = 40, BackColor = System.Drawing.Color.FromArgb(13, 148, 136), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };

    private class LineVm
    {
        public int ColorId { get; set; }
        public string Article { get; set; } = "";
        public string Color { get; set; } = "";
        public int Bunches { get; set; }
        public int SuitsPerBunch { get; set; }
        public decimal MetersPerSuit { get; set; }
        public decimal CostPerSuit { get; set; }
        public int Suits => Bunches * SuitsPerBunch;
        public decimal Meters => Suits * MetersPerSuit;
        public decimal LineTotal => Suits * CostPerSuit;
    }

    private readonly BindingList<LineVm> _lines = new();
    private List<FinancialAccount> _accounts = new();
    private string _key = Guid.NewGuid().ToString("N");
    private bool _ready, _colLoading;

    public PurchaseBillForm()
    {
        _fin = Program.Services.GetRequiredService<FinanceService>();
        _articles = Program.Services.GetRequiredService<ArticleService>();
        _setup = Program.Services.GetRequiredService<ExpenseSetupService>();

        Text = "Purchase Bill"; Font = new System.Drawing.Font("Segoe UI", 10.5F);
        Size = new Size(1250, 800); StartPosition = FormStartPosition.CenterScreen;
        _due.Value = DateTime.Today.AddDays(30);
        _grid.DataSource = _lines;

        Label L(string t) => new() { Text = t, AutoSize = true, Margin = new Padding(10, 8, 3, 0) };

        // ---- upar: bill
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 92, Padding = new Padding(6), WrapContents = true, BackColor = System.Drawing.Color.FromArgb(245, 248, 252) };
        top.Controls.Add(L("Supplier")); top.Controls.Add(_sup);
        top.Controls.Add(L("Bill no")); top.Controls.Add(_no);
        top.Controls.Add(L("Bill ki tareekh")); top.Controls.Add(_date);
        top.Controls.Add(L("Due date")); top.Controls.Add(_due);
        top.Controls.Add(L("Notes")); top.Controls.Add(_notes);

        // ---- line
        var line = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 96, Padding = new Padding(6), WrapContents = true };
        line.Controls.Add(L("Article")); line.Controls.Add(_art);
        line.Controls.Add(L("Color")); line.Controls.Add(_col);
        line.Controls.Add(L("Bunches")); line.Controls.Add(_bunch);
        line.Controls.Add(L("Suits/bunch")); line.Controls.Add(_spb);
        line.Controls.Add(L("Meter/suit")); line.Controls.Add(_mps);
        line.Controls.Add(L("Cost/suit")); line.Controls.Add(_cost);
        var add = new Button { Text = "Line Add Karein", Width = 150, Height = 32, Margin = new Padding(10, 3, 3, 3) };
        var rem = new Button { Text = "Selected Hatayen", Width = 150, Height = 32 };
        add.Click += (s, e) => AddLine();
        rem.Click += (s, e) => { if (_grid.CurrentRow?.DataBoundItem is LineVm l) { _lines.Remove(l); Recalc(); } };
        line.Controls.Add(add); line.Controls.Add(rem); line.Controls.Add(_preview);

        // ---- neeche: payment
        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 130, Padding = new Padding(8), WrapContents = true, BackColor = System.Drawing.Color.FromArgb(240, 246, 255) };
        bottom.Controls.Add(_total);
        bottom.SetFlowBreak(_total, true);
        bottom.Controls.Add(L("Abhi diya")); bottom.Controls.Add(_paid);
        bottom.Controls.Add(L("Method")); bottom.Controls.Add(_method);
        bottom.Controls.Add(L("Account")); bottom.Controls.Add(_acc);
        bottom.Controls.Add(_due2);
        bottom.Controls.Add(_save);
        bottom.Controls.Add(new Label
        {
            Text = "Bill total mein sirf maal ki cost hai. Transport supplier ki zimmedari hai, yahan nahi likhna. Ek bill mein ek hi article ke kai colors likhein (rate article ke hisab se).",
            AutoSize = true,
            ForeColor = System.Drawing.Color.DimGray,
            Margin = new Padding(10, 8, 3, 0)
        });

        Controls.Add(_grid); Controls.Add(bottom); Controls.Add(line); Controls.Add(top);

        _art.SelectedIndexChanged += async (s, e) => { if (_ready) await LoadColorsAsync(); };
        foreach (var n in new[] { _bunch, _spb, _mps, _cost }) n.ValueChanged += (s, e) => Preview();
        _paid.ValueChanged += (s, e) => Recalc();
        _method.SelectedIndexChanged += (s, e) => PickAccount();
        _save.Click += async (s, e) => await SaveAsync();

        Load += async (s, e) => await InitAsync();
    }

    private async Task InitAsync()
    {
        try
        {
            _sup.DataSource = (await _fin.GetSuppliersAsync(null)).ToList();
            _art.DataSource = (await _articles.GetArticlesAsync(true)).ToList();
            _accounts = (await _fin.GetAccountsAsync()).ToList();
            _acc.DataSource = _accounts.ToList();
            var methods = (await _setup.GetMethodsAsync(true)).Select(m => m.MethodName).ToList();
            _method.DataSource = methods;
            var ci = methods.FindIndex(m => m.Equals("Cash", StringComparison.OrdinalIgnoreCase));
            if (ci >= 0) _method.SelectedIndex = ci;
            PickAccount();
            _ready = true;
            await LoadColorsAsync();
            FormatGrid();
            Recalc();
            if (_sup.Items.Count == 0)
                MessageBox.Show("Pehle Parties form mein Supplier banayen.", "Supplier nahi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private void FormatGrid()
    {
        foreach (var (c, t) in new[] { ("Article", "Article"), ("Color", "Color"), ("Bunches", "Bunches"), ("SuitsPerBunch", "Suits/bunch"),
                                       ("MetersPerSuit", "Meter/suit"), ("CostPerSuit", "Cost/suit"), ("Suits", "Kul suits"),
                                       ("Meters", "Kul meter"), ("LineTotal", "Line total") })
            if (_grid.Columns.Contains(t == "" ? c : c)) _grid.Columns[c]!.HeaderText = t;
        if (_grid.Columns.Contains("ColorId")) _grid.Columns["ColorId"]!.Visible = false;
        foreach (var c in new[] { "CostPerSuit", "LineTotal" })
            if (_grid.Columns.Contains(c)) { _grid.Columns[c]!.DefaultCellStyle.Format = "N0"; _grid.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
        if (_grid.Columns.Contains("Meters")) _grid.Columns["Meters"]!.DefaultCellStyle.Format = "0.###";
    }

    private async Task LoadColorsAsync()
    {
        try
        {
            _colLoading = true;
            _col.DataSource = _art.SelectedItem is Article a ? (await _articles.GetColorsAsync(a.ArticleId, true)).ToList() : null;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally { _colLoading = false; }
    }

    private void PickAccount()
    {
        var cash = _method.Text.Equals("Cash", StringComparison.OrdinalIgnoreCase);
        var a = _accounts.FirstOrDefault(x => cash ? x.AccountType == "Cash" : x.AccountType != "Cash");
        if (a != null) _acc.SelectedValue = a.AccountId;
    }

    private void Preview()
    {
        var suits = (int)(_bunch.Value * _spb.Value);
        _preview.Text = $"{suits:N0} suits  |  {suits * _mps.Value:0.###} meter  |  line total {suits * _cost.Value:N0}";
    }

    private void AddLine()
    {
        if (_col.SelectedValue is not int colorId || _art.SelectedItem is not Article art)
        { MessageBox.Show("Article aur color chunen.", "Check"); return; }
        if (_bunch.Value <= 0 || _spb.Value <= 0 || _mps.Value <= 0)
        { MessageBox.Show("Bunches, suits/bunch aur meter 0 se zyada likhein.", "Check"); return; }
        if (_cost.Value <= 0 &&
            MessageBox.Show("Cost 0 hai. Phir bhi add karna hai?", "Cost", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        if (_lines.Any(l => l.ColorId == colorId &&
            MessageBox.Show("Ye color is bill mein pehle se hai. Alag batch ke taur par add karein?", "Color dobara", MessageBoxButtons.YesNo) != DialogResult.Yes))
            return;

        // Ek article ka rate ek hi hona chahiye
        var other = _lines.FirstOrDefault(l => l.Article == art.ArticleName && l.CostPerSuit != _cost.Value);
        if (other != null &&
            MessageBox.Show($"{art.ArticleName} ki cost pehli line mein {other.CostPerSuit:N0} hai, ab {_cost.Value:N0} likhi hai. Phir bhi add karein?",
                "Rate alag", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        _lines.Add(new LineVm
        {
            ColorId = colorId,
            Article = art.ArticleName,
            Color = _col.Text,
            Bunches = (int)_bunch.Value,
            SuitsPerBunch = (int)_spb.Value,
            MetersPerSuit = _mps.Value,
            CostPerSuit = _cost.Value
        });
        _bunch.Value = 0;
        Recalc();
        _bunch.Focus();
    }

    private decimal Total => _lines.Sum(l => l.LineTotal);

    private void Recalc()
    {
        var t = Total;
        _total.Text = $"BILL TOTAL: {t:N0}     ({_lines.Count} batch, {_lines.Sum(l => l.Suits):N0} suits)";
        var left = t - _paid.Value;
        if (left > 0) { _due2.Text = $"BAQI UDHAAR: {left:N0}"; _due2.ForeColor = System.Drawing.Color.DarkOrange; }
        else if (left < 0) { _due2.Text = "Diya hua bill se zyada hai!"; _due2.ForeColor = System.Drawing.Color.Firebrick; }
        else { _due2.Text = "Poora ada (cash)"; _due2.ForeColor = System.Drawing.Color.FromArgb(0, 110, 50); }
        _due.Enabled = left > 0;
    }

    private async Task SaveAsync()
    {
        try
        {
            if (_sup.SelectedValue is not int sid) { MessageBox.Show("Supplier chunen.", "Check"); return; }
            var req = new PurchaseBillRequest
            {
                SupplierId = sid,
                InvoiceNo = _no.Text,
                InvoiceDate = _date.Value.Date,
                DueDate = _paid.Value < Total ? _due.Value.Date : null,
                Notes = string.IsNullOrWhiteSpace(_notes.Text) ? null : _notes.Text.Trim(),
                User = CurrentUser,
                PaidNow = _paid.Value,
                AccountId = _acc.SelectedValue as int?,
                Method = _method.Text,
                Key = _key,
                Items = _lines.Select(l => new PurchaseItemRequest
                {
                    ColorId = l.ColorId,
                    Bunches = l.Bunches,
                    SuitsPerBunch = l.SuitsPerBunch,
                    MetersPerSuit = l.MetersPerSuit,
                    CostPerSuit = l.CostPerSuit
                }).ToList()
            };

            var left = req.Total - req.PaidNow;
            var msg = $"{_sup.Text}   |   Bill {req.InvoiceNo}\nTotal: {req.Total:N0}\nAbhi diya: {req.PaidNow:N0} ({_method.Text}, {_acc.Text})\n" +
                      (left > 0 ? $"Baqi udhaar: {left:N0}, due {_due.Value:dd-MMM-yyyy}\n" : "Poora ada.\n") +
                      $"Batches: {_lines.Count}, suits: {_lines.Sum(l => l.Suits):N0}\n\nSave karna hai?";
            if (MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _save.Enabled = false;
            await _fin.PurchaseStockInAsync(req);
            MessageBox.Show("Bill aur stock save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

            _lines.Clear(); _no.Clear(); _notes.Clear(); _paid.Value = 0;
            _key = Guid.NewGuid().ToString("N");
            Recalc();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Bill save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        finally { _save.Enabled = true; }
    }
}