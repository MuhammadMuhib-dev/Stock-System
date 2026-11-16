using ClothStock.Business.Services;
using ClothStock.Models.Finance;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Finance;

public class CashBookForm : Form
{
    private static string CurrentUser => SessionManager.Username;

    private readonly FinanceService _fin;
    private readonly ExpenseSetupService _setup;
    private readonly ComboBox _acc = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160, DisplayMember = "AccountName", ValueMember = "AccountId" };
    private readonly ComboBox _range = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140 };
    private readonly CheckBox _void = new() { Text = "Void bhi dikhayen", AutoSize = true, Margin = new Padding(10, 6, 3, 3) };
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
    private readonly Label _bal = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(0, 70, 140) };
    private readonly Label _sum = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold) };
    private readonly Label _src = new() { AutoSize = true, ForeColor = System.Drawing.Color.DimGray };
    private List<FinancialAccount> _accounts = new();
    private bool _ready;

    private static string SourceName(string? r) => r switch
    {
        "ShopSale" => "Shop sale",
        "CustomerReceipt" => "Customer receipt",
        "SupplierPayment" => "Supplier payment",
        "Expense" => "Kharche",
        "COD" => "COD",
        "Transfer" => "Transfer",
        "OwnerIn" => "Owner ne dala",
        "OwnerOut" => "Owner ne nikala",
        _ => "Baaqi"
    };

    public CashBookForm()
    {
        _fin = Program.Services.GetRequiredService<FinanceService>();
        _setup = Program.Services.GetRequiredService<ExpenseSetupService>();
        Text = "Cash Book"; Font = new System.Drawing.Font("Segoe UI", 10.5F);
        Size = new Size(1300, 780); StartPosition = FormStartPosition.CenterScreen;

        _range.Items.AddRange(new object[] { "Aaj", "Is mahine", "Pichla mahina", "Is saal", "Sab" });
        _range.SelectedIndex = 1;

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 140, Padding = new Padding(6), WrapContents = true };
        top.Controls.Add(new Label { Text = "Account", AutoSize = true, Margin = new Padding(3, 8, 3, 0) });
        top.Controls.Add(_acc);
        top.Controls.Add(new Label { Text = "Muddat", AutoSize = true, Margin = new Padding(10, 8, 3, 0) });
        top.Controls.Add(_range); top.Controls.Add(_void);

        Button Btn(string t, int w, Func<Task> act, System.Drawing.Color? back = null)
        {
            var b = new Button { Text = t, Width = w, Height = 32, Margin = new Padding(6, 3, 3, 3) };
            if (back.HasValue) { b.BackColor = back.Value; b.ForeColor = System.Drawing.Color.White; b.FlatStyle = FlatStyle.Flat; }
            b.Click += async (s, e) => { try { b.Enabled = false; await act(); } finally { b.Enabled = true; } };
            return b;
        }
        top.Controls.Add(Btn("Refresh", 90, LoadAsync));
        top.Controls.Add(Btn("COD mila (courier se)", 190, CodAsync, System.Drawing.Color.FromArgb(13, 148, 136)));
        top.Controls.Add(Btn("Paisa jama (Owner)", 170, () => ManualAsync("Receipt")));
        top.Controls.Add(Btn("Owner ne nikala", 150, () => ManualAsync("Payment")));
        top.Controls.Add(Btn("Transfer", 100, TransferAsync));
        top.Controls.Add(Btn("Void Selected", 130, VoidAsync, System.Drawing.Color.FromArgb(200, 40, 40)));
        top.Controls.Add(_bal);

        var bottom = new Panel { Dock = DockStyle.Bottom, Height = 76 };
        bottom.Controls.Add(_sum); _sum.Location = new Point(8, 8);
        bottom.Controls.Add(_src); _src.Location = new Point(8, 40);

        Controls.Add(_grid); Controls.Add(bottom); Controls.Add(top);

        _acc.SelectedIndexChanged += async (s, e) => { if (_ready) await LoadAsync(); };
        _range.SelectedIndexChanged += async (s, e) => { if (_ready) await LoadAsync(); };
        _void.CheckedChanged += async (s, e) => { if (_ready) await LoadAsync(); };
        _grid.DataBindingComplete += (s, e) =>
        {
            foreach (DataGridViewRow r in _grid.Rows)
            {
                if (r.DataBoundItem is not CashBookRow x) continue;
                if (x.Status == "Void") r.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                else if (x.Direction == "Receipt") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(228, 245, 228);
                else if (x.Direction == "Payment") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 232, 232);
                else r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(232, 240, 255);
            }
            _grid.ClearSelection();
        };
        Load += async (s, e) => await InitAsync();
    }

    private async Task InitAsync()
    {
        try
        {
            _accounts = (await _fin.GetAccountsAsync()).ToList();
            var list = new List<FinancialAccount> { new() { AccountId = 0, AccountName = "-- Sab accounts --" } };
            list.AddRange(_accounts);
            _acc.DataSource = list;
            _ready = true;
            await LoadAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private (DateTime? from, DateTime? to) Range()
    {
        var t = DateTime.Today;
        return _range.SelectedIndex switch
        {
            0 => (t, t),
            1 => (new DateTime(t.Year, t.Month, 1), t),
            2 => (new DateTime(t.Year, t.Month, 1).AddMonths(-1), new DateTime(t.Year, t.Month, 1).AddDays(-1)),
            3 => (new DateTime(t.Year, 1, 1), t),
            _ => (null, null)
        };
    }

    private static void H(DataGridView g, string col, string text) { if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text; }

    private async Task LoadAsync()
    {
        try
        {
            var (from, to) = Range();
            int? acc = _acc.SelectedValue is int a && a > 0 ? a : null;
            var rows = (await _fin.GetCashBookAsync(acc, from, to, _void.Checked)).ToList();

            _grid.DataSource = rows;
            foreach (var c in new[] { "TransactionId", "AccountId", "Amount", "ReferenceType", "VoidReason", "CreatedBy" })
                if (_grid.Columns.Contains(c)) _grid.Columns[c]!.Visible = false;
            H(_grid, "TxnDate", "Tareekh"); H(_grid, "AccountName", "Account"); H(_grid, "Direction", "Qisam");
            H(_grid, "MethodName", "Method"); H(_grid, "PartyName", "Party"); H(_grid, "Notes", "Notes");
            H(_grid, "MoneyIn", "Aaya"); H(_grid, "MoneyOut", "Gaya"); H(_grid, "RunningBalance", "Account ka balance");
            if (_grid.Columns.Contains("TxnDate")) _grid.Columns["TxnDate"]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            foreach (var c in new[] { "MoneyIn", "MoneyOut", "RunningBalance" })
                if (_grid.Columns.Contains(c)) { _grid.Columns[c]!.DefaultCellStyle.Format = "N0"; _grid.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }

            var live = rows.Where(r => r.Status == "Active").ToList();
            var rin = live.Where(r => r.Direction == "Receipt").Sum(r => r.Amount);
            var pay = live.Where(r => r.Direction == "Payment").Sum(r => r.Amount);
            var tin = live.Where(r => r.Direction == "TransferIn").Sum(r => r.Amount);
            var tout = live.Where(r => r.Direction == "TransferOut").Sum(r => r.Amount);
            _sum.Text = $"Is muddat mein:  AAYA {rin:N0}   |   GAYA {pay:N0}   |   Net {rin - pay:N0}   |   Transfer in {tin:N0} / out {tout:N0}   |   Entries {live.Count}";

            // Source-wise: paisa kahan se aaya, kahan gaya
            var parts = live.Where(r => r.Direction is "Receipt" or "Payment")
                .GroupBy(r => (SourceName(r.ReferenceType), r.Direction))
                .OrderBy(g => g.Key.Direction).ThenBy(g => g.Key.Item1)
                .Select(g => $"{g.Key.Item1} {(g.Key.Direction == "Receipt" ? "+" : "-")}{g.Sum(x => x.Amount):N0}");
            _src.Text = "Source: " + string.Join("   |   ", parts);

            var bals = (await _fin.GetAccountBalancesAsync()).Where(b => b.IsActive).ToList();
            _bal.Text = "Abhi:  " + string.Join("   |   ", bals.Select(b => $"{b.AccountName} {b.Balance:N0}")) + $"   |   TOTAL {bals.Sum(b => b.Balance):N0}";
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private async Task<(List<string> methods, bool ok)> MethodsAsync()
    {
        var methods = (await _setup.GetMethodsAsync(true)).Select(m => m.MethodName).ToList();
        if (_accounts.Count == 0 || methods.Count == 0) { MessageBox.Show("Koi active account ya payment method nahi."); return (methods, false); }
        return (methods, true);
    }

    private async Task CodAsync()
    {
        try
        {
            var (methods, ok) = await MethodsAsync(); if (!ok) return;
            var pending = (await _fin.GetCodPendingAsync()).ToList();
            if (pending.Count == 0) { MessageBox.Show("Koi Delivered order COD ke intezar mein nahi.", "COD", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            using var dlg = new CodRemitDialog(pending, _accounts, methods);
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            await _fin.CodRemitAsync(dlg.OrderIds, dlg.Amount, dlg.AccountId, dlg.Method, dlg.Date, dlg.Notes, dlg.Key, CurrentUser);
            MessageBox.Show("COD ki entry ho gayi.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "COD entry nahi hui", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private async Task ManualAsync(string direction)
    {
        try
        {
            var (methods, ok) = await MethodsAsync(); if (!ok) return;
            var recv = direction == "Receipt";
            using var dlg = new MoneyDialog(recv ? "Paisa jama (Owner ne dala)" : "Owner ne nikala",
                recv ? "Ye sale nahi hai, sirf cash barhta hai. Notes mein wajah likhein." : "Ye kharcha nahi hai (profit mein nahi ginta). Notes mein wajah likhein.",
                _accounts, methods, false, recv ? "Kis account mein" : "Kis account se");
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            await _fin.CashManualAsync(direction, dlg.Amount, dlg.AccountId, dlg.Method, dlg.Date, dlg.Notes, dlg.Key, CurrentUser);
            MessageBox.Show("Entry ho gayi.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Entry nahi hui", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private async Task TransferAsync()
    {
        try
        {
            if (_accounts.Count < 2) { MessageBox.Show("Transfer ke liye kam az kam 2 account chahiye."); return; }
            var (methods, ok) = await MethodsAsync(); if (!ok) return;
            using var dlg = new MoneyDialog("Transfer", "Apne hi 2 accounts ke beech (sale ya kharcha nahi ginta)",
                _accounts, methods, true, "Kis account se");
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            await _fin.TransferAsync(dlg.AccountId, dlg.ToAccountId, dlg.Amount, dlg.Method, dlg.Date, dlg.Notes, dlg.Key, CurrentUser);
            MessageBox.Show("Transfer ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Transfer nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    private async Task VoidAsync()
    {
        if (_grid.CurrentRow?.DataBoundItem is not CashBookRow r)
        { MessageBox.Show("Pehle list se entry select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (r.Status == "Void") { MessageBox.Show("Ye entry pehle se void hai."); return; }

        var reason = Prompt.Ask(this, "Void",
            $"{r.TxnDate:dd-MMM-yyyy} | {r.AccountName} | {r.Direction} {r.Amount:N0}\n\nVoid ki wajah likhein:");
        if (reason == null) return;
        try
        {
            await _fin.VoidAsync(r.TransactionId, reason, CurrentUser);
            await LoadAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Void nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}