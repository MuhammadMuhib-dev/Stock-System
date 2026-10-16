using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Stock;
using Dapper;

namespace ClothStock.Data.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly DbConnectionFactory _factory;

    public ArticleRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Article>> GetAllAsync(bool activeOnly = true)
    {
        using var db = _factory.Create();
        var sql = @"SELECT ArticleId, ArticleNumber, ArticleName, SellingPrice, IsActive
                    FROM dbo.Articles
                    WHERE (@ActiveOnly = 0 OR IsActive = 1)
                    ORDER BY ArticleName";
        return await db.QueryAsync<Article>(sql, new { ActiveOnly = activeOnly ? 1 : 0 });
    }

    public async Task<Article?> GetByIdAsync(int articleId)
    {
        using var db = _factory.Create();
        var sql = @"SELECT ArticleId, ArticleNumber, ArticleName, SellingPrice, IsActive
                    FROM dbo.Articles WHERE ArticleId = @ArticleId";
        return await db.QueryFirstOrDefaultAsync<Article>(sql, new { ArticleId = articleId });
    }

    public async Task<int> AddAsync(Article a)
    {
        using var db = _factory.Create();
        var sql = @"INSERT INTO dbo.Articles (ArticleNumber, ArticleName, SellingPrice, IsActive)
                    VALUES (@ArticleNumber, @ArticleName, @SellingPrice, @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
        return await db.ExecuteScalarAsync<int>(sql, a);
    }

    public async Task UpdateAsync(Article a)
    {
        using var db = _factory.Create();
        var sql = @"UPDATE dbo.Articles
                    SET ArticleNumber = @ArticleNumber, ArticleName = @ArticleName,
                        SellingPrice = @SellingPrice, IsActive = @IsActive
                    WHERE ArticleId = @ArticleId";
        await db.ExecuteAsync(sql, a);
    }

    public async Task<bool> HasColorsAsync(int articleId)
    {
        using var db = _factory.Create();
        var count = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.Colors WHERE ArticleId = @ArticleId",
            new { ArticleId = articleId });
        return count > 0;
    }

    public async Task DeleteAsync(int articleId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            "DELETE FROM dbo.Articles WHERE ArticleId = @ArticleId",
            new { ArticleId = articleId });
    }
}