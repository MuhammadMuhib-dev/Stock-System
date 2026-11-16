namespace ClothStock.Models.Finance;

// Bill save hone ke baad database se wapas aane wala voucher ka khulasa
public class PurchaseVoucherInfo
{
    public int VoucherId { get; set; }
    public string VoucherNo { get; set; } = "";
    public DateTime VoucherDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
}