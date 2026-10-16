using ClothStock.Models.Stock;

namespace ClothStock.Data.Interfaces;

public interface IArticleRepository
{
    Task<IEnumerable<Article>> GetAllAsync(bool activeOnly = true);
    Task<Article?> GetByIdAsync(int articleId);
    Task<int> AddAsync(Article article);
    Task UpdateAsync(Article article);
    Task<bool> HasColorsAsync(int articleId);
    Task DeleteAsync(int articleId);
}