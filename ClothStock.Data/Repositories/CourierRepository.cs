using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Orders;
using Dapper;

namespace ClothStock.Data.Repositories;

public class CourierRepository : ICourierRepository
{
    private readonly DbConnectionFactory _factory;
    public CourierRepository(DbConnectionFactory factory) { _factory = factory; }

    public async Task<IEnumerable<Courier>> GetAllAsync(bool activeOnly = true)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<Courier>(
            @"SELECT CourierId, CourierName, IsActive FROM dbo.Couriers
              WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY CourierName",
            new { ActiveOnly = activeOnly ? 1 : 0 });
    }

    public async Task<int> AddAsync(Courier c)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.Couriers (CourierName, IsActive) VALUES (@CourierName, @IsActive);
              SELECT CAST(SCOPE_IDENTITY() AS INT);", c);
    }

    public async Task UpdateAsync(Courier c)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            "UPDATE dbo.Couriers SET CourierName=@CourierName, IsActive=@IsActive WHERE CourierId=@CourierId", c);
    }

    public async Task<bool> HasShipmentsAsync(int courierId)
    {
        using var db = _factory.Create();
        var n = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.Shipments WHERE CourierId=@CourierId", new { CourierId = courierId });
        return n > 0;
    }

    public async Task DeleteAsync(int courierId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("DELETE FROM dbo.Couriers WHERE CourierId=@CourierId", new { CourierId = courierId });
    }
}