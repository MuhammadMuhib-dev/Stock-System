namespace ClothStock.Models.System;

public static class Roles
{
    public const string Owner = "Owner";
    public const string Seller = "Seller";
}

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string FullName { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = Roles.Seller;
    public bool IsActive { get; set; } = true;
    public bool MustChangePassword { get; set; }
    public DateTime? LastLoginAt { get; set; }
}