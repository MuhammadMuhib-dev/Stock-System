
using System.Text.RegularExpressions;
using ClothStock.Data.Interfaces;
using ClothStock.Models.System;

namespace ClothStock.Business.Services;

public class AuthService
{
    private readonly IUserRepository _repo;
    public AuthService(IUserRepository repo) { _repo = repo; }

    private static string Hash(string pw) => BCrypt.Net.BCrypt.HashPassword(pw);

    private static void CheckPassword(string pw)
    {
        if (string.IsNullOrEmpty(pw) || pw.Length < 6)
            throw new InvalidOperationException("Password must be at least 6 characters long.");
    }

    // Pehli dafa: owner aur seller bana do (password badalna zaroori hoga)
    public async Task EnsureSeedAsync()
    {
        if (await _repo.CountAsync() > 0) return;
        await _repo.AddAsync(new User
        {
            Username = "owner",
            FullName = "Owner",
            Role = Roles.Owner,
            PasswordHash = Hash("Owner@123"),
            MustChangePassword = true
        });
        await _repo.AddAsync(new User
        {
            Username = "seller",
            FullName = "Online Seller",
            Role = Roles.Seller,
            PasswordHash = Hash("Seller@123"),
            MustChangePassword = true
        });
    }

    // Galat username ya password dono par ek hi message (andaza na lagaya ja sake)
    public async Task<User?> LoginAsync(string username, string password)
    {
        var u = await _repo.GetByUsernameAsync((username ?? "").Trim());
        if (u == null || !u.IsActive) { await Task.Delay(600); return null; }
        if (!BCrypt.Net.BCrypt.Verify(password ?? "", u.PasswordHash)) { await Task.Delay(600); return null; }
        await _repo.TouchLoginAsync(u.UserId);
        return u;
    }

    public async Task ChangePasswordAsync(int userId, string oldPw, string newPw)
    {
        var u = await _repo.GetByIdAsync(userId) ?? throw new InvalidOperationException("User not found.");
        if (!BCrypt.Net.BCrypt.Verify(oldPw ?? "", u.PasswordHash))
            throw new InvalidOperationException("The current password is incorrect.");
        CheckPassword(newPw);
        if (newPw == oldPw) throw new InvalidOperationException("The new password must be different from the current password.");
        await _repo.SetPasswordAsync(userId, Hash(newPw), false);
    }

    public Task<IEnumerable<User>> GetUsersAsync() => _repo.GetAllAsync();

    public async Task<int> CreateUserAsync(string username, string fullName, string role, string password)
    {
        username = (username ?? "").Trim();
        if (!Regex.IsMatch(username, @"^[A-Za-z0-9_]{3,30}$"))
            throw new InvalidOperationException("Username must be 3 to 30 characters long and contain only English letters, numbers, or underscores.");
        if (string.IsNullOrWhiteSpace(fullName))
            throw new InvalidOperationException("Please enter a name.");
        if (role != Roles.Owner && role != Roles.Seller)
            throw new InvalidOperationException("Invalid role selected.");
        CheckPassword(password);
        if (await _repo.GetByUsernameAsync(username) != null)
            throw new InvalidOperationException("This username already exists.");

        return await _repo.AddAsync(new User
        {
            Username = username,
            FullName = fullName.Trim(),
            Role = role,
            PasswordHash = Hash(password),
            MustChangePassword = true
        });
    }

    public async Task UpdateUserAsync(int id, string fullName, string role, bool active)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new InvalidOperationException("Please enter a name.");
        if (role != Roles.Owner && role != Roles.Seller)
            throw new InvalidOperationException("Invalid role selected.");
        if ((role != Roles.Owner || !active) && await _repo.CountActiveOwnersAsync(id) == 0)
            throw new InvalidOperationException("At least one active Owner must exist.");
        await _repo.UpdateAsync(id, fullName.Trim(), role, active);
    }

    public async Task ResetPasswordAsync(int id, string newPw)
    {
        CheckPassword(newPw);
        await _repo.SetPasswordAsync(id, Hash(newPw), true);
    }
}
