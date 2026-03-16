using System.Diagnostics;

namespace PatriotIndex.Domain.Telemetry;

internal static class PatriotIndexTelemetry
{
    public const string SourceName = "PatriotIndex.Domain";
    public static readonly ActivitySource Source = new(SourceName);
}
