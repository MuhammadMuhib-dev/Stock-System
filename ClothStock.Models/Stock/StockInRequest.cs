namespace ClothStock.Models.Stock;

public class StockInRequest
{
    public int ColorId { get; set; }
    public DateTime ReceivedDate { get; set; } = DateTime.Today;
    public int BunchesReceived { get; set; }
    public int SuitsPerBunch { get; set; }
    public decimal MetersPerSuit { get; set; }
    public decimal CostPricePerSuit { get; set; }
    public string? Supplier { get; set; }
    public string? Notes { get; set; }
}