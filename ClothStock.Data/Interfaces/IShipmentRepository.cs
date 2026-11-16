using ClothStock.Models.Orders;

namespace ClothStock.Data.Interfaces;

public interface IShipmentRepository
{
    Task<IEnumerable<ShipmentSummary>> GetAllAsync(string? status, string? search);
    Task<Shipment?> GetByIdAsync(int shipmentId);
    Task<IEnumerable<OrderSummary>> GetOrdersAsync(int shipmentId);
    Task<int> CreateAsync(ShipmentSaveRequest r);
    Task UpdateAsync(ShipmentSaveRequest r);
    Task AddOrdersAsync(int shipmentId, IEnumerable<int> orderIds, string? by);
    Task RemoveOrderAsync(int orderId, string? by);
    Task SetStatusAsync(int shipmentId, string newStatus, DateTime? date, string? by);
    Task CancelAsync(int shipmentId);
    Task ReturnAsync(int shipmentId, decimal? returnCharge, DateTime? date, string? by);
}