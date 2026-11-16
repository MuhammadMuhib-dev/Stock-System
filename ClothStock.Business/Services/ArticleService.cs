
using ClothStock.Data.Interfaces;
using ClothStock.Models.Stock;

namespace ClothStock.Business.Services;

public class ArticleService
{
    private readonly IArticleRepository _articles;
    private readonly IColorRepository _colors;

    public ArticleService(IArticleRepository articles, IColorRepository colors)
    {
        _articles = articles;
        _colors = colors;
    }

    public Task<IEnumerable<Article>> GetArticlesAsync(bool activeOnly = true)
        => _articles.GetAllAsync(activeOnly);

    public async Task<int> SaveArticleAsync(Article a)
    {
        a.ArticleNumber = a.ArticleNumber.Trim();
        a.ArticleName = a.ArticleName.Trim();

        if (a.ArticleNumber.Length == 0)
            throw new InvalidOperationException("Please enter an article number.");

        if (a.ArticleName.Length == 0)
            throw new InvalidOperationException("Please enter an article name.");

        if (a.SellingPrice < 0)
            throw new InvalidOperationException("Selling price cannot be negative.");

        if (a.ArticleId == 0)
            return await _articles.AddAsync(a);

        await _articles.UpdateAsync(a);
        return a.ArticleId;
    }

    public Task<IEnumerable<Color>> GetColorsAsync(int articleId, bool activeOnly = true)
        => _colors.GetByArticleAsync(articleId, activeOnly);

    public async Task<int> SaveColorAsync(Color c)
    {
        c.ColorName = c.ColorName.Trim();

        if (c.ArticleId <= 0)
            throw new InvalidOperationException("Please select an article first.");

        if (c.ColorName.Length == 0)
            throw new InvalidOperationException("Please enter a color name.");

        if (c.ColorId == 0)
            return await _colors.AddAsync(c);

        await _colors.UpdateAsync(c);
        return c.ColorId;
    }

    public async Task DeleteArticleAsync(int articleId)
    {
        if (articleId <= 0)
            throw new InvalidOperationException("Please select an article from the list first.");

        if (await _articles.HasColorsAsync(articleId))
            throw new InvalidOperationException(
                "This article has associated colors or stock and cannot be deleted.\n" +
                "To hide it from the list, uncheck Active and click Save.");

        await _articles.DeleteAsync(articleId);
    }

    public async Task DeleteColorAsync(int colorId)
    {
        if (colorId <= 0)
            throw new InvalidOperationException("Please select a color from the list first.");

        if (await _colors.HasBatchesAsync(colorId))
            throw new InvalidOperationException(
                "This color has associated stock and cannot be deleted.\n" +
                "To hide it from the list, uncheck Active and click Save.");

        await _colors.DeleteAsync(colorId);
    }
}
