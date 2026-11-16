using ClothStock.Business.Services;
using ClothStock.Models.Expenses;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;


namespace ClothStockSystem.Forms.Expenses;

public partial class ExpenseForm : Form
{
    private static string CurrentUser => SessionManager.Username;
    //private const string CurrentUser = "owner";   // login banne par yahan asal user aaye ga

    private readonly ExpenseService _svc;
    private readonly ExpenseSetupService _setup;
    private readonly WorkerService _workers;

    private List<ExpenseCategory> _cats = new();
    private List<WorkerBalance> _workerList = new();
    private bool _ready = false;
    private bool _loading = false;
    private int _editId = 0;

    public ExpenseForm()
    {
        InitializeComponent();
        _svc = Program.Services.GetRequiredService<ExpenseService>();
        _setup = Program.Services.GetRequiredService<ExpenseSetupService>();
        _workers = Program.Services.GetRequiredService<WorkerService>();

        cmbChannel.Items.AddRange(new object[] { "Shop", "Online", "Shared" });
        cmbChannel.SelectedIndex = 0;
        cmbChFilter.Items.AddRange(new object[] { "Sab", "Shop", "Online", "Shared" });
        cmbChFilter.SelectedIndex = 0;
        dtpDate.MaxDate = DateTime.Today;
    }

    private ExpenseCategory? CurrentCategory => cmbCategory.SelectedItem as ExpenseCategory;
    private WorkerBalance? CurrentWorker => cmbWorker.SelectedItem as WorkerBalance;
    private ExpenseItem? Selected => grid.CurrentRow?.DataBoundItem as ExpenseItem;

    // ---------------------------------------------------------------- load

    private async void ExpenseForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _loading = true;
            _cats = (await _setup.GetCategoriesAsync(true)).ToList();
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DataSource = _cats;

            cmbMethod.DataSource = (await _setup.GetMethodsAsync(true)).Select(m => m.MethodName).ToList();
            _loading = false;

            await LoadWorkersAsync();

            _ready = true;
            if (CurrentCategory != null) cmbChannel.SelectedItem = CurrentCategory.DefaultChannel;
            ApplyKind();
            SetMode();
            await LoadListAsync();
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task LoadWorkersAsync()
    {
        var was = _loading;
        _loading = true;
        var keep = cmbWorker.SelectedValue as int?;
        _workerList = (await _workers.GetAllAsync(true)).ToList();
        cmbWorker.DisplayMember = "WorkerName";
        cmbWorker.ValueMember = "WorkerId";
        cmbWorker.DataSource = _workerList;
        if (keep.HasValue && _workerList.Any(w => w.WorkerId == keep.Value)) cmbWorker.SelectedValue = keep.Value;
        _loading = was;
    }

    // ---------------------------------------------------------------- entry form

    private void dtpDate_Changed(object? sender, EventArgs e)
    {
        if (_loading) return;
        dtpForMonth.Value = dtpDate.Value;       // aam tor par usi mahine ka kharcha
    }

    private void cmbCategory_Changed(object? sender, EventArgs e)
    {
        if (_loading || !_ready) return;
        var c = CurrentCategory;
        if (c != null) cmbChannel.SelectedItem = c.DefaultChannel;
        ApplyKind();
        if (c?.Kind == "Salary" && _editId == 0) SuggestSalary();
    }

    private void cmbWorker_Changed(object? sender, EventArgs e)
    {
        if (_loading || !_ready) return;
        if (CurrentCategory?.Kind == "Salary" && _editId == 0) SuggestSalary();
        UpdateHint();
    }

    private void Hint_Changed(object? sender, EventArgs e)
    {
        if (!_ready) return;
        UpdateHint();
    }

    // Salary: amount = mahana salary, advance kata = jitna baqi ho (aap badal sakte hain)
    private void SuggestSalary()
    {
        var w = CurrentWorker;
        if (w == null) return;
        numAmount.Value = Math.Min(w.MonthlySalary, numAmount.Maximum);
        numCut.Value = Math.Min(w.AdvanceBalance, numAmount.Value);
        UpdateHint();
    }

    private void ApplyKind()
    {
        var kind = CurrentCategory?.Kind ?? "Normal";
        var needsWorker = kind != "Normal";
        lblWorker.Enabled = cmbWorker.Enabled = needsWorker;
        lblCut.Enabled = numCut.Enabled = kind == "Salary";
        if (kind != "Salary") numCut.Value = 0;
        UpdateHint();
    }

