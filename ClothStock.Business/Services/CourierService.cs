using ClothStock.Data.Interfaces;
using ClothStock.Models.Orders;

namespace ClothStock.Business.Services;

public class CourierService
{
    private readonly ICourierRepository _repo;
    public CourierService(ICourierRepository repo) { _repo = repo; }

    public Task<IEnumerable<Courier>> GetAllAsync(bool activeOnly = true) => _repo.GetAllAsync(activeOnly);

    public async Task<int> SaveAsync(Courier c)
    {
        c.CourierName = c.CourierName.Trim();
        if (c.CourierName.Length == 0) throw new InvalidOperationException("Courier ka naam likhein.");

        if (c.CourierId == 0) return await _repo.AddAsync(c);
        await _repo.UpdateAsync(c);
        return c.CourierId;
    }

    public async Task DeleteAsync(int courierId)
    {
        if (courierId <= 0) throw new InvalidOperationException("Pehle list se courier select karein.");
        if (await _repo.HasShipmentsAsync(courierId))
            throw new InvalidOperationException(
                "Is courier se shipments bani hain, delete nahi ho sakta.\nIsay chhupane ke liye Active ka tick hata kar Save karein.");
        await _repo.DeleteAsync(courierId);
    }
}