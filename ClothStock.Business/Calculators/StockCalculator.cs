namespace ClothStock.Business.Calculators;

public static class StockCalculator
{
    // 1 bunch ke meter = suits per bunch x meter per suit
    public static decimal MetersPerBunch(int suitsPerBunch, decimal metersPerSuit)
        => suitsPerBunch * metersPerSuit;

    // Total meter = bunches x suits per bunch x meter per suit
    public static decimal TotalMeters(int bunches, int suitsPerBunch, decimal metersPerSuit)
        => bunches * suitsPerBunch * metersPerSuit;

    // Total suits = total meter / meter per suit (sirf poore suits)
    public static int TotalSuits(decimal totalMeters, decimal metersPerSuit)
        => metersPerSuit <= 0 ? 0 : (int)Math.Floor(totalMeters / metersPerSuit);

    // Bachay hue chhote tukre (meter)
    public static decimal LooseMeters(decimal totalMeters, decimal metersPerSuit)
        => metersPerSuit <= 0 ? 0 : totalMeters - TotalSuits(totalMeters, metersPerSuit) * metersPerSuit;

    // Stock ki value
    public static decimal StockValue(int suits, decimal costPricePerSuit)
        => suits * costPricePerSuit;
}