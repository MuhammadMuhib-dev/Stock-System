namespace ClothStock.Models.Finance;

public class PurchaseItemRequest
{
    public int ColorId { get; set; }
    public int Bunches { get; set; }
    public int SuitsPerBunch { get; set; }
    public decimal MetersPerSuit { get; set; }
    public decimal CostPerSuit { get; set; }
    public decimal LineTotal => Bunches * SuitsPerBunch * CostPerSuit;
}

public class PurchaseBillRequest
{
    public int SupplierId { get; set; }
    public string InvoiceNo { get; set; } = "";
    public DateTime InvoiceDate { get; set; } = DateTime.Today;
    public DateTime? DueDate { get; set; }
    public string? Notes { get; set; }
    public string? User { get; set; }
    public List<PurchaseItemRequest> Items { get; set; } = new();
    public decimal PaidNow { get; set; }
    public int? AccountId { get; set; }
    public string? Method { get; set; }
    public string? Key { get; set; }
    public decimal Total => Items.Sum(i => i.LineTotal);
}