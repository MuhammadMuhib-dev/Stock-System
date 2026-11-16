using ClothStock.Models.System;

namespace ClothStockSystem.Helpers;

public static class SessionManager
{
    public static int UserId { get; private set; }
    public static string Username { get; private set; } = "system";
    public static string FullName { get; private set; } = "";
    public static string Role { get; private set; } = "";
    public static bool SellerSeesCost { get; set; }
    public static bool LogoutRequested { get; set; }

    public static bool IsOwner => Role == Roles.Owner;
    public static bool CanSeeCost => IsOwner || SellerSeesCost;

    public static void Set(User u)
    {
        UserId = u.UserId; Username = u.Username; FullName = u.FullName; Role = u.Role;
    }

    public static void Clear()
    {
        UserId = 0; Username = "system"; FullName = ""; Role = ""; SellerSeesCost = false; LogoutRequested = false;
    }
}