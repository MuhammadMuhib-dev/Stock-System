namespace ClothStock.Models.Orders;

public class OrderItemRequest
{
    public int ColorId { get; set; }
    public int Quantity { get; set; }
    public decimal SellingPrice { get; set; }
    public int? BatchId { get; set; }          // null = FIFO
}

public class OrderCreateRequest
{
    public string Channel { get; set; } = "Online";   // Online ya Shop
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public decimal? PackagingCharge { get; set; }     // null = Settings ka default
    public string? Notes { get; set; }
    public string? CreatedBy { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new();

    public int? CourierId { get; set; }
    public string? TrackingId { get; set; }
    public decimal CourierCharge { get; set; }

    public decimal? AmountPaid { get; set; }
}

public class OrderUpdateInfoRequest
{
    public int OrderId { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public decimal PackagingCharge { get; set; }
    public string? Notes { get; set; }
    public string? UpdatedBy { get; set; }
}

public class OrderFilter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Channel { get; set; }
    public string? Status { get; set; }
    public string? Search { get; set; }
}

public class OrderReplaceRequest : OrderCreateRequest
{
    public int OrderId { get; set; }
}
