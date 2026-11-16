using ClothStock.Business.Services;
using ClothStock.Models.Stock;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Stock;

public partial class StockViewForm : Form
{
    private readonly StockService _stock;
    private readonly ArticleService _articles;
    private readonly SettingsService _settings;
    private bool _loading = false;
    private bool _busy = false;

    private const string ColorWise = "Color wise (total)";
    private const string BatchWise = "Batch wise (tafseel)";

    public StockViewForm()
    {
        InitializeComponent();
        _stock = Program.Services.GetRequiredService<StockService>();
        _articles = Program.Services.GetRequiredService<ArticleService>();
        _settings = Program.Services.GetRequiredService<SettingsService>();
    }

    private int SelectedArticleId => cmbArticle.SelectedValue is int id ? id : 0;

    private async void StockViewForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _loading = true;

            var list = (await _articles.GetArticlesAsync(activeOnly: true)).ToList();
            list.Insert(0, new Article { ArticleId = 0, ArticleName = "-- Sab Articles --" });
            cmbArticle.DisplayMember = "ArticleName";
            cmbArticle.ValueMember = "ArticleId";
            cmbArticle.DataSource = list;

            cmbView.Items.Add(ColorWise);
            cmbView.Items.Add(BatchWise);
            cmbView.SelectedIndex = 0;

            _loading = false;

            await LoadGridAsync();

            var seconds = await _settings.GetIntAsync("AutoRefreshSeconds", 30);
            if (seconds > 0)
            {
                tmrRefresh.Interval = seconds * 1000;
                tmrRefresh.Start();
            }
        }
        catch (Exception ex)
        {
            _loading = false;
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void Filter_Changed(object? sender, EventArgs e)
    {
        if (_loading) return;
        await LoadGridAsync();
    }

    private async void btnRefresh_Click(object? sender, EventArgs e) => await LoadGridAsync();

    private async void tmrRefresh_Tick(object? sender, EventArgs e) => await LoadGridAsync();

    private async Task LoadGridAsync()
    {
        if (_busy) return;
        _busy = true;
        try
        {
            int? articleId = SelectedArticleId == 0 ? null : SelectedArticleId;
            bool alertsOnly = chkAlertsOnly.Checked;

            if (cmbView.SelectedItem as string == BatchWise)
                await ShowBatchWiseAsync(articleId, alertsOnly);
            else
                await ShowColorWiseAsync(articleId, alertsOnly);
        }
        catch (Exception ex)
        {
            tmrRefresh.Stop();
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _busy = false;
        }
    }

    private async Task ShowColorWiseAsync(int? articleId, bool alertsOnly)
    {
        var list = (await _stock.GetColorStockAsync(articleId)).ToList();
        if (alertsOnly) list = list.Where(x => x.IsLowStock).ToList();

        grid.DataSource = list;

        Hide("ArticleId", "ArticleNumber", "ColorId", "IsLowStock");
        Header("ArticleName", "Article");
        Header("ColorName", "Color");
        Header("TotalBatches", "Batches");
        Header("MetersRemaining", "Meter Baqi");
        Header("SuitsRemaining", "Suits Baqi");
        Header("StockValue", "Stock Value");
        Format("MetersRemaining", "0.###");
        Format("SuitsRemaining", "N0");
        Format("StockValue", "N0");

        lblSummary.Text =
            $"Colors: {list.Count:N0}   |   Total Suits: {list.Sum(x => x.SuitsRemaining):N0}   |   " +
            $"Total Meter: {list.Sum(x => x.MetersRemaining):0.###}" +
            (SessionManager.CanSeeCost ? $"   |   Stock Value: {list.Sum(x => x.StockValue):N0}" : "");

        // Seller ko cost / value nahi dikhni chahiye
        if (!SessionManager.CanSeeCost) Hide("StockValue");
    }

    private async Task ShowBatchWiseAsync(int? articleId, bool alertsOnly)
    {
        var list = (await _stock.GetAllBatchesAsync()).ToList();
        if (articleId.HasValue) list = list.Where(x => x.ArticleId == articleId.Value).ToList();
        if (alertsOnly) list = list.Where(x => x.IsOldStock).ToList();

        grid.DataSource = list;

        Hide("ArticleId", "ArticleNumber", "ColorId", "BatchId", "Supplier",
             "TotalMetersReceived", "IsOldStock", "SuitsPerBunch");
        Header("ArticleName", "Article");
        Header("ColorName", "Color");
        Header("BatchNo", "Batch No");
        Header("ReceivedDate", "Received");
        Header("MetersPerSuit", "Meter/Suit");
        Header("CostPricePerSuit", "Cost/Suit");
        Header("MetersRemaining", "Meter Baqi");
        Header("SuitsRemaining", "Suits Baqi");
        Header("LooseMeters", "Bacha Hua Tukra (m)");
        Header("AgeMonths", "Purana (mahine)");
        Header("StockValue", "Stock Value");
        Format("ReceivedDate", "dd-MMM-yyyy");
        Format("CostPricePerSuit", "N0");
        Format("MetersRemaining", "0.###");
        Format("LooseMeters", "0.###");
        Format("SuitsRemaining", "N0");
        Format("StockValue", "N0");

        lblSummary.Text =
            $"Batches: {list.Count:N0}   |   Total Suits: {list.Sum(x => x.SuitsRemaining):N0}   |   " +
            $"Total Meter: {list.Sum(x => x.MetersRemaining):0.###}" +
            (SessionManager.CanSeeCost ? $"   |   Stock Value: {list.Sum(x => x.StockValue):N0}" : "");

        if (!SessionManager.CanSeeCost) Hide("StockValue", "CostPricePerSuit");
    }

    private void grid_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow row in grid.Rows)
        {
            if (row.DataBoundItem is StockColorView c && c.IsLowStock)
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 220, 220);
            else if (row.DataBoundItem is StockBatchView b && b.IsOldStock)
                row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 235, 205);
        }
        grid.ClearSelection();
    }

    private void Hide(params string[] columns)
    {
        foreach (var c in columns)
            if (grid.Columns.Contains(c)) grid.Columns[c]!.Visible = false;
    }

    private void Header(string column, string text)
    {
        if (grid.Columns.Contains(column)) grid.Columns[column]!.HeaderText = text;
    }

    private void Format(string column, string format)
    {
        if (!grid.Columns.Contains(column)) return;
        var col = grid.Columns[column]!;
        col.DefaultCellStyle.Format = format;
        if (format != "dd-MMM-yyyy")
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
    }
}