using System.Data;
using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Expenses;
using Dapper;

namespace ClothStock.Data.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly DbConnectionFactory _factory;
    public ExpenseRepository(DbConnectionFactory factory) { _factory = factory; }

    public async Task<int> SaveAsync(ExpenseSaveRequest r)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@ExpenseId", r.ExpenseId);
        p.Add("@ExpenseDate", r.ExpenseDate.Date, DbType.Date);
        p.Add("@CategoryId", r.CategoryId);
        p.Add("@Channel", r.Channel);
        p.Add("@Amount", r.Amount, DbType.Decimal);
        p.Add("@WorkerId", r.WorkerId);
        p.Add("@ForMonth", r.ForMonth?.Date, DbType.Date);
        p.Add("@AdvanceCut", r.AdvanceCut, DbType.Decimal);
        p.Add("@PaymentMethod", r.PaymentMethod);
        p.Add("@Notes", r.Notes);
        p.Add("@User", r.User);
        p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("dbo.usp_ExpenseSave", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewId");
    }

    public async Task VoidAsync(int expenseId, string? user)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_ExpenseVoid",
            new { ExpenseId = expenseId, User = user }, commandType: CommandType.StoredProcedure);
    }

    public async Task<int> GenerateRecurringAsync(DateTime month, string? user)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@Month", month.Date, DbType.Date);
        p.Add("@User", user);
        return await db.ExecuteScalarAsync<int>("dbo.usp_RecurringGenerate", p,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ExpenseItem>> GetAsync(ExpenseFilter f)
    {
        using var db = _factory.Create();
        var like = string.IsNullOrWhiteSpace(f.Search) ? null : "%" + f.Search.Trim() + "%";
        return await db.QueryAsync<ExpenseItem>(
            @"SELECT * FROM dbo.vw_ExpenseList
              WHERE (@Period IS NULL OR PeriodMonth = @Period)
                AND (@Channel IS NULL OR Channel = @Channel)
                AND (@CategoryId IS NULL OR CategoryId = @CategoryId)
                AND (@IncludeVoid = 1 OR Status = 'Active')
                AND (@Like IS NULL OR CategoryName LIKE @Like OR WorkerName LIKE @Like OR Notes LIKE @Like)
              ORDER BY ExpenseDate DESC, ExpenseId DESC",
            new
            {
                Period = f.Month.HasValue ? new DateTime(f.Month.Value.Year, f.Month.Value.Month, 1) : (DateTime?)null,
                f.Channel,
                f.CategoryId,
                IncludeVoid = f.IncludeVoid ? 1 : 0,
                Like = like
            });
    }
}

public class WorkerRepository : IWorkerRepository
{
    private readonly DbConnectionFactory _factory;
    public WorkerRepository(DbConnectionFactory factory) { _factory = factory; }

    public async Task<IEnumerable<WorkerBalance>> GetAllAsync(bool activeOnly)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<WorkerBalance>(
            @"SELECT * FROM dbo.vw_WorkerBalance
              WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY WorkerName",
            new { ActiveOnly = activeOnly ? 1 : 0 });
    }

    public async Task<int> AddAsync(Worker w)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.Workers (WorkerName, Role, Phone, MonthlySalary, IsActive)
              VALUES (@WorkerName, @Role, @Phone, @MonthlySalary, @IsActive);
              SELECT CAST(SCOPE_IDENTITY() AS INT);", w);
    }

    public async Task UpdateAsync(Worker w)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            @"UPDATE dbo.Workers SET WorkerName=@WorkerName, Role=@Role, Phone=@Phone,
                     MonthlySalary=@MonthlySalary, IsActive=@IsActive WHERE WorkerId=@WorkerId", w);
    }

    public async Task<bool> HasExpensesAsync(int workerId)
    {
        using var db = _factory.Create();
        var n = await db.ExecuteScalarAsync<int>(
            @"SELECT (SELECT COUNT(*) FROM dbo.Expenses WHERE WorkerId=@Id)
                   + (SELECT COUNT(*) FROM dbo.RecurringExpenses WHERE WorkerId=@Id)",
            new { Id = workerId });
        return n > 0;
    }

    public async Task DeleteAsync(int workerId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("DELETE FROM dbo.Workers WHERE WorkerId=@Id", new { Id = workerId });
    }
}

public class ExpenseSetupRepository : IExpenseSetupRepository
{
    private readonly DbConnectionFactory _factory;
    public ExpenseSetupRepository(DbConnectionFactory factory) { _factory = factory; }

