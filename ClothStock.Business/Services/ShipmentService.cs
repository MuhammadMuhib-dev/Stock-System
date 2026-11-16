using ClothStock.Data.Interfaces;
using ClothStock.Models.Orders;

namespace ClothStock.Business.Services;

public class ShipmentService
{
    private readonly IShipmentRepository _repo;
    public ShipmentService(IShipmentRepository repo) { _repo = repo; }

    public Task<IEnumerable<ShipmentSummary>> GetAllAsync(string? status = null, string? search = null)
        => _repo.GetAllAsync(status, search);

    public Task<Shipment?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
    public Task<IEnumerable<OrderSummary>> GetOrdersAsync(int id) => _repo.GetOrdersAsync(id);

    private static void Check(ShipmentSaveRequest r)
    {
        r.TrackingId = (r.TrackingId ?? "").Trim();
        if (r.CourierId <= 0) throw new InvalidOperationException("Please select a courier.");
        if (r.TrackingId.Length == 0) throw new InvalidOperationException("Please enter tracking ID.");
        if (r.WeightKg < 0 || r.CourierCharge < 0 || r.ReturnCharge < 0)
            throw new InvalidOperationException("Weight or charge cannot be negative.");
    }

    public async Task<int> SaveAsync(ShipmentSaveRequest r)
    {
        Check(r);
        if (r.ShipmentId == 0)
        {
            if (r.OrderIds.Count == 0)
                throw new InvalidOperationException("Please select at least one order.");
            return await _repo.CreateAsync(r);
        }
        await _repo.UpdateAsync(r);
        return r.ShipmentId;
    }

    public Task AddOrdersAsync(int shipmentId, IEnumerable<int> orderIds, string? by)
    {
        var ids = orderIds.ToList();
        if (ids.Count == 0) throw new InvalidOperationException("No order selected.");
        return _repo.AddOrdersAsync(shipmentId, ids, by);
    }

    public Task RemoveOrderAsync(int orderId, string? by) => _repo.RemoveOrderAsync(orderId, by);

    public Task DispatchAsync(int shipmentId, DateTime? date, string? by)
        => _repo.SetStatusAsync(shipmentId, "Dispatched", date, by);

    public Task DeliverAsync(int shipmentId, DateTime? date, string? by)
        => _repo.SetStatusAsync(shipmentId, "Delivered", date, by);

    public Task CancelAsync(int shipmentId) => _repo.CancelAsync(shipmentId);

    public Task ReturnAsync(int shipmentId, decimal? returnCharge, DateTime? date, string? by)
    {
        if (returnCharge < 0) throw new InvalidOperationException("Return charge cannot be negative.");
        return _repo.ReturnAsync(shipmentId, returnCharge, date, by);
    }
}