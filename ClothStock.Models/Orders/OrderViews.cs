namespace ClothStock.Models.Orders;

public class OrderSummary
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public string Channel { get; set; } = "";
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public string Status { get; set; } = "";
    public int? ShipmentId { get; set; }
    public string? TrackingId { get; set; }
    public string? CourierName { get; set; }
    public string? ShipmentStatus { get; set; }
    public int Suits { get; set; }
    public int ReturnedSuits { get; set; }
    public decimal GrossSale { get; set; }
    public decimal NetSale { get; set; }
    public decimal NetCost { get; set; }
    public decimal TaxPercent { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal PackagingCharge { get; set; }
    public decimal CourierShare { get; set; }
    public decimal ReturnShare { get; set; }
    public decimal Profit { get; set; }
    public bool IsExpected { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Address { get; set; }
    public int? CourierId { get; set; }
    public decimal CourierCharge { get; set; }
    public decimal ReturnCharge { get; set; }
    public DateTime? ReturnedDate { get; set; }

    public decimal? AmountPaid { get; set; }
    public int? CustomerId { get; set; }
    public DateTime? DueDate { get; set; }
}

public class OrderLine
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = "";
    public string OrderStatus { get; set; } = "";
    public string ArticleNumber { get; set; } = "";
    public string ArticleName { get; set; } = "";
    public string ColorName { get; set; } = "";
    public string BatchNo { get; set; } = "";
    public DateTime ReceivedDate { get; set; }
    public int Quantity { get; set; }
    public int ReturnedQty { get; set; }
    public int KeptQty { get; set; }
    public decimal MetersPerSuit { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal CostPrice { get; set; }
    public decimal LineSale { get; set; }
    public int ColorId { get; set; }
    public int BatchId { get; set; }
}

public class SalesLine
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Channel { get; set; } = "";
    public string Status { get; set; } = "";
    public string ArticleName { get; set; } = "";
    public string ColorName { get; set; } = "";
    public int Quantity { get; set; }
    public int ReturnedQty { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal CostPrice { get; set; }
}
