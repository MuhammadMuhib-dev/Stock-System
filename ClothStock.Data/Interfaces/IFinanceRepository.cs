using ClothStock.Models.Finance;

namespace ClothStock.Data.Interfaces;

public interface IFinanceRepository
{
    Task<IEnumerable<FinancialAccount>> GetAccountsAsync();
    Task<IEnumerable<AccountBalance>> GetAccountBalancesAsync();
    Task<int> AddAccountAsync(string name, string type, decimal opening, bool active);
    Task UpdateAccountAsync(int id, string name, string type, decimal opening, bool active);

    Task<IEnumerable<CustomerBalance>> SearchCustomersAsync(string? search, bool includeInactive);
    Task<CustomerBalance?> GetCustomerAsync(int customerId);
    Task<int> AddCustomerAsync(Customer c);
    Task UpdateCustomerAsync(Customer c);

    Task<IEnumerable<SupplierBalance>> GetSuppliersAsync(string? search, bool includeInactive);
    Task<int> AddSupplierAsync(Supplier s);
    Task UpdateSupplierAsync(Supplier s);

    Task<int> CheckoutShopSaleAsync(ShopCheckoutRequest r);

    Task<int> CustomerReceiptAsync(int customerId, decimal amount, int accountId, string method, DateTime date, string? notes, string? key, string? user);
    Task<int> SupplierPaymentAsync(int supplierId, decimal amount, int accountId, string method, DateTime date, string? notes, string? key, string? user);
    Task<int> TransferAsync(int fromId, int toId, decimal amount, string method, DateTime date, string? notes, string? key, string? user);
    Task VoidTxnAsync(int transactionId, string reason, string? user);

    Task<IEnumerable<StatementRow>> GetCustomerStatementAsync(int customerId);
    Task<IEnumerable<StatementRow>> GetSupplierStatementAsync(int supplierId);
    Task<IEnumerable<DueItem>> GetDueAsync();
    Task<IEnumerable<CashBookRow>> GetCashBookAsync(int? accountId, DateTime? from, DateTime? to, bool includeVoid);
    Task<int> PurchaseStockInAsync(PurchaseBillRequest r);
    Task<PurchaseVoucherInfo?> GetPurchaseVoucherAsync(int invoiceId);
    Task<IEnumerable<PurchaseVoucherRow>> GetPurchaseVouchersAsync(DateTime? from, DateTime? to, string? search);
    Task<IEnumerable<PurchaseVoucherItemRow>> GetPurchaseVoucherItemsAsync(int voucherId);
    Task<IEnumerable<CodPending>> GetCodPendingAsync();
    Task<int> CodRemitAsync(IEnumerable<int> orderIds, decimal amount, int accountId, string method, DateTime date, string? notes, string? key, string? user);
    Task<int> CashManualAsync(string direction, decimal amount, int accountId, string method, DateTime date, string? notes, string? key, string? user);

    Task<int?> ReturnShopSaleAsync(ShopReturnRequest r);
    Task<decimal> GetOrderPaidAsync(int orderId);
}