using ClothStock.Models.Stock;

namespace ClothStock.Data.Interfaces;

public interface IStockRepository
{
    Task<int> StockInAsync(StockInRequest request);
    Task AdjustAsync(int batchId, decimal metersChange, string movementType, string? note);
    Task<IEnumerable<StockBatchView>> GetBatchesAsync(int colorId, bool onlyAvailable = true);
    Task<IEnumerable<StockBatchView>> GetAllBatchesAsync();
    Task<IEnumerable<StockColorView>> GetColorStockAsync(int? articleId = null);

    Task DeleteBatchAsync(int batchId);
}