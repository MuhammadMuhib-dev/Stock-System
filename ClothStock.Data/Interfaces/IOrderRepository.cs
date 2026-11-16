using ClothStock.Models.Orders;

namespace ClothStock.Data.Interfaces;

public interface IOrderRepository
{
    Task<int> CreateAsync(OrderCreateRequest r);
    Task CancelAsync(int orderId, string? by);
    Task UpdateInfoAsync(OrderUpdateInfoRequest r);
    Task ReturnItemAsync(int orderItemId, int qty, string? by);
    Task<IEnumerable<OrderSummary>> GetOrdersAsync(OrderFilter f);
    Task<IEnumerable<OrderSummary>> GetUnshippedAsync();
    Task<IEnumerable<OrderLine>> GetLinesAsync(int orderId);

    Task ReplaceShopSaleAsync(OrderReplaceRequest r);

    Task ReplaceOnlineOrderAsync(OrderReplaceRequest r);
    Task ReturnOrderAsync(int orderId, decimal? returnCharge, DateTime? date, string? by);
    Task DeliverOrderAsync(int orderId, string? by);
    Task<IEnumerable<SalesLine>> GetSalesLinesAsync(DateTime? from, DateTime? to);
}