using System.Data;
using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Orders;
using Dapper;

namespace ClothStock.Data.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private readonly DbConnectionFactory _factory;
    public ShipmentRepository(DbConnectionFactory factory) { _factory = factory; }

    public async Task<IEnumerable<ShipmentSummary>> GetAllAsync(string? status, string? search)
    {
        using var db = _factory.Create();
        var like = string.IsNullOrWhiteSpace(search) ? null : "%" + search.Trim() + "%";
        return await db.QueryAsync<ShipmentSummary>(
            @"SELECT * FROM dbo.vw_ShipmentSummary
              WHERE (@Status IS NULL OR Status = @Status)
                AND (@Like IS NULL OR TrackingId LIKE @Like OR CustomerName LIKE @Like OR CourierName LIKE @Like)
              ORDER BY ShipmentId DESC",
            new { Status = status, Like = like });
    }

    public async Task<Shipment?> GetByIdAsync(int shipmentId)
    {
        using var db = _factory.Create();
        return await db.QueryFirstOrDefaultAsync<Shipment>(
            @"SELECT ShipmentId, CourierId, TrackingId, WeightKg, CourierCharge, ReturnCharge, Status, Notes
              FROM dbo.Shipments WHERE ShipmentId = @ShipmentId",
            new { ShipmentId = shipmentId });
    }

    public async Task<IEnumerable<OrderSummary>> GetOrdersAsync(int shipmentId)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<OrderSummary>(
            "SELECT * FROM dbo.vw_OrderSummary WHERE ShipmentId = @ShipmentId ORDER BY OrderId",
            new { ShipmentId = shipmentId });
    }

    public async Task<int> CreateAsync(ShipmentSaveRequest r)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@CourierId", r.CourierId);
        p.Add("@TrackingId", r.TrackingId);
        p.Add("@WeightKg", r.WeightKg);
        p.Add("@CourierCharge", r.CourierCharge);
        p.Add("@Notes", r.Notes);
        p.Add("@CreatedBy", r.CreatedBy);
        p.Add("@OrderIds", r.OrderIds.Count == 0 ? null : string.Join(",", r.OrderIds));
        p.Add("@NewShipmentId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("dbo.usp_ShipmentCreate", p, commandType: CommandType.StoredProcedure);
        return p.Get<int>("@NewShipmentId");
    }

    public async Task UpdateAsync(ShipmentSaveRequest r)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_ShipmentUpdate",
            new
            {
                r.ShipmentId,
                r.CourierId,
                r.TrackingId,
                r.WeightKg,
                r.CourierCharge,
                r.ReturnCharge,
                r.Notes
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task AddOrdersAsync(int shipmentId, IEnumerable<int> orderIds, string? by)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_ShipmentAddOrders",
            new { ShipmentId = shipmentId, OrderIds = string.Join(",", orderIds), UpdatedBy = by },
            commandType: CommandType.StoredProcedure);
    }

    public async Task RemoveOrderAsync(int orderId, string? by)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_ShipmentRemoveOrder",
            new { OrderId = orderId, UpdatedBy = by }, commandType: CommandType.StoredProcedure);
    }

    public async Task SetStatusAsync(int shipmentId, string newStatus, DateTime? date, string? by)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@ShipmentId", shipmentId);
        p.Add("@NewStatus", newStatus);
        p.Add("@Date", date?.Date, DbType.Date);
        p.Add("@UpdatedBy", by);
        await db.ExecuteAsync("dbo.usp_ShipmentSetStatus", p, commandType: CommandType.StoredProcedure);
    }

    public async Task CancelAsync(int shipmentId)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_ShipmentCancel",
            new { ShipmentId = shipmentId }, commandType: CommandType.StoredProcedure);
    }

    public async Task ReturnAsync(int shipmentId, decimal? returnCharge, DateTime? date, string? by)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@ShipmentId", shipmentId);
        p.Add("@ReturnCharge", returnCharge, DbType.Decimal);
        p.Add("@ReturnedDate", date?.Date, DbType.Date);
        p.Add("@UpdatedBy", by);
        await db.ExecuteAsync("dbo.usp_ShipmentReturn", p, commandType: CommandType.StoredProcedure);
    }
}
