using ClothStock.Business.Services;
using ClothStock.Models.Finance;

namespace ClothStockSystem.Forms.Orders;

public class CustomerPickerDialog : Form
{
    private readonly FinanceService _fin;
    private readonly TextBox _search = new() { Width = 300, PlaceholderText = "Naam ya phone" };
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AllowUserToAddRows = false,
        RowHeadersVisible = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = System.Drawing.Color.White
    };
    private readonly TextBox _nName = new() { Width = 180 };
    private readonly TextBox _nPhone = new() { Width = 140, PlaceholderText = "03001234567" };
    private readonly NumericUpDown _nLimit = new() { Width = 120, Maximum = 1000000000, ThousandsSeparator = true };

    public CustomerBalance? Selected { get; private set; }     // null + OK = walk-in

    public CustomerPickerDialog(FinanceService fin)
    {
        _fin = fin;
        Text = "Customer chunein";
        Font = new System.Drawing.Font("Segoe UI", 10.5F);
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(900, 600);

        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 52, Padding = new Padding(8) };
        var go = new Button { Text = "Search", Width = 90, Height = 30 };
        go.Click += async (s, e) => await ReloadAsync();
        _search.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await ReloadAsync(); } };
        top.Controls.Add(_search); top.Controls.Add(go);

        var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 120, Padding = new Padding(8) };
        var pick = new Button { Text = "Ye customer chunein", Width = 190, Height = 34 };
        var walkin = new Button { Text = "Walk-in (customer hatao)", Width = 210, Height = 34 };
        pick.Click += (s, e) =>
        {
            if (_grid.CurrentRow?.DataBoundItem is not CustomerBalance c)
            { MessageBox.Show("Pehle list se customer select karein."); return; }
            Selected = c; DialogResult = DialogResult.OK;
        };
        walkin.Click += (s, e) => { Selected = null; DialogResult = DialogResult.OK; };
        bottom.Controls.Add(pick); bottom.Controls.Add(walkin);
        bottom.SetFlowBreak(walkin, true);

        bottom.Controls.Add(new Label { Text = "Naya customer:", AutoSize = true, Margin = new Padding(3, 10, 3, 0) });
        bottom.Controls.Add(_nName); bottom.Controls.Add(_nPhone);
        bottom.Controls.Add(new Label { Text = "Credit limit", AutoSize = true, Margin = new Padding(6, 10, 3, 0) });
        bottom.Controls.Add(_nLimit);
        var add = new Button { Text = "Save aur chunein", Width = 150, Height = 32 };
        add.Click += async (s, e) => await AddAsync();
        bottom.Controls.Add(add);

        Controls.Add(_grid); Controls.Add(top); Controls.Add(bottom);
        _grid.CellDoubleClick += (s, e) => { if (_grid.CurrentRow?.DataBoundItem is CustomerBalance c) { Selected = c; DialogResult = DialogResult.OK; } };
        Load += async (s, e) => { await ReloadAsync(); _search.Focus(); };
    }

    private async Task ReloadAsync()
    {
        try
        {
            _grid.DataSource = (await _fin.SearchCustomersAsync(_search.Text)).ToList();
            foreach (var c in new[] { "CustomerId", "IsActive", "OpeningBalance", "Sales", "Received", "Refunded", "OldestDue" })
                if (_grid.Columns.Contains(c)) _grid.Columns[c]!.Visible = false;
            if (_grid.Columns.Contains("CustomerName")) _grid.Columns["CustomerName"]!.HeaderText = "Customer";
            if (_grid.Columns.Contains("CreditLimit")) _grid.Columns["CreditLimit"]!.HeaderText = "Credit limit";
            if (_grid.Columns.Contains("Balance")) _grid.Columns["Balance"]!.HeaderText = "Baqi (+ lena, - advance)";
            if (_grid.Columns.Contains("Overdue")) _grid.Columns["Overdue"]!.HeaderText = "Guzra hua udhaar";
            foreach (var c in new[] { "CreditLimit", "Balance", "Overdue" })
                if (_grid.Columns.Contains(c)) _grid.Columns[c]!.DefaultCellStyle.Format = "N0";
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private async Task AddAsync()
    {
        try
        {
            var id = await _fin.AddCustomerAsync(_nName.Text, _nPhone.Text, _nLimit.Value);
            Selected = await _fin.GetCustomerAsync(id);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}