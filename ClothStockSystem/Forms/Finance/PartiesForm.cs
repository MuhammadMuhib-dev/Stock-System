using ClothStock.Business.Services;
using ClothStock.Models.Finance;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Finance;

public class PartiesForm : Form
{
    private readonly FinanceService _fin;
    private readonly TabControl _tabs = new() { Dock = DockStyle.Fill };

    // customers
    private readonly DataGridView _gc = MkGrid();
    private readonly TextBox _cName = new() { Width = 200, MaxLength = 100 };
    private readonly TextBox _cPhone = new() { Width = 150, MaxLength = 15, PlaceholderText = "03001234567" };
    private readonly TextBox _cAddr = new() { Width = 260, MaxLength = 250 };
    private readonly NumericUpDown _cLimit = Num();
    private readonly NumericUpDown _cOpen = Num();
    private readonly CheckBox _cActive = new() { Text = "Active", Checked = true, AutoSize = true };
    private int _cId; private bool _cLoad;

    // suppliers
    private readonly DataGridView _gs = MkGrid();
    private readonly TextBox _sName = new() { Width = 200, MaxLength = 100 };
    private readonly TextBox _sPhone = new() { Width = 150, MaxLength = 15, PlaceholderText = "03001234567" };
    private readonly TextBox _sAddr = new() { Width = 260, MaxLength = 250 };
    private readonly NumericUpDown _sOpen = Num();
    private readonly CheckBox _sActive = new() { Text = "Active", Checked = true, AutoSize = true };
    private int _sId; private bool _sLoad;

