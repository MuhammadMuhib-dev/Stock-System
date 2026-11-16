using ClothStock.Business.Services;
using ClothStock.Models.Expenses;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Expenses;

public partial class ExpenseSetupForm : Form
{
    private readonly ExpenseSetupService _setup;
    private readonly WorkerService _workers;

    private int _catId = 0;
    private int _recId = 0;
    private bool _catLoading = false;
    private bool _payLoading = false;
    private bool _recLoading = false;

    private record WorkerOpt(int Id, string Name);

    public ExpenseSetupForm()
    {
        InitializeComponent();
        _setup = Program.Services.GetRequiredService<ExpenseSetupService>();
        _workers = Program.Services.GetRequiredService<WorkerService>();

        cmbKind.Items.AddRange(new object[] { "Normal", "Salary", "Bonus", "Advance" });
        cmbKind.SelectedIndex = 0;
        cmbCCh.Items.AddRange(new object[] { "Shop", "Online", "Shared" });
        cmbCCh.SelectedIndex = 0;
        cmbRCh.Items.AddRange(new object[] { "Shop", "Online", "Shared" });
        cmbRCh.SelectedIndex = 0;
    }

    private static void Hdr(DataGridView g, string col, string text)
    {
        if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text;
    }

    private async void ExpenseSetupForm_Load(object? sender, EventArgs e)
    {
        try
        {
            await LoadCategoriesAsync();
            await LoadMethodsAsync();
            await LoadRecLookupsAsync();
            await LoadRecurringAsync();
            ClearCat();
            ClearRec();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ================================================================ categories

    private async Task LoadCategoriesAsync()
    {
        var list = (await _setup.GetCategoriesAsync(false)).ToList();
        _catLoading = true;
        gridCat.DataSource = list;
        if (gridCat.Columns.Contains("CategoryId")) gridCat.Columns["CategoryId"]!.Visible = false;
        Hdr(gridCat, "CategoryName", "Category"); Hdr(gridCat, "GroupName", "Group");
        Hdr(gridCat, "Kind", "Qisam"); Hdr(gridCat, "DefaultChannel", "Default channel"); Hdr(gridCat, "IsActive", "Active");
        gridCat.ClearSelection();
        _catLoading = false;
    }

    private void ClearCat()
    {
        _catId = 0;
        txtCName.Clear(); txtCGroup.Clear();
        cmbKind.SelectedIndex = 0; cmbCCh.SelectedIndex = 0;
        chkCActive.Checked = true;
        gridCat.ClearSelection();
        txtCName.Focus();
    }

    private void btnCNew_Click(object? sender, EventArgs e) => ClearCat();

    private void gridCat_SelectionChanged(object? sender, EventArgs e)
    {
        if (_catLoading) return;
        if (gridCat.CurrentRow?.DataBoundItem is ExpenseCategory c)
        {
            _catId = c.CategoryId;
            txtCName.Text = c.CategoryName;
            txtCGroup.Text = c.GroupName;
            cmbKind.SelectedItem = c.Kind;
            cmbCCh.SelectedItem = c.DefaultChannel;
            chkCActive.Checked = c.IsActive;
        }
    }

    private async void btnCSave_Click(object? sender, EventArgs e)
    {
        try
        {
            btnCSave.Enabled = false;
            await _setup.SaveCategoryAsync(new ExpenseCategory
            {
                CategoryId = _catId,
                CategoryName = txtCName.Text,
                GroupName = txtCGroup.Text,
                Kind = cmbKind.Text,
                DefaultChannel = cmbCCh.Text,
                IsActive = chkCActive.Checked
            });
            MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadCategoriesAsync();
            await LoadRecLookupsAsync();
            ClearCat();
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
                ? "Ye category pehle se maujood hai." : ex.Message;
            MessageBox.Show(msg, "Save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { btnCSave.Enabled = true; }
    }

    private async void btnCDelete_Click(object? sender, EventArgs e)
    {
        if (_catId == 0)
        {
            MessageBox.Show("Pehle list se category select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (MessageBox.Show($"Category '{txtCName.Text}' delete karni hai?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            await _setup.DeleteCategoryAsync(_catId);
            await LoadCategoriesAsync();
            await LoadRecLookupsAsync();
            ClearCat();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Delete nahi ho saki", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ================================================================ payment methods

    private async Task LoadMethodsAsync()
    {
        var list = (await _setup.GetMethodsAsync(false)).ToList();
        _payLoading = true;
        gridPay.DataSource = list;
        Hdr(gridPay, "MethodName", "Payment method"); Hdr(gridPay, "IsActive", "Active");
        gridPay.ClearSelection();
        _payLoading = false;
    }

    private async void btnMAdd_Click(object? sender, EventArgs e)
    {
        try
        {
            await _setup.AddMethodAsync(txtMName.Text);
            txtMName.Clear();
            await LoadMethodsAsync();
            await LoadRecLookupsAsync();
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("PRIMARY KEY", StringComparison.OrdinalIgnoreCase)
                ? "Ye method pehle se maujood hai." : ex.Message;
            MessageBox.Show(msg, "Add nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnMToggle_Click(object? sender, EventArgs e)
    {
        if (gridPay.CurrentRow?.DataBoundItem is not PaymentMethod m)
        {
            MessageBox.Show("Pehle list se method select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            await _setup.SetMethodActiveAsync(m.MethodName, !m.IsActive);
            await LoadMethodsAsync();
            await LoadRecLookupsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // ================================================================ fixed kharche

    private async Task LoadRecLookupsAsync()
    {
        _recLoading = true;
        var cats = (await _setup.GetCategoriesAsync(true)).ToList();
        cmbRCat.DisplayMember = "CategoryName"; cmbRCat.ValueMember = "CategoryId"; cmbRCat.DataSource = cats;

        var ws = (await _workers.GetAllAsync(true)).Select(w => new WorkerOpt(w.WorkerId, w.WorkerName)).ToList();
        ws.Insert(0, new WorkerOpt(0, "-- koi nahi --"));
        cmbRWorker.DisplayMember = "Name"; cmbRWorker.ValueMember = "Id"; cmbRWorker.DataSource = ws;

        cmbRMethod.DataSource = (await _setup.GetMethodsAsync(true)).Select(m => m.MethodName).ToList();
        _recLoading = false;
    }

    private async Task LoadRecurringAsync()
    {
        var list = (await _setup.GetRecurringAsync()).ToList();
        _recLoading = true;
        gridRec.DataSource = list;
        foreach (var c in new[] { "RecurringId", "CategoryId", "WorkerId" })
            if (gridRec.Columns.Contains(c)) gridRec.Columns[c]!.Visible = false;
        Hdr(gridRec, "CategoryName", "Category"); Hdr(gridRec, "WorkerName", "Worker");
        Hdr(gridRec, "Amount", "Amount (khali = salary)"); Hdr(gridRec, "PaymentMethod", "Payment"); Hdr(gridRec, "IsActive", "Active");
        if (gridRec.Columns.Contains("Amount")) gridRec.Columns["Amount"]!.DefaultCellStyle.Format = "N0";
        gridRec.ClearSelection();
        _recLoading = false;
    }

    private void ClearRec()
    {
        _recId = 0;
        numRAmt.Value = 0;
        chkRSalary.Checked = false;
        txtRNotes.Clear();
        chkRActive.Checked = true;
        if (cmbRWorker.Items.Count > 0) cmbRWorker.SelectedIndex = 0;
        gridRec.ClearSelection();
    }

    private void chkRSalary_Changed(object? sender, EventArgs e) => numRAmt.Enabled = !chkRSalary.Checked;

    private void btnRNew_Click(object? sender, EventArgs e) => ClearRec();

    private void gridRec_SelectionChanged(object? sender, EventArgs e)
    {
        if (_recLoading) return;
        if (gridRec.CurrentRow?.DataBoundItem is RecurringItem r)
        {
            _recId = r.RecurringId;
            cmbRCat.SelectedValue = r.CategoryId;
            cmbRCh.SelectedItem = r.Channel;
            cmbRWorker.SelectedValue = r.WorkerId ?? 0;
            chkRSalary.Checked = r.Amount == null;
            numRAmt.Value = Math.Min(r.Amount ?? 0, numRAmt.Maximum);
            cmbRMethod.SelectedItem = r.PaymentMethod;
            txtRNotes.Text = r.Notes ?? "";
            chkRActive.Checked = r.IsActive;
        }
    }

    private async void btnRSave_Click(object? sender, EventArgs e)
    {
        try
        {
            int? workerId = cmbRWorker.SelectedValue is int w && w > 0 ? w : null;
            decimal? amount = chkRSalary.Checked ? null : numRAmt.Value;

            btnRSave.Enabled = false;
            await _setup.SaveRecurringAsync(new RecurringItem
            {
                RecurringId = _recId,
                CategoryId = cmbRCat.SelectedValue is int c ? c : 0,
                Channel = cmbRCh.Text,
                WorkerId = workerId,
                Amount = amount,
                PaymentMethod = cmbRMethod.Text,
                Notes = string.IsNullOrWhiteSpace(txtRNotes.Text) ? null : txtRNotes.Text.Trim(),
                IsActive = chkRActive.Checked
            });
            MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadRecurringAsync();
            ClearRec();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { btnRSave.Enabled = true; }
    }

    private async void btnRDelete_Click(object? sender, EventArgs e)
    {
        if (_recId == 0)
        {
            MessageBox.Show("Pehle list se entry select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (MessageBox.Show("Ye fixed kharcha delete karna hai?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            await _setup.DeleteRecurringAsync(_recId);
            await LoadRecurringAsync();
            ClearRec();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Delete nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}