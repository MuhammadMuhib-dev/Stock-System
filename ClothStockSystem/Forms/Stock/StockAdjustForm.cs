using ClothStock.Business.Calculators;
using ClothStock.Business.Services;
using ClothStock.Models.Stock;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Stock;

public partial class StockAdjustForm : Form
{
    private readonly StockService _stock;
    private readonly ArticleService _articles;
    private bool _loading = false;

    private const string TypeDamaged = "Damaged stock (reduce meters)";
    private const string TypeMinus = "Correction: reduce meters";
    private const string TypePlus = "Correction: increase meters";

    public StockAdjustForm()
    {
        InitializeComponent();
        _stock = Program.Services.GetRequiredService<StockService>();
        _articles = Program.Services.GetRequiredService<ArticleService>();
    }

    private int SelectedArticleId => cmbArticle.SelectedValue is int id ? id : 0;
    private int SelectedColorId => cmbColor.SelectedValue is int id ? id : 0;
    private StockBatchView? SelectedBatch => grid.CurrentRow?.DataBoundItem as StockBatchView;

    private async void StockAdjustForm_Load(object? sender, EventArgs e)
    {
        try
        {
            cmbType.Items.AddRange(new object[] { TypeDamaged, TypeMinus, TypePlus });
            cmbType.SelectedIndex = 0;

            _loading = true;
            var list = (await _articles.GetArticlesAsync(activeOnly: true)).ToList();
            cmbArticle.DisplayMember = "ArticleName";
            cmbArticle.ValueMember = "ArticleId";
            cmbArticle.DataSource = list;
            _loading = false;

            await LoadColorsAsync();
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
                grpBatches.Text = "This article has no colors";
                UpdateInfo();
                return;
            }

            var all = (await _stock.GetAllBatchesForColorAsync(SelectedColorId)).ToList();
            grid.DataSource = all;
            grpBatches.Text = $"Select a batch ({all.Count} batches)";

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

            UpdateInfo();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetHeader(string column, string text)
    {
        if (grid.Columns.Contains(column)) grid.Columns[column]!.HeaderText = text;
    }

    private void grid_SelectionChanged(object? sender, EventArgs e) => UpdateInfo();
    private void Preview_Changed(object? sender, EventArgs e) => UpdateInfo();

    // + ya - meter, jo type se tay hota hai
    private decimal SignedMeters()
    {
        var m = numMeters.Value;
        return cmbType.SelectedItem as string == TypePlus ? m : -m;
    }

    private void UpdateInfo()
    {
        var b = SelectedBatch;
        if (b == null)
        {
            lblInfo.Text = "Select a batch";
            return;
        }

        var after = b.MetersRemaining + SignedMeters();
        if (after < 0)
        {
            lblInfo.Text = $"{b.BatchNo}: not enough stock available (currently {b.MetersRemaining:0.###} meters)";
            return;
        }

        var suitsNow = StockCalculator.TotalSuits(b.MetersRemaining, b.MetersPerSuit);
        var suitsAfter = StockCalculator.TotalSuits(after, b.MetersPerSuit);
        lblInfo.Text =
            $"{b.BatchNo}:  abhi {b.MetersRemaining:0.###} m ({suitsNow:N0} suits)   →   " +
            $"baad {after:0.###} m ({suitsAfter:N0} suits)";
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        var b = SelectedBatch;
        if (b == null)
        {
            MessageBox.Show("Please select a batch from the list first.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (numMeters.Value <= 0)
        {
            MessageBox.Show("Please enter meters greater than 0.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            numMeters.Focus();
            return;
        }

        var change = SignedMeters();
        var type = cmbType.SelectedItem as string == TypeDamaged ? "Damaged" : "Adjustment";
        var note = string.IsNullOrWhiteSpace(txtNote.Text) ? null : txtNote.Text.Trim();

        var confirm = MessageBox.Show(
            $"Batch: {b.BatchNo}\n{cmbType.Text}\nMeter: {change:+0.###;-0.###}\n\nSave?",
            "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        try
        {
            btnSave.Enabled = false;
            await _stock.AdjustAsync(b.BatchId, change, type, note);

            MessageBox.Show("Adjust ho gaya.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            numMeters.Value = 0;
            txtNote.Clear();
            await LoadBatchesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Could not adjust",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }
}