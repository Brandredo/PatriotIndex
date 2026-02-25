using PatriotIndex.Domain.Entities;
using PatriotIndex.Ingestion.Converters;

namespace PatriotIndex.Ingestion;

public interface IPeriodTransformer
{
    Period Transform(Game game, JsonNavigator nav, DriveAggregator driveAggregator);
}