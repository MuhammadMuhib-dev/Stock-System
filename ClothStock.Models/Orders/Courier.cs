namespace ClothStock.Models.Orders;

public class Courier
{
    public int CourierId { get; set; }
    public string CourierName { get; set; } = "";
    public bool IsActive { get; set; } = true;
}