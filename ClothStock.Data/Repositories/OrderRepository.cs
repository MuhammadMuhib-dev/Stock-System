using System.Data;
using System.Text.Json;
using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Orders;
using Dapper;

namespace ClothStock.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DbConnectionFactory _factory;
    public OrderRepository(DbConnectionFactory factory) { _factory = factory; }

    public async Task<int> CreateAsync(OrderCreateRequest r)
    {
        using var db = _factory.Create();
        var json = JsonSerializer.Serialize(
            r.Items.Select(i => new { i.ColorId, i.Quantity, i.SellingPrice, i.BatchId }));

        var p = new DynamicParameters();
        p.Add("@Channel", r.Channel);
        p.Add("@CustomerName", r.CustomerName);
        p.Add("@Phone", r.Phone);
        p.Add("@Address", r.Address);
        p.Add("@City", r.City);
        p.Add("@PackagingCharge", r.PackagingCharge, DbType.Decimal);
        p.Add("@Notes", r.Notes);
        p.Add("@CreatedBy", r.CreatedBy);
        p.Add("@ItemsJson", json);
        p.Add("@NewOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await db.ExecuteAsync("dbo.usp_OrderCreate", p, commandType: CommandType.StoredProcedure);
        var id = p.Get<int>("@NewOrderId");

        if (r.Channel == "Online")
            await db.ExecuteAsync("dbo.usp_OrderSetCourier",
                new { OrderId = id, r.CourierId, r.TrackingId, r.CourierCharge },
                commandType: CommandType.StoredProcedure);
        else if (r.AmountPaid.HasValue)
            await db.ExecuteAsync("dbo.usp_OrderSetPaid",
                new { OrderId = id, AmountPaid = r.AmountPaid.Value },
                commandType: CommandType.StoredProcedure);

        return id;
    }

    public async Task CancelAsync(int orderId, string? by)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_OrderCancel",
            new { OrderId = orderId, UpdatedBy = by }, commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateInfoAsync(OrderUpdateInfoRequest r)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_OrderUpdateInfo",
            new
            {
                r.OrderId,
                r.CustomerName,
                r.Phone,
                r.Address,
                r.City,
                r.PackagingCharge,
                r.Notes,
                r.UpdatedBy
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task ReturnItemAsync(int orderItemId, int qty, string? by)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_OrderItemReturn",
            new { OrderItemId = orderItemId, ReturnQty = qty, UpdatedBy = by },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<OrderSummary>> GetOrdersAsync(OrderFilter f)
    {
        using var db = _factory.Create();
        var like = string.IsNullOrWhiteSpace(f.Search) ? null : "%" + f.Search.Trim() + "%";
        var sql = @"SELECT * FROM dbo.vw_OrderSummary
                    WHERE (@From IS NULL OR OrderDate >= @From)
                      AND (@To IS NULL OR OrderDate <= @To)
                      AND (@Channel IS NULL OR Channel = @Channel)
                      AND (@Status IS NULL OR Status = @Status)
                      AND (@Like IS NULL OR OrderNo LIKE @Like OR CustomerName LIKE @Like
                           OR Phone LIKE @Like OR TrackingId LIKE @Like)
                    ORDER BY OrderId DESC";
        return await db.QueryAsync<OrderSummary>(sql, new
        {
            From = f.From?.Date,
            To = f.To?.Date,
            f.Channel,
            f.Status,
            Like = like
        });
    }

    public async Task<IEnumerable<OrderSummary>> GetUnshippedAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<OrderSummary>(
            @"SELECT * FROM dbo.vw_OrderSummary
              WHERE Channel = 'Online' AND Status = 'Placed' AND ShipmentId IS NULL
              ORDER BY OrderId");
    }

    public async Task<IEnumerable<OrderLine>> GetLinesAsync(int orderId)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<OrderLine>(
            "SELECT * FROM dbo.vw_OrderLines WHERE OrderId = @OrderId ORDER BY OrderItemId",
            new { OrderId = orderId });
    }

    public async Task ReplaceShopSaleAsync(OrderReplaceRequest r)
    {
        using var db = _factory.Create();
        var json = JsonSerializer.Serialize(
            r.Items.Select(i => new { i.ColorId, i.Quantity, i.SellingPrice, i.BatchId }));

        await db.ExecuteAsync("dbo.usp_ShopSaleUpdate",
            new { r.OrderId, r.CustomerName, r.Phone, r.Notes, UpdatedBy = r.CreatedBy, ItemsJson = json },
            commandType: CommandType.StoredProcedure);

        if (r.AmountPaid.HasValue)
            await db.ExecuteAsync("dbo.usp_OrderSetPaid",
                new { r.OrderId, AmountPaid = r.AmountPaid.Value },
                commandType: CommandType.StoredProcedure);
    }

    public async Task ReplaceOnlineOrderAsync(OrderReplaceRequest r)
    {
        using var db = _factory.Create();
        var json = JsonSerializer.Serialize(
            r.Items.Select(i => new { i.ColorId, i.Quantity, i.SellingPrice, i.BatchId }));

        await db.ExecuteAsync("dbo.usp_OnlineOrderUpdate",
            new
            {
                r.OrderId,
                r.CustomerName,
                r.Phone,
                r.Address,
                r.City,
                PackagingCharge = r.PackagingCharge ?? 0,
                r.CourierId,
                r.TrackingId,
                r.CourierCharge,
                r.Notes,
                UpdatedBy = r.CreatedBy,
                ItemsJson = json
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task ReturnOrderAsync(int orderId, decimal? returnCharge, DateTime? date, string? by)
    {
        using var db = _factory.Create();
        var p = new DynamicParameters();
        p.Add("@OrderId", orderId);
        p.Add("@ReturnCharge", returnCharge, DbType.Decimal);
        p.Add("@ReturnedDate", date?.Date, DbType.Date);
        p.Add("@UpdatedBy", by);
        await db.ExecuteAsync("dbo.usp_OrderReturn", p, commandType: CommandType.StoredProcedure);
    }

    public async Task DeliverOrderAsync(int orderId, string? by)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("dbo.usp_OrderSetDelivered",
            new { OrderId = orderId, UpdatedBy = by }, commandType: CommandType.StoredProcedure);
    }
    public async Task<IEnumerable<SalesLine>> GetSalesLinesAsync(DateTime? from, DateTime? to)
    {
        using var db = _factory.Create();
        return await db.QueryAsync<SalesLine>(
            @"SELECT * FROM dbo.vw_SalesLines
              WHERE (@From IS NULL OR OrderDate >= @From) AND (@To IS NULL OR OrderDate <= @To)",
            new { From = from?.Date, To = to?.Date });
    }
}