using ClothStock.Business.Services;
using ClothStock.Models.Expenses;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Expenses;

public partial class WorkersForm : Form
{
    private readonly WorkerService _svc;
    private int _id = 0;
    private bool _loading = false;

    public WorkersForm()
    {
        InitializeComponent();
        _svc = Program.Services.GetRequiredService<WorkerService>();
    }

    private async void WorkersForm_Load(object? sender, EventArgs e)
    {
        await LoadGridAsync();
        ClearForm();
    }

    private async Task LoadGridAsync()
    {
        try
        {
            var list = (await _svc.GetAllAsync(false)).ToList();
            _loading = true;
            grid.DataSource = list;
            if (grid.Columns.Contains("WorkerId")) grid.Columns["WorkerId"]!.Visible = false;
            H("WorkerName", "Naam"); H("Role", "Kaam"); H("MonthlySalary", "Mahana Salary");
            H("IsActive", "Active"); H("AdvanceGiven", "Advance diya");
            H("AdvanceRecovered", "Salary se kata"); H("AdvanceBalance", "Baqi advance");
            foreach (var c in new[] { "MonthlySalary", "AdvanceGiven", "AdvanceRecovered", "AdvanceBalance" })
                if (grid.Columns.Contains(c)) grid.Columns[c]!.DefaultCellStyle.Format = "N0";
            grid.ClearSelection();
            _loading = false;

            var owed = list.Where(x => x.IsActive).Sum(x => x.AdvanceBalance);
            lblInfo.Text = $"Active workers: {list.Count(x => x.IsActive)}   |   Mahana salary ka jod: {list.Where(x => x.IsActive).Sum(x => x.MonthlySalary):N0}   |   " +
                           $"Workers ke ooper baqi advance: {owed:N0}";
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void H(string col, string text)
    {
        if (grid.Columns.Contains(col)) grid.Columns[col]!.HeaderText = text;
    }

    private void ClearForm()
    {
        _id = 0;
        txtName.Clear(); txtRole.Clear(); txtPhone.Clear();
        numSalary.Value = 0;
        chkActive.Checked = true;
        grid.ClearSelection();
        txtName.Focus();
    }

    private void btnNew_Click(object? sender, EventArgs e) => ClearForm();

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            btnSave.Enabled = false;
            await _svc.SaveAsync(new Worker
            {
                WorkerId = _id,
                WorkerName = txtName.Text,
                Role = string.IsNullOrWhiteSpace(txtRole.Text) ? null : txtRole.Text.Trim(),
                Phone = txtPhone.Text,
                MonthlySalary = numSalary.Value,
                IsActive = chkActive.Checked
            });
            MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadGridAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Save nahi hua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally { btnSave.Enabled = true; }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_id == 0)
        {
            MessageBox.Show("Pehle list se worker select karein.", "Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (MessageBox.Show($"Worker '{txtName.Text}' delete karna hai?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            await _svc.DeleteAsync(_id);
            await LoadGridAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Delete nahi ho saka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        if (grid.CurrentRow?.DataBoundItem is WorkerBalance w)
        {
            _id = w.WorkerId;
            txtName.Text = w.WorkerName;
            txtRole.Text = w.Role ?? "";
            txtPhone.Text = w.Phone ?? "";
            numSalary.Value = Math.Min(w.MonthlySalary, numSalary.Maximum);
            chkActive.Checked = w.IsActive;
        }
    }

    private void grid_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow r in grid.Rows)
            if (r.DataBoundItem is WorkerBalance w)
            {
                if (!w.IsActive) r.DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                else if (w.AdvanceBalance > 0) r.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 248, 220);
            }
    }
}