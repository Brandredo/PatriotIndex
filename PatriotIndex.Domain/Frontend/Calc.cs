namespace PatriotIndex.Domain.Frontend;

/// <summary>
/// Internal math helpers shared across all stat calculators.
/// All division is guarded against divide-by-zero, returning a configurable fallback.
/// </summary>
internal static class Calc
{
    /// <summary>Computes (numerator / denominator) × 100, or <paramref name="fallback"/> if denominator is zero.</summary>
    internal static double Pct(double numerator, double denominator, double fallback = 0.0)
        => denominator == 0.0 ? fallback : numerator / denominator * 100.0;

    /// <summary>Computes numerator / denominator, or <paramref name="fallback"/> if denominator is zero.</summary>
    internal static double Ratio(double numerator, double denominator, double fallback = 0.0)
        => denominator == 0.0 ? fallback : numerator / denominator;

    /// <summary>Rounds a double to <paramref name="decimals"/> decimal places.</summary>
    internal static double Round(double value, int decimals = 2)
        => Math.Round(value, decimals, MidpointRounding.AwayFromZero);
    
    /// <summary>Rounds a decimal to <paramref name="decimals"/> decimal places.</summary>
    internal static decimal Round(decimal value, int decimals = 2)
        => Math.Round(value, decimals, MidpointRounding.AwayFromZero);

    /// <summary>Clamps <paramref name="value"/> between <paramref name="min"/> and <paramref name="max"/>.</summary>
    internal static double Clamp(double value, double min, double max)
        => Math.Min(Math.Max(value, min), max);
}
