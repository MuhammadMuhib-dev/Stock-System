using ClothStock.Business.Calculators;
using ClothStock.Data.Interfaces;
using ClothStock.Models.Orders;

namespace ClothStock.Business.Services;

public class OrderService
{
    private readonly IOrderRepository _orders;
    private readonly SettingsService _settings;

    public OrderService(IOrderRepository orders, SettingsService settings)
    {
        _orders = orders;
        _settings = settings;
    }

    // Order form ke liye defaults (Settings se, fixed nahi)
    public Task<decimal> GetTaxPercentAsync() => _settings.GetDecimalAsync("TaxPercent", 0);
    public Task<decimal> GetDefaultPackagingAsync() => _settings.GetDecimalAsync("PackagingPerOrder", 0);

    public decimal ExpectedProfit(decimal netSale, decimal netCost, decimal taxPercent, decimal packaging)
        => ProfitCalculator.ExpectedProfit(netSale, netCost, taxPercent, packaging);

    public async Task<int> CreateAsync(OrderCreateRequest r)

    {

        if (r.Channel != "Online" && r.Channel != "Shop")
            throw new InvalidOperationException("Channel must be either Online or Shop.");

        r.Phone = NormalizePhone(r.Phone);
        if (r.CourierCharge < 0) throw new InvalidOperationException("Courier charge cannot be negative.");

        if (r.Channel == "Online")
        {
            if (string.IsNullOrWhiteSpace(r.CustomerName)) throw new InvalidOperationException("Please enter customer name.");
            if (string.IsNullOrWhiteSpace(r.Phone)) throw new InvalidOperationException("Please enter phone number.");
            if (string.IsNullOrWhiteSpace(r.Address)) throw new InvalidOperationException("Please enter address.");
            if (string.IsNullOrWhiteSpace(r.City)) throw new InvalidOperationException("Please enter city.");
        }

        if (r.Items.Count == 0) throw new InvalidOperationException("Please add at least one item.");
        foreach (var i in r.Items)
        {
            if (i.ColorId <= 0) throw new InvalidOperationException("Please select a color for each line.");
            if (i.Quantity <= 0) throw new InvalidOperationException("Quantity must be greater than 0.");
            if (i.SellingPrice < 0) throw new InvalidOperationException("Selling price cannot be negative.");
        }
        if (r.PackagingCharge < 0) throw new InvalidOperationException("Packaging charge cannot be negative.");

        return await _orders.CreateAsync(r);
    }

    public Task CancelAsync(int orderId, string? by)
    {
        if (orderId <= 0) throw new InvalidOperationException("Please select an order first.");
        return _orders.CancelAsync(orderId, by);
    }

    public Task UpdateInfoAsync(OrderUpdateInfoRequest r)
    {
        if (r.OrderId <= 0) throw new InvalidOperationException("Please select an order first.");
        return _orders.UpdateInfoAsync(r);
    }

    public Task ReturnItemAsync(int orderItemId, int qty, string? by)
    {
        if (orderItemId <= 0) throw new InvalidOperationException("Please select a line first.");
        if (qty <= 0) throw new InvalidOperationException("Please enter return quantity greater than 0.");
        return _orders.ReturnItemAsync(orderItemId, qty, by);
    }

    public Task<IEnumerable<OrderSummary>> GetOrdersAsync(OrderFilter f) => _orders.GetOrdersAsync(f);
    public Task<IEnumerable<OrderSummary>> GetUnshippedAsync() => _orders.GetUnshippedAsync();
    public Task<IEnumerable<OrderLine>> GetLinesAsync(int orderId) => _orders.GetLinesAsync(orderId);
    public Task<IEnumerable<SalesLine>> GetSalesLinesAsync(DateTime? from, DateTime? to) => _orders.GetSalesLinesAsync(from, to);
    public Task<bool> GetAllowBelowCostAsync() => _settings.GetBoolAsync("AllowSaleBelowCost", false);

    // Phone 11 digit, 03 se shuru (jaise 03001234567). Khali ho to null.
    public static string? NormalizePhone(string? raw)
    {
        var d = new string((raw ?? "").Where(char.IsDigit).ToArray());
        if (d.Length == 0) return null;
        if (d.StartsWith("92") && d.Length == 12) d = "0" + d.Substring(2);
        if (d.Length != 11 || !d.StartsWith("03"))
            throw new InvalidOperationException("Phone number must be 11 digits and start with 03, like 03001234567.");
        return d;
    }

    public async Task UpdateShopSaleAsync(OrderReplaceRequest r)
    {
        if (r.OrderId <= 0) throw new InvalidOperationException("Please select a sale first.");
        r.Phone = NormalizePhone(r.Phone);
        if (r.Items.Count == 0) throw new InvalidOperationException("Please add at least one item.");
        foreach (var i in r.Items)
        {
            if (i.ColorId <= 0) throw new InvalidOperationException("Please select a color for each line.");
            if (i.Quantity <= 0) throw new InvalidOperationException("Quantity must be greater than 0.");
            if (i.SellingPrice < 0) throw new InvalidOperationException("Selling price cannot be negative.");
        }
        await _orders.ReplaceShopSaleAsync(r);
    }

    public async Task UpdateOnlineOrderAsync(OrderReplaceRequest r)
    {
        if (r.OrderId <= 0) throw new InvalidOperationException("Please select an order first.");
        r.Phone = NormalizePhone(r.Phone);
        if (string.IsNullOrWhiteSpace(r.CustomerName)) throw new InvalidOperationException("Please enter customer name.");
        if (string.IsNullOrWhiteSpace(r.Phone)) throw new InvalidOperationException("Please enter phone number.");
        if (string.IsNullOrWhiteSpace(r.Address)) throw new InvalidOperationException("Please enter address.");
        if (string.IsNullOrWhiteSpace(r.City)) throw new InvalidOperationException("Please enter city.");
        if (r.Items.Count == 0) throw new InvalidOperationException("Please add at least one item.");
        if (r.CourierCharge < 0 || r.PackagingCharge < 0)
            throw new InvalidOperationException("Charge cannot be negative.");
        foreach (var i in r.Items)
        {
            if (i.ColorId <= 0) throw new InvalidOperationException("Please select a color for each line.");
            if (i.Quantity <= 0) throw new InvalidOperationException("Quantity must be greater than 0.");
            if (i.SellingPrice < 0) throw new InvalidOperationException("Selling price cannot be negative.");
        }
        await _orders.ReplaceOnlineOrderAsync(r);
    }

    public Task ReturnOrderAsync(int orderId, decimal? returnCharge, string? by)
    {
        if (orderId <= 0) throw new InvalidOperationException("Please select an order first.");
        if (returnCharge < 0) throw new InvalidOperationException("Return charge cannot be negative.");
        return _orders.ReturnOrderAsync(orderId, returnCharge, null, by);
    }

    public Task DeliverOrderAsync(int orderId, string? by)
    {
        if (orderId <= 0) throw new InvalidOperationException("Please select an order first.");
        return _orders.DeliverOrderAsync(orderId, by);
    }
}