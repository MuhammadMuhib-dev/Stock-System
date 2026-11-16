using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Models.System;
using Dapper;

namespace ClothStock.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _factory;
    public UserRepository(DbConnectionFactory factory) { _factory = factory; }

    private const string Cols =
        "UserId, Username, FullName, PasswordHash, Role, IsActive, MustChangePassword, LastLoginAt";

    public async Task<int> CountAsync()
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM dbo.Users");
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var db = _factory.Create();
        return await db.QueryFirstOrDefaultAsync<User>(
            $"SELECT {Cols} FROM dbo.Users WHERE Username = @Username", new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var db = _factory.Create();
        return await db.QueryFirstOrDefaultAsync<User>(
            $"SELECT {Cols} FROM dbo.Users WHERE UserId = @Id", new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var db = _factory.Create();
        return await db.QueryAsync<User>($"SELECT {Cols} FROM dbo.Users ORDER BY Username");
    }

    public async Task<int> AddAsync(User u)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            @"INSERT INTO dbo.Users (Username, FullName, PasswordHash, Role, IsActive, MustChangePassword)
              VALUES (@Username, @FullName, @PasswordHash, @Role, @IsActive, @MustChangePassword);
              SELECT CAST(SCOPE_IDENTITY() AS INT);", u);
    }

    public async Task UpdateAsync(int id, string fullName, string role, bool active)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            "UPDATE dbo.Users SET FullName=@FullName, Role=@Role, IsActive=@Active WHERE UserId=@Id",
            new { Id = id, FullName = fullName, Role = role, Active = active });
    }

    public async Task SetPasswordAsync(int id, string hash, bool mustChange)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync(
            "UPDATE dbo.Users SET PasswordHash=@Hash, MustChangePassword=@Must WHERE UserId=@Id",
            new { Id = id, Hash = hash, Must = mustChange });
    }

    public async Task TouchLoginAsync(int id)
    {
        using var db = _factory.Create();
        await db.ExecuteAsync("UPDATE dbo.Users SET LastLoginAt = SYSDATETIME() WHERE UserId=@Id", new { Id = id });
    }

    public async Task<int> CountActiveOwnersAsync(int excludeUserId)
    {
        using var db = _factory.Create();
        return await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM dbo.Users WHERE Role='Owner' AND IsActive=1 AND UserId <> @Id",
            new { Id = excludeUserId });
    }
}