    // ---- categories
    public async Task<IEnumerable<ExpenseCategory>> GetCategoriesAsync(bool activeOnly)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<ExpenseCategory>(
            @"SELECT CategoryId, CategoryName, GroupName, Kind, DefaultChannel, IsActive
              FROM dbo.ExpenseCategories WHERE (@ActiveOnly = 0 OR IsActive = 1)
              ORDER BY GroupName, CategoryName",
            new { ActiveOnly = activeOnly ? 1 : 0 });
    }

    public async Task<int> AddCategoryAsync(ExpenseCategory c)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.ExpenseCategories (CategoryName, GroupName, Kind, DefaultChannel, IsActive)
              VALUES (@CategoryName, @GroupName, @Kind, @DefaultChannel, @IsActive);
              SELECT CAST(SCOPE_IDENTITY() AS INT);", c);
    }

    public async Task UpdateCategoryAsync(ExpenseCategory c)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            @"UPDATE dbo.ExpenseCategories SET CategoryName=@CategoryName, GroupName=@GroupName,
                     Kind=@Kind, DefaultChannel=@DefaultChannel, IsActive=@IsActive
              WHERE CategoryId=@CategoryId", c);
    }

    public async Task<bool> CategoryHasExpensesAsync(int categoryId)
    {
        using var db = _factory.Create();
        var n = await db.ExecuteScalarAsync<int>(
            @"SELECT (SELECT COUNT(*) FROM dbo.Expenses WHERE CategoryId=@Id)
                   + (SELECT COUNT(*) FROM dbo.RecurringExpenses WHERE CategoryId=@Id)",
            new { Id = categoryId });
        return n > 0;
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("DELETE FROM dbo.ExpenseCategories WHERE CategoryId=@Id", new { Id = categoryId });
    }

    // ---- payment methods
    public async Task<IEnumerable<PaymentMethod>> GetMethodsAsync(bool activeOnly)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<PaymentMethod>(
            "SELECT MethodName, IsActive FROM dbo.PaymentMethods WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY MethodName",
            new { ActiveOnly = activeOnly ? 1 : 0 });
    }

    public async Task AddMethodAsync(string name)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("INSERT INTO dbo.PaymentMethods (MethodName) VALUES (@Name)", new { Name = name });
    }

    public async Task SetMethodActiveAsync(string name, bool active)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("UPDATE dbo.PaymentMethods SET IsActive=@Active WHERE MethodName=@Name",
            new { Name = name, Active = active });
    }

    // ---- recurring (fixed kharche)
    public async Task<IEnumerable<RecurringItem>> GetRecurringAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<RecurringItem>(
            @"SELECT r.RecurringId, r.CategoryId, c.CategoryName, r.Channel, r.WorkerId, w.WorkerName,
                     r.Amount, r.PaymentMethod, r.Notes, r.IsActive
              FROM dbo.RecurringExpenses r
              JOIN dbo.ExpenseCategories c ON c.CategoryId = r.CategoryId
              LEFT JOIN dbo.Workers w ON w.WorkerId = r.WorkerId
              ORDER BY c.GroupName, c.CategoryName, w.WorkerName");
    }

    public async Task<int> AddRecurringAsync(RecurringItem r)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.RecurringExpenses (CategoryId, Channel, WorkerId, Amount, PaymentMethod, Notes, IsActive)
              VALUES (@CategoryId, @Channel, @WorkerId, @Amount, @PaymentMethod, @Notes, @IsActive);
              SELECT CAST(SCOPE_IDENTITY() AS INT);", r);
    }

    public async Task UpdateRecurringAsync(RecurringItem r)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            @"UPDATE dbo.RecurringExpenses SET CategoryId=@CategoryId, Channel=@Channel, WorkerId=@WorkerId,
                     Amount=@Amount, PaymentMethod=@PaymentMethod, Notes=@Notes, IsActive=@IsActive
              WHERE RecurringId=@RecurringId", r);
    }

    public async Task<bool> RecurringUsedAsync(int recurringId)
    {
        using var db = _factory.Create();
        var n = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.Expenses WHERE RecurringId=@Id", new { Id = recurringId });
        return n > 0;
    }

    public async Task DeleteRecurringAsync(int recurringId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("DELETE FROM dbo.RecurringExpenses WHERE RecurringId=@Id", new { Id = recurringId });
    }
}