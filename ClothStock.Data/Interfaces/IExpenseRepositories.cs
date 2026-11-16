using ClothStock.Models.Expenses;

namespace ClothStock.Data.Interfaces;

public interface IExpenseRepository
{
    Task<int> SaveAsync(ExpenseSaveRequest r);
    Task VoidAsync(int expenseId, string? user);
    Task<int> GenerateRecurringAsync(DateTime month, string? user);
    Task<IEnumerable<ExpenseItem>> GetAsync(ExpenseFilter f);
}

public interface IWorkerRepository
{
    Task<IEnumerable<WorkerBalance>> GetAllAsync(bool activeOnly);
    Task<int> AddAsync(Worker w);
    Task UpdateAsync(Worker w);
    Task<bool> HasExpensesAsync(int workerId);
    Task DeleteAsync(int workerId);
}

public interface IExpenseSetupRepository
{
    Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync(bool activeOnly);
    Task<int> AddCategoryAsync(ExpenseCategory c);
    Task UpdateCategoryAsync(ExpenseCategory c);
    Task<bool> CategoryHasExpensesAsync(int categoryId);
    Task DeleteCategoryAsync(int categoryId);

    Task<IEnumerable<PaymentMethod>> GetMethodsAsync(bool activeOnly);
    Task AddMethodAsync(string name);
    Task SetMethodActiveAsync(string name, bool active);

    Task<IEnumerable<RecurringItem>> GetRecurringAsync();
    Task<int> AddRecurringAsync(RecurringItem r);
    Task UpdateRecurringAsync(RecurringItem r);
    Task<bool> RecurringUsedAsync(int recurringId);
    Task DeleteRecurringAsync(int recurringId);
}