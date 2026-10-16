using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Stock;
using Dapper;

namespace ClothStock.Data.Repositories;

public class ColorRepository : IColorRepository
{
    private readonly DbConnectionFactory _factory;

    public ColorRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Color>> GetByArticleAsync(int articleId, bool activeOnly = true)
    {
        using var db = _factory.Create();
        var sql = @"SELECT ColorId, ArticleId, ColorName, IsActive
                    FROM dbo.Colors
                    WHERE ArticleId = @ArticleId AND (@ActiveOnly = 0 OR IsActive = 1)
                    ORDER BY ColorName";
        return await db.QueryAsync<Color>(sql, new { ArticleId = articleId, ActiveOnly = activeOnly ? 1 : 0 });
    }

    public async Task<int> AddAsync(Color c)
    {
        using var db = _factory.Create();
        var sql = @"INSERT INTO dbo.Colors (ArticleId, ColorName, IsActive)
                    VALUES (@ArticleId, @ColorName, @IsActive);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
        return await db.ExecuteScalarAsync<int>(sql, c);
    }

    public async Task UpdateAsync(Color c)
    {
        using var db = _factory.Create();
        var sql = @"UPDATE dbo.Colors SET ColorName = @ColorName, IsActive = @IsActive
                    WHERE ColorId = @ColorId";
        await db.ExecuteAsync(sql, c);
    }

    public async Task<bool> HasBatchesAsync(int colorId)
    {
        using var db = _factory.Create();
        var count = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.StockBatches WHERE ColorId = @ColorId",
            new { ColorId = colorId });
        return count > 0;
    }

    public async Task DeleteAsync(int colorId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            "DELETE FROM dbo.Colors WHERE ColorId = @ColorId",
            new { ColorId = colorId });
    }
}