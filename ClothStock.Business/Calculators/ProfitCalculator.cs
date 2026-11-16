namespace ClothStock.Business.Calculators;

public static class ProfitCalculator
{
    public static decimal Tax(decimal netSale, decimal taxPercent)
        => netSale * taxPercent / 100m;

    // Courier charge shipment banne par pata chalta hai, isliye abhi 0 rehta hai
    public static decimal ExpectedProfit(decimal netSale, decimal netCost,
        decimal taxPercent, decimal packaging, decimal courierShare = 0)
        => netSale - netCost - Tax(netSale, taxPercent) - courierShare - packaging;
}