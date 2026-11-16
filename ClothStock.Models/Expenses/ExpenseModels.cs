namespace ClothStock.Models.Expenses;

public class Worker
{
    public int WorkerId { get; set; }
    public string WorkerName { get; set; } = "";
    public string? Role { get; set; }
    public string? Phone { get; set; }
    public decimal MonthlySalary { get; set; }
    public bool IsActive { get; set; } = true;
}

public class WorkerBalance
{
    public int WorkerId { get; set; }
    public string WorkerName { get; set; } = "";
    public string? Role { get; set; }
    public string? Phone { get; set; }
    public decimal MonthlySalary { get; set; }
    public bool IsActive { get; set; }
    public decimal AdvanceGiven { get; set; }
    public decimal AdvanceRecovered { get; set; }
    public decimal AdvanceBalance { get; set; }
}

public class ExpenseCategory
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string GroupName { get; set; } = "";
    public string Kind { get; set; } = "Normal";            // Normal / Salary / Bonus / Advance
    public string DefaultChannel { get; set; } = "Shop";    // Shop / Online / Shared
    public bool IsActive { get; set; } = true;
}

public class PaymentMethod
{
    public string MethodName { get; set; } = "";
    public bool IsActive { get; set; } = true;
}

public class RecurringItem
{
    public int RecurringId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string Channel { get; set; } = "Shop";
    public int? WorkerId { get; set; }
    public string? WorkerName { get; set; }
    public decimal? Amount { get; set; }                    // null = worker ki salary
    public string PaymentMethod { get; set; } = "Cash";
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
}

public class ExpenseItem
{
    public int ExpenseId { get; set; }
    public DateTime ExpenseDate { get; set; }
    public DateTime PeriodMonth { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string GroupName { get; set; } = "";
    public string Kind { get; set; } = "";
    public string Channel { get; set; } = "";
    public decimal Amount { get; set; }
    public decimal AdvanceCut { get; set; }
    public int? WorkerId { get; set; }
    public string? WorkerName { get; set; }
    public string PaymentMethod { get; set; } = "";
    public string? Notes { get; set; }
    public string Status { get; set; } = "";
    public int? RecurringId { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal ExpenseAmount { get; set; }
    public decimal CashOut { get; set; }
    public decimal ShopPart { get; set; }
    public decimal OnlinePart { get; set; }
}

public class ExpenseSaveRequest
{
    public int ExpenseId { get; set; }                      // 0 = naya
    public DateTime ExpenseDate { get; set; } = DateTime.Today;
    public DateTime? ForMonth { get; set; }                 // kis mahine ke hisab mein (null = ExpenseDate ka mahina)
    public int CategoryId { get; set; }
    public string Channel { get; set; } = "Shop";
    public decimal Amount { get; set; }
    public int? WorkerId { get; set; }
    public decimal AdvanceCut { get; set; }
    public string PaymentMethod { get; set; } = "Cash";
    public string? Notes { get; set; }
    public string? User { get; set; }
}

public class ExpenseFilter
{
    public DateTime? Month { get; set; }                    // PeriodMonth (null = sab)
    public string? Channel { get; set; }
    public int? CategoryId { get; set; }
    public bool IncludeVoid { get; set; }
    public string? Search { get; set; }
}