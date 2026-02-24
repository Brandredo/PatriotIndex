using PatriotIndex.Ingestion.Converters;
using PatriotIndex.Ingestion.Converters.Transformers;

namespace PatriotIndex.Ingestion;

// Factory creates a fresh instance per game — no shared state
public class DriveAggregatorFactory(DriveTransformer driveTransformer, DriveEventTransformer driveEventTransformer)
{
    public DriveAggregator Create() => new(driveTransformer, driveEventTransformer);
}