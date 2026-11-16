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
        if (r.ColorId <= 0) throw new InvalidOperationException("Please select a color.");
        if (r.BunchesReceived <= 0) throw new InvalidOperationException("Bunches must be greater than 0.");
        if (r.SuitsPerBunch <= 0) throw new InvalidOperationException("Suits per bunch must be greater than 0.");
        if (r.MetersPerSuit <= 0) throw new InvalidOperationException("Meters per suit must be greater than 0.");
        if (r.CostPricePerSuit < 0) throw new InvalidOperationException("Cost price cannot be negative.");
        if (r.ReceivedDate.Date > DateTime.Today)
            throw new InvalidOperationException("Received date cannot be in the future.");

        return await _stockRepo.StockInAsync(r);
    }

    public Task AdjustAsync(int batchId, decimal metersChange, string movementType, string? note)
    {
        if (batchId <= 0) throw new InvalidOperationException("Please select a batch.");
        if (metersChange == 0) throw new InvalidOperationException("Meters cannot be zero.");
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
        if (batchId <= 0) throw new InvalidOperationException("Please select a batch from the list first.");
        return _stockRepo.DeleteBatchAsync(batchId);
    }

    public Task<StockBatch?> GetBatchByIdAsync(int batchId) => _stockRepo.GetBatchByIdAsync(batchId);

    public Task UpdateBatchAsync(StockUpdateRequest r)
    {
        if (r.BatchId <= 0) throw new InvalidOperationException("Please select a batch.");
        if (r.BunchesReceived <= 0) throw new InvalidOperationException("Bunches must be greater than 0.");
        if (r.SuitsPerBunch <= 0) throw new InvalidOperationException("Suits per bunch must be greater than 0.");
        if (r.MetersPerSuit <= 0) throw new InvalidOperationException("Meters per suit must be greater than 0.");
        if (r.CostPricePerSuit < 0) throw new InvalidOperationException("Cost price cannot be negative.");
        if (r.ReceivedDate.Date > DateTime.Today)
            throw new InvalidOperationException("Received date cannot be in the future.");
        return _stockRepo.UpdateBatchAsync(r);
    }

    // Edit/Delete ke liye: is color ke sab batches (khali bhi)
    public async Task<IEnumerable<StockBatchView>> GetAllBatchesForColorAsync(int colorId)
        => await _stockRepo.GetBatchesAsync(colorId, onlyAvailable: false);
}