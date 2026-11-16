using ClothStock.Data.Interfaces;
using ClothStock.Models.Expenses;

namespace ClothStock.Business.Services;

public class ExpenseService
{
    private readonly IExpenseRepository _repo;
    private readonly SettingsService _settings;

    public ExpenseService(IExpenseRepository repo, SettingsService settings)
    {
        _repo = repo;
        _settings = settings;
    }

    public Task<decimal> GetSharedShopPercentAsync() => _settings.GetDecimalAsync("SharedShopPercent", 50);

    public async Task<int> SaveAsync(ExpenseSaveRequest r)
    {
        if (r.CategoryId <= 0) throw new InvalidOperationException("Category select karein.");
        if (r.Channel is not ("Shop" or "Online" or "Shared"))
            throw new InvalidOperationException("Channel Shop, Online ya Shared chunen.");
        if (r.Amount <= 0) throw new InvalidOperationException("Amount 0 se zyada likhein.");
        if (r.ExpenseDate.Date > DateTime.Today)
            throw new InvalidOperationException("Kharche ki tareekh aane wali nahi ho sakti.");
        if (r.AdvanceCut < 0 || r.AdvanceCut > r.Amount)
            throw new InvalidOperationException("Advance kata amount se zyada ya minus nahi ho sakta.");
        if (string.IsNullOrWhiteSpace(r.PaymentMethod))
            throw new InvalidOperationException("Payment method chunen.");
        return await _repo.SaveAsync(r);
    }

    public Task VoidAsync(int expenseId, string? user)
    {
        if (expenseId <= 0) throw new InvalidOperationException("Pehle list se entry select karein.");
        return _repo.VoidAsync(expenseId, user);
    }

    public Task<int> GenerateRecurringAsync(DateTime month, string? user) => _repo.GenerateRecurringAsync(month, user);

    public Task<IEnumerable<ExpenseItem>> GetAsync(ExpenseFilter f) => _repo.GetAsync(f);
}

public class WorkerService
{
    private readonly IWorkerRepository _repo;
    public WorkerService(IWorkerRepository repo) { _repo = repo; }

    public Task<IEnumerable<WorkerBalance>> GetAllAsync(bool activeOnly = true) => _repo.GetAllAsync(activeOnly);

    public async Task<int> SaveAsync(Worker w)
    {
        w.WorkerName = w.WorkerName.Trim();
        if (w.WorkerName.Length == 0) throw new InvalidOperationException("Worker ka naam likhein.");
        if (w.MonthlySalary < 0) throw new InvalidOperationException("Salary minus nahi ho sakti.");
        w.Phone = OrderService.NormalizePhone(w.Phone);

        if (w.WorkerId == 0) return await _repo.AddAsync(w);
        await _repo.UpdateAsync(w);
        return w.WorkerId;
    }

    public async Task DeleteAsync(int workerId)
    {
        if (workerId <= 0) throw new InvalidOperationException("Pehle list se worker select karein.");
        if (await _repo.HasExpensesAsync(workerId))
            throw new InvalidOperationException(
                "Is worker ki salary/advance ki entries hain, delete nahi ho sakta.\nIsay chhupane ke liye Active ka tick hata kar Save karein.");
        await _repo.DeleteAsync(workerId);
    }
}

public class ExpenseSetupService
{
    private readonly IExpenseSetupRepository _repo;
    public ExpenseSetupService(IExpenseSetupRepository repo) { _repo = repo; }

    // categories
    public Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync(bool activeOnly = true) => _repo.GetCategoriesAsync(activeOnly);

    public async Task<int> SaveCategoryAsync(ExpenseCategory c)
    {
        c.CategoryName = c.CategoryName.Trim();
        c.GroupName = c.GroupName.Trim();
        if (c.CategoryName.Length == 0) throw new InvalidOperationException("Category ka naam likhein.");
        if (c.GroupName.Length == 0) throw new InvalidOperationException("Group likhein (jaise Shop, Staff, Marketing).");
        if (c.Kind is not ("Normal" or "Salary" or "Bonus" or "Advance"))
            throw new InvalidOperationException("Kind sahi nahi.");
        if (c.DefaultChannel is not ("Shop" or "Online" or "Shared"))
            throw new InvalidOperationException("Channel sahi nahi.");

        if (c.CategoryId == 0) return await _repo.AddCategoryAsync(c);
        await _repo.UpdateCategoryAsync(c);
        return c.CategoryId;
    }

    public async Task DeleteCategoryAsync(int id)
    {
        if (id <= 0) throw new InvalidOperationException("Pehle list se category select karein.");
        if (await _repo.CategoryHasExpensesAsync(id))
            throw new InvalidOperationException(
                "Is category ki entries hain, delete nahi ho sakti.\nIsay chhupane ke liye Active ka tick hata kar Save karein.");
        await _repo.DeleteCategoryAsync(id);
    }

    // payment methods
    public Task<IEnumerable<PaymentMethod>> GetMethodsAsync(bool activeOnly = true) => _repo.GetMethodsAsync(activeOnly);

    public async Task AddMethodAsync(string name)
    {
        name = (name ?? "").Trim();
        if (name.Length == 0) throw new InvalidOperationException("Payment method ka naam likhein.");
        await _repo.AddMethodAsync(name);
    }

    public Task SetMethodActiveAsync(string name, bool active) => _repo.SetMethodActiveAsync(name, active);

    // fixed kharche
    public Task<IEnumerable<RecurringItem>> GetRecurringAsync() => _repo.GetRecurringAsync();

    public async Task<int> SaveRecurringAsync(RecurringItem r)
    {
        if (r.CategoryId <= 0) throw new InvalidOperationException("Category select karein.");
        if (r.Channel is not ("Shop" or "Online" or "Shared")) throw new InvalidOperationException("Channel sahi nahi.");
        if (r.Amount < 0) throw new InvalidOperationException("Amount minus nahi ho sakta.");
        if (r.WorkerId == null && (r.Amount == null || r.Amount <= 0))
            throw new InvalidOperationException("Amount likhein (ya salary ke liye worker select karein).");
        if (string.IsNullOrWhiteSpace(r.PaymentMethod)) throw new InvalidOperationException("Payment method chunen.");

        if (r.RecurringId == 0) return await _repo.AddRecurringAsync(r);
        await _repo.UpdateRecurringAsync(r);
        return r.RecurringId;
    }

    public async Task DeleteRecurringAsync(int id)
    {
        if (id <= 0) throw new InvalidOperationException("Pehle list se entry select karein.");
        if (await _repo.RecurringUsedAsync(id))
            throw new InvalidOperationException(
                "Is se kharche ban chuke hain, delete nahi ho sakta.\nIsay band karne ke liye Active ka tick hata kar Save karein.");
        await _repo.DeleteRecurringAsync(id);
    }
}