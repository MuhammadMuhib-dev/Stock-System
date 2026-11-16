namespace ClothStock.Models.Finance;

public class AccountBalance
{
    public int AccountId { get; set; }
    public string AccountName { get; set; } = "";
    public string AccountType { get; set; } = "Cash";
    public bool IsActive { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal Receipts { get; set; }
    public decimal Payments { get; set; }
    public decimal TransferIn { get; set; }
    public decimal TransferOut { get; set; }
    public decimal Balance { get; set; }
}

public class Supplier
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = "";
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public decimal OpeningBalance { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SupplierBalance
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = "";
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal Billed { get; set; }
    public decimal Paid { get; set; }
    public decimal Balance { get; set; }
    public decimal Overdue { get; set; }
    public DateTime? OldestDue { get; set; }
}

public class StatementRow
{
    public DateTime Dt { get; set; }
    public int Ord { get; set; }
    public int RefId { get; set; }
    public string Ref { get; set; } = "";
    public string Kind { get; set; } = "";
    public decimal Increase { get; set; }
    public decimal Decrease { get; set; }
    public string? Note { get; set; }
}

public class StatementLine
{
    public DateTime? Date { get; set; }
    public string Ref { get; set; } = "";
    public string Kind { get; set; } = "";
    public string? Note { get; set; }
    public decimal Increase { get; set; }
    public decimal Decrease { get; set; }
    public decimal Balance { get; set; }
}

public class StatementResult
{
    public decimal OpeningBefore { get; set; }
    public decimal Closing { get; set; }
    public decimal TotalIncrease { get; set; }
    public decimal TotalDecrease { get; set; }
    public List<StatementLine> Lines { get; set; } = new();
}

public class DueItem
{
    public string Kind { get; set; } = "";          // Lena / Dena
    public int PartyId { get; set; }
    public string PartyName { get; set; } = "";
    public string? Phone { get; set; }
    public string Ref { get; set; } = "";
    public DateTime DocDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Total { get; set; }
    public decimal Paid { get; set; }
    public decimal Due { get; set; }
    public int DaysOverdue { get; set; }
}

public class CashBookRow
{
    public int TransactionId { get; set; }
    public DateTime TxnDate { get; set; }
    public int AccountId { get; set; }
    public string AccountName { get; set; } = "";
    public string Direction { get; set; } = "";
    public decimal Amount { get; set; }
    public string MethodName { get; set; } = "";
    public string? PartyName { get; set; }
    public string? ReferenceType { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = "";
    public string? VoidReason { get; set; }
    public string? CreatedBy { get; set; }
    public decimal MoneyIn { get; set; }
    public decimal MoneyOut { get; set; }
    public decimal RunningBalance { get; set; }
}
public class CodPending
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public string? CustomerName { get; set; }
    public string? City { get; set; }
    public string? CourierName { get; set; }
    public string? TrackingId { get; set; }
    public decimal NetSale { get; set; }
    public decimal CourierCharge { get; set; }
    public decimal Expected { get; set; }
    public int DaysWaiting { get; set; }
}