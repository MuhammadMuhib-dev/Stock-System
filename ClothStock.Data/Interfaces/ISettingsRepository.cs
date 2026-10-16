using ClothStock.Models.System;

namespace ClothStock.Data.Interfaces;

public interface ISettingsRepository
{
    Task<IEnumerable<Setting>> GetAllAsync();
    Task<string?> GetValueAsync(string key);
    Task SetValueAsync(string key, string value);
}