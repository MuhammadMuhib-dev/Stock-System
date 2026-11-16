using ClothStock.Models.Orders;

namespace ClothStock.Models.Finance;

public class Customer
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal OpeningBalance { get; set; }
    public bool IsActive { get; set; } = true;
}

// vw_CustomerBalance ki shakal
public class CustomerBalance
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public string? Phone { get; set; }
    public decimal CreditLimit { get; set; }
    public bool IsActive { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal Sales { get; set; }
    public decimal Received { get; set; }
    public decimal Refunded { get; set; }
    public decimal Balance { get; set; }          // + = customer ne dene hain, - = advance
    public decimal Overdue { get; set; }
    public DateTime? OldestDue { get; set; }
}

public class FinancialAccount
{
    public int AccountId { get; set; }
    public string AccountName { get; set; } = "";
    public string AccountType { get; set; } = "Cash";
    public bool IsActive { get; set; }
}

public class ShopCheckoutRequest
{
    public int OrderId { get; set; }              // 0 = nayi sale, >0 = edit
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Phone { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemRequest> Items { get; set; } = new();
    public decimal AmountPaid { get; set; }
    public int? AccountId { get; set; }
    public string? Method { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Key { get; set; }              // double click / retry se bachao
    public string? User { get; set; }
}