    private void UpdateHint()
    {
        var c = CurrentCategory;
        if (c == null) { lblHint.Text = ""; return; }

        var w = c.Kind != "Normal" ? CurrentWorker : null;
        var who = w != null ? $"{w.WorkerName}: mahana salary {w.MonthlySalary:N0}, baqi advance {w.AdvanceBalance:N0}.   " : "";
        lblHint.ForeColor = System.Drawing.Color.DimGray;

        switch (c.Kind)
        {
            case "Advance":
                lblHint.Text = who + "Advance kharcha nahi, profit mein 0. Salary ki entry mein kata jaye ga.";
                break;
            case "Salary":
                lblHint.Text = who + $"Poori salary kharcha ginti hai. Cash nikle ga: {numAmount.Value - numCut.Value:N0}";
                if (w != null && _editId == 0 && numCut.Value > w.AdvanceBalance)
                {
                    lblHint.Text = who + "Advance kata baqi advance se zyada hai!";
                    lblHint.ForeColor = System.Drawing.Color.Firebrick;
                }
                break;
            case "Bonus":
                lblHint.Text = who + "Bonus poora kharcha ginta hai.";
                break;
            default:
                lblHint.Text = "Dhyan: courier / COD charge, return loss aur order ki packaging yahan na likhein (orders mein pehle se kat chuki hain).";
                break;
        }
    }

    private void SetMode()
    {
        var edit = _editId > 0;
        lblMode.Text = edit ? $"EDIT MODE: entry #{_editId}" : "Naya kharcha";
        lblMode.ForeColor = edit ? System.Drawing.Color.DarkOrange : System.Drawing.Color.DimGray;
        btnSave.Text = edit ? "Update" : "Save";
        btnNew.Text = edit ? "Cancel" : "New";
    }

    private void ClearForm()
    {
        _loading = true;
        _editId = 0;
        dtpDate.Value = DateTime.Today;
        dtpForMonth.Value = DateTime.Today;
        numAmount.Value = 0;
        numCut.Value = 0;
        txtNotes.Clear();
        _loading = false;
        if (CurrentCategory != null) cmbChannel.SelectedItem = CurrentCategory.DefaultChannel;
        SetMode();
        ApplyKind();
    }

    private void btnNew_Click(object? sender, EventArgs e) => ClearForm();

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            var c = CurrentCategory;
            if (c == null)
            {
                MessageBox.Show("Category select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? workerId = null;
            if (c.Kind != "Normal" && cmbWorker.SelectedValue is int wid) workerId = wid;

            var req = new ExpenseSaveRequest
            {
                ExpenseId = _editId,
                ExpenseDate = dtpDate.Value.Date,
                ForMonth = new DateTime(dtpForMonth.Value.Year, dtpForMonth.Value.Month, 1),
                CategoryId = c.CategoryId,
                Channel = cmbChannel.Text,
                Amount = numAmount.Value,
                WorkerId = workerId,
                AdvanceCut = numCut.Value,
                PaymentMethod = cmbMethod.Text,
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                User = CurrentUser
            };

            btnSave.Enabled = false;
            await _svc.SaveAsync(req);
            MessageBox.Show(_editId > 0 ? "Update ho gaya." : "Kharcha save ho gaya.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearForm();
            await LoadWorkersAsync();
            await LoadListAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { btnSave.Enabled = true; }
    }

    // ---------------------------------------------------------------- list

    private async void Filter_Changed(object? sender, EventArgs e)
    {
        if (_ready) await LoadListAsync();
    }

    private async void chkAllMonths_Changed(object? sender, EventArgs e)
    {
        dtpMonth.Enabled = !chkAllMonths.Checked;
        if (_ready) await LoadListAsync();
    }

    private async void btnSearch_Click(object? sender, EventArgs e) => await LoadListAsync();

    private async void txtSearch_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            await LoadListAsync();
        }
    }

    private static void Hdr(DataGridView g, string col, string text)
    {
        if (g.Columns.Contains(col)) g.Columns[col]!.HeaderText = text;
    }

