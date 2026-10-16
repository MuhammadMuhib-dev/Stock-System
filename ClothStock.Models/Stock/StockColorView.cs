namespace ClothStock.Models.Stock;

public class StockColorView
{
    public int ArticleId { get; set; }
    public string ArticleNumber { get; set; } = "";
    public string ArticleName { get; set; } = "";
    public int ColorId { get; set; }
    public string ColorName { get; set; } = "";
    public int TotalBatches { get; set; }
    public decimal MetersRemaining { get; set; }
    public int SuitsRemaining { get; set; }
    public decimal StockValue { get; set; }
    public bool IsLowStock { get; set; }
}