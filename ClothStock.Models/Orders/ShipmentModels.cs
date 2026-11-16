namespace ClothStock.Models.Orders;

public class Shipment
{
    public int ShipmentId { get; set; }
    public int CourierId { get; set; }
    public string TrackingId { get; set; } = "";
    public decimal WeightKg { get; set; }
    public decimal CourierCharge { get; set; }
    public decimal ReturnCharge { get; set; }
    public string Status { get; set; } = "";
    public string? Notes { get; set; }
}

public class ShipmentSummary
{
    public int ShipmentId { get; set; }
    public string TrackingId { get; set; } = "";
    public string CourierName { get; set; } = "";
    public string Status { get; set; } = "";
    public decimal WeightKg { get; set; }
    public decimal CourierCharge { get; set; }
    public decimal ReturnCharge { get; set; }
    public DateTime? DispatchDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public string? Notes { get; set; }
    public int OrdersCount { get; set; }
    public int Suits { get; set; }
    public string? CustomerName { get; set; }
    public string? City { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ShipmentSaveRequest
{
    public int ShipmentId { get; set; }               // 0 = naya
    public int CourierId { get; set; }
    public string TrackingId { get; set; } = "";
    public decimal WeightKg { get; set; }
    public decimal CourierCharge { get; set; }
    public decimal ReturnCharge { get; set; }
    public string? Notes { get; set; }
    public string? CreatedBy { get; set; }
    public List<int> OrderIds { get; set; } = new();  // sirf naye shipment ke waqt
}