    private async Task LoadListAsync()
    {
        try
        {
            var f = new ExpenseFilter
            {
                Month = chkAllMonths.Checked ? (DateTime?)null : dtpMonth.Value,
                Channel = cmbChFilter.SelectedIndex > 0 ? cmbChFilter.Text : null,
                IncludeVoid = chkVoid.Checked,
                Search = txtSearch.Text
            };
            var list = (await _svc.GetAsync(f)).ToList();

            grid.DataSource = list;
            string[] keep = { "ExpenseDate", "PeriodMonth", "CategoryName", "Channel", "WorkerName", "Amount",
                              "AdvanceCut", "ExpenseAmount", "CashOut", "PaymentMethod", "Notes", "Status" };
            foreach (DataGridViewColumn c in grid.Columns) c.Visible = keep.Contains(c.Name);
            Hdr(grid, "ExpenseDate", "Tareekh"); Hdr(grid, "PeriodMonth", "Mahina");
            Hdr(grid, "CategoryName", "Category"); Hdr(grid, "WorkerName", "Worker");
            Hdr(grid, "AdvanceCut", "Advance kata"); Hdr(grid, "ExpenseAmount", "Kharcha (profit mein)");
            Hdr(grid, "CashOut", "Cash nikla"); Hdr(grid, "PaymentMethod", "Payment");
            if (grid.Columns.Contains("ExpenseDate")) grid.Columns["ExpenseDate"]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            if (grid.Columns.Contains("PeriodMonth")) grid.Columns["PeriodMonth"]!.DefaultCellStyle.Format = "MMM-yyyy";
            foreach (var c in new[] { "Amount", "AdvanceCut", "ExpenseAmount", "CashOut" })
                if (grid.Columns.Contains(c)) grid.Columns[c]!.DefaultCellStyle.Format = "N0";
            grid.ClearSelection();

            ShowSummary(list);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ShowSummary(List<ExpenseItem> list)
    {
        var act = list.Where(x => x.Status == "Active").ToList();
        var voided = list.Count - act.Count;

        var shop = act.Sum(x => x.ShopPart);
        var online = act.Sum(x => x.OnlinePart);
        var total = act.Sum(x => x.ExpenseAmount);
        var cash = act.Sum(x => x.CashOut);
        var byPay = string.Join("  |  ", act.GroupBy(x => x.PaymentMethod).Select(g => $"{g.Key} {g.Sum(x => x.CashOut):N0}"));
        var adv = act.Where(x => x.Kind == "Advance").Sum(x => x.Amount);
        var pay = act.Where(x => x.Kind == "Salary" || x.Kind == "Bonus").Sum(x => x.ExpenseAmount);

        lblSum1.Text = $"KHARCHA (profit se kat'ta hai):   Shop {shop:N0}   |   Online {online:N0}   |   TOTAL {total:N0}";
        lblSum2.Text = $"Cash nikla: {cash:N0}" + (byPay.Length > 0 ? $" ({byPay})" : "") +
                       $"   |   Salary + Bonus: {pay:N0}   |   Advance diye (profit mein nahi): {adv:N0}   |   Entries: {act.Count}" +
                       (voided > 0 ? $"   |   Delete hui: {voided}" : "");
    }

    private void grid_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow r in grid.Rows)
        {
            if (r.DataBoundItem is not ExpenseItem x) continue;
            if (x.Status == "Void") r.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
            else if (x.Kind == "Advance") r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 248, 220);
        }
    }

    // ---------------------------------------------------------------- edit / delete / fixed

    private void btnEdit_Click(object? sender, EventArgs e)
    {
        var x = Selected;
        if (x == null)
        {
            MessageBox.Show("Pehle list se entry select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (x.Status == "Void")
        {
            MessageBox.Show("Delete hui entry edit nahi hoti.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!_cats.Any(c => c.CategoryId == x.CategoryId))
        {
            MessageBox.Show("Is entry ki category band hai. Expense Setup mein pehle Active karein.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _loading = true;
        cmbCategory.SelectedValue = x.CategoryId;
        cmbChannel.SelectedItem = x.Channel;
        if (x.WorkerId.HasValue) cmbWorker.SelectedValue = x.WorkerId.Value;
        dtpDate.Value = x.ExpenseDate;
        dtpForMonth.Value = x.PeriodMonth;
        numAmount.Value = Math.Min(x.Amount, numAmount.Maximum);
        numCut.Value = Math.Min(x.AdvanceCut, numCut.Maximum);
        cmbMethod.SelectedItem = x.PaymentMethod;
        txtNotes.Text = x.Notes ?? "";
        _loading = false;

        _editId = x.ExpenseId;
        SetMode();
        ApplyKind();
        numCut.Value = Math.Min(x.AdvanceCut, numCut.Maximum);
    }

    private async void btnVoid_Click(object? sender, EventArgs e)
    {
        var x = Selected;
        if (x == null)
        {
            MessageBox.Show("Pehle list se entry select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (x.Status == "Void")
        {
            MessageBox.Show("Ye entry pehle se delete hai.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"Ye kharcha delete karna hai?\n\n{x.ExpenseDate:dd-MMM-yyyy}  |  {x.CategoryName}  |  {x.WorkerName}\nAmount: {x.Amount:N0}\n\n" +
            "Total se nikal jaye ga. Entry list mein grey record rahe gi.",
            "Delete confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;

        try
        {
            await _svc.VoidAsync(x.ExpenseId, CurrentUser);
            if (_editId == x.ExpenseId) ClearForm();
            await LoadWorkersAsync();
            await LoadListAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Delete nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void btnGenerate_Click(object? sender, EventArgs e)
    {
        var m = new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
        var confirm = MessageBox.Show(
            $"{m:MMM-yyyy} ke fixed kharche (rent, bills, salary) banane hain?\n\n" +
            "Jo is mahine mein pehle ban chuke hain wo dobara nahi banen ge.\nBanne ke baad har entry ka amount badal sakte hain.",
            "Fixed kharche", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        try
        {
            var n = await _svc.GenerateRecurringAsync(m, CurrentUser);
            MessageBox.Show(n == 0
                    ? "Koi nayi entry nahi bani (ya to pehle ban chuki hain, ya Expense Setup mein fixed kharche abhi nahi bhare)."
                    : $"{n} entries ban gayin. Ab zaroorat ho to amount edit karein (jaise bijli ka bill).",
                "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            chkAllMonths.Checked = false;
            await LoadListAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}