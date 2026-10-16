using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.System;
using Dapper;

namespace ClothStock.Data.Repositories;

public class SettingsRepository : ISettingsRepository
{
    private readonly DbConnectionFactory _factory;

    public SettingsRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<Setting>> GetAllAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<Setting>(
            "SELECT SettingKey, SettingValue, Description FROM dbo.Settings ORDER BY SettingKey");
    }

    public async Task<string?> GetValueAsync(string key)
    {
        using var db = _factory.Create();
        return await db.QueryFirstOrDefaultAsync<string>(
            "SELECT SettingValue FROM dbo.Settings WHERE SettingKey = @Key", new { Key = key });
    }

    public async Task SetValueAsync(string key, string value)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            "UPDATE dbo.Settings SET SettingValue = @Value WHERE SettingKey = @Key",
            new { Key = key, Value = value });
    }
}