namespace ClothStock.Models.Finance;

public class ShopReturnItem
{
    public int OrderItemId { get; set; }
    public int Qty { get; set; }
}

public class ShopReturnRequest
{
    public int OrderId { get; set; }
    public List<ShopReturnItem> Items { get; set; } = new();
    public decimal RefundAmount { get; set; }
    public int? AccountId { get; set; }
    public string? Method { get; set; }
    public string? Notes { get; set; }
    public string? Key { get; set; }
    public string? User { get; set; }
}