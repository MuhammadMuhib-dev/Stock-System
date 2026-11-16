using ClothStock.Data.Interfaces;
using ClothStock.Models.Finance;

namespace ClothStock.Business.Services;

public class FinanceService
{
    private readonly IFinanceRepository _repo;
    private readonly SettingsService _settings;

    public FinanceService(IFinanceRepository repo, SettingsService settings)
    {
        _repo = repo;
        _settings = settings;
    }

    // ---------------------------------------------------------------- lookups
    public Task<IEnumerable<FinancialAccount>> GetAccountsAsync() => _repo.GetAccountsAsync();
    public Task<IEnumerable<AccountBalance>> GetAccountBalancesAsync() => _repo.GetAccountBalancesAsync();
    public Task<IEnumerable<CustomerBalance>> SearchCustomersAsync(string? s, bool includeInactive = false)
        => _repo.SearchCustomersAsync(s, includeInactive);
    public Task<CustomerBalance?> GetCustomerAsync(int id) => _repo.GetCustomerAsync(id);
    public Task<IEnumerable<SupplierBalance>> GetSuppliersAsync(string? s, bool includeInactive = false)
        => _repo.GetSuppliersAsync(s, includeInactive);
    public Task<int> GetDefaultCreditDaysAsync() => _settings.GetIntAsync("DefaultCreditDays", 30);

