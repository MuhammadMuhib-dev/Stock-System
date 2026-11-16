using ClothStock.Business.Services;
using ClothStock.Models.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Orders;

public partial class CourierForm : Form
{
    private readonly CourierService _service;
    private int _currentId = 0;
    private bool _loading = false;

    public CourierForm()
    {
        InitializeComponent();
        _service = Program.Services.GetRequiredService<CourierService>();
    }

    private async void CourierForm_Load(object? sender, EventArgs e)
    {
        await LoadGridAsync();
        ClearForm();
    }

    private async Task LoadGridAsync()
    {
        try
        {
            _loading = true;
            grid.DataSource = (await _service.GetAllAsync(activeOnly: false)).ToList();
            if (grid.Columns.Contains("CourierId")) grid.Columns["CourierId"]!.Visible = false;
            if (grid.Columns.Contains("CourierName")) grid.Columns["CourierName"]!.HeaderText = "Courier";
            if (grid.Columns.Contains("IsActive")) grid.Columns["IsActive"]!.HeaderText = "Active";
            grid.ClearSelection();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { _loading = false; }
    }

    private void ClearForm()
    {
        _currentId = 0;
        txtName.Clear();
        chkActive.Checked = true;
        grid.ClearSelection();
        txtName.Focus();
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            btnSave.Enabled = false;
            await _service.SaveAsync(new Courier
            {
                CourierId = _currentId,
                CourierName = txtName.Text,
                IsActive = chkActive.Checked
            });
            MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadGridAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
                ? "Ye courier pehle se maujood hai." : ex.Message;
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { btnSave.Enabled = true; }
    }

    private void btnNew_Click(object? sender, EventArgs e) => ClearForm();

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_currentId == 0)
        {
            MessageBox.Show("Pehle list se courier select karein.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (MessageBox.Show($"Courier '{txtName.Text}' delete karna hai?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try
        {
            await _service.DeleteAsync(_currentId);
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
        if (grid.CurrentRow?.DataBoundItem is Courier c)
        {
            _currentId = c.CourierId;
            txtName.Text = c.CourierName;
            chkActive.Checked = c.IsActive;
        }
    }
}