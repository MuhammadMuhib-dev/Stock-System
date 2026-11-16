namespace ClothStock.Models.Stock;

public class StockUpdateRequest : StockInRequest
{
    public int BatchId { get; set; }
}