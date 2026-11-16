using ClothStock.Models.System;

namespace ClothStock.Data.Interfaces;

public interface IUserRepository
{
    Task<int> CountAsync();
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<int> AddAsync(User u);
    Task UpdateAsync(int id, string fullName, string role, bool active);
    Task SetPasswordAsync(int id, string hash, bool mustChange);
    Task TouchLoginAsync(int id);
    Task<int> CountActiveOwnersAsync(int excludeUserId);
}