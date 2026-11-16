using ClothStock.Models.Orders;

namespace ClothStock.Data.Interfaces;

public interface ICourierRepository
{
    Task<IEnumerable<Courier>> GetAllAsync(bool activeOnly = true);
    Task<int> AddAsync(Courier c);
    Task UpdateAsync(Courier c);
    Task<bool> HasShipmentsAsync(int courierId);
    Task DeleteAsync(int courierId);
}