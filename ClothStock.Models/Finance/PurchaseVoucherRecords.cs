namespace ClothStock.Models.Finance;

// Purchase bills ki list (har row ek voucher / bill)
public class PurchaseVoucherRow
{
    public int VoucherId { get; set; }
    public string VoucherNo { get; set; } = "";
    public DateTime VoucherDate { get; set; }
    public string InvoiceNo { get; set; } = "";
    public string SupplierName { get; set; } = "";
    public DateTime? DueDate { get; set; }
    public int Suits { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string? PaymentText { get; set; }
    public string? Notes { get; set; }
    public string? CreatedBy { get; set; }
    public decimal Balance => TotalAmount - PaidAmount;
}

// Bill ke andar ki lines (article / color / suits / cost)
public class PurchaseVoucherItemRow
{
    public int VoucherItemId { get; set; }
    public string ArticleName { get; set; } = "";
    public string ColorName { get; set; } = "";
    public int Bunches { get; set; }
    public int SuitsPerBunch { get; set; }
    public decimal MetersPerSuit { get; set; }
    public decimal CostPerSuit { get; set; }
    public int Suits => Bunches * SuitsPerBunch;
    public decimal Meters => Suits * MetersPerSuit;
    public decimal LineTotal => Suits * CostPerSuit;
}