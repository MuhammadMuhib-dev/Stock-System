namespace ClothStock.Models.Stock;

public class StockBatch
{
    public int BatchId { get; set; }
    public string BatchNo { get; set; } = "";
    public int ColorId { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string? Supplier { get; set; }
    public int BunchesReceived { get; set; }
    public int SuitsPerBunch { get; set; }
    public decimal MetersPerSuit { get; set; }
    public decimal CostPricePerSuit { get; set; }
    public decimal MetersRemaining { get; set; }
    public string? Notes { get; set; }
}