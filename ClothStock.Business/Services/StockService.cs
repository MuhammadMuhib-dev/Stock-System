using ClothStock.Business.Calculators;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Stock;

namespace ClothStock.Business.Services;

public class StockService
{
    private readonly IStockRepository _stockRepo;

    public StockService(IStockRepository stockRepo)
    {
        _stockRepo = stockRepo;
    }

    // Stock In form ke "preview" ke liye (save se pehle dikhane ko)
    public (decimal MetersPerBunch, decimal TotalMeters, int TotalSuits) Preview(
        int bunches, int suitsPerBunch, decimal metersPerSuit)
    {
        var perBunch = StockCalculator.MetersPerBunch(suitsPerBunch, metersPerSuit);
        var total = StockCalculator.TotalMeters(bunches, suitsPerBunch, metersPerSuit);
        var suits = StockCalculator.TotalSuits(total, metersPerSuit);
        return (perBunch, total, suits);
    }

    public async Task<int> StockInAsync(StockInRequest r)
    {
        if (r.ColorId <= 0) throw new InvalidOperationException("Color select karein.");
        if (r.BunchesReceived <= 0) throw new InvalidOperationException("Bunches 0 se zyada hon.");
        if (r.SuitsPerBunch <= 0) throw new InvalidOperationException("Suits per bunch 0 se zyada hon.");
        if (r.MetersPerSuit <= 0) throw new InvalidOperationException("Meter per suit 0 se zyada hon.");
        if (r.CostPricePerSuit < 0) throw new InvalidOperationException("Cost price minus nahi ho sakti.");
        if (r.ReceivedDate.Date > DateTime.Today)
            throw new InvalidOperationException("Received date aane wali tareekh nahi ho sakti.");

        return await _stockRepo.StockInAsync(r);
    }

    public Task AdjustAsync(int batchId, decimal metersChange, string movementType, string? note)
    {
        if (batchId <= 0) throw new InvalidOperationException("Batch select karein.");
        if (metersChange == 0) throw new InvalidOperationException("Meter zero nahi ho sakte.");
        return _stockRepo.AdjustAsync(batchId, metersChange, movementType, note);
    }

    // FIFO: pehli batch sab se purani hoti hai, default wohi
    public async Task<(IEnumerable<StockBatchView> Batches, StockBatchView? Default)> GetBatchesAsync(int colorId)
    {
        var list = (await _stockRepo.GetBatchesAsync(colorId, onlyAvailable: true)).ToList();
        return (list, list.FirstOrDefault());
    }

    public Task<IEnumerable<StockBatchView>> GetAllBatchesAsync() => _stockRepo.GetAllBatchesAsync();

    public Task<IEnumerable<StockColorView>> GetColorStockAsync(int? articleId = null)
        => _stockRepo.GetColorStockAsync(articleId);

    public Task DeleteBatchAsync(int batchId)
    {
        if (batchId <= 0) throw new InvalidOperationException("Pehle list se batch select karein.");
        return _stockRepo.DeleteBatchAsync(batchId);
    }
}