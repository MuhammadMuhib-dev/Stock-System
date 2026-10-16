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

        if (a.ArticleNumber.Length == 0) throw new InvalidOperationException("Article number likhein.");
        if (a.ArticleName.Length == 0) throw new InvalidOperationException("Article ka naam likhein.");
        if (a.SellingPrice < 0) throw new InvalidOperationException("Selling price minus nahi ho sakti.");

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
        if (c.ArticleId <= 0) throw new InvalidOperationException("Pehle article select karein.");
        if (c.ColorName.Length == 0) throw new InvalidOperationException("Color ka naam likhein.");

        if (c.ColorId == 0)
            return await _colors.AddAsync(c);

        await _colors.UpdateAsync(c);
        return c.ColorId;
    }

    public async Task DeleteArticleAsync(int articleId)
    {
        if (articleId <= 0)
            throw new InvalidOperationException("Pehle list se article select karein.");

        if (await _articles.HasColorsAsync(articleId))
            throw new InvalidOperationException(
                "Is article ke colors ya stock maujood hain, is liye delete nahi ho sakta.\n" +
                "Isay list se hatane ke liye Active ka tick hata kar Save karein.");

        await _articles.DeleteAsync(articleId);
    }

    public async Task DeleteColorAsync(int colorId)
    {
        if (colorId <= 0)
            throw new InvalidOperationException("Pehle list se color select karein.");

        if (await _colors.HasBatchesAsync(colorId))
            throw new InvalidOperationException(
                "Is color ka stock maujood hai, is liye delete nahi ho sakta.\n" +
                "Isay chhupane ke liye Active ka tick hata kar Save karein.");

        await _colors.DeleteAsync(colorId);
    }
}