    private static bool IsDup(Exception ex, string key) =>
        ex.Message.Contains(key, StringComparison.OrdinalIgnoreCase) || ex.Message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase);

    // ---------------------------------------------------------------- accounts
    public async Task SaveAccountAsync(int id, string name, string type, decimal opening, bool active)
    {
        name = (name ?? "").Trim();
        if (name.Length == 0) throw new InvalidOperationException("Please enter account name.");
        if (type is not ("Cash" or "Bank" or "Wallet")) throw new InvalidOperationException("Account type is incorrect.");
        if (opening < 0) throw new InvalidOperationException("Opening balance cannot be negative.");
        try
        {
            if (id == 0) await _repo.AddAccountAsync(name, type, opening, active);
            else await _repo.UpdateAccountAsync(id, name, type, opening, active);
        }
        catch (Exception ex) when (IsDup(ex, "AccountName"))
        { throw new InvalidOperationException("Is naam ka account pehle se maujood hai."); }
    }

    // ---------------------------------------------------------------- customers
    public Task<int> AddCustomerAsync(string name, string? phone, decimal creditLimit)
        => SaveCustomerAsync(new Customer { CustomerName = name, Phone = phone, CreditLimit = creditLimit });

    public async Task<int> SaveCustomerAsync(Customer c)
    {
        c.CustomerName = (c.CustomerName ?? "").Trim();
        if (c.CustomerName.Length == 0) throw new InvalidOperationException("Customer ka naam likhein.");
        if (c.CreditLimit < 0) throw new InvalidOperationException("Credit limit minus nahi ho sakti.");
        if (c.OpeningBalance < 0) throw new InvalidOperationException("Opening balance minus nahi ho sakta.");
        c.Phone = OrderService.NormalizePhone(c.Phone);
        try
        {
            if (c.CustomerId == 0) return await _repo.AddCustomerAsync(c);
            await _repo.UpdateCustomerAsync(c);
            return c.CustomerId;
        }
        catch (Exception ex) when (IsDup(ex, "UX_Customers_Phone"))
        { throw new InvalidOperationException("Is phone number ka customer pehle se maujood hai."); }
    }

    // ---------------------------------------------------------------- suppliers
    public async Task<int> SaveSupplierAsync(Supplier s)
    {
        s.SupplierName = (s.SupplierName ?? "").Trim();
        if (s.SupplierName.Length == 0) throw new InvalidOperationException("Supplier ka naam likhein.");
        if (s.OpeningBalance < 0) throw new InvalidOperationException("Opening balance minus nahi ho sakta.");
        s.Phone = OrderService.NormalizePhone(s.Phone);
        try
        {
            if (s.SupplierId == 0) return await _repo.AddSupplierAsync(s);
            await _repo.UpdateSupplierAsync(s);
            return s.SupplierId;
        }
        catch (Exception ex) when (IsDup(ex, "SupplierName"))
        { throw new InvalidOperationException("Is naam ka supplier pehle se maujood hai."); }
    }

    // ---------------------------------------------------------------- shop checkout
    public async Task<int> CheckoutShopSaleAsync(ShopCheckoutRequest r)
    {
        if (r.Items.Count == 0) throw new InvalidOperationException("Kam az kam ek item jorein.");
        foreach (var i in r.Items)
        {
            if (i.ColorId <= 0) throw new InvalidOperationException("Har line mein color select karein.");
            if (i.Quantity <= 0) throw new InvalidOperationException("Quantity 0 se zyada hon.");
            if (i.SellingPrice < 0) throw new InvalidOperationException("Selling price minus nahi ho sakti.");
        }
        if (r.AmountPaid < 0) throw new InvalidOperationException("Paid amount minus nahi ho sakta.");
        r.Phone = OrderService.NormalizePhone(r.Phone);
        return await _repo.CheckoutShopSaleAsync(r);
    }

    // ---------------------------------------------------------------- money
    private static void CheckMoney(decimal amount, int accountId, string method)
    {
        if (amount <= 0) throw new InvalidOperationException("Amount 0 se zyada likhein.");
        if (accountId <= 0) throw new InvalidOperationException("Account chunen.");
        if (string.IsNullOrWhiteSpace(method)) throw new InvalidOperationException("Payment method chunen.");
    }

    public Task<int> CustomerReceiptAsync(int customerId, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        CheckMoney(amount, accountId, method);
        return _repo.CustomerReceiptAsync(customerId, amount, accountId, method, date, notes, key, user);
    }

    public Task<int> SupplierPaymentAsync(int supplierId, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        CheckMoney(amount, accountId, method);
        return _repo.SupplierPaymentAsync(supplierId, amount, accountId, method, date, notes, key, user);
    }

    public Task<int> TransferAsync(int fromId, int toId, decimal amount, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        CheckMoney(amount, fromId, method);
        if (toId <= 0 || toId == fromId) throw new InvalidOperationException("Dono account alag chunen.");
        return _repo.TransferAsync(fromId, toId, amount, method, date, notes, key, user);
    }

    public Task VoidAsync(int transactionId, string reason, string? user)
    {
        if (transactionId <= 0) throw new InvalidOperationException("Pehle entry select karein.");
        if (string.IsNullOrWhiteSpace(reason) || reason.Trim().Length < 3)
            throw new InvalidOperationException("Void ki wajah likhein.");
        return _repo.VoidTxnAsync(transactionId, reason.Trim(), user);
    }

    // ---------------------------------------------------------------- statements
    private static StatementResult Build(decimal opening, List<StatementRow> rows, DateTime? from, DateTime? to)
    {
        var res = new StatementResult { OpeningBefore = opening };
        var run = opening;
        foreach (var r in rows.OrderBy(x => x.Dt).ThenBy(x => x.Ord).ThenBy(x => x.RefId))
        {
            if (to.HasValue && r.Dt.Date > to.Value.Date) break;
            run += r.Increase - r.Decrease;
            if (from.HasValue && r.Dt.Date < from.Value.Date) { res.OpeningBefore = run; continue; }
            res.Lines.Add(new StatementLine
            {
                Date = r.Dt,
                Ref = r.Ref,
                Kind = r.Kind,
                Note = r.Note,
                Increase = r.Increase,
                Decrease = r.Decrease,
                Balance = run
            });
            res.TotalIncrease += r.Increase;
            res.TotalDecrease += r.Decrease;
        }
        res.Closing = run;
        return res;
    }

    public async Task<StatementResult> CustomerStatementAsync(int id, DateTime? from, DateTime? to)
    {
        var c = await _repo.GetCustomerAsync(id) ?? throw new InvalidOperationException("Customer nahi mila.");
        return Build(c.OpeningBalance, (await _repo.GetCustomerStatementAsync(id)).ToList(), from, to);
    }

    public async Task<StatementResult> SupplierStatementAsync(int id, DateTime? from, DateTime? to)
    {
        var s = (await _repo.GetSuppliersAsync(null, true)).FirstOrDefault(x => x.SupplierId == id)
                ?? throw new InvalidOperationException("Supplier nahi mila.");
        return Build(s.OpeningBalance, (await _repo.GetSupplierStatementAsync(id)).ToList(), from, to);
    }

    public Task<IEnumerable<DueItem>> GetDueAsync() => _repo.GetDueAsync();

    public Task<IEnumerable<CashBookRow>> GetCashBookAsync(int? accountId, DateTime? from, DateTime? to, bool includeVoid)
        => _repo.GetCashBookAsync(accountId, from, to, includeVoid);

    public async Task<int> PurchaseStockInAsync(PurchaseBillRequest r)
    {
        r.InvoiceNo = (r.InvoiceNo ?? "").Trim();
        if (r.SupplierId <= 0) throw new InvalidOperationException("Supplier chunen.");
        if (r.InvoiceNo.Length == 0) throw new InvalidOperationException("Bill number likhein.");
        if (r.Items.Count == 0) throw new InvalidOperationException("Kam az kam ek batch jorein.");
        foreach (var i in r.Items)
        {
            if (i.ColorId <= 0) throw new InvalidOperationException("Har line mein color chunen.");
            if (i.Bunches <= 0 || i.SuitsPerBunch <= 0 || i.MetersPerSuit <= 0)
                throw new InvalidOperationException("Bunches, suits aur meter 0 se zyada hon.");
            if (i.CostPerSuit < 0) throw new InvalidOperationException("Cost minus nahi ho sakti.");
        }
        var total = r.Total;
        if (total <= 0) throw new InvalidOperationException("Bill ka total 0 se zyada hona chahiye.");
        if (r.InvoiceDate.Date > DateTime.Today) throw new InvalidOperationException("Bill ki tareekh aane wali nahi ho sakti.");
        if (r.PaidNow < 0 || r.PaidNow > total) throw new InvalidOperationException("Abhi diya hua amount bill se zyada ya minus nahi ho sakta.");
        if (r.PaidNow < total && r.DueDate == null) throw new InvalidOperationException("Baqi udhaar hai to due date likhein.");
        if (r.DueDate.HasValue && r.DueDate.Value.Date < r.InvoiceDate.Date) throw new InvalidOperationException("Due date bill ki tareekh se pehle nahi ho sakti.");
        if (r.PaidNow > 0 && (r.AccountId == null || string.IsNullOrWhiteSpace(r.Method)))
            throw new InvalidOperationException("Paisa diya hai to account aur payment method chunen.");
        if (r.PaidNow >= total) r.DueDate = null;

        try { return await _repo.PurchaseStockInAsync(r); }
        catch (Exception ex) when (ex.Message.Contains("UQ_PurchaseInvoices", StringComparison.OrdinalIgnoreCase))
        { throw new InvalidOperationException("Is supplier ka ye bill number pehle se maujood hai."); }
    }
    public Task<IEnumerable<CodPending>> GetCodPendingAsync() => _repo.GetCodPendingAsync();

    public Task<int> CodRemitAsync(List<int> orderIds, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        if (orderIds.Count == 0) throw new InvalidOperationException("Kam az kam ek order chunen.");
        CheckMoney(amount, accountId, method);
        return _repo.CodRemitAsync(orderIds, amount, accountId, method, date, notes, key, user);
    }

    public Task<int> CashManualAsync(string direction, decimal amount, int accountId, string method,
        DateTime date, string? notes, string? key, string? user)
    {
        if (direction is not ("Receipt" or "Payment")) throw new InvalidOperationException("Qisam sahi nahi.");
        CheckMoney(amount, accountId, method);
        if (string.IsNullOrWhiteSpace(notes) || notes.Trim().Length < 3)
            throw new InvalidOperationException("Wajah (notes) likhna zaroori hai.");
        return _repo.CashManualAsync(direction, amount, accountId, method, date, notes.Trim(), key, user);
    }

    public Task<decimal> GetOrderPaidAsync(int orderId) => _repo.GetOrderPaidAsync(orderId);

    public Task<int?> ReturnShopSaleAsync(ShopReturnRequest r)
    {
        if (r.OrderId <= 0) throw new InvalidOperationException("Pehle sale select karein.");
        if (r.Items.Count == 0 || r.Items.Any(i => i.Qty <= 0))
            throw new InvalidOperationException("Return quantity 0 se zyada likhein.");
        if (r.RefundAmount < 0) throw new InvalidOperationException("Refund minus nahi ho sakta.");
        if (r.RefundAmount > 0 && (r.AccountId == null || string.IsNullOrWhiteSpace(r.Method)))
            throw new InvalidOperationException("Refund ke liye account aur payment method chunen.");
        return _repo.ReturnShopSaleAsync(r);
    }
}