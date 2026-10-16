namespace ClothStock.Models.Stock;

public class StockMovement
{
    public int MovementId { get; set; }
    public int BatchId { get; set; }
    public string MovementType { get; set; } = "";
    public decimal Meters { get; set; }
    public string? ReferenceType { get; set; }
    public int? ReferenceId { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}