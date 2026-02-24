using PatriotIndex.Domain.Entities;
using PatriotIndex.Domain.Enums;

namespace PatriotIndex.Ingestion.Converters.Transformers;

public class PlayerTransformer : IJsonTransformer<Player>
{
    public Player Transform(JsonNavigator nav)
    {
        var player = new Player
        {
            Id = nav["id"].GetGuid(), 
            Name = nav["name"].GetString(),
            Jersey = nav.Optional("jersey")?.GetString() ?? null,
            LastName = nav["last_name"].GetString(),
            FirstName = nav["first_name"].GetString(),
            BirthDate = nav.Optional("birth_date")?.GetDateOnly() ?? null,
            Weight = nav.Optional("weight")?.GetDecimal() ?? null,
            Height = nav.Optional("height")?.GetInt16() ?? null,
            Position = Enum.Parse<PlayerPosition>(nav["position"].GetString()),
            College = nav.Optional("college")?.GetString() ?? null,
            RookieYear = nav.Optional("rookie_year")?.GetInt16() ?? null,
            Status = nav.Optional("status")?.GetString() ?? null,
            SrId = nav.Optional("sr_id")?.GetString() ?? null,
            Experience = nav.Optional("experience")?.GetInt16() ?? null,
            Salary = nav.Optional("salary")?.GetInt32() ?? null,
        };

        return player;
    }
}