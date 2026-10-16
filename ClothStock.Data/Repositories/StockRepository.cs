using System.Data;
using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Stock;
using Dapper;

namespace ClothStock.Data.Repositories;

public class StockRepository : IStockRepository
{
    private readonly DbConnectionFactory _factory;

    public StockRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<int> StockInAsync(StockInRequest r)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@ColorId", r.ColorId);
        p.Add("@ReceivedDate", r.ReceivedDate.Date, DbType.Date);
        p.Add("@BunchesReceived", r.BunchesReceived);
        p.Add("@SuitsPerBunch", r.SuitsPerBunch);
        p.Add("@MetersPerSuit", r.MetersPerSuit);
        p.Add("@CostPricePerSuit", r.CostPricePerSuit);
        p.Add("@Supplier", r.Supplier);
        p.Add("@Notes", r.Notes);
        p.Add("@NewBatchId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("dbo.usp_StockIn", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewBatchId");
    }

    public async Task AdjustAsync(int batchId, decimal metersChange, string movementType, string? note)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_StockAdjust",
            new { BatchId = batchId, MetersChange = metersChange, MovementType = movementType, Note = note },
            commandType: CommandType.StoredProcedure);
    }

    // FIFO: sab se purani Received date pehle
    public async Task<IEnumerable<StockBatchView>> GetBatchesAsync(int colorId, bool onlyAvailable = true)
    {
        using var db = _factory.Create();
        var sql = @"SELECT * FROM dbo.vw_StockByBatch
                    WHERE ColorId = @ColorId
                      AND (@OnlyAvailable = 0 OR SuitsRemaining > 0)
                    ORDER BY ReceivedDate, BatchId";
        return await db.QueryAsync<StockBatchView>(sql,
            new { ColorId = colorId, OnlyAvailable = onlyAvailable ? 1 : 0 });
    }

    public async Task<IEnumerable<StockBatchView>> GetAllBatchesAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<StockBatchView>(
            "SELECT * FROM dbo.vw_StockByBatch ORDER BY ArticleName, ColorName, ReceivedDate");
    }

    public async Task<IEnumerable<StockColorView>> GetColorStockAsync(int? articleId = null)
    {
        using var db = _factory.Create();
        var sql = @"SELECT * FROM dbo.vw_StockByColor
                    WHERE (@ArticleId IS NULL OR ArticleId = @ArticleId)
                    ORDER BY ArticleName, ColorName";
        return await db.QueryAsync<StockColorView>(sql, new { ArticleId = articleId });
    }

    public async Task DeleteBatchAsync(int batchId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_StockDeleteBatch",
            new { BatchId = batchId },
            commandType: CommandType.StoredProcedure);
    }
}