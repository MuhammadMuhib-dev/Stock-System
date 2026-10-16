using ClothStock.Business.Services;
using ClothStock.Models.Stock;
using Microsoft.Extensions.DependencyInjection;
using StockColor = ClothStock.Models.Stock.Color;

namespace ClothStockSystem.Forms.Stock;

public partial class ColorForm : Form
{
    private readonly ArticleService _service;
    private int _currentColorId = 0;
    private bool _loading = false;

    public ColorForm()
    {
        InitializeComponent();
        _service = Program.Services.GetRequiredService<ArticleService>();
    }

    private async void ColorForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _loading = true;
            var articles = (await _service.GetArticlesAsync(activeOnly: true)).ToList();
            cmbArticle.DisplayMember = "ArticleName";
            cmbArticle.ValueMember = "ArticleId";
            cmbArticle.DataSource = articles;
            _loading = false;

            await LoadGridAsync();
            ClearForm();
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
        await LoadGridAsync();
        ClearForm();
    }

    private int SelectedArticleId =>
        cmbArticle.SelectedValue is int id ? id : 0;

    private async Task LoadGridAsync()
    {
        try
        {
            _loading = true;
            if (SelectedArticleId == 0)
            {
                grid.DataSource = null;
                return;
            }

            var list = (await _service.GetColorsAsync(SelectedArticleId, activeOnly: false)).ToList();
            grid.DataSource = list;

            if (grid.Columns.Contains("ColorId")) grid.Columns["ColorId"]!.Visible = false;
            if (grid.Columns.Contains("ArticleId")) grid.Columns["ArticleId"]!.Visible = false;
            if (grid.Columns.Contains("ColorName")) grid.Columns["ColorName"]!.HeaderText = "Color";
            if (grid.Columns.Contains("IsActive")) grid.Columns["IsActive"]!.HeaderText = "Active";

            grid.CurrentCell = null;
            grid.ClearSelection();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private void ClearForm()
    {
        _currentColorId = 0;
        txtColor.Clear();
        chkActive.Checked = true;
        grid.ClearSelection();
        txtColor.Focus();
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            if (SelectedArticleId == 0)
            {
                MessageBox.Show("Pehle article select karein.", "Check",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var c = new StockColor
            {
                ColorId = _currentColorId,
                ArticleId = SelectedArticleId,
                ColorName = txtColor.Text,
                IsActive = chkActive.Checked
            };

            btnSave.Enabled = false;
            await _service.SaveColorAsync(c);

            MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadGridAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
                ? "Ye color is article mein pehle se maujood hai."
                : ex.Message;
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnNew_Click(object? sender, EventArgs e) => ClearForm();

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_currentColorId == 0)
        {
            MessageBox.Show("Pehle list se color select karein.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show($"Color '{txtColor.Text}' delete karna hai?",
            "Delete confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes) return;

        try
        {
            btnDelete.Enabled = false;
            await _service.DeleteColorAsync(_currentColorId);
            MessageBox.Show("Color delete ho gaya.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadGridAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Delete nahi ho saka",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            btnDelete.Enabled = true;
        }
    }

    private void grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (_loading) return;

        if (grid.CurrentRow?.DataBoundItem is StockColor c)
        {
            _currentColorId = c.ColorId;
            txtColor.Text = c.ColorName;
            chkActive.Checked = c.IsActive;
        }
    }
}