using ClothStock.Models.Stock;

namespace ClothStock.Data.Interfaces;

public interface IColorRepository
{
    Task<IEnumerable<Color>> GetByArticleAsync(int articleId, bool activeOnly = true);
    Task<int> AddAsync(Color color);
    Task UpdateAsync(Color color);
    Task<bool> HasBatchesAsync(int colorId);
    Task DeleteAsync(int colorId);
}