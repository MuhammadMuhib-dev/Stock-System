using ClothStock.Business.Services;
using ClothStock.Models.Stock;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem.Forms.Stock;

public partial class ArticleForm : Form
{
    private readonly ArticleService _service;
    private int _currentId = 0;
    private bool _loading = false;

    public ArticleForm()
    {
        InitializeComponent();
        _service = Program.Services.GetRequiredService<ArticleService>();
    }

    private async void ArticleForm_Load(object? sender, EventArgs e)
    {
        await LoadGridAsync();
        ClearForm();
    }

    private async Task LoadGridAsync()
    {
        try
        {
            _loading = true;
            var list = (await _service.GetArticlesAsync(activeOnly: false)).ToList();
            grid.DataSource = list;

            if (grid.Columns.Contains("ArticleId"))
                grid.Columns["ArticleId"]!.Visible = false;

            SetHeader("ArticleNumber", "Article No");
            SetHeader("ArticleName", "Article Name");
            SetHeader("SellingPrice", "Selling Price");
            SetHeader("IsActive", "Active");

            if (grid.Columns.Contains("SellingPrice"))
            {
                grid.Columns["SellingPrice"]!.DefaultCellStyle.Format = "N0";
                grid.Columns["SellingPrice"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Load ke baad koi row khud select na rahe
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

    private void SetHeader(string column, string text)
    {
        if (grid.Columns.Contains(column))
            grid.Columns[column]!.HeaderText = text;
    }

    private void ClearForm()
    {
        _currentId = 0;
        txtNumber.Clear();
        txtName.Clear();
        txtPrice.Clear();
        chkActive.Checked = true;
        grid.ClearSelection();
        txtNumber.Focus();
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!decimal.TryParse(txtPrice.Text.Trim(), out var price))
            {
                MessageBox.Show("Please enter selling price as a valid number.", "Check",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            var article = new Article
            {
                ArticleId = _currentId,
                ArticleNumber = txtNumber.Text,
                ArticleName = txtName.Text,
                SellingPrice = price,
                IsActive = chkActive.Checked
            };

            btnSave.Enabled = false;
            await _service.SaveArticleAsync(article);

            MessageBox.Show("Save ho gaya.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadGridAsync();
            ClearForm();
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
                ? "This Article Number already exists. Please enter a different number."
                : ex.Message;
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnSave.Enabled = true;
        }
    }

    private void btnNew_Click(object? sender, EventArgs e) => ClearForm();

    private void grid_SelectionChanged(object? sender, EventArgs e)
    {
        if (_loading) return;

        if (grid.CurrentRow?.DataBoundItem is Article a)
        {
            _currentId = a.ArticleId;
            txtNumber.Text = a.ArticleNumber;
            txtName.Text = a.ArticleName;
            txtPrice.Text = a.SellingPrice.ToString("0.##");
            chkActive.Checked = a.IsActive;
        }
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (_currentId == 0)
        {
            MessageBox.Show("Please select an article from the list first.", "Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"Do you want to delete article '{txtNumber.Text} - {txtName.Text}'?",
            "Delete confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm != DialogResult.Yes) return;

        try
        {
            btnDelete.Enabled = false;
            await _service.DeleteArticleAsync(_currentId);

            MessageBox.Show("Article delete ho gaya.", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            await LoadGridAsync();
            ClearForm();
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
}