using ClothStock.Data.Interfaces;
using ClothStock.Models.System;

namespace ClothStock.Business.Services;

public class SettingsService
{
    private readonly ISettingsRepository _repo;

    public SettingsService(ISettingsRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<Setting>> GetAllAsync() => _repo.GetAllAsync();

    public async Task<string> GetTextAsync(string key, string defaultValue = "")
    {
        var v = await _repo.GetValueAsync(key);
        return string.IsNullOrWhiteSpace(v) ? defaultValue : v;
    }

    public async Task<decimal> GetDecimalAsync(string key, decimal defaultValue = 0)
    {
        var v = await _repo.GetValueAsync(key);
        return decimal.TryParse(v, out var d) ? d : defaultValue;
    }

    public async Task<int> GetIntAsync(string key, int defaultValue = 0)
    {
        var v = await _repo.GetValueAsync(key);
        return int.TryParse(v, out var i) ? i : defaultValue;
    }

    public async Task<bool> GetBoolAsync(string key, bool defaultValue = false)
    {
        var v = await _repo.GetValueAsync(key);
        return v == null ? defaultValue : v == "1" || v.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public Task SaveAsync(string key, string value) => _repo.SetValueAsync(key, value);
}