    // accounts
    private readonly DataGridView _ga = MkGrid();
    private readonly TextBox _aName = new() { Width = 180, MaxLength = 50 };
    private readonly ComboBox _aType = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110 };
    private readonly NumericUpDown _aOpen = Num();
    private readonly CheckBox _aActive = new() { Text = "Active", Checked = true, AutoSize = true };
    private int _aId; private bool _aLoad;

    private static DataGridView MkGrid() => new()
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
    private static NumericUpDown Num() => new() { DecimalPlaces = 2, Maximum = 1000000000, ThousandsSeparator = true, Width = 130, TextAlign = HorizontalAlignment.Right };

    private static void Hide(DataGridView g, params string[] cols) { foreach (var c in cols) if (g.Columns.Contains(c)) g.Columns[c]!.Visible = false; }
    private static void H(DataGridView g, string col, string text) { if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text; }
    private static void N0(DataGridView g, params string[] cols) { foreach (var c in cols) if (g.Columns.Contains(c)) { g.Columns[c]!.DefaultCellStyle.Format = "N0"; g.Columns[c]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; } }

    private static void Add(FlowLayoutPanel p, string label, Control c)
    { p.Controls.Add(new Label { Text = label, AutoSize = true, Margin = new Padding(10, 8, 3, 0) }); p.Controls.Add(c); }

    private static Button Btn(string text, Func<Task> act)
    {
        var b = new Button { Text = text, Width = 100, Height = 32, Margin = new Padding(8, 4, 3, 3) };
        b.Click += async (s, e) => { try { b.Enabled = false; await act(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning); } finally { b.Enabled = true; } };
        return b;
    }

    public PartiesForm()
    {
        _fin = Program.Services.GetRequiredService<FinanceService>();
        Text = "Parties"; Font = new System.Drawing.Font("Segoe UI", 10.5F);
        Size = new Size(1100, 700); StartPosition = FormStartPosition.CenterScreen;
        _aType.Items.AddRange(new object[] { "Cash", "Bank", "Wallet" }); _aType.SelectedIndex = 0;

        _tabs.TabPages.Add(CustomersTab());
        _tabs.TabPages.Add(SuppliersTab());
        _tabs.TabPages.Add(AccountsTab());
        Controls.Add(_tabs);
        Load += async (s, e) => { await LoadCustomers(); await LoadSuppliers(); await LoadAccounts(); };
    }

    // ================================================================ customers
    private TabPage CustomersTab()
    {
        var pg = new TabPage("Customers");
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 150, Padding = new Padding(6) };
        Add(top, "Naam", _cName); Add(top, "Phone", _cPhone); Add(top, "Address", _cAddr);
        Add(top, "Credit limit (0 = udhaar nahi)", _cLimit); Add(top, "Purana baqi (opening)", _cOpen);
        top.Controls.Add(_cActive);
        top.Controls.Add(Btn("Save", SaveCustomer)); top.Controls.Add(Btn("New", () => { ClearCustomer(); return Task.CompletedTask; }));
        pg.Controls.Add(_gc); pg.Controls.Add(top);
        _gc.SelectionChanged += (s, e) =>
        {
            if (_cLoad || _gc.CurrentRow?.DataBoundItem is not CustomerBalance c) return;
            _cId = c.CustomerId; _cName.Text = c.CustomerName; _cPhone.Text = c.Phone ?? ""; _cAddr.Text = "";
            _cLimit.Value = Math.Min(c.CreditLimit, _cLimit.Maximum); _cOpen.Value = Math.Min(c.OpeningBalance, _cOpen.Maximum);
            _cActive.Checked = c.IsActive;
        };
        return pg;
    }

    private async Task LoadCustomers()
    {
        var list = (await _fin.SearchCustomersAsync(null, true)).ToList();
        _cLoad = true; _gc.DataSource = list;
        Hide(_gc, "CustomerId", "Sales", "Received", "Refunded", "OldestDue");
        H(_gc, "CustomerName", "Customer"); H(_gc, "CreditLimit", "Credit limit"); H(_gc, "OpeningBalance", "Opening");
        H(_gc, "IsActive", "Active"); H(_gc, "Balance", "Baqi (+ lena / - advance)"); H(_gc, "Overdue", "Guzra hua");
        N0(_gc, "CreditLimit", "OpeningBalance", "Balance", "Overdue");
        _gc.ClearSelection(); _cLoad = false;
    }

    private void ClearCustomer()
    { _cId = 0; _cName.Clear(); _cPhone.Clear(); _cAddr.Clear(); _cLimit.Value = 0; _cOpen.Value = 0; _cActive.Checked = true; _gc.ClearSelection(); _cName.Focus(); }

    private async Task SaveCustomer()
    {
        await _fin.SaveCustomerAsync(new Customer
        {
            CustomerId = _cId,
            CustomerName = _cName.Text,
            Phone = _cPhone.Text,
            Address = string.IsNullOrWhiteSpace(_cAddr.Text) ? null : _cAddr.Text.Trim(),
            CreditLimit = _cLimit.Value,
            OpeningBalance = _cOpen.Value,
            IsActive = _cActive.Checked
        });
        MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        await LoadCustomers(); ClearCustomer();
    }

    // ================================================================ suppliers
    private TabPage SuppliersTab()
    {
        var pg = new TabPage("Suppliers");
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 150, Padding = new Padding(6) };
        Add(top, "Naam", _sName); Add(top, "Phone", _sPhone); Add(top, "Address", _sAddr);
        Add(top, "Purana dena (opening)", _sOpen);
        top.Controls.Add(_sActive);
        top.Controls.Add(Btn("Save", SaveSupplier)); top.Controls.Add(Btn("New", () => { ClearSupplier(); return Task.CompletedTask; }));
        pg.Controls.Add(_gs); pg.Controls.Add(top);
        _gs.SelectionChanged += (s, e) =>
        {
            if (_sLoad || _gs.CurrentRow?.DataBoundItem is not SupplierBalance x) return;
            _sId = x.SupplierId; _sName.Text = x.SupplierName; _sPhone.Text = x.Phone ?? ""; _sAddr.Text = "";
            _sOpen.Value = Math.Min(x.OpeningBalance, _sOpen.Maximum); _sActive.Checked = x.IsActive;
        };
        return pg;
    }

    private async Task LoadSuppliers()
    {
        var list = (await _fin.GetSuppliersAsync(null, true)).ToList();
        _sLoad = true; _gs.DataSource = list;
        Hide(_gs, "SupplierId", "OldestDue");
        H(_gs, "SupplierName", "Supplier"); H(_gs, "OpeningBalance", "Opening"); H(_gs, "IsActive", "Active");
        H(_gs, "Billed", "Bills"); H(_gs, "Paid", "Diya"); H(_gs, "Balance", "Baqi dena"); H(_gs, "Overdue", "Guzra hua");
        N0(_gs, "OpeningBalance", "Billed", "Paid", "Balance", "Overdue");
        _gs.ClearSelection(); _sLoad = false;
    }

    private void ClearSupplier()
    { _sId = 0; _sName.Clear(); _sPhone.Clear(); _sAddr.Clear(); _sOpen.Value = 0; _sActive.Checked = true; _gs.ClearSelection(); _sName.Focus(); }

    private async Task SaveSupplier()
    {
        await _fin.SaveSupplierAsync(new Supplier
        {
            SupplierId = _sId,
            SupplierName = _sName.Text,
            Phone = _sPhone.Text,
            Address = string.IsNullOrWhiteSpace(_sAddr.Text) ? null : _sAddr.Text.Trim(),
            OpeningBalance = _sOpen.Value,
            IsActive = _sActive.Checked
        });
        MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        await LoadSuppliers(); ClearSupplier();
    }

    // ================================================================ accounts
    private TabPage AccountsTab()
    {
        var pg = new TabPage("Accounts (Cash / Bank)");
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 130, Padding = new Padding(6) };
        Add(top, "Naam", _aName); Add(top, "Qisam", _aType); Add(top, "Opening balance", _aOpen);
        top.Controls.Add(_aActive);
        top.Controls.Add(Btn("Save", SaveAccount)); top.Controls.Add(Btn("New", () => { ClearAccount(); return Task.CompletedTask; }));
        top.Controls.Add(new Label
        {
            Text = "Opening balance sirf shuru mein set karein. Baad mein badalne se purana hisab badal jata hai. JazzCash jaisa naya account yahin se banayen (Qisam: Wallet).",
            AutoSize = false,
            Width = 900,
            Height = 40,
            ForeColor = System.Drawing.Color.DimGray,
            Margin = new Padding(10, 6, 3, 0)
        });
        pg.Controls.Add(_ga); pg.Controls.Add(top);
        _ga.SelectionChanged += (s, e) =>
        {
            if (_aLoad || _ga.CurrentRow?.DataBoundItem is not AccountBalance a) return;
            _aId = a.AccountId; _aName.Text = a.AccountName; _aType.SelectedItem = a.AccountType;
            _aOpen.Value = Math.Min(a.OpeningBalance, _aOpen.Maximum); _aActive.Checked = a.IsActive;
        };
        return pg;
    }

    private async Task LoadAccounts()
    {
        var list = (await _fin.GetAccountBalancesAsync()).ToList();
        _aLoad = true; _ga.DataSource = list;
        Hide(_ga, "AccountId");
        H(_ga, "AccountName", "Account"); H(_ga, "AccountType", "Qisam"); H(_ga, "IsActive", "Active");
        H(_ga, "OpeningBalance", "Opening"); H(_ga, "Receipts", "Aaya"); H(_ga, "Payments", "Gaya");
        H(_ga, "TransferIn", "Transfer in"); H(_ga, "TransferOut", "Transfer out"); H(_ga, "Balance", "Abhi balance");
        N0(_ga, "OpeningBalance", "Receipts", "Payments", "TransferIn", "TransferOut", "Balance");
        _ga.ClearSelection(); _aLoad = false;
    }

    private void ClearAccount()
    { _aId = 0; _aName.Clear(); _aType.SelectedIndex = 0; _aOpen.Value = 0; _aActive.Checked = true; _ga.ClearSelection(); _aName.Focus(); }

    private async Task SaveAccount()
    {
        await _fin.SaveAccountAsync(_aId, _aName.Text, _aType.Text, _aOpen.Value, _aActive.Checked);
        MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
        await LoadAccounts(); ClearAccount();
    }
}