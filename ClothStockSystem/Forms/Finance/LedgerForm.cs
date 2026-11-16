using ClothStock.Business.Services;
using ClothStock.Models.Finance;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Finance;

public class LedgerForm : Form
{
    private static string CurrentUser => SessionManager.Username;

    private readonly FinanceService _fin;
    private readonly ExpenseSetupService _setup;
    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill };

    // khata tab
    private readonly ComboBox _type = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly TextBox _search = new() { Width = 200, PlaceholderText = "Naam ya phone" };
    private readonly DataGridView _gParty = Grid();
    private readonly DataGridView _gStmt = Grid();
    private readonly ComboBox _range = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140 };
    private readonly Label _head = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Margin = new Padding(6, 8, 3, 3) };
    private readonly Label _sum = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(0, 70, 140) };
    private readonly Button _act = new() { Width = 200, Height = 34, BackColor = System.Drawing.Color.FromArgb(13, 148, 136), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat };

    // due tab
    private readonly ComboBox _dKind = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly CheckBox _dOver = new() { Text = "Sirf guzra hua", AutoSize = true, Margin = new Padding(10, 6, 3, 3) };
    private readonly DataGridView _gDue = Grid();
    private readonly Label _dSum = new() { AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold), ForeColor = System.Drawing.Color.FromArgb(0, 70, 140) };

    private record PartyRow(int Id, string Name, string? Phone, decimal Balance, decimal Overdue);
    private List<PartyRow> _parties = new();
    private bool _ready, _loading;

    private bool IsCustomer => _type.SelectedIndex == 0;
    private PartyRow? Selected => _gParty.CurrentRow?.DataBoundItem as PartyRow;

    private static DataGridView Grid() => new()
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
    private static void H(DataGridView g, string col, string text) { if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text; }
    private static void N0(DataGridView g, params string[] cols) { foreach (var c in cols) if (g.Columns.Contains(c)) { g.Columns[c]!.DefaultCellStyle.Format = "N0"; g.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; } }
    private static void D(DataGridView g, params string[] cols) { foreach (var c in cols) if (g.Columns.Contains(c)) g.Columns[c]!.DefaultCellStyle.Format = "dd-MMM-yyyy"; }

    public LedgerForm()
    {
        _fin = Program.Services.GetRequiredService<FinanceService>();
        _setup = Program.Services.GetRequiredService<ExpenseSetupService>();
        Text = "Ledger"; Font = new System.Drawing.Font("Segoe UI", 10.5F);
        Size = new Size(1250, 750); StartPosition = FormStartPosition.CenterScreen;

        _type.Items.AddRange(new object[] { "Customer (lena)", "Supplier (dena)" }); _type.SelectedIndex = 0;
        _range.Items.AddRange(new object[] { "Sab", "Is mahine", "Pichla mahina", "Is saal" }); _range.SelectedIndex = 0;
        _dKind.Items.AddRange(new object[] { "Sab", "Lena", "Dena" }); _dKind.SelectedIndex = 0;

        var tk = new TabPage("Khata (Statement)");
        var tdue = new TabPage("Due List (kis se lena / kisko dena)");
        _tabs.TabPages.Add(tk); _tabs.TabPages.Add(tdue);
        Controls.Add(_tabs);

        // ---- khata
        var left = new Panel { Dock = DockStyle.Left, Width = 420 };
        var lt = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 84, Padding = new Padding(4) };
        var go = new Button { Text = "Search", Width = 80, Height = 30 };
        go.Click += async (s, e) => await LoadPartiesAsync();
        _search.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await LoadPartiesAsync(); } };
        lt.Controls.Add(_type); lt.Controls.Add(_search); lt.Controls.Add(go);
        left.Controls.Add(_gParty); left.Controls.Add(lt);

        var right = new Panel { Dock = DockStyle.Fill };
        var rt = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 96, Padding = new Padding(4), FlowDirection = FlowDirection.LeftToRight, WrapContents = true };
        rt.Controls.Add(_head); rt.SetFlowBreak(_head, true);
        rt.Controls.Add(new Label { Text = "Muddat", AutoSize = true, Margin = new Padding(6, 8, 3, 0) });
        rt.Controls.Add(_range);
        rt.Controls.Add(_act);
        var rb = new Panel { Dock = DockStyle.Bottom, Height = 44 }; rb.Controls.Add(_sum); _sum.Location = new Point(8, 10);
        right.Controls.Add(_gStmt); right.Controls.Add(rb); right.Controls.Add(rt);
        tk.Controls.Add(right); tk.Controls.Add(left);

        _type.SelectedIndexChanged += async (s, e) => { if (_ready) { UpdateActText(); await LoadPartiesAsync(); } };
        _range.SelectedIndexChanged += async (s, e) => { if (_ready) await LoadStatementAsync(); };
        _gParty.SelectionChanged += async (s, e) => { if (_ready && !_loading) await LoadStatementAsync(); };
        _act.Click += async (s, e) => await ActAsync();
        _gStmt.DataBindingComplete += (s, e) =>
        {
            foreach (DataGridViewRow r in _gStmt.Rows)
            {
                if (r.DataBoundItem is not StatementLine l) continue;
                if (l.Kind is "Receipt" or "Payment") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(228, 245, 228);
                else if (l.Kind == "Opening") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            }
            _gStmt.ClearSelection();
        };

        // ---- due list
        var dt = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(4) };
        var dr = new Button { Text = "Refresh", Width = 100, Height = 30 };
        dr.Click += async (s, e) => await LoadDueAsync();
        dt.Controls.Add(new Label { Text = "Dekhein", AutoSize = true, Margin = new Padding(6, 8, 3, 0) });
        dt.Controls.Add(_dKind); dt.Controls.Add(_dOver); dt.Controls.Add(dr);
        var db = new Panel { Dock = DockStyle.Bottom, Height = 44 }; db.Controls.Add(_dSum); _dSum.Location = new Point(8, 10);
        tdue.Controls.Add(_gDue); tdue.Controls.Add(db); tdue.Controls.Add(dt);
        _dKind.SelectedIndexChanged += async (s, e) => { if (_ready) await LoadDueAsync(); };
        _dOver.CheckedChanged += async (s, e) => { if (_ready) await LoadDueAsync(); };
        _gDue.DataBindingComplete += (s, e) =>
        {
            foreach (DataGridViewRow r in _gDue.Rows)
            {
                if (r.DataBoundItem is not DueItem d) continue;
                if (d.DaysOverdue > 0) r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 220, 220);
                else if ((d.DueDate.Date - DateTime.Today).TotalDays <= 7) r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 240, 205);
            }
            _gDue.ClearSelection();
        };
        _tabs.SelectedIndexChanged += async (s, e) => { if (_ready && _tabs.SelectedTab == tdue) await LoadDueAsync(); };

        UpdateActText();
        Load += async (s, e) => { _ready = true; await LoadPartiesAsync(); await LoadDueAsync(); };
    }

    private void UpdateActText() => _act.Text = IsCustomer ? "Receipt lein (paisa aaya)" : "Payment dein (paisa diya)";

    // ---------------------------------------------------------------- khata
    private async Task LoadPartiesAsync()
    {
        try
        {
            var keep = Selected?.Id;
            _loading = true;
            _parties = IsCustomer
                ? (await _fin.SearchCustomersAsync(_search.Text)).Select(c => new PartyRow(c.CustomerId, c.CustomerName, c.Phone, c.Balance, c.Overdue)).ToList()
                : (await _fin.GetSuppliersAsync(_search.Text)).Select(s => new PartyRow(s.SupplierId, s.SupplierName, s.Phone, s.Balance, s.Overdue)).ToList();
            _gParty.DataSource = _parties;
            if (_gParty.Columns.Contains("Id")) _gParty.Columns["Id"]!.Visible = false;
            H(_gParty, "Name", IsCustomer ? "Customer" : "Supplier");
            H(_gParty, "Balance", IsCustomer ? "Baqi (+ lena)" : "Baqi dena"); H(_gParty, "Overdue", "Guzra hua");
            N0(_gParty, "Balance", "Overdue");
            _gParty.ClearSelection();
            if (keep.HasValue)
            {
                var i = _parties.FindIndex(p => p.Id == keep.Value);
                if (i >= 0) { _gParty.Rows[i].Selected = true; _gParty.CurrentCell = _gParty.Rows[i].Cells[1]; }
            }
            _loading = false;
            await LoadStatementAsync();
        }
        catch (Exception ex) { _loading = false; MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private (DateTime? from, DateTime? to) Range()
    {
        var t = DateTime.Today;
        return _range.SelectedIndex switch
        {
            1 => (new DateTime(t.Year, t.Month, 1), t),
            2 => (new DateTime(t.Year, t.Month, 1).AddMonths(-1), new DateTime(t.Year, t.Month, 1).AddDays(-1)),
            3 => (new DateTime(t.Year, 1, 1), t),
            _ => (null, null)
        };
    }

    private async Task LoadStatementAsync()
    {
        try
        {
            var p = Selected;
            if (p == null) { _gStmt.DataSource = null; _head.Text = "Left list se party chunein"; _sum.Text = ""; return; }

            var (from, to) = Range();
            var r = IsCustomer ? await _fin.CustomerStatementAsync(p.Id, from, to) : await _fin.SupplierStatementAsync(p.Id, from, to);

            var lines = new List<StatementLine> { new() { Kind = "Opening", Ref = "Pichla baqi", Balance = r.OpeningBefore } };
            lines.AddRange(r.Lines);
            _gStmt.DataSource = lines;
            H(_gStmt, "Date", "Tareekh"); H(_gStmt, "Ref", "Reference"); H(_gStmt, "Kind", "Qisam");
            H(_gStmt, "Increase", IsCustomer ? "Sale / Refund (barha)" : "Bill (barha)");
            H(_gStmt, "Decrease", IsCustomer ? "Receipt (kam hua)" : "Payment (kam hua)");
            H(_gStmt, "Balance", "Baqi");
            D(_gStmt, "Date"); N0(_gStmt, "Increase", "Decrease", "Balance");

            var meaning = IsCustomer
                ? (r.Closing > 0 ? "customer ne dene hain" : r.Closing < 0 ? "customer ka advance hai" : "hisab saaf")
                : (r.Closing > 0 ? "humne dene hain" : r.Closing < 0 ? "supplier ne humein dene hain" : "hisab saaf");
            _head.Text = $"{p.Name}   {p.Phone}   |   Baqi {r.Closing:N0}  ({meaning})";
            _sum.Text = $"Pichla baqi {r.OpeningBefore:N0}   +   {(IsCustomer ? "Sale" : "Bills")} {r.TotalIncrease:N0}   -   {(IsCustomer ? "Receipts" : "Payments")} {r.TotalDecrease:N0}   =   {r.Closing:N0}";
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private async Task ActAsync()
    {
        var p = Selected;
        if (p == null) { MessageBox.Show("Pehle party chunein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        try
        {
            var accounts = (await _fin.GetAccountsAsync()).ToList();
            var methods = (await _setup.GetMethodsAsync(true)).Select(m => m.MethodName).ToList();
            if (accounts.Count == 0 || methods.Count == 0) { MessageBox.Show("Koi active account ya payment method nahi."); return; }

            var info = IsCustomer ? $"{p.Name}: abhi baqi {p.Balance:N0}" : $"{p.Name}: abhi dena {p.Balance:N0}";
            using var dlg = new MoneyDialog(IsCustomer ? "Receipt (customer se paisa aaya)" : "Payment (supplier ko paisa diya)",
                info, accounts, methods, false, IsCustomer ? "Kis account mein aaya" : "Kis account se diya");
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            _act.Enabled = false;
            if (IsCustomer)
                await _fin.CustomerReceiptAsync(p.Id, dlg.Amount, dlg.AccountId, dlg.Method, dlg.Date, dlg.Notes, dlg.Key, CurrentUser);
            else
                await _fin.SupplierPaymentAsync(p.Id, dlg.Amount, dlg.AccountId, dlg.Method, dlg.Date, dlg.Notes, dlg.Key, CurrentUser);

            MessageBox.Show("Entry ho gayi.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadPartiesAsync();
            await LoadDueAsync();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Entry nahi hui", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        finally { _act.Enabled = true; }
    }

    // ---------------------------------------------------------------- due list
    private async Task LoadDueAsync()
    {
        try
        {
            var all = (await _fin.GetDueAsync()).ToList();
            var list = all.Where(d => (_dKind.SelectedIndex == 0 || d.Kind == _dKind.Text) && (!_dOver.Checked || d.DaysOverdue > 0)).ToList();
            _gDue.DataSource = list;
            H(_gDue, "Kind", "Lena / Dena"); H(_gDue, "PartyName", "Party"); H(_gDue, "Ref", "Bill");
            H(_gDue, "DocDate", "Bill ki tareekh"); H(_gDue, "DueDate", "Due date"); H(_gDue, "Total", "Bill");
            H(_gDue, "Paid", "Mil gaya / diya"); H(_gDue, "Due", "Baqi"); H(_gDue, "DaysOverdue", "Kitne din late");
            if (_gDue.Columns.Contains("PartyId")) _gDue.Columns["PartyId"]!.Visible = false;
            D(_gDue, "DocDate", "DueDate"); N0(_gDue, "Total", "Paid", "Due");

            var lena = all.Where(d => d.Kind == "Lena"); var dena = all.Where(d => d.Kind == "Dena");
            _dSum.Text = $"LENA: {lena.Sum(d => d.Due):N0} (guzra {lena.Where(d => d.DaysOverdue > 0).Sum(d => d.Due):N0})     |     " +
                         $"DENA: {dena.Sum(d => d.Due):N0} (guzra {dena.Where(d => d.DaysOverdue > 0).Sum(d => d.Due):N0})     |     Laal = guzra hua, peela = 7 din ke andar";
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
}