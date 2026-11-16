using ClothStock.Business.Services;
using ClothStock.Models.Stock;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Stock;

public partial class StockInForm : Form
{
    private readonly StockService _stock;
    private readonly ArticleService _articles;
    private bool _loading = false;
    private int _editBatchId = 0;

    public StockInForm()
    {
        InitializeComponent();
        _stock = Program.Services.GetRequiredService<StockService>();
        _articles = Program.Services.GetRequiredService<ArticleService>();
        dtpReceived.MaxDate = DateTime.Today;
    }

    private int SelectedArticleId => cmbArticle.SelectedValue is int id ? id : 0;
    private int SelectedColorId => cmbColor.SelectedValue is int id ? id : 0;

    private async void StockInForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _loading = true;
            var list = (await _articles.GetArticlesAsync(activeOnly: true)).ToList();
            cmbArticle.DisplayMember = "ArticleName";
            cmbArticle.ValueMember = "ArticleId";
            cmbArticle.DataSource = list;
            _loading = false;

            await LoadColorsAsync();
            UpdatePreview();
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void cmbArticle_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        await LoadColorsAsync();
    }

    private async void cmbColor_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loading) return;
        await LoadBatchesAsync();
    }

    private async Task LoadColorsAsync()
    {
        try
        {
            _loading = true;
            if (SelectedArticleId == 0)
            {
                cmbColor.DataSource = null;
                grid.DataSource = null;
                return;
            }

            var colors = (await _articles.GetColorsAsync(SelectedArticleId, activeOnly: true)).ToList();
            cmbColor.DisplayMember = "ColorName";
            cmbColor.ValueMember = "ColorId";
            cmbColor.DataSource = colors;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _loading = false;
        }

        await LoadBatchesAsync();
    }

    private async Task LoadBatchesAsync()
    {
        try
        {
            if (SelectedColorId == 0)
            {
                grid.DataSource = null;
                grpBatches.Text = "Current batches for this color (no color selected)";
                return;
            }

            // Sab batches (khali bhi), taake edit/delete ho sake
            var all = (await _stock.GetAllBatchesForColorAsync(SelectedColorId)).ToList();
            grid.DataSource = all;

            grpBatches.Text = $"Current batches for this color ({all.Count} rows)";

            string[] hide =
            {
                "ArticleId", "ArticleNumber", "ArticleName", "ColorId", "ColorName",
                "BatchId", "Supplier", "TotalMetersReceived", "LooseMeters",
                "AgeMonths", "StockValue", "IsOldStock"
            };
            foreach (var h in hide)
                if (grid.Columns.Contains(h)) grid.Columns[h]!.Visible = false;

            SetHeader("BatchNo", "Batch No");
            SetHeader("ReceivedDate", "Received");
            SetHeader("MetersPerSuit", "Meter/Suit");
            SetHeader("CostPricePerSuit", "Cost/Suit");
            SetHeader("MetersRemaining", "Meters Remaining");
            SetHeader("SuitsRemaining", "Suits Remaining");

            if (grid.Columns.Contains("ReceivedDate"))
                grid.Columns["ReceivedDate"]!.DefaultCellStyle.Format = "dd-MMM-yyyy";
            if (grid.Columns.Contains("CostPricePerSuit"))
                grid.Columns["CostPricePerSuit"]!.DefaultCellStyle.Format = "N0";

            // Pehli row khud select ho jaye, taake Edit/Delete foran chale
            if (grid.Rows.Count > 0)
            {
                var firstVisible = grid.Rows[0].Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                if (firstVisible != null) grid.CurrentCell = firstVisible;
                grid.Rows[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetHeader(string column, string text)
    {
        if (grid.Columns.Contains(column))
            grid.Columns[column]!.HeaderText = text;
    }

    private void Preview_Changed(object? sender, EventArgs e) => UpdatePreview();

    private void UpdatePreview()
    {
        var p = _stock.Preview(
            (int)numBunches.Value,
            (int)numSuitsPerBunch.Value,
            numMetersPerSuit.Value);

        lblPerBunchValue.Text = p.MetersPerBunch.ToString("0.###");
        lblTotalMetersValue.Text = p.TotalMeters.ToString("0.###");
        lblTotalSuitsValue.Text = p.TotalSuits.ToString("N0");
    }

    private void ClearInputs()
    {
        _editBatchId = 0;
        SetEditMode(false);
        numBunches.Value = 0;
        numSuitsPerBunch.Value = 0;
        numMetersPerSuit.Value = 0;
        numCost.Value = 0;
        txtSupplier.Clear();
        txtNotes.Clear();
        dtpReceived.Value = DateTime.Today;
        UpdatePreview();
        numBunches.Focus();
    }

    private void btnClear_Click(object? sender, EventArgs e) => ClearInputs();

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_editBatchId > 0)
            {
                var upd = new StockUpdateRequest
                {
                    BatchId = _editBatchId,
                    ColorId = SelectedColorId,
                    ReceivedDate = dtpReceived.Value.Date,
                    BunchesReceived = (int)numBunches.Value,
                    SuitsPerBunch = (int)numSuitsPerBunch.Value,
                    MetersPerSuit = numMetersPerSuit.Value,
                    CostPricePerSuit = numCost.Value,
                    Supplier = string.IsNullOrWhiteSpace(txtSupplier.Text) ? null : txtSupplier.Text.Trim(),
                    Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim()
                };

                btnSave.Enabled = false;
                await _stock.UpdateBatchAsync(upd);
                MessageBox.Show("Batch update ho gaya.", "Done",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                await LoadBatchesAsync();
                return;
            }

            var request = new StockInRequest
            {
                ColorId = SelectedColorId,
                ReceivedDate = dtpReceived.Value.Date,
                BunchesReceived = (int)numBunches.Value,
                SuitsPerBunch = (int)numSuitsPerBunch.Value,
                MetersPerSuit = numMetersPerSuit.Value,
                CostPricePerSuit = numCost.Value,
                Supplier = string.IsNullOrWhiteSpace(txtSupplier.Text) ? null : txtSupplier.Text.Trim(),
                Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim()
            };

            var p = _stock.Preview(request.BunchesReceived, request.SuitsPerBunch, request.MetersPerSuit);

            var confirm = MessageBox.Show(
                $"{cmbArticle.Text} - {cmbColor.Text}\n\n" +
                $"Total meter: {p.TotalMeters:0.###}\n" +
                $"Total suits: {p.TotalSuits:N0}\n" +
                $"Cost per suit: {request.CostPricePerSuit:N2}\n\n" +
                "Stock save karna hai?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            btnSave.Enabled = false;
            await _stock.StockInAsync(request);

            MessageBox.Show("Stock save ho gaya.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearInputs();
            await LoadBatchesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (grid.CurrentRow?.DataBoundItem is not StockBatchView b)
        {
            MessageBox.Show("Please select a batch from the list to delete.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"Delete batch '{b.BatchNo}'?\n\n" +
            $"Received: {b.ReceivedDate:dd-MMM-yyyy}\n" +
            $"Meters: {b.MetersRemaining:0.###}\n" +
            $"Suits: {b.SuitsRemaining:N0}\n\n" +
            "This cannot be undone.",
            "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            btnDelete.Enabled = false;
            await _stock.DeleteBatchAsync(b.BatchId);

            MessageBox.Show("Batch deleted.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadBatchesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Could not delete",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            btnDelete.Enabled = true;
        }
    }

    private async void btnEdit_Click(object? sender, EventArgs e)
    {
        if (grid.CurrentRow?.DataBoundItem is not StockBatchView b)
        {
            MessageBox.Show("Please select a batch from the list to edit.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var batch = await _stock.GetBatchByIdAsync(b.BatchId);
            if (batch == null) return;

            _editBatchId = batch.BatchId;
            dtpReceived.Value = batch.ReceivedDate;
            txtSupplier.Text = batch.Supplier ?? "";
            txtNotes.Text = batch.Notes ?? "";
            numBunches.Value = batch.BunchesReceived;
            numSuitsPerBunch.Value = batch.SuitsPerBunch;
            numMetersPerSuit.Value = batch.MetersPerSuit;
            numCost.Value = batch.CostPricePerSuit;
            UpdatePreview();

            SetEditMode(true, batch.BatchNo);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetEditMode(bool editing, string batchNo = "")
    {
        cmbArticle.Enabled = !editing;
        cmbColor.Enabled = !editing;
        btnSave.Text = editing ? "Update Stock" : "Save Stock";
        grpDetails.Text = editing ? $"Edit Batch: {batchNo}" : "Stock In (New Items)";
    }
}