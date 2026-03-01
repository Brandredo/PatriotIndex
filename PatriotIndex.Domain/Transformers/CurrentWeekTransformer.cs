using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Helpers;

namespace PatriotIndex.Domain.Transformers;

public class CurrentWeekTransformer(string json)
{
    private IEnumerable<Game>? _games;

    public IEnumerable<Game>? Transform()
    {
        using var trf = new JsonTraverser(json);
        var seasonType = trf.GetStringN("type");
        var seasonYear = trf.GetShortN("year");
        var seasonId =  trf.GetGuid("id");
        var weekSequence = trf.GetIntN("week.sequence");
        var weekTitle = trf.GetStringN("week.title");
        var weekId = trf.GetGuid("week.id");
        
        _games = trf.GetArrayList("games", g => new Game
        {
            Id = g.GetGuid("id") ?? throw new NullReferenceException("game id is null"),
            Status = g.GetStringN("status"),
            Scheduled = g.GetDateTime("scheduled"),
            Title = g.GetStringN("title"),
            SeasonId = seasonId,
            SeasonYear = seasonYear,
            SeasonType = seasonType,
            WeekSequence = weekSequence,
            WeekTitle = weekTitle,
            WeekId = weekId,
        });
        
        return _games;
    }
}