using System.Data;
using System.Text.Json;
using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Finance;
using Dapper;

namespace ClothStock.Data.Repositories;

public class FinanceRepository : IFinanceRepository
{
    private readonly DbConnectionFactory _factory;
    public FinanceRepository(DbConnectionFactory factory) { _factory = factory; }

    private static string? Like(string? s) =>
        string.IsNullOrWhiteSpace(s) ? null
        : "%" + s.Trim().Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]") + "%";

    // ---------------------------------------------------------------- accounts
    public async Task<IEnumerable<FinancialAccount>> GetAccountsAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<FinancialAccount>(
            "SELECT AccountId, AccountName, AccountType, IsActive FROM dbo.FinancialAccounts WHERE IsActive = 1 ORDER BY AccountId");
    }

    public async Task<IEnumerable<AccountBalance>> GetAccountBalancesAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<AccountBalance>("SELECT * FROM dbo.vw_AccountBalance ORDER BY AccountId");
    }

    public async Task<int> AddAccountAsync(string name, string type, decimal opening, bool active)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.FinancialAccounts (AccountName, AccountType, OpeningBalance, IsActive)
              VALUES (@name, @type, @opening, @active); SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new { name, type, opening, active });
    }

    public async Task UpdateAccountAsync(int id, string name, string type, decimal opening, bool active)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            @"UPDATE dbo.FinancialAccounts SET AccountName=@name, AccountType=@type, OpeningBalance=@opening, IsActive=@active
              WHERE AccountId=@id", new { id, name, type, opening, active });
    }

    // ---------------------------------------------------------------- customers
    public async Task<IEnumerable<CustomerBalance>> SearchCustomersAsync(string? search, bool includeInactive)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<CustomerBalance>(
            @"SELECT TOP 500 * FROM dbo.vw_CustomerBalance
              WHERE (@All = 1 OR IsActive = 1) AND (@Like IS NULL OR CustomerName LIKE @Like OR Phone LIKE @Like)
              ORDER BY CustomerName", new { All = includeInactive ? 1 : 0, Like = Like(search) });
    }

    public async Task<CustomerBalance?> GetCustomerAsync(int customerId)
    {
        using var db = _factory.Create();
        return await db.QueryFirstOrDefaultAsync<CustomerBalance>(
            "SELECT * FROM dbo.vw_CustomerBalance WHERE CustomerId = @Id", new { Id = customerId });
    }

    public async Task<int> AddCustomerAsync(Customer c)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.Customers (CustomerName, Phone, Address, CreditLimit, OpeningBalance, IsActive)
              VALUES (@CustomerName, @Phone, @Address, @CreditLimit, @OpeningBalance, @IsActive);
              SELECT CAST(SCOPE_IDENTITY() AS INT);", c);
    }

    public async Task UpdateCustomerAsync(Customer c)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            @"UPDATE dbo.Customers SET CustomerName=@CustomerName, Phone=@Phone, Address=@Address,
                     CreditLimit=@CreditLimit, OpeningBalance=@OpeningBalance, IsActive=@IsActive
              WHERE CustomerId=@CustomerId", c);
    }

    // ---------------------------------------------------------------- suppliers
    public async Task<IEnumerable<SupplierBalance>> GetSuppliersAsync(string? search, bool includeInactive)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<SupplierBalance>(
            @"SELECT TOP 500 * FROM dbo.vw_SupplierBalance
              WHERE (@All = 1 OR IsActive = 1) AND (@Like IS NULL OR SupplierName LIKE @Like OR Phone LIKE @Like)
              ORDER BY SupplierName", new { All = includeInactive ? 1 : 0, Like = Like(search) });
    }

    public async Task<int> AddSupplierAsync(Supplier s)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.Suppliers (SupplierName, Phone, Address, OpeningBalance, IsActive)
              VALUES (@SupplierName, @Phone, @Address, @OpeningBalance, @IsActive);
              SELECT CAST(SCOPE_IDENTITY() AS INT);", s);
    }

    public async Task UpdateSupplierAsync(Supplier s)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            @"UPDATE dbo.Suppliers SET SupplierName=@SupplierName, Phone=@Phone, Address=@Address,
                     OpeningBalance=@OpeningBalance, IsActive=@IsActive WHERE SupplierId=@SupplierId", s);
    }

    // ---------------------------------------------------------------- shop checkout
    public async Task<int> CheckoutShopSaleAsync(ShopCheckoutRequest r)
    {
        using var db = _factory.Create();
        var json = JsonSerializer.Serialize(
            r.Items.Select(i => new { i.ColorId, i.Quantity, i.SellingPrice, i.BatchId }));

        var p = new DynamicParameters();
        p.Add("@OrderId", r.OrderId);
        p.Add("@CustomerId", r.CustomerId);
        p.Add("@CustomerName", r.CustomerName);
        p.Add("@Phone", r.Phone);
        p.Add("@Notes", r.Notes);
        p.Add("@ItemsJson", json);
        p.Add("@AmountPaid", r.AmountPaid, DbType.Decimal);
        p.Add("@AccountId", r.AccountId);
        p.Add("@Method", r.Method);
        p.Add("@DueDate", r.DueDate?.Date, DbType.Date);
        p.Add("@Key", r.Key);
        p.Add("@User", r.User);
        p.Add("@ResultOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("dbo.usp_ShopSaleCheckout", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@ResultOrderId");
    }

    // ---------------------------------------------------------------- money
    public async Task<int> CustomerReceiptAsync(int customerId, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@CustomerId", customerId); p.Add("@Amount", amount, DbType.Decimal);
        p.Add("@AccountId", accountId); p.Add("@Method", method);
        p.Add("@TxnDate", date.Date, DbType.Date); p.Add("@Key", key); p.Add("@Notes", notes); p.Add("@User", user);
        p.Add("@NewTxnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await db.ExecuteAsync("dbo.usp_CustomerReceipt", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewTxnId");
    }

    public async Task<int> SupplierPaymentAsync(int supplierId, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@SupplierId", supplierId); p.Add("@Amount", amount, DbType.Decimal);
        p.Add("@AccountId", accountId); p.Add("@Method", method);
        p.Add("@TxnDate", date.Date, DbType.Date); p.Add("@Key", key); p.Add("@Notes", notes); p.Add("@User", user);
        p.Add("@NewTxnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await db.ExecuteAsync("dbo.usp_SupplierPayment", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewTxnId");
    }

    public async Task<int> TransferAsync(int fromId, int toId, decimal amount, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@FromAccountId", fromId); p.Add("@ToAccountId", toId); p.Add("@Amount", amount, DbType.Decimal);
        p.Add("@TxnDate", date.Date, DbType.Date); p.Add("@Method", method);
        p.Add("@Notes", notes); p.Add("@Key", key); p.Add("@User", user);
        p.Add("@NewOutId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await db.ExecuteAsync("dbo.usp_Transfer", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewOutId");
    }

    public async Task VoidTxnAsync(int transactionId, string reason, string? user)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_FinTxnVoid",
            new { TransactionId = transactionId, Reason = reason, User = user },
            commandType: CommandType.StoredProcedure);
    }

    // ---------------------------------------------------------------- reports
    public async Task<IEnumerable<StatementRow>> GetCustomerStatementAsync(int customerId)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<StatementRow>(
            "SELECT Dt, Ord, RefId, Ref, Kind, Increase, Decrease, Note FROM dbo.vw_CustomerStatement WHERE PartyId = @Id",
            new { Id = customerId });
    }

    public async Task<IEnumerable<StatementRow>> GetSupplierStatementAsync(int supplierId)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<StatementRow>(
            "SELECT Dt, Ord, RefId, Ref, Kind, Increase, Decrease, Note FROM dbo.vw_SupplierStatement WHERE PartyId = @Id",
            new { Id = supplierId });
    }

    public async Task<IEnumerable<DueItem>> GetDueAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<DueItem>("SELECT * FROM dbo.vw_DueList ORDER BY DueDate, Ref");
    }

    public async Task<IEnumerable<CashBookRow>> GetCashBookAsync(int? accountId, DateTime? from, DateTime? to, bool includeVoid)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<CashBookRow>(
            @"SELECT * FROM dbo.vw_CashBook
              WHERE (@Acc IS NULL OR AccountId = @Acc)
                AND (@From IS NULL OR TxnDate >= @From) AND (@To IS NULL OR TxnDate <= @To)
                AND (@Inc = 1 OR Status = 'Active')
              ORDER BY TxnDate, TransactionId",
            new { Acc = accountId, From = from?.Date, To = to?.Date, Inc = includeVoid ? 1 : 0 });
    }
    public async Task<int> PurchaseStockInAsync(PurchaseBillRequest r)
    {
        using var db = _factory.Create();
        var json = JsonSerializer.Serialize(r.Items.Select(i => new
        {
            i.ColorId,
            i.Bunches,
            i.SuitsPerBunch,
            i.MetersPerSuit,
            i.CostPerSuit
        }));

        var p = new DynamicParameters();
        p.Add("@SupplierId", r.SupplierId);
        p.Add("@InvoiceNo", r.InvoiceNo);
        p.Add("@InvoiceDate", r.InvoiceDate.Date, DbType.Date);
        p.Add("@DueDate", r.DueDate?.Date, DbType.Date);
        p.Add("@Notes", r.Notes);
        p.Add("@User", r.User);
        p.Add("@ItemsJson", json);
        p.Add("@PaidNow", r.PaidNow, DbType.Decimal);
        p.Add("@AccountId", r.AccountId);
        p.Add("@Method", r.Method);
        p.Add("@Key", r.Key);
        p.Add("@NewInvoiceId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("dbo.usp_PurchaseStockIn", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewInvoiceId");
    }

    public async Task<PurchaseVoucherInfo?> GetPurchaseVoucherAsync(int invoiceId)
    {
        using var db = _factory.Create();
        return await db.QueryFirstOrDefaultAsync<PurchaseVoucherInfo>(
            @"SELECT VoucherId, VoucherNo, VoucherDate, TotalAmount, PaidAmount
              FROM dbo.Vouchers
              WHERE VoucherType = 'Purchase' AND PurchaseInvoiceId = @Id",
            new { Id = invoiceId });
    }
    public async Task<IEnumerable<CodPending>> GetCodPendingAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<CodPending>("SELECT * FROM dbo.vw_CodPending ORDER BY OrderId");
    }

    public async Task<int> CodRemitAsync(IEnumerable<int> orderIds, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@OrderIds", string.Join(",", orderIds)); p.Add("@Amount", amount, DbType.Decimal);
        p.Add("@AccountId", accountId); p.Add("@Method", method);
        p.Add("@TxnDate", date.Date, DbType.Date); p.Add("@Notes", notes); p.Add("@Key", key); p.Add("@User", user);
        p.Add("@NewTxnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await db.ExecuteAsync("dbo.usp_CodRemit", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewTxnId");
    }

    public async Task<int> CashManualAsync(string direction, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@Direction", direction); p.Add("@Amount", amount, DbType.Decimal);
        p.Add("@AccountId", accountId); p.Add("@Method", method);
        p.Add("@TxnDate", date.Date, DbType.Date); p.Add("@Notes", notes); p.Add("@Key", key); p.Add("@User", user);
        p.Add("@NewTxnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await db.ExecuteAsync("dbo.usp_CashManual", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewTxnId");
    }
    public async Task<int?> ReturnShopSaleAsync(ShopReturnRequest r)
    {
        using var db = _factory.Create();
        var json = JsonSerializer.Serialize(r.Items.Select(i => new { i.OrderItemId, i.Qty }));
        var p = new DynamicParameters();
        p.Add("@OrderId", r.OrderId); p.Add("@ItemsJson", json);
        p.Add("@RefundAmount", r.RefundAmount, DbType.Decimal);
        p.Add("@AccountId", r.AccountId); p.Add("@Method", r.Method);
        p.Add("@Notes", r.Notes); p.Add("@Key", r.Key); p.Add("@User", r.User);
        p.Add("@NewTxnId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await db.ExecuteAsync("dbo.usp_ShopSaleReturn", p, commandType: CommandType.StoredProcedure);
        return p.Get<int?>("@NewTxnId");
    }

    public async Task<decimal> GetOrderPaidAsync(int orderId)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<decimal?>(
            "SELECT Paid FROM dbo.vw_OrderPaid WHERE OrderId = @Id", new { Id = orderId }) ?? 0;
    }
}