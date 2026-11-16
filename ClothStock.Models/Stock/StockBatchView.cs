namespace ClothStock.Models.Stock;

public class StockBatchView
{
    public int ArticleId { get; set; }
    public string ArticleNumber { get; set; } = "";
    public string ArticleName { get; set; } = "";
    public int ColorId { get; set; }
    public string ColorName { get; set; } = "";
    public int BatchId { get; set; }
    public string BatchNo { get; set; } = "";
    public DateTime ReceivedDate { get; set; }
    public string? Supplier { get; set; }
    public decimal MetersPerSuit { get; set; }
    public decimal CostPricePerSuit { get; set; }
    public decimal TotalMetersReceived { get; set; }
    public decimal MetersRemaining { get; set; }
    public int SuitsRemaining { get; set; }
    public decimal LooseMeters { get; set; }
    public int AgeMonths { get; set; }
    public decimal StockValue { get; set; }
    public bool IsOldStock { get; set; }

    public int SuitsPerBunch { get; set; }
}